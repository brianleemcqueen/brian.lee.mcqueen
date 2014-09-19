<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="billTracker.aspx.cs" Inherits="BEAR.billTracker.billTracker" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>Bill Tracker</title>

    <script src="bearbox/jqueryBillTracker.js" type="text/javascript"></script>
    <link href="bearbox/facebox.css" rel="stylesheet" type="text/css" />
    <script src="bearbox/facebox.js" type="text/javascript"></script>
    
    <link href="../style/global.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/style.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/headerGridLines.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/progress.css" rel="stylesheet" type="text/css" />
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
        var maximumNumberOfLockedCells = 6;
        var selectAllTip = "Select All<br /><em>this page only</em>";
        var clearAllTip = "Clear All<br /><em>this page only</em>";
        var lockColumnTip = "Lock / Unlock Column";
        
        /*
        *used to lock the column on a grid - like freezing a column in Excel
        */
        function lockColumn(cellId) {
            var tableRowCountAdjustment = 2;
            if(document.getElementById("hasPagerRow").value != 'true') {
                tableRowCountAdjustment = 0;
            }
            var table = document.getElementById(tableID);
            var tableRowsCollection = table.getElementsByTagName('tr');  

            //if the className is not set, the className needs to toggle to hidden
            if (table.rows[0].cells[cellId].className == '') 
            {
                //loop through all columns - selected and to the left
                for (h = 2; h <=cellId; h++) 
                {
                    //loop through all the cells in column[h]
                    for (i = 0; i < tableRowsCollection.length-tableRowCountAdjustment; i++)
                    {
                        var tr = tableRowsCollection.item(i);
                        if(tr.cells[h].className == 'hidden')
                        {
                            //leave hidden cells alone
                        }
                        else
                        {
                            tr.cells[h].className = 'locked';
                        }
                    }
                    //change the tack icon
                    document.getElementById(h).src = "../images/controls/freezeColumnOn.gif";
                    //set the lockedColumnNumber hidden input.  This is used during postback to keep 
                    //locked columns locked.
                    document.getElementById("lockedColumnNumber").value=cellId;
                    
                }
            } 
            else 
            {
                //unlock cells with CellID > passed in CellID
                for (h = 0; h < maximumNumberOfLockedCells; h++) 
                {
                    if(h<cellId) 
                    {
                        //do nothing
                    }
                    else 
                    {
                        for (i = 0; i < tableRowsCollection.length-tableRowCountAdjustment; i++)
                        {
                            var tr = tableRowsCollection.item(i);
                            if(tr.cells[h].className == 'hidden')
                            {
                                //leave hidden columns alone
                            }
                            else
                            {
                                tr.cells[h].className = '';
                            }
                        }
                        document.getElementById(h).src = "../images/controls/freezeColumnOff.gif";
                        document.getElementById("lockedColumnNumber").value=cellId-1;
                    }
                }
            }
        }
        
        
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

        }
        
        
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
        
    </script>

</head>
<body>

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
                var x = Math.round(gridViewBounds.width / 2) - Math.round(pnlPopupBounds.width / 2) + gridView.offsetParent.scrollLeft;
                //var x = Math.round(document.body.offsetWidth / 2) - Math.round(pnlPopupBounds.width / 2);
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


            function setUpPage() {
                var tbl = document.getElementById("dataGridView");
                var tableRows = tbl.getElementsByTagName('tr');
                //lock the pagerRow from scrolling left / right
                if(document.getElementById("hasPagerRow").value == 'true') 
                {
                    tableRows.item(tableRows.length-3).cells[0].className = 'locked';
                }
                       
                for( i=2; i < maximumNumberOfLockedCells; i++) 
                {
                    if (tbl.rows[0].cells[i].className == 'hidden') 
                    {
                        document.getElementById("hiddenColumnNumber").value=i;
                    }
                    else if (tbl.rows[0].cells[i].className == 'locked') 
                    {
                        document.getElementById(i).src = "../images/controls/freezeColumnOn.gif";
                    } 
                }
            }
            
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setUpPage);
            
        </script>
    

    <div id="title" class="title">
        <img alt="BINGHAM" src="../images/binghamLogoSmall.gif" />
        <img id="titlePic"  alt="Bill Tracker" src="../images/BillTrackerTitle.gif" class="titlePic" />
            
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

        &nbsp;
    </div>

    
    <div 
        id="options" 
        class="options"
        runat="server"
        visible="true">
        <asp:Button 
            ID="buttonSave" 
            runat="server" 
            Text="Save" 
            Width="50"
            Height="20px"
            Font-Size="8"
            />
        <asp:Button 
            ID="buttonChangeParameters" 
            runat="server" 
            Text="New Search" 
            Width="70"
            onclick="buttonChangeParametersClick" 
            Height="20px"
            Font-Size="8"
            />
        <input  
            type="button" 
            id="exitButton" 
            name="exitButton"
            class="exitButton"
            runat="server" 
            onclick="ExitApplication()" 
            value="Exit"
            />
        
        <br />
      
    </div>

    
    <div id="header" class="header">
        <table id="parameters" cellspacing="0" class="parmTable">
            <tr>
                <td class="parameter">Billing Period: <b><%= getBillingPeriod()%></b></td>
                <td class="parameter">Threshold Applied: <b><%= Request["threshold"].ToString()%></b></td>
                <td class="parameter">Invoicing Atty: <b><%= getInvoicingAttorney()%></b></td>
                <td class="parameter">Proforma Attorney: <b><%= getBillingAttorney()%></b></td>
                <td class="parameter">Billing Specialist: <b><%= getBillingSpecialist()%></b></td>
                <td class="parameter">Arrangement: <b><%= Request["arrangement"].ToString()%></b></td>
                <%
                    if (getPageCount() != 1)
                        Response.Write("<td class='parameterSpecialNotes'><b>" + getPageCount() + " Pages Generated</b></td>");
                %>
                <% if (getPageCount() > 0)
                       Response.Write("<td class='parameter'>Page <b>" + getCurrentPage() + "</b> of <b>" + getPageCount() + "</b></td>");
                %>
            </tr>
        </table>
    </div>


    <div 
        id="data"
        onscroll="SetDataDivPosition()"
        runat="server"
        class="data">
                <asp:GridView 
                    ID="dataGridView"
                    runat="server"
                    AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource_Elite_uspBMcExceptionList"
                    CellPadding="2"
                    ForeColor="#333333"
                    AllowSorting="True"
                    Font-Names="Arial"
                    RowStyle-Height="20px"
                    HeaderStyle-Height="20px"
                    HeaderStyle-Wrap="false"
                    AllowPaging="True"
                    DataKeyNames="billing_attorney,invoice_attorney"
                    PageSize="11"
                    OnRowDataBound="GridViewDataDivRowBindEvent"
                    OnDataBound="GridViewDataBoundEvent"
                    OnSelectedIndexChanged="GridViewDataSelectedIndexChanged"
                    OnPageIndexChanged="GridViewDataPageIndexChanged"
                    AlternatingRowStyle-Wrap="false"
                    RowStyle-Wrap="false"
                    CssClass="billTrackerGridView"
                    EmptyDataText='No Results Found.  Please select "New Search" to run another query.'
                    >
                    
                    <SelectedRowStyle CssClass="GridViewSelectedRow"/>
                    <EmptyDataRowStyle
                        BackColor="#2a76b2" 
                        Font-Bold="True" 
                        ForeColor="#FFFFFF" 
                        />
                    <PagerSettings 
                        Mode="NextPreviousFirstLast" 
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
                            HeaderText="Draft Sent<br /></a><img id='ck1' src='../images/controls/checkAll.gif' class='headerImage' onclick='checkColumn(1, true)' onmouseover='Tip(selectAllTip)' onmouseout='UnTip()' />&nbsp;<img id='uck1' src='../images/controls/checkNone.gif' class='headerImage' onclick='checkColumn(1, false)' onmouseover='Tip(clearAllTip)' onmouseout='UnTip()'/>"
                            SortExpression="draftSent"
                            ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass="locked"
                            ItemStyle-CssClass="locked"
                            >
                            <ItemTemplate>
                                <asp:CheckBox 
                                    ID="review1CB"
                                    runat="server" 
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Checked='<%# Bind("draftSent") %>'
                                    />
                            </ItemTemplate>
                        </asp:TemplateField>
                            
                         <asp:TemplateField
                            HeaderText="Ready to Bill<br /></a><img id='ck1' src='../images/controls/checkAll.gif' class='headerImage' onclick='checkColumn(2, true)' onmouseover='Tip(selectAllTip)' onmouseout='UnTip()' />&nbsp;<img id='uck1' src='../images/controls/checkNone.gif' class='headerImage' onclick='checkColumn(2, false)' onmouseover='Tip(clearAllTip)' onmouseout='UnTip()'/>"
                            SortExpression="readyToBill"
                            ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass="locked"
                            ItemStyle-CssClass="locked"
                            >
                            <ItemTemplate>
                                <asp:CheckBox 
                                    ID="review2CB"
                                    runat="server" 
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Checked='<%# Bind("readyToBill") %>'
                                    />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField 
                            DataField="_1" 
                            HeaderText="COLUMN1</a>&nbsp;&nbsp;&nbsp;<img id='2' runat='server' src='../images/controls/freezeColumnOff.gif' class='headerImage' onclick='lockColumn(2)' onmouseover='Tip(lockColumnTip)' onmouseout='UnTip()'/><a>" 
                            SortExpression="_1_SORT"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_2" 
                            HeaderText="COLUMN2</a>&nbsp;&nbsp;&nbsp;<img id='3' runat='server' src='../images/controls/freezeColumnOff.gif' class='headerImage' onclick='lockColumn(3)' onmouseover='Tip(lockColumnTip)' onmouseout='UnTip()'/><a>"
                            SortExpression="_2_SORT"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_3" 
                            HeaderText="COLUMN3</a>&nbsp;&nbsp;&nbsp;<img id='4' runat='server' src='../images/controls/freezeColumnOff.gif' class='headerImage' onclick='lockColumn(4)' onmouseover='Tip(lockColumnTip)' onmouseout='UnTip()'/><a>"
                            SortExpression="_3_SORT"
                            HtmlEncode="false"
                            HeaderStyle-Wrap="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_4" 
                            HeaderText="COLUMN4</a>&nbsp;&nbsp;&nbsp;<img id='5' runat='server' src='../images/controls/freezeColumnOff.gif' class='headerImage' onclick='lockColumn(5)' onmouseover='Tip(lockColumnTip)' onmouseout='UnTip()'/><a>" 
                            SortExpression="_4_SORT"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_5" 
                            HeaderText="COLUMN5" 
                            SortExpression="_5_SORT"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_6" 
                            HeaderText="COLUMN6" 
                            SortExpression="_6_SORT"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_7" 
                            HeaderText="COLUMN7" 
                            SortExpression="_7_SORT"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_8" 
                            HeaderText="COLUMN8" 
                            SortExpression="_8_SORT"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_9" 
                            HeaderText="COLUMN9" 
                            HtmlEncode="false"
                            SortExpression="_9_SORT"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_10" 
                            HeaderText="COLUMN10" 
                            SortExpression="_10_SORT" 
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_11" 
                            HeaderText="COLUMN11" 
                            SortExpression="_11_SORT" 
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_12" 
                            HeaderText="COLUMN12" 
                            SortExpression="_12_SORT" 
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="_13" 
                            HeaderText="COLUMN13" 
                            SortExpression="_13_SORT" 
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                       <asp:TemplateField
                            HeaderText="Reversal Code"
                            SortExpression="reversalCode"
                            ItemStyle-Wrap="false"
                            ControlStyle-CssClass="ShadedDataEntryColumn">
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="reversalTB"
                                    runat="server"
                                    onChange="ChangeRow('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    Text='<%# Bind("[reversalCode]") %>'
                                    BorderStyle="None"
                                    Width="50"
                                    />
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderText="Notes"
                            SortExpression="comment"
                            ItemStyle-Wrap="false"
                            ControlStyle-CssClass="ShadedDataEntryColumn">
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="NotesTB"
                                    runat="server"
                                    onChange="ChangeRow('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    Text='<%# Bind("[comment]") %>'
                                    BorderStyle="None"
                                    Width="200"
                                    />
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <big><span onmouseover="Tip('Expand Notes Field')" onmouseout="UnTip()">+</span></big>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField 
                            DataField="exception" 
                            HeaderText="Exception?" 
                            SortExpression="exception" 
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Wrap="false"
                            />
                        <asp:TemplateField
                            HeaderText="Exception Reason"
                            SortExpression="reason"
                            ItemStyle-Wrap="false"
                            ControlStyle-CssClass="ShadedDataEntryColumn">
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="ReasonTB"
                                    runat="server"
                                    onChange="ChangeRow('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    Text='<%# Bind("[reason]") %>'
                                    BorderStyle="None"
                                    Width="250"
                                    />
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <big><span onmouseover="Tip('Expand Exception Field')" onmouseout="UnTip()">+</span></big>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField 
                            DataField="reason" 
                            HeaderText="Exception Reason"
                            SortExpression="reason"
                            ItemStyle-HorizontalAlign="Left"
                            />
                        <asp:BoundField 
                            DataField="mcurrency" 
                            HeaderText="WIP & Billed<br />Currency" 
                            SortExpression="mcurrency" 
                            ItemStyle-HorizontalAlign="Center"
                            HtmlEncode="false"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden"
                            ItemStyle-Wrap="false" 
                            />
                        <asp:BoundField 
                            DataField="rate_code" 
                            HeaderText="Rate Code"
                            SortExpression="rate_code"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden"
                            ItemStyle-Wrap = "false"
                            />
                       <asp:BoundField 
                            DataField="practice_area" 
                            HeaderText="Practice Area"
                            SortExpression="practice_area"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden"                            
                            ItemStyle-Wrap = "false"
                            />
                       <asp:BoundField 
                            DataField="invoice_attorney" 
                            HeaderText="invoice_attorney"
                            SortExpression="clnum"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden"                            
                            ItemStyle-Wrap = "false"
                            />
                       <asp:BoundField 
                            DataField="billing_attorney" 
                            HeaderText="Billing Attorney"
                            SortExpression="billing_attorney"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden"                            
                            ItemStyle-Wrap = "false"
                            />
                       <asp:BoundField 
                            DataField="arrangement_code" 
                            HeaderText="Arrangement Code"
                            SortExpression="arrangement_code"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden"                            
                            ItemStyle-Wrap = "false"
                            />
                       <asp:BoundField 
                            DataField="ID" 
                            HeaderText="ID"
                            SortExpression="ID"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden"                            
                            ItemStyle-Wrap = "false"
                            />

                    </Columns>

                </asp:GridView>
        <asp:Panel ID="pnlPopup" runat="server" CssClass="progress" style="display:none;">
            <div class="progressContainer">
                <div class="progressHeader">Loading...</div>
                <div class="progressBody">
                    <img src="../images/activity.gif" alt="" />
                </div>
            </div>
        </asp:Panel>         
        
        
        <asp:SqlDataSource 
            ID="SqlDataSource_Elite_uspBMcExceptionList" 
            runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARBillTrackerColumns" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_Elite_uspBMcBEARBillTracker_Selecting"
            >
            <SelectParameters>
                <asp:QueryStringParameter 
                    Name="billingAttorney" 
                    QueryStringField="billtk" 
                    DefaultValue="All" 
                    Type="String" 
                    />
                <asp:QueryStringParameter 
                    Name="billingSpecialist" 
                    QueryStringField="billspec" 
                    DefaultValue="All" 
                    Type="String" 
                    />
                <asp:QueryStringParameter 
                    Name="billingPeriod" 
                    QueryStringField="billpd" 
                    DefaultValue="All" 
                    Type="Int32"
                    />
                <asp:QueryStringParameter 
                    Name="invoiceAttorney" 
                    QueryStringField="invoiceatty" 
                    DefaultValue="All" 
                    Type="String" 
                    />
                <asp:QueryStringParameter 
                    Name="arrangementCode" 
                    QueryStringField="arrangement" 
                    DefaultValue="All" 
                    Type="String" />
                <asp:QueryStringParameter 
                    Name="rtb" 
                    QueryStringField="rtb" 
                    DefaultValue="All" 
                    Type="String" />
                <asp:QueryStringParameter 
                    Name="matter" 
                    QueryStringField="matter" 
                    DefaultValue="All" 
                    Type="String" />
                <asp:QueryStringParameter 
                    Name="clnum" 
                    QueryStringField="clnum" 
                    DefaultValue="All" 
                    Type="String" />
                <asp:QueryStringParameter 
                    Name="threshold" 
                    QueryStringField="threshold" 
                    DefaultValue="false" 
                    Type="Boolean" />
                <asp:QueryStringParameter 
                    Name="iaofc" 
                    QueryStringField="iaofc" 
                    DefaultValue="false" 
                    Type="String" />
                <asp:QueryStringParameter 
                    Name="pa" 
                    QueryStringField="pa" 
                    DefaultValue="false" 
                    Type="String" />
                <asp:QueryStringParameter 
                    Name="exception" 
                    QueryStringField="exception" 
                    DefaultValue="All" 
                    Type="String" />
                <asp:QueryStringParameter 
                    Name="ofcType" 
                    QueryStringField="ofcType" 
                    DefaultValue="All" 
                    Type="String" />



            </SelectParameters>
        </asp:SqlDataSource>

    </div>
    
    </form>
    
    <script type="text/javascript">
        setUpPage();
    </script>

</body>
</html>