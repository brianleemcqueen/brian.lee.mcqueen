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

namespace BEAR.exceptionRates
{
    public class UtilityExceptionRates
    {
        protected bool[] rowChanged;

        public void SetRowChanged(int totalRows)
        {
            this.rowChanged = new bool[totalRows];
        }
        public bool[] GetRowChanged()
        {
            return this.rowChanged;
        }

    }
}
