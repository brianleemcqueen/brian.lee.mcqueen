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
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Mail;

namespace BEAR
{
    /// <summary>
    /// BearCode is a global class with methods for all BEAR applications
    /// </summary>
    public class BearCode
    {
        
        /// <summary>
        ///Customizes the Text on the Pager Row of a GridView to be BEAR Compliant.
        ///If the GridView has select turned on, this will need to be called from the OnSelectedIndexChanged event as
        ///well as the OnRowBound event.
        ///
        /// displays "Page x of xx     Change Page: " in the pager row
        /// </summary>
        /// <param name="gv">ID of the asp:GridView</param>
        /// <param name="pagerRow">pass in the pager row.  Generally (in BEAR) with e.Row or GridViewID.BottomPagerRow </param>
        public void GridViewCustomizePagerRow(GridView gv, GridViewRow pagerRow)
        {
            int currentPage = (int)gv.PageIndex + 1;
            TableCell newCell = new TableCell();
            newCell.Text = "<span class='pageCount'>Page "
                        + currentPage
                        + " of "
                        + gv.PageCount
                        + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Change Page: ";
            Table tbl = (Table)pagerRow.Cells[0].Controls[0];
            tbl.Rows[0].Cells.AddAt(0, newCell);

        }
        

        /// <summary>
        /// Example - '01' will return 'Boston'<br />
        /// <b>Database Operation</b><br />
        /// Selects a count from elite.  If Count > 0, Cost Code is Valid, else it is not.<br />
        /// SQL:<br />
        /// SELECT ldesc FROM dbo.location (nolock) WHERE locode = '" + LocationCode + "'
        /// </summary>
        /// <param name="LocationCode">Elite Location Code</param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <returns>the location's description from the Elite Location table</returns>
        public String GetLocationDescription(String LocationCode, String errorLogFileName)
        {
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String locationDescription = "";
            String sql = "";
            try
            {
                sql = " SELECT ldesc " +
                      " FROM dbo.location (nolock) " +
                      " WHERE locode = '" + LocationCode + "' ";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    locationDescription = reader[0].ToString();
                }
            }
            catch (SqlException sqle) {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetLocationDescription()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetLocationDescription()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return locationDescription;

        }


        /// <summary>
        /// Gets location and proforma attorney information from a matter<br />
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// uspBMcBEARMatterTimekeeperInfo
         /// </summary>
        /// <param name="matter">Matter Number</param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <returns>
        /// a String Array with the follow configuration:<br />
        ///    - 0 = matter.mloc - matter location
        ///    - 1 = location.ldesc - location description
        ///    - 2 = matter.mbillaty - billing / proforma attorney tkid
        ///    - 3 = timekeep.tkfirst + timekeep.tklast - billing / proforma attorney name
        /// </returns>
        public String[] GetMatterTimekeeperInfo(String matter, String errorLogFileName)
        {
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String[] matterTimekeeperInfo = new String[4] {"","","",""};
            String sql = "";
            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARMatterTimekeeperInfo";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@matter", matter);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    matterTimekeeperInfo[0] = reader[0].ToString();
                    matterTimekeeperInfo[1] = reader[1].ToString();
                    matterTimekeeperInfo[2] = reader[2].ToString();
                    matterTimekeeperInfo[3] = reader[3].ToString();
                }
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetMatterTimekeeperInfo()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetMatterTimekeeperInfo()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return matterTimekeeperInfo;

        }


        /// <summary>
        /// Gets all locations that are stored in the elite timekeep table<br />
        /// NOTE: <br />
        /// Bingham Strategic Advisors is Changed to BSA<br />
        /// Bingham Sports Conculting is Changed to BSC<br />
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// SELECT DISTINCT bt.tkloc, l.ldesc FROM dbo.timekeep (nolock) AS bt INNER JOIN dbo.location (nolock) AS l ON bt.tkloc = l.locode ORDER BY tkloc
        /// </summary>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <returns>List of ListItems containing Location Descriptions and Location Codes for all locations that are stored in the timekeep table</returns>
        public List<ListItem> GetTimekeeperLocations(String errorLogFileName)
        {
            List<ListItem> offices = new List<ListItem>();
            ListItem li = new ListItem();

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String locationNumber = "";
            String locationDescription = "";
            String sql = "";
            try
            {
                sql = " SELECT DISTINCT bt.tkloc, l.ldesc " +
                      " FROM dbo.timekeep (nolock) AS bt " +
                      " INNER JOIN dbo.location (nolock) AS l ON " +
                      " 	bt.tkloc = l.locode " +
                      " ORDER BY tkloc";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    locationNumber = reader[0].ToString();
                    locationDescription = reader[1].ToString();

                    if (locationDescription.Equals("Bingham Strategic Advisors"))
                    {
                        locationDescription = "BSA";
                    }
                    else if (locationDescription.Equals("Bingham Sports Consulting"))
                    {
                        locationDescription = "BSC";
                    }

                    li = new ListItem(locationNumber + " - " + locationDescription, locationNumber, true);
                    offices.Add(li);
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetTimekeeperLocations()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetTimekeeperLocations()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return offices;

        }


        /// <summary>
        /// Get all locations that are stored in the Matter table<br />
        /// Bingham Strategic Advisors is Changed to BSA<br />
        /// Bingham Sports Conculting is Changed to BSC<br />
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// SELECT DISTINCT m.mloc, l.ldesc FROM dbo.matter (nolock) AS m INNER JOIN dbo.location (nolock) AS l ON m.mloc = l.locode ORDER BY m.mloc
        /// </summary>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <returns>a List of ListItems containing Location Descriptions and Location Codes for all locations that are stored in the Matter table</returns>
        public List<ListItem> GetMatterLocations(String errorLogFileName)
        {
            List<ListItem> offices = new List<ListItem>();
            ListItem li = new ListItem();

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String locationNumber = "";
            String locationDescription = "";
            String sql = "";
            try
            {
                sql = " SELECT DISTINCT m.mloc, l.ldesc " +
                        " FROM dbo.matter (nolock) AS m " +
                        " INNER JOIN dbo.location (nolock) AS l ON m.mloc = l.locode " +
                        " ORDER BY m.mloc";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 120;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    locationNumber = reader[0].ToString();
                    locationDescription = reader[1].ToString();

                    if (locationDescription.Equals("Bingham Strategic Advisors"))
                    {
                        locationDescription = "BSA";
                    }
                    else if (locationDescription.Equals("Bingham Sports Consulting"))
                    {
                        locationDescription = "BSC";
                    }

                    li = new ListItem(locationNumber + " - " + locationDescription, locationNumber, true);
                    offices.Add(li);
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetMatterLocations()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetMatterLocations()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return offices;

        }

        /// <summary>
        /// Gets all locations that are stored in the locations table <br />
        /// Bingham Strategic Advisors is Changed to BSA <br />
        /// Bingham Sports Conculting is Changed to BSC <br />
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// SELECT DISTINCT locode, ldesc FROM dbo.location (nolock) WHERE locode <> '00' AND ldesc NOT LIKE '%do not use%' ORDER BY locode 
        /// </summary>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <returns>a List of ListItems containing Location Descriptions and Location Codes for all locations that are stored in the locations table</returns>
        public List<ListItem> GetLocations(String errorLogFileName)
        {
            List<ListItem> offices = new List<ListItem>();
            ListItem li = new ListItem();

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String locationNumber = "";
            String locationDescription = "";
            String sql = "";
            try
            {
                sql = " SELECT DISTINCT locode, ldesc " +
                      " FROM dbo.location (nolock) " +
                      " WHERE locode <> '00' " +
                      " AND ldesc NOT LIKE '%do not use%' " +
                      " ORDER BY locode ";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    locationNumber = reader[0].ToString();
                    locationDescription = reader[1].ToString();

                    if (locationDescription.Equals("Bingham Strategic Advisors"))
                    {
                        locationDescription = "BSA";
                    }
                    else if (locationDescription.Equals("Bingham Sports Consulting"))
                    {
                        locationDescription = "BSC";
                    }

                    li = new ListItem(locationNumber + " - " + locationDescription, locationNumber, true);
                    offices.Add(li);
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetLocations()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetLocations()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return offices;

        }


        /// <summary>
        /// Get Billing Specialist from Elite's udf
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// uspBMcBEARExceptionRatesBillingSpecialists
        /// </summary>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <returns>a List of ListItems containing Billing Specialist ID and Name as stored in Elite</returns>
        public List<ListItem> GetBillingSpecialists(String errorLogFileName)
        {
            List<ListItem> billingSpecialists = new List<ListItem>();
            ListItem li = new ListItem();

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String billSpecValue = "";
            String billSpecText = "";
            try
            {

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARExceptionRatesBillingSpecialists";
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    billSpecValue = reader["udvalue"].ToString();
                    billSpecText = reader["ConcatResult"].ToString();

                    li = new ListItem(billSpecText, billSpecValue, true);
                    billingSpecialists.Add(li);
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetBillingSpecialists()", "");
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetBillingSpecialists()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return billingSpecialists;

        }


        /// <summary>
        /// Gets all Start Dates for a Master Matter and places them in a List
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// uspBMcBEARMasterMatterStartDates
        /// </summary>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <param name="matterNumber">Master Matter Number</param>
        /// <returns>a List of ListItems containing Master Start Dates as stored in Elite's mattSub table</returns>
        public List<ListItem> GetMasterMatterStartDates(String errorLogFileName, String matterNumber)
        {
            List<ListItem> startDates = new List<ListItem>();
            ListItem li = new ListItem();

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String startDatesValue = "";
            String startDatesText = "";
            try
            {

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARMasterMatterStartDates";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@matter", matterNumber);

                SqlDataReader reader = command.ExecuteReader();
                int counter = 0;
                while (reader.Read())
                {
                    if (counter == 0)
                    {
                        ListItem liDefault = new ListItem("Please Select a Start Date", "-1", true);
                        startDates.Add(liDefault);
                    }
                    startDatesValue = reader["dateValue"].ToString();
                    startDatesText = reader["dateText"].ToString();

                    li = new ListItem(startDatesText, startDatesValue, true);
                    startDates.Add(li);
                    counter++;
                }

                if (counter == 0)
                {
                    ListItem liDefault = new ListItem("No Start Dates Exist for This Matter Number", "-1", true);
                    startDates.Add(liDefault);
                }


                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetMasterMatterStartDates()", "");
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetMasterMatterStartDates()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return startDates;

        }

        /// <summary>
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// SELECT approval_group_id AS department FROM dbo.glnat (nolock) WHERE natcode = " + GLCode
        /// </summary>
        /// <param name="GLCode"></param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <returns></returns>
        public String GetDepartmentCodeFromGLCode(String GLCode, String errorLogFileName)
        {
            String department = "";
            String sql = "";
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            try
            {
                //sql = " select d.head1 as department " 
                //     + " from bmelite.son_db.dbo.deptlab as d (nolock) "
                //     + " inner join glnat as n (nolock) on n.approval_group_id = d.delcode "
                //     + " where n.natcode = " + GLCode;

                sql = " SELECT approval_group_id AS department FROM dbo.glnat (nolock) WHERE natcode = " + GLCode;

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    department = reader["department"].ToString();
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetDepartmentFromGLCode()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetDepartmentFromGLCode()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return department;

        }

        /// <summary>
        /// Gets a list of departments from Elite that have had the label1 field set to Y in the deptlab table
        /// This method always goes against BMELITE
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// SELECT b.delcode, b.head1 FROM bmelite.son_db.dbo.deptlab as b (nolock) WHERE b.label1 = 'Y' ORDER BY b.head1
        /// </summary>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <returns>a List of ListItems containing Departments from Elite PRODUCTION that are marked as valid in Label1</returns>
        public List<ListItem> GetEliteDepartments(String errorLogFileName)
        {
            List<ListItem> departments = new List<ListItem>();
            ListItem li = new ListItem();

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String deptCode = "";
            String deptDesc = "";
            String sql = "";
            try
            {
                sql = " SELECT b.delcode, b.head1 FROM bmelite.son_db.dbo.deptlab as b (nolock) WHERE b.label1 = 'Y' ORDER BY b.head1 ";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    deptCode = reader[0].ToString();
                    deptDesc = reader[1].ToString();

                    li = new ListItem(deptDesc + " (" + deptCode + ") ", deptCode, true);
                    departments.Add(li);
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetEliteDepartments()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetEliteDepartments()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return departments;

        }


        /// <summary>
        /// Populates a ListBox of locations from the matter or timekeeper table.
        /// Calls GetMatterLocations or GetTimekeeperLocations based on what is passed in to the source parameter
        /// </summary>
        /// <param name="lb">a ListBox to populate</param>
        /// <param name="source">"matter" or "timekeeper"</param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        public void PopulateListLocations(ListBox lb, String source, String errorLogFileName)
        {
            List<ListItem> offices;
            if (source.Equals("matter"))
            {
                offices = GetMatterLocations(errorLogFileName);
            }
            else if (source.Equals("timekeeper"))
            {
                offices = GetTimekeeperLocations(errorLogFileName);
            }
            else
            {
                offices = GetLocations(errorLogFileName);
            }
            for (int i = 0; i < offices.Count; i++)
            {
                lb.Items.Add(offices[i]);
            }
        }


        /// <summary>
        /// Populates a RadioButtonList of locations from the matter or timekeeper table.
        /// Calls GetMatterLocations or GetTimekeeperLocations based on what is passed in to the source parameter
        /// </summary>
        /// <param name="rbl">RadioButtonList to populate</param>
        /// <param name="source">"matter" or "timekeeper"</param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        public void PopulateListLocations(RadioButtonList rbl, String source, String errorLogFileName)
        {
            List<ListItem> offices;
            if (source.Equals("matter"))
            {
                offices = GetMatterLocations(errorLogFileName);
            }
            else if (source.Equals("timekeeper"))
            {
                offices = GetTimekeeperLocations(errorLogFileName);
            }
            else
            {
                offices = GetLocations(errorLogFileName);
            }
            for (int i = 0; i < offices.Count; i++)
            {
                rbl.Items.Add(offices[i]);
            }
        }


        /// <summary>
        /// Populates a CheckBoxList of locations from the matter or timekeeper table.
        /// Calls GetMatterLocations or GetTimekeeperLocations based on what is passed in to the source parameter
        /// </summary>
        /// <param name="lb">CheckBoxList to populate</param>
        /// <param name="source">"matter" or "timekeeper"</param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        public void PopulateListLocations(CheckBoxList lb, String source, String errorLogFileName)
        {
            List<ListItem> offices;
            if (source.Equals("matter"))
            {
                offices = GetMatterLocations(errorLogFileName);
            }
            else if (source.Equals("timekeeper"))
            {
                offices = GetTimekeeperLocations(errorLogFileName);
            }
            else
            {
                offices = GetLocations(errorLogFileName);
            }
            for (int i = 0; i < offices.Count; i++)
            {
                lb.Items.Add(offices[i]);
            }
        }


        /// <summary>
        /// Populates a CheckBoxList with deparments from Elite
        /// Calls GetEliteDepartments
        /// </summary>
        /// <param name="lb">CheckBoxList to populate</param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        public void PopulateDepartments(CheckBoxList lb, String errorLogFileName)
        {
            List<ListItem> departments;
            departments = GetEliteDepartments(errorLogFileName);

            for (int i = 0; i < departments.Count; i++)
            {
                lb.Items.Add(departments[i]);
            }
        }



        /// <summary>
        /// Populates a DropDownList with Billing Specialists from Elite
        /// Calls GetBillingSpecialists
        /// </summary>
        /// <param name="ddl">DropDownList to populate</param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        public void PopulateListBillingSpecialists(DropDownList ddl, String errorLogFileName)
        {
            List<ListItem> billingSpecialists = GetBillingSpecialists(errorLogFileName);
            for (int i = 0; i < billingSpecialists.Count; i++)
            {
                ddl.Items.Add(billingSpecialists[i]);
            }
        }


        /// <summary>
        /// Populates a DropDownList with Master Matter Start Dates from Elite
        /// Calls GetMasterMatterStartDates
        /// </summary>
        /// <param name="ddl">DropDownList to populate</param>
        /// <param name="matterNumber">Master Matter Number</param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        public void PopulateMasterMatterStartDates(DropDownList ddl, String matterNumber, String errorLogFileName)
        {
            List<ListItem> startDates = GetMasterMatterStartDates(errorLogFileName, matterNumber);
            for (int i = 0; i < startDates.Count; i++)
            {
                ddl.Items.Add(startDates[i]);
            }
        }

        //This was intended for use with LockBox.
        //public static void SendEmail(String errorFileName, String Body, String Subject, String From, String recipient, bool lookupEmail)
        //{
        //    MailMessage message = new MailMessage();
        //    SmtpClient client = new SmtpClient();
        //    message.From = new MailAddress(From);
        //    message.To.Clear();
        //    message.CC.Clear();
        //    message.Bcc.Clear();

        //    if (lookupEmail)
        //    {
        //        //query Elite for email address based on recipient and change recipient variable to valid email address
        //        SqlConnection con = new SqlConnection(
        //                ConfigurationManager.ConnectionStrings["lockboxConnectionString"].ConnectionString);
        //        try
        //        {

        //            con.Open();

        //            SqlCommand command = con.CreateCommand();
        //            command.CommandText = "SelectEmailAddressFromEliteLive";
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@LoginId", recipient);

        //            SqlDataReader reader = command.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                recipient = "\"" + reader["tkFullName"].ToString() +"\"<" + reader["tkemail"].ToString() +">"; 
        //            }

        //            reader.Close();

        //        }
        //        catch (SqlException sqle)
        //        {
        //            Logger.QuickLog(errorFileName, sqle.Message, "SendEmail()", "");
        //        }

        //    }
        //    try
        //    {
        //        message.To.Add(new MailAddress(recipient));
        //    }
        //    catch (FormatException fe)
        //    {
        //        Logger.QuickLog(errorFileName, fe.Message, "SendEmail()", "");
        //    }

        //    message.Subject = Subject;
        //    message.Body = Body;
        //    message.IsBodyHtml = true;
        //    try
        //    {
        //        client.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.QuickLog(errorFileName, ex.Message, "BearCode.SendEmail()", "");
        //    }
        //}



        /// <summary>
        /// Returns a field's value from the timekeep table in Elite
        /// Field must be identified in uspBMcBEARTimekeeperInfo storedProcedure
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// uspBMcBEARTimekeeperInfo
        /// </summary>
        /// <param name="Tkid">TKID to query</param>
        /// <param name="field">Field to Return from Elite's timekeeper table</param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <returns>a field's value from timekeep table in Elite</returns>
        public String GetTimekeeperInfo(String Tkid, String field, String errorLogFileName)
        {
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            
            ///set up default return values
            String returnValue = "Tkid Not Found in Elite";
            if (field.Equals("tkloc"))
            {
                returnValue = "-1";
            }
            
            try
            {

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARTimekeeperInfo";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tkidToSelect",Tkid);

                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    returnValue = reader[field].ToString();
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetTimekeeperInfo()", "");
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetTimekeeperInfo()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return returnValue;

        }


        /// <summary>
        /// Get Matter Description from Elite<br />
        /// <b>Database Operation</b><br />
        /// SQL:<br />
        /// uspBMcBEARMatterInfo
        /// </summary>
        /// <param name="matter">Matter Number</param>
        /// <param name="errorLogFileName">File location for the Error Log</param>
        /// <returns>first description line on a matter from elite</returns>
        public String GetMatterDesc1(String matter, String errorLogFileName)
        {

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String returnValue = "Matter Not Found in Elite";

            try
            {

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARMatterInfo";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@matterToSelect", AddLeadingZeros(matter, "matter"));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    returnValue = reader["mdesc1"].ToString();
                }

                reader.Close();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetMatterDesc1()", "");
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(errorLogFileName, nre.Message, "GetMatterDesc1()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return returnValue;

        }

 

        /// <summary>
        /// Method to Pad zeros to TKID, Client Number or Matter Number
        /// 
        /// TKID should be 5 characters
        /// 
        /// Client should be 7 characters
        /// 
        /// Matter should be 10 characters
        /// </summary>
        /// <param name="inputValue">value to pad</param>
        /// <param name="inputType">tkid, client or matter</param>
        /// <returns>Zero padded TKID, Client or Matter</returns>
        public String AddLeadingZeros(String inputValue, String inputType)
        {
            int inputLength = inputValue.Length;
            String returnValue = inputValue;

            if (inputType.ToLower().Equals("tkid"))
            {
                switch (inputLength)
                {
                    case 1:
                        returnValue = "0000" + inputValue;
                        break;
                    case 2:
                        returnValue = "000" + inputValue;
                        break;
                    case 3:
                        returnValue = "00" + inputValue;
                        break;
                    case 4:
                        returnValue = "0" + inputValue;
                        break;
                    default:
                        break;
                }

            }
            else if (inputType.ToLower().Equals("client"))
            {
                switch (inputLength)
                {
                    case 1:
                        returnValue = "000000" + inputValue;
                        break;
                    case 2:
                        returnValue = "00000" + inputValue;
                        break;
                    case 3:
                        returnValue = "0000" + inputValue;
                        break;
                    case 4:
                        returnValue = "000" + inputValue;
                        break;
                    case 5:
                        returnValue = "00" + inputValue;
                        break;
                    case 6:
                        returnValue = "0" + inputValue;
                        break;
                    default:
                        break;
                }

            }
            else if (inputType.ToLower().Equals("matter"))
            {
                switch (inputLength)
                {
                    case 1:
                        returnValue = "000000000" + inputValue;
                        break;
                    case 2:
                        returnValue = "00000000" + inputValue;
                        break;
                    case 3:
                        returnValue = "0000000" + inputValue;
                        break;
                    case 4:
                        returnValue = "000000" + inputValue;
                        break;
                    case 5:
                        returnValue = "00000" + inputValue;
                        break;
                    case 6:
                        returnValue = "0000" + inputValue;
                        break;
                    case 7:
                        returnValue = "000" + inputValue;
                        break;
                    case 8:
                        returnValue = "00" + inputValue;
                        break;
                    case 9:
                        returnValue = "0" + inputValue;
                        break;
                    default:
                        break;
                }

            }

            return returnValue;

        }




        /// <summary>
        /// used to validate a variable to check if it is a valid number 
        /// </summary>
        /// <param name="value">value to test</param>
        /// <returns>True if value is a number.  False if valuse is not a number</returns>
        public bool IsNumber(String value)
        {
            return IsNumber(value, false);
        }

        /// <summary>
        /// used to validate a variable to check if it is a valid int or double 
        /// </summary>
        /// <param name="value">value to test</param>
        /// <param name="isInt">true will test for int.  false will test for double</param>
        /// <returns>True if value is a number.  False if valuse is not a number</returns>
        public bool IsNumber(String value, bool isInt)
        {
            bool isNumber = false;
            if (isInt)
            {
                int num;
                isNumber = int.TryParse(value, out num);
            }
            else
            {
                double num;
                isNumber = double.TryParse(value, out num);
            }
            return isNumber;

        }

        /// <summary>
        /// used to validate a variable to check if it is a valid date or dateTime
        /// </summary>
        /// <param name="value">value to test</param>
        /// <returns>True if value is a DateTime.  False if value is not a DateTime</returns>
        public bool IsDate(String value)
        {
            DateTime dateValue;
            bool isDate = DateTime.TryParse(value, out dateValue);

            return isDate;
        }


        /// <summary>
        /// Breaks a ToolTip after 20 characters
        /// </summary>
        /// <param name="textToFormat"></param>
        /// <param name="LeadingText"></param>
        /// <returns></returns>
        public String FormatToolTip(String textToFormat, String LeadingText)
        {
            int rowLength = 20;
            int numberOfRows = (int)Math.Ceiling((decimal)textToFormat.Length / 20);
            String formattedText = LeadingText;
            for (int i = 1; i <= numberOfRows; i++)
            {
                if (i == numberOfRows)
                {
                    formattedText += textToFormat.Substring(rowLength * (i - 1)) + "<br />";
                }
                else
                {
                    formattedText += textToFormat.Substring(rowLength * (i - 1), rowLength) + "<br />";
                }
            }
            return formattedText;
        }



    }
}
