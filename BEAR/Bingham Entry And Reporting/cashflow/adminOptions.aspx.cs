using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
    public partial class adminOptions : System.Web.UI.Page
    {
        String userName = ""; ///<network ID of user

        protected void Page_Load(object sender, EventArgs e)
        {
            this.userName = Page.User.Identity.Name.ToString().Substring(8);
            //this.userName = "bergcl";
            UtilityCashFlow utility = new UtilityCashFlow(this.userName);

            if ( !utility.GetIsAdminUser() && !utility.GetIsCMUser() )
            {
                Response.Redirect(VariablesCashManager.NO_ACCESS_PAGE);
            }

            if (!Page.IsPostBack)
            {
                LabelMessage.Text = "";
                GetAmountToPay();
            }

        }

        /// <summary>
        /// Called before the page is rendered.  This is used for the save functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                BearCode bearCode = new BearCode();
                if (bearCode.IsNumber(TextBoxAmountToPay.Text) || TextBoxAmountToPay.Text.Equals(""))
                {
                    String amountToPay = TextBoxAmountToPay.Text;
                    if (amountToPay.Equals(""))
                    {
                        amountToPay = "0";
                    }
                    try
                    {
                        con.Open();
                        SqlCommand command = con.CreateCommand();
                        command.CommandText = "uspBMcBEARCashFlowManagerUpdateAmountToPay";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@amountToPay", amountToPay);
                        command.Parameters.AddWithValue("@networkId", this.userName);
                        command.ExecuteNonQuery();
                        LabelMessage.Text = "Value Updated";
                    }
                    catch (SqlException sqle)
                    {
                        Logger.QuickLog(VariablesCashManager.ERROR_LOG_FILE_NAME, sqle.Message, "adminOptions.aspx: Page_PreRender()", "");
                    }
                    finally
                    {
                        if (con != null)
                        {
                            con.Close();
                        }
                    }
                }
                else
                {
                    LabelMessage.Text = "Please enter a valid amount";
                }
            }
        }

        protected void ButtonFilter_Click(object sender, EventArgs e)
        {
            Response.Redirect(VariablesCashManager.PARAM_PAGE);
        }

        protected void ButtonClose_Click(object sender, EventArgs e)
        {
            Response.Redirect(VariablesCashManager.DATA_PAGE);
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(VariablesCashManager.MANUAL_ENTRY_PAGE);
        }

        protected void GetAmountToPay()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            String amountToPay = "0";
            try
            {
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = "SELECT isnull(amountToPay,'0') as amountToPay FROM BMcBEARCashFlowManagerUsers WHERE networkId = '" + this.userName + "' ";
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    amountToPay = reader["amountToPay"].ToString();
                }

            }

            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCashManager.ERROR_LOG_FILE_NAME, sqle.Message, "GetAmountToPay()", "");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            TextBoxAmountToPay.Text = amountToPay;
        }

    }
}
