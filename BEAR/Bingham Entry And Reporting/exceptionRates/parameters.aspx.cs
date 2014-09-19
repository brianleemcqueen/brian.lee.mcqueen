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
using System.Collections.Generic;

namespace BEAR.exceptionRates
{
    public partial class parameters : System.Web.UI.Page
    {

        protected String errorLogFileName = VariablesExceptionRates.ERROR_LOG_FILE_NAME;
        protected BearCode bearCode = new BearCode();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;

            if (!Page.IsPostBack)
            {
                //populate the Billing Specialist and Office Control on the parameter page
                //accomplished with code behind to add database entries after the hardcoded "All" item
                bearCode.PopulateListBillingSpecialists(dropDownListBillingSpecialist, errorLogFileName);
                bearCode.PopulateListLocations(listboxBillingTimekeeperOffice, "timekeeper", errorLogFileName);
            }

            //resetCookie session variable is used to reset the scroll position
            Session["resetCookie"] = "true";


            /*
             *add onFocus to textbox controls on the parameter page.  The commands are in the aspx file. 
             */
            textboxBillingTkid.Attributes.Add("onFocus", "TextBoxOnFocusAllToBlank('textboxBillingTkid')");
            textboxClient.Attributes.Add("onFocus", "TextBoxOnFocusAllToBlank('textboxClient')");
            RadioButtonListSearchResultsTKID.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsTKID', 'textboxBillingTkid')");
            RadioButtonListSearchResultsClient.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsClient', 'textboxClient')");


            //Set Tool Tips
            buttonSubmitTkid.Attributes.Add("onmouseover", "Tip('" + VariablesExceptionRates.TOOLTIP_SUBMIT_BUTTON + "')");
            buttonSubmitTkid.Attributes.Add("onmouseout", "UnTip()");
            buttonReportOnly.Attributes.Add("onmouseover", "Tip('" + VariablesExceptionRates.TOOLTIP_PRINT_BUTTON + "')");
            buttonReportOnly.Attributes.Add("onmouseout", "UnTip()");
            ImageButtonClient.Attributes.Add("onmouseover", "Tip('" + VariablesExceptionRates.TOOLTIP_SEARCH_CLIENT_BUTTON + "')");
            ImageButtonClient.Attributes.Add("onmouseout", "UnTip()");
            ImageButtonTKID.Attributes.Add("onmouseover", "Tip('" + VariablesExceptionRates.TOOLTIP_SEARCH_TKID_BUTTON + "')");
            ImageButtonTKID.Attributes.Add("onmouseout", "UnTip()");


        }


        /*
         * The TKID and client searchs show where the picture is.  When the search button is clicked, 
         * the picture hides and the searchTKID / searchClient toggles betweek visible and invisible.  
         */
        protected void imageButtonTKIDClick(object sender, EventArgs e)
        {
            picture.Visible = false;
            PanelSearchClient.Visible = false;
            PanelSearchTKID.Visible = true;
        }


        /*
         * The TKID and client searchs show where the picture is.  When the search button is clicked, 
         * the picture hides and the searchTKID / searchClient toggles betweek visible and invisible.  
         */
        protected void imageButtonClientClick(object sender, EventArgs e)
        {
            picture.Visible = false;
            PanelSearchTKID.Visible = false;
            PanelSearchClient.Visible = true;
        }

        
        /*
         * Sets the visibility of the TKID search results (after a search is requested)
         */
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


        /*
         * Sets the visibility of the TKID search results (after a search is requested)
         */
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


        /*
         * If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
         */
        protected void SqlDataSource_TKID_EscapeSingleQuote(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchvar", textBoxSearchTKID.Text.Replace("'","''")));
            e.Command.Parameters.Add(new SqlParameter("@tkeflag", "Y"));
        }


        /*
         * If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
         */
        protected void SqlDataSource_Client_EscapeSingleQuote(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchvar", textBoxSearchClient.Text.Replace("'", "''")));
        }

        
        /*
         * redirects to the Data Page with parameters in the URL
         * URLs are emailed to the partners / admins so they can open their report
         * without going through the parameter screen.  
         * This is why parameters are passed through the URL
         */
        protected void buttonLoginSubmitClick(object sender, EventArgs e)
        {
    
            if (validateBeforeSubmit(true))
            {
                //redirect to data page
                Response.Redirect(VariablesExceptionRates.DATA_PAGE
                    + "?billtk=" + textboxBillingTkid.Text
                    + "&billspec=" + dropDownListBillingSpecialist.SelectedValue.ToString()
                    + "&client=" + textboxClient.Text
                    + "&billtkofc=" + listboxBillingTimekeeperOffice.SelectedValue.ToString()
                    + "&tcb=" + TCB.SelectedValue.ToString()
                    + "&cmb=" + CMB.SelectedValue.ToString()
                    + "&year=" + listboxCalendarYear.SelectedValue.ToString()
                    + "&attorneyReview=" + ListBoxAttorneyReviewed.SelectedValue.ToString()
                    + "&billingReview=" + ListBoxBillingReviewed.SelectedValue.ToString()
                    + "&finalized=" + ListBoxFinalized.SelectedValue.ToString()
                    );
            }

        }


        /*
         * Using the parameters page, the Reporting Services report can be called
         */
        protected void buttonReportOnlyClick(object sender, EventArgs e)
        {
            if (validateBeforeSubmit(false))
            {
                Response.Redirect(
                    System.Configuration.ConfigurationSettings.AppSettings["ReportServer"]
                    + VariablesExceptionRates.REPORT_FOLDER
                    + "/"
                    + VariablesExceptionRates.REPORT_SERVICES_PRINT_NAME
                    + "&rc:Parameters=false&rs:Command=Render&rs:ClearSession=true"
                    + "&ApplicationServer="
                    + Environment.MachineName.ToString()
                    + "&billingtimekeeper=" + textboxBillingTkid.Text
                    + "&billingspecialist=" + dropDownListBillingSpecialist.SelectedValue.ToString() 
                    + "&client=" + textboxClient.Text
                    + "&billingtimekeeperoffice=" + listboxBillingTimekeeperOffice.SelectedValue.ToString() 
                    + "&tcb=" + TCB.SelectedValue.ToString() 
                    + "&cmb=" + CMB.SelectedValue.ToString() 
                    + "&year=" + listboxCalendarYear.SelectedValue.ToString()
                    + "&billingReview=" + ListBoxBillingReviewed.SelectedValue.ToString()
                    + "&attorneyReview=" + ListBoxAttorneyReviewed.SelectedValue.ToString()
                     );
 
            }

        }


        /*
         * Helps Ensure the parameters page is a valid request.
         */
        protected bool validateBeforeSubmit(bool checkForAll)
        {
            /*
             * The dataset is too large to run without any filtering.  Therefore, the user is prevented from selecting
             * All Billing TKIDs AND All Clients AND All Billing Specialists
             * However, with the report, this is not an issue
            */
            if (
                    checkForAll
                    &&
                    (textboxBillingTkid.Text.ToUpper().Equals("ALL") || textboxBillingTkid.Text.ToUpper().Equals(""))
                    &&
                    (textboxClient.Text.ToUpper().Equals("ALL") || textboxClient.Text.ToUpper().Equals(""))
                    &&
                    dropDownListBillingSpecialist.SelectedValue.Equals("All")
                    &&
                    listboxBillingTimekeeperOffice.SelectedValue.Equals("All")
                   
                )
            {
                LabelServerMessage.Text = VariablesExceptionRates.ERROR_MESSAGE_PARAMETERS_NOT_COMPLETE;
                PanelSearchTKID.Visible = false;
                PanelSearchClient.Visible = false;
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

                return true;
            }
        }

    }
}
