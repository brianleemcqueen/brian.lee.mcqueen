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

namespace BEAR.search
{
    /// <summary>
    /// Variables Classes are constants used by BEAR.  These variables could be placed in a database file and given an interface and be made into user defined fields.
    /// </summary>
    public class VariablesSearchToykoClientMatter
    {
        public static String ERROR_LOG_FILE_NAME = "\\\\bmelite\\elite\\work\\logs\\BEAR\\ToykoClientMatterSearch.log"; ///<Error log file location

        public static String TOOLTIP_TEXTBOXOFFICES = "Enter office locations numbers separated by a comma or a semi-colon.<br>Or, select the offices from the list on the right."; ///<Tool Tip Text 
        public static String TOOLTIP_RESIZE_COLUMNS = "Click to reset columns to<br>default column widths."; ///<Tool Tip Text 
        public static String TOOLTIP_SEARCH_EXECUTE = "Click here or press Enter to Search"; ///<Tool Tip Text 
    }
}
