using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace BEAR.exceptionRates
{
    public partial class exceptionRates : System.Web.UI.Page
    {
        #region Global Variables

        protected String errorLogFileName = VariablesExceptionRates.ERROR_LOG_FILE_NAME;
        protected int widestMatter = VariablesExceptionRates.MINIMUM_WIDTH_MATTER;

        bool testEnvironment = ConfigurationManager.AppSettings["Environment"].ToString().ToLower().Equals("test");

        //protected bool[] rowChanged;

        protected UtilityExceptionRates utility;
        
        protected int columnBillingAttorneyName = 0;
        protected int columnClinetName = 1;
        protected int columnMatterDescription = 2;
        protected int columnRateType = 3;
        protected int columnEffectiveDate = 4;
        protected int columnDeviationPercent = 5;
        protected int columnMaximum = 6;
        protected int columnRateCode = 7;
        protected int columnSpecialRate = 8;
        protected int columnCurrentStandardRate = 9;
        protected int columnNewStandardRate = 10;
        protected int columnNewExceptionRate = 11;
        protected int columnReview1 = 12;
        protected int columnNotes = 13;
        protected int columnReview2 = 14;
        protected int columnReview3 = 15;
        protected int columnBillingAttorneyTkid = 16;
        protected int columnClientNumber = 17;
        protected int columnMatterNumber = 18;

        protected int calendarYear = 0;

        protected bool firstPassThroughResultSet = true;

        protected String clientPreviousRowValue = "";
        protected String matterPreviousRowValue = "";

        protected String currentTestString = "";
        protected String previousTestString = "";

        protected BearCode bearCode = new BearCode();
 

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            utility = new UtilityExceptionRates();

            if (testEnvironment)
            {
                Response.Redirect("http://bosweb3/bear/exceptionRates/" + VariablesExceptionRates.DATA_PAGE
                    + "?billtk=" + Request["billtk"].ToString()
                    + "&billspec=" + Request["billspec"].ToString()
                    + "&client=" + Request["client"].ToString()
                    + "&billtkofc=" + Request["billtkofc"].ToString()
                    + "&tcb=" + Request["tcb"].ToString()
                    + "&cmb=" + Request["cmb"].ToString()
                    + "&year=" + Request["year"].ToString()
                    + "&attorneyReview=" + Request["attorneyReview"].ToString()
                    + "&billingReview=" + Request["billingReview"].ToString()
                    + "&finalized=" + Request["finalized"].ToString()
                    );
            }
            


            Page.MaintainScrollPositionOnPostBack = true;
            int totalRows = 0;
            try
            {
                totalRows = dataGridView.Rows.Count;
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "BEAR.exceptionRates.Page_Load()", "int totalRows = dataGridView.Rows.Count");
            }

            utility.SetRowChanged(totalRows);
            hasPagerRow.Value = "false";
            calendarYear = int.Parse(Request["year"].ToString());

            //Set Tool Tips
            buttonSave.Attributes.Add("onmouseover", "Tip('" + VariablesExceptionRates.TOOLTIP_SAVE_BUTTON + "')");
            buttonSave.Attributes.Add("onmouseout", "UnTip()");
            buttonPrint.Attributes.Add("onmouseover", "Tip('" + VariablesExceptionRates.TOOLTIP_PRINT_BUTTON + "')");
            buttonPrint.Attributes.Add("onmouseout", "UnTip()");
            buttonChangeParameters.Attributes.Add("onmouseover", "Tip('" + VariablesExceptionRates.TOOLTIP_RERUN_BUTTON + "')");
            buttonChangeParameters.Attributes.Add("onmouseout", "UnTip()");
            exitButton.Attributes.Add("onmouseover", "Tip('" + VariablesExceptionRates.TOOLTIP_EXIT_BUTTON + "')");
            exitButton.Attributes.Add("onmouseout", "UnTip()");

            if (!Page.IsPostBack)
            {
                LogUserAccess();
            }
        }


        protected void SqlDataSource_Elite_uspBMcExceptionList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 0;
        }


        /// <summary>
        /// Called before the page is rendered.  This is used for the save functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {

            if (Page.IsPostBack)
            {
                String sessionResetCookie = "false";
                if (Session["resetCookie"] != null)
                {
                    sessionResetCookie = Session["resetCookie"].ToString();
                }

                if (sessionResetCookie.Equals("true"))
                {
                    Session["resetCookie"] = "false";
                }

                int totalRows = dataGridView.Rows.Count;
                for (int r = 0; r < totalRows; r++)
                {
                    if (utility.GetRowChanged()[r])
                    {
                        Session["rowEdited"] = r;

                        GridViewRow thisGridViewRow = dataGridView.Rows[r];

                        String clientID = "";
                        String billingAttorneyID = "";
                        String matterID = "";
                        String effectiveDate = "";
                        String rateType = "";
                        String exceptionRate = "";

                        int review1 = 0;
                        int review2 = 0;
                        int review3 = 0;

                        String comment = "";

                        try
                        {
                            billingAttorneyID = thisGridViewRow.Cells[columnBillingAttorneyTkid].Text == "&nbsp;" ? "" : thisGridViewRow.Cells[columnBillingAttorneyTkid].Text;
                            clientID = thisGridViewRow.Cells[columnClientNumber].Text == "&nbsp;" ? "" : thisGridViewRow.Cells[columnClientNumber].Text;
                            matterID = thisGridViewRow.Cells[columnMatterNumber].Text == "&nbsp;" ? "" : thisGridViewRow.Cells[columnMatterNumber].Text;

                            rateType = thisGridViewRow.Cells[columnRateType].Text == "&nbsp;" ? "" : thisGridViewRow.Cells[columnRateType].Text.Replace("'", "''");
                            effectiveDate = thisGridViewRow.Cells[columnEffectiveDate].Text == "&nbsp;" ? "" : thisGridViewRow.Cells[columnEffectiveDate].Text;

                            review1 = ((CheckBox)thisGridViewRow.FindControl("review1CB")).Checked == true ? 1 : 0;
                            review2 = ((CheckBox)thisGridViewRow.FindControl("review2CB")).Checked == true ? 1 : 0;
                            review3 = ((CheckBox)thisGridViewRow.FindControl("review3CB")).Checked == true ? 1 : 0;

                            comment = ((TextBox)thisGridViewRow.FindControl("notesTB")).Text.Replace("'", "''");
                            exceptionRate = ((TextBox)thisGridViewRow.FindControl("SRTB")).Text;

                            currentTestString = billingAttorneyID + clientID + matterID + rateType + effectiveDate + calendarYear;

                        }
                        catch (FormatException)
                        {
                            //this is to catch when set to a nonNumber
                            //non numbers are converted to zero
                        }
                        catch (ArgumentOutOfRangeException ae)
                        {
                            //when using paging
                            Logger.QuickLog(errorLogFileName, ae.Message, "Page_PreRender()", "");
                        }

                        if (currentTestString.Equals(previousTestString))
                        {
                        }
                        else
                        {
                            SqlConnection con = null;
                            /**
                             * Archive
                             * " INSERT INTO [BMcExceptionRatesArchiveChanges] "
                             * + " SELECT * "
                             * + " FROM BMcExceptionRates "
                             * + " WHERE [billingAtty] = '" + billingAttorneyID
                             * + "' AND [client] = '" + clientID
                             * + "' AND [matter] = '" + matterID
                             * + "' AND [rateType] = '" + rateType
                             * + "' AND [calendarYear] = " + calendarYear;
                             */
                            String sqlArchive = " INSERT INTO [BMcExceptionRatesArchiveChanges] "
                                              + " SELECT * "
                                              + " FROM BMcExceptionRates "
                                              + " WHERE [billingAtty] = '" + billingAttorneyID
                                              + "' AND [client] = '" + clientID
                                              + "' AND [matter] = '" + matterID
                                              + "' AND [rateType] = '" + rateType
                                              + "' AND [calendarYear] = " + calendarYear;

                            /**
                             * Delete
                             * "DELETE FROM [BMcExceptionRates] " +
                             * " WHERE [billingAtty] = '" + billingAttorneyID +
                             * "' AND [client] = '" + clientID +
                             * "' AND [matter] = '" + matterID +
                             * "' AND [rateType] = '" + rateType +
                             * "' AND [calendarYear] = " + calendarYear;
                             */
                            String sqlDelete = "DELETE FROM [BMcExceptionRates] " +
                                          " WHERE [billingAtty] = '" + billingAttorneyID +
                                           "' AND [client] = '" + clientID +
                                           "' AND [matter] = '" + matterID +
                                           "' AND [rateType] = '" + rateType +
                                           "' AND [calendarYear] = " + calendarYear;

                            /**
                             * Insert
                             * "INSERT INTO [BMcExceptionRates] " +
                                * " ([billingAtty], " +
                                *   " [client], " +
                                * " [matter], " +
                                * " [rateType], " +
                                * " [calendarYear], " +
                                * " [effectiveDate], " +
                                * " [exceptionRate], " +
                                * " [notes], " +
                                * " [updatedBy], " +
                                * " [updateTime], " +
                                * " [review1], " +
                                * " [review2], " +
                                * " [review3] " +
                                * " ) " +
                                * " VALUES " +
                                * "('"
                                * + billingAttorneyID + "'," +
                                * "'" + clientID + "'," +
                                * "'" + matterID + "'," +
                                * "'" + rateType + "'," +
                                * calendarYear + "," +
                                * "'" + effectiveDate + "'," +
                                * "'" + exceptionRate + "'," +
                                * "'" + comment + "'," +
                                * "'" + Page.User.Identity.Name.ToString().Substring(8) + "'," +
                                * "'" + DateTime.Now + "'," +
                                * review1 + "," +
                                * review2 + "," +
                                * review3 + ") ";
                             */
                            String sqlInsert = "INSERT INTO [BMcExceptionRates] " +
                                                " ([billingAtty], " +
                                                  " [client], " +
                                                  " [matter], " +
                                                  " [rateType], " +
                                                  " [calendarYear], " +
                                                  " [effectiveDate], " +
                                                  " [exceptionRate], " +
                                                  " [notes], " +
                                                  " [updatedBy], " +
                                                  " [updateTime], " +
                                                  " [review1], " +
                                                  " [review2], " +
                                                  " [review3] " +
                                                  " ) " +
                                                " VALUES " +
                                                "('"
                                                    + billingAttorneyID + "'," +
                                                "'" + clientID + "'," +
                                                "'" + matterID + "'," +
                                                "'" + rateType + "'," +
                                                      calendarYear + "," +
                                                "'" + effectiveDate + "'," +
                                                "'" + exceptionRate + "'," +
                                                "'" + comment + "'," +
                                                "'" + Page.User.Identity.Name.ToString().Substring(8) + "'," +
                                                "'" + DateTime.Now + "'," +
                                                      review1 + "," +
                                                      review2 + "," +
                                                      review3 + ") ";

                            
                            try
                            {
                                con = new SqlConnection(
                                        ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                                con.Open();
                                SqlCommand command = con.CreateCommand();
                                command.CommandType = CommandType.Text;
                                command.CommandText = sqlArchive;
                                command.ExecuteNonQuery();

                                try
                                {
                                    command.CommandText = sqlDelete;
                                    command.ExecuteNonQuery();

                                    try
                                    {
                                        command.CommandText = sqlInsert;
                                        command.ExecuteNonQuery();
                                    }
                                    catch (SqlException sqleInsert)
                                    {
                                        Logger.QuickLog(errorLogFileName, sqleInsert.Message, "Page_PreRender()", sqlInsert);
                                    }

                                }
                                catch (SqlException sqleDelete)
                                {
                                    Logger.QuickLog(errorLogFileName, sqleDelete.Message, "Page_PreRender()", sqlDelete);
                                }

                            }
                            catch (SqlException sqleArchive)
                            {
                                Logger.QuickLog(errorLogFileName, sqleArchive.Message, "Page_PreRender()", sqlArchive);
                            }
                            finally
                            {
                                if (con != null)
                                {
                                    con.Close();
                                }
                            }

                            previousTestString = currentTestString;

                        } //end else on previous = current

                    } //end "if (rowChanged[r])"

                } //end "for (int r = 0; r < totalRows; r++)"
                dataGridView.DataBind();

            } //end if (Page.IsPostBack)

        } //end Page_PreRender()


        protected void buttonPrintClick(object sender, EventArgs e)
        {
            Page_PreRender(sender, e);
            String billtk = "";
            String client = "";

            if (Request["billtk"] == null || Request["billtk"].Equals(""))
            {
                billtk = "All";
            }
            else
            {
                billtk = Request["billtk"];
            }


            if (Request["client"] == null || Request["client"].Equals(""))
            {
                client = "All";
            }
            else
            {
                client = Request["client"];
            }

            String url = System.Configuration.ConfigurationSettings.AppSettings["ReportServer"]
                            + VariablesExceptionRates.REPORT_FOLDER
                            + "/"
                            + VariablesExceptionRates.REPORT_SERVICES_PRINT_NAME
                            + "&rc:Parameters=false&rs:Command=Render&rs:ClearSession=true"
                            + "&ApplicationServer="
                            + Environment.MachineName.ToString()
                            + "&billingtimekeeper="
                            + Request["billtk"]

                            + "&billingspecialist="
                            + Request["billspec"]

                            + "&client="
                            + Request["client"]

                            + "&billingtimekeeperoffice="
                            + Request["billtkofc"]

                            + "&tcb="
                            + Request["tcb"]

                            + "&cmb="
                            + Request["cmb"]

                            + "&year="
                            + Request["year"] ;

            if (Request["attorneyReview"] != null)
            {
                url = url
                    + "&attorneyReview="
                    + Request["attorneyReview"];
            }

            if (Request["billingReview"] != null)
            {
                url = url
                    + "&billingReview="
                    + Request["billingReview"];
            }

            if (Request["finalized"] != null)
            {
                url = url
                    + "&finalized="
                    + Request["finalized"];
            }

            Response.Redirect(url);


        }


        protected void buttonChangeParametersClick(object sender, EventArgs e)
        {
            Response.Redirect(VariablesExceptionRates.HOME_PAGE);
        }


        /// <summary>
        /// When the text is changed in the dataGrid, the row is marked as changed<br />
        /// this boolean is used in Page_PreRender to save changed rows<br />
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            utility.GetRowChanged()[row] = true;
        }


        protected void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            CheckBox thisCheckBox = (CheckBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            utility.GetRowChanged()[row] = true;
        }


        protected void GridViewDataDivRowBindEvent(object sender, GridViewRowEventArgs e)
        {
 
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {

                //HEADER ROW SPECIFIC
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    dataGridView.Columns[columnMatterDescription].ItemStyle.Width = widestMatter;
                }

                //DATAROW SPECIFIC
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if ( ((TextBox)e.Row.Cells[columnNotes].FindControl("notesTB")).Text.Length > 28 ) 
                    {
                        e.Row.Cells[columnNotes].Attributes.Add("onmouseover", "Tip('<b><u>Full Text:</u></b><br/>" + ((TextBox)e.Row.Cells[columnNotes].FindControl("notesTB")).Text + "', WIDTH, 200)");
                        e.Row.Cells[columnNotes].Attributes.Add("onmouseout", "UnTip()");
                    }

                    e.Row.Cells[columnMatterDescription].Attributes.Add("nowrap", "nowrap");

                    if (e.Row.Cells[columnDeviationPercent].Text.Equals("0.00")) e.Row.Cells[columnDeviationPercent].Text = "-";
                    if (e.Row.Cells[columnMaximum].Text.Equals("0.00")) e.Row.Cells[columnMaximum].Text = "-";
                    if (e.Row.Cells[columnSpecialRate].Text.Equals("-9999.99")) e.Row.Cells[columnSpecialRate].Text = "-";
                    if (e.Row.Cells[columnCurrentStandardRate].Text.Equals("0.00")) e.Row.Cells[columnCurrentStandardRate].Text = "-";
                    if (e.Row.Cells[columnNewStandardRate].Text.Equals("0.00")) e.Row.Cells[columnNewStandardRate].Text = "-";

                    if (e.Row.Cells[columnMatterDescription].Text.Equals("Client Level Exception"))
                    {
                        
                        String clientNumber = e.Row.Cells[columnClientNumber].Text;
                        String attorneyTkid = e.Row.Cells[columnBillingAttorneyTkid].Text;
                        String alertText = GetClientMattersByAttorney(clientNumber, attorneyTkid, e.Row.Cells[columnClinetName].Text, e.Row.Cells[columnBillingAttorneyName].Text);
                        e.Row.Cells[columnMatterDescription].Attributes.Add("Style", "cursor: hand");
                        e.Row.Cells[columnMatterDescription].Attributes.Add("onclick", "javascript:alert(\"" + alertText + "\")");

                    }

                    if (firstPassThroughResultSet)
                    {
                        Session["AttorneyName"] = e.Row.Cells[columnBillingAttorneyName].Text.ToString();
                        Session["ClientName"] = e.Row.Cells[columnClinetName].Text.ToString();
                        firstPassThroughResultSet = false;
                    }

                    if (e.Row.RowIndex != 0)
                    {
                        if (e.Row.Cells[columnClinetName].Text.Equals(clientPreviousRowValue) &&
                            e.Row.Cells[columnMatterDescription].Text.Equals(matterPreviousRowValue))
                        {
                        }
                        else
                        {
                            for (int r = 0; r < e.Row.Cells.Count; r++)
                            {
                                e.Row.Cells[r].Attributes.Add("style", "border-top-style: solid");
                                e.Row.Cells[r].Attributes.Add("style", "border-top-width: 2pt");
                            }
                        }
                    }

                    clientPreviousRowValue = e.Row.Cells[columnClinetName].Text;
                    matterPreviousRowValue = e.Row.Cells[columnMatterDescription].Text;


                }

                //FOR BOTH HEADER AND DATAROWS

                //find columns that were locked previously and keep them locked.  
                //find columns that were hidden previously and keep them hidden.
                try
                {
                    //if lockedColumnNumber is zero of greater, then we need to keep things locked.
                    if (int.Parse(lockedColumnNumber.Value.ToString()) > -1)
                    {
                        //only look at columns with a column number <= the locked column (greater columns are unlocked)
                        for (int i = 0; i <= int.Parse(lockedColumnNumber.Value.ToString()); i++)
                        {

                            if (Request["billtk"].ToString().Equals("All") &&
                                !Request["client"].ToString().Equals("All") && 
                                i==0)
                            {
                                /*
                                 * billtk is the first column
                                 * client is the second
                                 * if we want all billtk and not all clients, then the columns are
                                 * 0 = Locked; 1 = Hidden
                                 * if we are in this for loop, we know there are locked columns
                                 * if there are locked columns, then the first column must be locked, so lock it.
                                 */
                                e.Row.Cells[i].CssClass = "locked";
                            }
                            else if (i > int.Parse(hiddenColumnNumber.Value.ToString()))
                            {
                                //if we are on a column that is greater than the locked column, 
                                //then lock all the cells in that column
                                e.Row.Cells[i].CssClass = "locked";
                            }
                        }
                    }
                }
                catch
                {
                    //catch if lockedColumnNumber is not set
                }

                if (Request["billtk"] == null
                    || Request["billtk"].ToString().Equals("All")
                    || Request["billtk"].ToString().Equals(""))
                {
                    //do nothing if billtk is not set to a Billing Attorney TKID    
                }
                else
                {
                    //hide the Billing Attorney Column
                    e.Row.Cells[columnBillingAttorneyName].Attributes.Add("class", "hidden");
                }

                
                if (Request["client"] == null
                    || Request["client"].ToString().Equals("All")
                    || Request["client"].ToString().Equals(""))
                {
                    //do nothing if client is not set to a Client #
                }
                else
                {
                    //hide the Client column
                    e.Row.Cells[columnClinetName].Attributes.Add("class", "hidden");
                }

                //setting the width of Matter Description Column
                if (e.Row.Cells[columnMatterDescription] != null)
                {
                    String matterDescription = e.Row.Cells[columnMatterDescription].Text.ToString();
                    int matterDescriptionLength = matterDescription.Length * 6;

                    if (matterDescriptionLength > widestMatter)
                    {
                        widestMatter = matterDescriptionLength;
                        dataGridView.Columns[columnMatterDescription].ItemStyle.Width = Unit.Pixel(widestMatter);
                        dataGridView.Columns[columnMatterDescription].ItemStyle.Wrap = false;
                    }

                }

            }
            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                bearCode.GridViewCustomizePagerRow(dataGridView, e.Row);
            }

        }

        
        protected void GridViewDataBoundEvent(object sender, EventArgs e)
        {
            data.Attributes.Add("Style", VariablesExceptionRates.DATA_DIV_CUSTOM_STYLE);
            dataGridView.PageSize = VariablesExceptionRates.DATA_PAGE_SIZE;
            if (dataGridView.PageCount > 1)
            {
                hasPagerRow.Value = "true";
            }

        }

        
        #region Custom Get Methods
        public String getBillingAttorney()
        {
            String billtk = Request["billtk"].ToString() == "" ? "All" : Request["billtk"].ToString();

            if (billtk.Equals("All"))
            { }
            else
            {
                if (Session["AttorneyName"] != null)
                {
                    billtk = Session["AttorneyName"].ToString();
                }
            }
            return billtk;
        }

        public String getBillingSpecialist()
        {
            String billspec = Request["billspec"].ToString() == "" ? "All" : Request["billspec"].ToString();
            return billspec;
        }

        public String getClientNumber()
        {
            String client = Request["client"].ToString() == "" ? "All" : Request["client"].ToString();

            if (client.Equals("All"))
            { }
            else
            {
                if (Session["ClientName"] != null)
                {
                    client = Session["ClientName"].ToString();
                }
            }
            return client;
        }

        public String getExceptions()
        {
            String tcb = Request["tcb"].ToString();
            if (tcb.Equals("T"))
                tcb = "Timekeeper";
            else if (tcb.Equals("C"))
                tcb = "Costcode";
            else
                tcb = "Both";

            return tcb;
        }

        public String getRates()
        {
            String cmb = Request["cmb"].ToString();
            if (cmb.Equals("C"))
                cmb = "Client";
            else if (cmb.Equals("M"))
                cmb = "Matter";
            else
                cmb = "Both";

            return cmb;
        }

        public int getYear()
        {
            return calendarYear;
        }

        public int getCurrentPage()
        {
            return (int)dataGridView.PageIndex + 1;
        }

        public int getPageCount()
        {
            return (int)dataGridView.PageCount;
        }
        #endregion


        /// <summary>
        /// calls stored procedure: uspBMcBEARExceptionRates_AttorneyMattersByClient
        /// </summary>
        /// <param name="clientNumber"></param>
        /// <param name="attorneyTkid"></param>
        /// <param name="clientName"></param>
        /// <param name="attorneyName"></param>
        /// <returns></returns>
        protected String GetClientMattersByAttorney(String clientNumber, String attorneyTkid, String clientName, String attorneyName)
        {
            SqlConnection con = null;
            String ClientMattersByAttorney = "Matters for Timekeeper: " + attorneyName + " / Client: " + clientName + ": \\n";
            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "uspBMcBEARExceptionRates_AttorneyMattersByClient";
                command.Parameters.AddWithValue("@billingtimekeeper", attorneyTkid);
                command.Parameters.AddWithValue("@client", clientNumber);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ClientMattersByAttorney = ClientMattersByAttorney + "\\n" + reader["matter"].ToString().Replace("'", "\\'");
                }
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetClientMattersByAttorney()", "Client = " + clientNumber + " / TKID = " + attorneyTkid);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return ClientMattersByAttorney;

        }


        /// <summary>
        /// Logs User Access to the BMcExceptionRatesLog table using values from the URL
        /// </summary>
        protected void LogUserAccess()
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO [BMcExceptionRatesLog] "
                                        + " ([networkId] "
                                        + " ,[logIntime] "
                                        + " ,[billingAtty] "
                                        + " ,[client] "
                                        + " ,[office] "
                                        + " ,[billSpecialist] "
                                        + " ,[exceptions] "
                                        + " ,[rates] "
                                        + " ,[year] "
                                        + " ,[billingReview] "
                                        + " ,[attorneyReview] "
                                        + " ,[finalized] ) "
                                  + " VALUES "
                                        + " ( '" + Page.User.Identity.Name.ToString().Substring(8) + "', "
                                        + "'" + DateTime.Now + "', "
                                        + "'" + Request["billtk"].ToString() + "', "
                                        + "'" + Request["client"].ToString() + "', "
                                        + "'" + Request["billtkofc"].ToString() + "', "
                                        + "'" + Request["billspec"].ToString() + "', "
                                        + "'" + Request["tcb"].ToString() + "', "
                                        + "'" + Request["cmb"].ToString() + "', "
                                              + Request["year"].ToString() + ", "
                                        + "'" + Request["attorneyReview"].ToString() + "', "
                                        + "'" + Request["billingReview"].ToString() + "', "
                                        + "'" + Request["finalized"].ToString() + "' "
                                        + " )";
                command.ExecuteNonQuery();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "LogUserAccess()", "");
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
}
