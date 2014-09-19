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
    public partial class cashreceipts : System.Web.UI.Page
    {
        protected BearCode bearCode = new BearCode();
        protected int columnClientNumber = 1;
        protected int columnClientName = 2;
        protected int columnMatterNumber = 3;
        protected int columnMatterDescription = 4;
        protected int columnMatterStatus = 5;
        protected int columnInvoiceNumber = 6;
        protected int columnLedgerDescription = 7;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                GridViewSearchResults.SelectedIndex = -1;
            }
        }

        protected void imageButtonSearchClick(object sender, EventArgs e)
        {
            GridViewSearchResults.DataBind();
            GridViewSearchResults.Visible = true;

        }

        protected void GridViewSearchResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink
                                                (this.GridViewSearchResults, "Select$" + e.Row.RowIndex);

                if (e.Row.Cells[columnClientNumber].Text.Equals("0")) e.Row.Cells[columnClientNumber].Text = "-";
                if (e.Row.Cells[columnMatterNumber].Text.Equals("0")) e.Row.Cells[columnMatterNumber].Text = "-";
                if (e.Row.Cells[columnInvoiceNumber].Text.Equals("0")) e.Row.Cells[columnInvoiceNumber].Text = "-";

            }

            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                bearCode.GridViewCustomizePagerRow(GridViewSearchResults, e.Row);
            }
        }


        protected void GridViewSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            bearCode.GridViewCustomizePagerRow(GridViewSearchResults, GridViewSearchResults.BottomPagerRow);

            LabelSelectedMessage.Text = "Most Recently Selected Row:";
            LabelSelectedClientNumber.Text = "Client #:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnClientNumber].Text;
            LabelSelectedClientName.Text = "Client Name:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnClientName].Text;
            LabelSelectedMatterNumber.Text = "Matter #: (" + GridViewSearchResults.SelectedRow.Cells[columnMatterStatus].Text +")</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnMatterNumber].Text;
            LabelSelectedMatterDescription.Text = "Matter:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnMatterDescription].Text;
            LabelInvoiceNumber.Text = "Invoice #:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnInvoiceNumber].Text;
            LabelLedgerDescription.Text = "Description:</td><td>" + GridViewSearchResults.SelectedRow.Cells[columnLedgerDescription].Text;

            String[] matterTimekeeperInfo = new String[4];
            matterTimekeeperInfo = bearCode.GetMatterTimekeeperInfo(GridViewSearchResults.SelectedRow.Cells[columnMatterNumber].Text, VariablesSearchCashReceipts.ERROR_LOG_FILE_NAME);

            LabelMatterLocation.Text = "Matter Location:</td><td>" + matterTimekeeperInfo[0].ToString() + " - " + matterTimekeeperInfo[1].ToString();
            LabelBillingAttorney.Text = "Billing Attorney:</td><td>" + matterTimekeeperInfo[2].ToString() + " - " + matterTimekeeperInfo[3].ToString();

        }


        /*
         * If searching for a word with a single quote ('), it needs to be sent to SQLServer as two single quotes ('')
         */
        protected void SqlDataSource_OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 0;
            String searchInvoice = "N";
            if (CheckBoxSearchInvoice.Checked)
            {
                searchInvoice = "Y";
            }

            e.Command.Parameters.Add(new SqlParameter("@searchTxt", textBoxSearch.Text.Replace("'", "''")));
            e.Command.Parameters.Add(new SqlParameter("@searchInvoice", searchInvoice));
            e.Command.Parameters.Add(new SqlParameter("@matterStatus", RadioButtonListMatterStatus.SelectedValue));
        }

        
    }
}
