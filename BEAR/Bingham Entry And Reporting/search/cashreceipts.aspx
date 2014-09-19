<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cashreceipts.aspx.cs" Inherits="BEAR.search.cashreceipts" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="../style/global.css" rel="stylesheet" type="text/css" />
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <link href="../style/search.css" rel="stylesheet" type="text/css" />
    <link href="../style/progress.css" rel="stylesheet" type="text/css" />
    <title>Cash Receipts Client / Matter Search</title>
    
    <script language="javascript" type="text/javascript">
        var oldgridSelectedColor;

        function setMouseOverColor(element)
        {
            oldgridSelectedColor = element.style.backgroundColor;
            element.style.backgroundColor='#EB8123';
            element.style.cursor='hand';
            element.style.textDecoration='underline';
        }

        function setMouseOutColor(element)
        {
            element.style.backgroundColor=oldgridSelectedColor;
            element.style.textDecoration='none';
        }
    </script>

    
    
</head>
<body>
    <form id="form1" runat="server" defaultbutton="ImageButtonSearch">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>

    <script type="text/javascript">

            function onUpdating(){
                // get the update progress div
                var pnlPopup = $get('<%= this.pnlPopup.ClientID %>'); 

                //  get the gridview element        
                var gridView = $get('<%= this.search.ClientID %>');
                
                // get the bounds of both the gridview and the progress div
                var gridViewBounds = Sys.UI.DomElement.getBounds(gridView);
                var pnlPopupBounds = Sys.UI.DomElement.getBounds(pnlPopup);
                
                //  center of gridview
                var x = Math.round(document.body.offsetWidth / 2) - (110); //Math.round(pnlPopupBounds.width / 2); //hardcoded 110 to match the width of the activity bar.  pnlpopupbounds was not working.
                var y = Math.round(gridViewBounds.height / 2) - Math.round(pnlPopupBounds.height / 2);	    

                //document.title = 'x = ' + x + '; offsetWidth = ' + document.body.offsetWidth + '; pnlPopupBounds.width = ' + pnlPopupBounds.width;
                //	set the progress element to this position
                Sys.UI.DomElement.setLocation(pnlPopup, x, y);           
                
                // make it visible
                pnlPopup.style.display = '';
                
            }

            function onUpdated() {
                // get the update progress div
                var pnlPopup = $get('<%= this.pnlPopup.ClientID %>'); 
                // make it invisible
                pnlPopup.style.display = 'none';
            }            

            
        </script>

    
    <div id="title" class="title">
        <img 
            alt="BINGHAM" 
            src="../images/binghamLogoSmall.gif" 
            />
        <img 
            alt="Exception Rates" 
            src="../images/ClientMatterSearchTitle.gif" 
            class="titlePic"
            id ="titlePic"
            />
    </div>
    
    
    
    <div id="searchSelectedItems" runat="server" class="searchSelected">
        <asp:UpdatePanel ID="UpdatePanelSelectedItem" runat="server">
            <ContentTemplate>
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr><td colspan="2">
                    <b><asp:Label ID="LabelSelectedMessage" runat="server" Text=""></asp:Label></b><br />
                 </td></tr><tr><td nowrap="nowrap" width="25%">
                    <asp:Label ID="LabelSelectedClientNumber" runat="server" Text=""></asp:Label><br />
                </td></tr><tr><td nowrap="nowrap">
                    <asp:Label ID="LabelSelectedClientName" runat="server" Text=""></asp:Label><br />
                </td></tr><tr><td nowrap="nowrap" valign="top">
                    <asp:Label ID="LabelSelectedMatterNumber" runat="server" Text=""></asp:Label><br />
                </td></tr><tr><td nowrap="nowrap">
                    <asp:Label ID="LabelSelectedMatterDescription" runat="server" Text=""></asp:Label><br />
                </td></tr><tr><td nowrap="nowrap" valign="top">
                    <asp:Label ID="LabelInvoiceNumber" runat="server" Text=""></asp:Label><br />
                </td></tr><tr><td nowrap="nowrap" valign="top">
                    <asp:Label ID="LabelLedgerDescription" runat="server" Text=""></asp:Label><br />
                </td></tr><tr><td nowrap="nowrap" valign="top">
                    <asp:Label ID="LabelBillingAttorney" runat="server" Text=""></asp:Label><br />
                </td></tr><tr><td nowrap="nowrap" valign="top">
                    <asp:Label ID="LabelMatterLocation" runat="server" Text=""></asp:Label><br />
                </td></tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="GridViewSearchResults" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    
    
    

    <div id="search" runat="server" class="searchDivGridView">
        <br /><br /><br />
        <span class="searchTitle">Cash Receipts Client / Matter Search</span><br />
        <span class="searchInstructions">Search Client Number / Name / Address, and Matter Number / Description</span>
        <br />
        <br />
        <asp:RadioButtonList ID="RadioButtonListMatterStatus" RepeatDirection = "Horizontal" runat="server">
            <asp:ListItem Text="Search Open and Closed Matters" Value="B" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Search Open Matters" Value="O"></asp:ListItem>
            <asp:ListItem Text="Search Closed Matters" Value="C"></asp:ListItem>
        </asp:RadioButtonList>
        <asp:CheckBox ID="CheckBoxSearchInvoice" runat="server" Text="Add Invoice Number & Description to Search" Checked="false"/>
        <br />
        <br />
        <asp:Label ID="labelSearch" runat="server" Text="Enter Search Criteria" CssClass="parameterLabel" /><br />
        <asp:TextBox ID="textBoxSearch" runat="server" Width="271px"/>
        <asp:ImageButton ID="ImageButtonSearch" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="imageButtonSearchClick"/>
        <br />
        <asp:Label ID="labelSearchResults" runat="server" Text="" CssClass="leftBold" Visible="false"/>
        <br />
    </div>
    
    <div id="searchResults" class="searchResultsGridView">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>     
                <asp:GridView 
                    ID="GridViewSearchResults" 
                    runat="server" 
                    AllowSorting="True" 
                    AllowPaging="True"
                    DataSourceID="SqlDataSourceSearchCashReceipts" 
                    Visible="False" 
                    CellPadding="4" 
                    ForeColor="#333333" 
                    GridLines="None" 
                    AutoGenerateColumns="False"
                    OnRowDataBound="GridViewSearchResults_RowDataBound"
                    OnSelectedIndexChanged="GridViewSearchResults_SelectedIndexChanged"
                    PageSize="20"
                    EmptyDataText='No Matching Results Found.'
                    >
                    <FooterStyle CssClass="GridViewFooter" />
                    <RowStyle CssClass="GridViewRow" />
                    <PagerStyle CssClass="GridViewPager" ForeColor="#FFFFFF"/>
                    <HeaderStyle BackColor="#2a76b2" Font-Bold="True" ForeColor="#FFFFFF" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <SelectedRowStyle CssClass="GridViewSelectedRow"/>
                    <PagerSettings  Mode="NextPreviousFirstLast" 
                                    FirstPageText="First"
                                    LastPageText="Last"
                                    NextPageText="Next"
                                    PreviousPageText="Previous"
                                    Position="Bottom"
                                    />
                    
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" Visible="False" />
                        <asp:BoundField 
                            DataField="ClientNum" 
                            HeaderText="Client<br />Number" 
                            SortExpression="ClientNum"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="ClientName" 
                            HeaderText="Client<br />Name" 
                            SortExpression="ClientName"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Left"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="MatterNum" 
                            HeaderText="Matter<br />Number" 
                            SortExpression="MatterNum"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="MatterDesc" 
                            HeaderText="Matter<br />Description"
                            SortExpression="MatterDesc"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Left"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="MatterStatus" 
                            HeaderText="Matter<br />Status" 
                            SortExpression="MatterStatus"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="InvoiceNumber" 
                            HeaderText="Invoice<br />Number" 
                            SortExpression="InvoiceNumber"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="LedgerDesc" 
                            HeaderText="Ledger<br />Description" 
                            SortExpression="LedgerDesc"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>

                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ImageButtonSearch" />
            </Triggers>
        </asp:UpdatePanel>
        <cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server" TargetControlID="UpdatePanel1" >
            <Animations>
                <OnUpdating>
                    <Parallel duration="0">
                        <%-- place the update progress div over the gridview control --%>
                        <ScriptAction Script="onUpdating();" /> 
                    </Parallel>
                </OnUpdating>
                <OnUpdated>
                    <Parallel duration="0">
                        <%--find the update progress div and place it over the gridview control--%>
                        <ScriptAction Script="onUpdated();" /> 
                    </Parallel> 
                </OnUpdated>
            </Animations>
        </cc1:UpdatePanelAnimationExtender>
        <asp:Panel ID="pnlPopup" runat="server" CssClass="progress" style="display:none;">
            <div class="progressContainer">
                <div class="progressHeader">Loading...</div>
                <div class="progressBody">
                    <img src="../images/activity.gif" alt="" />
                </div>
            </div>
        </asp:Panel>     
    </div>


    <asp:SqlDataSource ID="SqlDataSourceSearchCashReceipts" runat="server" 
                ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
                SelectCommand="uspBMcBEARSearchCashReceipts" 
                SelectCommandType="StoredProcedure"
                OnSelecting="SqlDataSource_OnSelecting">
            </asp:SqlDataSource>
        
    </form>
</body>
</html>
