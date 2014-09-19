using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

namespace BEAR.rat
{
    public partial class Default : System.Web.UI.Page
    {
        protected int columnID = 1;
        protected int columnClient = 2;
        protected int columnClientName = 3;
        protected int columnBillingPointPerson = 4;
        protected int columnScope = 5;
        protected int columnClientEngagementPartners = 6;
        protected int columnNotes = 7;
        protected int columnEffectedClients = 8;
        protected int columnResponseDueDate = 9;
        protected int columnSentTo = 10;
        protected int columnProfitabilityAttached = 11;
        protected int columnApprovalComments = 12;
        protected int columnAgreementDescription = 13;
        protected int columnEliteComments = 14;
        protected int columnConfirmRelationshipPartner = 15;
        protected int columnCommunicatedToPartners = 16;
        protected int columnCommunicatedToBilling = 17;
        protected int columnInspectedBy = 18;
        protected int columnAssignedToClientAcctg = 19;
        protected int columnSetUpComplete = 20;
        protected int columnProjectedRealizationAgreementMatters = 21;
        protected int columnClientUDF = 22;
        protected int columnExtendNoticeVolumeDiscount = 23;
        protected int columnBillingManagerReview = 24;
        protected int columnCostWriteOffsSentToClientAcctg = 25;
        protected int columnAgreementStartDate = 26;
        protected int columnAgreementEndDate = 27;
        protected int columnActualRealization = 28;
        protected int columnFinalized = 29;
        protected int columnModifiedBy = 30;
        protected int columnDelete = 31;

        protected UtilityRAT utility;
        protected BearCode bearCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            utility = new UtilityRAT();
            bearCode = new BearCode();

            if (Page.IsPostBack)
            {
                int totalRows = 0;
                
                try
                {
                    totalRows = GridViewSearchResults.Rows.Count;
                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(VariablesRAT.ERROR_LOG_FILE_NAME, sqle.Message, "Page_Load()", "int totalRows = dataGridView.Rows.Count");
                }
                utility.SetRowChanged(totalRows);

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
                Session["rowSelected"] = "-1";
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
                //int selectedRowIndex = GridViewSearchResults.SelectedIndex;

                int totalRows = GridViewSearchResults.Rows.Count;
                if (totalRows > 0)
                {
                    for (int r = 0; r < totalRows; r++)
                    {
                        if (utility.GetRowChanged()[r])
                        {

                            GridViewRow thisGridViewRow = GridViewSearchResults.Rows[r];

                            String id = "";
                            String billingPointPerson = "";
                            String scope = "";
                            String clientEngagementPartners = "";
                            String notes = "";
                            String effectedClients = "";
                            String responseDueDate = "";
                            String sentTo = "";
                            bool profitabilityAttached = false;
                            String approvalComments = "";
                            String agreementDescription = "";
                            String eliteComments = "";
                            bool confirmRelationshipPartner = false;
                            bool communicatedToPartners = false;
                            bool communicatedToBilling = false;
                            String inspectedBy = "";
                            String assignedToClientAcctg = "";
                            bool setUpComplete = false;
                            String projectedRealizationAgreementMatters = "";
                            String clientUDF = "";
                            bool extendNoticeVolumeDiscount = false;
                            bool billingManagerReview = false;
                            bool costWriteOffSentToClientAcctg = false;
                            String agreementStartDate = "";
                            String agreementEndDate = "";
                            String actualRealization = "";
                            bool finalized = false;
                            String modifiedBy = Page.User.Identity.Name.ToString().Substring(8);

                            String status = RadioButtonListStatus.SelectedValue.ToString();

                            try
                            {
                                //Global Column
                                id = ((Label)thisGridViewRow.FindControl("idLabel")).Text.ToString();
                                billingPointPerson = ((TextBox)thisGridViewRow.FindControl("billingPointPersonTB")).Text.Replace("'", "''");
                                scope = ((TextBox)thisGridViewRow.FindControl("scopeTB")).Text.Replace("'", "''");
                                clientEngagementPartners = ((TextBox)thisGridViewRow.FindControl("cepTB")).Text.Replace("'", "''");
                                notes = ((TextBox)thisGridViewRow.FindControl("notesTB")).Text.Replace("'", "''");
                                effectedClients = ((Label)thisGridViewRow.FindControl("effectedClientsLabel")).Text.ToString();
                                finalized = ((CheckBox)thisGridViewRow.FindControl("finalizedCB")).Checked;

                                if (!status.Equals("P"))
                                {
                                    //Finalized Columns
                                    agreementDescription = ((TextBox)thisGridViewRow.FindControl("agreementDescriptionTB")).Text.Replace("'", "''");
                                    eliteComments = ((TextBox)thisGridViewRow.FindControl("eliteCommentsTB")).Text.Replace("'", "''");
                                    confirmRelationshipPartner = ((CheckBox)thisGridViewRow.FindControl("crpCB")).Checked;
                                    communicatedToPartners = ((CheckBox)thisGridViewRow.FindControl("ctpCB")).Checked;
                                    communicatedToBilling = ((CheckBox)thisGridViewRow.FindControl("ctbCB")).Checked;
                                    inspectedBy = ((TextBox)thisGridViewRow.FindControl("inspectedByTB")).Text.Replace("'", "''");
                                    assignedToClientAcctg = ((TextBox)thisGridViewRow.FindControl("atcaTB")).Text.Replace("'", "''");
                                    setUpComplete = ((CheckBox)thisGridViewRow.FindControl("setUpCompleteCB")).Checked;
                                    projectedRealizationAgreementMatters = ((TextBox)thisGridViewRow.FindControl("pramTB")).Text.Replace("'", "''");
                                    clientUDF = ((TextBox)thisGridViewRow.FindControl("clientUDFTB")).Text.Replace("'", "''");
                                    extendNoticeVolumeDiscount = ((CheckBox)thisGridViewRow.FindControl("envdCB")).Checked;
                                    billingManagerReview = ((CheckBox)thisGridViewRow.FindControl("bmrCB")).Checked;
                                    costWriteOffSentToClientAcctg = ((CheckBox)thisGridViewRow.FindControl("cwostcaCB")).Checked;
                                    agreementStartDate = ((TextBox)thisGridViewRow.FindControl("agreementStartDateTB")).Text.Replace("'", "''");
                                    agreementEndDate = ((TextBox)thisGridViewRow.FindControl("agreementEndDateTB")).Text.Replace("'", "''");
                                    actualRealization = ((TextBox)thisGridViewRow.FindControl("actualRealizationTB")).Text.Replace("'", "''");
                                }
                                if (!status.Equals("F"))
                                {
                                    //Proposal Columns
                                    responseDueDate = ((TextBox)thisGridViewRow.FindControl("rddTB")).Text.Replace("'", "''");
                                    sentTo = ((TextBox)thisGridViewRow.FindControl("sentToTB")).Text.Replace("'", "''");
                                    profitabilityAttached = ((CheckBox)thisGridViewRow.FindControl("profitabilityCB")).Checked;
                                    approvalComments = ((TextBox)thisGridViewRow.FindControl("approvalCommentsTB")).Text.Replace("'", "''");
                                }

                            }
                            catch (FormatException)
                            {
                                //this is to catch forecast when set to a nonNumber
                                //non numbers are converted to zero
                            }
                            catch (ArgumentOutOfRangeException ae)
                            {
                                //when using paging
                                Logger.QuickLog(VariablesRAT.ERROR_LOG_FILE_NAME, ae.Message, "Page_PreRender()", "");
                            }

                            SqlConnection con = null;

                            String sqlArchive = " INSERT INTO BMcBEARRateAgreementTrackerArchive " +
                                                " SELECT id " +
                                                    " ,client " +
                                                    " ,clientName " +
                                                    " ,billingPointPerson " +
                                                    " ,scope " +
                                                    " ,notes " +
                                                    " ,clientEngagementPartners " +
                                                    " ,responseDueDate " +
                                                    " ,sentTo " +
                                                    " ,profitabilityAttached " +
                                                    " ,finalized " +
                                                    " ,approvalComments " +
                                                    " ,effectedClients " +
                                                    " ,agreementDescription " +
                                                    " ,eliteComments " +
                                                    " ,confirmRelationshipPartner " +
                                                    " ,communicatedToPartners " +
                                                    " ,communicatedtoBilling " +
                                                    " ,inspectedBy " +
                                                    " ,assignedToClientAcctg " +
                                                    " ,setUpComplete " +
                                                    " ,projectedRealizationAgreementMatters " +
                                                    " ,clientUDF " +
                                                    " ,extendNoticeVolumeDiscount " +
                                                    " ,billingManagerReview " +
                                                    " ,costWriteOffsSentToClientAcctg " +
                                                    " ,agreementStartDate " +
                                                    " ,agreementEndDate " +
                                                    " ,actualRealization " +
                                                    " ,modifiedBy " +
                                                    " ,updateTime " +
                                                " FROM dbo.BMcBEARRateAgreementTracker (nolock) " +
                                                " WHERE id = " + id;



                            String sqlUpdate = " UPDATE BMcBEARRateAgreementTracker "
                                              + " SET billingPointPerson = '" + billingPointPerson + "' "
                                                  + " ,scope = '" + scope + "' "
                                                  + " ,notes = '" + notes + "' "
                                                  + " ,clientEngagementPartners = '" + clientEngagementPartners + "' "
                                                  + " ,effectedClients = '" + effectedClients + "' "
                                                  + " ,finalized = '" + finalized + "' ";

                            if (!status.Equals("F"))
                            {
                                sqlUpdate = sqlUpdate
                                                  + " ,responseDueDate = '" + responseDueDate + "' "
                                                  + " ,sentTo = '" + sentTo + "' "
                                                  + " ,profitabilityAttached = '" + profitabilityAttached + "' "
                                                  + " ,approvalComments = '" + approvalComments + "' ";
                            }
                            if (!status.Equals("P"))
                            {
                                sqlUpdate = sqlUpdate
                                                  + " ,agreementDescription = '" + agreementDescription + "' "
                                                  + " ,eliteComments = '" + eliteComments + "' "
                                                  + " ,confirmRelationshipPartner = '" + confirmRelationshipPartner + "' "
                                                  + " ,communicatedToPartners = '" + communicatedToPartners + "' "
                                                  + " ,communicatedtoBilling = '" + communicatedToBilling + "' "
                                                  + " ,inspectedBy = '" + inspectedBy + "' "
                                                  + " ,assignedToClientAcctg = '" + assignedToClientAcctg + "' "
                                                  + " ,setUpComplete = '" + setUpComplete + "' "
                                                  + " ,projectedRealizationAgreementMatters = '" + projectedRealizationAgreementMatters + "' "
                                                  + " ,clientUDF = '" + clientUDF + "' "
                                                  + " ,extendNoticeVolumeDiscount = '" + extendNoticeVolumeDiscount + "' "
                                                  + " ,billingManagerReview = '" + billingManagerReview + "' "
                                                  + " ,costWriteOffsSentToClientAcctg = '" + costWriteOffSentToClientAcctg + "' "
                                                  + " ,agreementStartDate = '" + agreementStartDate + "' "
                                                  + " ,agreementEndDate = '" + agreementEndDate + "' "
                                                  + " ,actualRealization = '" + actualRealization + "' ";
                            }



                            sqlUpdate = sqlUpdate
                                                  + " ,modifiedBy = '" + Page.User.Identity.Name.ToString().Substring(8) + "' "
                                                  + " ,updateTime = '" + DateTime.Now + "' "
                                                  + " WHERE id = " + id;

                            try
                            {
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
                                    Logger.QuickLog(VariablesRAT.ERROR_LOG_FILE_NAME, sqleUpdate.Message, "Page_PreRender()", sqlUpdate);
                                }

                            }
                            catch (SqlException sqleArchive)
                            {
                                Logger.QuickLog(VariablesRAT.ERROR_LOG_FILE_NAME, sqleArchive.Message, "Page_PreRender()", sqlArchive);
                            }
                            finally
                            {
                                if (con != null)
                                {
                                    con.Close();
                                }
                            }


                        } //end "if (rowChanged[r])"

                    } //end "for (int r = 0; r < totalRows; r++)"
                }

                GridViewSearchResults.DataBind();

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
                    Logger.QuickLog(VariablesRAT.ERROR_LOG_FILE_NAME, sqle.Message, "BindGrid()", "int totalRows = dataGridView.Rows.Count");
                    totalRows = 0;
                }


            } //end if (Page.IsPostBack)

        }


        protected void GridViewSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            LabelMessage.Text = "";
            RadioButtonListNotesType.Items.Clear();
            RadioButtonListNotesType.Items.Add(new ListItem("Clients", "Clients"));
            RadioButtonListNotesType.Items.Add(new ListItem("Notes", "Notes"));
            if (!RadioButtonListStatus.SelectedValue.Equals("F"))
            {
                RadioButtonListNotesType.Items.Add(new ListItem("Approval", "ApprovalComments"));
            }
            if (!RadioButtonListStatus.SelectedValue.Equals("P"))
            {
                RadioButtonListNotesType.Items.Add(new ListItem("Agreement", "Agreement"));
                RadioButtonListNotesType.Items.Add(new ListItem("Elite", "EliteComments"));
            }
            RadioButtonListNotesType.Visible = true;
            LinkButtonCloseNotesHistory.Visible = true;
            if (RadioButtonListNotesType.SelectedIndex != -1)
            {
                DisplaySideDetails();
            }

        }


        protected void GridViewSearchResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (RadioButtonListStatus.SelectedIndex == -1)
            {
                GridViewSearchResults.Visible = false;
            }
            else
            {
                GridViewSearchResults.Visible = true;
                if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
                {

                    if (RadioButtonListStatus.SelectedValue.ToString().Equals("P"))
                    {
                        //Hide Finalized Columns
                        e.Row.Cells[columnAgreementDescription].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnAgreementDescription].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnEliteComments].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnEliteComments].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnConfirmRelationshipPartner].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnConfirmRelationshipPartner].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnCommunicatedToPartners].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnCommunicatedToPartners].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnCommunicatedToBilling].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnCommunicatedToBilling].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnInspectedBy].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnInspectedBy].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnAssignedToClientAcctg].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnAssignedToClientAcctg].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnSetUpComplete].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnSetUpComplete].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnProjectedRealizationAgreementMatters].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnProjectedRealizationAgreementMatters].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnClientUDF].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnClientUDF].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnExtendNoticeVolumeDiscount].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnExtendNoticeVolumeDiscount].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnBillingManagerReview].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnBillingManagerReview].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnCostWriteOffsSentToClientAcctg].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnCostWriteOffsSentToClientAcctg].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnAgreementStartDate].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnAgreementStartDate].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnAgreementEndDate].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnAgreementEndDate].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnActualRealization].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnActualRealization].Attributes.CssStyle.Add("visibility", "hidden");
                    }
                    else if (RadioButtonListStatus.SelectedValue.ToString().Equals("F"))
                    {
                        //Hide Proposal Columns
                        e.Row.Cells[columnResponseDueDate].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnResponseDueDate].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnSentTo].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnSentTo].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnProfitabilityAttached].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnProfitabilityAttached].Attributes.CssStyle.Add("visibility", "hidden");
                        e.Row.Cells[columnApprovalComments].Attributes.CssStyle.Add("display", "none");
                        e.Row.Cells[columnApprovalComments].Attributes.CssStyle.Add("visibility", "hidden");
                    }
                    else
                    {
                        //Show All Columns 

                    }


                    if (e.Row.RowType == DataControlRowType.DataRow)
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
                        e.Row.Cells[columnClient].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                                                        (this.GridViewSearchResults, "Select$" + e.Row.RowIndex);
                        e.Row.Cells[columnModifiedBy].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                                                        (this.GridViewSearchResults, "Select$" + e.Row.RowIndex);
                        e.Row.Cells[columnEffectedClients].Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                                                        (this.GridViewSearchResults, "Select$" + e.Row.RowIndex);
                        
                        e.Row.Cells[columnBillingPointPerson].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("billingPointPersonTB").ClientID + "')";
                        e.Row.Cells[columnScope].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("scopeTB").ClientID + "')";
                        e.Row.Cells[columnResponseDueDate].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("rddTB").ClientID + "')";
                        e.Row.Cells[columnSentTo].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("sentToTB").ClientID + "')";
                        e.Row.Cells[columnCommunicatedToBilling].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("ctbCB").ClientID + "')";
                        e.Row.Cells[columnInspectedBy].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("inspectedByTB").ClientID + "')";
                        e.Row.Cells[columnAssignedToClientAcctg].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("atcaTB").ClientID + "')";
                        e.Row.Cells[columnProjectedRealizationAgreementMatters].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("pramTB").ClientID + "')";
                        e.Row.Cells[columnClientUDF].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("clientUDFTB").ClientID + "')";
                        e.Row.Cells[columnAgreementStartDate].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("agreementStartDateTB").ClientID + "')";
                        e.Row.Cells[columnAgreementEndDate].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("agreementEndDateTB").ClientID + "')";
                        e.Row.Cells[columnActualRealization].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("actualRealizationTB").ClientID + "')";

                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        ((LinkButton)e.Row.Cells[columnID].FindControl("LinkButtonCancel")).Attributes.Add("onclick", "javascript:return confirm('Cancel Insert?')");
                        e.Row.Cells[columnClient].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("clientTBF").ClientID + "')";
                        e.Row.Cells[columnClientName].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("clientNameTBF").ClientID + "')";
                        e.Row.Cells[columnBillingPointPerson].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("billingPointPersonTBF").ClientID + "')";
                        e.Row.Cells[columnScope].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("scopeTBF").ClientID + "')";
                        e.Row.Cells[columnClientEngagementPartners].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("cepTBF").ClientID + "')";
                        e.Row.Cells[columnNotes].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("notesTBF").ClientID + "')";
                        e.Row.Cells[columnEffectedClients].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("effectedClientsTBF").ClientID + "')";
                        e.Row.Cells[columnResponseDueDate].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("rddTBF").ClientID + "')";
                        e.Row.Cells[columnSentTo].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("sentToTBF").ClientID + "')";
                        e.Row.Cells[columnApprovalComments].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("approvalCommentsTBF").ClientID + "')";
                        e.Row.Cells[columnAgreementDescription].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("agreementDescriptionTBF").ClientID + "')";
                        e.Row.Cells[columnEliteComments].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("eliteCommentsTBF").ClientID + "')";
                        e.Row.Cells[columnCommunicatedToBilling].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("ctbCBF").ClientID + "')";
                        e.Row.Cells[columnInspectedBy].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("inspectedByTBF").ClientID + "')";
                        e.Row.Cells[columnAssignedToClientAcctg].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("atcaTBF").ClientID + "')";
                        e.Row.Cells[columnProjectedRealizationAgreementMatters].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("pramTBF").ClientID + "')";
                        e.Row.Cells[columnClientUDF].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("clientUDFTBF").ClientID + "')";
                        e.Row.Cells[columnAgreementStartDate].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("agreementStartDateTBF").ClientID + "')";
                        e.Row.Cells[columnAgreementEndDate].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("agreementEndDateTBF").ClientID + "')";
                        e.Row.Cells[columnActualRealization].Attributes["onclick"] = "GiveControlFocus('" + e.Row.FindControl("actualRealizationTBF").ClientID + "')";

                    }

                }

            }

        }


        protected void DisplayClientSelection()
        {
            CheckBoxListClients.Items.Clear();
            RadioButtonListClientType.Visible = true;
            CheckBoxListClients.Items.Add(new ListItem("Client 1", "1"));
            CheckBoxListClients.Items.Add(new ListItem("Client 2", "2"));
            CheckBoxListClients.Items.Add(new ListItem("Client 3", "3"));
            CheckBoxListClients.Items.Add(new ListItem("Client 4", "4"));
            CheckBoxListClients.Items.Add(new ListItem("Client 5", "5"));
            CheckBoxListClients.Items.Add(new ListItem("Client 6", "6"));
            CheckBoxListClients.Items.Add(new ListItem("Client 7", "7"));
        }
        
        
        protected void LinkButtonCloseNotesHistoryClick(object sender, EventArgs e)
        {
            GridViewSearchResults.SelectedIndex = -1;
            HideSelectedDetails();
            LabelMessage.Text = "";
        }


        protected void HideSelectedDetails()
        {
            LabelSelectedMessage.Text = "";
            LabelNotesHistory.Text = "";
            LinkButtonCloseNotesHistory.Visible = false;
            RadioButtonListNotesType.Visible = false;
            RadioButtonListNotesType.SelectedIndex = -1;
            CheckBoxListClients.Items.Clear();
            RadioButtonListClientType.Visible = false;
        }


        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            utility.GetRowChanged()[row] = true;
        }


        protected void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            CheckBox thisCheckBox = (CheckBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisCheckBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            utility.GetRowChanged()[row] = true;
        }


        protected void RadioButtonListStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewSearchResults.SelectedIndex = -1;
            HideSelectedDetails();
        }


        protected void RadioButtonListNotesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySideDetails();
        }


        protected void DisplaySideDetails()
        {
            String NotesType = RadioButtonListNotesType.SelectedValue.ToString();
            

            if (NotesType.Equals("Clients"))
            {
                LabelSelectedMessage.Text = "Select Affected Clients";
                LabelNotesHistory.Text = "";
                DisplayClientSelection();
            }
            else
            {
                CheckBoxListClients.Items.Clear();
                RadioButtonListClientType.Visible = false;
                String field = "";
                if (NotesType.Equals("Notes"))
                {
                    LabelSelectedMessage.Text = "Notes History";
                    field = "notes";
                }
                else if (NotesType.Equals("ApprovalComments"))
                {
                    LabelSelectedMessage.Text = "Approval Comments";
                    field = "approvalComments";
                }
                else if (NotesType.Equals("Agreement"))
                {
                    LabelSelectedMessage.Text = "Agreement Description";
                    field = "agreementDescription";
                }
                else if (NotesType.Equals("EliteComments"))
                {
                    LabelSelectedMessage.Text = "Elite Comments";
                    field = "eliteComments";
                }
                DisplayNotes(field);
            }

        }
            

        


        protected void DisplayNotes(String field)
        {
            String notesHistory = "";
            GridViewRow thisGridViewRow = GridViewSearchResults.SelectedRow;

            SqlConnection con = null;

            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                String sql = "uspBMcBEARRateAgreementTrackerNotesHistory";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", ((Label)thisGridViewRow.FindControl("idLabel")).Text.ToString());
                command.Parameters.AddWithValue("@field", field);
                SqlDataReader reader = command.ExecuteReader();
                int counter = 1;
                while (reader.Read())
                {
                    if (reader["Type"].ToString().Equals("Current"))
                    {
                        notesHistory = "<b><u>Current Note</u></b><br />" +
                                       reader["updateTime"].ToString() + " (" + reader["modifiedBy"].ToString() + "): " +
                                       reader["notes"].ToString();
                        counter++;
                    }
                    else
                    {
                        if (counter == 2)
                        {
                            notesHistory = notesHistory +
                                "<br /><hr /><br />" +
                                "<b><u>Notes Log</u></b><br />";
                        }

                        notesHistory = notesHistory +
                            reader["updateTime"].ToString() + " (" + reader["modifiedBy"].ToString() + "): " +
                            reader["notes"].ToString() +
                            "<br /><br />";

                        counter++;
                    }
                }


            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesRAT.ERROR_LOG_FILE_NAME, sqle.Message, "GridViewSearchResults_SelectedIndexChanged()", "");
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            LabelNotesHistory.Text = notesHistory;
        }


        protected void LinkButtonInsert_Click(object sender, EventArgs e)
        {
            GridViewRow thisGridViewFooterRow = GridViewSearchResults.FooterRow;
            String clientNumber = ((TextBox)thisGridViewFooterRow.Cells[columnClient].FindControl("clientTBF")).Text.ToString();
            bool clientNumberOk = false;
            String message = "";
            if (!clientNumber.Equals(""))
            {
                try
                {
                    int clientNumberTest = int.Parse(clientNumber);
                    clientNumberOk = true;
                }
                catch (FormatException)
                {
                    message = "Client Number is Invalid.";
                }
            }
            else
            {
                message = "Client Number is Required";
            }

            if (clientNumberOk)
            {
                InsertNewRecord();
                LabelMessage.Text = "New Record Inserted";
            }
            else
            {
                LabelMessage.Text = message;
            }

        }


        protected void InsertNewRecord()
        {

            
            GridViewRow thisGridViewRow = GridViewSearchResults.FooterRow;

            String client = "";
            String clientName = "";
            String billingPointPerson = "";
            String scope = "";
            String clientEngagementPartners = "";
            String notes = "";
            String effectedClients = "";
            String responseDueDate = "";
            String sentTo = "";
            bool profitabilityAttached = false;
            String approvalComments = "";
            String agreementDescription = "";
            String eliteComments = "";
            bool confirmRelationshipPartner = false;
            bool communicatedToPartners = false;
            bool communicatedToBilling = false;
            String inspectedBy = "";
            String assignedToClientAcctg = "";
            bool setUpComplete = false;
            String projectedRealizationAgreementMatters = "";
            String clientUDF = "";
            bool extendNoticeVolumeDiscount = false;
            bool billingManagerReview = false;
            bool costWriteOffSentToClientAcctg = false;
            String agreementStartDate = "";
            String agreementEndDate = "";
            String actualRealization = "";
            bool finalized = false;
            String modifiedBy = Page.User.Identity.Name.ToString().Substring(8);

            String status = RadioButtonListStatus.SelectedValue.ToString();

            try
            {
                //Global Column
                client = ((TextBox)thisGridViewRow.FindControl("clientTBF")).Text.Replace("'", "''");
                clientName = ((TextBox)thisGridViewRow.FindControl("clientNameTBF")).Text.Replace("'", "''");
                billingPointPerson = ((TextBox)thisGridViewRow.FindControl("billingPointPersonTBF")).Text.Replace("'", "''");
                scope = ((TextBox)thisGridViewRow.FindControl("scopeTBF")).Text.Replace("'", "''");
                clientEngagementPartners = ((TextBox)thisGridViewRow.FindControl("cepTBF")).Text.Replace("'", "''");
                notes = ((TextBox)thisGridViewRow.FindControl("notesTBF")).Text.Replace("'", "''");
                effectedClients = ((TextBox)thisGridViewRow.FindControl("effectedClientsTBF")).Text.Replace("'", "''");
                finalized = ((CheckBox)thisGridViewRow.FindControl("finalizedCBF")).Checked;

                if (!status.Equals("P"))
                {
                    //Finalized Columns
                    agreementDescription = ((TextBox)thisGridViewRow.FindControl("agreementDescriptionTBF")).Text.Replace("'", "''");
                    eliteComments = ((TextBox)thisGridViewRow.FindControl("eliteCommentsTBF")).Text.Replace("'", "''");
                    confirmRelationshipPartner = ((CheckBox)thisGridViewRow.FindControl("crpCBF")).Checked;
                    communicatedToPartners = ((CheckBox)thisGridViewRow.FindControl("ctpCBF")).Checked;
                    communicatedToBilling = ((CheckBox)thisGridViewRow.FindControl("ctbCBF")).Checked;
                    inspectedBy = ((TextBox)thisGridViewRow.FindControl("inspectedByTBF")).Text.Replace("'", "''");
                    assignedToClientAcctg = ((TextBox)thisGridViewRow.FindControl("atcaTBF")).Text.Replace("'", "''");
                    setUpComplete = ((CheckBox)thisGridViewRow.FindControl("setUpCompleteCBF")).Checked;
                    projectedRealizationAgreementMatters = ((TextBox)thisGridViewRow.FindControl("pramTBF")).Text.Replace("'", "''");
                    clientUDF = ((TextBox)thisGridViewRow.FindControl("clientUDFTBF")).Text.Replace("'", "''");
                    extendNoticeVolumeDiscount = ((CheckBox)thisGridViewRow.FindControl("envdCBF")).Checked;
                    billingManagerReview = ((CheckBox)thisGridViewRow.FindControl("bmrCBF")).Checked;
                    costWriteOffSentToClientAcctg = ((CheckBox)thisGridViewRow.FindControl("cwostcaCBF")).Checked;
                    agreementStartDate = ((TextBox)thisGridViewRow.FindControl("agreementStartDateTBF")).Text.Replace("'", "''");
                    agreementEndDate = ((TextBox)thisGridViewRow.FindControl("agreementEndDateTBF")).Text.Replace("'", "''");
                }
                if (!status.Equals("F"))
                {
                    //Proposal Columns
                    responseDueDate = ((TextBox)thisGridViewRow.FindControl("rddTBF")).Text.Replace("'", "''");
                    sentTo = ((TextBox)thisGridViewRow.FindControl("sentToTBF")).Text.Replace("'", "''");
                    profitabilityAttached = ((CheckBox)thisGridViewRow.FindControl("profitabilityCBF")).Checked;
                    approvalComments = ((TextBox)thisGridViewRow.FindControl("approvalCommentsTBF")).Text.Replace("'", "''");
                }

            }
            catch (FormatException)
            {
                //this is to catch forecast when set to a nonNumber
                //non numbers are converted to zero
            }
            catch (ArgumentOutOfRangeException ae)
            {
                //when using paging
                Logger.QuickLog(VariablesRAT.ERROR_LOG_FILE_NAME, ae.Message, "Page_PreRender()", "");
            }

            SqlConnection con = null;

            String sqlInsert = " INSERT INTO dbo.BMcBEARRateAgreementTracker ( " +
                                    "  client " +
                                    " ,clientName " +
                                    " ,billingPointPerson " +
                                    " ,scope " +
                                    " ,notes " +
                                    " ,clientEngagementPartners " +
                                    " ,responseDueDate " +
                                    " ,sentTo " +
                                    " ,profitabilityAttached " +
                                    " ,finalized " +
                                    " ,approvalComments " +
                                    " ,effectedClients " +
                                    " ,agreementDescription " +
                                    " ,eliteComments " +
                                    " ,confirmRelationshipPartner " +
                                    " ,communicatedToPartners " +
                                    " ,communicatedtoBilling " +
                                    " ,inspectedBy " +
                                    " ,assignedToClientAcctg " +
                                    " ,setUpComplete " +
                                    " ,projectedRealizationAgreementMatters " +
                                    " ,clientUDF " +
                                    " ,extendNoticeVolumeDiscount " +
                                    " ,billingManagerReview " +
                                    " ,costWriteOffsSentToClientAcctg " +
                                    " ,agreementStartDate " +
                                    " ,agreementEndDate " +
                                    " ,actualRealization " +
                                    " ,modifiedBy " +
                                    " ,updateTime) " +
                                " VALUES ( " +
                                    " '" + client + "', " +
                                    " '" + clientName + "', " +
                                    " '" + billingPointPerson + "', " +
                                    " '" + scope + "', " +
                                    " '" + notes + "', " +
                                    " '" + clientEngagementPartners + "', " +
                                    " '" + responseDueDate + "', " +
                                    " '" + sentTo + "', " +
                                    " '" + profitabilityAttached + "', " +
                                    " '" + finalized + "', " +
                                    " '" + approvalComments + "', " +
                                    " '" + effectedClients + "', " +
                                    " '" + agreementDescription + "', " +
                                    " '" + eliteComments + "', " +
                                    " '" + confirmRelationshipPartner + "', " +
                                    " '" + communicatedToPartners + "', " +
                                    " '" + communicatedToBilling + "', " +
                                    " '" + inspectedBy + "', " +
                                    " '" + assignedToClientAcctg + "', " +
                                    " '" + setUpComplete + "', " +
                                    " '" + projectedRealizationAgreementMatters + "', " +
                                    " '" + clientUDF + "', " +
                                    " '" + extendNoticeVolumeDiscount + "', " +
                                    " '" + billingManagerReview + "', " +
                                    " '" + costWriteOffSentToClientAcctg + "', " +
                                    " '" + agreementStartDate + "', " +
                                    " '" + agreementEndDate + "', " +
                                    " '" + actualRealization + "', " +
                                    " '" + Page.User.Identity.Name.ToString().Substring(8) + "', " +
                                    " '" + DateTime.Now + "' " +
                                ")";

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
            catch (SqlException sqleArchive)
            {
                Logger.QuickLog(VariablesRAT.ERROR_LOG_FILE_NAME, sqleArchive.Message, "InsertNewRecord()", sqlInsert);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }


        }

        
        protected void LinkButtonCancel_Click(object sender, EventArgs e)
        {
            CancelInsert();
        }

        
        protected void CancelInsert()
        {


            GridViewRow thisGridViewRow = GridViewSearchResults.FooterRow;

            String client = "";
            String clientName = "";
            String billingPointPerson = "";
            String scope = "";
            String clientEngagementPartners = "";
            String notes = "";
            String effectedClients = "";
            String responseDueDate = "";
            String sentTo = "";
            bool profitabilityAttached = false;
            String approvalComments = "";
            String agreementDescription = "";
            String eliteComments = "";
            bool confirmRelationshipPartner = false;
            bool communicatedToPartners = false;
            bool communicatedToBilling = false;
            String inspectedBy = "";
            String assignedToClientAcctg = "";
            bool setUpComplete = false;
            String projectedRealizationAgreementMatters = "";
            String clientUDF = "";
            bool extendNoticeVolumeDiscount = false;
            bool billingManagerReview = false;
            bool costWriteOffSentToClientAcctg = false;
            String agreementStartDate = "";
            String agreementEndDate = "";
            String actualRealization = "";
            bool finalized = false;

            String status = RadioButtonListStatus.SelectedValue.ToString();

            try
            {
                //Global Column
                ((TextBox)thisGridViewRow.FindControl("clientTBF")).Text = client;
                ((TextBox)thisGridViewRow.FindControl("clientNameTBF")).Text = clientName;
                ((TextBox)thisGridViewRow.FindControl("billingPointPersonTBF")).Text = billingPointPerson;
                ((TextBox)thisGridViewRow.FindControl("scopeTBF")).Text = scope;
                ((TextBox)thisGridViewRow.FindControl("cepTBF")).Text = clientEngagementPartners;
                ((TextBox)thisGridViewRow.FindControl("notesTBF")).Text = notes;
                ((TextBox)thisGridViewRow.FindControl("effectedClientsTBF")).Text = effectedClients;
                ((CheckBox)thisGridViewRow.FindControl("finalizedCBF")).Checked = finalized;

                if (!status.Equals("P"))
                {
                    //Finalized Columns
                    ((TextBox)thisGridViewRow.FindControl("agreementDescriptionTBF")).Text = agreementDescription;
                    ((TextBox)thisGridViewRow.FindControl("eliteCommentsTBF")).Text = eliteComments;
                    ((CheckBox)thisGridViewRow.FindControl("crpCBF")).Checked = confirmRelationshipPartner;
                    ((CheckBox)thisGridViewRow.FindControl("ctpCBF")).Checked = communicatedToPartners;
                    ((CheckBox)thisGridViewRow.FindControl("ctbCBF")).Checked = communicatedToBilling;
                    ((TextBox)thisGridViewRow.FindControl("inspectedByTBF")).Text = inspectedBy;
                    ((TextBox)thisGridViewRow.FindControl("atcaTBF")).Text = assignedToClientAcctg;
                    ((CheckBox)thisGridViewRow.FindControl("setUpCompleteCBF")).Checked = setUpComplete;
                    ((TextBox)thisGridViewRow.FindControl("pramTBF")).Text = projectedRealizationAgreementMatters;
                    ((TextBox)thisGridViewRow.FindControl("clientUDFTBF")).Text = clientUDF;
                    ((CheckBox)thisGridViewRow.FindControl("envdCBF")).Checked = extendNoticeVolumeDiscount;
                    ((CheckBox)thisGridViewRow.FindControl("bmrCBF")).Checked = billingManagerReview;
                    ((CheckBox)thisGridViewRow.FindControl("cwostcaCBF")).Checked = costWriteOffSentToClientAcctg;
                    ((TextBox)thisGridViewRow.FindControl("agreementStartDateTBF")).Text = agreementStartDate;
                    ((TextBox)thisGridViewRow.FindControl("agreementEndDateTBF")).Text = agreementEndDate;
                    ((TextBox)thisGridViewRow.FindControl("actualRealizationTBF")).Text = actualRealization;
                }
                if (!status.Equals("F"))
                {
                    //Proposal Columns
                    ((TextBox)thisGridViewRow.FindControl("rddTBF")).Text = responseDueDate;
                    ((TextBox)thisGridViewRow.FindControl("sentToTBF")).Text = sentTo;
                    ((CheckBox)thisGridViewRow.FindControl("profitabilityCBF")).Checked = profitabilityAttached;
                    ((TextBox)thisGridViewRow.FindControl("approvalCommentsTBF")).Text = approvalComments;
                }

            }

            catch (ArgumentOutOfRangeException ae)
            {
                //when using paging
                Logger.QuickLog(VariablesRAT.ERROR_LOG_FILE_NAME, ae.Message, "CancelInsert()", "");
            }


        }
        
                
        /// <summary>
        /// If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
        /// StoredProcedure parameters are set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SqlDataSource_OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@clientNumber", "All"));
            String status = "N";
            if (RadioButtonListStatus.SelectedIndex != -1)
            {
                status = RadioButtonListStatus.SelectedValue.ToString();
            }
            e.Command.Parameters.Add(new SqlParameter("@status", status));
        }


    }
}
