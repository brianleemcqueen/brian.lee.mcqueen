using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BEAR.billTracker
{
    public partial class billTracker : System.Web.UI.Page
    {
        #region Global Variables

        protected UtilityBillTracker utility;
        protected BearCode bearCode;

        protected String errorLogFileName = VariablesBillTracker.ERROR_LOG_FILE_NAME;

        protected int columnDraftSent = VariablesBillTracker.COLUMN_DRAFT_SENT;
        protected int columnReadyToBill = VariablesBillTracker.COLUMN_READY_TO_BILL;

        //column place holders are always in the same position, regardless of what data is in the column
        protected int columnPlaceHolder1 = VariablesBillTracker.COLUMN_PLACEHOLDER_1;
        protected int columnPlaceHolder2 = VariablesBillTracker.COLUMN_PLACEHOLDER_2;
        protected int columnPlaceHolder3 = VariablesBillTracker.COLUMN_PLACEHOLDER_3;
        protected int columnPlaceHolder4 = VariablesBillTracker.COLUMN_PLACEHOLDER_4;
        protected int columnPlaceHolder5 = VariablesBillTracker.COLUMN_PLACEHOLDER_5;
        protected int columnPlaceHolder6 = VariablesBillTracker.COLUMN_PLACEHOLDER_6;
        protected int columnPlaceHolder7 = VariablesBillTracker.COLUMN_PLACEHOLDER_7;
        protected int columnPlaceHolder8 = VariablesBillTracker.COLUMN_PLACEHOLDER_8;
        protected int columnPlaceHolder9 = VariablesBillTracker.COLUMN_PLACEHOLDER_9;
        protected int columnPlaceHolder10 = VariablesBillTracker.COLUMN_PLACEHOLDER_10;
        protected int columnPlaceHolder11 = VariablesBillTracker.COLUMN_PLACEHOLDER_11;
        protected int columnPlaceHolder12 = VariablesBillTracker.COLUMN_PLACEHOLDER_12;
        protected int columnPlaceHolder13 = VariablesBillTracker.COLUMN_PLACEHOLDER_13;

        protected int columnReversalCode = VariablesBillTracker.COLUMN_REVERSAL_CODE;
        protected int columnNotes = VariablesBillTracker.COLUMN_NOTES;
        protected int columnExpandButton = VariablesBillTracker.COLUMN_EXPAND_BUTTON_NOTES;
        protected int columnException = VariablesBillTracker.COLUMN_EXCEPTION;
        protected int columnExceptionReason = VariablesBillTracker.COLUMN_EXCEPTION_REASON;
        protected int columnExpandButtonReason = VariablesBillTracker.COLUMN_EXPAND_BUTTON_REASON;
        protected int columnExceptionReasonRO = VariablesBillTracker.COLUMN_EXCEPTION_REASON_READ_ONLY;
        protected int columnLocalCurrency = VariablesBillTracker.COLUMN_LOCAL_CURRENCY;
        protected int columnRateCode = VariablesBillTracker.COLUMN_RATE_CODE;
        protected int columnPracticeArea = VariablesBillTracker.COLUMN_PRACTICE_AREA;
        protected int columnInvoicingAttorney = VariablesBillTracker.COLUMN_INVOICING_ATTORNEY;
        protected int columnBillingAttorneyTkid = VariablesBillTracker.COLUMN_PROFORMA_ATTORNEY_TKID;
        protected int columnArrangementCode = VariablesBillTracker.COLUMN_ARRANGEMENT_CODE;
        protected int columnID = VariablesBillTracker.COLUMN_ID;


        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            utility = new UtilityBillTracker(Page.User.Identity.Name.ToString().Substring(8));
            bearCode = new BearCode();

            Page.MaintainScrollPositionOnPostBack = true;
            int totalRows = 0;
            try
            {
                totalRows = dataGridView.Rows.Count;
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "Page_Load()", "int totalRows = dataGridView.Rows.Count");
            }

            utility.SetRowChanged(totalRows);
            hasPagerRow.Value = "false";

            //Set Tool Tips
            buttonSave.Attributes.Add("onmouseover", "Tip('" + VariablesBillTracker.TOOLTIP_SAVE_BUTTON + "')");
            buttonSave.Attributes.Add("onmouseout", "UnTip()");
            buttonChangeParameters.Attributes.Add("onmouseover", "Tip('" + VariablesBillTracker.TOOLTIP_RERUN_BUTTON + "')");
            buttonChangeParameters.Attributes.Add("onmouseout", "UnTip()");
            exitButton.Attributes.Add("onmouseover", "Tip('" + VariablesBillTracker.TOOLTIP_EXIT_BUTTON + "')");
            exitButton.Attributes.Add("onmouseout", "UnTip()");

            if (!Page.IsPostBack)
            {
                LogUserAccess();
            }

        }


        /// <summary>
        /// Called before the page is rendered.  This is used for the save functionality.<br />
        /// First the current data is archived to: BMcBillTrackerArchive<br />
        /// Then, the data is saved to BMcBillTracker
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

                        int billingPeriod = Int32.Parse(Request["billpd"].ToString());
                        bool thresholdApplied = Convert.ToBoolean(Request["threshold"].ToString());

                        String reversalCode = "";
                        String id = "";
                        String comment = "";
                        String exceptionReason = "";
                        int review1_DraftSent = 0;
                        int review2_ReadyToBill = 0;
                        int exception = 0;

                        try
                        {
                            id = thisGridViewRow.Cells[columnID].Text;
                            reversalCode = ((TextBox)thisGridViewRow.FindControl("reversalTB")).Text.Replace("'", "''");
                            review1_DraftSent = ((CheckBox)thisGridViewRow.FindControl("review1CB")).Checked == true ? 1 : 0;
                            review2_ReadyToBill = ((CheckBox)thisGridViewRow.FindControl("review2CB")).Checked == true ? 1 : 0;
                            comment = ((TextBox)thisGridViewRow.FindControl("notesTB")).Text.Replace("'", "''");
                            if (utility.IsAdminUser)
                            {
                                exceptionReason = ((TextBox)thisGridViewRow.FindControl("ReasonTB")).Text.Replace("'", "''");
                                if (exceptionReason.Equals(""))
                                {
                                    exception = 0;
                                }
                                else
                                {
                                    exception = 1;
                                }
                            }


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

                        SqlConnection con = null;

                        String sqlArchive = " INSERT INTO BMcBillTrackerArchive "
                                          + " SELECT ID "
                                          + " ,readyToBill "
                                          + " ,readyToBillDate "
                                          + " ,draftSent "
                                          + " ,draftSentDate "
                                          + " ,reversalCode "
                                          + " ,comment "
                                          + " ,exception "
                                          + " ,reason "
                                          + " ,billing_attorney "
                                          + " ,invoice_attorney "
                                          + " ,rate_code "
                                          + " ,tmatter "
                                          + " ,clnum "
                                          + " ,billingPeriod "
                                          + " ,updatedBy "
                                          + " ,updateTime "
                                          + " FROM BMcBillTracker "
                                          + " WHERE ID = " + id;


                        String sqlUpdate = " UPDATE BMcBillTracker "
                                          + " SET readyToBill = " + review2_ReadyToBill
                                              + " ,draftSent = " + review1_DraftSent
                                              + " ,reversalCode = '" + reversalCode + "' "
                                              + " ,comment = '" + comment + "' ";
                        if (utility.IsAdminUser)
                        {
                            sqlUpdate = sqlUpdate
                                              + " ,reason = '" + exceptionReason + "'"
                                              + " ,exception = " + exception;
                        }

                        sqlUpdate = sqlUpdate
                                              + " ,updatedBy = '" + Page.User.Identity.Name.ToString().Substring(8) + "' "
                                              + " ,updateTime = '" + DateTime.Now + "' "
                                              + " WHERE ID = " + id;

                        

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
                                command.CommandText = sqlUpdate;
                                command.ExecuteNonQuery();
                            }
                            catch (SqlException sqleUpdate)
                            {
                                Logger.QuickLog(errorLogFileName, sqleUpdate.Message, "Page_PreRender()", sqlUpdate);
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

                    } //end "if (rowChanged[r])"

                } //end "for (int r = 0; r < totalRows; r++)"
                dataGridView.DataBind();

            } //end if (Page.IsPostBack)
            else
            {
                dataGridView.SelectedIndex = -1;
            }

        } //end Page_PreRender()


        /// <summary>
        /// When the check is changed in the dataGrid, the row is marked as changed<br />
        /// this boolean is used in Page_PreRender to save changed rows<br />
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            CheckBox thisCheckBox = (CheckBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            utility.GetRowChanged()[row] = true;
        }


        /// <summary>
        /// Sets the SQL Parameters which set the column order in the stored procedure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SqlDataSource_Elite_uspBMcBEARBillTracker_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 0;
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode1(), "1" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode2(), "2" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode3(), "3" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode4(), "4" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode5(), "5" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode6(), "6" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode7(), "7" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode8(), "8" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode9(), "9" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode10(), "10" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode11(), "11" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode12(), "12" ));
            e.Command.Parameters.Add(new SqlParameter(utility.GetSQLColumnCode13(), "13" ));
        }


        /// <summary>
        /// Formats the rows on bind.  Sets visibility, currency, column headings, onclick commands, alignment, width, mouseover
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewDataDivRowBindEvent(object sender, GridViewRowEventArgs e)
        {
            String localCurrency = "";
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {

                //HEADER ROW SPECIFIC
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (utility.IsAdminUser)
                    {
                        e.Row.Cells[columnExceptionReasonRO].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnExceptionReasonRO].Attributes.CssStyle.Add("visibility", "hidden");
                    }
                    else
                    {
                        e.Row.Cells[columnExpandButtonReason].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnExpandButtonReason].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnExceptionReason].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnExceptionReason].Attributes.CssStyle.Add("visibility", "hidden");

                    }
                    dataGridView.Columns[columnPlaceHolder1].HeaderText = dataGridView.Columns[columnPlaceHolder1].HeaderText.Replace("COLUMN1", getHeading(1));
                    dataGridView.Columns[columnPlaceHolder2].HeaderText = dataGridView.Columns[columnPlaceHolder2].HeaderText.Replace("COLUMN2", getHeading(2));
                    dataGridView.Columns[columnPlaceHolder3].HeaderText = dataGridView.Columns[columnPlaceHolder3].HeaderText.Replace("COLUMN3", getHeading(3));
                    dataGridView.Columns[columnPlaceHolder4].HeaderText = dataGridView.Columns[columnPlaceHolder4].HeaderText.Replace("COLUMN4", getHeading(4));
                    dataGridView.Columns[columnPlaceHolder5].HeaderText = dataGridView.Columns[columnPlaceHolder5].HeaderText.Replace("COLUMN5", getHeading(5));
                    dataGridView.Columns[columnPlaceHolder6].HeaderText = dataGridView.Columns[columnPlaceHolder6].HeaderText.Replace("COLUMN6", getHeading(6));
                    dataGridView.Columns[columnPlaceHolder7].HeaderText = dataGridView.Columns[columnPlaceHolder7].HeaderText.Replace("COLUMN7", getHeading(7));
                    dataGridView.Columns[columnPlaceHolder8].HeaderText = dataGridView.Columns[columnPlaceHolder8].HeaderText.Replace("COLUMN8", getHeading(8));
                    dataGridView.Columns[columnPlaceHolder9].HeaderText = dataGridView.Columns[columnPlaceHolder9].HeaderText.Replace("COLUMN9", getHeading(9));
                    dataGridView.Columns[columnPlaceHolder10].HeaderText = dataGridView.Columns[columnPlaceHolder10].HeaderText.Replace("COLUMN10", getHeading(10));
                    dataGridView.Columns[columnPlaceHolder11].HeaderText = dataGridView.Columns[columnPlaceHolder11].HeaderText.Replace("COLUMN11", getHeading(11));
                    dataGridView.Columns[columnPlaceHolder12].HeaderText = dataGridView.Columns[columnPlaceHolder12].HeaderText.Replace("COLUMN12", getHeading(12));
                    dataGridView.Columns[columnPlaceHolder13].HeaderText = dataGridView.Columns[columnPlaceHolder13].HeaderText.Replace("COLUMN13", getHeading(13));

                }

                //DATAROW SPECIFIC
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    if (utility.IsAdminUser)
                    {
                        e.Row.Cells[columnExceptionReasonRO].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnExceptionReasonRO].Attributes.CssStyle.Add("visibility", "hidden");
                    }
                    else
                    {
                        e.Row.Cells[columnExpandButtonReason].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnExpandButtonReason].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnExceptionReason].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnExceptionReason].Attributes.CssStyle.Add("visibility", "hidden");

                    }


                    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";

                    e.Row.Cells[columnPlaceHolder1].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder2].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder3].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder4].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder6].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder5].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder7].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder8].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder9].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder10].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder11].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex); 
                    e.Row.Cells[columnPlaceHolder12].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnPlaceHolder13].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnLocalCurrency].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnException].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[columnExpandButton].Attributes["onclick"] = "populateBox('" + ((TextBox)e.Row.Cells[columnNotes].FindControl("NotesTB")).ClientID + "')";
                    e.Row.Cells[columnExpandButtonReason].Attributes["onclick"] = "populateBox('" + ((TextBox)e.Row.Cells[columnExceptionReason].FindControl("ReasonTB")).ClientID + "')";

                    if (!utility.IsAdminUser)
                    {
                        e.Row.Cells[columnExceptionReasonRO].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.dataGridView, "Select$" + e.Row.RowIndex);
                    }

                    localCurrency = e.Row.Cells[columnLocalCurrency].Text.ToString();

                    int columnPosition;
                    foreach (int i in utility.CurrencyColumnsLocal)
                    {
                        columnPosition = i + 1;
                        if (localCurrency.Equals("USD"))
                        {
                            e.Row.Cells[columnPosition].Text = "$" + e.Row.Cells[columnPosition].Text;
                        }
                        else if (localCurrency.Equals("GBP"))
                        {
                            e.Row.Cells[columnPosition].Text = "£" + e.Row.Cells[columnPosition].Text;
                        }
                        else if (localCurrency.Equals("EUR"))
                        {
                            e.Row.Cells[columnPosition].Text = "€" + e.Row.Cells[columnPosition].Text;
                        }
                        else if (localCurrency.Equals("JPY"))
                        {
                            e.Row.Cells[columnPosition].Text = "¥" + e.Row.Cells[columnPosition].Text;
                        }
                        else
                        {
                            e.Row.Cells[columnPosition].Text = e.Row.Cells[columnPosition].Text + " " + localCurrency;
                        }
                    }

                    foreach (int i in utility.CurrencyColumnsUSD)
                    {
                        columnPosition = i + 1;
                        e.Row.Cells[columnPosition].Text = "$" + e.Row.Cells[columnPosition].Text;
                    }

                    foreach (int i in utility.DateColumns)
                    {
                        columnPosition = i + 1;
                        if (e.Row.Cells[columnPosition].Text.Equals("Jan 01, 1900"))
                        {
                            e.Row.Cells[columnPosition].Text = "-";
                        }
                    }



                    if (((TextBox)e.Row.Cells[columnNotes].FindControl("NotesTB")).Text.Length > 28)
                    {
                        e.Row.Cells[columnNotes].Attributes.Add("onmouseover", "Tip('<b><u>Full Text:</u></b><br/>" + ((TextBox)e.Row.Cells[columnNotes].FindControl("NotesTB")).Text + "', WIDTH, 200)");
                        e.Row.Cells[columnNotes].Attributes.Add("onmouseout", "UnTip()");
                    }





                    if (e.Row.Cells[columnException].Text.ToLower().Equals("true"))
                    {
                        e.Row.Cells[columnException].Text = "Y";
                    }
                    else
                    {
                        e.Row.Cells[columnException].Text = "-";
                    }

                    if (utility.FirstPassThroughResultSet)
                    {
                        utility.FirstPassThroughResultSet = false;
                    }

                }

                //FOR BOTH HEADER AND DATAROWS

                //find columns that were locked previously and keep them locked.  
                //find columns that were hidden previously and keep them hidden.
                try
                {
                    //if lockedColumnNumber is zero or greater, then we need to keep things locked.
                    if (int.Parse(lockedColumnNumber.Value.ToString()) > -1)
                    {
                        //only look at columns with a column number <= the locked column (greater columns are unlocked)
                        for (int i = 2; i <= int.Parse(lockedColumnNumber.Value.ToString()); i++)
                        {

                            //if (Request["billtk"].ToString().Equals("All") && i == columnDynamic1)
                            if (i == VariablesBillTracker.COLUMN_PLACEHOLDER_1)
                            {
                                e.Row.Cells[i].CssClass = "locked";
                            }

                            //if (Request["invoiceatty"].ToString().Equals("All") && i == columnDynamic2)
                            if (i == VariablesBillTracker.COLUMN_PLACEHOLDER_2)
                            {
                                e.Row.Cells[i].CssClass = "locked";
                            }

                            //if (Request["billspec"].ToString().Equals("All") && i == columnDynamic3)
                            if (i == VariablesBillTracker.COLUMN_PLACEHOLDER_3)
                            {
                                e.Row.Cells[i].CssClass = "locked";
                            }

                            //if (Request["arrangement"].ToString().Equals("All") && i == columnDynamic4)
                            if (i == VariablesBillTracker.COLUMN_PLACEHOLDER_4)
                            {
                                e.Row.Cells[i].CssClass = "locked";
                            }

                        }
                    }
                }
                catch
                {
                    //catch if lockedColumnNumber is not set
                }




                e.Row.Cells[columnPlaceHolder1].HorizontalAlign = utility.GetColumnAlignment1();
                e.Row.Cells[columnPlaceHolder2].HorizontalAlign = utility.GetColumnAlignment2();
                e.Row.Cells[columnPlaceHolder3].HorizontalAlign = utility.GetColumnAlignment3();
                e.Row.Cells[columnPlaceHolder4].HorizontalAlign = utility.GetColumnAlignment4();
                e.Row.Cells[columnPlaceHolder5].HorizontalAlign = utility.GetColumnAlignment5();
                e.Row.Cells[columnPlaceHolder6].HorizontalAlign = utility.GetColumnAlignment6();
                e.Row.Cells[columnPlaceHolder7].HorizontalAlign = utility.GetColumnAlignment7();
                e.Row.Cells[columnPlaceHolder8].HorizontalAlign = utility.GetColumnAlignment8();
                e.Row.Cells[columnPlaceHolder9].HorizontalAlign = utility.GetColumnAlignment9();
                e.Row.Cells[columnPlaceHolder10].HorizontalAlign = utility.GetColumnAlignment10();
                e.Row.Cells[columnPlaceHolder11].HorizontalAlign = utility.GetColumnAlignment11();
                e.Row.Cells[columnPlaceHolder12].HorizontalAlign = utility.GetColumnAlignment12();
                e.Row.Cells[columnPlaceHolder13].HorizontalAlign = utility.GetColumnAlignment13();

                e.Row.Cells[columnPlaceHolder1].Attributes.Add("width", utility.GetColumnWidth1());
                e.Row.Cells[columnPlaceHolder2].Attributes.Add("width", utility.GetColumnWidth2());
                e.Row.Cells[columnPlaceHolder3].Attributes.Add("width", utility.GetColumnWidth3() );
                e.Row.Cells[columnPlaceHolder4].Attributes.Add("width", utility.GetColumnWidth4() );
                e.Row.Cells[columnPlaceHolder5].Attributes.Add("width", utility.GetColumnWidth5() );
                e.Row.Cells[columnPlaceHolder6].Attributes.Add("width", utility.GetColumnWidth6() );
                e.Row.Cells[columnPlaceHolder7].Attributes.Add("width", utility.GetColumnWidth7() );
                e.Row.Cells[columnPlaceHolder8].Attributes.Add("width", utility.GetColumnWidth8() );
                e.Row.Cells[columnPlaceHolder9].Attributes.Add("width", utility.GetColumnWidth9() );
                e.Row.Cells[columnPlaceHolder10].Attributes.Add("width", utility.GetColumnWidth10() );
                e.Row.Cells[columnPlaceHolder11].Attributes.Add("width", utility.GetColumnWidth11() );
                e.Row.Cells[columnPlaceHolder12].Attributes.Add("width", utility.GetColumnWidth12() );
                e.Row.Cells[columnPlaceHolder13].Attributes.Add("width", utility.GetColumnWidth13() );

                e.Row.Cells[columnLocalCurrency].Attributes.Add("width", VariablesBillTracker.FIXED_WIDTH_CURRENCY);
                e.Row.Cells[columnDraftSent].Attributes.Add("width", VariablesBillTracker.FIXED_WIDTH_CHECKBOXES);
                e.Row.Cells[columnReadyToBill].Attributes.Add("width", VariablesBillTracker.FIXED_WIDTH_CHECKBOXES);
                e.Row.Cells[columnReversalCode].Attributes.Add("width", VariablesBillTracker.FIXED_WIDTH_REVERSALCODE);
                e.Row.Cells[columnNotes].Attributes.Add("width", VariablesBillTracker.FIXED_WIDTH_NOTES);
                e.Row.Cells[columnException].Attributes.Add("width", VariablesBillTracker.FIXED_WIDTH_EXCEPTION);
                e.Row.Cells[columnExceptionReason].Attributes.Add("width", VariablesBillTracker.FIXED_WIDTH_EXCEPTIONREASON);
                e.Row.Cells[columnExceptionReasonRO].Attributes.Add("width", VariablesBillTracker.FIXED_WIDTH_EXCEPTIONREASON);
                e.Row.Cells[columnExpandButton].Attributes.Add("width", VariablesBillTracker.FIXED_WIDTH_EXPANDBUTTON);
                e.Row.Cells[columnExpandButtonReason].Attributes.Add("width", VariablesBillTracker.FIXED_WIDTH_EXPANDBUTTON);

                
            }
            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                bearCode.GridViewCustomizePagerRow(dataGridView, e.Row);

                if ((dataGridView.PageIndex + 1) == dataGridView.PageCount)
                {
                    if (dataGridView.Rows.Count < VariablesBillTracker.NUMBER_ROWS_ON_FINAL_PAGE_TO_KEEP_FIXED_PAGERROW)
                    {
                        //This is to remove the fixed footer class when it is the last page
                        e.Row.CssClass = "";
                    }
                }

            }

        }


        /// <summary>
        /// sets data grid size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewDataBoundEvent(object sender, EventArgs e)
        {
            data.Attributes.Add("Style", VariablesBillTracker.DATA_DIV_CUSTOM_STYLE);
            dataGridView.PageSize = VariablesBillTracker.DATA_PAGE_SIZE;
            if (dataGridView.PageCount > 1)
            {
                hasPagerRow.Value = "true";
            }

        }

        
        /// <summary>
        /// Clicking on the New Search button sends the user to the home page to reset the parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonChangeParametersClick(object sender, EventArgs e)
        {
            Response.Redirect(VariablesBillTracker.HOME_PAGE);
        }


        #region Custom Get Methods
        public String getBillingAttorney()
        {
            String billtk = Request["billtk"].ToString() == "" ? "All" : Request["billtk"].ToString();

            return billtk;
        }

        public String getBillingSpecialist()
        {
            String billspec = Request["billspec"].ToString() == "" ? "All" : Request["billspec"].ToString();
            return billspec;
        }

        public String getInvoicingAttorney()
        {
            String invoiceAtty = Request["invoiceatty"].ToString() == "" ? "All" : Request["invoiceatty"].ToString();

            return invoiceAtty;
        }

        public String getBillingPeriod()
        {
            String billPd = Request["billpd"].ToString();
            String billPdYear = billPd.Substring(0, 4);
            String billPdMonth = billPd.Substring(4, 2);

            return billPdMonth + "/" + billPdYear;
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
        /// User access is inserted into the BMcBillTrackerLog table
        /// </summary>
        protected void LogUserAccess()
        {
            byte thresholdFlag = 0;
            if (Request["threshold"].ToString().ToUpper().Equals("TRUE"))
            {
                thresholdFlag = 1;
            }

            SqlConnection con = null;
            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO dbo.BMcBillTrackerLog ("
                                        + "  networkId "
                                        + " ,logIntime "
                                        + " ,billingAttyTkid "
                                        + " ,invoiceAttyTkid "
                                        + " ,billSpecialist "
                                        + " ,thresholdUsed "
                                        + " ,billingPeriod "
                                        + " ,arrangementCode "
                                        + " ,practiceArea "
                                        + " ,clnum "
                                        + " ,tmatter "
                                        + " ,readyToBill "
                                        + " ,office "
                                        + " ,officeType "
                                        + " ,Exceptions "
                                  + ") VALUES ( "
                                        + "'" + Page.User.Identity.Name.ToString().Substring(8) + "', "
                                        + "'" + DateTime.Now + "', "
                                        + "'" + Request["billtk"].ToString() + "', "
                                        + "'" + Request["invoiceatty"].ToString() + "', "
                                        + "'" + Request["billspec"].ToString() + "', "
                                              + thresholdFlag + ", "
                                              + Request["billpd"].ToString() + ", "
                                        + "'" + Request["arrangement"].ToString() + "', "
                                        + "'" + Request["pa"].ToString() + "', "
                                        + "'" + Request["clnum"].ToString() + "', "
                                        + "'" + Request["matter"].ToString() + "', "
                                        + "'" + Request["rtb"].ToString() + "', "
                                        + "'" + Request["iaofc"].ToString() + "', "
                                        + "'" + Request["ofcType"].ToString() + "', "
                                        + "'" + Request["exception"].ToString() + "' "
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


        /// <summary>
        /// Keeps the Pager Row style set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewDataSelectedIndexChanged(object sender, EventArgs e)
        {
            bearCode.GridViewCustomizePagerRow(dataGridView, dataGridView.BottomPagerRow);
        }


        /// <summary>
        /// remove any selected row when the page is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridViewDataPageIndexChanged(object sender, EventArgs e)
        {
            dataGridView.SelectedIndex = -1;
        }


        /// <summary>
        /// Gets the Column Headings for the user's preferences
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        protected String getHeading(int columnNumber)
        {
            String columnHeading = "";

            switch (columnNumber)
            {
                case 1:
                    columnHeading = utility.GetColumnHeading1();
                    break;
                case 2:
                    columnHeading = utility.GetColumnHeading2();
                    break;
                case 3:
                    columnHeading = utility.GetColumnHeading3();
                    break;
                case 4:
                    columnHeading = utility.GetColumnHeading4();
                    break;
                case 5:
                    columnHeading = utility.GetColumnHeading5();
                    break;
                case 6:
                    columnHeading = utility.GetColumnHeading6();
                    break;
                case 7:
                    columnHeading = utility.GetColumnHeading7();
                    break;
                case 8:
                    columnHeading = utility.GetColumnHeading8();
                    break;
                case 9:
                    columnHeading = utility.GetColumnHeading9();
                    break;
                case 10:
                    columnHeading = utility.GetColumnHeading10();
                    break;
                case 11:
                    columnHeading = utility.GetColumnHeading11();
                    break;
                case 12:
                    columnHeading = utility.GetColumnHeading12();
                    break;
                case 13:
                    columnHeading = utility.GetColumnHeading13();
                    break;
                default:
                    break;
            }

            return columnHeading;
        }


    }
}
