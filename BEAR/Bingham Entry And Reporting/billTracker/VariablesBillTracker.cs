using System;

namespace BEAR.billTracker
{
    /// <summary>
    /// Variables Classes are constants used by BEAR.  These variables could be placed in a database file and given an interface and be made into user defined fields.
    /// </summary>
    public class VariablesBillTracker
    {
        public static String FIXED_WIDTH_CHECKBOXES = "50px";  ///<used to set the width of checkbox columns
        public static String FIXED_WIDTH_NOTES = "200px"; ///<used to set the width of Notes columns
        public static String FIXED_WIDTH_EXCEPTION = "50px"; ///<used to set the width of the exception column
        public static String FIXED_WIDTH_EXCEPTIONREASON = "250px"; ///<used to set the width of exception reason column
        public static String FIXED_WIDTH_REVERSALCODE = "50px"; ///<used to set the width of the reversal code column
        public static String FIXED_WIDTH_CURRENCY = "60px"; ///<used to set the width of Currency columns
        public static String FIXED_WIDTH_EXPANDBUTTON = "20px"; ///<used to set the width of Expand Control column


        public static String ERROR_LOG_FILE_NAME = "\\\\bmelite\\elite\\work\\logs\\BEAR\\BillTracker.log"; ///<Error log file location

        public static String ERROR_MESSAGE_PARAMETERS_NOT_COMPLETE = "Selecting All records will cause the generation of the application to time-out.<br />Please enter at least one filter before submitting.";  ///<Message shown when not enough filters have been entered on Exception Rate Parameter Screen

        public static bool CHECK_FOR_ALL_ON_SUBMIT = false; ///<variable to determine if validation is run before submitting.

        public static String HOME_PAGE = "Default.aspx"; ///<Application's Home Page
        public static String DATA_PAGE = "billTracker.aspx"; ///<Application's page called after parameters have been entered
        public static String HOME_PAGE_DIRECTORY = "billTracker/"; ///<BEAR Directory of Application
        public static String USER_PREFERENCES_PAGE = "UserPreferences.aspx"; ///<page for User Preferences

        public static String REPORT_SERVICES_PRINT_NAME = "BillTracker"; ///<Reporting Services Report that is called when the Print Button is clicked
        public static String REPORT_FOLDER = "/Financial%20Accounting/Bear/Bill%20Tracker"; ///<Reporting Services Folder

        public static String DATA_DIV_CUSTOM_STYLE = "height: 500px";  ///<Sets the height of the data results grid.  asp:GridView = dataGridView
        public static int DATA_PAGE_SIZE = 75; ///<number of rows on a page

        public static String TOOLTIP_SUBMIT_BUTTON = "View Bill Tracker Application with the above parameters<br /><br />" +
                                                    "<em>Pressing the Enter Key <br>has same effect as clicking this Submit button<em>"; ///<Tool Tip Text 
        public static String TOOLTIP_SAVE_BUTTON = "Save Changes<br><br><em>Pressing the Enter Key <br>has same effect as clicking this Update button<em>"; ///<Tool Tip Text 
        public static String TOOLTIP_RERUN_BUTTON = "Click this button to <br>change the Application Parameters"; ///<Tool Tip Text 
        public static String TOOLTIP_EXIT_BUTTON = "Exit this application<br> and close the browser"; ///<Tool Tip Text 
        public static String TOOLTIP_SEARCH_TKID_BUTTON = "Click to Search Billing Attorneys"; ///<Tool Tip Text 

        /// <summary>
        /// COLUMN_NAME variables are used to designate the column order of the output.  <br />
        /// These need to be in the same order as the columns are defined in the gridView.  <br />
        /// This allows easier access to finding a column in the code behind.  For Example:  <br />
        /// as opposed to entering this command: <code>e.Row.Cells[0].Attributes.Add("class", "hidden");</code><br />
        /// this command may be entered: <code>e.Row.Cells[VariablesBillTracker.COLUMN_DRAFT_SENT].Attributes.Add("class", "hidden");</code><br />
        /// If the columns are reordered, the only code that need to change is the int that is assigned to the variable.<br />
        /// 
        /// In BillTracker, Columns 2-14 are user defined as to the position they are in.
        /// </summary>
        public static int COLUMN_DRAFT_SENT = 0;
        public static int COLUMN_READY_TO_BILL = 1;
        //columns 2-14 are configurable
        public static int COLUMN_REVERSAL_CODE = 15;
        public static int COLUMN_NOTES = 16;
        public static int COLUMN_EXPAND_BUTTON_NOTES = 17;
        public static int COLUMN_EXCEPTION = 18;
        public static int COLUMN_EXCEPTION_REASON = 19;
        public static int COLUMN_EXPAND_BUTTON_REASON = 20;
        public static int COLUMN_EXCEPTION_REASON_READ_ONLY = 21;
        public static int COLUMN_LOCAL_CURRENCY = 22;
        public static int COLUMN_RATE_CODE = 23;
        public static int COLUMN_PRACTICE_AREA = 24;
        public static int COLUMN_INVOICING_ATTORNEY = 25;
        public static int COLUMN_PROFORMA_ATTORNEY_TKID = 26;
        public static int COLUMN_ARRANGEMENT_CODE = 27;
        public static int COLUMN_ID = 28;

        /// <summary>
        /// COLUMN_PLACEHOLDER_# variables are used to hold the dynamic columns.  They are not given a specific name because they will be different by user.
        /// </summary>
        public static int COLUMN_PLACEHOLDER_1 = 2;
        public static int COLUMN_PLACEHOLDER_2 = 3;
        public static int COLUMN_PLACEHOLDER_3 = 4;
        public static int COLUMN_PLACEHOLDER_4 = 5;
        public static int COLUMN_PLACEHOLDER_5 = 6;
        public static int COLUMN_PLACEHOLDER_6 = 7;
        public static int COLUMN_PLACEHOLDER_7 = 8;
        public static int COLUMN_PLACEHOLDER_8 = 9;
        public static int COLUMN_PLACEHOLDER_9 = 10;
        public static int COLUMN_PLACEHOLDER_10 = 11;
        public static int COLUMN_PLACEHOLDER_11 = 12;
        public static int COLUMN_PLACEHOLDER_12 = 13;
        public static int COLUMN_PLACEHOLDER_13 = 14;


        public static int NUMBER_OF_DYNAMIC_COLUMNS = 13; ///<Constant used to determine the number of user defined column positions

        public static int NUMBER_ROWS_ON_FINAL_PAGE_TO_KEEP_FIXED_PAGERROW = 14; ///< if there are less rows on the final page, the pager row will be below the last row of the page and not use the css expression to keep the pager row fixed.


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
