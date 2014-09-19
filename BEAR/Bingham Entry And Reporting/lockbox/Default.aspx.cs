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

namespace BEAR.lockbox
{
    public partial class _default : System.Web.UI.Page
    {
        protected String nextPageImport = "processImportedFile.aspx";
        protected String nextPageExport = "processExport.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            uploadControl.uploadFolder = Server.MapPath("uploadedFiles/");
            uploadControl.nextPage = nextPageImport;
        }

        protected void ButtonImport_Click(object sender, EventArgs e)
        {
            Session["process"] = "import";
            uploadControl.Visible = true;
            SelectGroup.Visible = false;
            DropDownListEdit.Visible = false;
            RadioButtonListTestOrFinal.Visible = false;
        }

        protected void ButtonExport_Click(object sender, EventArgs e)
        {
            LabelGroupType.Text = "Export";
            populateDropDownListGroupId();

            Session["process"] = "export";
            uploadControl.Visible = false;
            SelectGroup.Visible = true;
            DropDownListEdit.Visible = true;
            RadioButtonListTestOrFinal.Visible = true;
        }

        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            LabelGroupType.Text = "Edit";
            populateDropDownListGroupId();

            Session["process"] = "edit";
            uploadControl.Visible = false;
            SelectGroup.Visible = true;
            DropDownListEdit.Visible = true;
            RadioButtonListTestOrFinal.Visible = false;
        }

        protected void populateDropDownListGroupId()
        {
            String sql = " SELECT DISTINCT GroupID " 
                       + " FROM ImportLockbox " 
                       + " ORDER BY GroupID ";

            SqlConnection con = null;
            List<ListItem> GroupIDs = new List<ListItem>();
            ListItem li = new ListItem();

            String groupId = "";

            try
            {
                con = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["lockboxConnectionString"].ConnectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    groupId = reader["GroupID"].ToString();
                    li = new ListItem(groupId, groupId);
                    GroupIDs.Add(li);
                }

                reader.Close();

                DropDownListEdit.Items.Clear();

                DropDownListEdit.Items.Add(new ListItem("Select GroupID","-1"));

                for (int i = 0; i < GroupIDs.Count; i++)
                {
                    DropDownListEdit.Items.Add(GroupIDs[i]);
                }

            }
            catch (SqlException sqle)
            {
                Logger.QuickLog(VariablesLockbox.ERROR_LOG_FILE_NAME, sqle.Message, "populateDropDownListGroupId()", sql);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        protected void ImageButtonEditNext_Click(object sender, ImageClickEventArgs e)
        {
            String nextPage = "";
            if (LabelGroupType.Text.Equals("Export"))
            {
                nextPage = this.nextPageExport;
                Session["exportType"] = RadioButtonListTestOrFinal.SelectedValue.ToString();
            }
            else
            {
                nextPage = this.nextPageImport;
                Session["process"] = "edit";
            }


            if (DropDownListEdit.SelectedValue.Equals("-1"))
            {
                LabelSelectGroupMessage.Text = "Please make a selection before clicking next.";
            }
            else
            {
                LabelSelectGroupMessage.Text = "Processing " + DropDownListEdit.SelectedValue;
                Session["groupID"] = DropDownListEdit.SelectedValue;
                Response.Redirect(nextPage);
            }
        }

    }
}
