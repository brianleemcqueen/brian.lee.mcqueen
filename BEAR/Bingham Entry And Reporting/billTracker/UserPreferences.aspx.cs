using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace BEAR.billTracker
{
    public partial class UserPreferences : System.Web.UI.Page
    {
        protected String user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            user = Page.User.Identity.Name.ToString().Substring(8);
            SetColumnOrder();
            ButtonRestore.Attributes.Add("onclick", "javascript:return confirm('Erase all custom column order settings?')");
            ButtonReset.Attributes.Add("onclick", "javascript:return confirm('Restore column order to the last save point?')");
        }


        /// <summary>
        /// Clicking Parameters Buttons redirects to Home Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonButtonBillTracker_Click(object sender, EventArgs e)
        {
            Response.Redirect(VariablesBillTracker.HOME_PAGE);
        }


        /// <summary>
        /// On Save, the user's preferences are inserted or updated in the BMcBEARBillTrackerUserPreferences table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            String Column1 = TextBox1.Text;
            String Column2 = TextBox2.Text;
            String Column3 = TextBox3.Text;
            String Column4 = TextBox4.Text;
            String Column5 = TextBox5.Text;
            String Column6 = TextBox6.Text;
            String Column7 = TextBox7.Text;
            String Column8 = TextBox8.Text;
            String Column9 = TextBox9.Text;
            String Column10 = TextBox10.Text;
            String Column11 = TextBox11.Text;
            String Column12 = TextBox12.Text;
            String Column13 = TextBox13.Text;

            String sql = "";

            if (UserHasPreferences())
            {
                sql = " UPDATE dbo.BMcBEARBillTrackerUserPreferences "
                     + " SET column1 = " + Column1
                         + " ,column2 = " + Column2
                         + " ,column3 = " + Column3
                         + " ,column4 = " + Column4
                         + " ,column5 = " + Column5
                         + " ,column6 = " + Column6
                         + " ,column7 = " + Column7
                         + " ,column8 = " + Column8
                         + " ,column9 = " + Column9
                         + " ,column10 = " + Column10
                         + " ,column11 = " + Column11
                         + " ,column12 = " + Column12
                         + " ,column13 = " + Column13
                     + " WHERE UserId = '" + user + "' ";
            }
            else
            {
                sql = " INSERT INTO dbo.BMcBEARBillTrackerUserPreferences "
                         + " (UserId, Column1, Column2, Column3, Column4, Column5, Column6, "
                         + " Column7, Column8, Column9, Column10, Column11, Column12, Column13) "
                     + " VALUES ( "
                         + " '" + user + "' "
                         + "," + Column1
                         + "," + Column2
                         + "," + Column3
                         + "," + Column4
                         + "," + Column5
                         + "," + Column6
                         + "," + Column7
                         + "," + Column8
                         + "," + Column9
                         + "," + Column10
                         + "," + Column11
                         + "," + Column12
                         + "," + Column13
                     + " ) ";
            }

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesBillTracker.ERROR_LOG_FILE_NAME, sqle.Message, "ButtonSave_Click()", sql);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            SetColumnOrder();

        }


        /// <summary>
        /// Resetting the columns bypasses the save and restores the column order from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonRestore_Click(object sender, EventArgs e)
        {
            SetColumnOrder();
        }


        /// <summary>
        /// resetting to default order deletes the users preferences from the BMcBEARBillTrackerUserPreferences table.  Without a record in this table, the user gets the default order.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonReset_Click(object sender, EventArgs e)
        {
            String sql = " DELETE FROM dbo.BMcBEARBillTrackerUserPreferences WHERE UserId = '" + user + "' ";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesBillTracker.ERROR_LOG_FILE_NAME, sqle.Message, "ButtonReset_Click()", sql);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            SetColumnOrder();

        }


        /// <summary>
        /// A method to determine if the user has a record in the BMcBEARBillTrackerUserPreferences table.
        /// </summary>
        /// <returns></returns>
        protected bool UserHasPreferences()
        {
            bool userHasPreferences = false;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            String sqlCheckForPreferences =
                " SELECT count(*) AS userHasPreferences" +
                " FROM BMcBEARBillTrackerUserPreferences " +
                " WHERE UserId = '" + user + "' ";
            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sqlCheckForPreferences;
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (Convert.ToInt16(reader["userHasPreferences"].ToString()) == 0)
                    {
                        userHasPreferences = false;
                    }
                    else
                    {
                        userHasPreferences = true;
                    }
                }
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesBillTracker.ERROR_LOG_FILE_NAME, sqle.Message, "SetColumnOrder()", sqlCheckForPreferences);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return userHasPreferences;
        }

        
        /// <summary>
        /// This sets the column order when the page is loaded.  Either the default or the user defined based on the result from UserHasPreferences()
        /// </summary>
        protected void SetColumnOrder()
        {
            if (UserHasPreferences())
            {
                SetCustomColumnOrder();
            }
            else
            {
                SetDefaultColumnOrder();
            }

        }


        /// <summary>
        /// SELECT id, replace(Heading, '<br />', ' ') as Heading FROM BMcBEARBillTrackerDynamicColumns ORDER BY ColumnNumber
        /// </summary>
        protected void SetDefaultColumnOrder()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String sql =
                " SELECT id, replace(Heading, '<br />', ' ') as Heading FROM BMcBEARBillTrackerDynamicColumns ORDER BY ColumnNumber ";
            StringBuilder sb = new StringBuilder("");
            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    sb.Append("<li id=\"" + reader["id"].ToString() + "\" class=\"ui-state-default\">" + reader["Heading"].ToString() + " </li>");
                }
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesBillTracker.ERROR_LOG_FILE_NAME, sqle.Message, "SetDefaultColumnOrder()", sql);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            sortable.InnerHtml = sb.ToString();
            
        }


        /// <summary>
        /// Calls GetColumnPosition() to set the order of the columns
        /// </summary>
        protected void SetCustomColumnOrder()
        {
            StringBuilder sb = new StringBuilder("");

            for (int i = 1; i <= VariablesBillTracker.NUMBER_OF_DYNAMIC_COLUMNS; i++)
            {
                sb.Append("<li id=\"" + GetColumnPosition(i)[0].ToString() + "\" class=\"ui-state-default\">" + GetColumnPosition(i)[1].ToString() + " </li>");
            }

            sortable.InnerHtml = sb.ToString();

        }


        /// <summary>
        /// SELECT c.id, replace(c.Heading, '<br />', ' ') as Heading 
        /// FROM BMcBEARBillTrackerDynamicColumns c 
        /// INNER JOIN BMcBEARBillTrackerUserPreferences p ON p.column" + column + " = c.id 
        /// AND p.UserId = '" + user + "' 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected String[] GetColumnPosition(int column)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String sql =
                " SELECT c.id, replace(c.Heading, '<br />', ' ') as Heading " +
                " FROM BMcBEARBillTrackerDynamicColumns c " +
                " INNER JOIN BMcBEARBillTrackerUserPreferences p ON p.column" + column + " = c.id " +
                " AND p.UserId = '" + user + "' ";
            
            String[] position = new String[2];
            position[0] = "id";
            position[1] = "position";

            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    position[0] = reader["id"].ToString();
                    position[1] = reader["heading"].ToString();
                }
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesBillTracker.ERROR_LOG_FILE_NAME, sqle.Message, "GetColumnPosition()", sql);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return position;
        }



    }
}
