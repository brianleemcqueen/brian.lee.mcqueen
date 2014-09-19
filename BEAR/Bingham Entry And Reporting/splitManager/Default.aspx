<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BEAR.splitManager.Default" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>Split Manager</title>
    <link href="../style/global.css" rel="stylesheet" type="text/css" />
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <link href="../style/splitManager.css" rel="stylesheet" type="text/css" />
    <link href="../style/progress.css" rel="stylesheet" type="text/css" />
    <link href="../style/lockedHeader.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/general.js" type="text/javascript"></script>
    <script src="../scripts/grid.js" type="text/javascript"></script>
    <script src="../scripts/div.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        
        window.onload = CreateDataDivCookie;  //see div.js
        var resetCookie="<%=Session["resetCookie"] %>";

        if(resetCookie=="true") {
            ResetDataDivPosition();
        }
        resetCookie = "";
        
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
                
            }
            
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(setUpPage);
            
    </script>

    <div id="options" class="options" runat="server">
        
    </div>
    
    <div id="title" class="title">
        <table>
            <tr>
                <td>
                    <img 
                        alt="BINGHAM" 
                        src="../images/binghamLogoSmall.gif" 
                        />
                    <a href="Default.aspx">
                        <img 
                            alt="Split Manager" 
                            src="../images/SplitManagerTitle.gif" 
                            class="titlePic"
                            id ="titlePic"
                            style="border-style: none"
                            />
                    </a>
                </td>
                <td>
                    &nbsp;&nbsp;&nbsp;<asp:Button ID="ButtonCopy" runat="server" Text="Create Copy" OnClick="ButtonCopy_Click" class="Button2"/>    
                </td>
            </tr>
        </table>
    </div>
    
    <asp:UpdatePanel runat="server">
    <ContentTemplate>
    <div id="picture" class="picture" runat="server">
 
        <asp:Label ID="LabelServerMessage" CssClass="LabelServerMessage" runat="server" Visible="False"  />

        <script type="text/javascript" language="JavaScript">
            loadPoster();
        </script>
    </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="DropDownListStartDates" />
    </Triggers>
    </asp:UpdatePanel>

    
    <asp:Panel ID="PanelHeader" runat="server" DefaultButton="ButtonGetStartDates">
        <div id="DivHeader" runat="server" class="headerDiv">
        <asp:UpdatePanel ID="UpdatePanelHeader" runat="server">
            <ContentTemplate>
            <table border="0" >
                <tr>
                    <td>
                        <asp:Label ID="LabelMasterMatter" runat="server" Text="Master Matter"></asp:Label>
                    </td><td>
                        <asp:TextBox ID="TextBoxMasterMatter" runat="server"  Width="75px" OnTextChanged="TextBoxMasterMatter_TextChanged" /><asp:ImageButton ID="ImageButtonMasterMatter" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="ImageButtonMasterMatter_Click"/>
                        <asp:Button ID="ButtonGetStartDates" runat="server" Text="Get Start Dates" OnClick="ButtonGetStartDates_Click" class="Button2"/>
                        <asp:Button ID="ButtonChangeMasterMatter" runat="server" Text="Change Matter" OnClick="ButtonChangeMasterMatter_Click" visible="false" class="Button2"/>
                    </td>
                </tr><tr>
                    <td>
                    </td><td>
                        <asp:Label ID="LabelMasterMatterDescription" runat="server" Text="" />
                    </td>
                </tr><tr>
                    <td>
                        <asp:Label ID="LabelStartDates" runat="server" Text="Start Date" Visible="false"/>
                    </td><td>
                        <asp:DropDownList ID="DropDownListStartDates" runat="server" Visible="false" OnSelectedIndexChanged="DropDownListStartDates_SelectedIndexChanged" AutoPostBack="true"/>
                    </td>
                </tr>
            </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonGetStartDates" />
            </Triggers>
        </asp:UpdatePanel>
        </div>
    </asp:Panel>
    
    
    <asp:UpdatePanel ID="UpdatePanelSearchMasterMatter" runat="server">
    <ContentTemplate>
        <asp:Panel ID="PanelSearchMasterMatter" runat="server" DefaultButton="ImageButtonSearchMasterMatter" Visible="false">
            <div id="SearchMasterMatter" runat="server" class="searchDivMasterMatter">
                <p>
                    <span class="searchTitle">Master Matter Search</span>
                    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><asp:LinkButton ID="LinkButtonCloseSearchMasterMatter" runat="server" class="searchClose" OnClick="LinkButtonCloseSearchClick">X</asp:LinkButton>
                    <br />
                    <span class="searchInstructions">Searchs Master Matter Descriptions and Number</span>
                </p>
                <asp:Label ID="LabelSearchMasterMatter" runat="server" Text="Enter Search Criteria" CssClass="parameterLabel" /><br />
                <asp:TextBox ID="TextBoxSearchMasterMatter" runat="server" />
                <asp:ImageButton ID="ImageButtonSearchMasterMatter" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="ImageButtonSearchMasterMatter_Click"/>
                <br />
                <div id="searchResultsMasterMatter" class="searchResults">
                    <asp:UpdatePanel ID="UpdatePanelSearchMasterMatterResults" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LabelSearchResultsMasterMatter" runat="server" Text="" CssClass="parameterLabel" /><br />
                            <asp:RadioButtonList 
                                ID="RadioButtonListSearchResultsMasterMatter" 
                                runat="server" 
                                Visible="False" 
                                DataSourceID="SqlDataSource_MasterMatter" 
                                DataTextField="ConcatResult" 
                                DataValueField="mmatter"
                                >
                            </asp:RadioButtonList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ImageButtonSearchMasterMatter" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ImageButtonMasterMatter" />
    </Triggers>
    </asp:UpdatePanel>

    <asp:Panel ID="PanelDataGridView" runat="server" DefaultButton="ButtonSave" >
    <asp:UpdatePanel ID="UpdatePanelDataGridView" runat="server">
        <ContentTemplate>
            <div id="divTotals" runat="server" class="divTotals" visible="false">
                <table class="tableTotals" cellspacing="0">
                    <tr>
                        <td class="cellNoRightBorder">
                            Total Amount:
                        </td>
                        <td class="cellNoLeftBorder">
                            <%--<asp:TextBox ID="TextBoxTotalAmount" runat="server" Text="" Enabled="false"></asp:TextBox>--%>
                            <b><asp:Label ID="LabelTotalAmount" runat="server" Text=""></asp:Label></b>
                        </td>
                        <td colspan="2" class="cellNoBorders"></td>
                        <td class="cellNoRightBorder">
                            Total Actual Percent:
                        </td>
                        <td class="cellNoLeftBorder">
                            <%--<asp:TextBox ID="TextBoxTotalActualPercent" runat="server" Text="" Enabled="false"></asp:TextBox>--%>
                            <b><asp:Label ID="LabelTotalActualPercent" runat="server" Text=""></asp:Label></b>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divGridButtons" runat="server" class="divGridButtons">
            <table><tr><td>
                    <asp:Button ID="ButtonSave" runat="server" Text="Save" class="GridHeaderButton" Visible="false" OnClick="ButtonSave_Click"/>
                </td><td>
                    <asp:RadioButtonList ID="RadioButtonListCalculate" runat="server" RepeatDirection="Horizontal" Visible="false" class="GridHeaderRadioButtonList">
                        <asp:ListItem Text="Calculate" Value="C" />
                        <asp:ListItem Text="Spread" Value="S" />
                        <asp:ListItem Text="None" Value="0" Selected="True"/>
                    </asp:RadioButtonList>
                </td><td>
                    <asp:Button ID="ButtonAddRecord" runat="server" Text="Add" class="GridHeaderButton" Visible="false" OnClick="ButtonAddRecord_Click" />
                </td><td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td><td>
                    <asp:ImageButton ID="ImageButtonCalcToActual" runat="server" ImageUrl="~/images/controls/ArrowCalcActual.gif" OnClick="ImageButtonCalcToActual_Click" Visible="false"/>
                </td><td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td><td>
                    <asp:ImageButton ID="ImageButtonUpdateFromElite" runat="server" ImageUrl="~/images/controls/ArrowUpdateFromElite.gif" OnClick="ImageButtonUpdateFromElite_Click" OnClientClick="javascript:return confirm('Are you sure you want to reset from Elite?');" Visible="false"/>
                </td><td>
                    &nbsp;&nbsp;
                    <asp:ImageButton ID="ImageButtonElite" runat="server" OnClientClick="javascript:return confirm('Are you sure you want to update Elite?');" OnClick="ImageButtonElite_Click" Visible="false" ImageUrl="../images/controls/updateElite.png" />
                </td></tr></table>
            </div>
            <div 
                id="data"
                onscroll="SetDataDivPosition()"
                runat="server"
                class="splitManagerDataDiv"
                >
                <asp:GridView 
                    ID="dataGridView"
                    runat="server"
                    AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource_Elite_uspBMcBEARSplitManager"
                    Visible="False"
                    CellPadding="3"
                    ForeColor="#333333"
                    AllowSorting="True"
                    Font-Names="Arial"
                    RowStyle-Height="20px"
                    HeaderStyle-Height="20px"
                    HeaderStyle-Wrap="false"
                    AllowPaging="False"
                    DataKeyNames="id"
                    OnRowDataBound="dataGridView_RowDataBound"
                    OnDataBound="dataGridView_DataBound"
                    AlternatingRowStyle-Wrap="false"
                    RowStyle-Wrap="false"
                    EmptyDataText='No Results Found.'
                    CssClass="splitManagerGridView"
                    GridLines="Both"
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
                    <PagerStyle CssClass="GridViewPager GridViewFixedFooter" ForeColor="#FFFFFF"/> 
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
                        
                        <asp:BoundField 
                            DataField="subMatter" 
                            HeaderText="Sub-Matter" 
                            SortExpression="subMatter"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="100px"
                            HeaderStyle-Width="100px"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>

                        <asp:BoundField 
                            DataField="matterDescription" 
                            HeaderText="Sub-Matter Description" 
                            SortExpression="matterDescription"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="175px"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        
                        <asp:TemplateField
                            HeaderText="Amount"
                            SortExpression="Amount"
                            ItemStyle-Wrap="false"
                            ItemStyle-Width="100px"
                            HeaderStyle-Width="100px"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="amountTB"
                                    runat="server"
                                    onChange="ChangeRow('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    Text='<%# Bind("[Amount]") %>'
                                    BorderStyle="None"
                                    Width="100"
                                    Font-Size="8pt"
                                    CssClass="ShadedDataEntryColumn right"
                                    />
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>

                        <asp:BoundField 
                            DataField="PctCalculated" 
                            HeaderText="Calculated<br/>Percent" 
                            SortExpression="PctCalculated"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Width="75px"
                            HeaderStyle-Width="75px"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        
                        <asp:TemplateField
                            HeaderText="Actual<br />Percent"
                            SortExpression="PctActual"
                            ItemStyle-Wrap="false"
                            ItemStyle-Width="75px"
                            HeaderStyle-Width="75px"
                            >
                            <ItemTemplate>
                                <asp:TextBox
                                    ID="PctActualTB"
                                    runat="server"
                                    onChange="ChangeRow('ChangedRow')"
                                    OnTextChanged="TextBox_TextChanged"
                                    Text='<%# Bind("[PctActual]") %>'
                                    BorderStyle="None"
                                    Width="75"
                                    Font-Size="8pt"
                                    CssClass="ShadedDataEntryColumn right"
                                    />
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        
                    <asp:BoundField 
                        DataField="PctElite" 
                        HeaderText="Elite Percent" 
                        SortExpression="PctElite"
                        HtmlEncode="false"
                        ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="75px"
                        HeaderStyle-Width="75px"
                        >
                        <ItemStyle Wrap="false" />
                    </asp:BoundField>

                        
                    <asp:TemplateField 
                        HeaderText=""
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
                                />
                        </ItemTemplate>
                   </asp:TemplateField>
                        
                        
                        <asp:BoundField 
                            DataField="id" 
                            HeaderText="id" 
                            SortExpression="id"
                            ItemStyle-CssClass="hidden"
                            HeaderStyle-CssClass="displayNone"
                            FooterStyle-CssClass="displayNone"                            
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DropDownListStartDates" EventName="SelectedIndexChanged" />
        </Triggers>
       </asp:UpdatePanel>
       </asp:Panel>
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
            ID="SqlDataSource_Elite_uspBMcBEARSplitManager" 
            runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARSplitManager" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_Elite_uspBMcBEARSplitManager_Selecting"
            DeleteCommand="uspBMcBEARSplitManagerDeleteDetail"
            DeleteCommandType="StoredProcedure"
            
            >
        </asp:SqlDataSource>


    <asp:UpdatePanel ID="UpdatePanelAdminMessage" runat="server">
        <ContentTemplate>
            <div id="DivAdminMessage" runat="server" class="DivMessage">
                <asp:Label ID="LabelAdminMessage" runat="server" Text=""></asp:Label>
            </div>     
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ImageButtonElite" />
        </Triggers>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpdatePanelAddRecord" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelAddRecord" runat="server" DefaultButton="ButtonSaveAdd" Visible="false">
                <div id="DivMessage" runat="server" class="DivMessage">
                    <asp:Label ID="LabelMessage" runat="server" Text=""></asp:Label>
                </div>        
                <div id="DivAddRecord" runat="server" class="addRecordDiv">
                    <table border="0" cellpadding="0">
                        <tr>
                            <td colspan = "2">
                                <table class="headerTitleCell">
                                    <tr>
                                        <td>
                                            Add Row
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr><tr>
                            <td>
                                <asp:Label ID="LabelSubMatterAdd" runat="server" Text="Sub-Matter:"></asp:Label>
                            </td><td>
                                <asp:TextBox ID="TextBoxSubMatterAdd" runat="server"  Width="75px" CssClass="ShadedDataEntryColumn" BorderStyle="None" Font-Size="8pt"/></td><td><asp:ImageButton ID="ImageButtonSubMatter" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="ImageButtonSubMatter_Click"/>
                            </td>
                        </tr><tr>
                            <td>
                                <asp:Label ID="LabelAmountAdd" runat="server" Text="Amount:"></asp:Label>
                            </td><td>
                                <asp:TextBox ID="TextBoxAmountAdd" runat="server"  Width="75px" CssClass="ShadedDataEntryColumn" BorderStyle="None" Font-Size="8pt"/>
                            </td>
                        </tr><tr>
                            <td>
                                <asp:Label ID="LabelActualPctAdd" runat="server" Text="Actual %:"></asp:Label>
                            </td><td>
                                <asp:TextBox ID="TextBoxActualPctAdd" runat="server"  Width="75px" CssClass="ShadedDataEntryColumn" BorderStyle="None" Font-Size="8pt"/>
                            </td>
                        </tr><tr>
                            <td>
                                <asp:Button ID="ButtonSaveAdd" runat="server" Text="Add" class="Button2" OnClick="ButtonSaveAdd_Click" />
                            </td>
                            <td>
                                <asp:Button ID="ButtonCancelAdd" runat="server" Text="Cancel" class="Button2" OnClick="ButtonCancelAdd_Click"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>  
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonAddRecord" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanelSearchSubMatter" runat="server">
    <ContentTemplate>
        <asp:Panel ID="PanelSearchSubMatter" runat="server" DefaultButton="ImageButtonSearchSubMatter" Visible="false">
            <div id="SearchSubMatter" runat="server" class="searchDivSubMatter">
                <p>
                    <span class="searchTitle">Sub-Matter Search</span>
                    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><asp:LinkButton ID="LinkButtonCloseSearchSubMatter" runat="server" class="searchClose" OnClick="LinkButtonCloseSearchClick">X</asp:LinkButton>
                    <br />
                    <span class="searchInstructions">Searchs Matter Descriptions and Number<br /> (excludes Closed Matters)</span>
                </p>
                <asp:Label ID="LabelSearchSubMatter" runat="server" Text="Enter Search Criteria" CssClass="parameterLabel" /><br />
                <asp:TextBox ID="TextBoxSearchSubMatter" runat="server" />
                <asp:ImageButton ID="ImageButtonSearchSubMatter" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="ImageButtonSearchSubMatter_Click"/>
                <br />
                <div id="searchResultsSubMatter" class="searchResultsSubMatter">
                    <asp:UpdatePanel ID="UpdatePanelSearchSubMatterResults" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="LabelSearchResultsSubMatter" runat="server" Text="" CssClass="parameterLabel" /><br />
                            <asp:RadioButtonList 
                                ID="RadioButtonListSearchResultsSubMatter" 
                                runat="server" 
                                Visible="False" 
                                DataSourceID="SqlDataSource_SubMatter" 
                                DataTextField="ConcatResult" 
                                DataValueField="mmatter"
                                >
                            </asp:RadioButtonList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ImageButtonSearchSubMatter" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ImageButtonSubMatter" />
    </Triggers>
    </asp:UpdatePanel>
        
        

        <asp:SqlDataSource ID="SqlDataSource_MasterMatter" runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARSearchMasterMatter" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_MasterMatter_EscapeSingleQuote">
         </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource_SubMatter" runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARSearchSubMatter" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_SubMatter_EscapeSingleQuote">
         </asp:SqlDataSource>


    
    
    </form>
</body>
</html>
