using System;
using System.Data;
using System.Data.SqlClient;
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
    public class UtilityCashFlow
    {
        protected String userName = ""; ///<network ID of user
        protected String location = "";
        protected String department = "";
        protected bool isAdminUser = false;
        protected bool isDeptUser = false;
        protected bool isCMUser = false;
        protected decimal amountToPay = 0;

        protected bool[] rowChanged;

        public UtilityCashFlow()
        {
        }

        public UtilityCashFlow(String user)
        {
            this.userName = user;
            SetUpUser();
        }

        /// <summary>
        /// Sets the User's location, access, and user defined amount to pay field <br />
        /// SELECT isnull(amountToPay,'0') as amountToPay, isAdminUser, isDeptUser, isCMUser, location, department " <br />
        /// + " FROM dbo.BMcBEARCashFlowManagerUsers (nolock) " <br />
        /// + " WHERE networkId = '" + this.userName + "' ";
        /// </summary>
        protected void SetUpUser()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            try
            {
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = " SELECT isnull(amountToPay,'0') as amountToPay, isAdminUser, isDeptUser, isCMUser, location, department "
                                                + " FROM dbo.BMcBEARCashFlowManagerUsers (nolock) "
                                                + " WHERE networkId = '" + this.userName + "' ";
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    this.isAdminUser = Convert.ToBoolean(reader["isAdminUser"].ToString());
                    this.isCMUser = Convert.ToBoolean(reader["isCMUser"].ToString());
                    this.isDeptUser = Convert.ToBoolean(reader["isDeptUser"].ToString());
                    this.location = reader["location"].ToString();
                    this.department = reader["department"].ToString();
                    this.amountToPay = Convert.ToDecimal(reader["amountToPay"].ToString());
                }

            }

            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCashManager.ERROR_LOG_FILE_NAME, sqle.Message, "SetUpUser()", "");
            }
            catch (FormatException fe)
            {
                Logger.QuickLog(VariablesCashManager.ERROR_LOG_FILE_NAME, fe.Message, "SetUpUser()", "");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public void SetRowChanged(int totalRows)
        {
            this.rowChanged = new bool[totalRows];
        }
        public bool[] GetRowChanged()
        {
            return this.rowChanged;
        }



        public String GetUserName()
        {
            return this.userName;
        }
        public String GetLocation()
        {
            return this.location;
        }
        public String GetDepartment()
        {
            return this.department;
        }
        public bool GetIsAdminUser()
        {
            return this.isAdminUser;
        }
        public bool GetIsDeptUser()
        {
            return this.isDeptUser;
        }
        public bool GetIsCMUser()
        {
            return this.isCMUser;
        }
        public decimal GetAmountToPay()
        {
            return this.amountToPay;
        }

        public void SetUserName(String u)
        {
            this.userName = u;
        }
        public void SetLocation(String l)
        {
            this.location = l;
        }
        public void SetDepartment(String d)
        {
            this.department = d;
        }
        public void SetIsAdminUser(bool u)
        {
            this.isAdminUser = u;
        }
        public void SetIsDeptUser(bool u)
        {
            this.isDeptUser = u;
        }
        public void SetIsCMUser(bool u)
        {
            this.isCMUser = u;
        }
        public void SetAmountToPay(decimal a)
        {
            this.amountToPay = a;
        }


    }


}
