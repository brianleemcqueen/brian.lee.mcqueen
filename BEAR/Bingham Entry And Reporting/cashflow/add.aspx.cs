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
using System.Text;
using System.Collections.Generic;


namespace BEAR.cashflow
{
    public partial class add : System.Web.UI.Page
    {
        BearCode bearCode = new BearCode();
        String userName = ""; ///<network ID of user

        protected void Page_Load(object sender, EventArgs e)
        {
            this.userName = Page.User.Identity.Name.ToString().Substring(8);
            UtilityCashFlow utility = new UtilityCashFlow(this.userName);

            if (!utility.GetIsAdminUser() && !utility.GetIsCMUser())
            {
                Response.Redirect(VariablesCashManager.NO_ACCESS_PAGE);
            }
            
            if (!Page.IsPostBack)
            {
                PopulateOfficeLocations();
            }

        }

        #region Option Buttons
        protected void ButtonAdminOptionsClick(object sender, EventArgs e)
        {
            Response.Redirect(VariablesCashManager.ADMIN_PAGE);
        }

        protected void ButtonFilter_Click(object sender, EventArgs e)
        {
            Response.Redirect(VariablesCashManager.PARAM_PAGE);
        }
        #endregion

        #region Search and Delete
        protected void ImageButtonSearch_Click(object sender, EventArgs e)
        {
            bool barcodeExists = !IsBarcodeUnique();

            if (barcodeExists)
            {
                ButtonSave.Text = "Update";
                ImageButtonDelete.ImageUrl = "~/images/controls/delete.png";
                LabelMessage.Text = "";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                String sql = "";
                try
                {
                    sql = " select company, office, costGlCode, vendorId, vendorName, invoiceNumber, amount, currency, description "
                            + ", year(invoiceDate) as year "
                            + ", month(invoiceDate) as month "
                            + ", day(invoiceDate) as day "
                            + " from BMcBEARCashFlowManagerManualEntries "
                            + " where barcode = " + TextBoxBarcode.Text;

                    con.Open();

                    SqlCommand command = con.CreateCommand();
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        String company = reader["company"].ToString();
                        String office = reader["office"].ToString();
                        String dateFormat = "";
                        int year = Convert.ToInt16(reader["year"].ToString());
                        int month = Convert.ToInt16(reader["month"].ToString());
                        int day = Convert.ToInt16(reader["day"].ToString());
                        DateTime dbDate = new DateTime(year, month, day);

                        if (company.Equals("1"))
                        {
                            dateFormat = "MM/dd/yyyy";
                        }
                        else
                        {
                            dateFormat = "yyyy/MM/dd";
                        }

                        RadioButtonListLocation.SelectedValue = company;
                        DropDownListOffice.SelectedValue = office;
                        TextBoxGlCode.Text = reader["costGlCode"].ToString();
                        TextBoxVendorID.Text = reader["vendorId"].ToString();
                        TextBoxVendorName.Text = reader["vendorName"].ToString();
                        TextBoxInvoiceNumber.Text = reader["invoiceNumber"].ToString();
                        TextBoxDate.Text = dbDate.ToString(dateFormat);
                        TextBoxAmount.Text = reader["amount"].ToString();
                        RadioButtonListCurrency.SelectedValue = reader["currency"].ToString();
                        TextBoxDescription.Text = reader["description"].ToString();
                        TextBoxBarcode.Enabled = false;
                    }

                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(VariablesCashManager.ERROR_LOG_FILE_NAME, sqle.Message, "ImageButtonSearch_Click()", sql);
                }
                finally
                {
                    if (con != null)
                        con.Close();
                }
            }
            else
            {
                resetForm(false);
                LabelMessage.Text = "<b>Barcode Does Not Exist</b>";
            }

        }

        protected void ImageButtonDelete_Click(object sender, EventArgs e)
        {
            PanelDelete.Visible = true;
            PanelMessage.Visible = false;
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            String sql = "";
            String barcodeToDelete = TextBoxBarcode.Text;
            try
            {
                sql = " delete from BMcBEARCashFlowManagerManualEntries "
                     + " where barcode = " + barcodeToDelete;

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                command.ExecuteNonQuery();

                LabelMessage.Text = barcodeToDelete + " has been deleted.";

                resetForm();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCashManager.ERROR_LOG_FILE_NAME, sqle.Message, "ButtonDelete_Click()", sql);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            PanelDelete.Visible = false;
            LabelMessage.Text = "Delete Canceled";
            PanelMessage.Visible = true;
        }
        #endregion

        #region Form Buttons - Save and Reset
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (ButtonSave.Text.Equals("Update"))
            {
                ButtonUpdate_Click();
            }

            else
            {
                if (IsValidManualEntry(false))
                {
                    LabelMessage.Text = "";

                    String barcode = TextBoxBarcode.Text;
                    String description = TextBoxDescription.Text.Replace("'","''");
                    String amount = TextBoxAmount.Text.Replace(",", "");
                    String costGlCode = TextBoxGlCode.Text;
                    String vendorId = TextBoxVendorID.Text;
                    String vendorName = TextBoxVendorName.Text.Replace("'", "''");
                    String invoiceNumber = TextBoxInvoiceNumber.Text;
                    String invoiceDate = TextBoxDate.Text;
                    String department = bearCode.GetDepartmentCodeFromGLCode(costGlCode, VariablesCashManager.ERROR_LOG_FILE_NAME);
                    String company = RadioButtonListLocation.SelectedValue;
                    String currency = RadioButtonListCurrency.SelectedValue;
                    String office = DropDownListOffice.SelectedValue;

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                    String sqlInsert = "";
                    try
                    {
                        sqlInsert = " INSERT INTO dbo.BMcBEARCashFlowManagerManualEntries "
                                                 + " (barcode, enteredBy, description, amount, costGlCode, vendorId, vendorName, invoiceNumber "
                                                 + " ,invoiceDate, department, company, office, currency, modifiedTime) "
                                                 + " VALUES ( "
                                                 + barcode
                                                 + ", '" + Page.User.Identity.Name.ToString().Substring(8) + "' "
                                                 + ", '" + description + "' "
                                                 + ", " + amount
                                                 + ", '" + costGlCode + "' "
                                                 + ", '" + vendorId + "' "
                                                 + ", '" + vendorName + "' "
                                                 + ", '" + invoiceNumber + "' "
                                                 + ", '" + invoiceDate + "' "
                                                 + ", '" + department + "' "
                                                 + ", '" + company + "' "
                                                 + ", '" + office + "' "
                                                 + ", '" + currency + "' "
                                                 + ", '" + DateTime.Now + "' "
                                                 + " ) ";

                        con.Open();

                        SqlCommand command = con.CreateCommand();
                        command.CommandText = sqlInsert;
                        command.CommandType = CommandType.Text;

                        command.ExecuteNonQuery();

                        if (Session["barcode"] != null)
                        {
                            LabelMessage.Text = "Barcode " + Session["barcode"].ToString() + " Successfully Added.";
                            Session["barcode"] = null;
                        }
                        resetForm();

                    }
                    catch (SqlException sqle)
                    {
                        Logger.QuickLog(VariablesCashManager.ERROR_LOG_FILE_NAME, sqle.Message, "ButtonSave_Click()", sqlInsert);
                    }
                    finally
                    {
                        if (con != null)
                            con.Close();
                    }

                }

            }
        }

        protected void ButtonUpdate_Click()
        {
            if (IsValidManualEntry(true))
            {
                LabelMessage.Text = "";

                String barcode = TextBoxBarcode.Text;
                String description = TextBoxDescription.Text.Replace("'", "''");
                String amount = TextBoxAmount.Text;
                String costGlCode = TextBoxGlCode.Text;
                String vendorId = TextBoxVendorID.Text;
                String vendorName = TextBoxVendorName.Text.Replace("'", "''");
                String invoiceNumber = TextBoxInvoiceNumber.Text;
                String invoiceDate = TextBoxDate.Text;
                String department = bearCode.GetDepartmentCodeFromGLCode(costGlCode, VariablesCashManager.ERROR_LOG_FILE_NAME);
                String company = RadioButtonListLocation.SelectedValue;
                String currency = RadioButtonListCurrency.SelectedValue;
                String office = DropDownListOffice.SelectedValue;

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                String sql = "";
                try
                {
                    sql = " UPDATE dbo.BMcBEARCashFlowManagerManualEntries "
                                  + " SET " 
                                  + "  enteredBy = '" + Page.User.Identity.Name.ToString().Substring(8) + "' "
                                  + " ,description = '" + description + "' "
                                  + " ,amount = " + amount
                                  + " ,costGlCode = '" + costGlCode + "' "
                                  + " ,vendorId = '" + vendorId + "' "
                                  + " ,vendorName = '" + vendorName + "' "
                                  + " ,invoiceNumber = '" + invoiceNumber + "' "
                                  + " ,invoiceDate = '" + invoiceDate + "' "
                                  + " ,department = '" + department + "' "
                                  + " ,company = '" + company + "' "
                                  + " ,office = '" + office + "' "
                                  + " ,currency = '" + currency + "' "
                                  + " ,modifiedTime = '" + DateTime.Now + "' "
                                  + " WHERE barcode =  " + barcode;

                    con.Open();

                    SqlCommand command = con.CreateCommand();
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();

                    if (Session["barcode"] != null)
                    {
                        LabelMessage.Text = "Barcode " + Session["barcode"].ToString() + " Successfully Updated.";
                        Session["barcode"] = null;
                    }
                    
                    resetForm();

                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(VariablesCashManager.ERROR_LOG_FILE_NAME, sqle.Message, "ButtonUpdate_Click()", sql);
                }
                finally
                {
                    if (con != null)
                        con.Close();
                }

            }
        }

        protected void ButtonReset_Click(object sender, EventArgs e)
        {
            resetForm();
            LabelMessage.Text = "";
        }

        protected void resetForm()
        {
            resetForm(true);
        }

        protected void resetForm(bool includeBarcode)
        {
            if (includeBarcode)
            {
                TextBoxBarcode.Text = "";
                toggleErrorLabel(LabelBarcode, false);
            }
            RadioButtonListLocation.SelectedIndex = -1;
            DropDownListOffice.SelectedValue = "-1";
            TextBoxGlCode.Text = "";
            TextBoxVendorID.Text = "";
            TextBoxVendorName.Text = "";
            TextBoxInvoiceNumber.Text = "";
            TextBoxDate.Text = "";
            TextBoxAmount.Text = "";
            RadioButtonListCurrency.SelectedIndex = -1;
            TextBoxDescription.Text = "";
            ButtonSave.Text = "Save";
            TextBoxBarcode.Enabled = true;
            ImageButtonDelete.ImageUrl = "~/images/controls/smallwhitedot.gif";
            PanelMessage.Visible = true;
            PanelDelete.Visible = false;

            toggleErrorLabel(LabelLocation, false);
            toggleErrorLabel(LabelOffice, false);
            toggleErrorLabel(LabelGlCode, false);
            toggleErrorLabel(LabelVendorID, false);
            toggleErrorLabel(LabelVendorName, false);
            toggleErrorLabel(LabelInvoiceNumber, false);
            toggleErrorLabel(LabelInvoiceDate, false);
            toggleErrorLabel(LabelAmount, false);
            toggleErrorLabel(LabelCurrency, false);
            toggleErrorLabel(LabelDescription, false);
        }
        #endregion

        #region utility methods
        protected void PopulateOfficeLocations()
        {
            ListItem li = new ListItem("Select an Office", "-1");
            DropDownListOffice.Items.Add(li);

            List<ListItem> offices = bearCode.GetLocations(VariablesCashManager.ERROR_LOG_FILE_NAME);

            for (int i = 0; i < offices.Count; i++)
            {
                DropDownListOffice.Items.Add(offices[i]);
            }
        }

        protected bool IsValidManualEntry(bool isUpdate)
        {
            bool isValid = false;

            bool isLocationValid = false;
            bool isBarcodeValid = false;
            bool isGlCodeValid = false;
            bool isVendorIdValid = false;
            bool isVendorNameValid = false;
            bool isInvoiceNumberValid = false;
            bool isInvoiceDateValid = false;
            bool isAmountValid = false;
            bool isCurrencyValid = false;
            bool isDescriptionValid = false;
            bool isOfficeValid = false;

            if (RadioButtonListLocation.SelectedIndex != -1)
            {
                isLocationValid = true;
                toggleErrorLabel(LabelLocation, false);
            }

            if (TextBoxBarcode.Text.Length == 7 && bearCode.IsNumber(TextBoxBarcode.Text, true) )
            {
                if ((!isUpdate && IsBarcodeUnique()) || isUpdate)
                {
                    isBarcodeValid = true;
                    toggleErrorLabel(LabelBarcode, false);
                    Session["barcode"] = TextBoxBarcode.Text;
                }
            }

            if (!DropDownListOffice.SelectedValue.Equals("-1") && DropDownListOffice.SelectedIndex != -1)
            {
                isOfficeValid = true;
                toggleErrorLabel(LabelOffice, false);
            }
            
            if (TextBoxGlCode.Text.Length == 5 && bearCode.IsNumber(TextBoxGlCode.Text, true) )
            {
                isGlCodeValid = true;
                toggleErrorLabel(LabelGlCode, false);
            }

            if (bearCode.IsNumber(TextBoxVendorID.Text, true))
            {
                isVendorIdValid = true;
                toggleErrorLabel(LabelVendorID, false);
            }

            if (TextBoxVendorName.Text.Length < 100)
            {
                isVendorNameValid = true;
                toggleErrorLabel(LabelVendorName, false);
            }

            if (TextBoxInvoiceNumber.Text.Length < 30)
            {
                isInvoiceNumberValid = true;
                toggleErrorLabel(LabelInvoiceNumber, false);
            }

            if (bearCode.IsDate(TextBoxDate.Text))
            {
                isInvoiceDateValid = true;
                toggleErrorLabel(LabelInvoiceDate, false);
            }

            if (bearCode.IsNumber(TextBoxAmount.Text))
            {
                isAmountValid = true;
                toggleErrorLabel(LabelAmount, false);
            }

            if (RadioButtonListCurrency.SelectedIndex != -1)
            {
                isCurrencyValid = true;
                toggleErrorLabel(LabelCurrency, false);
            }

            if (TextBoxDescription.Text.Length < 500)
            {
                isDescriptionValid = true;
                toggleErrorLabel(LabelDescription, false);
            }

            if (isLocationValid && isOfficeValid && isGlCodeValid && isVendorIdValid && isVendorNameValid &&
                 isBarcodeValid && isInvoiceNumberValid && isInvoiceDateValid && isAmountValid &&
                 isCurrencyValid && isDescriptionValid)
            {
                isValid = true;
                LabelMessage.Text = "";
            }
            else
            {
                StringBuilder message = new StringBuilder("<table>");
                message.Append("<tr><td colspan='2'><b><u>Error Messages:</u></b></td></tr>");

                if (!isLocationValid)
                {
                    message.Append(newTableRow("Location:", "Please Select a Location"));
                    toggleErrorLabel(LabelLocation, true);
                }

                if (!isBarcodeValid)
                {
                    if (TextBoxBarcode.Text.Equals(""))
                    {
                        message.Append(newTableRow("Barcode:", "Barcode may not be blank"));
                    }
                    else if (!IsBarcodeUnique())
                    {
                        message.Append(newTableRow("Barcode:", "This barcode already exists"));
                    }
                    else
                    {
                        message.Append(newTableRow("Barcode:", "Must be a number 7 digits long"));
                    }
                    toggleErrorLabel(LabelBarcode, true);
                }

                if (!isOfficeValid)
                {
                    message.Append(newTableRow("Office:", "Please select an Office"));
                    toggleErrorLabel(LabelOffice, true);
                }

                if (!isGlCodeValid)
                {
                    message.Append(newTableRow("GL Code:", "Must be a number 5 digits long"));
                    toggleErrorLabel(LabelGlCode, true);
                }

                if (!isVendorIdValid)
                {
                    message.Append(newTableRow("Vendor ID:", "Must use numeric characters only"));
                    toggleErrorLabel(LabelVendorID, true);
                }

                if (!isVendorNameValid)
                {
                    message.Append(newTableRow("Vendor Name:", "Maximum length of 100 characters"));
                    toggleErrorLabel(LabelVendorName, true);
                }

                if (!isInvoiceNumberValid)
                {
                    message.Append(newTableRow("Invoice #:", "Must be under 30 characters"));
                    toggleErrorLabel(LabelInvoiceNumber, true);
                }

                if (!isInvoiceDateValid)
                {
                    message.Append(newTableRow("Invoice Date:", "Please enter a valid Date in mm/dd/yyyy or yyyy/mm/dd format"));
                    toggleErrorLabel(LabelInvoiceDate, true);
                }

                if (!isAmountValid)
                {
                    message.Append(newTableRow("Amount:", "Please enter a valid amount"));
                    toggleErrorLabel(LabelAmount, true);
                }

                if (!isCurrencyValid)
                {
                    message.Append(newTableRow("Currency:", "Please select a currency"));
                    toggleErrorLabel(LabelCurrency, true);
                }

                if (!isDescriptionValid)
                {
                    message.Append(newTableRow("Description:", "Must be under 500 characters"));
                    toggleErrorLabel(LabelDescription, true);
                }

                message.Append("</table>");

                LabelMessage.Text = message.ToString();

            }
            
            return isValid;
        }

        protected String newTableRow(String Cell1, String Cell2)
        {
            return "<tr><td>" + Cell1+ "</td><td>" + Cell2 + "</td></tr>";
        }

        protected void toggleErrorLabel(Label lbl, bool isError)
        {
            if (isError)
            {
                lbl.Attributes.Add("Style", "color: Red;");
            }
            else
            {
                lbl.Attributes.Add("Style", "color: Black;");
            }
        }

        protected bool IsBarcodeUnique()
        {
            bool isBarcodeUnique = false;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            String sql = "";
            try
            {
                sql =    " select count(*) as barcodeCount" 
                        + " from BMcBEARCashFlowManagerManualEntries "
                        + " where barcode = " + TextBoxBarcode.Text;

                con.Open();

                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int barcodeCount = Convert.ToInt16(reader["barcodeCount"].ToString());
                    if (barcodeCount == 0)
                    {
                        isBarcodeUnique = true;
                    }
                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCashManager.ERROR_LOG_FILE_NAME, sqle.Message, "IsBarcodeUnique()", sql);
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            return isBarcodeUnique;

        }
        #endregion
    }
}
