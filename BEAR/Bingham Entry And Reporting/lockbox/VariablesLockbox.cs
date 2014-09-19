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

namespace BEAR.lockbox
{
    /// <summary>
    /// Variables Classes are constants used by BEAR.  These variables could be placed in a database file and given an interface and be made into user defined fields.
    /// </summary>
    public class VariablesLockbox
    {
        public static String ERROR_LOG_FILE_NAME = "\\\\bmelite\\elite\\work\\logs\\BEAR\\Lockbox.log"; ///<Error log file location
        public static String EXPORT_PATH = "\\\\bmelite\\elite\\BEAR\\lockbox\\export\\"; ///<Path to place the export file

        public static String EMAIL_FROM_ADDRESS = "\"Exception Rates (Please Do Not Reply To This Message)\"<Lockbox@bingham.com>"; ///<email address to show as sent by
    }
}
