<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BEAR.billTracker.Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>Bill Tracker Parameter Input</title>
    <link href="../style/style.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/billTracker.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/progress.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/general.js" type="text/javascript"></script>
</head>

<body>    

    <script type="text/javascript">

            function ToggleThreshold() {
            var checkBox = document.getElementById('CheckBoxThreshold');
            if(checkBox.checked) {
                document.getElementById('labelArrangementMessage').style.display='inline';
                document.getElementById('labelArrangementMessage').style.visibility='visible';
                document.getElementById('DropDownListArrangementCode').disabled = true;
                document.getElementById('DropDownListArrangementCode').style.background='#C8C8C8';
            } else {
                document.getElementById('labelArrangementMessage').style.display='inline';
                document.getElementById('labelArrangementMessage').style.visibility='hidden';
                document.getElementById('DropDownListArrangementCode').disabled = false;
                document.getElementById('DropDownListArrangementCode').style.background='White';
                
            }
        }
        
    </script>
    
    <script src="../scripts/wz_tooltip.js" type="text/javascript"></script>
    
    <form id="form1" runat="server" defaultbutton="buttonSubmitTkid">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
        </asp:ScriptManager>
        
        <script type="text/javascript">
            function showProgress() {
                // get the update progress div
                var processingPopup = $get('<%= this.processingPopup.ClientID %>'); 
                // make it visible
                processingPopup.style.display = '';
            }            
            
        </script>
        <asp:Panel ID="processingPopup" runat="server" CssClass="manuallyPlaced" style="display:none;">
        
            <img src="../images/processing.gif" alt="Proccessing Your Request..."/>
        </asp:Panel>
        
        <div id="title" class="titleBT">
            <img id="binghamLogo" alt="BINGHAM" src="../images/binghamLogoSmall.gif" />
            <img id="titlePic"  alt="Bill Tracker" src="../images/BillTrackerTitle.gif" class="titlePic" />
        </div>
        
        <div id="picture" class="pictureBT" runat="server">
     
            <asp:Label ID="LabelServerMessage" CssClass="LabelServerMessage" runat="server" Visible="False"  />

            <script type="text/javascript" language="JavaScript">
                loadPoster();
            </script>
        </div>
        
        <div id="options" class="options" runat="server">
            <asp:LinkButton ID="LinkButtonUserPreferences" runat="server" OnClick="LinkButtonUserPreferences_Click" Visible="false">Set Column Order</asp:LinkButton>
        </div>

        
        <div id="login" class="loginBT">
            <table cellpadding="3px">
            <tr>
                <td>
                    <asp:Label   ID="labelInvoiceAtty"   runat="server" CssClass="parameterLabel"   Text="Invoicing Attorney TKID:" /><br />
                    <asp:TextBox ID="textboxInvoiceAtty" runat="server" CssClass="parameterControl" Text="All" Visible="true"  /><asp:ImageButton ID="ImageButtonTKIDInvoiceAtty" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="ImageButtonTKIDInvoiceAttyClick"/>
                </td>
                <td>
                    <asp:Label   ID="labelBillingTkid"   runat="server" CssClass="parameterLabel"   Text="Proforma Attorney TKID:" /><br />
                    <asp:TextBox ID="textboxBillingTkid" runat="server" CssClass="parameterControl" Text="All" Visible="true"  /><asp:ImageButton ID="ImageButtonTKID" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="imageButtonTKIDClick"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="labelClient"     runat="server" CssClass="parameterLabel"   Text="Client Number:" /><br />
                    <asp:TextBox ID="textboxClient" runat="server" CssClass="parameterControl" Text="All" Visible="true" /><asp:ImageButton ID="ImageButtonClient" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="imageButtonClientClick"/>
                </td>
                <td>
                    <asp:Label ID="labelMatter"     runat="server" CssClass="parameterLabel"   Text="Matter Number:" /><br />
                    <asp:TextBox ID="textboxMatter" runat="server" CssClass="parameterControl" Text="All" Visible="true" /><asp:ImageButton ID="ImageButtonMatter" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="imageButtonMatterClick"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelReadyToBillFilter" runat="server" Text="Ready To Bill:" CssClass="parameterLabel" /><br />
                    <asp:ListBox ID="ListBoxReadyToBillFilter" runat="server" CssClass="parameterControlWide parameterControlThreeHigh">
                        <asp:ListItem Selected="True" Value="All" Text="Show All" />
                        <asp:ListItem Selected="False" Value="1" Text="Show Ready To Bill" />
                        <asp:ListItem Selected="False" Value="0" Text="Show Not Ready To Bill" />
                    </asp:ListBox>
                </td>
                <td valign="top" align="left">
                    <asp:Label   ID="labelBillingPeriod"   runat="server" CssClass="parameterLabel" Text="Billing Period:" /><br />
                    <asp:ListBox ID="listboxBillingPeriod" runat="server" CssClass="parameterControl parameterControlThreeHigh" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="labelInvoicingAttorneyOffice"     runat="server" CssClass="parameterLabel" Text="Office:" /><br />
                    <asp:ListBox ID="listboxInvoicingAttorneyOffice" runat="server" CssClass="parameterControlWide"
                                 SelectionMode="Single">
                        <asp:ListItem Selected="True">All</asp:ListItem>
                    </asp:ListBox>
                </td>
                <td>
                    <asp:RadioButtonList ID="RadioButtonListAttorneyOrBillingSpecialist" runat="server" RepeatDirection="Vertical">
                        <asp:ListItem Text="Invoicing Attorney" Value="IA" Selected="True" />
                        <asp:ListItem Text="Billing Specialist" Value="BS" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelException" runat="server" Text="Exceptions:" CssClass="parameterLabel" /><br />
                    <asp:ListBox ID="ListBoxException" runat="server" CssClass="parameterControlWide parameterControlThreeHigh">
                        <asp:ListItem Selected="True" Value="All" Text="Show All" />
                        <asp:ListItem Selected="False" Value="1" Text="Show Exceptions" />
                        <asp:ListItem Selected="False" Value="0" Text="Show Non-Exceptions" />
                    </asp:ListBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                        <td colspan="2">
                            <asp:Label ID="labelArrangementCode" runat="server" CssClass="parameterLabel" Text="Arrangement Code: " /><asp:Label ID="labelArrangementMessage" runat="server" Text=" (Sanctionable WIP = MP, MC, H, and HE)" CssClass="Arial10Black" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                            <asp:DropDownList ID="DropDownListArrangementCode" runat="server" Font-Size="9pt" Width="250" Enabled="false" BackColor="#C8C8C8">
                                <asp:ListItem Selected="True" Value="All" Text="All" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox ID="CheckBoxThreshold" runat="server" Text="Sanctionable WIP Only" Checked="true" />
                        </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label          ID="labelBillingSpecialist"        runat="server" Text="Billling Specialist:" CssClass="parameterLabel" /><br />
                    <asp:DropDownList   ID="dropDownListBillingSpecialist" runat="server"
                                        Font-Size="9pt" Width="250">
                        <asp:ListItem Selected="True">All</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="labelPracticeArea"     runat="server" CssClass="parameterLabel" Text="Practice Area:" /><br />
                    <asp:DropDownList ID="DropDownListPracticeArea" runat="server" Font-Size="9pt" Width="250" >
                        <asp:ListItem Selected="True" Value="All" Text="All" />
                    </asp:DropDownList>
                </td>
            </tr>
            </table>            
            
            <table cellpadding="0" border="0" cellspacing="0"><tr><td nowrap="nowrap">
            <asp:Button ID="buttonSubmitTkid" runat="server" 
                        Visible="true" 
                        CssClass="parameterButton" 
                        Text="Submit" 
                        OnClientClick="showProgress()"
                        OnClick="buttonLoginSubmitClick" 
                        />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="buttonReportOnly" runat="server" 
                        Visible="true" 
                        CssClass="parameterButton" 
                        Text="Report Only" 
                        OnClientClick="showProgress()"
                        OnClick="buttonReportOnlyClick" 
                        />
            </td></tr></table>
        </div>
        
        <asp:Panel ID="PanelSearchTKID" runat="server" DefaultButton="ImageButtonSearchTKID" Visible="false">
            <div id="searchTKID" runat="server" class="searchDivBT">
                <p>
                    <span class="searchTitle">Timekeeper ID Search</span> 
                        <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButtonCloseSearch" runat="server" class="searchClose" OnClick="LinkButtonCloseSearchClick">X</asp:LinkButton>
                        </span><br />
                    <span class="searchInstructions">Searchs First Name, Last Names, Titles, TKID and Location</span>
                </p>
                <asp:Label ID="labelSearchTKID" runat="server" Text="Enter Search Criteria" CssClass="parameterLabel" /><br />
                <asp:TextBox ID="textBoxSearchTKID" runat="server"/>
                <asp:ImageButton ID="ImageButtonSearchTKID" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="imageButtonSearchTKIDClick"/>
                <br />
                <div id="searchResultsTKID" class="searchResults">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="labelSearchResultsTKID" runat="server" Text="" CssClass="parameterLabel" Visible="true" /><br />
                            <asp:RadioButtonList 
                                ID="RadioButtonListSearchResultsTKID" 
                                runat="server" 
                                Visible="False" 
                                DataSourceID="SqlDataSource_BillingTimeKeepers" 
                                DataTextField="ConcatResult" 
                                DataValueField="tkinit"
                                >
                            </asp:RadioButtonList>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ImageButtonSearchTKID" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </asp:Panel>
        
        <asp:Panel ID="PanelSearchClient" runat="server" DefaultButton="ImageButtonSearchClient" Visible="false">
            <div id="searchClient" runat="server" class="searchDivBT">
                <p>
                    <span class="searchTitle">Client Search</span>
                    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton1" runat="server" class="searchClose" OnClick="LinkButtonCloseSearchClick">X</asp:LinkButton>
                    <br />
                    <span class="searchInstructions">Searchs Client Name, First Address Line, and Client Number</span>
                </p>
                <asp:Label ID="labelSearchClient" runat="server" Text="Enter Search Criteria" CssClass="parameterLabel" /><br />
                <asp:TextBox ID="textBoxSearchClient" runat="server" />
                <asp:ImageButton ID="ImageButtonSearchClient" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="imageButtonSearchClientClick"/>
                <br />
                <div id="searchResultsClient" class="searchResults">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="labelSearchResultsClient" runat="server" Text="" CssClass="parameterLabel" /><br />
                            <asp:RadioButtonList 
                                ID="RadioButtonListSearchResultsClient" 
                                runat="server" 
                                Visible="False" 
                                DataSourceID="SqlDataSource_Client" 
                                DataTextField="ConcatResult" 
                                DataValueField="clnum"
                                >
                            </asp:RadioButtonList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ImageButtonSearchClient" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </asp:Panel>

        
        <asp:Panel ID="PanelSearchMatter" runat="server" DefaultButton="ImageButtonSearchMatter" Visible="false">
            <div id="SearchMatter" runat="server" class="searchDivBT">
                <p>
                    <span class="searchTitle">Matter Search</span>
                    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LinkButton2" runat="server" class="searchClose" OnClick="LinkButtonCloseSearchClick">X</asp:LinkButton>
                    <br />
                    <span class="searchInstructions">Searchs Matter Descriptions and Matter Number (Returns top 1,000 Results)</span>
                </p>
                <asp:Label ID="labelSearchMatter" runat="server" Text="Enter Search Criteria" CssClass="parameterLabel" /><br />
                <asp:TextBox ID="textBoxSearchMatter" runat="server" />
                <asp:ImageButton ID="ImageButtonSearchMatter" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="imageButtonSearchMatterClick"/>
                <br />
                <div id="searchResultsMatter" class="searchResults">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
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
        
        <asp:SqlDataSource ID="SqlDataSource_BillingTimeKeepers" runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARSearchTKID" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_TKID_EscapeSingleQuote">
       </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource_Client" runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARSearchClient" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_Client_EscapeSingleQuote">
         </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource_Matter" runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARSearchMatter" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_Matter_EscapeSingleQuote">
         </asp:SqlDataSource>

    </form>
    
<%--        <script type="text/javascript">
            document.getElementById('DropDownListArrangementCode').disabled = true;
            document.getElementById('DropDownListArrangementCode').style.background='#C8C8C8';
        </script>--%>
    
</body>
</html>
