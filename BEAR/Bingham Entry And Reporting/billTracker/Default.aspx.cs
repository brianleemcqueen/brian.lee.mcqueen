using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BEAR.billTracker
{
    public partial class Default : System.Web.UI.Page
    {
        protected String errorLogFileName = VariablesBillTracker.ERROR_LOG_FILE_NAME;
        protected BearCode bearCode = new BearCode();

        /// <summary>
        /// Ran each time the page is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButtonUserPreferences.Visible = true;

            Page.MaintainScrollPositionOnPostBack = true;

            ///if this is not a postback operation, <br />
            ///populate the Billing Specialist and Office Control on the parameter page
            ///accomplished with code behind to add database entries after the hardcoded "All" item
            if (!Page.IsPostBack)
            {
                
                bearCode.PopulateListBillingSpecialists(dropDownListBillingSpecialist, errorLogFileName);
                bearCode.PopulateListLocations(listboxInvoicingAttorneyOffice, "timekeeper", errorLogFileName);
                PopulateListArrangementCodes();
                PopulateListBillingPeriods();
                PopulateListPracticeAreas();
            }

            CheckBoxThreshold.Attributes.Add("onClick", "ToggleThreshold()");

            ///resetCookie session variable is used to reset the scroll position
            Session["resetCookie"] = "true";


            /**
             *add onFocus to textbox controls on the parameter page.  The commands are in the aspx file. 
             */
            textboxBillingTkid.Attributes.Add("onFocus", "TextBoxOnFocusAllToBlank('textboxBillingTkid')");
            textboxInvoiceAtty.Attributes.Add("onFocus", "TextBoxOnFocusAllToBlank('textboxInvoiceAtty')");
            textboxClient.Attributes.Add("onFocus", "TextBoxOnFocusAllToBlank('textboxClient')");
            textboxMatter.Attributes.Add("onFocus", "TextBoxOnFocusAllToBlank('textboxMatter')");
            
            ///Set Tool Tips
            buttonSubmitTkid.Attributes.Add("onmouseover", "Tip('" + VariablesBillTracker.TOOLTIP_SUBMIT_BUTTON + "')");
            buttonSubmitTkid.Attributes.Add("onmouseout", "UnTip()");
            ImageButtonTKID.Attributes.Add("onmouseover", "Tip('" + VariablesBillTracker.TOOLTIP_SEARCH_TKID_BUTTON + "')");
            ImageButtonTKID.Attributes.Add("onmouseout", "UnTip()");


        }


        /// <summary>
        /// Opens page to allow Users to set their own column order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButtonUserPreferences_Click(object sender, EventArgs e)
        {
            Response.Redirect(VariablesBillTracker.USER_PREFERENCES_PAGE);
        }


        /// <summary>
        /// Sets the visibility of the Client search results (after a search is requested)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imageButtonSearchClientClick(object sender, EventArgs e)
        {
            labelSearchResultsClient.Text = "Select One:";
            RadioButtonListSearchResultsClient.DataBind();
            RadioButtonListSearchResultsClient.Visible = true;
            if (RadioButtonListSearchResultsClient.Items.Count.ToString().Equals("0"))
            {
                labelSearchResultsClient.Text = VariablesGlobal.SEARCH_NO_RESULTS_FOUND;
            }
        }


        /// <summary>
        /// Sets the visibility of the Matter search results (after a search is requested)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imageButtonSearchMatterClick(object sender, EventArgs e)
        {
            labelSearchResultsClient.Text = "Select One:";
            RadioButtonListSearchResultsMatter.DataBind();
            RadioButtonListSearchResultsMatter.Visible = true;
            if (RadioButtonListSearchResultsMatter.Items.Count.ToString().Equals("0"))
            {
                labelSearchResultsMatter.Text = VariablesGlobal.SEARCH_NO_RESULTS_FOUND;
            }
        }


        /// <summary>
        /// Sets the visibility of the TKID search results (after a search is requested)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imageButtonSearchTKIDClick(object sender, EventArgs e)
        {

            labelSearchResultsTKID.Text = "Select One:";
            RadioButtonListSearchResultsTKID.DataBind();
            RadioButtonListSearchResultsTKID.Visible = true;
            if (RadioButtonListSearchResultsTKID.Items.Count.ToString().Equals("0"))
            {
                labelSearchResultsTKID.Text = VariablesGlobal.SEARCH_NO_RESULTS_FOUND;

            }
        }


        /// <summary>
        /// Closes all Search Panels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButtonCloseSearchClick(object sender, EventArgs e)
        {
            picture.Visible = true;
            PanelSearchTKID.Visible = false;
            PanelSearchClient.Visible = false;
            PanelSearchMatter.Visible = false;
        }


        /// <summary>
        /// The searchs display where the picture is.  When the search button is clicked, 
        /// the picture hides and the searchTKID toggles betweek visible and invisible.  
        /// Based on which TKID search is selected, the onClick event is changed to set the appropriate
        /// textbox to populate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButtonTKIDInvoiceAttyClick(object sender, EventArgs e)
        {
            picture.Visible = false;
            PanelSearchTKID.Visible = true;
            PanelSearchClient.Visible = false;
            PanelSearchMatter.Visible = false;
            RadioButtonListSearchResultsTKID.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsTKID', 'textboxInvoiceAtty')");
        }


        /// <summary>
        /// The searchs display where the picture is.  When the search button is clicked, 
        /// the picture hides and the searchTKID toggles betweek visible and invisible.  
        /// Based on which TKID search is selected, the onClick event is changed to set the appropriate
        /// textbox to populate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imageButtonTKIDClick(object sender, EventArgs e)
        {
            picture.Visible = false;
            PanelSearchTKID.Visible = true;
            PanelSearchClient.Visible = false;
            PanelSearchMatter.Visible = false;
            RadioButtonListSearchResultsTKID.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsTKID', 'textboxBillingTkid')");
        }


        /// <summary>
        /// The TKID and client searchs show where the picture is.  When the search button is clicked, 
        /// the picture hides and the searchTKID / searchClient toggles betweek visible and invisible.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imageButtonClientClick(object sender, EventArgs e)
        {
            picture.Visible = false;
            PanelSearchTKID.Visible = false;
            PanelSearchClient.Visible = true;
            PanelSearchMatter.Visible = false;
            RadioButtonListSearchResultsClient.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsClient', 'textboxClient')");
        }


        /// <summary>
        /// The searchs show where the picture is.  When the search button is clicked, 
        /// the picture hides and the searchs toggles betweek visible and invisible.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imageButtonMatterClick(object sender, EventArgs e)
        {
            picture.Visible = false;
            PanelSearchTKID.Visible = false;
            PanelSearchClient.Visible = false;
            PanelSearchMatter.Visible = true;
            RadioButtonListSearchResultsMatter.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsMatter', 'textboxMatter')");
        }


        /// <summary>
        /// If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SqlDataSource_Client_EscapeSingleQuote(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchvar", textBoxSearchClient.Text.Replace("'", "''")));
        }


        /// <summary>
        /// If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SqlDataSource_Matter_EscapeSingleQuote(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchvar", textBoxSearchMatter.Text.Replace("'", "''")));
        }


        /// <summary>
        /// If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SqlDataSource_TKID_EscapeSingleQuote(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchvar", textBoxSearchTKID.Text.Replace("'", "''")));
            e.Command.Parameters.Add(new SqlParameter("@tkeflag", "Y"));
        }


        /// <summary>
        /// redirects to the Data Page with parameters in the URL<br />
        /// URLs are emailed to the partners / admins so they can open their report<br />
        /// without going through the parameter screen.  <br />
        /// This is why parameters are passed through the URL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonLoginSubmitClick(object sender, EventArgs e)
        {

            if (validateBeforeSubmit(VariablesBillTracker.CHECK_FOR_ALL_ON_SUBMIT))
            {
                //redirect to data page
                Response.Redirect(VariablesBillTracker.DATA_PAGE
                    + "?billtk=" + textboxBillingTkid.Text
                    + "&invoiceatty=" + textboxInvoiceAtty.Text
                    + "&billspec=" + dropDownListBillingSpecialist.SelectedValue.ToString()
                    + "&arrangement=" + DropDownListArrangementCode.SelectedValue.ToString()
                    + "&billpd=" + listboxBillingPeriod.SelectedValue.ToString()
                    + "&threshold=" + CheckBoxThreshold.Checked.ToString()
                    + "&rtb=" + ListBoxReadyToBillFilter.SelectedValue.ToString()
                    + "&clnum=" + textboxClient.Text
                    + "&matter=" + textboxMatter.Text
                    + "&iaofc=" + listboxInvoicingAttorneyOffice.SelectedValue.ToString()
                    + "&ofcType=" +RadioButtonListAttorneyOrBillingSpecialist.SelectedValue.ToString()
                    + "&pa=" + DropDownListPracticeArea.SelectedValue.ToString()
                    + "&exception=" + ListBoxException.SelectedValue.ToString()
                    );
            }

        }


        /// <summary>
        /// Using the parameters page, the Reporting Services report can be called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonReportOnlyClick(object sender, EventArgs e)
        {
            if (validateBeforeSubmit(false))
            {
                Response.Redirect(
                    System.Configuration.ConfigurationSettings.AppSettings["ReportServer"]
                    + VariablesBillTracker.REPORT_FOLDER
                    + "/"
                    + VariablesBillTracker.REPORT_SERVICES_PRINT_NAME
                    + "&rc:Parameters=false&rs:Command=Render&rs:ClearSession=true"
                    + "&ApplicationServer="
                    + Environment.MachineName.ToString()
                    + "&billingAttorney=" + textboxBillingTkid.Text
                    + "&invoiceAttorney=" + textboxInvoiceAtty.Text
                    + "&billingSpecialist=" + dropDownListBillingSpecialist.SelectedValue.ToString()
                    + "&arrangementCode=" + DropDownListArrangementCode.SelectedValue.ToString()
                    + "&billingPeriod=" + listboxBillingPeriod.SelectedValue.ToString()
                    + "&threshold=" + CheckBoxThreshold.Checked.ToString()
                    + "&rtb=" + ListBoxReadyToBillFilter.SelectedValue.ToString()
                    + "&clnum=" + textboxClient.Text
                    + "&matter=" + textboxMatter.Text
                    + "&iaofc=" + listboxInvoicingAttorneyOffice.SelectedValue.ToString()
                    + "&ofcType=" + RadioButtonListAttorneyOrBillingSpecialist.SelectedValue.ToString()
                    + "&pa=" + DropDownListPracticeArea.SelectedValue.ToString()
                    + "&exception=" + ListBoxException.SelectedValue.ToString()
                     );

            }

        }


        /// <summary>
        /// Validates the parameters page before sending the request
        /// </summary>
        /// <param name="checkForAll">Pass in False to bypass this validation</param>
        /// <returns></returns>
        protected bool validateBeforeSubmit(bool checkForAll)
        {
            /**
             * The dataset is too large to run without any filtering.  Therefore, the user is prevented from selecting
             * All Billing TKIDs AND All Clients AND All Billing Specialists
             * However, with the report, this is not an issue
            */
            if (
                    checkForAll
                    &&
                    (textboxBillingTkid.Text.ToUpper().Equals("ALL") || textboxBillingTkid.Text.ToUpper().Equals(""))
                    &&
                    (textboxInvoiceAtty.Text.ToUpper().Equals("ALL") || textboxInvoiceAtty.Text.ToUpper().Equals(""))
                    &&
                    (textboxClient.Text.ToUpper().Equals("ALL") || textboxClient.Text.ToUpper().Equals(""))
                    &&
                    (textboxMatter.Text.ToUpper().Equals("ALL") || textboxMatter.Text.ToUpper().Equals(""))
                    &&
                    dropDownListBillingSpecialist.SelectedValue.Equals("All")
                    &&
                    DropDownListArrangementCode.SelectedValue.Equals("All")
                    &&
                    listboxInvoicingAttorneyOffice.SelectedValue.Equals("All")

                )
            {
                LabelServerMessage.Text = VariablesBillTracker.ERROR_MESSAGE_PARAMETERS_NOT_COMPLETE;
                PanelSearchTKID.Visible = false;
                picture.Visible = true;
                LabelServerMessage.Visible = true;
                return false;
            }
            else
            {
                //allowing for not adding the leading zeros on Billing TKID
                if (textboxBillingTkid.Text.ToUpper() != "ALL")
                {
                    if (textboxBillingTkid.Text.Length == 3)
                    {
                        textboxBillingTkid.Text = "00" + textboxBillingTkid.Text;
                    }
                    else if (textboxBillingTkid.Text.Length == 4)
                    {
                        textboxBillingTkid.Text = "0" + textboxBillingTkid.Text;
                    }
                    else if (textboxBillingTkid.Text.Equals(""))
                    {
                        textboxBillingTkid.Text = "All";
                    }
                }
                else
                {
                    //ensures the correct case for All - Capital A, lower ll
                    textboxBillingTkid.Text = "All";
                }


                if (textboxInvoiceAtty.Text.ToUpper() != "ALL")
                {
                    if (textboxInvoiceAtty.Text.Length == 3)
                    {
                        textboxInvoiceAtty.Text = "00" + textboxInvoiceAtty.Text;
                    }
                    else if (textboxInvoiceAtty.Text.Length == 4)
                    {
                        textboxInvoiceAtty.Text = "0" + textboxInvoiceAtty.Text;
                    }
                    else if (textboxInvoiceAtty.Text.Equals(""))
                    {
                        textboxInvoiceAtty.Text = "All";
                    }
                }
                else
                {
                    //ensures the correct case for All - Capital A, lower ll
                    textboxInvoiceAtty.Text = "All";
                }

                if (textboxClient.Text.ToUpper() != "ALL")
                {
                    //Allowing for not adding the leading zeros on Client
                    if (textboxClient.Text.Length == 3)
                    {
                        textboxClient.Text = "0000" + textboxClient.Text;
                    }
                    else if (textboxClient.Text.Length == 4)
                    {
                        textboxClient.Text = "000" + textboxClient.Text;
                    }
                    else if (textboxClient.Text.Length == 5)
                    {
                        textboxClient.Text = "00" + textboxClient.Text;
                    }
                    else if (textboxClient.Text.Length == 6)
                    {
                        textboxClient.Text = "0" + textboxClient.Text;
                    }
                    else if (textboxClient.Text.Equals(""))
                    {
                        textboxClient.Text = "All";
                    }
                }
                else
                {
                    //ensures the correct case for All - Capital A, lower ll
                    textboxClient.Text = "All";
                }



                if (textboxMatter.Text.ToUpper() != "ALL")
                {
                    //Allowing for not adding the leading zeros on Client
                    if (textboxMatter.Text.Length == 1)
                    {
                        textboxMatter.Text = "000000000" + textboxMatter.Text;
                    }
                    else if (textboxMatter.Text.Length == 2)
                    {
                        textboxMatter.Text = "00000000" + textboxMatter.Text;
                    }
                    else if (textboxMatter.Text.Length == 3)
                    {
                        textboxMatter.Text = "0000000" + textboxMatter.Text;
                    }
                    else if (textboxMatter.Text.Length == 4)
                    {
                        textboxMatter.Text = "000000" + textboxMatter.Text;
                    }
                    else if (textboxMatter.Text.Length == 5)
                    {
                        textboxMatter.Text = "00000" + textboxMatter.Text;
                    }
                    else if (textboxMatter.Text.Length == 6)
                    {
                        textboxMatter.Text = "0000" + textboxMatter.Text;
                    }
                    else if (textboxMatter.Text.Length == 7)
                    {
                        textboxMatter.Text = "000" + textboxMatter.Text;
                    }
                    else if (textboxMatter.Text.Length == 8)
                    {
                        textboxMatter.Text = "00" + textboxMatter.Text;
                    }
                    else if (textboxMatter.Text.Length == 9)
                    {
                        textboxMatter.Text = "0" + textboxMatter.Text;
                    }
                    else if (textboxMatter.Text.Equals(""))
                    {
                        textboxMatter.Text = "All";
                    }
                }
                else
                {
                    //ensures the correct case for All - Capital A, lower ll
                    textboxMatter.Text = "All";
                }



                return true;
            }
        }


        /// <summary>
        /// Populates DropDownListArrangementCode by callling GetArrangementCodes()
        /// </summary>
        public void PopulateListArrangementCodes()
        {
            List<ListItem> arrangementCodes = GetArrangementCodes();
            for (int i = 0; i < arrangementCodes.Count; i++)
            {
                DropDownListArrangementCode.Items.Add(arrangementCodes[i]);
            }
        }


        /// <summary>
        /// Gets a listing of Arrangement Codes in Elite<br />
        /// SQL:<br />
        /// uspBMcBEARBillTrackerGetArrangementCodes
        /// </summary>
        /// <returns>a List of ListItems containing Elite Arrangement Codes</returns>
        public List<ListItem> GetArrangementCodes()
        {
            List<ListItem> arrangementCodes = new List<ListItem>();
            ListItem li = new ListItem();

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String arrangementCode = "";
            String arrangementCodeDescription = "";
            try
            {

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARBillTrackerGetArrangementCodes";
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    arrangementCode = reader["arrangement_code"].ToString();
                    arrangementCodeDescription = reader["arrangementCodeDescription"].ToString();

                    li = new ListItem(arrangementCodeDescription, arrangementCode, true);
                    arrangementCodes.Add(li);
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetArrangementCodes()", "");
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetArrangementCodes()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return arrangementCodes;

        }


        /// <summary>
        /// Populates DropDownListPracticeArea by calling GetPracticeAreas()
        /// </summary>
        public void PopulateListPracticeAreas()
        {
            List<ListItem> practiceAreas = GetPracticeAreas();
            for (int i = 0; i < practiceAreas.Count; i++)
            {
                DropDownListPracticeArea.Items.Add(practiceAreas[i]);
            }
        }


        /// <summary>
        /// Gets a listing of Practice Areas from Elite
        /// </summary>
        /// <returns>a List of ListItems containing Elite Practice Areas</returns>
        public List<ListItem> GetPracticeAreas()
        {
            List<ListItem> practiceAreas = new List<ListItem>();
            ListItem li = new ListItem();

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String paDisplay = "";
            String paValue = "";
            try
            {

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARBillTrackerPracArea";
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    paDisplay = reader["paConcat"].ToString();
                    paValue = reader["practice_area"].ToString();

                    li = new ListItem(paDisplay, paValue, true);
                    practiceAreas.Add(li);
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetPracticeAreas()", "");
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetPracticeAreas()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return practiceAreas;

        }


        /// <summary>
        /// Populates listboxBillingPeriod by calling GetBillingPeriods()
        /// </summary>
        public void PopulateListBillingPeriods()
        {
            List<ListItem> billingPeriods = GetBillingPeriods();
            for (int i = 0; i < billingPeriods.Count; i++)
            {
                listboxBillingPeriod.Items.Add(billingPeriods[i]);
                if (i == 0)
                {
                    listboxBillingPeriod.Items[i].Selected = true;
                }
            }
        }
        

        /// <summary>
        /// Gets a listing of Billing Periods for BillTracker to show on the parameters page
        /// </summary>
        /// <returns>a List of ListItems containing Billing Periods</returns>
        public List<ListItem> GetBillingPeriods()
        {
            List<ListItem> billingPeriods = new List<ListItem>();
            ListItem li = new ListItem();

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String billingPeriod = "";
            try
            {

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARBillTrackerGetBillingPeriods";
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    billingPeriod = reader["billingPeriod"].ToString();

                    li = new ListItem(billingPeriod, billingPeriod, true);
                    billingPeriods.Add(li);
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetBillingPeriods()", "");
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetBillingPeriods()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return billingPeriods;

        }
        
    }
}
