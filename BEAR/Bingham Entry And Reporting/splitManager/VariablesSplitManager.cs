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

namespace BEAR.splitManager
{
    /// <summary>
    /// Variables Classes are constants used by BEAR.  These variables could be placed in a database file and given an interface and be made into user defined fields.
    /// </summary>
    public class VariablesSplitManager
    {
        public static String ERROR_LOG_FILE_NAME = "\\\\bmelite\\elite\\work\\logs\\BEAR\\SplitManager.log"; ///<Error log file location
        public static String DATA_DIV_CUSTOM_STYLE = "height: 490px;"; ///<Sets the height of the data results grid.  asp:GridView = dataGridView
        public static String HOME_PAGE = "Default.aspx"; ///<Application's Home Page
        public static String COPY_PAGE = "Copy.aspx"; ///<Page name used to copy a split to a new date
    }
}
