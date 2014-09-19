using System;

namespace BEAR
{
    /// <summary>
    /// Variables Classes are constants used by BEAR.  These variables could be placed in a database file and given an interface and be made into user defined fields.
    /// </summary>
    public class Variables4QCashForecast
    {

        public static int CLIENT_NAME_MINIMUM_WIDTH = 190; ///<sets the minimum width of the client name column
        public static String ERROR_LOG_FILE_NAME = "\\\\bmelite\\elite\\work\\logs\\BEAR\\4thQAttorneyCashForecastErrorLog.log"; ///<Error log file location

        //Tool Tip Text
        public static String TOOLTIP_SUBMIT_BUTTON = "View Data for the Time Keeper ID entered.<br><br><em>Pressing the Enter Key <br>has same effect as clicking this Submit button<em>"; ///<Tool Tip Text 
        public static String TOOLTIP_SAVE_BUTTON = "Update Variance Column<br>Update Totals<br><br><em>Pressing the Enter Key <br>has same effect as clicking this Update button<em>"; ///<Tool Tip Text 
        public static String TOOLTIP_PRINT_BUTTON = "Printable Version of this Page<br><br><em>Matter Details will not be shown on the Report<br><br>After the Report displays,<br>clicking on the Orange Report Title will return to this application.</em>"; ///<Tool Tip Text 
        public static String TOOLTIP_REPORT_BUTTON = "Run a Report Showing Totals<br>for Each Billing Attorney<br>in a Practice Area<br><br>Select a Practice Area from a Drop Down.<br>Then Click the View Report button<br><br><em>After the Report displays,<br>clicking on the Orange Report Title will return to this application.</em>"; ///<Tool Tip Text 
        public static String TOOLTIP_TKID_BUTTON = "Click this button to <br>change the Time Keeper ID"; ///<Tool Tip Text 
        public static String TOOLTIP_EXIT_BUTTON = "Exit this application<br> and close the browser"; ///<Tool Tip Text 
        public static String TOOLTIP_DETAILS = "Matter Details"; ///<Tool Tip Text 

        public static String HOME_PAGE = "attorneyforecast.aspx"; ///<Application's Home Page
        public static String HOME_PAGE_DIRECTORY = "4qcashforecast/"; ///<BEAR Directory of Application
        public static String ERROR_PAGE_TKID_NOT_FOUND = "../errorPages/invalid_tkid.htm"; ///<page to display if an invalid TKID is entered in the URL

        public static String REPORT_SERVICES_REPORT_NAME = "AttnyCashForecastByPracticeArea"; ///<Reporting Services Report that is called when the Report Button is clicked
        public static String REPORT_SERVICES_PRINT_NAME = "AttnyCashForecast"; ///<Reporting Services Report that is called when the Print Button is clicked
        public static String REPORT_FOLDER = "/Financial%20Accounting/Bear/AttnyCashForecast"; ///<Reporting Services Folder

        private string variableTkid = "";
        public string tkid
        {
            get
            {
                return variableTkid;
            }
            set
            {
                variableTkid = value;
            }
        }




    }
}
