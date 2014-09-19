using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

namespace BEAR.lockbox
{
    public partial class processImportedFile : System.Web.UI.Page
    {
        protected String GroupId = "";
        String errorLogFileName = VariablesLockbox.ERROR_LOG_FILE_NAME;

        UtilityLockboxImport utility;

        //int columnId = 0;
        //int columnBatchId = 1;
        //int columnCheckSeq = 2;
        //int columnCheckAmt = 3;
        //int columnDocNo = 4;
        int columnInvoiceNo = 5;
        //int columnInvoiceAmount = 6;
        //int columnInvoiceDesc = 7;
        //int columnNotes = 8;

        String currentDocNum = "";
        String previousDocNum = "";
        
        String suser = "";

        bool import = false;
        int NumberOfLinesProcessed = 0;
        double BatchTotal = 0D;
        int BatchCount = 0;
        int TotalInvoiceCount = 0;
        int TotalCheckCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             * after postback, scroll position is saved.  Also used to reduce page flashing on postback
             */
            Page.MaintainScrollPositionOnPostBack = true;

            this.suser = Page.User.Identity.Name.ToString().Substring(8);

            utility = new UtilityLockboxImport();

            if (!Page.IsPostBack)
            {
                if (Session["process"] != null)
                {
                    if (Session["process"].ToString().ToLower().Equals("import"))
                    {
                        this.import = true;
                    }
                }

                if (this.import)
                {
                    ImportFile();
                }

                GridViewImportedDataBind();
            
            }

            PopulateLabelProcessingOutput();
            utility.SetRowChanged(GridViewImportedData.Rows.Count);
        }

        /*
         * Parse through bank file, save data to database, bind gridview
         */
        protected void ImportFile()
        {

            int iCheckCount = 0;
            int iTotalCheckCount = 0;
            int iBatchCount = 0;
            int iInvoiceCount = 0;
            int iTotalInvoiceCount = 0;
            double cBatchAmount = 0D;
            double cTotalAmount = 0D;
            double cCheckAmount = 0D;
            double cTotalCheckAmt = 0D;

            int firstCharacter = 0;
            String _01RoutingNumber = ""; 
            String _02SystemCreditDate = ""; 
            String _03SystemCreditTime = ""; 
            String _04LockboxNo = ""; 
            String _05BatchID = ""; 
            String _06CustAcctNo = ""; 
            String _07CheckSeq = ""; 
            double _08CheckAmt = 0D; 
            String _09MICR_RT_ID = ""; 
            String _10MICR_ACCT_ID = ""; 
            String _11DocNo = ""; 
            String _12InvoiceNo = ""; 
            double _13InvoiceAmt = 0D; 
            String _14InvoiceDesc = ""; 
            String _15LedgerCode = "";

            SqlConnection con = null;

            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["lockboxConnectionString"].ConnectionString);
                con.Open();


                if (Session["uploadedFilePathAndName"] != null)
                {
                    LabelProcessingMessage.Text = "";
                    LabelProcessingOutput.Text = "";
                    StreamReader file = new StreamReader(Session["uploadedFilePathAndName"].ToString());
                    String line;
                    int counter = 1;

                    while ((line = file.ReadLine()) != null)
                    {
                        firstCharacter = Int32.Parse(line.Trim().Substring(0, 1));

                        if (counter == 1)
                        {
                            this.GroupId = line.Trim().Substring(30, 2) + line.Trim().Substring(32, 2) + line.Trim().Substring(28, 2); //mmddyy
                            SetGroupId();


                        }

                        switch (firstCharacter)
                        {
                            case 1:
                                _01RoutingNumber = line.Trim().Substring(16, 10);
                                _02SystemCreditDate = line.Trim().Substring(30, 2) + "/" + line.Trim().Substring(32, 2) + "/" + line.Trim().Substring(26, 4);
                                _03SystemCreditTime = line.Trim().Substring(34, 2) + ":" + line.Trim().Substring(36, 2);
                                _04LockboxNo = line.Trim().Substring(39, 6);
                                _08CheckAmt = 0D;
                                iTotalCheckCount = 0;
                                iTotalInvoiceCount = 0;
                                iBatchCount = 0;
                                cTotalAmount = 0;
                                break;
                            case 5:
                                _05BatchID = line.Trim().Substring(1, 3);
                                if (!line.Trim().Substring(4, 6).Equals(_04LockboxNo))
                                {
                                    LabelProcessingMessage.Text += "File is Corrupt. line: " + counter + " / ! case 5: LockboxNo<br />";
                                }
                                if (!_02SystemCreditDate.Equals(line.Trim().Substring(14, 2) + "/" + line.Trim().Substring(16, 2) + "/" + line.Trim().Substring(10, 4)))
                                {
                                    LabelProcessingMessage.Text += "File is Corrupt. line: " + counter + " / case 5: SystemCreditDate<br />";
                                }
                                _06CustAcctNo = line.Trim().Substring(18);
                                iBatchCount++;
                                iCheckCount = 0;
                                iInvoiceCount = 0;

                                break;
                            case 6:
                                cCheckAmount = 0;
                                if (!_05BatchID.Equals(line.Trim().Substring(1, 3)))
                                {
                                    LabelProcessingMessage.Text += "File is Corrupt. line: " + counter + " / case 6: BatchID<br />";
                                }
                                _07CheckSeq = line.Trim().Substring(4, 3);
                                _08CheckAmt = Double.Parse(line.Trim().Substring(7, 10)) / 100;
                                _09MICR_RT_ID = line.Trim().Substring(17, 10);
                                _10MICR_ACCT_ID = line.Trim().Substring(27, 14);
                                _11DocNo = line.Trim().Substring(41, 10).TrimStart('0');
                                _14InvoiceDesc = line.Trim().Substring(67, 26).Trim().Replace("'", "");
                                _15LedgerCode = line.Trim().Substring(93, 7);
                                iCheckCount++;
                                iTotalCheckCount++;
                                cTotalCheckAmt += _08CheckAmt;
                                break;
                            case 4:
                                _12InvoiceNo = line.Trim().Substring(7, 7);
                                _13InvoiceAmt = Double.Parse(line.Trim().Substring(14, 10))/100;
                                if (!_05BatchID.Equals(line.Trim().Substring(1, 3)))
                                {
                                    LabelProcessingMessage.Text += "File is corrupt.  The Batch ID for Invoice " + _12InvoiceNo + " does not match the batch it should be a part of.<br />";
                                }
                                if (!_07CheckSeq.Equals(line.Trim().Substring(4, 3)))
                                {
                                    LabelProcessingMessage.Text += "File is corrupt.  The Check Number for Batch " + _05BatchID + " and Invoice " + _12InvoiceNo + " does not match the check group it is part of.";
                                }
                                iInvoiceCount++;
                                iTotalInvoiceCount++;
                                cTotalAmount += _13InvoiceAmt;
                                cBatchAmount += _13InvoiceAmt;
                                cCheckAmount += _13InvoiceAmt;


                                //insert into ImportLockbox
                                String insertSql = "INSERT INTO ImportLockbox (groupID, RoutingNumber, systemCreditDate, systemCreditTime, lockboxNo, batchID, "
                                                                            + " custAcctNo, checkSeq, checkAmt, micr_rt_id, micr_acct_id, docno, invoiceno, "
                                                                            + " invoiceamount, invoicedesc, ledgercode, Notes) "
                                                                            + " VALUES "
                                                                            + "('"
                                                                            + GroupId + "' ,'" + _01RoutingNumber + "' ,'" + _02SystemCreditDate + "' , '" + _03SystemCreditTime + "' ,'"
                                                                            + _04LockboxNo + "' ,'" + _05BatchID + "' ,'" + _06CustAcctNo + "' , '" + _07CheckSeq + "' ,"
                                                                            + _08CheckAmt + " ,'" + _09MICR_RT_ID + "' ,'" + _10MICR_ACCT_ID + "' ,'" + _11DocNo + "' ,'"
                                                                            + _12InvoiceNo + "' ," + _13InvoiceAmt + " ,'" + _14InvoiceDesc + "' ,'" + _15LedgerCode + "','');"
                                                + "INSERT INTO LogImportChanges (ImportID, UserID, TimeStamp, CheckAmt, DocNo, InvoiceNo, "
                                                                            + " InvoiceAmount, InvoiceDesc, Notes ) "
                                                                            + " SELECT ID as ImportID, '" + this.suser + "' AS UserID, '" + DateTime.Now + "' AS TimeStamp, " 
                                                                                   + " CheckAmt, DocNo, InvoiceNo, "
                                                                                   + " InvoiceAmount, InvoiceDesc, Notes "
                                                                            + " FROM ImportLockbox "
                                                                            + " WHERE groupID = '" + GroupId + "' " 
                                                                                + " AND RoutingNumber = '" + _01RoutingNumber + "' "
                                                                                + " AND systemCreditDate = '" + _02SystemCreditDate + "' "
                                                                                + " AND systemCreditTime =  '" + _03SystemCreditTime + "' "
                                                                                + " AND lockboxNo = '" + _04LockboxNo + "' "
                                                                                + " AND batchID = '" + _05BatchID + "' "
                                                                                + " AND custAcctNo = '" + _06CustAcctNo + "' "
                                                                                + " AND checkSeq = '" + _07CheckSeq + "' "
                                                                                + " AND checkAmt = " + _08CheckAmt
                                                                                + " AND micr_rt_id = '" + _09MICR_RT_ID + "' "
                                                                                + " AND micr_acct_id = '" + _10MICR_ACCT_ID + "' "
                                                                                + " AND DocNo = '" + _11DocNo + "' "
                                                                                + " AND InvoiceNo = '" + _12InvoiceNo + "' "
                                                                                + " AND InvoiceAmount = " + _13InvoiceAmt
                                                                                + " AND InvoiceDesc = '" + _14InvoiceDesc + "' "
                                                                                + " AND LedgerCode = '" + _15LedgerCode + "' "
                                                                                + " AND Notes = '';";


                                SqlCommand command = con.CreateCommand();
                                command.CommandType = CommandType.Text;
                                command.CommandText = insertSql;
                                command.ExecuteNonQuery();

                                break;
                            case 7:
                                if (!_05BatchID.Equals(line.Trim().Substring(1, 3)))
                                {
                                    LabelProcessingMessage.Text += "File is corrupt. / case 7: BatchId";
                                }
                                if (iCheckCount != Int32.Parse(line.Trim().Substring(4, 3)))
                                {
                                    LabelProcessingMessage.Text += "File is corrupt. / case 7: Check Count";
                                }
                                if (iInvoiceCount != Int32.Parse(line.Trim().Substring(7, 4)))
                                {
                                    LabelProcessingMessage.Text += "File is corrupt. / case 7: Invoice Count";
                                }
                                break;
                            case 8:
                                if (iBatchCount != Int32.Parse(line.Trim().Substring(1, 3)))
                                {
                                    LabelProcessingMessage.Text += "File is corrupt. / case 8: Batch Count.";
                                }
                                if (iTotalCheckCount != Int32.Parse(line.Trim().Substring(4, 4)))
                                {
                                    LabelProcessingMessage.Text += "File is corrupt / case 8: Total Check Count.";
                                }
                                break;
                            case 9:
                                break;
                            default:
                                break;
                        } //end switch

                        counter++;
                    }
                    file.Close();

                    this.NumberOfLinesProcessed = counter;
                    this.BatchCount = iBatchCount;
                    this.TotalInvoiceCount = iTotalInvoiceCount;

                    SqlCommand command2 = con.CreateCommand();
                    command2.CommandType = CommandType.StoredProcedure;
                    command2.CommandText = "InsertLogRecord";
                    command2.Parameters.AddWithValue("@GroupID", this.GroupId);
                    command2.Parameters.AddWithValue("@ProcessDate", DateTime.Now);
                    command2.Parameters.AddWithValue("@CreditDate", _02SystemCreditDate);
                    command2.Parameters.AddWithValue("@UserId", this.suser);
                    command2.Parameters.AddWithValue("@Process", "Import");
                    command2.Parameters.AddWithValue("@RecordCount", counter);
                    command2.Parameters.AddWithValue("@Total", cTotalAmount);
                    command2.Parameters.AddWithValue("@BatchCount", iBatchCount);
                    command2.Parameters.AddWithValue("@CheckCount", iTotalCheckCount);
                    command2.Parameters.AddWithValue("@InvoiceCount", iTotalInvoiceCount);
                    command2.Parameters.AddWithValue("@FileName", Session["uploadedFilePathAndName"].ToString().Substring(Session["uploadedFilePathAndName"].ToString().LastIndexOf("\\")+1));
                    command2.Parameters.AddWithValue("@TotalCheckAmt", cTotalCheckAmt);
                        
                    command2.ExecuteNonQuery();

                }
            }
            catch (SqlException sqle)
            {
                LabelProcessingMessage.Text += sqle.Message;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        /*
         * Check to see if the groupid is in the database if it is, add an A to the end of the GroupID
         */
        protected void SetGroupId()
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["lockboxConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT DISTINCT groupID FROM LogLockbox WHERE groupID LIKE '" + this.GroupId + "%' ORDER BY groupID";
                SqlDataReader reader = command.ExecuteReader();
                String groupIdDatabase = this.GroupId;

                bool groupIDFound = false;

                while (reader.Read())
                {
                    groupIdDatabase = reader["groupID"].ToString();
                    groupIDFound = true;
                }

                if (groupIDFound)
                {
                    this.GroupId = groupIdDatabase + "A";
                }

                Session["GroupId"] = this.GroupId;

            }
            catch (SqlException sqle)
            {
                LabelProcessingMessage.Text += sqle.Message;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            
        }


        /*
         * Bind Gridview to Database table with values from import file
         */
        protected void GridViewImportedDataBind()
        {
            SqlConnection con = null;
            //String groupId = "";
            if (Session["GroupId"] != null)
            {
                this.GroupId = Session["GroupId"].ToString();
            }
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["lockboxConnectionString"].ConnectionString);
                SqlCommand command = new SqlCommand("SelectImportedRecords", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("GroupID", this.GroupId);
                SqlDataAdapter sa = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sa.Fill(dt);
                GridViewImportedData.DataSource = dt;
                GridViewImportedData.DataBind();
            }
            catch (SqlException sqle)
            {
                LabelProcessingMessage.Text += sqle.Message;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        
        /// <summary>
        /// Called before the page is rendered.  This is used for the save functionality. <br />
        /// Looping through all the rows that are changed saving Notes and Forecast for each changed row<br />
        /// after all changed rows have been saved to the database, the grids are rebound<br />
        /// Rebinding GridView3 (Detail) is done to allow it to continue to show after a postback<br />
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                int totalRows = GridViewImportedData.Rows.Count;
                for (int r = 0; r < totalRows; r++)
                {
                    if (utility.GetRowChanged()[r])
                    {
                        Session["rowEdited"] = r;

                        GridViewRow thisGridViewRow = GridViewImportedData.Rows[r];
                        String ID = thisGridViewRow.Cells[0].Text;
                        decimal CheckAmt = decimal.Parse(((TextBox)thisGridViewRow.FindControl("TextBoxCheckAmt")).Text, System.Globalization.NumberStyles.Currency);
                        String DocNo = ((TextBox)thisGridViewRow.FindControl("TextBoxDocNo")).Text;
                        String InvoiceNo = ((TextBox)thisGridViewRow.FindControl("TextBoxInvoiceNo")).Text;
                        decimal InvoiceAmt = decimal.Parse(((TextBox)thisGridViewRow.FindControl("TextBoxInvoiceAmt")).Text, System.Globalization.NumberStyles.Currency);
                        String InvoiceDesc = ((TextBox)thisGridViewRow.FindControl("TextBoxInvoiceDesc")).Text;
                        String Notes = ((TextBox)thisGridViewRow.FindControl("TextBoxNotes")).Text;

                        String sql = " UPDATE ImportLockbox " +
                                     " SET CheckAmt = " + CheckAmt +
                                       " , DocNo = '" + DocNo + "' " +
                                       " , InvoiceNo = '" + InvoiceNo + "' " +
                                       " , InvoiceAmount = " + InvoiceAmt +
                                       " , InvoiceDesc = '" + InvoiceDesc + "' " +
                                       " , Notes = '" + Notes + "' " +
                                     " WHERE ID = " + ID + ";" + 

                                     " INSERT INTO LogImportChanges " +
                                      " (ImportID, UserID, TimeStamp, CheckAmt, DocNo, InvoiceNo, InvoiceAmount, InvoiceDesc, Notes) " +
                                      " VALUES " +
                                      " (" + ID + ", '" + this.suser + "', '" + DateTime.Now + "', " 
                                           + CheckAmt + ", '" + DocNo + "', '" + InvoiceNo + "', " 
                                           + InvoiceAmt + ", '" + InvoiceDesc + "', '" + Notes + "');";

                        SqlConnection con = null;
                        try
                        {
                            con = new SqlConnection(
                                    ConfigurationManager.ConnectionStrings["lockboxConnectionString"].ConnectionString);
                            con.Open();
                            SqlCommand command = con.CreateCommand();
                            command.CommandText = sql;
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                        }
                        catch (SqlException sqle)
                        {
                            Logger.QuickLog(errorLogFileName, sqle.Message, "Page_PreRender()", sql);
                        }
                        finally
                        {
                            if (con != null)
                            {
                                con.Close();
                            }
                        }

                    }
                }

                GridViewImportedDataBind();
                PopulateLabelProcessingOutput();
            }
        }


        protected void PopulateLabelProcessingOutput()
        {
            String ProcessingOutput =
              "Group ID: " + this.GroupId + "<br />";
            if (import)
            {
                ProcessingOutput = ProcessingOutput 
                    + "Number of Lines Processed: " + this.NumberOfLinesProcessed + "<br />";
            }
            ProcessingOutput = ProcessingOutput
                + "Batch Total: " + String.Format("{0:$##,###.##}", this.BatchTotal.ToString()) + "<br />";
            if (import)
            {
                ProcessingOutput = ProcessingOutput
                    + "Batch Count: " + this.BatchCount + "<br />"
                    + "Invoice Count: " + this.TotalInvoiceCount + "<br />";
            }
            ProcessingOutput = ProcessingOutput 
                + "Check Count: " + this.TotalCheckCount + "<br />";

            LabelProcessingOutput.Text = ProcessingOutput;

        }

        
        /*
         * When the text is changed in the dataGrid, the row is marked as changed
         * this boolean is used in Page_PreRender to save changed rows
         */
        protected void TextBoxTextChanged(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            utility.GetRowChanged()[row] = true;
        }


        protected void ButtonHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }


        protected void GridViewImportedData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                this.BatchTotal = this.BatchTotal + double.Parse(((TextBox)e.Row.FindControl("TextBoxInvoiceAmt")).Text.Replace("$", "").Replace(",", ""));
                this.currentDocNum = ((TextBox)e.Row.FindControl("TextBoxDocNo")).Text;
                if (!currentDocNum.Equals(previousDocNum))
                {
                    this.TotalCheckCount++;
                }

                if ( ((TextBox)e.Row.FindControl("TextBoxInvoiceNo")).Text.Equals("9999999") )
                {
                    //add special style to 9999999 invoice number
                    e.Row.Cells[columnInvoiceNo].Attributes.Add("style", "background-color: Red");
                }


                
                
                
                previousDocNum = currentDocNum;
            }
        }

    }
}
