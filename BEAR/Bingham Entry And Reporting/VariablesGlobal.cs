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

namespace BEAR
{
    /// <summary>
    /// Variables Classes are constants used by BEAR.  These variables could be placed in a database file and given an interface and be made into user defined fields.
    /// </summary>
    public class VariablesGlobal
    {
        public static String SEARCH_NO_RESULTS_FOUND = "No Matching Results Found"; ///<Message to Display when no results found on Matter, TKID or Client Searchs
    }
}
