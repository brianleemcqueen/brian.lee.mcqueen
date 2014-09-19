using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BEAR.cas
{
    /** \class UtilityCAS
     *  a utility class designed to place common functions and global variables specific to the BEAR CAS Application
     */
    public class UtilityCAS
    {

        protected bool[] rowChanged; ///<an array of rows in the Results Grid used to track rows that need to be saved.*/

        public void SetRowChanged(int totalRows)
        {
            this.rowChanged = new bool[totalRows];
        }
        public bool[] GetRowChanged()
        {
            return this.rowChanged;
        }


        /// <summary>
        /// <b>Database Operation</b><br />
        /// Retrieves the CostCode Description from Elite database<br />
        /// SELECT codesc1<br />
        /// FROM dbo.costcode (nolock)<br />
        /// WHERE cocode = '" + CostCode + "' "<br />
        /// </summary>
        /// <param name="CostCode">Cost Code to Retrive</param>
        /// <returns>Cost Code Description for CostCode</returns>
        public String GetCostCodeDescription(String CostCode)
        {
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String costCodeDesc = "Code Not Found in Elite";
            String sql = "";
            try
            {
                sql = " SELECT codesc1" +
                      " FROM dbo.costcode (nolock)" +
                      " WHERE cocode = '" + CostCode + "' ";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    costCodeDesc = reader["codesc1"].ToString();

                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "GetCostCodeDescription()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, nre.Message, "GetCostCodeDescription()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return costCodeDesc;

        }
        

        /// <summary>
        /// <b>Database Operation</b><br />
        /// Selects a count from elite.  If Count > 0, Cost Code is Valid, else it is not.<br />
        /// SQL<br />
        /// SELECT count(*) as count<br />
        /// FROM dbo.Costcode (nolock)<br />
        /// WHERE cocode = '" + CostCodeToValidate + "' <br />
        /// AND cocode NOT LIKE 'E%' <br />
        /// AND cocode NOT LIKE 'L%' <br />
        /// AND codesc1 NOT LIKE '%DO NOT USE%'<br />
        /// </summary>
        /// <param name="CostCodeToValidate">Cost Code</param>
        /// <returns>bool to represent if Cost Code is Valid</returns>
        public bool ValidateCostCode(String CostCodeToValidate)
        {
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            bool ValidCostCode = false;
            String sql = "";
            try
            {
                sql = " SELECT count(*) as count" +
                      " FROM dbo.Costcode (nolock)" +
                      " WHERE cocode = '" + CostCodeToValidate + "' " + 
                      " AND cocode NOT LIKE 'E%' " +
                      " AND cocode NOT LIKE 'L%' " +
                      " AND codesc1 NOT LIKE '%DO NOT USE%' ";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (Convert.ToInt16(reader["count"].ToString()) > 0)
                    {
                        ValidCostCode = true;
                    }

                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "ValidateCostCode()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, nre.Message, "ValidateCostCode()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return ValidCostCode;

        }



        /// <summary>
        /// <b>Database Operation</b><br />
        /// Selects a count from elite.  If Count > 0, GLString is Valid.<br />
        /// Next code checks to see if the GLString is Active.  (glstat = "I")
        /// SQL<br />
        /// SELECT count(*) as isValid, UPPER(isnull(max(glstat),'-1')) as glstat<br />
        /// FROM dbo.gl (nolock) <br />
        /// WHERE glnum = '" + GLString + "'  <br />
        /// </summary>
        /// <param name="GLString"></param>
        /// <returns></returns>
        public int GetGLStatusCode(String GLString)
        {
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            String sql = "";

            int GLStatusCode = -1;

            bool isValid = false;
            String GLStatus = "I";
            bool isActive = false;
            try
            {
                sql = " SELECT count(*) as isValid, UPPER(isnull(max(glstat),'-1')) as glstat " +
                      " FROM dbo.gl (nolock) " +
                      " WHERE glnum = '" + GLString + "' ";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (!reader["isValid"].ToString().Equals("0"))
                    {
                        isValid = true;
                    }

                    if (isValid)
                    {
                        GLStatus = reader["glstat"].ToString();
                        if (reader["glstat"].ToString().Equals("A"))
                        {
                            isActive = true;
                        }
                    }

                    if (!isValid)
                    {
                        GLStatusCode = StatusCAS.InvalidGLString;
                    }
                    else if (!isActive)
                    {
                        GLStatusCode = StatusCAS.InactiveGLString;
                    }
                    else
                    {
                        GLStatusCode = 0;
                    }

                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "ValidateGLString()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, nre.Message, "ValidateGLString()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return GLStatusCode;

        }




        /// <summary>
        /// <b>Database Operation</b><br />
        /// Selects a count from elite.  If Count > 0, Timekeeper is Valid, else it is not.<br />
        /// SQL<br />
        /// SELECT count(*) as count<br />
        /// FROM dbo.Timekeep (nolock) <br />
        /// WHERE tkinit = '" + TkidToValidate + "' <br />
        /// AND tkceflag = 'Y' <br />
        /// </summary>
        /// <param name="TkidToValidate">TKID</param>
        /// <returns>bool to represent if TKID is Valid</returns>
        public bool ValidateTimekeeper(String TkidToValidate)
        {
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            bool ValidTimekeep = false;
            String sql = "";
            try
            {
                sql = " SELECT count(*) as count " +
                      " FROM dbo.Timekeep (nolock)" +
                      " WHERE tkinit = '" + TkidToValidate + "' " +
                        " AND tkceflag = 'Y' " ;

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (Convert.ToInt16(reader["count"].ToString()) > 0)
                    {
                        ValidTimekeep = true;
                    }

                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "ValidateTimekeeper()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, nre.Message, "ValidateTimekeeper()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return ValidTimekeep;

        }


        /// <summary>
        /// <b>Database Operation</b><br />
        /// Matter Status from Elite and compares to VariablesCAS.VALID_MATTER_STATUS<br />
        /// SQL:<br />
        /// SELECT mstatus<br />
        /// FROM dbo.matter (nolock) <br />
        /// WHERE mmatter = '" + MatterToValidate + "' <br />
        /// </summary>
        /// <param name="TkidToValidate">Matter Number</param>
        /// <returns>bool to represent if Matter Status is Valid</returns>
        public bool ValidateMatterStatus(String MatterToValidate)
        {
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            bool ValidMatterStatus = false;
            String sql = "";
            try
            {
                sql = " SELECT mstatus" +
                      " FROM dbo.matter (nolock)" +
                      " WHERE mmatter = '" + MatterToValidate + "' ";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    String matterStatus = reader["mstatus"].ToString();

                    foreach (string validStatus in VariablesCAS.VALID_MATTER_STATUS)
                    {
                        if (matterStatus.Equals(validStatus))
                        {
                            ValidMatterStatus = true;
                        }
                    }

                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "ValidateMatterStatus()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, nre.Message, "ValidateMatterStatus()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return ValidMatterStatus;

        }


        /// <summary>
        /// <b>Database Operation</b><br />
        /// Selects a count from elite.  If Count > 0, Matter is Valid, else it is not.<br />
        /// SQL<br />
        /// SELECT count(*) as count<br />
        /// FROM dbo.matter (nolock) <br />
        /// WHERE mmatter = '" + MatterToValidate + "' <br />
        /// </summary>
        /// <param name="MatterToValidate">Matter</param>
        /// <returns>bool to represent if Matter is Valid</returns>
        public bool ValidateMatter(String MatterToValidate)
        {
            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

            bool ValidMatter = false;
            String sql = "";
            try
            {
                sql = " SELECT count(*) as count" +
                      " FROM dbo.matter " +
                      " WHERE mmatter = '" + MatterToValidate + "' ";

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (Convert.ToInt16(reader["count"].ToString()) > 0)
                    {
                        ValidMatter = true;
                    }

                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "ValidateMatter()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, nre.Message, "ValidateMatter()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return ValidMatter;

        }


        /// <summary>
        /// <b>Database Operation</b><br />
        /// Selects a count from elite.  If Count > 0, ClientMatter is Valid, else it is not.<br />
        /// SQL<br />
        /// SELECT count(*) as count<br />
        /// FROM dbo.matter (nolock) <br />
        /// WHERE mmatter = '" + MatterToValidate + "' <br />
        /// </summary>
        /// <param name="MatterToValidate">Matter Number</param>
        /// <param name="ClientToValidate">Client Number</param>
        /// <returns>bool to represent if Client Matter is Valid</returns>
         public bool ValidateClientMatter(String MatterToValidate, String ClientToValidate)
        {
            bool ValidClient = false;

            if (ValidateMatter(MatterToValidate))
            {
                SqlConnection con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);

                String sql = "";
                try
                {
                    sql = " SELECT count(*) as count" +
                          " FROM dbo.matter (nolock) " +
                          " WHERE mmatter = '" + MatterToValidate + "' " +
                           " AND mclient = '" + ClientToValidate + "' ";

                    con.Open();

                    SqlCommand command = con.CreateCommand();
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        if (Convert.ToInt16(reader["count"].ToString()) > 0)
                        {
                            ValidClient = true;
                        }
                    }
                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "ValidateClientMatter()", sql);
                }
                catch (NullReferenceException nre)
                {
                    Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, nre.Message, "ValidateClientMatter()", "");
                }
                finally
                {
                    if (con != null)
                        con.Close();
                }

            }
            return ValidClient;

        }


        /// <summary>
         /// Runs ValidateTimekeeper, ValidateMatter, ValidateMatterStatus, ValidateCostCode<br />
         /// and sets the status according to the results.
        /// </summary>
        /// <param name="currentStatus">Current Status</param>
        /// <param name="savedTkid">TKID to Validate</param>
        /// <param name="savedMatterNumber">Matter to Validate</param>
        /// <param name="savedCostCode">Cost Code to Validate</param>
        /// <returns>New Status Code</returns>
        public int GetStatusForSaveCost(int currentStatus, String savedTkid, String savedMatterNumber, String savedCostCode)
        {

            bool validTkid = ValidateTimekeeper(savedTkid);
            bool validMatterNumber = ValidateMatter(savedMatterNumber);
            bool validMatterStatus = ValidateMatterStatus(savedMatterNumber);
            bool validCostCode = ValidateCostCode(savedCostCode);

            int newStatus = SetStatus(currentStatus, StatusCAS.InvalidTimekeeper, !validTkid);
            newStatus = SetStatus(newStatus, StatusCAS.InvalidClientMatter, !validMatterNumber);
            newStatus = SetStatus(newStatus, StatusCAS.ClosedMatter, !validMatterStatus);
            newStatus = SetStatus(newStatus, StatusCAS.InvalidCostCode, !validCostCode);

            if (newStatus == 0)
            {
                newStatus = StatusCAS.Valid;
            }

            return newStatus;

        }


        public int GetStatusForSaveGL(int currentStatus, String savedGLString)
        {

            ///SetStatus works like this:
            /// if addFlagToStatus is true and flag is already in status, then status is unchanged
            ///
            ///if addFlagToStatus is true and flag is not already in status, then flag is added to status
            ///
            ///if addFlagToStatus is false and flag is already in status, then flag is removed from status
            ///
            ///if addFlagToStatus is false and flag is not already in status, then status is unchanged
            ///
            ///Applying this to GLStatus:
            /// GLStatus can either be 0 (no issues), 512 (Inactive) or 1024 (Invalid).
            ///It cannot be both 512 and 1024
            ///If GLStatus = 0 - remove both 512 & 1024
            ///If GLStatus = 512 - check for 512, add if not there, remove 1024 if there
            ///If GLStatus = 1024 - check for 1024, add if not there, remove 512 if there
            ///
            
            int GLStatusCode = GetGLStatusCode(savedGLString);
            int newStatus = 0;

            if (GLStatusCode == StatusCAS.InactiveGLString) //512
            {
                newStatus = SetStatus(currentStatus, StatusCAS.InactiveGLString, true); //add 512
                newStatus = SetStatus(currentStatus, StatusCAS.InvalidGLString, false); //subtract 1024
            }
            else if (GLStatusCode == StatusCAS.InvalidGLString)  //1024
            {
                newStatus = SetStatus(currentStatus, StatusCAS.InactiveGLString, false); //subtract 512
                newStatus = SetStatus(currentStatus, StatusCAS.InvalidGLString, true); //add 1024
            }
            else //0
            {
                newStatus = SetStatus(currentStatus, StatusCAS.InactiveGLString, false); //subtract 512
                newStatus = SetStatus(currentStatus, StatusCAS.InvalidGLString, false); //subtract 1024
            }

            return newStatus;
        }


        /// <summary>
        /// Validates a records TKID, Matter Number and Cost Code<br />
        /// Calls:   ValidateTimekeeper(Tkid) <br />
        ///            ValidateMatter(MatterNumber) <br />
        ///            ValidateMatterStatus(MatterNumber) <br />
        ///            ValidateCostCode(CostCode)<br />
        /// </summary>
        /// <param name="Tkid">TKID to Validate</param>
        /// <param name="MatterNumber">Matter Number to Validate</param>
        /// <param name="CostCode">Cost Code to Validate</param>
        /// <returns>records Valid Status in form of a boolean</returns>
        public bool IsStatusValidForWebApplication(String Tkid, String MatterNumber, String CostCode)
        {
            bool IsValid = false;
            if (    ValidateTimekeeper(Tkid) 
                &&  ValidateMatter(MatterNumber) 
                &&  ValidateMatterStatus(MatterNumber) 
                &&  ValidateCostCode(CostCode)
                )
            {
                IsValid = true;
            }

            return IsValid;
        }


        /// <summary>
        /// checks to see if the passed-in flag (status) is included in the current status
        /// using bitwise operation
        /// </summary>
        /// <param name="status">Current Status</param>
        /// <param name="flag">Status to Check</param>
        /// <returns>bool</returns>
        public bool GetStatus(int status, int flag)
        {
            int CheckStatus = status & flag;
            return (CheckStatus == flag);

        }


        /// <summary>
        /// sets the current status to contain the passed-in flag value
        ///if addFlagToStatus is true and flag is already in status, then status is unchanged
        ///
        ///if addFlagToStatus is true and flag is not already in status, then flag is added to status
        ///
        ///if addFlagToStatus is false and flag is already in status, then flag is removed from status
        ///
        ///if addFlagToStatus is false and flag is not already in status, then status is unchanged
        /// </summary>
        /// <param name="status">Current Status</param>
        /// <param name="flag">Status to Check</param>
        /// <param name="addFlagToStatus">Add or Remove (see method summary)</param>
        /// <returns>new or unchanged status</returns>
        public int SetStatus(int status, int flag, bool addFlagToStatus)
        {
            if (GetStatus(status, flag) != addFlagToStatus)
            {
                if (addFlagToStatus)
                {
                    status = (status | flag);	// Add status
                }
                else
                {
                    status = (status ^ flag);	// Substract status
                }
            }

            return status;
        }


        /// <summary>
        /// <b>Database Operation</b><br />
        /// Sets or Removes Override Flag in dbo.BEAR_OverrideOfficeLocation<br />
        /// to set a flag, the TempId is added to the table with the overridden column set to 1 with this SQL statement<br />
        /// INSERT INTO dbo.BEAR_OverrideOfficeLocation (TempId, overridden ) VALUES (" + id.ToString() + ", 1 )";<br />
        /// 
        /// to remove a flag, the record is deleted from the table with this SQL statement<br />
        /// DELETE FROM dbo.BEAR_OverrideOfficeLocation WHERE TempId = " + id.ToString();<br />
        /// 
        /// </summary>
        /// <param name="id">TempId value to set/remove</param>
        /// <param name="overrideOn">Determines if Override is to be set (true) or removed(false)</param>
        public void SetOverrideOfficeFlag(String id, bool overrideOn)
        {
            if (overrideOn && GetOverrideOfficeFlag(id))
            {
                // do nothing - override flag already set
            }
            else
            {
                SqlConnection con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["casConnectionString"].ConnectionString);

                String sqlDelete = " DELETE FROM dbo.BEAR_OverrideOfficeLocation WHERE TempId = " + id.ToString();
                String sqlInsert = " INSERT INTO dbo.BEAR_OverrideOfficeLocation " +
                                    " (TempId, overridden ) VALUES (" + id.ToString() + ", 1 )";
                String sql = "";
                try
                {
                    if (overrideOn)
                    {
                        sql = sqlInsert;
                    }
                    else
                    {
                        sql = sqlDelete;
                    }

                    con.Open();

                    SqlCommand command = con.CreateCommand();
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "SetOverrideOfficeFlag()", sql);
                }
                catch (NullReferenceException nre)
                {
                    Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, nre.Message, "SetOverrideOfficeFlag()", "");
                }
                finally
                {
                    if (con != null)
                        con.Close();
                }

            }
            
        }


        /// <summary>
        /// <b>Database Operation</b><br />
        /// Determines if the Office Override Flag is set by counting the number of records for a TempID in BEAR_OverrideOfficeLoacation using this SQL:<br />
        /// SELECT count(*) AS count <br />
        /// FROM dbo.BEAR_OverrideOfficeLocation (nolock)<br />
        /// WHERE TempId = " + id.ToString() "<br />
        /// AND overridden = 1 <br />
        /// </summary>
        /// <param name="id">TempID to check</param>
        /// <returns>bool indicating if flag is set or not</returns>
        public bool GetOverrideOfficeFlag(String id)
        {
            bool overrideFlagSet = false;

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["casConnectionString"].ConnectionString);

            String sql =    " SELECT count(*) AS count " + 
                            " FROM dbo.BEAR_OverrideOfficeLocation (nolock)" + 
                            " WHERE TempId = " + id.ToString() + 
                              " AND overridden = 1 ";
            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["count"].ToString().Equals("1"))
                    {
                        overrideFlagSet = true;
                    }
                }
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "GetOverrideOfficeFlag()", sql);
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, nre.Message, "GetOverrideOfficeFlag()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return overrideFlagSet;

        }


        /// <summary>
        /// <b>Database Operation</b><br />
        /// UPDATE dbo.tblTempRec<br />
        /// SET status = " + status +<br />
        /// WHERE TempId = " + TempId;<br />
        /// 
        /// The status should already be calculated and validated before this method is called.
        /// </summary>
        /// <param name="TempId">TempId to Update</param>
        /// <param name="status">Status Value</param>
        public void UpdateStatusCode(int TempId, int status)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["casConnectionString"].ConnectionString);

            String sql =  " UPDATE dbo.tblTempRec " + 
                               " SET status = " + status +
                               " WHERE TempId = " + TempId;
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
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "UpdateStatusCode()", sql);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

        }

    }

}
