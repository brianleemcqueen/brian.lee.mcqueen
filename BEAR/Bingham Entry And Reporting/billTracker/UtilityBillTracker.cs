using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace BEAR.billTracker
{
    public class UtilityBillTracker
    {

        public UtilityBillTracker(String user)
        {
            SetupDynamicColumns(user);
            IsAdministrator(user);
        }


        private List<int> currencyColumnsLocal = new List<int>();
        public List<int> CurrencyColumnsLocal
        {
            get { return this.currencyColumnsLocal; }
        }


        private List<int> currencyColumnsUSD = new List<int>();
        public List<int> CurrencyColumnsUSD
        {
            get { return this.currencyColumnsUSD; }
        }


        private List<int> dateColumns = new List<int>();
        public List<int> DateColumns
        {
            get { return this.dateColumns; }
        }


        protected bool[] rowChanged;
        public void SetRowChanged(int totalRows)
        {
            this.rowChanged = new bool[totalRows];
        }
        public bool[] GetRowChanged()
        {
            return this.rowChanged;
        }


        private bool isAdminUser = false;
        public bool IsAdminUser
        {
            get { return this.isAdminUser; }
            set { this.isAdminUser = value; }
        }


        private bool firstPassThroughResultSet = true;
        public bool FirstPassThroughResultSet
        {
            get { return this.firstPassThroughResultSet; }
            set { this.firstPassThroughResultSet = value; }
        }
        

        private String ColumnHeading1 = "";
        private String ColumnHeading2 = "";
        private String ColumnHeading3 = "";
        private String ColumnHeading4 = "";
        private String ColumnHeading5 = "";
        private String ColumnHeading6 = "";
        private String ColumnHeading7 = "";
        private String ColumnHeading8 = "";
        private String ColumnHeading9 = "";
        private String ColumnHeading10 = "";
        private String ColumnHeading11 = "";
        private String ColumnHeading12 = "";
        private String ColumnHeading13 = "";

        private String SQLColumnCode1 = "";
        private String SQLColumnCode2 = "";
        private String SQLColumnCode3 = "";
        private String SQLColumnCode4 = "";
        private String SQLColumnCode5 = "";
        private String SQLColumnCode6 = "";
        private String SQLColumnCode7 = "";
        private String SQLColumnCode8 = "";
        private String SQLColumnCode9 = "";
        private String SQLColumnCode10 = "";
        private String SQLColumnCode11 = "";
        private String SQLColumnCode12 = "";
        private String SQLColumnCode13 = "";

        private String ColumnWidth1 = "";
        private String ColumnWidth2 = "";
        private String ColumnWidth3 = "";
        private String ColumnWidth4 = "";
        private String ColumnWidth5 = "";
        private String ColumnWidth6 = "";
        private String ColumnWidth7 = "";
        private String ColumnWidth8 = "";
        private String ColumnWidth9 = "";
        private String ColumnWidth10 = "";
        private String ColumnWidth11 = "";
        private String ColumnWidth12 = "";
        private String ColumnWidth13 = "";

        private HorizontalAlign ColumnAlignment1;
        private HorizontalAlign ColumnAlignment2;
        private HorizontalAlign ColumnAlignment3;
        private HorizontalAlign ColumnAlignment4;
        private HorizontalAlign ColumnAlignment5;
        private HorizontalAlign ColumnAlignment6;
        private HorizontalAlign ColumnAlignment7;
        private HorizontalAlign ColumnAlignment8;
        private HorizontalAlign ColumnAlignment9;
        private HorizontalAlign ColumnAlignment10;
        private HorizontalAlign ColumnAlignment11;
        private HorizontalAlign ColumnAlignment12;
        private HorizontalAlign ColumnAlignment13;



        private void SetupDynamicColumns(String user)
        {

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String sql = " SELECT * " +
                         " FROM BMcBEARBillTrackerUserPreferences " +
                         " WHERE UserId = '" + user + "' ";

            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    for (int i = 1; i <= VariablesBillTracker.NUMBER_OF_DYNAMIC_COLUMNS; i++)
                    {
                        SetColumnDefinition(Convert.ToInt16(reader["Column" + i].ToString()), i);
                    }
                }

                else
                {
                    for (int i = 1; i <= VariablesBillTracker.NUMBER_OF_DYNAMIC_COLUMNS; i++)
                    {
                        SetColumnDefinition(i, i);
                    }
                }
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesBillTracker.ERROR_LOG_FILE_NAME, sqle.Message, "SetupDynamicColumns()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(VariablesBillTracker.ERROR_LOG_FILE_NAME, nre.Message, "SetupDynamicColumns()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

        }

        private void SetColumnDefinition(int ColumnNumber, int ColumnPosition)
        {

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String sql = " SELECT * " +
                         " FROM BMcBEARBillTrackerDynamicColumns " +
                         " WHERE ColumnNumber = " + ColumnNumber;

            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    String Heading = reader["Heading"].ToString();
                    String Width = reader["Width"].ToString();
                    HorizontalAlign Alignment = GetHorizontalAlignValue(reader["Alignment"].ToString());
                    String SQLColumnCode = reader["SQLColumnCode"].ToString();
                    if (SQLColumnCode.Equals("@LAST_BILL_DATE"))
                    {
                        this.dateColumns.Add(ColumnPosition);
                    }


                    String Currency = reader["Currency"].ToString();
                    if (Currency.Equals("local"))
                    {
                        this.CurrencyColumnsLocal.Add(ColumnPosition);
                    }
                    else if (Currency.Equals("usd"))
                    {
                        this.currencyColumnsUSD.Add(ColumnPosition);
                    }



                    switch (ColumnPosition)
                    {
                        case 1:
                            this.ColumnHeading1 = Heading;
                            this.ColumnWidth1 = Width;
                            this.ColumnAlignment1 = Alignment;
                            this.SQLColumnCode1 = SQLColumnCode;
                            break;
                        case 2:
                            this.ColumnHeading2 = Heading;
                            this.ColumnWidth2 = Width;
                            this.ColumnAlignment2 = Alignment;
                            this.SQLColumnCode2 = SQLColumnCode;
                            break;
                        case 3:
                            this.ColumnHeading3 = Heading;
                            this.ColumnWidth3 = Width;
                            this.ColumnAlignment3 = Alignment;
                            this.SQLColumnCode3 = SQLColumnCode;
                            break;
                        case 4:
                            this.ColumnHeading4 = Heading;
                            this.ColumnWidth4 = Width;
                            this.ColumnAlignment4 = Alignment;
                            this.SQLColumnCode4 = SQLColumnCode;
                            break;
                        case 5:
                            this.ColumnHeading5 = Heading;
                            this.ColumnWidth5 = Width;
                            this.ColumnAlignment5 = Alignment;
                            this.SQLColumnCode5 = SQLColumnCode;
                            break;
                        case 6:
                            this.ColumnHeading6 = Heading;
                            this.ColumnWidth6 = Width;
                            this.ColumnAlignment6 = Alignment;
                            this.SQLColumnCode6 = SQLColumnCode;
                            break;
                        case 7:
                            this.ColumnHeading7 = Heading;
                            this.ColumnWidth7 = Width;
                            this.ColumnAlignment7 = Alignment;
                            this.SQLColumnCode7 = SQLColumnCode;
                            break;
                        case 8:
                            this.ColumnHeading8 = Heading;
                            this.ColumnWidth8 = Width;
                            this.ColumnAlignment8 = Alignment;
                            this.SQLColumnCode8 = SQLColumnCode;
                            break;
                        case 9:
                            this.ColumnHeading9 = Heading;
                            this.ColumnWidth9 = Width;
                            this.ColumnAlignment9 = Alignment;
                            this.SQLColumnCode9 = SQLColumnCode;
                            break;
                        case 10:
                            this.ColumnHeading10 = Heading;
                            this.ColumnWidth10 = Width;
                            this.ColumnAlignment10 = Alignment;
                            this.SQLColumnCode10 = SQLColumnCode;
                            break;
                        case 11:
                            this.ColumnHeading11 = Heading;
                            this.ColumnWidth11 = Width;
                            this.ColumnAlignment11 = Alignment;
                            this.SQLColumnCode11 = SQLColumnCode;
                            break;
                        case 12:
                            this.ColumnHeading12 = Heading;
                            this.ColumnWidth12 = Width;
                            this.ColumnAlignment12 = Alignment;
                            this.SQLColumnCode12 = SQLColumnCode;
                            break;
                        case 13:
                            this.ColumnHeading13 = Heading;
                            this.ColumnWidth13 = Width;
                            this.ColumnAlignment13 = Alignment;
                            this.SQLColumnCode13 = SQLColumnCode;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesBillTracker.ERROR_LOG_FILE_NAME, sqle.Message, "SetColumnDefinition()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(VariablesBillTracker.ERROR_LOG_FILE_NAME, nre.Message, "SetColumnDefinition()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }


        }

        private HorizontalAlign GetHorizontalAlignValue(String alignCode)
        {
            HorizontalAlign h = HorizontalAlign.Left;

            if (alignCode.ToUpper().Equals("C"))
            {
                h = HorizontalAlign.Center;
            }
            else if (alignCode.ToUpper().Equals("R"))
            {
                h = HorizontalAlign.Right;
            }
            else if (alignCode.ToUpper().Equals("J"))
            {
                h = HorizontalAlign.Justify;
            }

            return h;
        }

        protected void IsAdministrator(String userName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            try
            {
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = "SELECT networkId FROM BMcBillTrackerAdminUsers WHERE networkId = '" + userName + "' ";
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    IsAdminUser = true;
                }

            }

            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesBillTracker.ERROR_LOG_FILE_NAME, sqle.Message, "IsAdministrator()", "");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }


        public String GetColumnHeading1() { return this.ColumnHeading1; }
        public String GetColumnHeading2() { return this.ColumnHeading2; }
        public String GetColumnHeading3() { return this.ColumnHeading3; }
        public String GetColumnHeading4() { return this.ColumnHeading4; }
        public String GetColumnHeading5() { return this.ColumnHeading5; }
        public String GetColumnHeading6() { return this.ColumnHeading6; }
        public String GetColumnHeading7() { return this.ColumnHeading7; }
        public String GetColumnHeading8() { return this.ColumnHeading8; }
        public String GetColumnHeading9() { return this.ColumnHeading9; }
        public String GetColumnHeading10() { return this.ColumnHeading10; }
        public String GetColumnHeading11() { return this.ColumnHeading11; }
        public String GetColumnHeading12() { return this.ColumnHeading12; }
        public String GetColumnHeading13() { return this.ColumnHeading13; }

        public String GetSQLColumnCode1() { return this.SQLColumnCode1; }
        public String GetSQLColumnCode2() { return this.SQLColumnCode2; }
        public String GetSQLColumnCode3() { return this.SQLColumnCode3; }
        public String GetSQLColumnCode4() { return this.SQLColumnCode4; }
        public String GetSQLColumnCode5() { return this.SQLColumnCode5; }
        public String GetSQLColumnCode6() { return this.SQLColumnCode6; }
        public String GetSQLColumnCode7() { return this.SQLColumnCode7; }
        public String GetSQLColumnCode8() { return this.SQLColumnCode8; }
        public String GetSQLColumnCode9() { return this.SQLColumnCode9; }
        public String GetSQLColumnCode10() { return this.SQLColumnCode10; }
        public String GetSQLColumnCode11() { return this.SQLColumnCode11; }
        public String GetSQLColumnCode12() { return this.SQLColumnCode12; }
        public String GetSQLColumnCode13() { return this.SQLColumnCode13; }
        
        public String GetColumnWidth1() { return this.ColumnWidth1; }
        public String GetColumnWidth2() { return this.ColumnWidth2; }
        public String GetColumnWidth3() { return this.ColumnWidth3; }
        public String GetColumnWidth4() { return this.ColumnWidth4; }
        public String GetColumnWidth5() { return this.ColumnWidth5; }
        public String GetColumnWidth6() { return this.ColumnWidth6; }
        public String GetColumnWidth7() { return this.ColumnWidth7; }
        public String GetColumnWidth8() { return this.ColumnWidth8; }
        public String GetColumnWidth9() { return this.ColumnWidth9; }
        public String GetColumnWidth10() { return this.ColumnWidth10; }
        public String GetColumnWidth11() { return this.ColumnWidth11; }
        public String GetColumnWidth12() { return this.ColumnWidth12; }
        public String GetColumnWidth13() { return this.ColumnWidth13; }

        public HorizontalAlign GetColumnAlignment1() { return this.ColumnAlignment1; } 
        public HorizontalAlign GetColumnAlignment2() { return this.ColumnAlignment2; } 
        public HorizontalAlign GetColumnAlignment3() { return this.ColumnAlignment3; } 
        public HorizontalAlign GetColumnAlignment4() { return this.ColumnAlignment4; } 
        public HorizontalAlign GetColumnAlignment5() { return this.ColumnAlignment5; } 
        public HorizontalAlign GetColumnAlignment6() { return this.ColumnAlignment6; } 
        public HorizontalAlign GetColumnAlignment7() { return this.ColumnAlignment7; } 
        public HorizontalAlign GetColumnAlignment8() { return this.ColumnAlignment8; } 
        public HorizontalAlign GetColumnAlignment9() { return this.ColumnAlignment9; } 
        public HorizontalAlign GetColumnAlignment10() { return this.ColumnAlignment10; } 
        public HorizontalAlign GetColumnAlignment11() { return this.ColumnAlignment11; } 
        public HorizontalAlign GetColumnAlignment12() { return this.ColumnAlignment12; } 
        public HorizontalAlign GetColumnAlignment13() { return this.ColumnAlignment13; }

        
    }
}
