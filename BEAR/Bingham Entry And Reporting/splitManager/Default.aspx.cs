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

namespace BEAR.splitManager
{
    public partial class Default : System.Web.UI.Page
    {
        protected int columnSubMatter = 0;
        protected int columnMatterDescription = 1;
        protected int columnAmount = 2;
        protected int columnCalculatedPercent = 3;
        protected int columnActualPercent = 4;
        protected int columnElitePerecnt = 5;
        protected int columnDeleteButton = 6;
        protected int columnId = 7;

        protected UtilitySplitManager utility; ///<Global variable to access the UtilitySplitManager class

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;
            utility = new UtilitySplitManager();

            utility.UserName = Page.User.Identity.Name.ToString().Substring(8);

            int totalRows = 0;
            try
            {
                totalRows = dataGridView.Rows.Count;
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "Page_Load()", "int totalRows = dataGridView.Rows.Count");
            }
            utility.SetRowChanged(totalRows);

            //TotalAmountHidden.Value = Convert.ToString(GetTotalAmount());

            if (!Page.IsPostBack)
            {
                //LogUserAccess();
            }
        }

        /// <summary>
        /// Called before the page is rendered.  This is used for the save functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
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

                int totalRows = utility.GetRowChanged().Length;
                int rowsChanged = 0;
                

                for (int r = 0; r < totalRows; r++)
                {
                    if (utility.GetRowChanged()[r])
                    {
                        rowsChanged++;
                        GridViewRow thisGridViewRow = dataGridView.Rows[r];

                        String id = "";
                        decimal Amount = 0;
                        decimal PctActual = 0;

                        SqlConnection con = null;
                        String sqlUpdate = "";
                        String sqlArchive = "";
                        try
                        {
                            id = thisGridViewRow.Cells[columnId].Text;
                            if (!((TextBox)thisGridViewRow.FindControl("amountTB")).Text.Equals(""))
                            {
                                Amount = Convert.ToDecimal(((TextBox)thisGridViewRow.FindControl("amountTB")).Text);
                            }
                            if (!((TextBox)thisGridViewRow.FindControl("PctActualTB")).Text.Equals(""))
                            {
                                PctActual = Convert.ToDecimal(((TextBox)thisGridViewRow.FindControl("PctActualTB")).Text);
                            }
                            
                            /**
                             * Archive First.
                             * " INSERT INTO dbo.BMcSplitManagerDetailArchive " 
                             * + " SELECT id, ParentId, ChildMatter, Amount, PctCalculated, PctActual, '"
                             * + utility.UserName + "' as updatedBy, " 
                             * + " getDate() as updateTime "
                             * + " FROM dbo.BMcSplitManagerDetail "
                             * + "  WHERE id = " + id; 
                             */
                            sqlArchive = " INSERT INTO dbo.BMcSplitManagerDetailArchive " 
                                            + " SELECT id, ParentId, ChildMatter, Amount, PctCalculated, PctActual, '"
                                            + utility.UserName + "' as updatedBy, " 
                                            + " getDate() as updateTime "
                                            + " FROM dbo.BMcSplitManagerDetail "
                                            + "  WHERE id = " + id;

                            /**
                             * Update after Archive.
                             * " UPDATE dbo.BMcSplitManagerDetail "
                             * + " SET Amount = " + Amount
                             * + " ,PctActual = " + PctActual
                             * + " WHERE id = " + id;
                             */
                            sqlUpdate = " UPDATE dbo.BMcSplitManagerDetail "
                                 + " SET Amount = " + Amount
                                       + " ,PctActual = " + PctActual
                                 + " WHERE id = " + id; 

                            con = new SqlConnection(
                                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                            con.Open();
                            SqlCommand command = con.CreateCommand();
                            command.CommandType = CommandType.Text;
                            command.CommandText = sqlArchive;
                            command.ExecuteNonQuery();

                            try
                            {
                                command.CommandText = sqlUpdate;
                                command.ExecuteNonQuery();
                            }
                            catch (SqlException sqleUpdate)
                            {
                                Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqleUpdate.Message, "Page_PreRender()", sqlUpdate);
                            }


                        }
                        catch (SqlException sqleArchive)
                        {
                            Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqleArchive.Message, "Page_PreRender()", sqlArchive);
                        }
                        catch (FormatException fe)
                        {
                            Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, fe.Message, "Page_PreRender()", "");
                        }
                        finally
                        {
                            if (con != null)
                            {
                                con.Close();
                            }
                        }
                        utility.GetRowChanged()[r] = false;


                    }

                    UpdateTotals();

                }

                if (rowsChanged > 0 || (!RadioButtonListCalculate.SelectedValue.Equals("0") && !DropDownListStartDates.SelectedValue.Equals("-1")))
                {
                    if (RadioButtonListCalculate.SelectedValue.Equals("C"))
                    {
                        UpdatePercentCalculated(false);
                    }
                    else if (RadioButtonListCalculate.SelectedValue.Equals("S"))
                    {
                        UpdatePercentCalculated(true);
                    }
                    dataGridView.DataBind();
                }

                

            }
        }

        /// <summary>
        /// Operations done on each row as the data is bound to the Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton b = (ImageButton)e.Row.FindControl("ImageButtonDelete");
                b.Attributes.Add("onclick", "javascript:return confirm('Are you sure you want to delete sub-matter " + DataBinder.Eval(e.Row.DataItem, "subMatter") + "? ')");
                if (Page.IsPostBack)
                {
                    
                }
                if (e.Row.Cells[columnId].Text.Equals("-1"))
                {
                    ((TextBox)e.Row.Cells[columnAmount].FindControl("amountTB")).Enabled = false;
                    ((TextBox)e.Row.Cells[columnActualPercent].FindControl("PctActualTB")).Enabled = false;
                }
                
            }
        }


        /// <summary>
        /// Operations after the gridview is finished binding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dataGridView_DataBound(object sender, EventArgs e)
        {
            ImageButtonCalcToActual.Attributes.Add("onclick", "javascript:return confirm('Copy Calculated To Actual Column? ')");

            //data.Attributes.Add("Style", VariablesSplitManager.DATA_DIV_CUSTOM_STYLE);
        }


        protected void TextBoxMasterMatter_TextChanged(object sender, EventArgs e)
        {
            Session["parentId"] = null;
            picture.Visible = true;
            PanelSearchMasterMatter.Visible = false;
            ToggleResultsGrid(false);

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

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            picture.Visible = false;
        }


        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            LabelAdminMessage.Text = "";
            Page_PreRender(sender, e);
        }


        protected void ButtonAddRecord_Click(object sender, EventArgs e)
        {
            if (!PanelAddRecord.Visible)
            {
                LabelAdminMessage.Text = "";
                TextBoxSubMatterAdd.Text = "";
                TextBoxAmountAdd.Text = "";
                TextBoxActualPctAdd.Text = "";
                LabelMessage.Text = "";
                PanelAddRecord.Visible = true;
            }

        }

        protected void ButtonCancelAdd_Click(object sender, EventArgs e)
        {
            TextBoxSubMatterAdd.Text = "";
            TextBoxAmountAdd.Text = "";
            TextBoxActualPctAdd.Text = "";
            LabelMessage.Text = "";
            PanelAddRecord.Visible = false;
            SearchSubMatter.Visible = false;
        }


        /// <summary>
        /// Checking if the SubMatter is unique for the master matter.  SubMatter can only be listed once per master matter.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="SubMatter"></param>
        /// <returns></returns>
        protected bool IsSubMatterUnique(String parentId, String SubMatter)
        {
            /**
             * " SELECT count(*) AS count "
             * + " FROM dbo.BMcSplitManagerDetail (nolock) "
             * + " WHERE parentId = " + parentId
             * + " AND ChildMatter = '" + SubMatter + "' ";
             */
            String sql = " SELECT count(*) AS count "
                            + " FROM dbo.BMcSplitManagerDetail (nolock) "
                            + " WHERE parentId = " + parentId
                            + " AND ChildMatter = '" + SubMatter + "' ";

            bool isSubMatterUnique = false;

            SqlConnection con = null;

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
                    if (Convert.ToInt16(reader["count"].ToString()) == 0)
                    {
                        isSubMatterUnique = true;
                    }
                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "IsSubMatterUnique()", sql);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            return isSubMatterUnique;


        }

        /// <summary>
        /// Calls stored procedure: uspBMcBEARSplitManagerResetFromElite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButtonUpdateFromElite_Click(object sender, EventArgs e)
        {
            LabelAdminMessage.Text = "";
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARSplitManagerResetFromElite";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@matter", TextBoxMasterMatter.Text);
                command.Parameters.AddWithValue("@startDate", DropDownListStartDates.SelectedValue);
                command.ExecuteNonQuery();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "ImageButtonUpdateFromElite_Click()", "");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            dataGridView.DataBind();

        }

        /// <summary>
        /// calls stored procedure: uspBMcBEARSplitManagerCalcToActual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButtonCalcToActual_Click(object sender, EventArgs e)
        {
            LabelAdminMessage.Text = "";
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARSplitManagerCalcToActual";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@matter", TextBoxMasterMatter.Text);
                command.Parameters.AddWithValue("@startDate", DropDownListStartDates.SelectedValue);
                command.Parameters.AddWithValue("@updatedBy", utility.UserName);
                command.ExecuteNonQuery();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "ImageButtonCalcToActual_Click()", "");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            dataGridView.DataBind();

        }


        /// <summary>
        /// Adds the record to the master matter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonSaveAdd_Click(object sender, EventArgs e)
        {
            if (Session["parentId"].ToString() == null || Session["parentId"].Equals(""))
            {
                LabelMessage.Text = "Error Saving.<br />Master Matter Number has been Lost.";
            }
            else
            {
                BearCode bearCode = new BearCode();
                String ParentId = Session["parentId"].ToString();
                String SubMatter = bearCode.AddLeadingZeros(TextBoxSubMatterAdd.Text, "matter");
                bool errorCaught = false;

                if (IsSubMatterUnique(ParentId, SubMatter))
                {
                    SqlConnection con = null;


                    String ActualPercent = TextBoxActualPctAdd.Text;
                    if (ActualPercent.Equals(""))
                    {
                        ActualPercent = "null";
                    }

                    String Amount = TextBoxAmountAdd.Text;
                    if (Amount.Equals(""))
                    {
                        Amount = "null";
                    }

                    /**
                     * " INSERT INTO dbo.BMcSplitManagerDetail "
                     * + " (ParentId, ChildMatter, Amount, PctCalculated, PctActual) "
                     * + " VALUES ( "
                                 * + ParentId
                                 * + " ,'" + SubMatter + "' "
                                 * + " ," + Amount
                                 * + " , null "
                                 * + " ," + ActualPercent
                             * + " )";
                     */
                    String sqlInsert = " INSERT INTO dbo.BMcSplitManagerDetail "
                                            + " (ParentId, ChildMatter, Amount, PctCalculated, PctActual) "
                                            + " VALUES ( "
                                            + ParentId
                                            + " ,'" + SubMatter + "' "
                                            + " ," + Amount
                                            + " , null "
                                            + " ," + ActualPercent
                                            + " )";
                    try
                    {
                        con = new SqlConnection(
                                ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                        con.Open();
                        SqlCommand command = con.CreateCommand();
                        command.CommandType = CommandType.Text;
                        command.CommandText = sqlInsert;
                        command.ExecuteNonQuery();

                    }
                    catch (SqlException sqle)
                    {
                        Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "ButtonSaveAdd_Click()", sqlInsert);
                        LabelMessage.Text = "<u>ERROR ADDING RECORD</u><br />System Message:<br />" + sqle.Message;
                        errorCaught = true;
                    }
                    finally
                    {
                        if (con != null)
                        {
                            con.Close();
                        }
                    }
                    if (!errorCaught)
                    {
                        LabelMessage.Text = "";
                        dataGridView.DataBind();
                    }
                }
                else
                {
                    LabelMessage.Text = "Unable to Add Record. <br/> Sub-Matter: " + SubMatter + " Already Exists for this Master Matter";
                }

            }


        }

        protected void ButtonChangeMasterMatter_Click(object sender, EventArgs e)
        {
            TextBoxMasterMatter.Enabled = true;
            TextBoxMasterMatter.Text = "";
            LabelMasterMatterDescription.Text = "";
            picture.Visible = true;
            ToggleResultsGrid(false);
            DropDownListStartDates.Visible = false;
            LabelStartDates.Visible = false;
            PanelSearchMasterMatter.Visible = false;
            ButtonChangeMasterMatter.Visible = false;
            ButtonGetStartDates.Visible = true;
        }

        protected void ButtonGetStartDates_Click(object sender, EventArgs e)
        {
            BearCode bearCode = new BearCode();
            String MasterMatterNumber = bearCode.AddLeadingZeros(TextBoxMasterMatter.Text, "matter");
            TextBoxMasterMatter.Enabled = false;
            DropDownListStartDates.Items.Clear();
            TextBoxMasterMatter.Text = MasterMatterNumber;
            bearCode.PopulateMasterMatterStartDates(DropDownListStartDates, MasterMatterNumber, VariablesSplitManager.ERROR_LOG_FILE_NAME);
            LabelStartDates.Visible = true;
            DropDownListStartDates.Visible = true;
            LabelMasterMatterDescription.Text = bearCode.GetMatterDesc1(MasterMatterNumber, VariablesSplitManager.ERROR_LOG_FILE_NAME);
            picture.Visible = false;
            ToggleResultsGrid(false);
            ButtonChangeMasterMatter.Visible = true;
            ButtonGetStartDates.Visible = false;

        }

        protected void ButtonCopy_Click(object sender, EventArgs e)
        {
            Page_PreRender(sender, e);
            Response.Redirect(VariablesSplitManager.COPY_PAGE);
        }

        /// <summary>
        /// Pushes data from Split Manager to Elite using Stored Procedure:<br />
        /// uspBMcBEARSplitManagerTotalPctActual & uspBMcBEARSplitManagerUpdateElite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButtonElite_Click(object sender, EventArgs e)
        {
            CloseAddRow();
            Page_PreRender(sender, e);
            String StartDate = DropDownListStartDates.SelectedValue;
            String MasterMatter = TextBoxMasterMatter.Text;
            SqlConnection con = null;
            decimal TotalPctActual = 0;
            con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            con.Open();
            SqlCommand command = con.CreateCommand();

            try
            {
                command.CommandText = "uspBMcBEARSplitManagerTotalPctActual";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@matter", MasterMatter);
                command.Parameters.AddWithValue("@startDate", StartDate);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    try
                    {
                        TotalPctActual = Convert.ToDecimal(reader["TotalPctActual"].ToString());
                    }
                    catch (FormatException fe)
                    {
                        Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, fe.Message, "Error Getting Total Actual Percent", "");
                    }
                }
                reader.Close();
                command.Parameters.Clear();
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "ImageButtonElite_Click()", "Checking Total Actual Percent.  \n MasterMatter = " + MasterMatter + " / StartDate = " + StartDate);
            }


            if (TotalPctActual.Equals(100))
            {
                try
                {

                    command.CommandText = "uspBMcBEARSplitManagerUpdateElite";

                    command.Parameters.AddWithValue("@matter", MasterMatter);
                    command.Parameters.AddWithValue("@startDate", StartDate);
                    command.Parameters.AddWithValue("@updatedBy", utility.UserName);
                    command.ExecuteNonQuery();

                    LabelAdminMessage.Text = "Update to Elite is Complete";
                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "ButtonSave_Click()", "Updating Elite, Calling StoredProcedure uspBMcBEARSplitManagerUpdateElite.  \n MasterMatter = " + MasterMatter + " / StartDate = " + StartDate + " / UpdatedBy = " + utility.UserName);
                    LabelAdminMessage.Text = "Error Updating Elite.";
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }
                dataGridView.DataBind();
            }
            else
            {
                LabelAdminMessage.Text = "Unable to Update Elite.  Total Actual Percent must equal 100%.  <br /> The total Actaul Percent is " + TotalPctActual;
            }

        }

        /// <summary>
        /// uses stored procedure: uspBMcBEARSplitManagerTotalPctActual & uspBMcBEARSplitManagerTotalAmount
        /// </summary>
        protected void UpdateTotals()
        {
            String StartDate = DropDownListStartDates.SelectedValue;
            String MasterMatter = TextBoxMasterMatter.Text;
            SqlConnection con = null;
            String TotalPctActual = "0";
            String TotalAmount = "0";
            con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            con.Open();
            SqlCommand command = con.CreateCommand();

            try
            {
                command.CommandText = "uspBMcBEARSplitManagerTotalPctActual";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@matter", MasterMatter);
                command.Parameters.AddWithValue("@startDate", StartDate);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    try
                    {
                        TotalPctActual = reader["TotalPctActual"].ToString();
                    }
                    catch (FormatException fe)
                    {
                        Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, fe.Message, "Error Getting Total Actual Percent", "");
                    }
                }
                reader.Close();
                //command.Parameters.Clear();
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "UpdateTotals()", "Checking Total Actual Percent.  \n MasterMatter = " + MasterMatter + " / StartDate = " + StartDate);
            }


            try
            {

                command.CommandText = "uspBMcBEARSplitManagerTotalAmount";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    try
                    {
                        TotalAmount = reader["TotalAmount"].ToString();
                    }
                    catch (FormatException fe)
                    {
                        Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, fe.Message, "Error Getting Total Amount", "");
                    }
                }
                reader.Close();
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "UpdateTotals()", "GettingTotal Amount.  \n MasterMatter = " + MasterMatter + " / StartDate = " + StartDate);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            //TextBoxTotalActualPercent.Text = TotalPctActual;
            //TextBoxTotalAmount.Text = TotalAmount;
            LabelTotalActualPercent.Text = TotalPctActual;
            LabelTotalAmount.Text = TotalAmount;
        }




        protected void ImageButtonMasterMatter_Click(object sender, EventArgs e)
        {
            ToggleResultsGrid(false);
            PanelSearchMasterMatter.Visible = true;
            picture.Visible = false;
            DropDownListStartDates.Items.Clear();
            DropDownListStartDates.Visible = false;
            LabelStartDates.Visible = false;
            RadioButtonListSearchResultsMasterMatter.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsMasterMatter', 'TextBoxMasterMatter')");
        }


        protected void ImageButtonSubMatter_Click(object sender, EventArgs e)
        {
            PanelSearchMasterMatter.Visible = false;
            PanelSearchSubMatter.Visible = true;
            picture.Visible = false;
            RadioButtonListSearchResultsSubMatter.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsSubMatter', 'TextBoxSubMatterAdd')");
        }


        protected void ImageButtonSearchMasterMatter_Click(object sender, EventArgs e)
        {
            PanelSearchSubMatter.Visible = false;
            LabelSearchResultsMasterMatter.Text = "Select One:";
            RadioButtonListSearchResultsMasterMatter.DataBind();
            RadioButtonListSearchResultsMasterMatter.Visible = true;
            if (RadioButtonListSearchResultsMasterMatter.Items.Count.ToString().Equals("0"))
            {
                LabelSearchResultsMasterMatter.Text = VariablesGlobal.SEARCH_NO_RESULTS_FOUND;
            }
        }


        protected void ImageButtonSearchSubMatter_Click(object sender, EventArgs e)
        {
            PanelSearchMasterMatter.Visible = false;
            LabelSearchResultsSubMatter.Text = "Select One:";
            RadioButtonListSearchResultsSubMatter.DataBind();
            RadioButtonListSearchResultsSubMatter.Visible = true;
            if (RadioButtonListSearchResultsSubMatter.Items.Count.ToString().Equals("0"))
            {
                LabelSearchResultsSubMatter.Text = VariablesGlobal.SEARCH_NO_RESULTS_FOUND;
            }
        }

        protected void LinkButtonCloseSearchClick(object sender, EventArgs e)
        {
            PanelSearchMasterMatter.Visible = false;
            PanelSearchSubMatter.Visible = false;
        }



        protected void DropDownListStartDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListStartDates.SelectedValue.Equals("-1"))
            {
                picture.Visible = true;
                PanelSearchMasterMatter.Visible = false;
                ToggleResultsGrid(false);
            }
            else
            {
                Session["parentId"] = utility.GetMasterMatterId(TextBoxMasterMatter.Text, DropDownListStartDates.SelectedValue);
                picture.Visible = false;
                PanelSearchMasterMatter.Visible = false;
                ToggleResultsGrid(true);
                UpdateTotals();
                dataGridView.DataBind();
                dataGridView.Visible = true;

            }
            
        }



        protected void ToggleResultsGrid(bool displayValue)
        {
            CloseAddRow();
            dataGridView.Visible = displayValue;
            ButtonSave.Visible = displayValue;
            RadioButtonListCalculate.Visible = displayValue;
            ButtonAddRecord.Visible = displayValue;
            ImageButtonCalcToActual.Visible = displayValue;
            ImageButtonUpdateFromElite.Visible = displayValue;
            ImageButtonElite.Visible = displayValue;
            LabelAdminMessage.Text = "";
            divTotals.Visible = displayValue;
        }


        protected void CloseAddRow()
        {
            PanelSearchSubMatter.Visible = false;
            PanelAddRecord.Visible = false;
        }



        protected void SqlDataSource_MasterMatter_EscapeSingleQuote(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchvar", TextBoxSearchMasterMatter.Text.Replace("'", "''")));
        }

        protected void SqlDataSource_SubMatter_EscapeSingleQuote(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchvar", TextBoxSearchMasterMatter.Text.Replace("'", "''")));
        }

        protected void SqlDataSource_Elite_uspBMcBEARSplitManager_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            String MasterMatter = TextBoxMasterMatter.Text;
            String StartDate = DropDownListStartDates.SelectedValue;

            e.Command.CommandTimeout = 0;
            e.Command.Parameters.Add(new SqlParameter("@matter", MasterMatter));
            e.Command.Parameters.Add(new SqlParameter("@startDate", StartDate));
        }

        /// <summary>
        /// uses stored procedure: uspBMcBEARSplitManagerCalculatePercent
        /// </summary>
        /// <param name="spreadEvenly"></param>
        protected void UpdatePercentCalculated(bool spreadEvenly)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = "uspBMcBEARSplitManagerCalculatePercent";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@matter", TextBoxMasterMatter.Text);
                command.Parameters.AddWithValue("@startDate", DropDownListStartDates.SelectedValue);
                if (spreadEvenly)
                {
                    command.Parameters.AddWithValue("@spreadEvenly", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@spreadEvenly", 0);
                }

                command.ExecuteNonQuery();
            }

            catch (SqlException sqle)
            {
                //if the total of the amounts = 0, then spread evenly
                if (sqle.Message.Equals("Divide by zero error encountered.\r\nThe statement has been terminated.") && !spreadEvenly)
                {
                    UpdatePercentCalculated(true);
                }
                else
                {
                    Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "UpdatePercentCalculated()", "");
                }
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


    }
}
