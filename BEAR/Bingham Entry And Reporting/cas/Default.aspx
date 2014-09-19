<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BEAR.cas.Default" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>CAS - Cost Allocation System</title>

    <link href="../style/global.css" rel="stylesheet" type="text/css" />
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <link href="../style/cas.search.css" rel="stylesheet" type="text/css" />
    <link href="../style/progress.css" rel="stylesheet" type="text/css" />
    <link href="../style/lockedHeader.css" rel="stylesheet" type="text/css" />
    <link href="../style/border.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/general.js" type="text/javascript"></script>
    <script src="../scripts/grid.js" type="text/javascript"></script>
    <script src="../scripts/div.js" type="text/javascript"></script>
    
    <script language="javascript" type="text/javascript">
        var oldgridSelectedColor;

        function setMouseOverColor(element, color)
        {
            oldgridSelectedColor = element.style.backgroundColor;
            element.style.cursor='hand';
            element.style.textDecoration='underline';
           
            if(color) 
            {
                element.style.backgroundColor='#CED0D3';
            }
            
        }

        function setMouseOutColor(element)
        {
            element.style.backgroundColor=oldgridSelectedColor;
            element.style.textDecoration='none';
        }
        
        function changeScreenSize(w,h)
        {
            window.resizeTo( w,h )
        }
        
        var tableID = "GridViewSearchResults";
        
        function checkColumn(checked) 
        {
            var table = document.getElementById(tableID);
            var tableRowsCollection = table.getElementsByTagName('tr');  
            var elementId = "";
        
            var tableRowCountAdjustment = 1;
        
            for (i = 0; i < tableRowsCollection.length-tableRowCountAdjustment; i++)
            {
                if((i+2) < 10) 
                {
                    elementId = tableID + "_ctl0" + (i+2) + "_bulkUpdateCB";
                }
                else
                {
                    elementId = tableID + "_ctl" + (i+2) + "_bulkUpdateCB";
                }
                if(document.getElementById(elementId) != null) 
                {        
                    document.getElementById(elementId).checked = checked;
                }
        
            }
        
        }
        
        
        function overrideOfficeLocation() {
            var checkBox = document.getElementById('CheckBoxOverrideOffice');
            if(checkBox.checked) {
                document.getElementById('DropDownListDetailsOffice').disabled = false;
            } else {
                document.getElementById('DropDownListDetailsOffice').disabled = true;
            }
        }
        
        
        function updateCheckedRows() 
        {
            var bulkMatterValue = document.getElementById("<%= TextBoxBulkMatter.ClientID %>").value;
            var bulkCostCodeValue = document.getElementById("<%= TextBoxBulkCostCode.ClientID %>").value;
            var bulkTkidValue = document.getElementById("<%= TextBoxBulkTkid.ClientID %>").value;
            var table = document.getElementById(tableID);
            var tableRowsCollection = table.getElementsByTagName('tr');  
            var elementIdMatter = "";
            var elementIdCostCode = "";
            var elementIdTimekeeper = "";
            var elementId = "";
            var checkedRow;
        
            var tableRowCountAdjustment = 1;
        
            for (i = 0; i < tableRowsCollection.length-tableRowCountAdjustment; i++)
            {
                if((i+2) < 10) 
                {
                    elementId = tableID + "_ctl0" + (i+2) + "_bulkUpdateCB";
                    elementIdMatter = tableID + "_ctl0" + (i+2) + "_matterTB";
                    elementIdCostCode = tableID + "_ctl0" + (i+2) + "_costcodeTB";
                    elementIdTimekeeper = tableID + "_ctl0" + (i+2) + "_timekeepTB";
                }
                else
                {
                    elementId = tableID + "_ctl" + (i+2) + "_bulkUpdateCB";
                    elementIdMatter = tableID + "_ctl" + (i+2) + "_matterTB";
                    elementIdCostCode = tableID + "_ctl" + (i+2) + "_costcodeTB";
                    elementIdTimekeeper = tableID + "_ctl" + (i+2) + "_timekeepTB";
                }
                if(document.getElementById(elementId) != null) 
                {        
                    if(document.getElementById(elementId).checked)
                    {
                        if(bulkMatterValue != "")   
                        {
                            document.getElementById(elementIdMatter).value = bulkMatterValue;
                            document.getElementById(elementIdMatter).className = "ChangedRow";
                        }
                        if(bulkCostCodeValue != "") 
                        {
                            document.getElementById(elementIdCostCode).value = bulkCostCodeValue;
                            document.getElementById(elementIdCostCode).className = "ChangedRow";
                        }
                        if(bulkTkidValue != "")     
                        {
                            document.getElementById(elementIdTimekeeper).value = bulkTkidValue;
                            document.getElementById(elementIdTimekeeper).className = "ChangedRow";
                        }
                    }
                }
        
            }
            
        }
        
        
        function clearBulkTextBoxValues()
        {
            document.getElementById("<%= TextBoxBulkMatter.ClientID %>").value = "";
            document.getElementById("<%= TextBoxBulkCostCode.ClientID %>").value = "";
            document.getElementById("<%= TextBoxBulkTkid.ClientID %>").value = "";
        }
        
        
    </script>
    <style type="text/css">
    BODY
    {
	    text-align:Left;
    }
    </style>
    
    
</head>
<body onload="changeScreenSize(1024,732)">

    <script src="../scripts/wz_tooltip.js" type="text/javascript"></script>
        
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>
    
    <input id="Hidden0" runat="server" type="hidden" value="52px" />
    <input id="Hidden1" runat="server" type="hidden" value="60px" />
    <input id="Hidden2" runat="server" type="hidden" value="37px" />
    <input id="Hidden3" runat="server" type="hidden" value="40px" />
    <input id="Hidden4" runat="server" type="hidden" value="40px" />
    <input id="Hidden5" runat="server" type="hidden" value="150px" />
    <input id="Hidden6" runat="server" type="hidden" value="60px" />
    <input id="Hidden7" runat="server" type="hidden" value="252px" />
    <input id="Hidden8" runat="server" type="hidden" value="252px" />
    <input id="Hidden9" runat="server" type="hidden" value="60px" />

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
            var x = Math.round(document.body.offsetWidth / 4) - (110); // changed to 4 to move to center over the first half of the screen //Math.round(pnlPopupBounds.width / 2); //hardcoded 110 to match the width of the activity bar.  pnlpopupbounds was not working.
            var y = Math.round(gridViewBounds.height / 2) - Math.round(pnlPopupBounds.height / 2);	    

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
    
    <div id="options" class="hidden" runat="server" />
    
    <div id="title" class="title">
        <asp:UpdatePanel ID="UpdatePanelSaveButton" runat="server">
            <ContentTemplate>
                <img 
                    alt="BINGHAM" 
                    src="../images/binghamLogoSmall.gif" 
                    />
                <img 
                    alt="CAS - Cost Allocation System" 
                    src="../images/CASTitle.gif" 
                    class="titlePic"
                    id ="titlePic"
                    />
                <asp:ImageButton 
                    ID="ImageButtonSave" 
                    runat="server" 
                    ToolTip="Save" 
                    Visible="false"
                    AlternateText="Save" 
                    ImageUrl="~/images/controls/disk.png" 
                    />
                    &nbsp;
               <asp:ImageButton 
                    ID="ImageButtonRecalc" 
                    runat="server" 
                    ToolTip="Recalculate Error Codes" 
                    Visible="false"
                    ImageUrl="~/images/controls/lightning.png" 
                    OnClick="ImageButtonRecalc_Click" 
                    />
                    &nbsp;
                <asp:ImageButton 
                    ID="ImageButtonBulk" 
                    runat="server" 
                    ToolTip="Bulk Update" 
                    Visible="false"
                    ImageUrl="~/images/controls/bulk.gif" 
                    OnClick="ImageButtonBulk_Click" 
                    />
                    &nbsp;
                <asp:ImageButton 
                    ID="ImageButtonShowAllRows" 
                    runat="server" 
                    ToolTip="Show All Rows" 
                    Visible="false"
                    AlternateText="Save" 
                    ImageUrl="~/images/controls/arrow_down.png" 
                    OnClick = "ImageButtonShowAllRows_Click" 
                    />
                <asp:ImageButton 
                    ID="ImageButtonPagerOn" 
                    runat="server" 
                    ToolTip="Turn Paging On" 
                    Visible="false"
                    AlternateText="Save" 
                    ImageUrl="~/images/controls/arrow_up.png" 
                    OnClick = "ImageButtonPagerOn_Click" 
                    />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownListProcessIDs" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    
    <asp:Panel ID="PanelHeaderDetails" runat="server">
        <div id="DivHeaderDetails" runat="server" class="headerDetails">
            <asp:UpdatePanel ID="UpdatePanelHeaderDetails" runat="server"  UpdateMode="Always">
                <ContentTemplate>
                    <asp:GridView 
                        ID="GridViewTotalAmount" 
                        runat="server"
                        AllowSorting="False" 
                        AllowPaging="False"
                        DataSourceID="SqlDataSourceCAS_ProcessIDTotal" 
                        Visible="False"
                        AutoGenerateColumns="false"
                        GridLines="Vertical"
                        >
                        <HeaderStyle BackColor="#2a76b2" Font-Bold="True" ForeColor="#FFFFFF" VerticalAlign="Bottom" Wrap="False" />
                        <Columns>
                            <asp:BoundField 
                                DataField="NumberOfRows" 
                                HeaderText="Row Count" 
                                HtmlEncode="false"
                                ItemStyle-HorizontalAlign="Center"
                                DataFormatString="{0:N0}"
                                ItemStyle-Width="100"
                                >
                                <ItemStyle Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField 
                                DataField="TotalAmount" 
                                HeaderText="Total Amount" 
                                HtmlEncode="false"
                                ItemStyle-HorizontalAlign="Center"
                                DataFormatString="{0:N2}"
                                ItemStyle-Width="100"
                                >
                                <ItemStyle Wrap="false" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    
    <asp:Panel ID="SearchSelectedPanel" runat="server" DefaultButton="ImageButtonUpdate" >
        <div id="searchSelectedItems" runat="server" class="searchSelected">
            <asp:UpdatePanel ID="UpdatePanelSelectedItem" runat="server" UpdateMode="Always" >
                <ContentTemplate>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td colspan="3" valign="middle">
                                <table width="300px">
                                <td>
                                    <b>
                                        <asp:Label ID="LabelSelectedMessage" runat="server" Text="" CssClass="searchTitle" ></asp:Label>
                                    </b>
                                </td>
                                <td align="right">
                                    <asp:ImageButton ID="ImageButtonUpdate" runat="server" ImageUrl="~/images/controls/smallwhitedot.gif" />
                                    <asp:ImageButton ID="ImageButtonDetails" runat="server" OnClick="ImageButtonDetails_Click" ToolTip="More Details" ImageUrl="~/images/controls/smallwhitedot.gif" />
                                    &nbsp;&nbsp;
                                    <asp:ImageButton ID="ImageButtonBulkUpdate" runat="server" OnClick="ImageButtonBulkUpdate_Click" ToolTip="" ImageUrl="~/images/controls/smallwhitedot.gif" />                            
                                    <span id="blankSpan" runat="server"></span>
                                    <asp:ImageButton ID="ImageButtonDelete" runat="server" OnClick="ImageButtonDelete_Click" ToolTip="Delete Record" ImageUrl="~/images/controls/smallwhitedot.gif" />                            
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButtonCloseSelected" runat="server" class="searchClose" OnClick="LinkButtonCloseSelectedClick" Text="" ToolTip="Close Section"></asp:LinkButton>
                                </td>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap">
                                <asp:ImageButton ID="ImageButtonSearchMatterPanel" runat="server" OnClick="ImageButtonSearchMatterPanel_Click" ToolTip="Search Matters" ImageUrl="~/images/controls/smallwhitedot.gif" />
                            </td>
                            <td>
                                <asp:Label ID="LabelMatterLabel" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LabelMatter" runat="server" Text=""></asp:Label><br />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap">
                                <asp:ImageButton ID="ImageButtonSearchTimekeeperPanel" runat="server" OnClick="ImageButtonSearchTimekeeperPanel_Click" ToolTip="Search Timekeepers" ImageUrl="~/images/controls/smallwhitedot.gif" />
                            </td>
                            <td>
                                <asp:Label ID="LabelTimekeeperLabel" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LabelTimekeeper" runat="server" Text=""></asp:Label><br />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap">
                                <asp:ImageButton ID="ImageButtonSelectCostCodePanel" runat="server"  OnClick="ImageButtonSelectCostCodePanel_Click" ToolTip="Select Cost Code" ImageUrl="~/images/controls/smallwhitedot.gif" />
                            </td>
                            <td>
                                <asp:Label ID="LabelCostCodeLabel" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LabelCostCode" runat="server" Text=""></asp:Label><br />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewSearchResults" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>

    <div id="search" runat="server" class="searchDivGridView">
        <table cellpadding="0" border="0">
            <tr>
                <td>
                    <span class="searchInstructions">AP Correction By </span><asp:Label ID="LabelSearchTitle" Text="Vendor" runat="server"></asp:Label>
                    <asp:LinkButton ID="LinkButtonChangeSearch" Text="Run By Timekeeper" runat="server" OnClick="LinkButtonChangeSearch_Click"></asp:LinkButton>
                </td>
                <td></td>
             </tr>
            <tr>
                <td valign="top">
                    <asp:UpdatePanel ID="UpdatePanelVendorsProcessIDs" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        <table>
                            <tr>
                                <td colspan="3">
                                    <asp:DropDownList ID="DropDownListVendors" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListVendors_SelectedIndexChanged" CssClass="dropDownList">
                                        <asp:ListItem Value="-1" Text="---Please Select A Vendor---" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="DropDownListProcessIDs" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="DropDownListProcessIDs_SelectedIndexChanged" CssClass="dropDownList">
                                        <asp:ListItem Value="-1" Text="---Please Select A Process ID---" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="RadioButtonListGLCost" runat="server" RepeatDirection="Horizontal" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonListGLCost_SelectedIndexChanged" >
                                        <asp:ListItem Text="Cost" Selected="True" Value="C"></asp:ListItem>
                                        <asp:ListItem Text="GL" Selected="False" Value="G"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td class="borderLineLeftBlack">
                                    <asp:RadioButtonList ID="RadioButtonListViewAll" runat="server" RepeatDirection="Horizontal" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonListViewAll_SelectedIndexChanged" >
                                        <asp:ListItem Text="All" Selected="False" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="All Errors" Selected="True" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="TK Errors" Selected="False" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                            <asp:Panel ID="PanelTimekeeperSubmit" runat="server" DefaultButton="ButtonSubmitTimekeeper">
                                <asp:Label ID="LabelTimekeeperEnter" runat="server" Text="Timekeeper: " Visible="false" /><asp:TextBox ID="TextBoxTimekeeperID" runat="server" Visible="false" Text="00000" />&nbsp;&nbsp;<asp:Button ID="ButtonSubmitTimekeeper" OnClick="ButtonSubmitTimekeeper_Click" runat="server" Text="Submit" Visible="false" class="Button1"/>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="DropDownListProcessIDs" EventName="SelectedIndexChanged"/>
                        </Triggers>
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
        <br /><br />
    </div>
    
    <div id="searchResults" class="searchResultsGridView">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView 
                    ID="GridViewSearchResults" 
                    runat="server" 
                    AllowSorting="True" 
                    AllowPaging="True"
                    DataSourceID="SqlDataSourceCAS" 
                    Visible="False" 
                    ForeColor="#333333" 
                    CellPadding="4"
                    GridLines="Vertical" 
                    AutoGenerateColumns="False"
                    OnRowDataBound="GridViewSearchResults_RowDataBound"
                    OnSelectedIndexChanged="GridViewSearchResults_SelectedIndexChanged"
                    PageSize="100"
                    EmptyDataText='No Matching Results Found.'
                    CssClass="GridViewSearchResults"
                    >
                    <FooterStyle CssClass="GridViewFooter" />
                    <RowStyle CssClass="GridViewRow" />
                    <PagerStyle CssClass="GridViewPager GridViewFixedFooter" ForeColor="#FFFFFF"/> 
                    <HeaderStyle BackColor="#2a76b2" Font-Bold="True" ForeColor="#FFFFFF" VerticalAlign="Bottom" Wrap="False" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <SelectedRowStyle BackColor="#aab0b7" ForeColor="#000000" Font-Bold="false" />
                    <PagerSettings  Mode="NextPreviousFirstLast" 
                                    FirstPageText="First"
                                    LastPageText="Last"
                                    NextPageText="Next"
                                    PreviousPageText="Previous"
                                    Position="Bottom"
                                    />
                    <Columns>
                        <asp:BoundField 
                            DataField="TempId" 
                            HeaderText="HiddenID" 
                            SortExpression="TempId"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="TempId" 
                            HeaderText="ID" 
                            SortExpression="TempId"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField
                            HeaderText = "Matter"
                            SortExpression="matter"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="matterTB"
                                    runat="server"
                                    Text='<%# Bind("[matter]") %>'
                                    BorderStyle="None"
                                    Font-Size="8pt"
                                    Width="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');VerifyNumeric(10, 'Matter');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                        </asp:TemplateField>
                       <asp:BoundField 
                            DataField="mstatus" 
                            HeaderText="Matter<br/>Status" 
                            SortExpression="mstatus"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField
                            HeaderText = "Cost<br>Code"
                            SortExpression="CostCode"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="costcodeTB"
                                    runat="server"
                                    Text='<%# Bind("[CostCode]") %>'
                                    BorderStyle="None"
                                    Font-Size="8pt"
                                    Width="35"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    CssClass="center"
                                    />
                            </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "TKID"
                            SortExpression="timekeep"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="timekeepTB"
                                    runat="server"
                                    Text='<%# Bind("[timekeep]") %>'
                                    BorderStyle="None"
                                    Font-Size="8pt"
                                    Width="35"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');VerifyNumeric(5, 'TKID');"
                                    OnTextChanged="TextBox_TextChanged"
                                    CssClass="center"
                                    />
                            </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "GL Account"
                            SortExpression="GLString"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="glstringTB"
                                    runat="server"
                                    Text='<%# Bind("[GLString]") %>'
                                    BorderStyle="None"
                                    Font-Size="8pt"
                                    Width="120"
                                    onChange="ChangeCell('ChangedRow right'); VerifyNumeric(-1, 'Amount');"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    OnTextChanged="TextBox_TextChanged"
                                    CssClass="right"
                                    />
                            </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Amount"
                            SortExpression="Amount"
                            ItemStyle-HorizontalAlign="Right"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="amountTB"
                                    runat="server"
                                    Text='<%# Bind("[Amount]") %>'
                                    BorderStyle="None"
                                    Font-Size="8pt"
                                    Width="60"
                                    onChange="ChangeCell('ChangedRow right'); VerifyNumeric(-1, 'Amount');"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    OnTextChanged="TextBox_TextChanged"
                                    CssClass="right"
                                    />
                            </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Description"
                            SortExpression="Description"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="descriptionTB"
                                    runat="server"
                                    Text='<%# Bind("[Description]") %>'
                                    BorderStyle="None"
                                    TextMode="MultiLine"
                                    Font-Size="8pt"
                                    Font-Names="Arial"
                                    Width="250"
                                    Height="57"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                            </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Description"
                            SortExpression="Description"
                            ItemStyle-HorizontalAlign="Center"
                            Visible="false"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="descriptionTB2"
                                    runat="server"
                                    Text='<%# Bind("[Description]") %>'
                                    BorderStyle="None"
                                    TextMode="MultiLine"
                                    Font-Size="8pt"
                                    Font-Names="Arial"
                                    Width="250"
                                    Height="16"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                            </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="Bulk Update<br /></a><img id='ck1' src='../images/controls/checkAll.gif' class='headerImage' onclick='checkColumn(true)'/>&nbsp;<img id='uck1' src='../images/controls/checkNone.gif' class='headerImage' onclick='checkColumn(false)'/>"
                            SortExpression="draftSent"
                            ItemStyle-HorizontalAlign="Center"
                            Visible="false"
                            >
                            <ItemTemplate>
                                <asp:CheckBox 
                                    ID="bulkUpdateCB"
                                    runat="server" 
                                    Checked='false'
                                    />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField 
                            DataField="status" 
                            HeaderText="HiddenStatus" 
                            SortExpression="status"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Left"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownListProcessIDs" EventName="SelectedIndexChanged"/>
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
    
    <asp:UpdatePanel ID="UpdatePanelSearchMatter" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelSearchMatter" runat="server" DefaultButton="ImageButtonSearchMatter" Visible="false">
                <div id="SearchMatter" runat="server" class="searchDivCAS">
                    <p>
                        <span class="searchTitle">Matter Search</span>
                        <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButtonCloseSearchMatterPanel" runat="server" class="searchClose" ToolTip="Close Section" OnClick="LinkButtonPanelsClick">X</asp:LinkButton></span>
                        <br />
                        <span class="searchInstructions">Searchs Matter Descriptions and Matter Number<br />(Returns top 1,000 Results)</span>
                    </p>
                    <asp:Label ID="labelSearchMatter" runat="server" Text="Enter Search Criteria" CssClass="parameterLabel" /><br />
                    <asp:TextBox ID="textBoxSearchMatter" runat="server" />
                    <asp:ImageButton ID="ImageButtonSearchMatter" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="ImageButtonSearchMatterClick"/>
                    <br />
                    <div id="searchResultsMatter" class="lookupResults">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="labelSearchResultsMatter" runat="server" Text="" CssClass="parameterLabel" /><br />
                                <asp:RadioButtonList 
                                    ID="RadioButtonListSearchResultsMatter" 
                                    runat="server" 
                                    Visible="False" 
                                    DataSourceID="SqlDataSource_Matter" 
                                    DataTextField="ConcatResult" 
                                    DataValueField="mmatter"
                                    >
                                </asp:RadioButtonList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ImageButtonSearchMatter" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ImageButtonSearchMatterPanel" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanelSearchTimekeeper" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelSearchTimekeeper" runat="server" DefaultButton="ImageButtonSearchTimekeeper" Visible="false">
                <div id="DivSearchTimekeeper" runat="server" class="searchDivCAS">
                    <p>
                        <span class="searchTitle">Timekeeper Search</span>
                        <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButtonCloseSearchTimekeeperPanel" runat="server" class="searchClose" ToolTip="Close Section" OnClick="LinkButtonPanelsClick">X</asp:LinkButton></span>
                        <br />
                        <span class="searchInstructions">Searchs Timekeeper Name, TKID, Title, and Location Description</span>
                    </p>
                    <asp:Label ID="labelSearchTimekeeper" runat="server" Text="Enter Search Criteria" CssClass="parameterLabel" /><br />
                    <asp:TextBox ID="textBoxSearchTimekeeper" runat="server" />
                    <asp:ImageButton ID="ImageButtonSearchTimekeeper" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="ImageButtonSearchTimekeeperClick"/>
                    <br />
                    <div id="DivSearchResultsTimekeeper" runat="server" class="lookupResults">
                        <asp:UpdatePanel ID="UpdatePanelSearchResultsTimekeeper" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="labelSearchResultsTimekeeper" runat="server" Text="" CssClass="parameterLabel" /><br />
                                <asp:RadioButtonList 
                                    ID="RadioButtonListSearchResultsTimekeeper" 
                                    runat="server" 
                                    Visible="False" 
                                    DataSourceID="SqlDataSource_Timekeeper" 
                                    DataTextField="ConcatResult" 
                                    DataValueField="tkinit"
                                    >
                                </asp:RadioButtonList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ImageButtonSearchTimekeeper" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ImageButtonSearchTimekeeperPanel" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanelDeleteRecord" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelDeleteRecord" runat="server" DefaultButton="ButtonDeleteCancel" Visible="false">
                <div id="DivDeleteRecord" runat="server" class="searchDivCAS">
                    <p>
                        <span class="searchTitle">Delete Selected Row</span>
                        <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButtonCloseDeleteRecordPanel" runat="server" class="searchClose" ToolTip="Close Section" OnClick="LinkButtonPanelsClick">X</asp:LinkButton></span>
                        <br />
                        <span class="searchInstructions"></span>
                    </p>
                    <asp:Label ID="labelDeleteRecord" runat="server" Text="Delete this Record?" CssClass="parameterLabel" /><br />
                    &nbsp;&nbsp;
                    <asp:Button ID="ButtonDeleteConfirm" runat="server" Text="Yes" OnClick="ButtonDeleteConfirm_Click" class="Button1" />
                    &nbsp;&nbsp;
                    <asp:Button ID="ButtonDeleteCancel" runat="server" Text="No" OnClick="ButtonDeleteCancel_Click" class="Button1" />
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ImageButtonDelete" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanelDetails" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelDetails" runat="server" DefaultButton="ButtonSaveDetails" Visible="false">
                <div id="DivDetails" runat="server" class="searchDivCAS">
                        <span class="searchTitle">Record Details</span>
                        <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButtonCloseDetailsPanel" runat="server" class="searchClose" ToolTip="Close Section" OnClick="LinkButtonPanelsClick">X</asp:LinkButton></span>
                        <br />
                        <span class="searchInstructions">Edit Client #, Quantity and/or Office</span>
                    <asp:Label ID="labelDetails" runat="server" Text="" CssClass="parameterLabel" /><br />
                    <table>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="LabelDetailsRecordNumber" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelDetailsClient" runat="server" Text="Client:"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="TextBoxDetailsClient" runat="server" onChange="ChangeCell('ChangedRow');VerifyNumeric(7, 'Client');"></asp:TextBox><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelDetailsQuantity" runat="server" Text="Quantity:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBoxDetailsQuantity" runat="server"></asp:TextBox><br />
                            </td>
                            <td>
                                Location Override
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LabelDetailsOffice" runat="server" Text="TK Office:"></asp:Label><br />
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListDetailsOffice" runat="server" OnSelectedIndexChanged="DropDownListDetailsOffice_SelectedIndexChanged" Enabled="false"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBoxOverrideOffice" OnClick="CheckBoxOverrideOffice_Click" OnCheckedChanged = "CheckBoxOverrideOffice_CheckChanged" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="ButtonSaveDetails" runat="server" Text="Save" OnClick="ButtonSaveDetails_Click" class="Button1" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonSaveDetails" />
        </Triggers>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpdatePanelCostCodesSelect" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelCostCodesSelect" runat="server" Visible="false">
                <div id="DivCostCodesSelect" runat="server" class="searchDivCAS">
                    <p>
                        <span class="searchTitle">Select Cost Code</span>
                        <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButtonCostCodesSelect" runat="server" class="searchClose" ToolTip="Close Section" OnClick="LinkButtonPanelsClick">X</asp:LinkButton></span>
                        <br />
                        <span class="searchInstructions">Select a Cost Code.  The list is generated from elite.</span>
                    </p>
                    <div id="DivCostCodesSelectResults" class="lookupResults">
                        <asp:RadioButtonList 
                            ID="RadioButtonListSearchResultsCostCode" 
                            runat="server" 
                            Visible="False" 
                            DataSourceID="SqlDataSource_CostCodes" 
                            DataTextField="ConcatResult" 
                            DataValueField="cocode"
                            >
                        </asp:RadioButtonList>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ImageButtonSelectCostCodePanel" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanelBulkUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelBulkUpdate" runat="server" Visible="false">
                <div id="DivBulkUpdate" runat="server" class="searchDivCAS">
                    <p>
                        <span class="searchTitle">Enter Values for Bulk Update</span>
                        <br />
                        <span class="searchInstructions">Enter in the new Value for Matter, Cost Code or Timekeeper ID.<br />Leave Blank to not Update.</span>
                    </p>
                    <div id="DivBulkUpdateTextBoxes" class="lookupResults">
                        <table>
                        <tr><td><asp:Label ID="LabelBulkMatter" runat="server" Text="New Matter Number"></asp:Label></td><td><asp:TextBox ID="TextBoxBulkMatter" runat="server" Text = ""></asp:TextBox></td></tr>
                        <tr><td><asp:Label ID="LabelBulkCostCode" runat="server" Text="New Cost Code"></asp:Label></td><td><asp:TextBox ID="TextBoxBulkCostCode" runat="server" Text = ""></asp:TextBox></td></tr>
                        <tr><td><asp:Label ID="LabelBulkTkid" runat="server" Text="New Tkid"></asp:Label></td><td><asp:TextBox ID="TextBoxBulkTkid" runat="server" Text = ""></asp:TextBox><asp:ImageButton ID="ImageButtonBulkSearchTimekeeper" runat="server" OnClick="ImageButtonBulkSearchTimekeeper_Click" ToolTip="Search Timekeepers" ImageUrl="~/images/controls/search.gif" /></td></tr>
                        <tr><td><asp:Label ID="LabelGLString" runat="server" Text="New GL String"></asp:Label></td><td><asp:TextBox ID="TextBoxBulkGLString" runat="server" Text = ""></asp:TextBox></td></tr>
                        <tr><td colspan="2">
                            <table>
                                <tr>
                                    <td width="100"><input id="Input_UpdateGrid" type="button" value="Update Grid" onclick="updateCheckedRows()" class="Button1" /></td>
                                    <td width="100"><input id="Input_ClearValues" type="button" value="Clear Values" onclick="clearBulkTextBoxValues()" class="Button1" /></td>
                                    <td width="100"><asp:Button ID="Button_BulkSave" runat="server" Text="Bulk Save" class="Button1"/></td>
                                </tr>
                            </table>
                        </td></tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ImageButtonSelectCostCodePanel" />
        </Triggers>
    </asp:UpdatePanel>
    

    <asp:SqlDataSource 
        ID="SqlDataSourceCAS" 
        runat="server" 
        ConnectionString="<%$ ConnectionStrings:CASConnectionString %>" 
        SelectCommand="cas_BEAR_get_APCostRecItem" 
        SelectCommandType="StoredProcedure"
        OnSelecting="SqlDataSource_OnSelecting">
    </asp:SqlDataSource>
    
    <asp:SqlDataSource 
        ID="SqlDataSourceCAS_ProcessIDTotal" 
        runat="server" 
        ConnectionString="<%$ ConnectionStrings:CASConnectionString %>" 
        SelectCommand="cas_BEAR_get_APProcessIdTotalAmount" 
        SelectCommandType="StoredProcedure"
        OnSelecting="SqlDataSourceCAS_ProcessIDTotal_OnSelecting">
    </asp:SqlDataSource>
    
    <asp:SqlDataSource 
        ID="SqlDataSource_Matter" 
        runat="server" 
        ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
        SelectCommand="uspBMcBEARSearchMatter" 
        SelectCommandType="StoredProcedure"
        OnSelecting="SqlDataSource_Matter_EscapeSingleQuote">
     </asp:SqlDataSource>

    <asp:SqlDataSource 
        ID="SqlDataSource_Timekeeper" 
        runat="server" 
        ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
        SelectCommand="uspBMcBEARSearchTKID" 
        SelectCommandType="StoredProcedure"
        OnSelecting="SqlDataSource_Timekeeper_EscapeSingleQuote">
     </asp:SqlDataSource>
     
    <asp:SqlDataSource
        ID="SqlDataSource_CostCodes"
        runat="server"
        ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
        SelectCommand="uspBMcBEARSelectCostCodes" 
        SelectCommandType="StoredProcedure"
        >
    </asp:SqlDataSource>

    </form>
    
</body>

</html>

