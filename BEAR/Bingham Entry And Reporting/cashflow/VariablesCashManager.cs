using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace BEAR.cashflow
{
    /// <summary>
    /// Variables Classes are constants used by BEAR.  These variables could be placed in a database file and given an interface and be made into user defined fields.
    /// </summary>
    public class VariablesCashManager
    {
        public static String ERROR_LOG_FILE_NAME = "\\\\bmelite\\elite\\work\\logs\\BEAR\\CashManager.log"; ///<Error log file location

        public static String PARAM_PAGE = "searchCashFlow.aspx"; ///<Search parameter page.  This is the page entered if coming from the BEAR home page
        public static String DATA_PAGE = "cashflow.aspx"; ///<Page called after parameters have been entered
        public static String ADMIN_PAGE = "adminOptions.aspx"; ///<Administration Options Page
        public static String MANUAL_ENTRY_PAGE = "add.aspx"; ///<Page to allow manual entry into the Application
        public static String NO_ACCESS_PAGE = "access.aspx"; ///<Page called when user rights are not granted.  Users are set up in the database.

        public static String REPORT_FOLDER = "/Financial%20Accounting/Bear/CashFlowManager"; ///<Reporting Services Folder
        public static String REPORT_SERVICES_CM_REPORT_NAME = "cashFlowManagerCM"; ///<Reporting Services Report Name for Cash Management (AP) users
        public static String REPORT_SERVICES_DEPT_REPORT_NAME = "cashFlowManagerDept"; ///<Reporting Services Report Name for general users

        public static String DATA_DIV_CUSTOM_STYLE = "height: 490px;"; ///<Sets the height of the data results grid.  asp:GridView = dataGridView

        public static String TOOLTIP_SAVE_BUTTON = "Save Changes<br><br><em>Pressing the Enter Key <br>has same effect as clicking this Update button<em>"; ///<Tool Tip Text
        //public static String TOOLTIP_SUBMIT_BUTTON = "View Cash Flow Manager Application with the above parameters<br /><br /><em>Pressing the Enter Key<br />has same effect as clicking this Submit button<em>"; ///<Tool Tip Text
        //public static String TOOLTIP_RERUN_BUTTON = "Click this button to <br>change the Application Parameters"; ///<Tool Tip Text
        //public static String TOOLTIP_EXIT_BUTTON = "Exit this application<br> and close the browser"; ///<Tool Tip Text

        /// <summary>
        /// COLUMN_X variables are used to designate the column order of the output.  <br />
        /// These need to be in the same order as the columns are defined in the gridView.  <br />
        /// This allows easier access to finding a column in the code behind.  For Example:  <br />
        /// as opposed to entering this command: <code>e.Row.Cells[0].Attributes.Add("class", "hidden");</code><br />
        /// this command may be entered: <code>e.Row.Cells[VariablesCashManager.COLUMN_PAY_FLAG].Attributes.Add("class", "hidden");</code><br />
        /// If the columns are reordered, the only code that need to change is the int that is assigned to the variable.
        /// </summary>
        public static int COLUMN_PAY_FLAG = 0;
        public static int COLUMN_PAYMENT_METHOD = 1;
        public static int COLUMN_PRIORITY_CM = 2;
        public static int COLUMN_PRIORITY_DEPT = 3;
        public static int COLUMN_DEPARTMENT = 4;
        public static int COLUMN_VENDOR_NAME = 5;
        public static int COLUMN_INVOICE_INFORMATION = 6;
        public static int COLUMN_AMOUNT = 7;
        public static int COLUMN_NOTES = 8;
        public static int COLUMN_DESCRIPTION = 9;
        public static int COLUMN_ENTERED_BY = 10;
        public static int COLUMN_SOURCE = 11;
        public static int COLUMN_BARCODE = 12;
        public static int COLUMN_LOCATION = 13;
        public static int COLUMN_OFFICE = 14;

        public static int COLUMN_INVOICE_NUMBER_HIDDEN = 15;
        public static int COLUMN_INVOICE_DATE_HIDDEN = 16;
        public static int COLUMN_VENDOR_ID_HIDDEN = 17;
        public static int COLUMN_VENDOR_NAME_HIDDEN = 18;
        public static int COLUMN_AMOUNT_HIDDEN = 19;
        public static int COLUMN_ID_HIDDEN = 20;
        public static int COLUMN_PAYMENT_METHOD_HIDDEN = 21;
        public static int COLUMN_PRIORITY_CM_HIDDEN = 22;
        public static int COLUMN_PRIORITY_DEPT_HIDDEN = 23;
        public static int COLUMN_CURRENCY_HIDDEN = 24;
        public static int COLUMN_UPDATE_TIME_HIDDEN = 25;
        
    }
}
