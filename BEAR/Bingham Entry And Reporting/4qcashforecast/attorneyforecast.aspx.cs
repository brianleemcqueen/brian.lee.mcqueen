using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BEAR
{
    public partial class attorneyforecast : System.Web.UI.Page
    {
        protected BEAR._4qcashforecast.UtilityAttorneyForecast utility;
        protected int widestClientName = Variables4QCashForecast.CLIENT_NAME_MINIMUM_WIDTH;

        protected int columnDataSelectIcon = 0;
        protected int columnDataRowNumber = 1;
        protected int columnDataClientNumber = 2;
        protected int columnDataClientName = 3;
        protected int columnDataNetInv = 4;
        protected int columnDataAR = 5;
        protected int columnDataWIP = 6;
        protected int columnDataReceipts = 7;
        protected int columnDataVariance = 8;
        protected int columnDataForecast = 9;
        protected int columnDataNotes = 10;

        protected int columnDetailMatterNumber = 0;
        protected int columnDetailMatterDescription = 1;
        protected int columnDetailNetInv = 2;
        protected int columnDetailAR = 3;
        protected int columnDetailWIP = 4;
        protected int columnDetailReceipts = 5;

        protected int columnHeaderTKID = 0;
        protected int columnHeaderAttorneyName = 1;
        protected int columnHeaderTitle = 2;
        protected int columnHeaderLocation = 3;
        protected int columnHeaderPracticeArea = 4;
        
        //An Error Log will be generated with this location / name for any SQL errors
        String errorLogFileName = Variables4QCashForecast.ERROR_LOG_FILE_NAME;

        /// <summary>
        /// Ran each time the page is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Variables4QCashForecast variables = new Variables4QCashForecast();
            variables.tkid = Request["tkid"];

            utility = new BEAR._4qcashforecast.UtilityAttorneyForecast(); 

            /**
             * adding mouseover events to allow for enhanced tool tips (wx_tooltip.js)
             */
            button_submitTkid.Attributes.Add("onmouseover", "Tip('" + Variables4QCashForecast.TOOLTIP_SUBMIT_BUTTON + "')");
            button_submitTkid.Attributes.Add("onmouseout", "UnTip()");


            /**
             * after postback, scroll position is saved.  Also used to reduce page flashing on postback
             */
            Page.MaintainScrollPositionOnPostBack = true;
            
            
            /**
             * tkid is passed in through the query string.  If the value is null, the user is at the screen 
             * requesting a tkid with a Bingham picture.  If there is a TKID, the GridViews will build.
             * By default, the controlls are set as if tkid is null (login page).
             * This 'if' changes the display when a tkid has been entered, adds toolTips to the buttons and sets up the boolean to track 
             * the number of rows in GridView2
             */
            if (variables.tkid != null)
            {
                button_submitTkid.Visible = false;
                textbox_tkid.Visible = false;
                picture.Visible = false;
                options.Visible = true;

                /**
                 * substring(8) is to remove //BINGHAM
                 * Page.User.Identity.Name requires Windows Authentication to be 
                 * enabled at the webserver
                 */ 
                CreateLogEntry(Page.User.Identity.Name.ToString().Substring(8));

                SetDataDivHeight();

                /**
                 * adding mouseover events to allow for enhanced tool tips (wx_tooltip.js)
                 */
                buttonSave.Attributes.Add("onmouseover", "Tip('" + Variables4QCashForecast.TOOLTIP_SAVE_BUTTON + "')");
                buttonSave.Attributes.Add("onmouseout", "UnTip()");

                buttonPrint.Attributes.Add("onmouseover", "Tip('" + Variables4QCashForecast.TOOLTIP_PRINT_BUTTON + "')");
                buttonPrint.Attributes.Add("onmouseout", "UnTip()");

                buttonReport.Attributes.Add("onmouseover", "Tip('" + Variables4QCashForecast.TOOLTIP_REPORT_BUTTON + "')");
                buttonReport.Attributes.Add("onmouseout", "UnTip()");

                button_changeTkid.Attributes.Add("onmouseover", "Tip('" + Variables4QCashForecast.TOOLTIP_TKID_BUTTON + "')");
                button_changeTkid.Attributes.Add("onmouseout", "UnTip()");

                exitButton.Attributes.Add("onmouseover", "Tip('" + Variables4QCashForecast.TOOLTIP_EXIT_BUTTON + "')");
                exitButton.Attributes.Add("onmouseout", "UnTip()");


                int totalRows = GridView2.Rows.Count;

                if (Request["tkid"].Equals("none"))
                {
                    Response.Redirect(Variables4QCashForecast.HOME_PAGE);
                }
                else if (textbox_tkid.Text.Equals("Enter TKID") && totalRows == 0)
                {
                    /**
                     * this would fire when a url is entered with an invalid TKID#.
                     * This could happen by bypassing the login screen.  In this case, if the TKID is invalid,
                     * the user is redirected to the login page.
                     */
                     Response.Redirect(Variables4QCashForecast.ERROR_PAGE_TKID_NOT_FOUND);
                     
                }
                else
                {
                    utility.SetRowChanged(totalRows);
                }

            }
            else
            {
                ///adds onFocus event to clear the contents of the TKID box when the cursor is placed inside the textbox
                textbox_tkid.Attributes.Add("onFocus", "textbox_tkid_onFocus()"); 
                
                ///setting this to false will create a new entry in the database log
                Session["LogCreated"] = false;

                ///used to ensure the client name is not wrapped and has a minimum length of 190px
                widestClientName = 190;
            }



            if (! Page.IsPostBack)
            {

            }

        }


        /// <summary>
        /// Submitting a new TKID will first add leading zeros if necessary.<br />
        /// The TKID is then validated to ensure it is in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void button_submitTkid_Click(object sender, EventArgs e)
        {
            if (textbox_tkid.Text.Length == 3)
                textbox_tkid.Text = "00" + textbox_tkid.Text;
            else if (textbox_tkid.Text.Length == 4)
                textbox_tkid.Text = "0" + textbox_tkid.Text;

            int tkidCode = ValidateTkid();
            if (tkidCode == 1)
            {
                LabelServerMessage.Visible = false;
                Response.Redirect(Variables4QCashForecast.HOME_PAGE + "?tkid=" + Server.UrlEncode(textbox_tkid.Text));
            }
            else if (tkidCode == 0)
            {
                String tkid = textbox_tkid.Text;
                LabelServerMessage.Visible = true;
                LabelServerMessage.Text = "TKID: " + tkid + " Not Found.  Please re-enter.";
                textbox_tkid.Text = tkid + " Not Found";
            }
            else if (tkidCode == -1)
            {
                LabelServerMessage.Visible = true;
                LabelServerMessage.Text = "Error Retrieving Data. Please Try Again.";
            }

        }


        /// <summary>
        /// Clicking the Report Button will redirect to Reporting Services
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonReport_Click(object sender, EventArgs e)
        {
            Page_PreRender(sender, e);
            Response.Redirect(System.Configuration.ConfigurationSettings.AppSettings["ReportServer"]
                            + Variables4QCashForecast.REPORT_FOLDER
                            + "/" 
                            + Variables4QCashForecast.REPORT_SERVICES_REPORT_NAME 
                            + "&rc:Parameters=true&rs:Command=Render&rs:ClearSession=true"
                            + "&ApplicationServer="
                            + Environment.MachineName.ToString()
                            + "&tkid="
                            + Request["tkid"]
                            );
        }


        /// <summary>
        /// Clicking the Print Button will redirect to Reporting Services
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonPrint_Click(object sender, EventArgs e)
        {
            Page_PreRender(sender, e);
            Response.Redirect(System.Configuration.ConfigurationSettings.AppSettings["ReportServer"]
                            + Variables4QCashForecast.REPORT_FOLDER
                            + "/" 
                            + Variables4QCashForecast.REPORT_SERVICES_PRINT_NAME
                            + "&rc:Parameters=false&rs:Command=Render&rs:ClearSession=true"
                            + "&ApplicationServer="
                            + Environment.MachineName.ToString()
                            + "&tkid="
                            + Request["tkid"]
                            );
        }
        

        /// <summary>
        /// Clicking the TKID button removed the TKID from the URL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void button_changeTkid_Click(object sender, EventArgs e)
        {
            Response.Redirect(Variables4QCashForecast.HOME_PAGE);
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
        /// IndexChanged is called when a row is selected in the data GridView<br />
        /// When a row is selected, two variables are set = clnum is set for the detail section's DataSource(client number) and <br />
        /// the session variable DetailGridHeader is set to infomation displayed in the header of the detail GridView<br />
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_IndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridView2.SelectedRow;
            Session["selectedIndex"] = row.RowIndex;
            SqlDataSource_Elite_WPReports_AttnyForecastDetails.SelectParameters["clnum"].DefaultValue =
                GridView2.SelectedDataKey.Value.ToString();
            Session["detailGridHeader"] =     "Line #: " 
                                            + (int.Parse(row.RowIndex.ToString())+1).ToString()
                                            + ":  Client = "
                                            + row.Cells[2].Text
                                            + " / " 
                                            + row.Cells[3].Text;
            //GridView2.SelectedRow.Attributes["style"] = GridView2.SelectedRowStyle.ToString();
            GridView3.Visible = true;

        }


        /// <summary>
        /// Performed when Sorting GridView2 (main data).  When Sorting, the detailed section is hidden and any selected row is off
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid_Data_Sorting(object sender, EventArgs e)
        {
            GridView3.Visible = false;
            Session["turnOffSelected"] = "true";
        }


        /// <summary>
        /// Looping through all the rows that are changed saving Notes and Forecast for each changed row
        /// after all changed rows have been saved to the database, the grids are rebound
        /// Rebinding GridView3 (Detail) is done to allow it to continue to show after a postback
        /// 
        /// Called before the page is rendered.  This is used for the save functionality.<br />
        /// SQL:<br />
        /// UPDATE [Forecast_Atty] "
        /// " SET [Notes] = '" + comment + "' , "
        /// " [forecast] = " + forecast + " , "
        /// " [variance] = " + variance
        /// WHERE [Tkinit] = '" + Request["tkid"] + "' AND [clnum] = '" + clientID + "' ";
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                int totalRows = GridView2.Rows.Count;
                for (int r = 0; r < totalRows; r++)
                {
                    if (utility.GetRowChanged()[r])
                    {
                        Session["rowEdited"] = r;

                        GridViewRow thisGridViewRow = GridView2.Rows[r];
                        String clientID = thisGridViewRow.Cells[2].Text;
                        String comment = ((TextBox)thisGridViewRow.FindControl("notesTB")).Text;

                        decimal receipts = 0;
                        try
                        {
                            receipts = decimal.Parse(thisGridViewRow.Cells[7].Text);
                        }
                        catch (FormatException)
                        {
                            //this is to catch receipts of zero which are set to '-'
                        }

                        decimal forecast = 0;
                        try
                        {
                            forecast = decimal.Parse(((TextBox)thisGridViewRow.FindControl("forecastTB")).Text);
                        }
                        catch (FormatException)
                        {
                            //this is to catch forecast when set to a nonNumber
                            //non numbers are converted to zero
                        }

                        decimal variance = receipts - forecast;
                        String sql = "UPDATE [Forecast_Atty] " +
                                    " SET [Notes] = '" + comment + "' , " +
                                        " [forecast] = " + forecast + " , " +
                                        " [variance] = " + variance +
                                    " WHERE [Tkinit] = '" + Request["tkid"] + "' AND [clnum] = '" + clientID + "' ";
                        SqlConnection con = null;
                        try
                        {
                            con = new SqlConnection(
                                    ConfigurationManager.ConnectionStrings["WPReportsConnectionString"].ConnectionString);
                            con.Open();
                            SqlCommand command = con.CreateCommand();
                            command.CommandText = sql;
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            //after the query executes, update variance on the GridView
                            thisGridViewRow.Cells[8].Text = variance.ToString();

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

                GridView2.DataBind();
                GridView3.DataBind();
                if (Session["turnOffSelected"] != null)
                {
                    if (Session["turnOffSelected"].Equals("true"))
                    {
                        GridView2.SelectedIndex = -1;
                        Session["turnOffSelected"] = "false";
                    }
                }


            }
        }


        /// <summary>
        /// Ensure the header div does not have any rows that will wrap.  <br />
        /// multiply the length by 7 (average character is 7 pixels wide)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_HeaderDiv_RowBindEvent(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[columnHeaderAttorneyName] != null)
                {
                    String attorneyName = e.Row.Cells[1].Text.ToString();
                    if ((attorneyName.Length * 7) > 190)
                    {
                        GridView1.Columns[columnHeaderAttorneyName].ItemStyle.Width = attorneyName.Length * 7;
                        GridView1.Columns[columnHeaderAttorneyName].ItemStyle.Wrap = false;
                    }

                    String title = e.Row.Cells[columnHeaderTitle].Text.ToString();
                    if ((title.Length * 7) > 130)
                    {
                        GridView1.Columns[columnHeaderTitle].ItemStyle.Width = title.Length * 7;
                        GridView1.Columns[columnHeaderTitle].ItemStyle.Wrap = false;
                    }

                    String location = e.Row.Cells[columnHeaderLocation].Text.ToString();
                    if ((location.Length * 7) > 130)
                    {
                        GridView1.Columns[columnHeaderLocation].ItemStyle.Width = location.Length * 7;
                        GridView1.Columns[columnHeaderLocation].ItemStyle.Wrap = false;
                    }

                    String practiceArea = e.Row.Cells[columnHeaderPracticeArea].Text.ToString();
                    if ((practiceArea.Length * 7) > 130)
                    {
                        GridView1.Columns[columnHeaderPracticeArea].ItemStyle.Width = practiceArea.Length * 7;
                        GridView1.Columns[columnHeaderPracticeArea].ItemStyle.Wrap = false;
                    }

                }

            }

        }


        /// <summary>
        /// As the Data Grid is being bound, in the <br />
        /// > DataRow, ToolTips are added to the select row button; zeros are replaced with '-'<br />
        /// > Header, The horizontal alignment is changed on columns not containing numbers to center; the width of the client name column is adjusted to ensure there <br />
        /// is not wrapping, ToolTips are added to the Variance Column and totals are added to the column headers.  HTML code is added to put the totals on a second line.<br />
        /// The HTML code will render because HtmlEncode is set to false in the aspx page.<br />
        /// > EmptyDataRow, changes the grid to an information table stating that no details exist for the tkid.  <br />
        /// This is here if someone comes in with the tkid in the URL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_DataDiv_RowBindEvent(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[columnDataSelectIcon].Attributes.Add("onmouseover","Tip('" + Variables4QCashForecast.TOOLTIP_DETAILS + "')");
                e.Row.Cells[columnDataSelectIcon].Attributes.Add("onmouseout", "UnTip()");
 
                if (e.Row.RowState.ToString().Contains("Selected"))
                {
                    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this,false);";
                }
                else
                {
                    e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this,true);";
                }
                e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";

                e.Row.Cells[columnDataRowNumber].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + e.Row.RowIndex);
                e.Row.Cells[columnDataClientNumber].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + e.Row.RowIndex);
                e.Row.Cells[columnDataClientName].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + e.Row.RowIndex);
                e.Row.Cells[columnDataNetInv].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + e.Row.RowIndex);
                e.Row.Cells[columnDataAR].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + e.Row.RowIndex);
                e.Row.Cells[columnDataWIP].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + e.Row.RowIndex);
                e.Row.Cells[columnDataReceipts].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + e.Row.RowIndex);
                e.Row.Cells[columnDataVariance].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + e.Row.RowIndex);

                e.Row.Cells[columnDataForecast].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("forecastTB").ClientID + "')";
                e.Row.Cells[columnDataNotes].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("NotesTB").ClientID + "')";

                if (((TextBox)e.Row.Cells[columnDataNotes].FindControl("NotesTB")).Text.Length > 20)
                {
                    e.Row.Cells[columnDataNotes].Attributes.Add("onmouseover", "Tip('<b><u>Full Text:</u></b><br/>" + ((TextBox)e.Row.Cells[columnDataNotes].FindControl("NotesTB")).Text + "', WIDTH, 200)");
                    e.Row.Cells[columnDataNotes].Attributes.Add("onmouseout", "UnTip()");
                }


                if (e.Row.Cells[columnDataNetInv].Text.Equals("0")) e.Row.Cells[4].Text = "-";
                if (e.Row.Cells[columnDataAR].Text.Equals("0")) e.Row.Cells[5].Text = "-";
                if (e.Row.Cells[columnDataWIP].Text.Equals("0")) e.Row.Cells[6].Text = "-";
                if (e.Row.Cells[columnDataReceipts].Text.Equals("0")) e.Row.Cells[7].Text = "-";
                if (e.Row.Cells[columnDataVariance].Text.Equals("0")) e.Row.Cells[8].Text = "-";

                if (e.Row.Cells[columnDataClientName] != null)
                {
                    String clientName = e.Row.Cells[3].Text.ToString();

                    int clientNameLength = clientName.Length * 7;

                    if (clientNameLength > widestClientName)
                    {
                        widestClientName = clientNameLength;
                        GridView2.Columns[columnDataClientName].ItemStyle.Width = widestClientName;
                        GridView2.Columns[columnDataClientName].ItemStyle.Wrap = false;
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[columnDataClientNumber].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[columnDataClientName].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[columnDataNotes].HorizontalAlign = HorizontalAlign.Center;

                GridView2.Columns[columnDataClientName].ItemStyle.Width = widestClientName;
                GridView2.Columns[columnDataClientName].ItemStyle.Wrap = false;
                e.Row.Cells[columnDataVariance].Attributes.Add("onmouseover", "Tip('Forecast Amount Less Receipts Amount<br><small><em>press Enter to update this column</em></small>')");
                e.Row.Cells[columnDataVariance].Attributes.Add("onmouseout", "UnTip()");

                String[] totals = CalculateDataTotals();
                GridView2.Columns[columnDataNetInv].HeaderText = "<u>Net Inv</u><br />" + totals[0];
                GridView2.Columns[columnDataAR].HeaderText = "<u>AR</u><br />" + totals[1];
                GridView2.Columns[columnDataWIP].HeaderText = "<u>WIP</u><br />" + totals[2];
                GridView2.Columns[columnDataReceipts].HeaderText = "<u>Receipts</u><br />" + totals[3];
                GridView2.Columns[columnDataVariance].HeaderText = "<u>Variance</u><br />" + totals[4];
                GridView2.Columns[columnDataForecast].HeaderText = "<u>Forecast</u><br />" + totals[5];

            }

        }


        /// <summary>
        /// As the Detail Grid is being bound, in the <br />
        /// > DataRow, zeros are replaced with '-'<br />
        /// > Header, two new rows are added to the top for heading information<br />
        /// > EmptyDataRow, changes the grid to an information table stating that no details exist for the selected row.<br />
        /// > Footer, checks to see if there is more than one row.  If there is more than one, totals are added to the footer.<br />
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView_DetailDiv_RowBindEvent(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[columnDetailNetInv].Text.Equals("0")) e.Row.Cells[columnDetailNetInv].Text = "-";
                if (e.Row.Cells[columnDetailAR].Text.Equals("0")) e.Row.Cells[columnDetailAR].Text = "-";
                if (e.Row.Cells[columnDetailWIP].Text.Equals("0")) e.Row.Cells[columnDetailWIP].Text = "-";
                if (e.Row.Cells[columnDetailReceipts].Text.Equals("0")) e.Row.Cells[columnDetailReceipts].Text = "-";

                e.Row.Attributes["onmouseover"] = "javascript:setDetailMouseOverColor(this,true);";
                e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";

            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                //Build custom header.
                GridView oGridView = (GridView)sender;
                GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                GridViewRow tGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell tTableCell = new TableCell();
                tTableCell.Text = "Matter Details";
                tTableCell.ColumnSpan = 6;
                tTableCell.HorizontalAlign = HorizontalAlign.Center;
                tGridViewRow.Cells.Add(tTableCell);

                oGridView.Controls[0].Controls.AddAt(0, tGridViewRow);


                TableCell oTableCell = new TableCell();
                oTableCell.Text = Session["detailGridHeader"].ToString();
                oTableCell.ColumnSpan = 6;
                oGridViewRow.Cells.Add(oTableCell);

                oGridView.Controls[0].Controls.AddAt(1, oGridViewRow);

            }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                if (Session["detailGridHeader"] != null)
                {
                    e.Row.Cells[0].Text = "There are No Matter Details for " + Session["detailGridHeader"].ToString();
                    e.Row.Cells[0].Width = e.Row.Cells[0].Text.Length * 7;
                    e.Row.Cells[0].Wrap = false;
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[0].Height = 22;
                }
                else
                {
                    e.Row.Cells[0].Text = "There are No Matter Details for this Row";
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (GridView3.Rows.Count > 1)
                {
                    GridView3.ShowFooter = true;
                    CalculateDetailTotals(e);
                }
                else
                {
                    GridView3.ShowFooter = false;
                }
            }

        }


        /// <summary>
        /// Provides an array to the GridView_DataDiv_RowBindEvent.  The totals of each column are added to the header column.
        /// Zeros are replaced with '-'
        /// 
        /// SQL<br />
        /// " SELECT "<br />
        /// " isNull(SUM([Forecast]),0) as Ttl_Forecast "<br />
        /// " , isNull(SUM([Variance]),0) as Ttl_Variance "<br />
        /// " , isNull(SUM([Net_Investment]),0) as Ttl_Net_Investment "<br />
        /// " , isNull(SUM([AR]),0) as Ttl_AR "<br />
        /// " , isNull(SUM([WIP]),0) as Ttl_WIP "<br />
        /// " , isNull(SUM([CurrentCR]),0) as Ttl_CurrentCR "<br />
        /// " FROM [Forecast_Atty]  "<br />
        /// " WHERE [Tkinit] = '" + Request["tkid"] + "' ";
        /// </summary>
        /// <returns></returns>
        protected String[] CalculateDataTotals()
        {

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["WPReportsConnectionString"].ConnectionString);

            decimal ttlNetInvestment = 0;
            decimal ttlAR = 0;
            decimal ttlWIP = 0;
            decimal ttlForecast = 0;
            decimal ttlCurrentCR = 0;
            decimal ttlVariance = 0;
            String[] totalArray = { "-", "-", "-", "-", "-", "-" };

            String sql = "";
            try
            {
                sql = " SELECT " +
                      " isNull(SUM([Forecast]),0) as Ttl_Forecast " +
                    " , isNull(SUM([Variance]),0) as Ttl_Variance " +
                    " , isNull(SUM([Net_Investment]),0) as Ttl_Net_Investment " +
                    " , isNull(SUM([AR]),0) as Ttl_AR " +
                    " , isNull(SUM([WIP]),0) as Ttl_WIP " +
                    " , isNull(SUM([CurrentCR]),0) as Ttl_CurrentCR " +
                    " FROM [Forecast_Atty]  " +
                    " WHERE [Tkinit] = '" + Request["tkid"] + "' ";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ttlForecast = decimal.Parse(reader["Ttl_Forecast"].ToString());
                    ttlVariance = decimal.Parse(reader["Ttl_Variance"].ToString());
                    ttlNetInvestment = decimal.Parse(reader["Ttl_Net_Investment"].ToString());
                    ttlAR = decimal.Parse(reader["Ttl_AR"].ToString());
                    ttlWIP = decimal.Parse(reader["Ttl_WIP"].ToString());
                    ttlCurrentCR = decimal.Parse(reader["Ttl_CurrentCR"].ToString());
                }

                reader.Close();

                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                nfi.NumberDecimalDigits = 0;

                totalArray[0] = ttlNetInvestment == 0 ? "-": ttlNetInvestment.ToString("N", nfi);
                totalArray[1] = ttlAR == 0 ? "-" : ttlAR.ToString("N", nfi);
                totalArray[2] = ttlWIP == 0 ? "-": ttlWIP.ToString("N", nfi);
                totalArray[3] = ttlCurrentCR == 0 ? "-": ttlCurrentCR.ToString("N", nfi);
                totalArray[4] = ttlVariance == 0 ? "-": ttlVariance.ToString("N", nfi);
                totalArray[5] = ttlForecast == 0 ? "-" : ttlForecast.ToString("N", nfi);

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "CalculateDataTotals()", sql);
                return totalArray;
            }
            catch (FormatException fe)
            {
                Logger.QuickLog(errorLogFileName, fe.Message, "CalculateDataTotals().FormatException", "This Error is thrown at times when formatting the total row of the Data Grid (GridView2).  It should not cause issues with the user experience of the application since it is being caught.");
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "CalculateDetailTotals()", "Setting Up SQL.  No Selected Row. GridView3 is Hidden.");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return totalArray;

        }


        /// <summary>
        /// If there is more than one line of the detail section, a total line will appear.  <br />
        /// This method calculates and displays that total. <br />
        /// zero values are displayed as '-' <br />
        /// SQL <br />
        /// SELECT isNull(SUM([Net_Investment]),0) AS Ttl_Net_Investment, " <br />
        ///    " isNull(SUM([AR]),0) AS Ttl_AR, " <br />
        ///    " isNull(SUM([WIP]),0) AS Ttl_WIP, " <br />
        ///    " isNull(SUM([CurrentCR]),0) AS Ttl_CurrentCR " <br />
        /// " FROM [Forecast_Atty_Detail]" <br />
        /// " WHERE [Tkinit] = '" + Request["tkid"] + "' " <br />
        /// " AND [clnum] = '" + GridView2.SelectedDataKey.Value.ToString() + "' ";
        /// </summary>
        /// <param name="e"></param>
        protected void CalculateDetailTotals(GridViewRowEventArgs e)
        {

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["WPReportsConnectionString"].ConnectionString);

            decimal ttlNetInvestment = 0;
            decimal ttlAR = 0;
            decimal ttlWIP = 0;
            decimal ttlCurrentCR = 0;
            String sql = "";
            try
            {
                sql = " SELECT isNull(SUM([Net_Investment]),0) AS Ttl_Net_Investment, " +
                                " isNull(SUM([AR]),0) AS Ttl_AR, " +
                                " isNull(SUM([WIP]),0) AS Ttl_WIP, " +
                                " isNull(SUM([CurrentCR]),0) AS Ttl_CurrentCR " +
                         " FROM [Forecast_Atty_Detail]" +
                         " WHERE [Tkinit] = '" + Request["tkid"] + "' " +
                              "AND [clnum] = '" + GridView2.SelectedDataKey.Value.ToString() + "' ";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ttlNetInvestment = decimal.Parse(reader["Ttl_Net_Investment"].ToString());
                    ttlAR = decimal.Parse(reader["Ttl_AR"].ToString());
                    ttlWIP = decimal.Parse(reader["Ttl_WIP"].ToString());
                    ttlCurrentCR = decimal.Parse(reader["Ttl_CurrentCR"].ToString());
                }

                reader.Close();

                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                nfi.NumberDecimalDigits = 0;

                e.Row.Cells[columnDetailNetInv].Text = ttlNetInvestment == 0 ? "-" : ttlNetInvestment.ToString("N", nfi);
                e.Row.Cells[columnDetailAR].Text = ttlAR == 0 ? "-" : ttlAR.ToString("N", nfi);
                e.Row.Cells[columnDetailWIP].Text = ttlWIP == 0 ? "-" : ttlWIP.ToString("N", nfi);
                e.Row.Cells[columnDetailReceipts].Text = ttlCurrentCR == 0 ? "-" : ttlCurrentCR.ToString("N", nfi);

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "CalculateDetailTotals()", sql);
            }
            catch (FormatException fe)
            {
                Logger.QuickLog(errorLogFileName, fe.Message, "CalculateDetailTotals().FormatException", "This Error is thrown at times when formatting the total row of the Detail Grid (GridView3).  It should not cause issues with the user experience of the application since it is being caught.");
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "CalculateDetailTotals()", "Setting Up SQL.  No Selected Row. GridView3 is Hidden.");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }


        }


        /// <summary>
        /// The default number of rows on the GridView is set.  The size of each row is also in a variable (remember to include padding)
        /// This funtion is to resize the div when there are less than the default number of rows.
        /// It also pushes up the detail grid.
        /// </summary>
        protected void SetDataDivHeight()
        {

            int numberOfDataRows = GridView2.Rows.Count;
            int defaultNumberOfRows = 14;

            if (numberOfDataRows <= defaultNumberOfRows)
            {
                int sizeOfEachRow = 24;
                //set height of datadiv & turn off scroll bar
                //+ 2 to account for header & footer
                String newStyle = "height:" + (((numberOfDataRows + 2) * sizeOfEachRow)) + "px";
                data.Attributes["style"] = newStyle;

                //set new top of detail section
                int detailDivTop = 464; //default top
                int detailDivTopAdjustment = ((defaultNumberOfRows - numberOfDataRows) * sizeOfEachRow);
                newStyle = "top:" + (detailDivTop - detailDivTopAdjustment) + "px;";
                details.Attributes["style"] = newStyle;


            }
        }


        /// <summary>
        /// Based on the value entered on the login page, this function checks to ensure the tkid is in the Forecast_atty table of the database.<br />
        /// Based on the result a code of -1, 0 or 1 is returned.<br />
        ///      - 1 = TKID is in the database and processing can continue<br />
        ///     - 0 = TKID was not found in the database<br />
        ///     - -1 = error in validation<br />
        /// SQL:<br />
        /// SELECT DISTINCT [Tkinit] <br />
        /// FROM [Forecast_Atty]  <br />
        /// WHERE [Tkinit] = '" + textbox_tkid.Text + "' ";<br />
        /// </summary>
        /// <returns></returns>
        protected int ValidateTkid()
        {
            int returnCode = -1;
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["WPReportsConnectionString"].ConnectionString);

            String sql = " SELECT DISTINCT [Tkinit] " +
                         " FROM [Forecast_Atty]  " +
                         " WHERE [Tkinit] = '" + textbox_tkid.Text + "' ";
            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    returnCode = 1;
                }
                else
                {
                    returnCode = 0;
                }

                reader.Close();

                return returnCode;

            }
            catch (SqlException)
            {
                return -1;
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

        }


        /// <summary>
        /// CreateLogEntry creates a log in a database table showing Network User ID, log in date/time and tkid being edited.<br />
        /// SQL<br />
        /// INSERT INTO [WPReports].[dbo].[BMc4QCashForecastLog] <br />
        /// ([NetworkUserID] <br />
        /// ,[LogInDateTime] <br />
        /// ,[Tkinit]) <br />
        /// VALUES <br />
        /// ('" + networkUserID + "' <br />
        /// , '" + DateTime.Now + "' <br />
        /// , '" + Request["tkid"] + "'<br />
        /// </summary>
        /// <param name="networkUserID"></param>
        protected void CreateLogEntry(String networkUserID)
        {
            if ((Session["LogCreated"] == null) || !((bool)(Session["LogCreated"])))
            {
                SqlConnection con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["WPReportsConnectionString"].ConnectionString);

                String sql = " INSERT INTO [WPReports].[dbo].[BMc4QCashForecastLog] " +
                             " ([NetworkUserID] " +
                             " ,[LogInDateTime] " +
                             " ,[Tkinit]) " +
                             " VALUES " +
                             " ('" + networkUserID + "' " +
                             " , '" + DateTime.Now + "' " +
                             " , '" + Request["tkid"] + "' )";
                try
                {
                    con.Open();

                    SqlCommand command = con.CreateCommand();
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                    Session["LogCreated"] = true;

                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(errorLogFileName, sqle.Message, "CreateLogEntry()", sql);
                }
                finally
                {
                    if (con != null)
                        con.Close();
                }
            }

        }


    }

}
