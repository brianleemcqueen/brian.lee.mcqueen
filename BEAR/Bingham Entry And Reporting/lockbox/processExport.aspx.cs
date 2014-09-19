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
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;


namespace BEAR.lockbox
{
    public partial class processExport : System.Web.UI.Page
    {
        private String exportPath = "";
        private String userId = "";
        double TotalInvoiceAmt = 0D;
        bool isFinal = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            userId = Page.User.Identity.Name.ToString().Substring(8);
            if (!Page.IsPostBack)
            {
                ExportLockbox();
            }
        }


        protected void ButtonHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }


        protected void ExportLockbox()
        {
            String exportFile = "test.txt";
            String groupId = "";
            String systemCreditDate = "";
            int importCounter = 0;
            String emailBody = "";
            String emailSubject = "Lockbox: Test Export Notification";

            String exportType = "";
            if (Session["exportType"] != null)
            {
                exportType = Session["exportType"].ToString();
                if (exportType.ToUpper().Equals("FINAL"))
                {
                    isFinal = true;
                    emailSubject = "Lockbox: Final Export Notification";
                }
            }

            

            if (Session["groupID"] != null)
            {
                groupId = Session["groupID"].ToString();
                if (! this.isFinal)
                {
                    exportFile = groupId + "_" + userId + ".txt";
                }
                else
                {
                    exportFile = groupId + ".txt";
                }
            }


            this.exportPath = VariablesLockbox.EXPORT_PATH + exportType + "\\";


            StreamWriter sw = File.CreateText(this.exportPath + exportFile);

            SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["lockboxConnectionString"].ConnectionString);

            try
            {
                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = "SelectImportedFieldsForExport";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@GroupID", groupId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    systemCreditDate = reader["SystemCreditDate"].ToString();
                    sw.WriteLine(
                          "|" + systemCreditDate //need To Format
                        + "|" + reader["InvoiceNo"].ToString() 
                        + "|" + reader["DocNo"].ToString() 
                        + "|" + reader["LedgerCode"].ToString() 
                        + "|" + reader["InvoiceAmount"].ToString() 
                        + "|" + reader["InvoiceDesc"].ToString() 
                        + "|" + reader["BankInterfaceIndex"].ToString() 
                        + "|" + reader["UserResearch"].ToString() 
                        + "|" + reader["OverridePriorityLedgerCode"].ToString() 
                        + "|" + reader["MultiPayerClientName"].ToString() 
                        + "|"
                        );

                    TotalInvoiceAmt = TotalInvoiceAmt + double.Parse(reader["InvoiceAmount"].ToString());
                    importCounter++;
                }

                reader.Close();

                String ExportFieldValue = "TestExport";
                if (this.isFinal)
                {
                    ExportFieldValue = "Export";
                }

                SqlCommand command2 = con.CreateCommand();
                command2.CommandType = CommandType.StoredProcedure;
                command2.CommandText = "InsertLogRecord";
                command2.Parameters.AddWithValue("@GroupID", groupId);
                command2.Parameters.AddWithValue("@ProcessDate", DateTime.Now);
                command2.Parameters.AddWithValue("@CreditDate", systemCreditDate);
                command2.Parameters.AddWithValue("@UserId", this.userId);
                command2.Parameters.AddWithValue("@Process", ExportFieldValue);
                command2.Parameters.AddWithValue("@RecordCount", importCounter);
                command2.Parameters.AddWithValue("@Total", TotalInvoiceAmt);
                command2.Parameters.AddWithValue("@BatchCount", -1);
                command2.Parameters.AddWithValue("@CheckCount", -1);
                command2.Parameters.AddWithValue("@InvoiceCount", importCounter);
                command2.Parameters.AddWithValue("@FileName", this.exportPath + exportFile);
                command2.Parameters.AddWithValue("@TotalCheckAmt", -1);

                command2.ExecuteNonQuery();

                //Build email body

                emailBody = "<table>"
                    + "<tr><td>"
                    + "Group ID:</td><td>" + groupId
                    + "</td></tr><tr><td>"
                    + "Export Date/Time:</td><td>" + DateTime.Now
                    + "</td></tr><tr><td>"
                    + "Credit Date:</td><td>" + DateTime.Parse(systemCreditDate).ToString("MM/dd/yyyy")
                    + "</td></tr><tr><td>"
                    + "Records Exported:</td><td>" + importCounter
                    + "</td></tr><tr><td>"
                    + "Total Invoice Amount:</td><td>" + TotalInvoiceAmt
                    + "</td></tr><tr><td>"
                    + "</td></tr></table>"
                    ;


/*
GroupID:      012209AAA

ProcessDate:   1/22/2009
Credit Date:   1/22/2009
Import FilePath:  G:/lockbox/imported/012209AAA

IMPORT
Batch Count:        1
Check Count:        13
Invoice Count:      32
Total Invoice Amount:           $156,512.72
Total Check Amount:             $156,512.72
User:               chinm


TEST EXPORT
Process Date: 1/22/2009
Export FilePath:  \\bmelite\elite\lockbox\TestExport\TEST012209AAAchinm.txt
Record Count:        32
Amount:              $156,512.72
User:                chinm
*/


                if (this.isFinal)
                {
                    SqlCommand command3 = con.CreateCommand();
                    command3.CommandText = "InsertImportedIntoExported";
                    command3.CommandType = CommandType.StoredProcedure;
                    command3.Parameters.AddWithValue("@GroupID", groupId);
                    command3.ExecuteNonQuery();

                    SqlCommand command4 = con.CreateCommand();
                    command4.CommandText = "DeleteImportedRecords";
                    command4.CommandType = CommandType.StoredProcedure;
                    command4.Parameters.AddWithValue("@GroupID", groupId);
                    command4.ExecuteNonQuery();

                }


            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesLockbox.ERROR_LOG_FILE_NAME, sqle.Message, "ExportLockbox()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
                    sw.Close();
            }

            //BearCode.SendEmail(VariablesLockbox.ERROR_LOG_FILE_NAME, emailBody, emailSubject, VariablesLockbox.EMAIL_FROM_ADDRESS, userId, true);

            LabelProcessingMessage.Text = "Finished Processing " + exportType + " Export File.<br />";
            LabelProcessingOutput.Text = "File Generated: " + this.exportPath + exportFile;
            LabelProcessingOutput2.Text = "Records Exported: " + importCounter + "<br />Amount Exported: " + TotalInvoiceAmt;

            ImageButtonOpenExportLocation.Visible = true;


        }




        protected void ImageButtonOpenExportLocation_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", this.exportPath);
        }




    }



}
