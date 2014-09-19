<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BEAR.rat.Default" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>Rate Agreement Tracker</title>

    <link href="../style/global.css" rel="stylesheet" type="text/css" />
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <link href="../style/rat.search.css" rel="stylesheet" type="text/css" />
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
        
       
    </script>
    <style type="text/css">
    BODY
    {
	    text-align:Left;
    }
    </style>
    
    
</head>

<body>
    <script src="../scripts/wz_tooltip.js" type="text/javascript"></script>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>

    <div id="options" class="hidden" runat="server" />
    
    <div id="title" class="titleRat">
        <asp:UpdatePanel ID="UpdatePanelSaveButton" runat="server">
            <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="1" >
                <tr>
                    <td valign="bottom">
                        <img 
                            alt="BINGHAM" 
                            src="../images/binghamLogoSmall.gif" 
                            />
                    </td>
                    <td valign="bottom">
                        <img 
                            alt="Rate Agreement Tracker" 
                            src="../images/RATTitle.gif" 
                            class="titlePic"
                            id ="titlePic"
                            />
                    </td>
                    <td valign="bottom">
                        <asp:RadioButtonList ID="RadioButtonListStatus" 
                                             runat="server" 
                                             RepeatDirection="Horizontal" 
                                             AutoPostBack="true" 
                                             OnSelectedIndexChanged="RadioButtonListStatus_SelectedIndexChanged"
                                             >
                            <asp:ListItem Text="Proposal" Value="P"></asp:ListItem>
                            <asp:ListItem Text="Final" Value="F"></asp:ListItem>
                            <asp:ListItem Text="All Columns" Value="B"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td valign="bottom">
                        &nbsp;
                        <asp:ImageButton 
                            ID="ImageButtonSave" 
                            runat="server" 
                            ToolTip="Save" 
                            Visible="true"
                            AlternateText="Save" 
                            ImageUrl="~/images/controls/disk.png" 
                            />
                    </td>
                </tr>
            </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ImageButtonSave" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    
    <asp:Panel ID="PanelHeaderDetails" runat="server">
        <div id="DivHeaderDetails" runat="server" class="headerDetails">
            <asp:UpdatePanel ID="UpdatePanelHeaderDetails" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Label ID="LabelMessage" runat="server" Text=""></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewSearchResults" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    
    
    <asp:Panel ID="SearchSelectedPanel" runat="server">
        <div id="searchSelectedItems" runat="server" class="searchSelected">
            <asp:UpdatePanel ID="UpdatePanelNotesSelector" runat="server" UpdateMode="Always">
                <ContentTemplate>
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="RadioButtonListNotesType" runat="server" Visible="false" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonListNotesType_SelectedIndexChanged">
                                    <asp:ListItem Text="Clients" Value="Clients" Selected="False" />
                                    <asp:ListItem Text="Notes" Value="Notes" Selected="False" />
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButtonCloseNotesHistory" runat="server" class="searchClose" ToolTip="Close History" visible="false" OnClick="LinkButtonCloseNotesHistoryClick">X</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewSearchResults" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanelSelectedItem" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td>
                                <b>
                                    <asp:Label ID="LabelSelectedMessage" runat="server" Text="" CssClass="searchTitle" ></asp:Label>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" width="300px">
                                <asp:Label ID="LabelNotesHistory" runat="server" Text=""></asp:Label>
                                <asp:RadioButtonList ID="RadioButtonListClientType" Visible="false" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                                    <asp:ListItem Selected="False" Text="Parent" Value="P" />
                                    <asp:ListItem Selected="False" Text="Related" Value="R" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 <asp:CheckBoxList ID="CheckBoxListClients" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="RadioButtonListNotesType" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    
    
    <div id="searchResults" class="searchResultsGridView" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView 
                    ID="GridViewSearchResults" 
                    runat="server" 
                    AllowSorting="True" 
                    AllowPaging="True"
                    DataSourceID="SqlDataSourceRAT" 
                    Visible="True" 
                    ForeColor="#333333" 
                    CellPadding="4"
                    GridLines="Both" 
                    AutoGenerateColumns="False"
                    OnRowDataBound="GridViewSearchResults_RowDataBound"
                    OnSelectedIndexChanged="GridViewSearchResults_SelectedIndexChanged"
                    PageSize="100"
                    EmptyDataText='No Matching Results Found.'
                    CssClass="GridViewSearchResults"
                    RowStyle-VerticalAlign="Top"
                    DataKeyNames="id"
                    ShowFooter="true"
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
                            DataField="id" 
                            HeaderText="HiddenID" 
                            SortExpression="id"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField 
                            HeaderText="<br /><br /><br /><br />ID" 
                            SortExpression="id"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Left"
                            >
                            <ItemTemplate>
                                <asp:Label 
                                    ID="idLabel"
                                    runat="server" 
                                    Text='<%# Bind("[id]") %>'
                                    CssClass="templateStyle"
                                    Width="50"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:LinkButton 
                                    ID="LinkButtonInsert" 
                                    runat="server"
                                    CausesValidation="false"
                                    OnClick="LinkButtonInsert_Click"
                                    Text="Insert"
                                    ToolTip="Insert New Record"
                                    >
                                </asp:LinkButton>
                                <br /><br />
                                <asp:LinkButton 
                                    ID="LinkButtonCancel" 
                                    runat="server"
                                    CausesValidation="false"
                                    OnClick="LinkButtonCancel_Click"
                                    Text="Cancel"
                                    ToolTip="Cancel Insert (blanks all fields)"
                                    >
                                </asp:LinkButton>
                                        
                             </FooterTemplate>
                            <ItemStyle Wrap="false" />
                        </asp:TemplateField>
                        <asp:TemplateField 
                            HeaderText="Client #" 
                            SortExpression="client"
                            ItemStyle-HorizontalAlign="Left"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:Label 
                                    ID="clientL"
                                    runat="server" 
                                    Text='<%# Bind("[client]") %>'
                                    CssClass="templateStyle"
                                    Width="60"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                 <asp:TextBox 
                                    ID="clientTBF" 
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    >
                                 </asp:TextBox>
                             </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField 
                            HeaderText="Client Name" 
                            SortExpression="clientName"
                            ItemStyle-HorizontalAlign="Left"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:Label 
                                    ID="clientNameL"
                                    runat="server" 
                                    Text='<%# Bind("[clientName]") %>'
                                    CssClass="templateStyle"
                                    Width="140"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox 
                                    ID="clientNameTBF" 
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="140"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    >
                                </asp:TextBox>
                            </FooterTemplate>
                            <ItemStyle Wrap="false" />
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Billing Point Person"
                            SortExpression="billingPointPerson"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="billingPointPersonTB"
                                    runat="server"
                                    Text='<%# Bind("[billingPointPerson]") %>'
                                    CssClass="templateStyle"
                                    Width="40"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox 
                                    ID="billingPointPersonTBF" 
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="40"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    >
                                </asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Scope"
                            SortExpression="scope"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="scopeTB"
                                    runat="server"
                                    Text='<%# Bind("[scope]") %>'
                                    CssClass="templateStyle"
                                    Width="80"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox 
                                    ID="scopeTBF" 
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="80"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    >
                                </asp:TextBox>
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Client<br />Engagement Partners"
                            SortExpression="clientEngagementPartners"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="cepTB"
                                    runat="server"
                                    Text='<%# Bind("[clientEngagementPartners]") %>'
                                    CssClass="templateStyle"
                                    TextMode="MultiLine"
                                    Width="120"
                                    Height="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox 
                                    ID="cepTBF" 
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="120"
                                    Height="60"
                                    TextMode="MultiLine"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    >
                                </asp:TextBox>
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Notes"
                            SortExpression="notes"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="notesTB"
                                    runat="server"
                                    Text='<%# Bind("[notes]") %>'
                                    CssClass="templateStyle"
                                    TextMode="MultiLine"
                                    Width="240"
                                    Height="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox 
                                    ID="notesTBF" 
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="240"
                                    Height="60"
                                    TextMode="MultiLine"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    >
                                </asp:TextBox>
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Effected Clients"
                            SortExpression="effectedClients"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:Label 
                                    ID="effectedClientsLabel" 
                                    runat="server" 
                                    Text='<%# Bind("[effectedClients]") %>'
                                    CssClass="templateStyle"
                                    Width="80"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox 
                                    ID="effectedClientsTBF" 
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="80"
                                    Height="60"
                                    TextMode="MultiLine"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    >
                                </asp:TextBox>
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Response Due Date"
                            SortExpression="responseDueDate"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="rddTB"
                                    runat="server"
                                    Text='<%# Bind("[responseDueDate]") %>'
                                    CssClass="templateStyle"
                                    Width="100"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox 
                                    ID="rddTBF" 
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="100"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    >
                                </asp:TextBox>
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Sent To"
                            SortExpression="sentTo"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="sentToTB"
                                    runat="server"
                                    Text='<%# Bind("[sentTo]") %>'
                                    CssClass="templateStyle"
                                    Width="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox 
                                    ID="sentToTBF" 
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    >
                                </asp:TextBox>
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Profitability Attached"
                            SortExpression="profitabilityAttached"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox
                                    ID="profitabilityCB"
                                    runat="server"
                                    Checked='<%# Bind("[profitabilityAttached]") %>'
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Width="85"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:CheckBox
                                    ID="profitabilityCBF"
                                    runat="server"
                                    Width="85"
                                    />
                             </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Approval Comments"
                            SortExpression="approvalComments"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="approvalCommentsTB"
                                    runat="server"
                                    Text='<%# Bind("[approvalComments]") %>'
                                    CssClass="templateStyle"
                                    TextMode="MultiLine"
                                    Width="180"
                                    Height="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:TextBox
                                    ID="approvalCommentsTBF"
                                    runat="server"
                                    CssClass="templateStyle"
                                    TextMode="MultiLine"
                                    Width="180"
                                    Height="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    />
                             </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Agreement Description"
                            SortExpression="agreementDescription"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="agreementDescriptionTB"
                                    runat="server"
                                    Text='<%# Bind("[agreementDescription]") %>'
                                    CssClass="templateStyle"
                                    TextMode="MultiLine"
                                    Width="240"
                                    Height="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="agreementDescriptionTBF"
                                    runat="server"
                                    CssClass="templateStyle"
                                    TextMode="MultiLine"
                                    Width="240"
                                    Height="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    />
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Elite Comments"
                            SortExpression="eliteComments"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="eliteCommentsTB"
                                    runat="server"
                                    Text='<%# Bind("[eliteComments]") %>'
                                    CssClass="templateStyle"
                                    TextMode="MultiLine"
                                    Width="240"
                                    Height="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="eliteCommentsTBF"
                                    runat="server"
                                    CssClass="templateStyle"
                                    TextMode="MultiLine"
                                    Width="240"
                                    Height="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    />
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Relationship<br />Partner<br />Confirmation"
                            SortExpression="confirmRelationshipPartner"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox
                                    ID="crpCB"
                                    runat="server"
                                    Checked='<%# Bind("[confirmRelationshipPartner]") %>'
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Width="85"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:CheckBox
                                    ID="crpCBF"
                                    runat="server"
                                    Width="85"
                                    />
                             </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Communicated<br />To Partners"
                            SortExpression="communicatedToPartners"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox
                                    ID="ctpCB"
                                    runat="server"
                                    Checked='<%# Bind("[communicatedToPartners]") %>'
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Width="85"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:CheckBox
                                    ID="ctpCBF"
                                    runat="server"
                                    Width="85"
                                    />
                              </FooterTemplate>
                       </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Communicated<br />To Billing"
                            SortExpression="communicatedToBilling"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox
                                    ID="ctbCB"
                                    runat="server"
                                    Checked='<%# Bind("[communicatedToBilling]") %>'
                                    OnCheckedChanged="CheckBox_CheckChanged"   
                                    Width="85"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:CheckBox
                                    ID="ctbCBF"
                                    runat="server"
                                    Width="85"
                                    />
                              </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Inspected By"
                            SortExpression="inspectedBy"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="inspectedByTB"
                                    runat="server"
                                    Text='<%# Bind("[inspectedBy]") %>'
                                    CssClass="templateStyle"
                                    Width="90"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="inspectedByTBF"
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="90"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    />
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Assign To (Client Acctg)"
                            SortExpression="assignedToClientAcctg"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="atcaTB"
                                    runat="server"
                                    Text='<%# Bind("[assignedToClientAcctg]") %>'
                                    CssClass="templateStyle"
                                    Width="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="atcaTBF"
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="60"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    />
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Set-Up<br />Complete"
                            SortExpression="setUpComplete"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox
                                    ID="setUpCompleteCB"
                                    runat="server"
                                    Checked='<%# Bind("[setUpComplete]") %>'
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Width="85"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:CheckBox
                                    ID="setUpCompleteCBF"
                                    runat="server"
                                    Width="85"
                                    />
                             </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Projected<br />Realization<br />for Agreement<br />Matters"
                            SortExpression="projectedRealizationAgreementMatters"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="pramTB"
                                    runat="server"
                                    Text='<%# Bind("[projectedRealizationAgreementMatters]") %>'
                                    CssClass="templateStyle"
                                    Width="80"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="pramTBF"
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="80"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    />
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Client UDF"
                            SortExpression="clientUDF"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="clientUDFTB"
                                    runat="server"
                                    Text='<%# Bind("[clientUDF]") %>'
                                    CssClass="templateStyle"
                                    Width="70"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="clientUDFTBF"
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="70"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    />
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Extend<br />Notice<br />Volume<br />Discount"
                            SortExpression="extendNoticeVolumeDiscount"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox
                                    ID="envdCB"
                                    runat="server"
                                    Checked='<%# Bind("[extendNoticeVolumeDiscount]") %>'
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Width="85"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:CheckBox
                                    ID="envdCBF"
                                    runat="server"
                                    Width="85"
                                    />
                             </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Billing<br />Manager<br />Review"
                            SortExpression="billingManagerReview"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox
                                    ID="bmrCB"
                                    runat="server"
                                    Checked='<%# Bind("[billingManagerReview]") %>'
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Width="85"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:CheckBox
                                    ID="bmrCBF"
                                    runat="server"
                                    Width="85"
                                    />
                             </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Cost<br />Write-Offs<br />Sent To<br />Client<br />Acctg"
                            SortExpression="costWriteOffsSentToClientAcctg"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox
                                    ID="cwostcaCB"
                                    runat="server"
                                    Checked='<%# Bind("[costWriteOffsSentToClientAcctg]") %>'
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Width="85"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:CheckBox
                                    ID="cwostcaCBF"
                                    runat="server"
                                    Width="85"
                                    />
                             </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Agreement<br />Start Date"
                            SortExpression="agreementStartDate"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="agreementStartDateTB"
                                    runat="server"
                                    Text='<%# Bind("[agreementStartDate]") %>'
                                    CssClass="templateStyle"
                                    Width="100"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="agreementStartDateTBF"
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="100"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    />
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Agreement<br />End Date"
                            SortExpression="agreementEndDate"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="agreementEndDateTB"
                                    runat="server"
                                    Text='<%# Bind("[agreementEndDate]") %>'
                                    CssClass="templateStyle"
                                    Width="100"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="agreementEndDateTBF"
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="100"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    />
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Actual<br />Realization"
                            SortExpression="actualRealization"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="actualRealizationTB"
                                    runat="server"
                                    Text='<%# Bind("[actualRealization]") %>'
                                    CssClass="templateStyle"
                                    Width="100"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    onChange="ChangeCell('ChangedRow');"
                                    OnTextChanged="TextBox_TextChanged"
                                    />
                             </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox
                                    ID="actualRealizationTBF"
                                    runat="server"
                                    CssClass="templateStyle"
                                    Width="100"
                                    onFocus="ChangeTextBoxBackgroundColor()"
                                    onBlur="RestoreTextBoxBackgroundColor()"
                                    />
                            </FooterTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText = "Finalized"
                            SortExpression="finalized"
                            ItemStyle-HorizontalAlign="Center"
                            FooterStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox
                                    ID="finalizedCB"
                                    runat="server"
                                    Checked='<%# Bind("[finalized]") %>'
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Width="85"
                                    />
                             </ItemTemplate>
                             <FooterTemplate>
                                <asp:CheckBox
                                    ID="finalizedCBF"
                                    runat="server"
                                    Width="85"
                                    />
                             </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField 
                            DataField="modifiedBy"  
                            HeaderText="Modified By" 
                            SortExpression="modifiedBy"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                    <asp:TemplateField 
                        HeaderText="Delete"
                        ItemStyle-Width="30px"
                        HeaderStyle-Width="30px"
                        ItemStyle-HorizontalAlign="Center"
                        >
                        <ItemTemplate>
                            <asp:ImageButton 
                                ID="ImageButtonDelete" 
                                runat="server" 
                                ImageUrl="~/images/controls/cross.png"
                                ImageAlign="Middle"
                                CommandArgument='<%# Eval("id") %>' 
                                CommandName="Delete"
                                OnClientClick="return confirm('Delete this Record?');"
                                />
                        </ItemTemplate>
                   </asp:TemplateField>
                        
                   </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="RadioButtonListStatus" EventName="SelectedIndexChanged"/>
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
    
    
    <asp:SqlDataSource 
        ID="SqlDataSourceRAT" 
        runat="server" 
        ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
        SelectCommand="uspBMcBEARRateAgreementTracker" 
        SelectCommandType="StoredProcedure"
        OnSelecting="SqlDataSource_OnSelecting"
        DeleteCommand="INSERT INTO dbo.BMcBEARRateAgreementTrackerDeleted SELECT * FROM dbo.BMcBEARRateAgreementTracker WHERE ID = @id; DELETE FROM BMcBEARRateAgreementTracker WHERE id = @id"
        DeleteCommandType="Text"
        >
    </asp:SqlDataSource>
    
    </form>
</body>
</html>
