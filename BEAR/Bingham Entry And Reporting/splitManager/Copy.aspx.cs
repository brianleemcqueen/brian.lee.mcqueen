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
    public partial class Copy : System.Web.UI.Page
    {

        protected UtilitySplitManager utility;


        protected void Page_Load(object sender, EventArgs e)
        {
            utility = new UtilitySplitManager();
            utility.UserName = Page.User.Identity.Name.ToString().Substring(8);
        }

        protected void ButtonHome_Click(object sender, EventArgs e)
        {
            Response.Redirect(VariablesSplitManager.HOME_PAGE);
        }

        protected void TextBoxMasterMatter_TextChanged(object sender, EventArgs e)
        {
            Session["parentId"] = null;
        }

        protected void ButtonGetStartDates_Click(object sender, EventArgs e)
        {
            BearCode bearCode = new BearCode();
            String MasterMatterNumber = bearCode.AddLeadingZeros(TextBoxMasterMatter.Text, "matter");

            DropDownListStartDates.Items.Clear();
            TextBoxMasterMatter.Text = MasterMatterNumber;
            bearCode.PopulateMasterMatterStartDates(DropDownListStartDates, MasterMatterNumber, VariablesSplitManager.ERROR_LOG_FILE_NAME);
            LabelStartDates.Visible = true;
            DropDownListStartDates.Visible = true;
            LabelMasterMatterDescription.Text = bearCode.GetMatterDesc1(MasterMatterNumber, VariablesSplitManager.ERROR_LOG_FILE_NAME);
        }

        protected void DropDownListStartDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListStartDates.SelectedValue.Equals("-1"))
            {
                PanelSearchMasterMatter.Visible = false;
                LabelNewStartDate.Visible = false;
                LabelNewStartDateInstructions.Visible = false;
                TextBoxNewStartDate.Visible = false;
                ButtonCancel.Visible = false;
                ButtonSave.Visible = false;
                LabelAdminMessage.Text = "";
            }
            else
            {
                Session["parentId"] = utility.GetMasterMatterId(TextBoxMasterMatter.Text, DropDownListStartDates.SelectedValue);
                PanelSearchMasterMatter.Visible = false;
                ButtonCancel.Visible = true;
                LabelAdminMessage.Text = "";
                ButtonSave.Visible = true;
                LabelNewStartDate.Visible = true;
                LabelNewStartDateInstructions.Visible = true;
                TextBoxNewStartDate.Visible = true;
                ButtonSave.Text = "Make Copy";
            }

        }

        protected void ImageButtonMasterMatter_Click(object sender, EventArgs e)
        {
            PanelSearchMasterMatter.Visible = true;
            DropDownListStartDates.Items.Clear();
            DropDownListStartDates.Visible = false;
            LabelStartDates.Visible = false;
            LabelNewStartDate.Visible = false;
            LabelNewStartDateInstructions.Visible = false;
            TextBoxNewStartDate.Text = "";
            TextBoxNewStartDate.Visible = false;
            ButtonSave.Visible = false;
            ButtonCancel.Visible = false;
            LabelAdminMessage.Text = "";
            RadioButtonListSearchResultsMasterMatter.Attributes.Add("onClick", "RadioButtonToTextBox('RadioButtonListSearchResultsMasterMatter', 'TextBoxMasterMatter')");
        }



        protected void LinkButtonCloseSearchClick(object sender, EventArgs e)
        {
            PanelSearchMasterMatter.Visible = false;
        }

        protected void ImageButtonSearchMasterMatter_Click(object sender, EventArgs e)
        {
            LabelSearchResultsMasterMatter.Text = "Select One:";
            RadioButtonListSearchResultsMasterMatter.DataBind();
            RadioButtonListSearchResultsMasterMatter.Visible = true;
            if (RadioButtonListSearchResultsMasterMatter.Items.Count.ToString().Equals("0"))
            {
                LabelSearchResultsMasterMatter.Text = VariablesGlobal.SEARCH_NO_RESULTS_FOUND;
            }
        }

        protected void SqlDataSource_MasterMatter_EscapeSingleQuote(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchvar", TextBoxSearchMasterMatter.Text.Replace("'", "''")));
        }

        /// <summary>
        /// uses Stored Procedures: uspBMcBEARSplitManagerCountHeaderRecords & uspBMcBEARSplitManagerCopySplit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            SqlConnection con = null;

            String MasterMatter = TextBoxMasterMatter.Text;

            BearCode bearCode = new BearCode();
            String NewStartDate = TextBoxNewStartDate.Text;

            if (bearCode.IsDate(NewStartDate))
            {
                try
                {
                    con = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                    con.Open();
                    SqlCommand command = con.CreateCommand();
                    command.CommandText = "uspBMcBEARSplitManagerCountHeaderRecords";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@matter", MasterMatter);
                    command.Parameters.AddWithValue("@startDate", NewStartDate);
                    SqlDataReader reader = command.ExecuteReader();
                    bool IsHeaderUnique = false;
                    if (reader.Read())
                    {
                        int countHeaderRecords = Convert.ToInt16(reader["countHeaderRecords"].ToString());
                        if (countHeaderRecords == 0)
                        {
                            IsHeaderUnique = true;
                        }
                        else
                        {
                            LabelAdminMessage.Text = "This Date Range / Master Matter<br />combination already exists";
                        }
                    }
                    reader.Close();

                    if (IsHeaderUnique)
                    {
                        try
                        {
                            command.CommandText = "uspBMcBEARSplitManagerCopySplit";
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@matter", MasterMatter);
                            command.Parameters.AddWithValue("@startDateCopyFrom", DropDownListStartDates.SelectedValue);
                            command.Parameters.AddWithValue("@startDateNew", NewStartDate);
                            command.Parameters.AddWithValue("@updatedBy", utility.UserName);
                            command.ExecuteNonQuery();

                            LabelAdminMessage.Text = "Record has been copied.";

                        }
                        catch (SqlException sqle)
                        {
                            Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "ButtonSave_Click()", "Copying Split");
                            LabelAdminMessage.Text = "Error in Copy<br />" + sqle.Message;
                        }

                    }
                }
                catch (SqlException sqle)
                {
                    Logger.QuickLog(VariablesSplitManager.ERROR_LOG_FILE_NAME, sqle.Message, "ButtonSave_Click()", "Checking if HeaderRecords is Unique");
                }
                finally
                {
                    if (con != null)
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                LabelAdminMessage.Text = "Please Enter a Valid New Start Date";
            }

        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            TextBoxMasterMatter.Text = "";
            LabelMasterMatterDescription.Text = "";
            LabelStartDates.Visible = false;
            LabelNewStartDate.Visible = false;
            LabelNewStartDateInstructions.Visible = false;
            DropDownListStartDates.Items.Clear();
            DropDownListStartDates.Visible = false;
            TextBoxNewStartDate.Text = "";
            TextBoxNewStartDate.Visible = false;
            ButtonSave.Visible = false;
            ButtonCancel.Visible = false;
            LabelAdminMessage.Text = "";
        }




    }
}
