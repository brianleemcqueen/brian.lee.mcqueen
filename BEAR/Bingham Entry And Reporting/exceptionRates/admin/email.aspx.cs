using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net.Mail;
using System.Collections.Generic;


namespace BEAR.exceptionRates.admin
{
    public partial class email : System.Web.UI.Page
    {
        protected String errorLogFileName = VariablesExceptionRates.ERROR_LOG_FILE_NAME;
        protected BearCode bearCode = new BearCode();
        bool production = ConfigurationManager.AppSettings["Environment"].ToString().ToLower().Equals("production");


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //populate the Billing Specialist and Office Control on the parameter page
                //accomplished with code behind to add database entries after the hardcoded "All" item
                bearCode.PopulateListBillingSpecialists(dropDownListBillingSpecialist, errorLogFileName);
                bearCode.PopulateListLocations(listboxBillingTimekeeperOffice, "timekeeper", errorLogFileName);
            }

            if (Request["Body"] != null)
            {

                if (Request["Body"].ToString().Equals("Default"))
                {
                    TextBoxEmailBodyOpeningLine.Text = VariablesExceptionRates.EMAIL_BODY_OPENING_LINE_DEFAULT;
                    TextBoxEmailBodyOpeningLine.Enabled = false;
                    TextBoxEmailBody.Text = VariablesExceptionRates.EMAIL_BODY_DEFAULT;
                    TextBoxEmailBody.Enabled = false;
                    ListBoxUdf.SelectedValue = "N";
                }
                else if (Request["Body"].ToString().Equals("Alternate"))
                {
                    TextBoxEmailBodyOpeningLine.Text = VariablesExceptionRates.EMAIL_BODY_OPENING_LINE_ALTERNATE;
                    TextBoxEmailBodyOpeningLine.Enabled = false;
                    TextBoxEmailBody.Text = VariablesExceptionRates.EMAIL_BODY_ALTERNATE;
                    TextBoxEmailBody.Enabled = false;
                    ListBoxUdf.SelectedValue = "Y";
                }
                else if (Request["Body"].ToString().Equals("DefaultRemind"))
                {
                    TextBoxEmailBodyOpeningLine.Text = VariablesExceptionRates.EMAIL_BODY_OPENING_LINE_REMINDER_DEFAULT;
                    TextBoxEmailBodyOpeningLine.Enabled = false;
                    TextBoxEmailBody.Text = VariablesExceptionRates.EMAIL_BODY_REMINDER_DEFAULT;
                    TextBoxEmailBody.Enabled = false;
                    ListBoxAttorneyReviewed.SelectedValue = "0";
                    ListBoxAttorneyReviewed.Enabled = false;
                    ListBoxUdf.SelectedValue = "N";
                    ListBoxFinalized.SelectedValue = "0";
                }
                else if (Request["Body"].ToString().Equals("AlternateRemind"))
                {
                    TextBoxEmailBodyOpeningLine.Text = VariablesExceptionRates.EMAIL_BODY_OPENING_LINE_REMINDER_ALTERNATE;
                    TextBoxEmailBodyOpeningLine.Enabled = false;
                    TextBoxEmailBody.Text = VariablesExceptionRates.EMAIL_BODY_REMINDER_ALTERNATE;
                    TextBoxEmailBody.Enabled = false;
                    ListBoxAttorneyReviewed.SelectedValue = "0";
                    ListBoxAttorneyReviewed.Enabled = false;
                    ListBoxUdf.SelectedValue = "Y";
                    ListBoxFinalized.SelectedValue = "0";
                }
            }
        }


        /// <summary>
        /// uses stored procedure: uspBMcBEARExceptionRatesDistinctAttorneys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonEmailSubmit_Click(object sender, EventArgs e)
        {
            if (!ListBoxUdf.SelectedValue.Equals(""))
            {
                if (TextBoxBillingAttorney.Text.ToUpper() != "ALL")
                {
                    if (TextBoxBillingAttorney.Text.Length == 3)
                    {
                        TextBoxBillingAttorney.Text = "00" + TextBoxBillingAttorney.Text;
                    }
                    else if (TextBoxBillingAttorney.Text.Length == 4)
                    {
                        TextBoxBillingAttorney.Text = "0" + TextBoxBillingAttorney.Text;
                    }
                }
                else
                {
                    //ensures the correct case for All - Capital A, lower ll
                    TextBoxBillingAttorney.Text = "All";
                }

                if (TextBoxClient.Text.ToUpper() != "ALL")
                {
                    //Allowing for not adding the leading zeros on Client
                    if (TextBoxClient.Text.Length == 3)
                    {
                        TextBoxClient.Text = "0000" + TextBoxClient.Text;
                    }
                    else if (TextBoxClient.Text.Length == 4)
                    {
                        TextBoxClient.Text = "000" + TextBoxClient.Text;
                    }
                    else if (TextBoxClient.Text.Length == 5)
                    {
                        TextBoxClient.Text = "00" + TextBoxClient.Text;
                    }
                    else if (TextBoxClient.Text.Length == 6)
                    {
                        TextBoxClient.Text = "0" + TextBoxClient.Text;
                    }
                }
                else
                {
                    //ensures the correct case for All - Capital A, lower ll
                    TextBoxClient.Text = "All";
                }



                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                try
                {
                    con.Open();
                    SqlCommand command = con.CreateCommand();
                    command.CommandText = "uspBMcBEARExceptionRatesDistinctAttorneys";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@billingtimekeeper", TextBoxBillingAttorney.Text);
                    command.Parameters.AddWithValue("@billingspecialist", dropDownListBillingSpecialist.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@client", TextBoxClient.Text);
                    command.Parameters.AddWithValue("@billingtimekeeperoffice", listboxBillingTimekeeperOffice.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@tcb", TCB.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@cmb", CMB.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@year", listboxCalendarYear.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@attorneyReview", ListBoxAttorneyReviewed.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@billingReview", ListBoxBillingReviewed.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@finalized", ListBoxFinalized.SelectedValue.ToString());
                    command.Parameters.AddWithValue("@attorneyUdf", ListBoxUdf.SelectedValue.ToString());

                    //0 = no timeout
                    command.CommandTimeout = 0;

                    SqlDataReader reader = command.ExecuteReader();

                    MailMessage message = new MailMessage();
                    SmtpClient client = new SmtpClient();
                    message.From = new MailAddress("\"Exception Rates (Please Do Not Reply To This Message)\"<ExceptionRateApplication@bingham.com>");

                    String billingAttorneyName = "";
                    String billingAttorneyTkid = "";
                    String emailAddress = "";
                    String emailAddressSectretary = "";
                    String sectretaryName = "";
                    String link = "";
                    String sendTo = "";
                    String sendCc = "";
                    String sendBcc = "\"Brian L. McQueen\"<brian.mcqueen@bingham.com>";

                    //email address / names used for Testing
                    String testName = "Brian L. McQueen (35486)";
                    String testEmailAddress = "brian.mcqueen@bingham.com";
                    String bodyString = "";
                    while (reader.Read())
                    {
                        emailAddress = reader["tkemail"].ToString();
                        emailAddressSectretary = reader["tkemail_sectretary1"].ToString();
                        billingAttorneyName = reader["billing_atty_name"].ToString();
                        billingAttorneyTkid = reader["billing_atty"].ToString();
                        sectretaryName = reader["sectretary1Name"].ToString();

                        //format email adress as "Name" <email@bingham.com>";
                        if (production)
                        {
                            sendTo = "\"" + billingAttorneyName + "\"<" + emailAddress + ">";
                            sendCc = "\"" + sectretaryName + "\"<" + emailAddressSectretary + ">";
                        }
                        else
                        {
                            sendTo = "\"" + testName + "\"<" + testEmailAddress + ">";
                            sendCc = "\"" + testName + "\"<" + testEmailAddress + ">";

                        }
                        if (emailAddress.Equals(""))
                        {
                            InsertEmailLogEntry(billingAttorneyTkid, billingAttorneyName, emailAddress, sectretaryName, emailAddressSectretary, "noLink", 1, 0);
                        }
                        else
                        {
                            link = "http://"
                                    + Environment.MachineName.ToString().ToLower()
                                    + "/bear/exceptionRates/exceptionRates.aspx"
                                    + "?billtk=" + billingAttorneyTkid
                                    + "&billspec=" + dropDownListBillingSpecialist.SelectedValue.ToString()
                                    + "&client=" + TextBoxClient.Text;
                            if (CheckBoxForceOfficeToAll.Checked)
                            {
                                link = link + "&billtkofc=All";
                            }
                            else
                            {
                                link = link + "&billtkofc=" + listboxBillingTimekeeperOffice.SelectedValue.ToString();
                            }

                            link = link
                                    + "&tcb=" + TCB.SelectedValue.ToString()
                                    + "&cmb=" + CMB.SelectedValue.ToString()
                                    + "&year=" + listboxCalendarYear.SelectedValue.ToString()
                                    + "&attorneyReview=" + ListBoxAttorneyReviewed.SelectedValue.ToString()
                                    + "&billingReview=" + ListBoxBillingReviewed.SelectedValue.ToString()
                                    + "&finalized=" + ListBoxFinalized.SelectedValue.ToString()
                                    + "&udf=-1"
                                    ;

                            message.To.Clear();
                            message.CC.Clear();
                            message.Bcc.Clear();

                            message.To.Add(new MailAddress(sendTo));

                            if (!emailAddressSectretary.Equals(""))
                            {
                                message.CC.Add(new MailAddress(sendCc));
                            }

                            message.Bcc.Add(new MailAddress(sendBcc));

                            message.Subject = TextBoxEmailSubject.Text + " / " + billingAttorneyName; // +"<br />" + emailAddress;
                            bodyString = "<div style='font-family: Arial; font-size: 10pt;'>"
                                            + TextBoxEmailBodyOpeningLine.Text
                                            + "<center><a href='"
                                            + link
                                            + "'>"
                                            + link
                                            + "</a></center>"
                                            + TextBoxEmailBody.Text
                                            + "</div>";


                            if (CheckBoxSentToInBody.Checked)
                            {
                                bodyString = bodyString
                                + "<div style='font-family: Arial; font-size: 8pt;'><br><br><br>"
                                + "This email was programmatically sent to Attorney: "
                                + billingAttorneyName + " / " + emailAddress;
                                if (!emailAddressSectretary.Equals(""))
                                {
                                    bodyString = bodyString + "<br />With a CC to: "
                                    + sectretaryName + " / " + emailAddressSectretary;
                                }

                                bodyString = bodyString + "</div>";
                            }

                            message.Body = bodyString;

                            message.IsBodyHtml = true;
                            try
                            {
                                client.Send(message);
                                InsertEmailLogEntry(billingAttorneyTkid, billingAttorneyName, emailAddress, sectretaryName, emailAddressSectretary, link, 0, 1);
                            }
                            catch (Exception ex)
                            {
                                Logger.QuickLog(errorLogFileName, ex.Message, "email: client.Send", "");
                                InsertEmailLogEntry(billingAttorneyTkid, billingAttorneyName, emailAddress, sectretaryName, emailAddressSectretary, link, 0, 0);
                            }
                        } //end else

                        emailAddress = "";
                        emailAddressSectretary = "";
                        billingAttorneyName = "";
                        billingAttorneyTkid = "";
                        sectretaryName = "";


                    } //end while

                    emailOptions.Visible = false;
                    LabelMessage.Text = "Finished Processing";
                    emailMessage.Visible = true;
                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(errorLogFileName, sqle.Message, "email: ButtonEmailSubmit_Click", "");
                }
                catch (NullReferenceException nre)
                {
                    Logger.QuickLog(errorLogFileName, nre.Message, "email: ButtonEmailSubmit_Click", "");
                }
                finally
                {
                    if (con != null)
                        con.Close();
                }
            }
        }


        /// <summary>
        /// uses stored procedure: uspBMcBEARExceptionRatesEmailLog
        /// </summary>
        /// <param name="tkid"></param>
        /// <param name="name"></param>
        /// <param name="emailAddress"></param>
        /// <param name="sectretaryName"></param>
        /// <param name="sectretaryEmailAddress"></param>
        /// <param name="link"></param>
        /// <param name="skipped"></param>
        /// <param name="success"></param>
        protected void InsertEmailLogEntry(String tkid, String name, String emailAddress, String sectretaryName, String sectretaryEmailAddress, String link, int skipped, int success)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARExceptionRatesEmailLog";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@tkid", tkid);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@emailAddress", emailAddress);
                command.Parameters.AddWithValue("@sectretaryName", sectretaryName);
                command.Parameters.AddWithValue("@sectretaryEmailAddress", sectretaryEmailAddress);
                command.Parameters.AddWithValue("@link", link);
                command.Parameters.AddWithValue("@timeSent", DateTime.Now);
                command.Parameters.AddWithValue("@sentBy", Page.User.Identity.Name.ToString().Substring(8));
                command.Parameters.AddWithValue("@skipped", skipped);
                command.Parameters.AddWithValue("@success", success);

                command.ExecuteNonQuery();
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "email: InsertEmailLogEntry()", "");
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

        }


        protected void ButtonToggleMessage_Click(object sender, EventArgs e)
        {
            emailMessage.Visible = false;
            emailOptions.Visible = true;
            
        }


        protected void ButtonEmailClear_Click(object sender, EventArgs e)
        {
            TextBoxBillingAttorney.Text = "All";
            TextBoxClient.Text = "All";
            listboxBillingTimekeeperOffice.SelectedIndex = 0;
            dropDownListBillingSpecialist.SelectedIndex = 0;
            TCB.SelectedIndex = 0;
            CMB.SelectedIndex = 3;
            TextBoxEmailSubject.Text = "";
            TextBoxEmailBody.Text = "";
        }

    }
}
