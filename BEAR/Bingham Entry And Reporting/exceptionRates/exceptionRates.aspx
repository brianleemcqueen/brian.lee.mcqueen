<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exceptionRates.aspx.cs" Inherits="BEAR.exceptionRates.exceptionRates" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>Exception Rates</title>
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
        var maximumNumberOfLockedCells = 4;
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
                for (h = 0; h <=cellId; h++) 
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
        
        
    </script>

</head>
<body onbeforeunload="closeIt()">

    <script src="../scripts/wz_tooltip.js" type="text/javascript"></script>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>
    
    <script type="text/javascript">


            function setUpPage() {
                var tbl = document.getElementById("dataGridView");
                var tableRows = tbl.getElementsByTagName('tr');
                //lock the pagerRow from scrolling left / right
                if(document.getElementById("hasPagerRow").value == 'true') 
                {
                    tableRows.item(tableRows.length-2).cells[0].className = 'locked';
                }
                       
                for( i=0; i < maximumNumberOfLockedCells; i++) 
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
        <img 
            alt="BINGHAM" 
            src="../images/binghamLogoSmall.gif" 
            />
        <img 
            alt="Exception Rates" 
            src="../images/ExceptionRatesTitle.gif" 
            class="titlePic"
            id ="titlePic"
            />
            
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
            ID="buttonPrint" 
            runat="server" 
            Text="Print" 
            Width="50"
            onclick="buttonPrintClick" 
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
                <td class="parameter">Billing Attorney: <b><%= getBillingAttorney()%></b></td>
                <td class="parameter">Billing Specialist: <b><%= getBillingSpecialist()%></b></td>
                <td class="parameter">Client: <b><%= getClientNumber()%></b></td>
                <td class="parameter">Billing Office: <b><%= Request.QueryString["billtkofc"].ToString()%></b></td>
                <td class="parameter">Exceptions: <b><%= getExceptions()%></b></td>
                <td class="parameter">Rates: <b><%= getRates()%></b></td>
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
                    DataKeyNames="billing_atty,client,matter,rate_type,effective_date"
                    PageSize="11"
                    OnRowDataBound="GridViewDataDivRowBindEvent"
                    OnDataBound="GridViewDataBoundEvent"
                    AlternatingRowStyle-Wrap="false"
                    RowStyle-Wrap="false"
                    EmptyDataText='No Results Found.  Please select "New Search" to run another query.'
                    >

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
                    <PagerStyle CssClass="GridViewPager" ForeColor="#FFFFFF"/>
                    <RowStyle CssClass="GridViewRow" />
                    <HeaderStyle 
                        BackColor="#2a76b2" 
                        Font-Bold="True" 
                        ForeColor="#FFFFFF" 
                        HorizontalAlign="Center"   
                        VerticalAlign="Bottom"
                        />
                    <SelectedRowStyle CssClass="GridViewSelectedRow"/>
                    <AlternatingRowStyle 
                        BackColor="White" 
                        ForeColor="#284775" 
                        />

                    <Columns>
                        <asp:BoundField 
                            DataField="billing_atty_name" 
                            HeaderText="Billing Attorney </a>&nbsp;&nbsp;&nbsp;<img id='0' runat='server' src='../images/controls/freezeColumnOff.gif' class='headerImage' onclick='lockColumn(0)' onmouseover='Tip(lockColumnTip)' onmouseout='UnTip()'/><a>" 
                            SortExpression="billing_atty_name"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="client_name" 
                            HeaderText="Client</a>&nbsp;&nbsp;&nbsp;<img id='1' runat='server' src='../images/controls/freezeColumnOff.gif' class='headerImage' onclick='lockColumn(1)' onmouseover='Tip(lockColumnTip)' onmouseout='UnTip()'/><a>"
                            SortExpression="client_name"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="matter_description" 
                            HeaderText="Matter</a>&nbsp;&nbsp;&nbsp;<img id='2' runat='server' src='../images/controls/freezeColumnOff.gif' class='headerImage' onclick='lockColumn(2)' onmouseover='Tip(lockColumnTip)' onmouseout='UnTip()'/><a>"
                            SortExpression="matter_description"
                            HtmlEncode="false"
                            HeaderStyle-Wrap="false"
                            HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-Width="190px"
                            >
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="rate_type" 
                            HeaderText="Rate Type</a>&nbsp;&nbsp;&nbsp;<img id='3' runat='server' src='../images/controls/freezeColumnOff.gif' class='headerImage' onclick='lockColumn(3)' onmouseover='Tip(lockColumnTip)' onmouseout='UnTip()'/><a>" 
                            SortExpression="rate_type"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="effective_date" 
                            HeaderText="Effective Date" 
                            DataFormatString="{0:MMM dd, yyyy}"
                            HtmlEncode="false"
                            SortExpression="effective_date">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="deviation_percent" 
                            HeaderText="Discount %" 
                            SortExpression="deviation_percent" 
                            ItemStyle-HorizontalAlign="Right"
                            />
                        <asp:BoundField 
                            DataField="maximum" 
                            HeaderText="Max" 
                            SortExpression="maximum"
                            ItemStyle-HorizontalAlign="Right" 
                            />
                        <asp:BoundField 
                            DataField="rate_code" 
                            HeaderText="Rate Code" 
                            SortExpression="rate_code"
                            ItemStyle-HorizontalAlign="Center"
                            />
                        <asp:BoundField 
                            DataField="rate" 
                            HeaderText="Current<br />Special Rate" 
                            SortExpression="rate"
                            ItemStyle-HorizontalAlign="Right"
                            HtmlEncode="false"
                            />
                        <asp:BoundField 
                            DataField="current_standard_rate" 
                            HeaderText="2009<br />Std Rate"
                            SortExpression="current_standard_rate" 
                            ItemStyle-HorizontalAlign="Right"
                            HtmlEncode="false"
                            />
                        <asp:BoundField 
                            DataField="newrate" 
                            HeaderText="2010<br />Std Rate" 
                            SortExpression="newrate"
                            ItemStyle-HorizontalAlign="Right"
                            HtmlEncode="false"
                            />
                        <asp:TemplateField
                            HeaderText="2010<br />Special<br />Rate"
                            SortExpression="exceptionRate"
                            ItemStyle-Wrap="false"
                            ControlStyle-CssClass="ShadedDataEntryColumn right"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="SRTB"
                                    runat="server"
                                    onChange="ChangeRow('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    Text='<%# Bind("[exceptionRate]") %>'
                                    BorderStyle="None"
                                    Width="75"

                                    />
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField
                            HeaderText="No Change<br /></a><img id='ck1' src='../images/controls/checkAll.gif' class='headerImage' onclick='checkColumn(1, true)' onmouseover='Tip(selectAllTip)' onmouseout='UnTip()' />&nbsp;<img id='uck1' src='../images/controls/checkNone.gif' class='headerImage' onclick='checkColumn(1, false)' onmouseover='Tip(clearAllTip)' onmouseout='UnTip()'/>"
                            SortExpression="review1"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox 
                                    ID="review1CB" 
                                    runat="server" 
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Checked='<%# Bind("review1") %>'
                                    />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        
                        
                        <asp:TemplateField
                            HeaderText="Notes"
                            SortExpression="Notes"
                            ItemStyle-Wrap="false"
                            ControlStyle-CssClass="ShadedDataEntryColumn">
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="NotesTB"
                                    runat="server"
                                    onChange="ChangeRow('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    Text='<%# Bind("[notes]") %>'
                                    BorderStyle="None"
                                    Width="200"
                                    />
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>

                        <asp:TemplateField
                            HeaderText="Billing<br />Reviewed<br /></a><img id='ck2' src='../images/controls/checkAll.gif' class='headerImage' onclick='checkColumn(2, true)' onmouseover='Tip(selectAllTip)' onmouseout='UnTip()'/>&nbsp;<img id='uck2' src='../images/controls/checkNone.gif' class='headerImage' onclick='checkColumn(2, false)' onmouseover='Tip(clearAllTip)' onmouseout='UnTip()'/>"
                            SortExpression="review2"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox 
                                    ID="review2CB" 
                                    runat="server" 
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Checked='<%# Bind("review2") %>'
                                    />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField
                            HeaderText="Finalized<br /></a><img id='ck3' src='../images/controls/checkAll.gif' class='headerImage' onclick='checkColumn(3, true)' onmouseover='Tip(selectAllTip)' onmouseout='UnTip()'/>&nbsp;<img id='uck3' src='../images/controls/checkNone.gif' class='headerImage' onclick='checkColumn(3, false)' onmouseover='Tip(clearAllTip)' onmouseout='UnTip()'/>"
                            SortExpression="review3"
                            ItemStyle-HorizontalAlign="Center"
                            >
                            <ItemTemplate>
                                <asp:CheckBox 
                                    ID="review3CB" 
                                    runat="server" 
                                    OnCheckedChanged="CheckBox_CheckChanged"
                                    Checked='<%# Bind("review3") %>'
                                    />
                            </ItemTemplate>
                        </asp:TemplateField>


                       <asp:BoundField 
                            DataField="billing_atty" 
                            HeaderText="Billing Attorney TKID"
                            SortExpression="billing_atty"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="client" 
                            HeaderText="Client #" 
                            SortExpression="client"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="matter" 
                            HeaderText="Matter #" 
                            SortExpression="matter"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="hidden"
                            FooterStyle-CssClass="hidden">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>

                    </Columns>

                </asp:GridView>

      
        
        

        <asp:SqlDataSource 
            ID="SqlDataSource_Elite_uspBMcExceptionList" 
            runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARExceptionRates" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_Elite_uspBMcExceptionList_Selecting"
            >
            <SelectParameters>
                <asp:QueryStringParameter 
                    Name="billingtimekeeper" 
                    QueryStringField="billtk" 
                    DefaultValue="All" 
                    Type="String" 
                    />
                <asp:QueryStringParameter 
                    Name="billingspecialist" 
                    QueryStringField="billspec" 
                    DefaultValue="All" 
                    Type="String" 
                    />
                <asp:QueryStringParameter 
                    Name="client" 
                    QueryStringField="client" 
                    DefaultValue="All" 
                    Type="String" 
                    />
                <asp:QueryStringParameter 
                    Name="billingtimekeeperoffice" 
                    QueryStringField="billtkofc" 
                    DefaultValue="All" 
                    Type="String" 
                    />
                <asp:QueryStringParameter 
                    Name="tcb" 
                    QueryStringField="tcb" 
                    DefaultValue="B" 
                    Type="String" />
                <asp:QueryStringParameter 
                    Name="cmb" 
                    QueryStringField="cmb" 
                    DefaultValue="B" 
                    Type="String" />
                <asp:QueryStringParameter 
                    Name="year" 
                    QueryStringField="year" 
                    Type="String" />
               <asp:QueryStringParameter 
                    Name="attorneyReview" 
                    QueryStringField="attorneyReview" 
                    Type="String"
                    DefaultValue="-1" />
               <asp:QueryStringParameter 
                    Name="billingReview" 
                    QueryStringField="billingReview" 
                    Type="String"
                    DefaultValue="-1" />
               <asp:QueryStringParameter 
                    Name="finalized" 
                    QueryStringField="finalized" 
                    Type="String"
                    DefaultValue="-1" />
               <asp:QueryStringParameter 
                    Name="attorneyUdf" 
                    QueryStringField="udf" 
                    Type="String"
                    DefaultValue="-1" />

            </SelectParameters>
        </asp:SqlDataSource>

    </div>
    
    </form>
    
    <script type="text/javascript">
        setUpPage();
    </script>

</body>
</html>