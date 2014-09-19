using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BEAR.cas
{
    public partial class Default : System.Web.UI.Page
    {
        protected int columnHiddenSelectButton = 0;
        protected int columnID = 1; 
        protected int columnMatter = 2;
        protected int columnMatterStatus = 3;
        protected int columnCostCode = 4;
        protected int columnTimekeeper = 5;
        protected int columnGLString = 6;
        protected int columnAmount = 7;
        protected int columnDescription = 8;
        protected int columnDescription2 = 9;
        protected int columnBulkUpdate = 10;
        protected int columnStatus = 11;

        protected UtilityCAS utility;
        protected BearCode bearCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            utility = new UtilityCAS();
            bearCode = new BearCode();
            ImageButtonRecalc.Attributes.Add("onclick", "javascript:return confirm('Recalc Status on All Records (this may take a while)?')");

            if (Page.IsPostBack)
            {
                int totalRows = 0;

                try
                {
                    totalRows = GridViewSearchResults.Rows.Count;
                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "Page_Load()", "int totalRows = dataGridView.Rows.Count");
                }
                utility.SetRowChanged(totalRows);

                //rowChanged = new bool[totalRows];

                if (GridViewSearchResults.SelectedRow != null)
                {
                    if (GridViewSearchResults.SelectedRow.RowIndex != -1)
                    {
                        Session["rowSelected"] = GridViewSearchResults.SelectedRow.RowIndex.ToString();
                    }
                }
            }
            else
            {
                Session["defaultColumnWidth1"] = Hidden0.Value.ToString();
                Session["defaultColumnWidth2"] = Hidden1.Value.ToString();
                Session["defaultColumnWidth3"] = Hidden2.Value.ToString();
                Session["defaultColumnWidth4"] = Hidden3.Value.ToString();
                Session["defaultColumnWidth5"] = Hidden4.Value.ToString();
                Session["defaultColumnWidth6"] = Hidden5.Value.ToString();
                Session["defaultColumnWidth7"] = Hidden6.Value.ToString();
                Session["defaultColumnWidth8"] = Hidden7.Value.ToString();
                Session["defaultColumnWidth9"] = Hidden8.Value.ToString();
                Session["defaultColumnWidth10"] = Hidden9.Value.ToString();

                Session["rowSelected"] = "-1";

                Session["BulkUpdate"] = "Off";

                TextBoxBulkGLString.Enabled = false;
                TextBoxBulkGLString.Attributes.CssStyle.Add("background-color", "#AAB0B7");
                
                PopulateVendors(DropDownListVendors);
                PopulateOfficeLocations();
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
                int selectedRowIndex = GridViewSearchResults.SelectedIndex;

                int totalRows = GridViewSearchResults.Rows.Count;
                for (int r = 0; r < totalRows; r++)
                {
                    if (utility.GetRowChanged()[r])
                    {

                        GridViewRow thisGridViewRow = GridViewSearchResults.Rows[r];
                        
                        String id = "";
                        String matter = "";
                        String timekeep = "";
                        decimal amount = 0;
                        String costCode = "";
                        String description = "";
                        String office = "";
                        int newStatus = 0;
                        String GLString = "";

                        bool isCost = true;
                        if (RadioButtonListGLCost.SelectedValue.Equals("G"))
                        {
                            isCost = false;
                        }

                        try
                        {
                            id = thisGridViewRow.Cells[columnID].Text;
                            description = ((TextBox)thisGridViewRow.FindControl("descriptionTB")).Text.Replace("'", "''");
                            if (isCost)
                            {
                                matter = bearCode.AddLeadingZeros(((TextBox)thisGridViewRow.FindControl("matterTB")).Text.Replace("'", "''"), "matter");
                                timekeep = bearCode.AddLeadingZeros(((TextBox)thisGridViewRow.FindControl("timekeepTB")).Text, "tkid");
                                costCode = ((TextBox)thisGridViewRow.FindControl("costcodeTB")).Text.Replace("'", "''");
                            }
                            else
                            {
                                GLString = ((TextBox)thisGridViewRow.FindControl("glstringTB")).Text;
                            }

                            if (CheckBoxOverrideOffice.Checked)
                            {
                                if (Session["OfficeValue"] != null)
                                {
                                    office = Session["OfficeValue"].ToString();
                                }
                                else
                                {
                                    office = DropDownListDetailsOffice.SelectedValue.ToString();
                                }
                                utility.SetOverrideOfficeFlag(id, true);
                            }
                            else
                            {
                                office = bearCode.GetTimekeeperInfo(timekeep, "tkloc", VariablesCAS.ERROR_LOG_FILE_NAME);
                                utility.SetOverrideOfficeFlag(id, false);
                            }
                            try
                            {
                                amount = decimal.Parse(((TextBox)thisGridViewRow.FindControl("amountTB")).Text);
                            }
                            catch (FormatException)
                            {
                                //this is to catch forecast when set to a nonNumber
                                //non numbers are converted to zero
                            }
                            if (isCost)
                            {
                                newStatus = utility.GetStatusForSaveCost(Convert.ToInt16(thisGridViewRow.Cells[columnStatus].Text), timekeep, matter, costCode);
                            }
                            else
                            {
                                newStatus = utility.GetStatusForSaveGL(Convert.ToInt16(thisGridViewRow.Cells[columnStatus].Text), GLString);
                            }


                        }
                        catch (FormatException)
                        {
                            //this is to catch when set to a nonNumber
                            //non numbers are converted to zero
                        }
                        catch (ArgumentOutOfRangeException ae)
                        {
                            //when using paging
                            Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, ae.Message, "Page_PreRender()", "");
                        }

                        SqlConnection con = null;

                        String sqlArchive = " INSERT INTO tblTempRecArchive "
                                          + " SELECT TempId "
                                          + " ,vendId "
                                          + " ,AppId "
                                          + " ,processId "
                                          + " ,InvoiceNo "
                                          + " ,InvoiceDate "
                                          + " ,client "
                                          + " ,matter "
                                          + " ,timekeep "
                                          + " ,offc "
                                          + " ,Amount "
                                          + " ,Tdate "
                                          + " ,CostCode "
                                          + " ,status "
                                          + " ,Description "
                                          + " ,ProcessDate "
                                          + " ,Quantity "
                                          + " ,Rate "
                                          + " ,Phone "
                                          + " ,ApprovedBy "
                                          + " ,DateApproved "
                                          + " ,BeforeTrans "
                                          + " ,GLString " 
                                          + " ,'" + Page.User.Identity.Name.ToString().Substring(8) + "' AS ArchivedBy "
                                          + " ,'" + DateTime.Now + "' AS ArchiveDateTime "
                                          + " FROM tblTempRec "
                                          + " WHERE TempId = " + id;


                        String sqlUpdate = " UPDATE tblTempRec "
                                          + " SET Description = '" + description + "' "
                                              + " ,Amount = " + amount
                                              + " ,status = " + newStatus
                                              + " ,offc = '" + office + "' ";
                        
                        if (isCost)
                        {
                            sqlUpdate = sqlUpdate
                                              + " ,matter = '" + matter + "' "
                                              + " ,timekeep = '" + timekeep + "' "
                                              + " ,CostCode = '" + costCode + "' ";
                        }
                        else
                        {
                            sqlUpdate = sqlUpdate
                                              + " ,GLString = '" + GLString + "' ";
                        }


                        sqlUpdate = sqlUpdate
                                              + " WHERE TempId = " + id;

                        try
                        {
                            con = new SqlConnection(
                                    ConfigurationManager.ConnectionStrings["CASConnectionString"].ConnectionString);
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
                                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqleUpdate.Message, "Page_PreRender()", sqlUpdate);
                            }

                        }
                        catch (SqlException sqleArchive)
                        {
                            Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqleArchive.Message, "Page_PreRender()", sqlArchive);
                        }
                        finally
                        {
                            if (con != null)
                            {
                                con.Close();
                            }
                        }

                        if (r == selectedRowIndex && !RadioButtonListViewAll.SelectedValue.Equals("0")) 
                        {
                            if (utility.IsStatusValidForWebApplication(timekeep, matter, costCode))
                            {
                                EraseGridViewSearchResults(false);
                            }
                        }


                    } //end "if (rowChanged[r])"

                } //end "for (int r = 0; r < totalRows; r++)"
                GridViewSearchResults.DataBind();
                if (LinkButtonChangeSearch.Text.Equals("Run By Timekeeper"))
                {
                    GridViewTotalAmount.DataBind();
                }
                if (PanelDetails.Visible)
                {
                    PopulateMoreDetails();
                }

                if (GridViewSearchResults.SelectedRow != null)
                {
                    if (GridViewSearchResults.SelectedRow.RowIndex != -1)
                    {
                        GridViewSearchResults_SelectedIndexChanged(sender, e);
                    }
                }


                totalRows = 0;

                try
                {
                    totalRows = GridViewSearchResults.Rows.Count;
                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "BindGrid()", "int totalRows = dataGridView.Rows.Count");
                    totalRows = 0;
                }

                if (totalRows == 0)
                {
                    ToggleApplicationButtons(false);
                    HideAllPanels(false);
                    if (LinkButtonChangeSearch.Text.Equals("Run By Timekeeper"))
                    {
                        if (!DropDownListProcessIDs.SelectedValue.Equals("-1") && ! DropDownListVendors.SelectedValue.Equals("-1"))
                        {
                            RadioButtonListViewAll.Visible = true;
                            RadioButtonListGLCost.Visible = true;
                            ImageButtonRecalc.Visible = true;
                        }
                    }
                }

            } //end if (Page.IsPostBack)

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


        protected void DropDownListDetailsOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            int row = GridViewSearchResults.SelectedIndex;
            utility.GetRowChanged()[row] = true;
            Session["OfficeValue"] = DropDownListDetailsOffice.SelectedValue.ToString();
        }


        protected void CheckBoxOverrideOffice_CheckChanged(object sender, EventArgs e)
        {
            int row = GridViewSearchResults.SelectedIndex;
            utility.GetRowChanged()[row] = true;
            if (!CheckBoxOverrideOffice.Checked)
            {
                Session["OfficeValue"] = null;
            }
        }
        

        protected void BindGrid(bool pagerRow)
        {
            if (pagerRow)
            {
                GridViewSearchResults.AllowPaging = true;
            }
            else
            {
                GridViewSearchResults.AllowPaging = false;
            }
            GridViewSearchResults.DataBind();
            GridViewSearchResults.PageIndex = 0;
            GridViewSearchResults.Visible = true;
            //ImageButtonSearchMatterPanel.Visible = true;
            
            if (TextBoxTimekeeperID.Text.Equals("00000"))
            {
                RadioButtonListViewAll.Visible = true;
                RadioButtonListGLCost.Visible = true;
                ImageButtonRecalc.Visible = true;
            }
            else
            {
                RadioButtonListViewAll.Visible = false;
                RadioButtonListGLCost.Visible = false;
                ImageButtonRecalc.Visible = false;
            }

            int totalRows = 0;

            try
            {
                totalRows = GridViewSearchResults.Rows.Count;
            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "BindGrid()", "int totalRows = dataGridView.Rows.Count");
                totalRows = 0;
            }
            utility.SetRowChanged(totalRows);
            if (totalRows > 0)
            {
                ToggleApplicationButtons(true);

                if (LinkButtonChangeSearch.Text.Equals("Run By Timekeeper"))
                {
                    GridViewTotalAmount.Visible = true;
                    GridViewTotalAmount.DataBind();
                }

            }
            else
            {
                ToggleApplicationButtons(false);
                HideAllPanels(false);
                if (LinkButtonChangeSearch.Text.Equals("Run By Timekeeper"))
                {
                    RadioButtonListViewAll.Visible = true;
                    RadioButtonListGLCost.Visible = true;
                    ImageButtonRecalc.Visible = true;
                }
                else
                {
                    RadioButtonListViewAll.Visible = false;
                    RadioButtonListGLCost.Visible = false;
                    ImageButtonRecalc.Visible = false;
                }
            }

        }

        
        protected void GridViewSearchResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                if (RadioButtonListGLCost.SelectedValue.Equals("C"))
                {
                    e.Row.Cells[columnGLString].Attributes.CssStyle.Add("display", "none");
                    e.Row.Cells[columnGLString].Attributes.CssStyle.Add("visibility", "hidden");
                }
                else if (RadioButtonListGLCost.SelectedValue.Equals("G"))
                {
                    e.Row.Cells[columnMatter].Attributes.CssStyle.Add("display", "none");
                    e.Row.Cells[columnMatter].Attributes.CssStyle.Add("visibility", "hidden");
                    e.Row.Cells[columnMatterStatus].Attributes.CssStyle.Add("display", "none");
                    e.Row.Cells[columnMatterStatus].Attributes.CssStyle.Add("visibility", "hidden");
                    e.Row.Cells[columnCostCode].Attributes.CssStyle.Add("display", "none");
                    e.Row.Cells[columnCostCode].Attributes.CssStyle.Add("visibility", "hidden");
                    e.Row.Cells[columnTimekeeper].Attributes.CssStyle.Add("display", "none");
                    e.Row.Cells[columnTimekeeper].Attributes.CssStyle.Add("visibility", "hidden");
                }



                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[1].Attributes.Add("width", Hidden0.Value.ToString());
                    e.Row.Cells[2].Attributes.Add("width", Hidden1.Value.ToString());
                    e.Row.Cells[3].Attributes.Add("width", Hidden2.Value.ToString());
                    e.Row.Cells[4].Attributes.Add("width", Hidden3.Value.ToString());
                    e.Row.Cells[5].Attributes.Add("width", Hidden4.Value.ToString());
                    e.Row.Cells[6].Attributes.Add("width", Hidden5.Value.ToString());
                    e.Row.Cells[7].Attributes.Add("width", Hidden6.Value.ToString());
                    e.Row.Cells[8].Attributes.Add("width", Hidden7.Value.ToString());
                    e.Row.Cells[9].Attributes.Add("width", Hidden8.Value.ToString());
                    e.Row.Cells[10].Attributes.Add("width", Hidden9.Value.ToString());

                }

                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowState.ToString().Contains("Selected"))  // != DataControlRowState.Selected)
                    {
                        e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this,false);";
                    }
                    else
                    {
                        e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this,true);";
                    }
                    e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
                    e.Row.Cells[columnID].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                                                    (this.GridViewSearchResults, "Select$" + e.Row.RowIndex);

                    e.Row.Cells[columnAmount].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("amountTB").ClientID + "')";
                    e.Row.Cells[columnCostCode].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("costcodeTB").ClientID + "')";
                    e.Row.Cells[columnMatter].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("matterTB").ClientID + "')";
                    e.Row.Cells[columnTimekeeper].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("timekeepTB").ClientID + "')";
                    e.Row.Cells[columnGLString].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("glstringTB").ClientID + "')";

                    if (e.Row.Cells[columnCostCode].Text.Equals("0")) e.Row.Cells[columnCostCode].Text = "-";
                    if (e.Row.Cells[columnDescription].Text.Equals("0")) e.Row.Cells[columnDescription].Text = "-";
                    if (e.Row.Cells[columnID].Text.Equals("0")) e.Row.Cells[columnID].Text = "-";

                    if (utility.GetStatus(Convert.ToInt16(e.Row.Cells[columnStatus].Text.ToString()), StatusCAS.InvalidCostCode))
                    {
                        e.Row.Cells[columnCostCode].Attributes.Add("class", "AlertCell");
                    }

                    if (utility.GetStatus(Convert.ToInt16(e.Row.Cells[columnStatus].Text.ToString()), StatusCAS.InvalidTimekeeper))
                    {
                        e.Row.Cells[columnTimekeeper].Attributes.Add("class", "AlertCell");
                    }

                    if (utility.GetStatus(Convert.ToInt16(e.Row.Cells[columnStatus].Text.ToString()), StatusCAS.InvalidClientMatter))
                    {
                        e.Row.Cells[columnMatter].Attributes.Add("class", "AlertCell");
                    }
                    else
                    {
                        if (utility.GetStatus(Convert.ToInt16(e.Row.Cells[columnStatus].Text.ToString()), StatusCAS.ClosedMatter))
                        {
                            e.Row.Cells[columnMatterStatus].Attributes.Add("class", "AlertCell");
                        }

                    }
                    if (utility.GetStatus(Convert.ToInt16(e.Row.Cells[columnStatus].Text.ToString()), StatusCAS.InactiveGLString)
                        || utility.GetStatus(Convert.ToInt16(e.Row.Cells[columnStatus].Text.ToString()), StatusCAS.InvalidGLString))
                    {
                        e.Row.Cells[columnGLString].Attributes.Add("class", "AlertCell");
                    }


                    if (utility.GetOverrideOfficeFlag(e.Row.Cells[columnID].Text))
                    {
                        ((TextBox)e.Row.FindControl("timekeepTB")).Attributes.Add("style", "color:#7D110C");  //.Attributes.Add("class", "OverriddenText");
                    }


                }
            }
            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                if ((GridViewSearchResults.PageIndex + 1) == GridViewSearchResults.PageCount)
                {
                    if (GridViewSearchResults.Rows.Count < VariablesCAS.NUMBER_ROWS_ON_FINAL_PAGE_TO_KEEP_FIXED_PAGERROW)
                    {
                        //This is to remove the fixed footer class when it is the last page
                        e.Row.CssClass = "";
                    }
                }

            }

        }


        protected void GridViewSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            BearCode bearCode = new BearCode();
            LabelSelectedMessage.Text = "Selected Row ID: " + GridViewSearchResults.SelectedRow.Cells[columnID].Text + "&nbsp;&nbsp;&nbsp;";
            LabelMatterLabel.Text = "Matter:";
            LabelMatter.Text = bearCode.GetMatterDesc1(
                ((TextBox)GridViewSearchResults.SelectedRow.FindControl("matterTB")).Text
                , VariablesCAS.ERROR_LOG_FILE_NAME);
            LabelTimekeeperLabel.Text = "Timekeeper:";
            LabelTimekeeper.Text = bearCode.GetTimekeeperInfo(
                ((TextBox)GridViewSearchResults.SelectedRow.FindControl("timekeepTB")).Text
                , "fullNameWithTkid"
                , VariablesCAS.ERROR_LOG_FILE_NAME);
            LabelCostCodeLabel.Text = "CostCode:";
            LabelCostCode.Text = utility.GetCostCodeDescription(
                ((TextBox)GridViewSearchResults.SelectedRow.FindControl("costcodeTB")).Text.Replace("'", "''"));
            ImageButtonDelete.ImageUrl = "~/images/controls/trash.gif";
            ImageButtonDetails.ImageUrl = "~/images/controls/magnifier.png";
            ImageButtonSearchMatterPanel.ImageUrl = "~/images/controls/search.gif";
            ImageButtonSearchTimekeeperPanel.ImageUrl = "~/images/controls/search.gif";
            ImageButtonSelectCostCodePanel.ImageUrl = "~/images/controls/search.gif";
            LinkButtonCloseSelected.Text = "X";
            if (Session["BulkUpdate"] != null)
            {
                if (Session["BulkUpdate"].ToString().Equals("On"))
                {
                    ImageButtonBulkUpdate.ImageUrl = "~/images/controls/bulk.gif";
                    ImageButtonBulkUpdate.ToolTip = "Display Bulk Update View";
                    blankSpan.InnerHtml = "&nbsp;&nbsp;";                
                }
                else
                {
                    ImageButtonBulkUpdate.ImageUrl = "~/images/controls/smallwhitedot.gif";
                    ImageButtonBulkUpdate.ToolTip = "";
                    blankSpan.InnerHtml = "";
                }
            }

            Session["MatterTBClientName"] = ((TextBox)GridViewSearchResults.SelectedRow.FindControl("matterTB")).ClientID.ToString();
            Session["TimekeeperTBClientName"] = ((TextBox)GridViewSearchResults.SelectedRow.FindControl("timekeepTB")).ClientID.ToString();
            Session["CostCodeTBClientName"] = ((TextBox)GridViewSearchResults.SelectedRow.FindControl("costcodeTB")).ClientID.ToString();
            
            if (Session["rowSelected"] != null)
            {
                if (!Session["rowSelected"].ToString().Equals( GridViewSearchResults.SelectedRow.RowIndex.ToString() ))
                {
                    HideAllPanels(true);
                }
            }
        }


        protected void LinkButtonChangeSearch_Click(object sender, EventArgs e)
        { //findme
            ToggleBulk(false);
            HideAllPanels(false);
            EraseGridViewSearchResults(true);
            ToggleApplicationButtons(false);
            DropDownListProcessIDs.Visible = false;
            DropDownListProcessIDs.SelectedValue = "-1";
            DropDownListVendors.SelectedValue = "-1";


            if (LabelSearchTitle.Text.Equals("Vendor"))
            {
                LabelSearchTitle.Text = "Timekeeper";
                LinkButtonChangeSearch.Text = "Run By Vendor";
                LabelTimekeeperEnter.Visible = true;
                ButtonSubmitTimekeeper.Visible = true;
                TextBoxTimekeeperID.Visible = true;
                TextBoxTimekeeperID.Text = "";
                DropDownListVendors.Visible = false;
            }
            else
            {
                LabelSearchTitle.Text = "Vendor";
                LinkButtonChangeSearch.Text = "Run By Timekeeper";
                LabelTimekeeperEnter.Visible = false;
                ButtonSubmitTimekeeper.Visible = false;
                TextBoxTimekeeperID.Visible = false;
                TextBoxTimekeeperID.Text = "00000";
                DropDownListVendors.Visible = true;
            }
        }

        
        protected void LinkButtonPanelsClick(object sender, EventArgs e)
        {
            HideAllPanels(true);
        }


        protected void LinkButtonCloseSelectedClick(object sender, EventArgs e)
        {
            HideAllPanels(false);
            EraseGridViewSearchResults(false);
        }


        protected void LinkButtonUpdate_Click(object sender, EventArgs e)
        {

        }


        protected void ButtonSubmitTimekeeper_Click(object sender, EventArgs e)
        {
            TextBoxTimekeeperID.Text = bearCode.AddLeadingZeros(TextBoxTimekeeperID.Text, "tkid");
            BindGrid(true);
        }



        /// <summary>
        /// archives delete in tblTempRecDeleted then deletes the record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonDeleteConfirm_Click(object sender, EventArgs e)
        {
            ButtonDeleteCancel.Visible = false;
            ButtonDeleteConfirm.Visible = false;

            SqlConnection con = null;

            String id = GridViewSearchResults.SelectedRow.Cells[columnID].Text;

            String sqlLogDelete = " INSERT INTO tblTempRecDeleted "
                              + " SELECT TempId "
                              + " ,vendId "
                              + " ,AppId "
                              + " ,processId "
                              + " ,InvoiceNo "
                              + " ,InvoiceDate "
                              + " ,client "
                              + " ,matter "
                              + " ,timekeep "
                              + " ,offc "
                              + " ,Amount "
                              + " ,Tdate "
                              + " ,CostCode "
                              + " ,status "
                              + " ,Description "
                              + " ,ProcessDate "
                              + " ,Quantity "
                              + " ,Rate "
                              + " ,Phone "
                              + " ,ApprovedBy "
                              + " ,DateApproved "
                              + " ,BeforeTrans "
                              + " ,'' as GLString " //hardcoded as an empty string.  The field exists in test, but not production
                              + " ,'" + Page.User.Identity.Name.ToString().Substring(8) + "' AS DeletededBy "
                              + " ,'" + DateTime.Now + "' AS DeletedDateTime "
                              + " FROM tblTempRec "
                              + " WHERE TempId = " + id;


            String sqlDelete = " DELETE FROM tblTempRec "
                              + " WHERE TempId = " + id;

            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["CASConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sqlLogDelete;
                command.ExecuteNonQuery();

                try
                {
                    command.CommandText = sqlDelete;
                    command.ExecuteNonQuery();
                }
                catch (SqlException sqleUpdate)
                {
                    Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqleUpdate.Message, "ButtonDeleteConfirm_Click()", sqlLogDelete);
                }

            }
            catch (SqlException sqleArchive)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqleArchive.Message, "ButtonDeleteConfirm_Click()", sqlDelete);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            labelDeleteRecord.Text = "Record " + id + " Deleted";
            EraseGridViewSearchResults(false);
            BindGrid(GridViewSearchResults.AllowPaging);

        }


        protected void ButtonDeleteCancel_Click(object sender, EventArgs e)
        {
            labelDeleteRecord.Text = "Record NOT Deleted";
            ButtonDeleteCancel.Visible = false;
            ButtonDeleteConfirm.Visible = false;
        }


        /// <summary>
        /// UPDATE dbo.tblTempRec Set client = '" + bearCode.AddLeadingZeros(TextBoxDetailsClient.Text, "client"), Quantity = " + TextBoxDetailsQuantity.Text WHERE tempid = " + GridViewSearchResults.SelectedRow.Cells[columnID].Text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonSaveDetails_Click(object sender, EventArgs e)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" UPDATE dbo.tblTempRec "
                       + " Set client = '" + bearCode.AddLeadingZeros(TextBoxDetailsClient.Text, "client") + "' ");

            if (TextBoxDetailsQuantity.Text.Equals(""))
            {
                sql.Append(" , Quantity = " + DBNull.Value );
            }
            else
            {
                sql.Append(" , Quantity = " + TextBoxDetailsQuantity.Text);
            }

            if (!DropDownListDetailsOffice.SelectedValue.Equals("-1") && DropDownListDetailsOffice.Enabled)
            {
                sql.Append(" , offc = '");
                sql.Append(DropDownListDetailsOffice.SelectedValue.ToString());
                sql.Append("' ");
            }
            
            sql.Append(" WHERE tempid = " + GridViewSearchResults.SelectedRow.Cells[columnID].Text);

            SqlConnection con = null;

            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["CASConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql.ToString();

                command.ExecuteNonQuery();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "ButtonSaveDetails_Click()", "");
            }
            finally
            {
                con.Close();
            }

            PopulateMoreDetails();


        }


        /// <summary>
        /// Sends 0 as the value for invalidTKOnly to the Stored Procedure
        /// SQL: <br />
        /// cas_BEAR_get_APCostRecItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButtonRecalc_Click(object sender, EventArgs e)
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["CASConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = new SqlCommand("cas_BEAR_get_APCostRecItem", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@vendor", DropDownListVendors.SelectedValue);
                command.Parameters.AddWithValue("@processid", DropDownListProcessIDs.SelectedValue);
                command.Parameters.AddWithValue("@invalidTKOnly", "0");
                command.Parameters.AddWithValue("@tkid", TextBoxTimekeeperID.Text.ToString());
                command.Parameters.AddWithValue("@costOrGL", "B");
                SqlDataReader reader = command.ExecuteReader();

                int tempId = 0;
                String timekeeper = "";
                String matter = "";
                String costCode = "";
                String glString = "";
                int status = 0;
                UtilityCAS util = new UtilityCAS();

                bool isCost = false;
                bool isGL = false;
                int i = 0;
                while (reader.Read())
                {
                    tempId = Convert.ToInt32(reader["TempId"].ToString());
                    timekeeper = reader["timekeep"].ToString();
                    matter = reader["matter"].ToString();
                    costCode = reader["CostCode"].ToString();
                    glString = reader["GLString"].ToString();

                    if (!glString.Equals(""))
                    {
                        isGL = true;
                    }
                    else if ((!matter.Equals("") || !costCode.Equals("")))
                    {
                        isCost = true;
                    }

                    if (isCost)
                    {
                        //hardcode 0 in method to indicate that the current status code = 0.  
                        //This is done to reset the code and then to recalculate.
                        status = util.GetStatusForSaveCost(0, timekeeper, matter, costCode);
                    }

                    if (isGL)
                    {
                        //hardcode 0 in method to indicate that the current status code = 0.  
                        //This is done to reset the code and then to recalculate.
                        status = util.GetStatusForSaveGL(0, glString);
                    }

                    //update the status for the current record
                    util.UpdateStatusCode(tempId, status);


                    i++;
                }

                if (i > 0)
                {
                    BindGrid(GridViewSearchResults.AllowPaging);
                    i = 0;
                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "ImageButtonRecalc_Click()", "");
            }
            finally
            {
                con.Close();
            }
            

        }


        protected void ImageButtonBulkUpdate_Click(object sender, EventArgs e)
        {
            HideAllPanels(false);
            PanelBulkUpdate.Visible = true;
        }


        protected void ImageButtonDelete_Click(object sender, EventArgs e)
        {
            HideAllPanels(false);
            PanelDeleteRecord.Visible = true;
            labelDeleteRecord.Text = "Delete this Record?";
            ButtonDeleteCancel.Visible = true;
            ButtonDeleteConfirm.Visible = true;
        }


        protected void ImageButtonDetails_Click(object sender, EventArgs e)
        {
            HideAllPanels(false);
            PanelDetails.Visible = true;
            PopulateMoreDetails();
            SetUpOfficeLocation(GridViewSearchResults.SelectedRow.Cells[columnID].Text);
        }


        protected void ImageButtonBulk_Click(object sender, EventArgs e)
        {
            if (Session["BulkUpdate"] != null)
            {
                HideAllPanels(false);

                if (Session["BulkUpdate"].ToString().Equals("Off"))
                {
                    ToggleBulk(true);
                    PanelBulkUpdate.Visible = true;
                }
                else
                {
                    ToggleBulk(false);
                }
            }
            else
            {
                ToggleBulk(false);
            }
        }


        protected void ImageButtonShowAllRows_Click(object sender, EventArgs e)
        {
            Session["PagerOn"] = false;
            ImageButtonPagerOn.Visible = true;
            ImageButtonShowAllRows.Visible = false;
            BindGrid(false);
        }


        protected void ImageButtonPagerOn_Click(object sender, EventArgs e)
        {
            Session["PagerOn"] = true;
            ImageButtonPagerOn.Visible = false;
            ImageButtonShowAllRows.Visible = true;
            BindGrid(true);
        }


        protected void ImageButtonSelectCostCodePanel_Click(object sender, EventArgs e)
        {
            String costCodeTextBoxToPopulate = "";
            if (Session["CostCodeTBClientName"] != null)
            {
                costCodeTextBoxToPopulate = Session["CostCodeTBClientName"].ToString();
            }

            HideAllPanels(false);
            PanelCostCodesSelect.Visible = true;
            RadioButtonListSearchResultsCostCode.Visible = true;
            RadioButtonListSearchResultsCostCode.SelectedIndex = -1;
            RadioButtonListSearchResultsCostCode.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsCostCode', '" + costCodeTextBoxToPopulate + "')");
        }
        

        protected void ImageButtonSearchMatterPanel_Click(object sender, EventArgs e)
        {
            String matterTextBoxToPopulate = "";
            if (Session["MatterTBClientName"] != null)
            {
                matterTextBoxToPopulate = Session["MatterTBClientName"].ToString();
            }

            HideAllPanels(false);
            PanelSearchMatter.Visible = true;
            RadioButtonListSearchResultsMatter.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsMatter', '" + matterTextBoxToPopulate + "')");
        }


        protected void ImageButtonBulkSearchTimekeeper_Click(object sender, EventArgs e)
        {
            HideAllPanels(true);
            PanelSearchTimekeeper.Visible = true;
            DivSearchTimekeeper.Attributes.Clear();
            DivSearchTimekeeper.Attributes.Add("class", "searchDivCASBulk");
            DivSearchResultsTimekeeper.Attributes.Clear();
            DivSearchResultsTimekeeper.Attributes.Add("class", "lookupResultsBulk");
            RadioButtonListSearchResultsTimekeeper.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsTimekeeper', 'TextBoxBulkTkid')");
        }


        protected void ImageButtonSearchTimekeeperPanel_Click(object sender, EventArgs e)
        {
            String timekeeperTextBoxToPopulate = "";
            if (Session["TimekeeperTBClientName"] != null)
            {
                timekeeperTextBoxToPopulate = Session["TimekeeperTBClientName"].ToString();
            }

            HideAllPanels(false);
            PanelSearchTimekeeper.Visible = true;
            DivSearchTimekeeper.Attributes.Clear();
            DivSearchTimekeeper.Attributes.Add("class", "searchDivCAS");
            DivSearchResultsTimekeeper.Attributes.Clear();
            DivSearchResultsTimekeeper.Attributes.Add("class", "lookupResults");
            RadioButtonListSearchResultsTimekeeper.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsTimekeeper', '" + timekeeperTextBoxToPopulate + "')");
        }


        /// <summary>
        /// Sets the visibility of the Matter search results (after a search is requested)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButtonSearchMatterClick(object sender, EventArgs e)
        {
            labelSearchResultsMatter.Text = "Select One:";
            RadioButtonListSearchResultsMatter.DataBind();
            RadioButtonListSearchResultsMatter.Visible = true;
            if (RadioButtonListSearchResultsMatter.Items.Count.ToString().Equals("0"))
            {
                labelSearchResultsMatter.Text = VariablesGlobal.SEARCH_NO_RESULTS_FOUND;
            }
        }


        /// <summary>
        /// Sets the visibility of the Timekeeper search results (after a search is requested)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButtonSearchTimekeeperClick(object sender, EventArgs e)
        {
            labelSearchResultsTimekeeper.Text = "Select One:";
            RadioButtonListSearchResultsTimekeeper.DataBind();
            RadioButtonListSearchResultsTimekeeper.Visible = true;
            if (RadioButtonListSearchResultsTimekeeper.Items.Count.ToString().Equals("0"))
            {
                labelSearchResultsTimekeeper.Text = VariablesGlobal.SEARCH_NO_RESULTS_FOUND;
            }
        }


        protected void DropDownListVendors_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PagerOn"] = null;
            Session["rowSelected"] = "-1";
            if (DropDownListVendors.SelectedValue.ToString().Equals("-1"))
            {
                DropDownListProcessIDs.Visible = false;
            }
            else
            {
                PopulateProcessIDs(DropDownListProcessIDs, Convert.ToInt32(DropDownListVendors.SelectedValue.ToString()));
                DropDownListProcessIDs.Visible = true;
            }
            ToggleBulk(false);
            EraseGridViewSearchResults(true);
            ToggleApplicationButtons(false); 
        }


        protected void DropDownListProcessIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PagerOn"] = null;
            Session["rowSelected"] = "-1";
            if (DropDownListProcessIDs.SelectedValue.ToString().Equals("-1"))
            {
                ToggleBulk(false);
                EraseGridViewSearchResults(true);
                ToggleApplicationButtons(false);
            }
            else
            {
                BindGrid(true);

            }

        }


        protected void RadioButtonListGLCost_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonIndexChangeEvent(RadioButtonListGLCost, RadioButtonListViewAll, "G", "2");
            if (RadioButtonListGLCost.SelectedValue.Equals("G"))
            {
                ImageButtonBulkSearchTimekeeper.Enabled = false;
                TextBoxBulkCostCode.Enabled = false;
                TextBoxBulkMatter.Enabled = false;
                TextBoxBulkTkid.Enabled = false;
                TextBoxBulkGLString.Enabled = true;
                TextBoxBulkCostCode.Attributes.CssStyle.Add("background-color", "#AAB0B7");
                TextBoxBulkMatter.Attributes.CssStyle.Add("background-color", "#AAB0B7");
                TextBoxBulkTkid.Attributes.CssStyle.Add("background-color", "#AAB0B7");
                TextBoxBulkGLString.Attributes.CssStyle.Add("background-color", "white");

            }
            else if (RadioButtonListGLCost.SelectedValue.Equals("C"))
            {
                ImageButtonBulkSearchTimekeeper.Enabled = true;
                TextBoxBulkCostCode.Enabled = true;
                TextBoxBulkMatter.Enabled = true;
                TextBoxBulkTkid.Enabled = true;
                TextBoxBulkGLString.Enabled = false;
                TextBoxBulkCostCode.Attributes.CssStyle.Add("background-color", "white");
                TextBoxBulkMatter.Attributes.CssStyle.Add("background-color", "white");
                TextBoxBulkTkid.Attributes.CssStyle.Add("background-color", "white");
                TextBoxBulkGLString.Attributes.CssStyle.Add("background-color", "#AAB0B7");

            }
        }


        protected void RadioButtonListViewAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonIndexChangeEvent(RadioButtonListViewAll,RadioButtonListGLCost, "2", "G");
        }


        protected void RadioButtonIndexChangeEvent(RadioButtonList rbl_parent, RadioButtonList rbl_child, String ParentItemValueToControlChild, String ChildItemTextToDisable)
        {
            bool pagerOn = true;
            GridViewSearchResults.SelectedIndex = -1;
            Session["rowSelected"] = "-1";
            if (Session["PagerOn"] != null)
            {
                pagerOn = Convert.ToBoolean(Session["PagerOn"].ToString());
            }
            BindGrid(pagerOn);

            for (int i = 0; i < rbl_child.Items.Count; i++)
            {
                if (rbl_child.Items[i].Value.Equals(ChildItemTextToDisable) && rbl_parent.SelectedValue.Equals(ParentItemValueToControlChild))
                {
                    rbl_child.Items[i].Enabled = false;
                }
                else
                {
                    rbl_child.Items[i].Enabled = true;
                }
            }

            EraseGridViewSearchResults(false);

        }


        protected void CheckBoxOverrideOffice_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Clears all data on the screen an resets button display
        /// </summary>
        /// <param name="hideSearchResults"></param>
        protected void EraseGridViewSearchResults(bool hideSearchResults)
        {
            GridViewSearchResults.SelectedIndex = -1;

            LabelSelectedMessage.Text = "";
            LabelMatterLabel.Text = "";
            LabelMatter.Text = "";
            LabelTimekeeperLabel.Text = "";
            LabelTimekeeper.Text = "";
            LabelCostCodeLabel.Text = "";
            LabelCostCode.Text = "";

            LinkButtonCloseSelected.Text = "";

            ImageButtonDelete.ImageUrl = "~/images/controls/smallwhitedot.gif";
            ImageButtonDetails.ImageUrl = "~/images/controls/smallwhitedot.gif";
            ImageButtonSearchMatterPanel.ImageUrl = "~/images/controls/smallwhitedot.gif";
            ImageButtonSearchTimekeeperPanel.ImageUrl = "~/images/controls/smallwhitedot.gif";
            ImageButtonSelectCostCodePanel.ImageUrl = "~/images/controls/smallwhitedot.gif";
            ImageButtonBulkUpdate.ImageUrl = "~/images/controls/smallwhitedot.gif";
            ImageButtonBulkUpdate.ToolTip = "";

            Session["MatterTBClientName"] = null;
            Session["TimekeeperTBClientName"] = null;
            Session["CostCodeTBClientName"] = null;
            HideAllPanels(true);

            if (hideSearchResults)
            {
                GridViewSearchResults.Visible = false;
                GridViewTotalAmount.Visible = false;
            }

        }


        /// <summary>
        /// Hides Search, Details, Cost Codes, Delete Panels.  Tests for BulkUpdate Mode to determine if it should be hidden or displayed
        /// </summary>
        /// <param name="testForBulkUpdateTurnedOn"></param>
        protected void HideAllPanels(bool testForBulkUpdateTurnedOn)
        {
            PanelSearchTimekeeper.Visible = false;
            PanelSearchMatter.Visible = false;
            PanelDetails.Visible = false;
            PanelCostCodesSelect.Visible = false;
            PanelDeleteRecord.Visible = false;
            if (testForBulkUpdateTurnedOn)
            {
                if (Session["BulkUpdate"] != null)
                {
                    if (Session["BulkUpdate"].ToString().Equals("On"))
                    {
                        PanelBulkUpdate.Visible = true;
                    }
                    else
                    {
                        PanelBulkUpdate.Visible = false;
                    }
                }
                else
                {
                    PanelBulkUpdate.Visible = false;
                }
            }
            else
            {
                PanelBulkUpdate.Visible = false;
            }

        }


        /// <summary>
        /// Toggle Bulk Mode
        /// </summary>
        /// <param name="activate"></param>
        protected void ToggleBulk(bool activate)
        {
            if (activate)
            {
                Session["BulkUpdate"] = "On";
                GridViewSearchResults.Columns[columnDescription].Visible = false;
                GridViewSearchResults.Columns[columnDescription2].Visible = true;
                GridViewSearchResults.Columns[columnAmount].Visible = false;
                GridViewSearchResults.Columns[columnBulkUpdate].Visible = true;
            }
            else
            {
                Session["BulkUpdate"] = "Off";
                GridViewSearchResults.Columns[columnDescription].Visible = true;
                GridViewSearchResults.Columns[columnDescription2].Visible = false; 
                GridViewSearchResults.Columns[columnAmount].Visible = true;
                GridViewSearchResults.Columns[columnBulkUpdate].Visible = false;
            }
        }


        /// <summary>
        /// Hide and Display - Save, Bulk, ShowAllRows / Pager Buttons
        /// </summary>
        /// <param name="activate">True = Display; False = Hide</param>
        protected void ToggleApplicationButtons(bool activate)
        {
            if (activate)
            {
                ImageButtonSave.Visible = true;
                //ImageButtonRecalc.Visible = true;  //change to true when the button coding returns...
                ImageButtonBulk.Visible = true;

                if (GridViewSearchResults.PageCount > 1)
                {
                    ImageButtonShowAllRows.Visible = true;
                    ImageButtonPagerOn.Visible = false;
                }
                else
                {
                    ImageButtonShowAllRows.Visible = false;
                    if (GridViewSearchResults.Rows.Count >= GridViewSearchResults.PageSize)
                    {
                        ImageButtonPagerOn.Visible = true;
                    }
                    else
                    {
                        ImageButtonPagerOn.Visible = false;
                    }
                }

            }
            else
            {
                ImageButtonSave.Visible = false;
                ImageButtonBulk.Visible = false;
                ImageButtonShowAllRows.Visible = false;
                ImageButtonPagerOn.Visible = false;
                RadioButtonListViewAll.Visible = false;
                RadioButtonListGLCost.Visible = false;
                ImageButtonRecalc.Visible = false;
            }
        }


        /// <summary>
        /// SQL<br />
        /// cas_BEAR_GetVendorsWithProcessId
        /// </summary>
        /// <param name="ddl">Name of DropDownList to populate</param>
        protected void PopulateVendors(DropDownList ddl)
        {
            SqlConnection con = null;
            List<ListItem> vendors = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["CASConnectionString"].ConnectionString); 
                con.Open();
                SqlCommand command = new SqlCommand("cas_BEAR_GetVendorsWithProcessId", con);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                vendors = new List<ListItem>();
                ListItem li = new ListItem();

                String vendorId = "";
                String vendorName = "";

                while (reader.Read())
                {
                    vendorId = reader["ID"].ToString();
                    vendorName = reader["Vendor"].ToString();

                    li = new ListItem(vendorName, vendorId, true);
                    vendors.Add(li);
                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "populateVendors()", "");
            }
            finally
            {
                con.Close();
            }

            for (int i = 0; i < vendors.Count; i++)
            {
                ddl.Items.Add(vendors[i]);
            }

        }


        /// <summary>
        /// Populates Office DropDownList.
        /// SQL is called from the BearCode class
        /// </summary>
        protected void PopulateOfficeLocations()
        {
            ListItem li = new ListItem("Select an Office", "-1");
            DropDownListDetailsOffice.Items.Add(li);

            BearCode bearCode = new BearCode();
            List<ListItem> offices = bearCode.GetLocations(VariablesCAS.ERROR_LOG_FILE_NAME);

            for (int i = 0; i < offices.Count; i++)
            {
                DropDownListDetailsOffice.Items.Add(offices[i]);
            }
        }


        /// <summary>
        /// Populates the Process ID DropDownList<br />
        /// SQL<br />
        /// cas_GetProcessIds_By_Vendor
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="vendor"></param>
        protected void PopulateProcessIDs(DropDownList ddl, int vendor)
        {
            ddl.Items.Clear();
            SqlConnection con = null;
            List<ListItem> vendors = new List<ListItem>();
            ListItem li = null;
            String selectionMessage = "---No Process IDs for Selected Vendor---";
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["CASConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = new SqlCommand("cas_GetProcessIds_By_Vendor", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@vendor", vendor);
                SqlDataReader reader = command.ExecuteReader();

                String processId = "";
                int counter = 0;

                while (reader.Read())
                {
                    counter++;
                    if (counter == 1)
                    {
                        selectionMessage = "---Please Select a Process ID---";
                    }

                    processId = reader["processId"].ToString();

                    li = new ListItem(processId, processId, true);
                    vendors.Add(li);
                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "populateVendors()", "");
            }
            finally
            {
                con.Close();
            }
            
            li = new ListItem(selectionMessage, "-1", true);
            ddl.Items.Add(li);
            
            for (int i = 0; i < vendors.Count; i++)
            {
                ddl.Items.Add(vendors[i]);
            }

        }


        /// <summary>
        /// Populates Client, Office and Quantity
        /// SQL:<br />
        /// SELECT isnull(client,'') as client  <br />
        /// ,isnull(offc,'') as offc  <br />
        /// ,isnull(Quantity,0) as quantity  <br />
        /// FROM dbo.tblTempRec <br />
        /// WHERE tempid = " + GridViewSearchResults.SelectedRow.Cells[columnID].Text <br />
        /// 
        /// </summary>
        protected void PopulateMoreDetails()
        {
            String sql = " SELECT isnull(client,'') as client "
                       +       " ,isnull(offc,'') as offc "
                       +       " ,isnull(Quantity,0) as quantity "
                       + " FROM dbo.tblTempRec"
                       + " WHERE tempid = " + GridViewSearchResults.SelectedRow.Cells[columnID].Text;

            SqlConnection con = null;
            String client = "";
            String offc = "";
            String quantity = "";

            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["CASConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    client = reader["client"].ToString();
                    offc = reader["offc"].ToString();
                    quantity = reader["quantity"].ToString();

                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesCAS.ERROR_LOG_FILE_NAME, sqle.Message, "populateVendors()", "");
            }
            finally
            {
                con.Close();
            }

            TextBoxDetailsClient.Text = client;
            TextBoxDetailsQuantity.Text = quantity;

            for (int i = 0; i < DropDownListDetailsOffice.Items.Count; i++)
            {
                if (DropDownListDetailsOffice.Items[i].Value.ToString().Equals(offc))
                {
                    DropDownListDetailsOffice.Items[i].Selected = true;
                }
                else
                {
                    DropDownListDetailsOffice.Items[i].Selected = false;
                }
            }

            if (DropDownListDetailsOffice.SelectedValue.Equals("-1"))
            {
                DropDownListDetailsOffice.Enabled = true;
            }
            else
            {
                if (CheckBoxOverrideOffice.Checked)
                {
                    DropDownListDetailsOffice.Enabled = true;
                }
                else
                {
                    DropDownListDetailsOffice.Enabled = false;
                }
            }

        }


        /// <summary>
        /// Sets the office override flag and value.  Sets the onClick event for CheckBoxOverrideOffice
        /// </summary>
        /// <param name="id"></param>
        protected void SetUpOfficeLocation(String id)
        {
            if (utility.GetOverrideOfficeFlag(id))
            {
                CheckBoxOverrideOffice.Checked = true;
                DropDownListDetailsOffice.Enabled = true;
            }
            else
            {
                CheckBoxOverrideOffice.Checked = false;
                DropDownListDetailsOffice.Enabled = false;
            }
            CheckBoxOverrideOffice.Attributes.Add("onClick", "overrideOfficeLocation()");
        }


        /// <summary>
        /// If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
        /// StoredProcedure parameters are set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SqlDataSource_OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@vendor", DropDownListVendors.SelectedValue.ToString()));
            e.Command.Parameters.Add(new SqlParameter("@processid", DropDownListProcessIDs.SelectedValue.ToString()));
            e.Command.Parameters.Add(new SqlParameter("@invalidTKOnly", RadioButtonListViewAll.SelectedValue));
            e.Command.Parameters.Add(new SqlParameter("@tkid", TextBoxTimekeeperID.Text.ToString()));
            e.Command.Parameters.Add(new SqlParameter("@costOrGL", RadioButtonListGLCost.SelectedValue));
        }


        /// <summary>
        /// StoredProcedure parameters are set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SqlDataSourceCAS_ProcessIDTotal_OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@vendor", DropDownListVendors.SelectedValue.ToString()));
            e.Command.Parameters.Add(new SqlParameter("@processid", DropDownListProcessIDs.SelectedValue.ToString()));
        }


        /// <summary>
        /// If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
        /// 
        /// StoredProcedure parameters are set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SqlDataSource_Matter_EscapeSingleQuote(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchvar", textBoxSearchMatter.Text.Replace("'", "''")));
        }


        /// <summary>
        /// If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
        /// 
        /// StoredProcedure parameters are set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SqlDataSource_Timekeeper_EscapeSingleQuote(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchvar", textBoxSearchTimekeeper.Text.Replace("'", "''")));
            e.Command.Parameters.Add(new SqlParameter("@tkeflag", ""));
            e.Command.Parameters.Add(new SqlParameter("@requireCostFlag", "Y"));
        }

    }
}
