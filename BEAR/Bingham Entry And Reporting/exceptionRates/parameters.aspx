<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="parameters.aspx.cs" Inherits="BEAR.exceptionRates.parameters" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Exception Rates Parameter Input</title>
    <link href="../style/style.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/progress.css" rel="stylesheet" type="text/css" />
    <link href="../style/exceptionRates.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/general.js" type="text/javascript"></script>
</head>

<body>    
    
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
        
        <div id="title" class="title">
            <img id="binghamLogo" alt="BINGHAM" src="../images/binghamLogoSmall.gif" />
            <img id="titlePic"  alt="Exception Rates" src="../images/ExceptionRatesTitle.gif" class="titlePic" />
        </div>
        
        <div id="picture" class="picture" runat="server">
     
            <asp:Label ID="LabelServerMessage" CssClass="LabelServerMessage" runat="server" Visible="False"  />

            <script type="text/javascript" language="JavaScript">
                loadPoster();
            </script>
        </div>
        
        <div id="login" class="login">
            
            <asp:Label   ID="labelBillingTkid"   runat="server" CssClass="parameterLabel"   Text="Billing Attorney TKID:" /><br />
            <asp:TextBox ID="textboxBillingTkid" runat="server" CssClass="parameterControl" Text="All" Visible="true"  /><asp:ImageButton ID="ImageButtonTKID" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="imageButtonTKIDClick"/>
            
            <br />
            <asp:Label ID="labelClient"     runat="server" CssClass="parameterLabel"   Text="Client Number:" /><br />
            <asp:TextBox ID="textboxClient" runat="server" CssClass="parameterControl" Text="All" Visible="true" /><asp:ImageButton ID="ImageButtonClient" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="imageButtonClientClick"/>
            
            <br /><br />
            <asp:Label ID="labelBillingTimekeeperOffice"  runat="server" CssClass="parameterLabel" Text="Billing TK Office:" /><br />
            <asp:ListBox ID="listboxBillingTimekeeperOffice" runat="server" CssClass="parameterControlWide"
                         SelectionMode="Single">
                <asp:ListItem Selected="True">All</asp:ListItem>
            </asp:ListBox>

            <br />
            <asp:Label          ID="labelBillingSpecialist"        runat="server" Text="Billling Specialist:" CssClass="parameterLabel" /><br />
            <asp:DropDownList   ID="dropDownListBillingSpecialist" runat="server"
                                Font-Size="9pt">
                <asp:ListItem Selected="True">All</asp:ListItem>
            </asp:DropDownList>

            
            <br />
            <asp:Label ID="labelTCB" runat="server" CssClass="parameterLabel" Text="Exceptions:" /><br />
            <asp:ListBox ID="TCB"    runat="server" CssClass="parameterControl parameterControlThreeHigh">
                <asp:ListItem Selected="True" Value="T" Text="Timekeeper" />
                <asp:ListItem Selected="False" Value="C" Text="Costcode" />
                <asp:ListItem Selected="False" Value="B" Text="Both" />
            </asp:ListBox>
            
            <br /><br />
            <asp:Label ID="labelCMB" runat="server" CssClass="parameterLabel" Text="Rates:" /><br />
            <asp:ListBox ID="CMB"    runat="server" CssClass="parameterControl parameterControlThreeHigh">
                <asp:ListItem Selected="False" Value="C" Text="Client Rates" />
                <asp:ListItem Selected="False" Value="M" Text="Matter Rates" />
                <asp:ListItem Selected="True" Value="B" Text="Both" />
            </asp:ListBox>
            
            <br /><br />
            <asp:Label   ID="labelCalendarYear"   runat="server" CssClass="parameterLabel" Text="Calendar Year:" /><br />
            <asp:ListBox ID="listboxCalendarYear" runat="server" CssClass="parameterControl parameterControlOneHigh">
                <asp:ListItem Selected="False" Value="2009" Text="2009" Enabled="False"/>
                <asp:ListItem Selected="True" Value="2010" Text="2010" Enabled="true"/>
                <asp:ListItem Selected="False" Value="2011" Text="2011" Enabled="false"/>
                <asp:ListItem Selected="False" Value="2012" Text="2012" Enabled="false"/>
                <asp:ListItem Selected="False" Value="2013" Text="2013" Enabled="false"/>
                <asp:ListItem Selected="False" Value="2014" Text="2014" Enabled="false"/>
                <asp:ListItem Selected="False" Value="2015" Text="2015" Enabled="false"/>
            </asp:ListBox>
            
            <br /><br />
            <asp:Label ID="labelAttorneyReviewed"     runat="server" CssClass="parameterLabel" Text="Attorney Reviewed Filter:" /><br />
            <asp:ListBox ID="ListBoxAttorneyReviewed" runat="server" CssClass="parameterControl parameterControlThreeHigh">
                <asp:ListItem Selected="True" Value="-1" Text="Show All" />
                <asp:ListItem Selected="False" Value="1" Text="Reviewed" />
                <asp:ListItem Selected="False" Value="0" Text="Not Reviewed" />
            </asp:ListBox>
            
            <br /><br />
            <asp:Label ID="labelBillingReviewed" runat="server" Text="Billing Reviewed Filter:" CssClass="parameterLabel" /><br />
            <asp:ListBox ID="ListBoxBillingReviewed" runat="server" CssClass="parameterControl parameterControlThreeHigh">
                <asp:ListItem Selected="True" Value="-1" Text="Show All" />
                <asp:ListItem Selected="False" Value="1" Text="Reviewed" />
                <asp:ListItem Selected="False" Value="0" Text="Not Reviewed" />
            </asp:ListBox>

            <br /><br />
            <asp:Label ID="labelFinalized" runat="server" Text="Finalized Filter:" CssClass="parameterLabel" /><br />
            <asp:ListBox ID="ListBoxFinalized" runat="server" CssClass="parameterControl parameterControlThreeHigh">
                <asp:ListItem Selected="True" Value="-1" Text="Show All" />
                <asp:ListItem Selected="False" Value="1" Text="Reviewed" />
                <asp:ListItem Selected="False" Value="0" Text="Not Reviewed" />
            </asp:ListBox>
            
            <br /><br />
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
            <div id="searchTKID" runat="server" class="searchDivER">
                <p>
                    <span class="searchTitle">Timekeeper ID Search</span><br />
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
            <div id="searchClient" runat="server" class="searchDivER">
                <p>
                    <span class="searchTitle">Client Search</span><br />
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
        
        <asp:SqlDataSource ID="SqlDataSource_BillingTimeKeepers" runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARExceptionRatesSearchTKID" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_TKID_EscapeSingleQuote">
       </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource_Client" runat="server" 
            ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
            SelectCommand="uspBMcBEARExceptionRatesSearchClient" 
            SelectCommandType="StoredProcedure"
            OnSelecting="SqlDataSource_Client_EscapeSingleQuote">
         </asp:SqlDataSource>

    </form>
</body>
</html>
