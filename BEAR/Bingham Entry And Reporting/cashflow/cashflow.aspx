<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cashflow.aspx.cs" Inherits="BEAR.cashflow.cashflow" EnableEventValidation="false"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>Cash Manager</title>

    <script src="bearbox/jqueryCashManager.js" type="text/javascript"></script>
    <link href="bearbox/facebox.css" rel="stylesheet" type="text/css" />
    <script src="bearbox/facebox.js" type="text/javascript"></script>
    
    <link href="../style/global.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/style.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/headerGridLines.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/progress.css" rel="stylesheet" type="text/css" />
    <link href="../style/cashFlowManager.css" rel="stylesheet" type="text/css" />
    <link href="../style/lockedHeader.css" rel="stylesheet" type="text/css" />
        
    <script src="../scripts/div.js" type="text/javascript"></script>
    <script src="../scripts/grid.js" type="text/javascript"></script>
    <script src="../scripts/general.js" type="text/javascript"></script>
    <script src="../scripts/pageClose.js" type="text/javascript"></script>

    <script type="text/javascript">
        
        window.onload = CreateDataDivCookie;  //see div.js
        var resetCookie="<%=Session["resetCookie"] %>";

        if(resetCookie=="true") {
            ResetDataDivPosition();
        }
        resetCookie = "";

        var tableID = "dataGridView";
        var selectAllTip = "Select All<br /><em>this page only</em>";
        var clearAllTip = "Clear All<br /><em>this page only</em>";
        
        
        function checkColumn(columnNumber, checked) {
            var table = document.getElementById(tableID);
            var tableRowsCollection = table.getElementsByTagName('tr');  
            var elementId = "";
            var tableRowCountAdjustment = 3;
            if(document.getElementById("hasPagerRow").value != 'true') {
                tableRowCountAdjustment = 1;
            }
            for (i = 0; i < tableRowsCollection.length-tableRowCountAdjustment; i++)
            {
                if((i+2) < 10) 
                {
                    elementId = tableID + "_ctl0" + (i+2) + "_review" + columnNumber + "CB";
                }
                else
                {
                    elementId = tableID + "_ctl" + (i+2) + "_review" + columnNumber + "CB";
                }
                              
                document.getElementById(elementId).checked = checked;
            }
            
            document.getElementById("AmountSelected").value = "Save to Update";
            document.getElementById("Remaining").value = "Save to Update";
            document.getElementById("AmountSelectedHidden").value = "-1";

        }
        

        var controlVariable = "";
        
        function populateBox(textboxID) 
        {
            controlVariable = textboxID;
            var textBoxValue = document.getElementById(textboxID).value;
            var htmlBeforeValue = "<textarea id='textArea1' name='comments' cols=50 rows=10 wrap=physical>";
            var htmlAfterValue = "</textarea>";
            var allPutTogether = htmlBeforeValue + textBoxValue + htmlAfterValue;
            jQuery.facebox(allPutTogether);
        }
        
        function AmountToPayChange() {
            document.getElementById("AmountToPayHidden").value = document.getElementById("AmountToPay").value;
        }

        function updateAmountSelected() {
            
            var amountToPay = stripCommas(document.getElementById("AmountToPay").value) * 1;
            var amountSelectedHidden = document.getElementById("AmountSelectedHidden").value * 1;

            var remainingHidden = amountToPay - amountSelectedHidden - 0;
            document.getElementById("RemainingHidden").value = remainingHidden;
            
            document.getElementById("AmountSelected").value = formatNumber(amountSelectedHidden.toFixed(2),2,',','.','','','(',')');
            document.getElementById("Remaining").value = formatNumber(remainingHidden.toFixed(2),2,',','.','','','(',')');
            document.getElementById("AmountToPay").value = formatNumber(amountToPay.toFixed(2),2,',','.','','','(',')');
        }            
        
        function SumToPay(amount, checkBoxId) { 
            var amountSelectedHidden = document.getElementById("AmountSelectedHidden").value * 1;
            if(document.getElementById("AmountSelectedHidden").value != -1) {
                var amountToPay = stripCommas(document.getElementById("AmountToPay").value) * 1;
                var amountSelected = document.getElementById("AmountSelected").value * 1;
                var remaining = document.getElementById("Remaining").value * 1;
                var remainingHidden = document.getElementById("RemainingHidden").value * 1;

                var checkBox = document.getElementById(checkBoxId);
                
                if(checkBox.checked) {
                    amountSelectedHidden = amountSelectedHidden + amount - 0;
                } else {
                    amountSelectedHidden = amountSelectedHidden - amount - 0;
                }
                remainingHidden = amountToPay - amountSelectedHidden - 0;
                
                document.getElementById("AmountSelectedHidden").value = amountSelectedHidden.toFixed(2);
                document.getElementById("RemainingHidden").value = remainingHidden.toFixed(2);
                document.getElementById("AmountSelected").value = formatNumber(amountSelectedHidden.toFixed(2),2,',','.','','','(',')');
                document.getElementById("Remaining").value = formatNumber(remainingHidden.toFixed(2),2,',','.','','','(',')');
            }
        }
        
    </script>

</head>
<body onload="changeScreenSize(1024,732)" >

    <script src="../scripts/wz_tooltip.js" type="text/javascript"></script>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>

    <script type="text/javascript">

            function onUpdating(){
                // get the update progress div
                var pnlPopup = $get('<%= this.pnlPopup.ClientID %>'); 

                //  get the gridview element        
                var gridView = $get('<%= this.dataGridView.ClientID %>');
                
                // get the bounds of both the gridview and the progress div
                var gridViewBounds = Sys.UI.DomElement.getBounds(gridView);
                var pnlPopupBounds = Sys.UI.DomElement.getBounds(pnlPopup);
                
                //  center of gridview
                //var x = Math.round(gridViewBounds.width / 2) - Math.round(pnlPopupBounds.width / 2) + gridView.offsetParent.scrollLeft;
                var x = Math.round(document.body.offsetWidth / 2) - Math.round(pnlPopupBounds.width / 2) - 100;
                //var y = Math.round(gridViewBounds.height / 2) - Math.round(pnlPopupBounds.height / 2);	    
                var y = Math.round(gridView.offsetParent.scrollTop + 100);	    

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
            
            function setUpPage() {
                if(document.getElementById("payColumnDisplayed").value == "true") {
                    updateAmountSelected();
                }
            }
            
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setUpPage);
            
        </script>

    <div id="title" class="title">
        <img alt="BINGHAM" src="../images/binghamLogoSmall.gif" />
        <img id="titlePic"  alt="Bill Tracker" src="../images/CashFlowManagerTitle.gif" class="titlePic" />
            
   <input
        id="lockedColumnNumber"
        runat="server"
        type="hidden"
        value="-1"
    />
   <input
        id="hiddenColumnNumber"
        runat="server"
        type="hidden"
        value="-1"
    />
    <input
        id="hasPagerRow"
        runat="server"
        type="hidden"
        value="false"
    />

    <input
        id="payColumnDisplayed"
        runat="server"
        type="hidden"
        value="true"
    />


        &nbsp;
    </div>


<div id="divTotalToPay" class="divTotalToPay" runat="server">
<asp:UpdatePanel ID="UpdatePanelTotalToPay" runat="server">
    <ContentTemplate>
        <table cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>Amount to Pay:</td>
            <td><input id="AmountToPay" type="text" class="inputReadOnlyLabel" runat="server"/></td>
        </tr>
        <tr>
            <td>Amount Selected:</td>
            <td><input id="AmountSelected" type="text" value="0" runat="server" class="inputReadOnlyLabel"/></td>
        </tr>
        <tr>
            <td>Remaining:</td>
            <td><input id="Remaining" type="text" value="0" runat="server" class="inputReadOnlyLabel"/></td>
        </tr>
        </table>
        <input id="AmountToPayHidden" type="hidden" value="0" runat="server" />
        <input id="AmountSelectedHidden" type="hidden" value="0" runat="server" />
        <input id="RemainingHidden" type="hidden" value="0" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="buttonSave" />
    </Triggers>
     </asp:UpdatePanel>
</div>

    
    <div 
        id="options" 
        class="options"
        runat="server"
        visible="true">
        <table>
            <tr>
                <td align="left">
                    <asp:Button 
                        ID="buttonSave" 
                        runat="server" 
                        Text="Save" 
                        Width="50"
                        Height="20px"
                        Font-Size="8"
                        />
                </td>
                <td align="center">
                    <asp:Button 
                        ID="buttonChangeParameters" 
                        runat="server" 
                        Text="Filter" 
                        Width="50"
                        onclick="buttonChangeParametersClick" 
                        Height="20px"
                        Font-Size="8"
                        />
                </td>
                <td align="right">
                    <input  
                        type="button" 
                        id="exitButton" 
                        name="exitButton"
                        class="exitButton"
                        runat="server" 
                        onclick="ExitApplication()" 
                        value="Exit"
                        />
                </td>
                <td align="right">
                    <asp:Button 
                        ID="buttonAdminOptions" 
                        runat="server" 
                        Text="Admin" 
                        Width="70"
                        onclick="buttonAdminOptionsClick" 
                        Height="20px"
                        Font-Size="8"
                        />
                </td>
            </tr>
            <tr>
                <td colspan="4">
<%--                    <table cellpadding="0" class="legendTable">
                        <tr>
                            <td colspan="4" class='legendTitleCell'>Priority Legend</td>
                        </tr>
                        <tr>
                            <td class='legendCell'>1: Pay When Possible</td>
                            <td class='legendCell'>3: Pay in 2010</td>
                         </tr>
                        <tr>
                            <td class='legendCell'>2: Pay By End of Year</td>
                            <td class='legendCell'>4: Hold</td>
                        </tr>
                    </table>
--%>
                    <table cellpadding="0" class="legendTable">
                        <tr>
                            <td colspan="4" class='legendTitleCell'>Priority Legend</td>
                        </tr>
                        <tr>
                            <td class='legendCell'>1: Pay in Next Check Run</td>
                         </tr>
                        <tr>
                            <td class='legendCell'>2: Hold</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    
    <div id="header" class="header">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <table id="parameters" cellspacing="0" class="parmTable">
                <tr>
                    <td class='parameter'>
                        <b><asp:Label ID="LabelCurrentPage" runat="server" Text="currentPage"></asp:Label></b>
                    </td>
                    <td class='parameter'>
                        <asp:Label ID="LabelMessage" runat="server" Text="" />
                    </td>
                </tr>
<%--
                <tr>
                    <%
                        if (getPageCount() != 1)
                            Response.Write("<td class='parameterSpecialNotes'><b>" + getPageCount() + " Pages Generated</b></td>");
                    %>
                    <% if (getPageCount() > 0)
                           Response.Write("<td class='parameter'>Page <b>" + getCurrentPage() + "</b> of <b>" + getPageCount() + "</b></td>");
                    %>
                    <td>
                        <asp:Label ID="LabelMessage" runat="server" Text="Brian"></asp:Label>
                    </td>
                </tr>
--%>
            </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="dataGridView" />
            </Triggers>
        </asp:UpdatePanel>
    </div>


    <div 
        id="data"
        onscroll="SetDataDivPosition()"
        runat="server"
        class="cashManagerData">
        <asp:UpdatePanel ID="UpdatePanelDataGridView" runat="server">
            <ContentTemplate>
                <asp:GridView 
                    ID="dataGridView"
                    runat="server"
                    AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource_Elite_uspBMcBEARCashFlowManager"
                    CellPadding="2"
                    ForeColor="#333333"
                    AllowSorting="True"
                    Font-Names="Arial"
                    RowStyle-Height="20px"
                    HeaderStyle-Height="20px"
                    HeaderStyle-Wrap="false"
                    PageSize="50"
                    AllowPaging="True"
                    DataKeyNames="barcode"
                    OnRowDataBound="GridViewDataDivRowBindEvent"
                    OnDataBound="GridViewDataBoundEvent"
                    OnSelectedIndexChanged="GridViewDataSelectedIndexChanged"
                    OnPageIndexChanged="GridViewDataPageIndexChanged"
                    AlternatingRowStyle-Wrap="false"
                    RowStyle-Wrap="false"
                    CssClass="cashManagerTrackerGridView"
                    EmptyDataText='No Results Found.  Please select "New Search" to run another query.'
                    >
                    
                    <SelectedRowStyle CssClass="GridViewSelectedRow"/>
                    <EmptyDataRowStyle
                        BackColor="#2a76b2" 
                        Font-Bold="True" 
                        ForeColor="#FFFFFF" 
                        />
                    <PagerSettings 
                        Mode="NumericFirstLast" 
                        FirstPageText="First"
                        LastPageText="Last"
                        NextPageText="Next"
                        PreviousPageText="Previous"
                        Position="Bottom"
                        />
                    <PagerStyle CssClass="GridViewPager GridViewFixedFooter" ForeColor="#FFFFFF" /> 
                    <RowStyle CssClass="GridViewRow" />
                    <HeaderStyle 
                        BackColor="#2a76b2" 
                        Font-Bold="True" 
                        ForeColor="#FFFFFF" 
                        HorizontalAlign="Center"   
                        VerticalAlign="Bottom"
                        />
                    <AlternatingRowStyle 
                        BackColor="White" 
                        ForeColor="#284775" 
                        />

                    <Columns>
                         <asp:TemplateField
                            HeaderText="Pay"
                            SortExpression="payFlag"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="25px"
                            >
                            <ItemTemplate>
                                <asp:CheckBox 
                                    ID="review1CB"
                                    runat="server" 
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Checked='<%# Bind("payFlag") %>'
                                    />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField
                            HeaderText="Pay Mthd"
                            SortExpression="paymentMethod"
                            ItemStyle-Wrap="false"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="50px"
                            ControlStyle-CssClass="ShadedDataEntryColumn">
                            <ItemTemplate>
                                <asp:DropDownList 
                                    ID="DropDownListPayMthd" 
                                    runat="server"
                                    Width="55"
                                    Font-Size="8"
                                    onChange="ChangeRow('ChangedRow')"
                                    OnSelectedIndexChanged="DropDownList_SelectedIndexChanged"
                                    >
                                    <asp:ListItem Selected="True" Text="" Value="" />
                                    <asp:ListItem Selected="False" Text="Bank" Value="Bank" />
                                    <asp:ListItem Selected="False" Text="PC-CY" Value="PC-CY" />
                                    <asp:ListItem Selected="False" Text="PC-FY" Value="PC-FY" />
                                </asp:DropDownList>  
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                      <asp:TemplateField
                            HeaderText="CM"
                            SortExpression="cmPriority"
                            ItemStyle-Wrap="false"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="26px"
                            >
                            <ItemTemplate>
                                <asp:RadioButtonList 
                                    ID="cmPriorityRadioButtonList" 
                                    runat="server"
                                    CellPadding="0"
                                    CellSpacing="0"
                                    OnSelectedIndexChanged="RadioButtonList_SelectedIndexChanged"
                                    >
                                    <asp:ListItem Selected="False" Text="1" Value="1" />
                                    <asp:ListItem Selected="True" Text="2" Value="2" />
<%--                                    
                                    <asp:ListItem Selected="False" Text="2" Value="2" />
                                    <asp:ListItem Selected="False" Text="3" Value="3" />
                                    <asp:ListItem Selected="True" Text="4" Value="4" />
--%>
                                </asp:RadioButtonList>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField
                            HeaderText="Dept"
                            SortExpression="deptPriority"
                            ItemStyle-Wrap="false"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="26px"
                            >
                            <ItemTemplate>
                                <asp:RadioButtonList 
                                    ID="deptPriorityRadioButtonList" 
                                    runat="server"
                                    CellPadding="0"
                                    CellSpacing="0"
                                    OnSelectedIndexChanged="RadioButtonList_SelectedIndexChanged"
                                    >
                                    <asp:ListItem Selected="False" Text="1" Value="1" />
                                    <asp:ListItem Selected="True" Text="2" Value="2" />
<%--
                                    <asp:ListItem Selected="False" Text="2" Value="2" />
                                    <asp:ListItem Selected="False" Text="3" Value="3" />
                                    <asp:ListItem Selected="True" Text="4" Value="4" />
--%>
                                </asp:RadioButtonList>
                                <asp:Label 
                                    ID="deptPriorityLabel" 
                                    runat="server" 
                                    Text='<%# Bind("[deptPriority]") %>'
                                    Width="10"
                                    Visible="false"
                                    />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderText="Dept"
                            SortExpression="department"
                            ItemStyle-Wrap="false"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="70px"
                            >
                            <ItemTemplate>
                                <asp:Label 
                                    ID="LabelDepartment" 
                                    runat="server" 
                                    Text='<%# Bind("[department]") %>'
                                    />
                                <asp:Label 
                                    ID="LabelGlCode" 
                                    runat="server" 
                                    Text='<%# Bind("[costGlCode]") %>'
                                    />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField 
                            DataField="vendorName" 
                            HeaderText="Vendor" 
                            SortExpression="vendorName"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="60px"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField
                            HeaderText="Invoice # / Date"
                            SortExpression="invoiceDate"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="55px"
                            >
                            <ItemTemplate>
                                <asp:Label 
                                    ID="LabelInvoiceNumber" 
                                    runat="server" 
                                    Text='<%# Bind("[invoiceNumber]") %>'
                                    Width="55px"
                                    />
                                <asp:Label 
                                    ID="LabelInvoiceDate" 
                                    runat="server" 
                                    Text='<%# Bind("[invoiceDate]") %>'
                                    />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField 
                            DataField="amount" 
                            HeaderText="Amount" 
                            SortExpression="amount"
                            HtmlEncode="false"
                            DataFormatString="{0:N2}"
                            ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Width="65px"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField
                            HeaderText="Notes"
                            SortExpression="deptNotes"
                            ItemStyle-Wrap="false"
                            ItemStyle-Width="175px"
                            >
                            <ItemTemplate>
                                <asp:Label ID="LabelExpandDept" runat="server" Text="+ Dept:"  Font-Underline="true"/><br />
                                <asp:TextBox
                                    ID="deptNotesTB"
                                    runat="server"
                                    onChange="ChangeRow('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    Text='<%# Bind("[deptNotes]") %>'
                                    BorderStyle="None"
                                    Width="175"
                                    Font-Size="8pt"
                                    CssClass="ShadedDataEntryColumn"
                                    />
                                <asp:Label 
                                    ID="deptNotesLabel" 
                                    runat="server" 
                                    Text='<%# Bind("[deptNotes]") %>'
                                    Width="175"
                                    Visible="false"
                                    />
                                <asp:Label ID="LabelExpandAP" runat="server" Text="+ Cash Management:" Font-Underline="true"/><br />
                                <asp:TextBox
                                    ID="cmNotesTB"
                                    runat="server"
                                    onChange="ChangeRow('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    Text='<%# Bind("[cmNotes]") %>'
                                    BorderStyle="None"
                                    Width="175"
                                    Font-Size="8pt"
                                    CssClass="ShadedDataEntryColumn"
                                    />
                                <asp:Label 
                                    ID="cmNotesLabel" 
                                    runat="server" 
                                    Text='<%# Bind("[cmNotes]") %>'
                                    Width="175"
                                    Visible="false"
                                    />
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:BoundField 
                            DataField="description" 
                            HeaderText="Description" 
                            SortExpression="description"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="120px"
                            >
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="enteredBy" 
                            HeaderText="Entered By" 
                            SortExpression="EnteredBy"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="45px"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="source" 
                            HeaderText="Src" 
                            SortExpression="source"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="30px"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="barcode" 
                            HeaderText="Barcode" 
                            SortExpression="barcode"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="45px"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="location" 
                            HeaderText="Loc" 
                            SortExpression="location" 
                            ItemStyle-Wrap = "false"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="25px"
                            >
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="office" 
                            HeaderText="Office" 
                            SortExpression="office" 
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="60px"
                            >
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="invoiceNumber" 
                            HeaderText="Invoice # Hidden" 
                            SortExpression="invoiceNumber" 
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            ItemStyle-Wrap = "false"
                            >
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="invoiceDateStamp" 
                            HeaderText="Invoice Date Hidden" 
                            SortExpression="invoiceDateStamp" 
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            
                            ItemStyle-Wrap = "false"
                            >
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="vendorId" 
                            HeaderText="Vendor ID" 
                            SortExpression="vendorId" 
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="vendorName" 
                            HeaderText="Vendor Name" 
                            SortExpression="vendorName" 
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="amount" 
                            HeaderText="AmountHidden" 
                            SortExpression="amount"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="id" 
                            HeaderText="idHidden" 
                            SortExpression="id"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="paymentMethod" 
                            HeaderText="paymentMethodHidden" 
                            SortExpression="paymentMethod"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="cmPriority" 
                            HeaderText="cmPriorityHidden" 
                            SortExpression="cmPriority"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="deptPriority" 
                            HeaderText="deptPriorityHidden" 
                            SortExpression="deptPriority"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="currency" 
                            HeaderText="currencyHidden" 
                            SortExpression="currency"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="updateTime" 
                            HeaderText="updateTime" 
                            SortExpression="updateTime"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        
                    </Columns>

                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="buttonSave" />
            </Triggers>
        </asp:UpdatePanel>
        <cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server" TargetControlID="UpdatePanelDataGridView" >
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
        
        <asp:SqlDataSource 
            ID="SqlDataSource_Elite_uspBMcBEARCashFlowManager" 
            runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARCashFlowManager" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_Elite_uspBMcBEARCashFlowManager_Selecting"
            >
        </asp:SqlDataSource>

    </div>
    
    </form>
    
    <script type="text/javascript">
        setUpPage();
    </script>

</body>

</html>
