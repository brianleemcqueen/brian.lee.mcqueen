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
using System.Collections.Generic;

namespace BEAR.cashflow
{
    public partial class searchCashFlow : System.Web.UI.Page
    {
        BearCode bearCode = new BearCode();
        protected bool isAdminUser = false;
        protected bool isDeptUser = false;
        protected bool isCMUser = false;
        protected String location = "";
        protected String userName = ""; ///<network ID of user
        protected String department = "";
        UtilityCashFlow utility;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.userName = Page.User.Identity.Name.ToString().Substring(8);
            //this.userName = "bergcl";
            utility = new UtilityCashFlow(this.userName);
            SetUpUser();

            TextBoxInvoiceNumber.Attributes.Add("onFocus", "TextBoxOnFocusAllToBlank('TextBoxInvoiceNumber')");
            TextBoxVendorID.Attributes.Add("onFocus", "TextBoxOnFocusAllToBlank('TextBoxVendorID')");
            TextBoxVendorName.Attributes.Add("onFocus", "TextBoxOnFocusAllToBlank('TextBoxVendorName')");
            TextBoxBarcode.Attributes.Add("onFocus", "TextBoxOnFocusAllToBlank('TextBoxBarcode')");

            if (!Page.IsPostBack)
            {
                bearCode.PopulateDepartments(CheckBoxListDepartments, VariablesCashManager.ERROR_LOG_FILE_NAME);
                SetUpUser();
                PopulateOfficeLocations();
            }
        }

        protected void ButtonClose_Click(object sender, EventArgs e)
        {
            Response.Redirect(VariablesCashManager.DATA_PAGE);
        }

        protected void ButtonRun_Click(object sender, EventArgs e)
        {
            String invoice = "All";
            if (!TextBoxInvoiceNumber.Text.Replace(" ", "").Equals(""))
            {
                invoice = TextBoxInvoiceNumber.Text;
            }

            String barcode = "All";
            if (!TextBoxBarcode.Text.Replace(" ", "").Equals(""))
            {
                barcode = TextBoxBarcode.Text;
            }

            String vendorId = "All";
            if (!TextBoxVendorID.Text.Replace(" ", "").Equals(""))
            {
                vendorId = TextBoxVendorID.Text;
            }

            String vendorName = "All";
            if (!TextBoxVendorName.Text.Replace(" ", "").Equals(""))
            {
                vendorName = TextBoxVendorName.Text;
            }

            String url =
                VariablesCashManager.DATA_PAGE
            + "?inv=" + invoice
            + "&vid=" + vendorId
            + "&vname=" + vendorName
            + "&dp=" + RadioButtonListDeptPriority.SelectedValue
            + "&cmp=" + RadioButtonListCMPriority.SelectedValue
            + "&cur=" + RadioButtonListCurrency.SelectedValue
            + "&src=" + RadioButtonListSource.SelectedValue
            + "&ofc=" + DropDownListOffice.SelectedValue
            + "&bc=" + barcode;

            if (isAdminUser || isCMUser || (isDeptUser && (this.location.Equals("All") || this.location.Equals("2"))))
            {
                String loc = RadioButtonListLocation.SelectedValue;
                if (loc.Equals("Def"))
                {
                    loc = this.location;
                }

                url = url
                + "&loc=" + loc;
            }
            if (isAdminUser || isCMUser)
            {
                url = url
                + "&dept=" + GetSelectedDepartments()
                + "&pay=" + RadioButtonListPayFlag.SelectedValue
                + "&mthd=" + RadioButtonListPaymentMethod.SelectedValue;
            }

            Response.Redirect(url);
        }

        protected void buttonReportClick(object sender, EventArgs e)
        {
            String loc = RadioButtonListLocation.SelectedValue;
            if (loc.Equals("Def") || (isDeptUser && !(this.location.Equals("All") || this.location.Equals("2"))))
            {
                loc = this.location;
            }

            String ReportName = "";
            if (this.isDeptUser)
            {
                ReportName = VariablesCashManager.REPORT_SERVICES_DEPT_REPORT_NAME;
            }
            else
            {
                ReportName = VariablesCashManager.REPORT_SERVICES_CM_REPORT_NAME;
            }

            String invoice = "All";
            if (!TextBoxInvoiceNumber.Text.Replace(" ", "").Equals(""))
            {
                invoice = TextBoxInvoiceNumber.Text;
            }

            String barcode = "All";
            if (!TextBoxBarcode.Text.Replace(" ", "").Equals(""))
            {
                barcode = TextBoxBarcode.Text;
            }

            String vendorId = "All";
            if (!TextBoxVendorID.Text.Replace(" ", "").Equals(""))
            {
                vendorId = TextBoxVendorID.Text;
            }

            String vendorName = "All";
            if (!TextBoxVendorName.Text.Replace(" ", "").Equals(""))
            {
                vendorName = TextBoxVendorName.Text;
            }

            String MachineName = Environment.MachineName.ToString();

            String url =
                System.Configuration.ConfigurationSettings.AppSettings["ReportServer"]
                + VariablesCashManager.REPORT_FOLDER
                + "/"
                + ReportName
                + "&rc:Parameters=false&rs:Command=Render&rs:ClearSession=true"
                + "&ApplicationServer="
                + MachineName
                + "&inv=" + invoice
                + "&vid=" + vendorId
                + "&vname=" + vendorName
                + "&dp=" + RadioButtonListDeptPriority.SelectedValue
                + "&src=" + RadioButtonListSource.SelectedValue
                + "&cur=" + RadioButtonListCurrency.SelectedValue
                + "&loc=" + loc
                + "&dept=" + this.department
                + "&ofc=" + DropDownListOffice.SelectedValue
                +"&bc=" + barcode;
            if (isAdminUser || isCMUser)
            {
                url = url 
                +"&cmp=" + RadioButtonListCMPriority.SelectedValue
                + "&dept=" + GetSelectedDepartments()
                + "&pay=" + RadioButtonListPayFlag.SelectedValue
                + "&mthd=" + RadioButtonListPaymentMethod.SelectedValue;
            }

            Response.Redirect(url);

            }

        protected void CheckBoxDepartmentsAll_CheckChanged(object sender, EventArgs e)
        {
            if (CheckBoxDepartmentsAll.Checked)
            {
                PanelDepartments.Visible = false;
            }
            else
            {
                PanelDepartments.Visible = true;
            }
        }

        protected void PopulateOfficeLocations()
        {
            ListItem li = new ListItem("All Offices", "All");
            DropDownListOffice.Items.Add(li);

            BearCode bearCode = new BearCode();
            List<ListItem> offices = bearCode.GetLocations(VariablesCashManager.ERROR_LOG_FILE_NAME);

            for (int i = 0; i < offices.Count; i++)
            {
                DropDownListOffice.Items.Add(offices[i]);
            }
        }

        protected String GetSelectedDepartments()
        {
            String departments = "";
            bool firstTimeLooping = true;

            if (CheckBoxDepartmentsAll.Checked)
            {
                departments = "All";
            }
            else
            {
                for (int i = 0; i < CheckBoxListDepartments.Items.Count; i++)
                {
                    if (CheckBoxListDepartments.Items[i].Selected)
                    {
                        if (firstTimeLooping)
                        {
                            departments = departments + CheckBoxListDepartments.Items[i].Value;
                            firstTimeLooping = false;
                        }
                        else
                        {
                            departments = departments + "," +  CheckBoxListDepartments.Items[i].Value;
                        }
                    }
                }
            }
            return departments;
        }

        /// <summary>
        /// Sets the User's location and access based on what is stored in the database.  See UtilityCashFlow
        /// </summary>
        protected void SetUpUser()
        {
            this.isAdminUser = utility.GetIsAdminUser();
            this.isCMUser = utility.GetIsCMUser();
            this.isDeptUser = utility.GetIsDeptUser();
            this.location = utility.GetLocation();
            this.department = utility.GetDepartment();

            if (!this.isAdminUser && !this.isCMUser && !this.isDeptUser)
            {
                Response.Redirect(VariablesCashManager.NO_ACCESS_PAGE);
            }

            if (isDeptUser)
            {
                LabelCMPriority.Visible = false;
                RadioButtonListCMPriority.Visible = false;

                if (this.location.Equals("All"))
                {
                    RadioButtonListLocation.Visible = true;
                    LabelLocation.Visible = true;
                }
                else if (this.location.Equals("2"))
                {
                    RadioButtonListLocation.Visible = true;
                    LabelLocation.Visible = true;
                    RadioButtonListLocation.Items.FindByValue("All").Attributes.Add("Style", "display: none");
                    RadioButtonListLocation.Items.FindByValue("1").Attributes.Add("Style", "display: none");
                }
                else
                {
                    LabelLocation.Visible = false;
                    RadioButtonListLocation.Visible = false;
                }

                LabelPayFlag.Visible = false;
                RadioButtonListPayFlag.Visible = false;

                LabelPaymentMethod.Visible = false;
                RadioButtonListPaymentMethod.Visible = false;

                LabelDepartments.Visible = false;
                CheckBoxDepartmentsAll.Visible = false;
            }
            
            if(isAdminUser || isCMUser)
            {
                RadioButtonListLocation.Visible = true;
                LabelLocation.Visible = true;
            }

        }

    }
}
