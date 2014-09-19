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

namespace BEAR.cashflow
{
    public partial class cashflow : System.Web.UI.Page
    {
        #region Global Variables
        protected UtilityCashFlow utility;

        protected int columnPayFlag = VariablesCashManager.COLUMN_PAY_FLAG;
        protected int columnSource = VariablesCashManager.COLUMN_SOURCE;
        protected int columnDepartment = VariablesCashManager.COLUMN_DEPARTMENT;
        protected int columnPriorityDept = VariablesCashManager.COLUMN_PRIORITY_DEPT;
        protected int columnPriorityCM = VariablesCashManager.COLUMN_PRIORITY_CM;
        protected int columnBarcode = VariablesCashManager.COLUMN_BARCODE;
        protected int columnEnteredBy = VariablesCashManager.COLUMN_ENTERED_BY;
        protected int columnDescription = VariablesCashManager.COLUMN_DESCRIPTION;
        protected int columnAmount = VariablesCashManager.COLUMN_AMOUNT;
        protected int columnVendorName = VariablesCashManager.COLUMN_VENDOR_NAME;
        protected int columnInvoiceInformation = VariablesCashManager.COLUMN_INVOICE_INFORMATION;
        protected int columnNotes = VariablesCashManager.COLUMN_NOTES;
        protected int columnPaymentMethod = VariablesCashManager.COLUMN_PAYMENT_METHOD;
        protected int columnLocation = VariablesCashManager.COLUMN_LOCATION;
        protected int columnOffice = VariablesCashManager.COLUMN_OFFICE;

        protected int columnInvoiceNumberHidden = VariablesCashManager.COLUMN_INVOICE_NUMBER_HIDDEN;
        protected int columnInvoiceDateHidden = VariablesCashManager.COLUMN_INVOICE_DATE_HIDDEN;
        protected int columnVendorIdHidden = VariablesCashManager.COLUMN_VENDOR_ID_HIDDEN;
        protected int columnVendorNameHidden = VariablesCashManager.COLUMN_VENDOR_NAME_HIDDEN;
        protected int columnAmountHidden = VariablesCashManager.COLUMN_AMOUNT_HIDDEN;
        protected int columnIdHidden = VariablesCashManager.COLUMN_ID_HIDDEN;
        protected int columnPaymentMethodHidden = VariablesCashManager.COLUMN_PAYMENT_METHOD_HIDDEN;
        protected int columnPriorityCMHidden = VariablesCashManager.COLUMN_PRIORITY_CM_HIDDEN;
        protected int columnPriorityDeptHidden = VariablesCashManager.COLUMN_PRIORITY_DEPT_HIDDEN;
        protected int columnCurrencyHidden = VariablesCashManager.COLUMN_CURRENCY_HIDDEN;
        protected int columnUpdateTimeHidden = VariablesCashManager.COLUMN_UPDATE_TIME_HIDDEN;

        protected String errorLogFileName = VariablesCashManager.ERROR_LOG_FILE_NAME;

        #endregion
        
         protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;
            String userName = Page.User.Identity.Name.ToString().Substring(8); 
            //userName = "bergcl";

            utility = new UtilityCashFlow(userName);

            SetUpUser();

            int totalRows = 0;

            if (Request["loc"] != null)
            {
                utility.SetLocation(Request["loc"].ToString());
            }
            if (Request["dept"] != null)
            {
                utility.SetDepartment(Request["dept"].ToString());
            }

            try
            {
                totalRows = dataGridView.Rows.Count;
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "Page_Load()", "int totalRows = dataGridView.Rows.Count");
            }

            if (utility.GetIsDeptUser())
            {
                data.Attributes.Add("class", "cashManagerData cashManagerDataDept");
                options.Attributes.Add("class", "optionsCashManagerDeptUser");
            }
            else if (utility.GetIsCMUser())
            {
                data.Attributes.Add("class", "cashManagerData cashManagerDataCM");
                options.Attributes.Add("class", "optionsCashManagerCMUser");
            }
            else
            {
                data.Attributes.Add("class", "cashManagerData cashManagerDataAdmin");
                options.Attributes.Add("class", "optionsCashManagerAdminUser");
            }
            utility.SetRowChanged(totalRows);
            
            hasPagerRow.Value = "false";

            //Set Tool Tips
            buttonSave.Attributes.Add("onmouseover", "Tip('" + VariablesCashManager.TOOLTIP_SAVE_BUTTON + "')");
            buttonSave.Attributes.Add("onmouseout", "UnTip()");
            exitButton.Attributes.Add("onmouseout", "UnTip()");

            if (utility.GetIsAdminUser() || utility.GetIsCMUser())
            {
                AmountSelectedHidden.Value = Convert.ToString(GetTotalChecked());
                if (Session["amountToPay"] != null)
                {
                    AmountToPay.Value = Session["amountToPay"].ToString();
                }
                else
                {
                    AmountToPay.Value = "0";
                }
            }
            else
            {
                divTotalToPay.Visible = false;
                buttonAdminOptions.Visible = false;
            }

            LabelCurrentPage.Text = "Page " + Convert.ToString(dataGridView.PageIndex + 1) + " of " + dataGridView.PageCount;

            if (!Page.IsPostBack)
            {
                LogUserAccess();
            }

        }

        /// <summary>
         /// Called before the page is rendered.  This is used for the save functionality.
         /// See Code for SQL - this is a large dynamic sql that is built using C#.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {

            String messageOutput = "";
            bool updateMessage = false;
            if (Page.IsPostBack)
            {
                String sessionResetCookie = "false";
                if (Session["resetCookie"] != null)
                {
                    sessionResetCookie = Session["resetCookie"].ToString();
                }

                if (sessionResetCookie.Equals("true"))
                {
                    Session["resetCookie"] = "false";
                }

                int totalRows = dataGridView.Rows.Count;

                for (int r = 0; r < totalRows; r++)
                {
                    if (utility.GetRowChanged()[r])
                    {
                        updateMessage = true;

                        GridViewRow thisGridViewRow = dataGridView.Rows[r];

                        String barcode = "";
                        String source = "";
                        String invoiceNumber = "";
                        String invoiceDate = "";
                        String vendorNumber = "";
                        int payFlag = 0;
                        decimal amount = 0;
                        //String cmPriority = "4";
                        //String deptPriority = "4";
                        String cmPriority = "2";
                        String deptPriority = "2";
                        String cmNotes = "";
                        String deptNotes = "";
                        String payMethod = "";
                        String updateTimeAtLoad = "";

                        try
                        {
                            barcode = thisGridViewRow.Cells[columnBarcode].Text == "-" ? "-1" : thisGridViewRow.Cells[columnBarcode].Text;
                            source = thisGridViewRow.Cells[columnSource].Text;
                            invoiceNumber = thisGridViewRow.Cells[columnInvoiceNumberHidden].Text.Replace("'", "''").Replace("&nbsp;","");
                            invoiceDate = thisGridViewRow.Cells[columnInvoiceDateHidden].Text;
                            vendorNumber = thisGridViewRow.Cells[columnVendorIdHidden].Text;
                            payFlag = ((CheckBox)thisGridViewRow.FindControl("review1CB")).Checked == true ? 1 : 0;
                            updateTimeAtLoad = thisGridViewRow.Cells[columnUpdateTimeHidden].Text;

                            try
                            {
                                amount = Convert.ToDecimal(thisGridViewRow.Cells[columnAmountHidden].Text);
                            }
                            catch (FormatException fe)
                            {
                                Logger.QuickLog(VariablesCashManager.ERROR_LOG_FILE_NAME, "Page_PreRender()", fe.Message, "converting amount from text to decimal: " + thisGridViewRow.Cells[columnAmountHidden].Text);
                            }
                            if (utility.GetIsDeptUser())
                            {
                                cmPriority = thisGridViewRow.Cells[columnPriorityCMHidden].Text;
                                payMethod = thisGridViewRow.Cells[columnPaymentMethodHidden].Text.Replace("&nbsp;", "");
                            }
                            else
                            {
                                cmPriority = ((RadioButtonList)thisGridViewRow.FindControl("cmPriorityRadioButtonList")).SelectedValue;
                                payMethod = ((DropDownList)thisGridViewRow.FindControl("DropDownListPayMthd")).SelectedValue;

                            }
                            if (utility.GetIsCMUser())
                            {
                                deptPriority = thisGridViewRow.Cells[columnPriorityDeptHidden].Text == "-1" ? "null" : thisGridViewRow.Cells[columnPriorityDeptHidden].Text;
                            }
                            else
                            {
                                deptPriority = ((RadioButtonList)thisGridViewRow.FindControl("deptPriorityRadioButtonList")).SelectedValue;
                            }
                            cmNotes = ((TextBox)thisGridViewRow.FindControl("cmNotesTB")).Text.Replace("'", "''");
                            deptNotes = ((TextBox)thisGridViewRow.FindControl("deptNotesTB")).Text.Replace("'", "''");

                        }
                        catch (FormatException)
                        {
                            //this is to catch when set to a nonNumber
                            //non numbers are converted to zero
                        }
                        catch (ArgumentOutOfRangeException ae)
                        {
                            //when using paging
                            Logger.QuickLog(errorLogFileName, ae.Message, "Page_PreRender()", "");
                        }

                        SqlConnection con = null;


                        String sqlDateCheck = " SELECT case when updateTime > '" + updateTimeAtLoad + "' then 0 else 1 end as okToSave " 
                                                      + " FROM BMcBEARCashFlowManager " 
                                                      + " WHERE barcode = " + barcode
                                                          + " AND dataSource = '" + source + "' "
                                                          + " AND invoiceNumber = '" + invoiceNumber + "' "
                                                          + " AND invoiceDate = '" + invoiceDate + "' "
                                                          + " AND vendorNumber = '" + vendorNumber + "' "
                                                          + " AND amount = " + amount;


                        String sqlArchive = " INSERT INTO dbo.BMcBEARCashFlowManagerArchive "
                                          + " SELECT ID "
                                          + " ,barcode "
                                          + " ,dataSource "
                                          + " ,invoiceNumber "
                                          + " ,invoiceDate "
                                          + " ,vendorNumber "
                                          + " ,payFlag "
                                          + " ,amount "
                                          + " ,priorityCM "
                                          + " ,priorityDept"
                                          + " ,notesCM "
                                          + " ,notesDept "
                                          + " ,paymentMethod "
                                          + " ,updatedBy "
                                          + " ,updateTime "
                                          + " FROM dbo.BMcBEARCashFlowManager (nolock)"
                                          + " WHERE barcode = " + barcode
                                              + " AND dataSource = '" + source + "' "
                                              + " AND invoiceNumber = '" + invoiceNumber + "' "
                                              + " AND invoiceDate = '" + invoiceDate + "' "
                                              + " AND vendorNumber = '" + vendorNumber + "' "
                                              + " AND amount = " + amount;

                        String sqlSelectCount = " SELECT count(*) as count "
                                                       + " FROM dbo.BMcBEARCashFlowManager (nolock) "
                                                      + " WHERE barcode = " + barcode
                                                          + " AND dataSource = '" + source + "' "
                                                          + " AND invoiceNumber = '" + invoiceNumber + "' "
                                                          + " AND invoiceDate = '" + invoiceDate + "' "
                                                          + " AND vendorNumber = '" + vendorNumber + "' "
                                                          + " AND amount = " + amount;

                        String sqlUpdate = " UPDATE BMcBEARCashFlowManager "
                                         + " SET updatedBy = '" + utility.GetUserName() + "' "
                                              + " , updateTime = '" + DateTime.Now + "' ";
                        if (!utility.GetIsDeptUser())
                        {
                            sqlUpdate = sqlUpdate
                                              + " , payFlag = " + payFlag
                                              + " , paymentMethod = '" + payMethod + "' "
                                              + " , notesCM = '" + cmNotes + "' "
                                              + " , priorityCM = " + cmPriority;
                        }
                        if (!utility.GetIsCMUser())
                        {
                            sqlUpdate = sqlUpdate
                                              + " , priorityDept = " + deptPriority
                                              + " , notesDept = '" + deptNotes + "' ";
                        }
                        sqlUpdate = sqlUpdate
                                          + " WHERE barcode = " + barcode
                                              + " AND dataSource = '" + source + "' "
                                              + " AND invoiceNumber = '" + invoiceNumber + "' "
                                              + " AND invoiceDate = '" + invoiceDate + "' "
                                              + " AND vendorNumber = '" + vendorNumber + "' "
                                              + " AND amount = " + amount;

                        String sqlInsert = " INSERT INTO dbo.BMcBEARCashFlowManager ( "
                                                    + " barcode, dataSource, invoiceNumber, invoiceDate, vendorNumber, "
                                                    + " payFlag, amount, priorityCM, priorityDept, notesCM, notesDept, "
                                                    + " paymentMethod, updatedBy, updateTime "
                                                + " ) VALUES ( "
                                                    + barcode
                                                    + ", '" + source + "' "
                                                    + ", '" + invoiceNumber + "' "
                                                    + ", '" + invoiceDate + "' "
                                                    + ", '" + vendorNumber + "' "
                                                    + ", " + payFlag
                                                    + ", " + amount;

                        if (cmPriority.Equals("-1"))
                        {
                            sqlInsert = sqlInsert
                                                    + ", NULL ";
                        }
                        else
                        {
                            sqlInsert = sqlInsert
                                                    + ", " + cmPriority;
                        }
                        sqlInsert = sqlInsert
                                                    + ", " + deptPriority
                                                    + ", '" + cmNotes + "' "
                                                    + ", '" + deptNotes + "' "
                                                    + ", '" + payMethod + "' "
                                                    + ", '" + utility.GetUserName() + "' "
                                                    + ", '" + DateTime.Now + "' "
                                                + " ) ";

                        try
                        {
                            con = new SqlConnection(
                                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                            con.Open();
                            SqlCommand command = con.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = sqlArchive;
                            command.ExecuteNonQuery();
                            int recordCount = 0;
                            try
                            {
                                command.CommandText = sqlSelectCount;
                                SqlDataReader readerCount = command.ExecuteReader();
                                if (readerCount.Read())
                                {
                                    recordCount = Convert.ToInt16(readerCount["count"].ToString());
                                }
                                readerCount.Close();
                            }
                            catch (SqlException sqleSelectCount)
                            {
                                Logger.QuickLog(errorLogFileName, sqleSelectCount.Message, "Page_PreRender()", sqlSelectCount);
                            }
                            catch (Exception)
                            {
                            }

                            if (recordCount > 0)
                            {
                                int okToSave = 0;
                                try
                                {
                                    command.CommandText = sqlDateCheck;
                                    SqlDataReader reader = command.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        okToSave = Convert.ToInt16(reader["okToSave"].ToString());
                                    }
                                    reader.Close();
                                }
                                catch (SqlException sqleDateCheck)
                                {
                                    Logger.QuickLog(errorLogFileName, sqleDateCheck.Message, "PagePreRender()", sqlDateCheck);
                                }
                                if (okToSave == 1)
                                {
                                    try
                                    {
                                        command.CommandText = sqlUpdate;
                                        command.ExecuteNonQuery();
                                    }
                                    catch (SqlException sqleUpdate)
                                    {
                                        Logger.QuickLog(errorLogFileName, sqleUpdate.Message, "Page_PreRender()", sqlUpdate);
                                    }
                                }
                                else
                                {
                                    if (messageOutput.Equals(""))
                                    {
                                        messageOutput = "Barcode(s): " + barcode;
                                    }
                                    else
                                    {
                                        messageOutput = messageOutput + ", " + barcode;
                                    }
                                }
                                
                            }
                            else
                            {
                                try
                                {
                                    command.CommandText = sqlInsert;
                                    command.ExecuteNonQuery();
                                }
                                catch (SqlException sqleInsert)
                                {
                                    Logger.QuickLog(errorLogFileName, sqleInsert.Message, "Page_PreRender()", sqlInsert);
                                }

                            }
                        }
                        catch (SqlException sqleArchive)
                        {
                            Logger.QuickLog(errorLogFileName, sqleArchive.Message, "Page_PreRender()", sqlArchive);
                        }
                        finally
                        {
                            if (con != null)
                            {
                                con.Close();
                            }
                        }
                        utility.GetRowChanged()[r] = false;
                    } //end "if (rowChanged[r])"

                } //end "for (int r = 0; r < totalRows; r++)"
                if (!messageOutput.Equals(""))
                {
                    messageOutput = messageOutput + " Not Saved due to being updated by someone else";
                }

                if (updateMessage)
                {
                    if (messageOutput.Equals(""))
                    {
                        messageOutput = "All Records Saved";
                        LabelMessage.CssClass = "Green";
                    }
                    else
                    {
                        LabelMessage.CssClass = "Red";
                    }
                    LabelMessage.Text = messageOutput;

                }
                else if (LabelMessage.Text.Equals("All Records Saved"))
                {
                    LabelMessage.Text = "";
                }


                dataGridView.DataBind();
                if (utility.GetIsAdminUser() || utility.GetIsCMUser() )
                {
                    AmountSelectedHidden.Value = Convert.ToString(GetTotalChecked());
                }

            } //end if (Page.IsPostBack)
            else
            {
                dataGridView.SelectedIndex = -1;
            }

        } //end Page_PreRender()

        protected void GridViewDataDivRowBindEvent(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                if (utility.GetIsDeptUser())
                {
                    e.Row.Cells[columnPayFlag].Attributes.Add("class", "hidden");
                    e.Row.Cells[columnPriorityCM].Attributes.Add("class", "hidden");
                    e.Row.Cells[columnPaymentMethod].Attributes.Add("class", "hidden");
                    payColumnDisplayed.Value = "false";
                }
                else
                {
                    e.Row.Cells[columnPayFlag].Attributes.Add("class", "locked");
                    payColumnDisplayed.Value = "true";
                }


                //HEADER ROW SPECIFIC
                if (e.Row.RowType == DataControlRowType.Header)
                {
                }

                //DATAROW SPECIFIC
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    decimal amount = 0;
                    try
                    {
                        amount = Convert.ToDecimal(e.Row.Cells[columnAmountHidden].Text);
                    }
                    catch (FormatException) { }
                    catch (Exception) { }

                    if (utility.GetIsAdminUser() || utility.GetIsCMUser() )
                    {
                        ((CheckBox)e.Row.Cells[columnPayFlag].FindControl("review1CB")).Attributes.Add("onClick", "SumToPay(" + amount + ", '" + e.Row.FindControl("review1CB").ClientID.ToString() + "')");
                        ((Label)e.Row.Cells[columnNotes].FindControl("LabelExpandAP")).Attributes["onclick"] = "populateBox('" + ((TextBox)e.Row.Cells[columnNotes].FindControl("cmNotesTB")).ClientID + "')";

                        if (!e.Row.Cells[columnPriorityCMHidden].Text.Equals("-1"))
                        {
                            //((RadioButtonList)e.Row.Cells[columnPriorityCM].FindControl("cmPriorityRadioButtonList")).Items.FindByValue("4").Selected = false;
                            ((RadioButtonList)e.Row.Cells[columnPriorityCM].FindControl("cmPriorityRadioButtonList")).Items.FindByValue("2").Selected = false;
                            ((RadioButtonList)e.Row.Cells[columnPriorityCM].FindControl("cmPriorityRadioButtonList")).Items.FindByValue(e.Row.Cells[columnPriorityCMHidden].Text).Selected = true;
                        }

                        if (!e.Row.Cells[columnIdHidden].Text.Equals("-1") &&
                                (
                                e.Row.Cells[columnPaymentMethodHidden].Text.Equals("Bank")
                                || e.Row.Cells[columnPaymentMethodHidden].Text.Equals("PC-CY")
                                || e.Row.Cells[columnPaymentMethodHidden].Text.Equals("PC-FY")
                                )
                            )
                        {
                            ((DropDownList)e.Row.Cells[columnPaymentMethod].FindControl("DropDownListPayMthd")).Items.FindByValue("").Selected = false;
                            ((DropDownList)e.Row.Cells[columnPaymentMethod].FindControl("DropDownListPayMthd")).Items.FindByValue(e.Row.Cells[columnPaymentMethodHidden].Text).Selected = true;
                        }

                        if (((TextBox)e.Row.Cells[columnNotes].FindControl("cmNotesTB")).Text.Length > 25)
                        {
                            ((TextBox)e.Row.Cells[columnNotes].FindControl("cmNotesTB")).Attributes.Add("onmouseover", "Tip('<b><u>Full Text:</u></b><br/>" + ((TextBox)e.Row.Cells[columnNotes].FindControl("cmNotesTB")).Text + "', WIDTH, 250)");
                            ((TextBox)e.Row.Cells[columnNotes].FindControl("cmNotesTB")).Attributes.Add("onmouseout", "UnTip()");
                        }

                        if (!utility.GetIsAdminUser())
                        {
                            ((TextBox)e.Row.Cells[columnNotes].FindControl("deptNotesTB")).Visible = false;
                            ((Label)e.Row.Cells[columnNotes].FindControl("deptNotesLabel")).Visible = true;
                            ((Label)e.Row.Cells[columnNotes].FindControl("LabelExpandDept")).Text = "Dept:";
                            ((RadioButtonList)e.Row.Cells[columnPriorityCM].FindControl("deptPriorityRadioButtonList")).Visible = false;
                            ((Label)e.Row.Cells[columnPriorityDept].FindControl("deptPriorityLabel")).Visible = true;
                            if (((Label)e.Row.Cells[columnPriorityDept].FindControl("deptPriorityLabel")).Text.Equals("-1"))
                            {
                                //((Label)e.Row.Cells[columnPriorityDept].FindControl("deptPriorityLabel")).Text = "4";
                                ((Label)e.Row.Cells[columnPriorityDept].FindControl("deptPriorityLabel")).Text = "2";
                            }

                        }

                    }

                    if (utility.GetIsDeptUser() || utility.GetIsAdminUser())
                    {
                        ((Label)e.Row.Cells[columnNotes].FindControl("LabelExpandDept")).Attributes["onclick"] = "populateBox('" + ((TextBox)e.Row.Cells[columnNotes].FindControl("deptNotesTB")).ClientID + "')";

                        if (((TextBox)e.Row.Cells[columnNotes].FindControl("deptNotesTB")).Text.Length > 25)
                        {
                            ((TextBox)e.Row.Cells[columnNotes].FindControl("deptNotesTB")).Attributes.Add("onmouseover", "Tip('<b><u>Full Text:</u></b><br/>" + ((TextBox)e.Row.Cells[columnNotes].FindControl("deptNotesTB")).Text + "', WIDTH, 250)");
                            ((TextBox)e.Row.Cells[columnNotes].FindControl("deptNotesTB")).Attributes.Add("onmouseout", "UnTip()");
                        }

                        if (!e.Row.Cells[columnPriorityDeptHidden].Text.Equals("-1"))
                        {
                            //((RadioButtonList)e.Row.Cells[columnPriorityDept].FindControl("deptPriorityRadioButtonList")).Items.FindByValue("4").Selected = false;
                            ((RadioButtonList)e.Row.Cells[columnPriorityDept].FindControl("deptPriorityRadioButtonList")).Items.FindByValue("2").Selected = false;
                            ((RadioButtonList)e.Row.Cells[columnPriorityDept].FindControl("deptPriorityRadioButtonList")).Items.FindByValue(e.Row.Cells[columnPriorityDeptHidden].Text).Selected = true;
                        }


                        if (!utility.GetIsAdminUser())
                        {
                            ((TextBox)e.Row.Cells[columnNotes].FindControl("cmNotesTB")).Visible = false;
                            ((Label)e.Row.Cells[columnNotes].FindControl("cmNotesLabel")).Visible = true;
                            ((Label)e.Row.Cells[columnNotes].FindControl("LabelExpandAP")).Text = "Cash Management:";
                        }

                    }

                    if (((Label)e.Row.Cells[columnInvoiceInformation].FindControl("LabelInvoiceNumber")).Text.Length > 9)
                    {
                        ((Label)e.Row.Cells[columnInvoiceInformation].FindControl("LabelInvoiceNumber")).Text = ((Label)e.Row.Cells[columnInvoiceInformation].FindControl("LabelInvoiceNumber")).Text.ToString().Substring(0, 8) + "+";

                        ((Label)e.Row.Cells[columnInvoiceInformation].FindControl("LabelInvoiceNumber")).Attributes.Add("onmouseover", "Tip('<b><u>Full Text:</u></b><br/>"
                                + e.Row.Cells[columnInvoiceNumberHidden].Text
                                + "<br />"
                                + ((Label)e.Row.Cells[columnInvoiceInformation].FindControl("LabelInvoiceDate")).Text
                                + "', WIDTH, 250)");
                        ((Label)e.Row.Cells[columnInvoiceInformation].FindControl("LabelInvoiceNumber")).Attributes.Add("onmouseout", "UnTip()");
                    }

                    if (e.Row.Cells[columnBarcode].Text.Equals("-1"))
                    {
                        e.Row.Cells[columnBarcode].Text = "-";
                    }


                    if (e.Row.Cells[columnCurrencyHidden].Text.ToString().Equals("USD"))
                    {
                        e.Row.Cells[columnAmount].Text = "$" + e.Row.Cells[columnAmount].Text;
                    }
                    else if (e.Row.Cells[columnCurrencyHidden].Text.ToString().Equals("GBP"))
                    {
                        e.Row.Cells[columnAmount].Text = "£" + e.Row.Cells[columnAmount].Text;
                    }
                    else if (e.Row.Cells[columnCurrencyHidden].Text.ToString().Equals("EUR"))
                    {
                        e.Row.Cells[columnAmount].Text = "€" + e.Row.Cells[columnAmount].Text;
                    }
                    else if (e.Row.Cells[columnCurrencyHidden].Text.ToString().Equals("JPY"))
                    {
                        e.Row.Cells[columnAmount].Text = "¥" + e.Row.Cells[columnAmount].Text;
                    }
                    else
                    {
                        e.Row.Cells[columnAmount].Text = e.Row.Cells[columnAmount].Text + " " + e.Row.Cells[columnCurrencyHidden].Text;
                    }

                }

            }

        }

        protected void GridViewDataBoundEvent(object sender, EventArgs e)
        {
            data.Attributes.Add("Style", VariablesCashManager.DATA_DIV_CUSTOM_STYLE);
            if (dataGridView.PageCount > 1)
            {
                hasPagerRow.Value = "true";
            }

        }

        protected void GridViewDataSelectedIndexChanged(object sender, EventArgs e)
        {
            BearCode bearCode = new BearCode();
            bearCode.GridViewCustomizePagerRow(dataGridView, dataGridView.BottomPagerRow);
        }

        protected void GridViewDataPageIndexChanged(object sender, EventArgs e)
        {
            dataGridView.SelectedIndex = -1;
            LabelCurrentPage.Text = "Page " + Convert.ToString(dataGridView.PageIndex + 1) + " of " + dataGridView.PageCount;
        }

        protected void SqlDataSource_Elite_uspBMcBEARCashFlowManager_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            String inv = "";
            String loc = "";
            String dept = "";
            String dp = "";
            String cmp = "";
            String pay = "";
            String mthd = "";
            String cur = "";
            String src = "";
            String ofc = "";
            String vid = "";
            String vname = "";
            String bc = "";

            if (Request["inv"] != null)
            {
                inv = Request["inv"].ToString();
            }
            if (Request["bc"] != null)
            {
                bc = Request["bc"].ToString();
            }
            if (Request["loc"] != null)
            {
                loc = Request["loc"].ToString();
            }
            if (Request["dept"] != null)
            {
                dept = Request["dept"].ToString();
            }
            if (Request["dp"] != null)
            {
                dp = Request["dp"].ToString();
            }
            if (Request["cmp"] != null)
            {
                cmp = Request["cmp"].ToString();
            }
            if (Request["pay"] != null)
            {
                pay = Request["pay"].ToString();
            }
            if (Request["mthd"] != null)
            {
                mthd = Request["mthd"].ToString();
            }
            if (Request["cur"] != null)
            {
                cur = Request["cur"].ToString();
            }
            if (Request["src"] != null)
            {
                src = Request["src"].ToString();
            }
            if (Request["ofc"] != null)
            {
                ofc = Request["ofc"].ToString();
            }
            if (Request["vid"] != null)
            {
                vid = Request["vid"].ToString();
            }
            if (Request["vname"] != null)
            {
                vname = Request["vname"].ToString();
            }


            e.Command.CommandTimeout = 0;
            if (utility.GetIsDeptUser())
            {
                e.Command.Parameters.Add(new SqlParameter("@deptUser", "1"));
            }
            if (!utility.GetLocation().Equals("All") && !utility.GetLocation().Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@location", utility.GetLocation()));
            }
            if (!utility.GetDepartment().Equals("All") && !utility.GetDepartment().Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@department", utility.GetDepartment()));
            }
            if (!dp.Equals("All") && !dp.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@deptPriority", dp));
            }
            if (!cmp.Equals("All") && !cmp.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@cmPriority", cmp));
            }
            if (!pay.Equals("All") && !pay.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@payFlag", pay));
            }
            if (!mthd.Equals("All") && !mthd.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@paymentMethod", mthd));
            }
            if (!inv.Equals("All") && !inv.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@invoiceNumber", inv));
            }
            if (!bc.Equals("All") && !bc.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@barcode", bc));
            }
            if (!cur.Equals("All") && !cur.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@currency", cur));
            }
            if (!src.Equals("All") && !src.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@source", src));
            }
            if (!ofc.Equals("All") && !ofc.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@office", ofc));
            }
            if (!vid.Equals("All") && !vid.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@vendorId", vid));
            }
            if (!vname.Equals("All") && !vname.Equals(""))
            {
                e.Command.Parameters.Add(new SqlParameter("@vendorName", vname));
            }


        }

        protected void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            CheckBox thisCheckBox = (CheckBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            utility.GetRowChanged()[row] = true;
        }

        protected void RadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList thisRbl = (RadioButtonList)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisRbl.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            utility.GetRowChanged()[row] = true;
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList thisDdl = (DropDownList)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisDdl.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            utility.GetRowChanged()[row] = true;
        }

        /// <summary>
        /// When the text is changed in the dataGrid, the row is marked as changed<br />
        /// this boolean is used in Page_PreRender to save changed rows<br />
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            utility.GetRowChanged()[row] = true;
        }

        protected void buttonChangeParametersClick(object sender, EventArgs e)
        {
            Response.Redirect(VariablesCashManager.PARAM_PAGE);
        }

        protected void buttonAdminOptionsClick(object sender, EventArgs e)
        {
            Response.Redirect(VariablesCashManager.ADMIN_PAGE);
        }

        #region Custom Get Methods
        public int getCurrentPage()
        {
            return (int)dataGridView.PageIndex + 1;
        }

        public int getPageCount()
        {
            return (int)dataGridView.PageCount;
        }

        #endregion

        /// <summary>
        /// Logs User Access using:<br />
        /// UPDATE dbo.BMcBEARCashFlowManagerUsers " +
        /// " SET lastLogin = getdate() " +
        /// " WHERE networkId = '" + utility.GetUserName() + "' ";
        /// </summary>
        protected void LogUserAccess()
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = " UPDATE dbo.BMcBEARCashFlowManagerUsers " +
                                                   " SET lastLogin = getdate() " +
                                                   " WHERE networkId = '" + utility.GetUserName() + "' ";
                command.ExecuteNonQuery();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "LogUserAccess()", "");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        /// <summary>
        /// uses Stored Procedure: uspBMcBEARCashFlowManager
        /// </summary>
        /// <returns>total amount of Checked Rows</returns>
        protected decimal GetTotalChecked()
        {
            String userName = utility.GetUserName(); 
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            decimal totalChecked = 0;
            try
            {
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARCashFlowManager";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@totalSelectedOnly", true);
                command.Parameters.AddWithValue("@location", utility.GetLocation());
                command.CommandTimeout = 200;
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    totalChecked = Convert.ToDecimal(reader["totalChecked"].ToString());
                }
            }

            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetTotalChecked()", "");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            return totalChecked;
        }

        /// <summary>
        /// Gets Payment Methods set up in the database using: <br />
        /// Select ID, PayMethodCode, PayMethod FROM dbo.BMcBEARCashFlowManagerPaymentMethods (nolock)
        /// </summary>
        protected void GetPaymentMethods()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = "Select ID, PayMethodCode, PayMethod FROM dbo.BMcBEARCashFlowManagerPaymentMethods (nolock)";
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                }
            }

            catch (SqlException sqle)
            {
                Logger.QuickLog(errorLogFileName, sqle.Message, "GetPaymentMethods()", "");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        /// <summary>
        /// Sets the User's amount to pay based on what is stored in the database.  See UtilityCashFlow
        /// </summary>
        protected void SetUpUser()
        {

            Session["amountToPay"] = utility.GetAmountToPay();

            if (!utility.GetIsAdminUser() && !utility.GetIsCMUser() && !utility.GetIsDeptUser())
            {
                Response.Redirect(VariablesCashManager.NO_ACCESS_PAGE);
            }

        }

    }
}