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


namespace BEAR.search
{
    public partial class tokyoclientmatter : System.Web.UI.Page
    {
        protected BearCode bearCode = new BearCode();
        protected int columnCurrency = 1;
        protected int columnLanguage = 2;
        protected int columnClientNumber = 3;
        protected int columnClientName = 4;
        protected int columnNTIClientNumber = 5;
        protected int columnBSMClientNumber = 6;
        protected int columnJapaneseClientName = 7;
        protected int columnMatterNumber = 8;
        protected int columnMatterDescription = 9;
        protected int columnNTIMatterNumber = 10;
        protected int columnBSMMatterNumber = 11;
        protected int columnJapaneseMatterDescription = 12;
        protected int columnLocationCode = 13;
        protected int columnMatterStatus = 14;
        protected int columnBsmOrNti = 15;
        protected int columnOverseasMatter = 16;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                GridViewSearchResults.SelectedIndex = -1;
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
                Session["defaultColumnWidth11"] = Hidden10.Value.ToString();
                Session["defaultColumnWidth12"] = Hidden11.Value.ToString();
                Session["defaultColumnWidth13"] = Hidden12.Value.ToString();
                Session["defaultColumnWidth14"] = Hidden13.Value.ToString();
                Session["defaultColumnWidth15"] = Hidden14.Value.ToString();
                Session["defaultColumnWidth16"] = Hidden15.Value.ToString();

                bearCode.PopulateListLocations(RadioButtonListOffices, "matter", VariablesSearchToykoClientMatter.ERROR_LOG_FILE_NAME);
                RadioButtonListOffices.Attributes.Add("onClick", "RadioButtonAppendTextBox('RadioButtonListOffices','TextBoxOffices')");

                TextBoxOffices.Attributes.Add("onmouseover", "Tip('" + VariablesSearchToykoClientMatter.TOOLTIP_TEXTBOXOFFICES + "')");
                TextBoxOffices.Attributes.Add("onmouseout", "UnTip()");
                //ImageButtonResetColumnWidths.Attributes.Add("onmouseover", "Tip('" + VariablesSearchToykoClientMatter.TOOLTIP_RESIZE_COLUMNS + "')");
                //ImageButtonResetColumnWidths.Attributes.Add("onmouseout", "UnTip()");
                ImageButtonSearch.Attributes.Add("onmouseover", "Tip('" + VariablesSearchToykoClientMatter.TOOLTIP_SEARCH_EXECUTE + "')");
                ImageButtonSearch.Attributes.Add("onmouseout", "UnTip()");

            }

            CheckBoxAllOffices.Attributes.Add("onClick", "ToggleOffices()");
        }


        protected void imageButtonSearchClick(object sender, EventArgs e)
        {
            GridViewSearchResults.DataBind();
            GridViewSearchResults.PageIndex = 0;
            GridViewSearchResults.Visible = true;
            ImageButtonResetColumnWidths.Visible = true;
        }

       
        protected void imageButtonResetColumnWidthsClick(object sender, EventArgs e)
        {
            try
            {
                Hidden0.Value = Session["defaultColumnWidth1"].ToString();
                Hidden1.Value = Session["defaultColumnWidth2"].ToString();
                Hidden2.Value = Session["defaultColumnWidth3"].ToString();
                Hidden3.Value = Session["defaultColumnWidth4"].ToString();
                Hidden4.Value = Session["defaultColumnWidth5"].ToString();
                Hidden5.Value = Session["defaultColumnWidth6"].ToString();
                Hidden6.Value = Session["defaultColumnWidth7"].ToString();
                Hidden7.Value = Session["defaultColumnWidth8"].ToString();
                Hidden8.Value = Session["defaultColumnWidth9"].ToString();
                Hidden9.Value = Session["defaultColumnWidth10"].ToString();
                Hidden10.Value = Session["defaultColumnWidth11"].ToString();
                Hidden11.Value = Session["defaultColumnWidth12"].ToString();
                Hidden12.Value = Session["defaultColumnWidth13"].ToString();
                Hidden13.Value = Session["defaultColumnWidth14"].ToString();
                Hidden14.Value = Session["defaultColumnWidth15"].ToString();
                Hidden15.Value = Session["defaultColumnWidth16"].ToString();

                GridViewSearchResults.HeaderRow.Cells[1].Attributes.Add("width", Hidden0.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[2].Attributes.Add("width", Hidden1.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[3].Attributes.Add("width", Hidden2.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[4].Attributes.Add("width", Hidden3.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[5].Attributes.Add("width", Hidden4.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[6].Attributes.Add("width", Hidden5.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[7].Attributes.Add("width", Hidden6.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[8].Attributes.Add("width", Hidden7.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[9].Attributes.Add("width", Hidden8.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[10].Attributes.Add("width", Hidden9.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[11].Attributes.Add("width", Hidden10.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[12].Attributes.Add("width", Hidden11.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[13].Attributes.Add("width", Hidden12.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[14].Attributes.Add("width", Hidden13.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[15].Attributes.Add("width", Hidden14.Value.ToString());
                GridViewSearchResults.HeaderRow.Cells[16].Attributes.Add("width", Hidden15.Value.ToString());
            }
            catch (NullReferenceException nre)
            {
                Logger.QuickLog(VariablesSearchToykoClientMatter.ERROR_LOG_FILE_NAME, nre.Message, "BEAR.toykoclientmatter.imageButtonResetColumnWidthsClick()", "");
            }

        }
        

        protected void GridViewSearchResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
                e.Row.Cells[11].Attributes.Add("width", Hidden10.Value.ToString());
                e.Row.Cells[12].Attributes.Add("width", Hidden11.Value.ToString());
                e.Row.Cells[13].Attributes.Add("width", Hidden12.Value.ToString());
                e.Row.Cells[14].Attributes.Add("width", Hidden13.Value.ToString());
                e.Row.Cells[15].Attributes.Add("width", Hidden14.Value.ToString());
                e.Row.Cells[16].Attributes.Add("width", Hidden15.Value.ToString());
            }

            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                                                (this.GridViewSearchResults, "Select$" + e.Row.RowIndex);

                if (e.Row.Cells[columnClientNumber].Text.Equals("0")) e.Row.Cells[columnClientNumber].Text = "-";
                if (e.Row.Cells[columnNTIClientNumber].Text.Equals("0")) e.Row.Cells[columnNTIClientNumber].Text = "-";
                if (e.Row.Cells[columnBSMClientNumber].Text.Equals("0")) e.Row.Cells[columnBSMClientNumber].Text = "-";
                if (e.Row.Cells[columnMatterNumber].Text.Equals("0")) e.Row.Cells[columnMatterNumber].Text = "-";
                if (e.Row.Cells[columnNTIMatterNumber].Text.Equals("0")) e.Row.Cells[columnNTIMatterNumber].Text = "-";
                if (e.Row.Cells[columnBSMMatterNumber].Text.Equals("0")) e.Row.Cells[columnBSMMatterNumber].Text = "-";

                e.Row.Cells[columnOverseasMatter].Text = e.Row.Cells[columnOverseasMatter].Text.Equals("1") ? "Yes" : "No";

                if (e.Row.Cells[columnMatterStatus].Text.Equals("CL"))
                {
                    for (int i = 0; i<15; i++)
                    {
                        e.Row.Cells[i].Attributes.Add("style", "color: #820505");
                    }
                }

            }

            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                bearCode.GridViewCustomizePagerRow(GridViewSearchResults, e.Row);
            }
        }


        protected void LinkButtonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                String JapaneseMatterNameOld = "";
                String JapaneseClientNameOld = "";
                int OverseasMatterOld = 0;

                if (Session["JapaneseMatterNameOld"] != null)
                {
                    JapaneseMatterNameOld = Session["JapaneseMatterNameOld"].ToString();
                }
                if (Session["JapaneseClientNameOld"] != null)
                {
                    JapaneseClientNameOld = Session["JapaneseClientNameOld"].ToString();
                }
                if (Session["OverseasMatterOld"] != null)
                {
                    if (Session["OverseasMatterOld"].ToString().Equals("1"))
                    {
                        OverseasMatterOld = 1;
                    }
                    else
                    {
                        OverseasMatterOld = 0;
                    }

                }

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eliteConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "uspBMcBEARSearchTokyoClientMatterUpdate";

                command.Parameters.AddWithValue("@clientNumber", LabelSelectedClientNumber.Text.Trim());

                if (CheckBoxOverseasMatter.Checked)
                {
                    command.Parameters.AddWithValue("@overseasMatterNew", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@overseasMatterNew", 0);
                }
                command.Parameters.AddWithValue("@overseasMatterOld", 0);

                if (TextBoxSelectedJapaneseClientName.Text.Equals(""))
                {
                    command.Parameters.AddWithValue("@clientNameJapanese", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@clientNameJapanese", TextBoxSelectedJapaneseClientName.Text);
                }
                command.Parameters.AddWithValue("@JapaneseClientNameOld", JapaneseClientNameOld);

                command.Parameters.AddWithValue("@matterNumber", LabelSelectedMatterNumber.Text.Trim());
                
                if (TextBoxSelectedJapaneseMatterDescription.Text.Equals(""))
                {
                    command.Parameters.AddWithValue("@matterDescriptionJapanese", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@matterDescriptionJapanese", TextBoxSelectedJapaneseMatterDescription.Text);
                }
                
                command.Parameters.AddWithValue("@JapaneseMatterNameOld", JapaneseMatterNameOld);

                command.Parameters.AddWithValue("@changeDate", DateTime.Now);
                command.Parameters.AddWithValue("@userId", Page.User.Identity.Name.ToString().Substring(8));

                command.ExecuteNonQuery();

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesSearchToykoClientMatter.ERROR_LOG_FILE_NAME, "LinkButtonUpdate_Click()", sqle.Message, "");
            }

            Session["JapaneseMatterNameOld"] = TextBoxSelectedJapaneseMatterDescription.Text;
            Session["JapaneseClientNameOld"] = TextBoxSelectedJapaneseClientName.Text;
            Session["OverseasMatterOld"] = CheckBoxOverseasMatter.Checked ? "1" : "0";

            GridViewSearchResults.DataBind();

        }


        protected void GridViewSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["JapaneseMatterNameOld"] = GridViewSearchResults.SelectedRow.Cells[columnJapaneseMatterDescription].Text.Replace("&nbsp;", "");
            Session["JapaneseClientNameOld"] = GridViewSearchResults.SelectedRow.Cells[columnJapaneseClientName].Text.Replace("&nbsp;", "");
            Session["OverseasMatterOld"] = GridViewSearchResults.SelectedRow.Cells[columnOverseasMatter].Text.ToUpper().Equals("YES") ? "1" : "0";

            TextBoxSelectedJapaneseClientName.Attributes.Add("style", "display: inline");
            TextBoxSelectedJapaneseMatterDescription.Attributes.Add("style", "display: inline");
            CheckBoxOverseasMatter.Attributes.Add("style", "display: inline");
            bearCode.GridViewCustomizePagerRow(GridViewSearchResults, GridViewSearchResults.BottomPagerRow);
            LinkButtonUpdate.Text = "Save";
            LabelSelectedMessage.Text = "Most Recently Selected Row:&nbsp;&nbsp;&nbsp;";
            LabelSelectedClientNameLabel.Text = "Client Name:";
            LabelSelectedClientName.Text = GridViewSearchResults.SelectedRow.Cells[columnClientName].Text;
            LabelSelectedJapaneseClientNameLabel.Text = "";
            TextBoxSelectedJapaneseClientName.Text = GridViewSearchResults.SelectedRow.Cells[columnJapaneseClientName].Text.Replace("&nbsp;","");
            LabelSelectedClientNumberLabel.Text = "Client #:";
            LabelSelectedClientNumber.Text = GridViewSearchResults.SelectedRow.Cells[columnClientNumber].Text;
            LabelSelectedNTIClientNumber.Text = "NTI Client #:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnNTIClientNumber].Text;
            LabelSelectedBSMClientNumber.Text = "BSM Client #:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnBSMClientNumber].Text;
            LabelSelectedMatterDescription.Text = "Matter:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnMatterDescription].Text;
            LabelSelectedJapaneseMatterDescriptionLabel.Text = "";
            TextBoxSelectedJapaneseMatterDescription.Text = GridViewSearchResults.SelectedRow.Cells[columnJapaneseMatterDescription].Text.Replace("&nbsp;", "");
            LabelSelectedMatterNumberLabel.Text = "Matter # (" + GridViewSearchResults.SelectedRow.Cells[columnMatterStatus].Text + "):";
            LabelSelectedMatterNumber.Text = GridViewSearchResults.SelectedRow.Cells[columnMatterNumber].Text;
            LabelSelectedNTIMatterNumber.Text = "NTI Matter #:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnNTIMatterNumber].Text;
            LabelSelectedBSMMatterNumber.Text = "BSM Matter #:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnBSMMatterNumber].Text;
            LabelOffice.Text = "Matter Location:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnLocationCode].Text + " - " + bearCode.GetLocationDescription(GridViewSearchResults.SelectedRow.Cells[columnLocationCode].Text, VariablesSearchToykoClientMatter.ERROR_LOG_FILE_NAME);
            LabelOverseasMatter.Text = "Overseas Matter?";
            CheckBoxOverseasMatter.Checked = GridViewSearchResults.SelectedRow.Cells[columnOverseasMatter].Text.Equals("Yes") ? true : false;
            ImageButtonUpdate.ImageUrl = "~/images/controls/disk.png";

            GridViewSearchResults.HeaderRow.Cells[1].Attributes.Add("width", Hidden0.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[2].Attributes.Add("width", Hidden1.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[3].Attributes.Add("width", Hidden2.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[4].Attributes.Add("width", Hidden3.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[5].Attributes.Add("width", Hidden4.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[6].Attributes.Add("width", Hidden5.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[7].Attributes.Add("width", Hidden6.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[8].Attributes.Add("width", Hidden7.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[9].Attributes.Add("width", Hidden8.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[10].Attributes.Add("width", Hidden9.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[11].Attributes.Add("width", Hidden10.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[12].Attributes.Add("width", Hidden11.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[13].Attributes.Add("width", Hidden12.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[14].Attributes.Add("width", Hidden13.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[15].Attributes.Add("width", Hidden14.Value.ToString());
            GridViewSearchResults.HeaderRow.Cells[16].Attributes.Add("width", Hidden15.Value.ToString());

        }


        /*
         * If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
         */
        protected void SqlDataSource_OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters.Add(new SqlParameter("@searchTxt", textBoxSearch.Text.Replace("'", "''")));
            e.Command.Parameters.Add(new SqlParameter("@searchFrom", RadioButtonListSearchFrom.SelectedValue));

            String offices = "";
            if (!CheckBoxAllOffices.Checked)
            {
                String[] textBoxValues = TextBoxOffices.Text.Replace("'", "").Replace(";", ",").Split(',');
                foreach (string s in textBoxValues)
                {
                    if (s.Length == 1)
                    {
                        offices += "'0" + s.Trim() + "',";
                    }
                    else
                    {
                        offices += "'" + s.Trim() + "',";
                    }
                }
                //to close off the text for the IN operand in the SQL statement.
                offices += "'-1'";
            }
            else
            {
                offices = "All";
            }

            if (offices.Equals("'','-1'") || offices.Equals("") || offices.Equals("'All','-1'"))
            {
                offices = "All";
            }


            e.Command.Parameters.Add(new SqlParameter("@offices", offices));

            String includeClosedMatters = "N";
            if (CheckBoxClosedMatters.Checked)
            {
                includeClosedMatters = "Y";
            }

            e.Command.Parameters.Add(new SqlParameter("@closedMatters", includeClosedMatters));

        }

        
    }
}
