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

namespace BEAR.splitManager
{
    /// <summary>
    /// \class UtilitySplitManager <br />
    ///  a utility class designed to place common functions and global variables specific to the BEAR Split Manager Application
    /// </summary>
    public class UtilitySplitManager
    {
        protected bool[] rowChanged; ///<an array of rows in the Results Grid used to track rows that need to be saved.*/
        private decimal totalAmount = 0; ///<tracks the total in the amount column
        private String userName = ""; ///<network ID of user


        /// <summary>
        /// Sets the global rowChanged variable
        /// </summary>
        public void SetRowChanged(int totalRows)
        {
            this.rowChanged = new bool[totalRows];
        }
        


        /// <summary>
        /// Gets the rowChanged variable
        /// </summary>
        /// <returns>rowChanged</returns>
        public bool[] GetRowChanged()
        {
            return this.rowChanged;
        }



        /// <summary>
        /// totalAmount Getter / Setter
        /// </summary>
        public decimal TotalAmount
        {
            get { return this.totalAmount; }
            set { this.totalAmount = value; }
        }



        /// <summary>
        /// userName Getter / Setter
        /// </summary>
        public String UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }



        /// <summary>
        /// Gets the id of the master matter from the BMcSplitManagerHeader table<br />
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// SELECT id from dbo.BMcSplitManagerHeader (nolock) 
        /// WHERE MasterMatter = '" + bearCode.AddLeadingZeros(MasterMatterNumber, "matter") +"' 
        /// AND StartDate = '" + StartDate + "'
        /// </summary>
        /// <param name="MasterMatterNumber">Master Matter Number</param>
        /// <param name="StartDate">Start Date of the Master Mater</param>
        /// <returns>Master Matter ID</returns>
        public String GetMasterMatterId(String MasterMatterNumber, String StartDate)
        {
            SqlConnection con = null;
            BearCode bearCode = new BearCode();

            String sql = " SELECT id from dbo.BMcSplitManagerHeader (nolock) " +
                             " WHERE MasterMatter = '" + bearCode.AddLeadingZeros(MasterMatterNumber, "matter") +"' " + 
                             " AND StartDate = '" + StartDate + "' ";

            String id = "";

            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    id = reader["id"].ToString();
                }
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "TextBoxMasterMatter_TextChanged()", sql);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            return id;
        }



    }
}
