using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace BEAR.exceptionRates.admin
{
    public partial class admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LaunchEmail(object sender, EventArgs e)
        {
            if (ListBoxEmailBody.SelectedValue.ToString().Equals("Blank"))
            {
                Response.Redirect(VariablesExceptionRates.EMAIL_PAGE);
            }
            else
            {
                Response.Redirect(VariablesExceptionRates.EMAIL_PAGE
                    + "?body=" + ListBoxEmailBody.SelectedValue.ToString());
            }
        }
    }
}
