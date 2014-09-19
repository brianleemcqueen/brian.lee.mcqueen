<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Copy.aspx.cs" Inherits="BEAR.splitManager.Copy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>Split Manager Copy Master Matter</title>
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <link href="../style/splitManager.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/general.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>
    
    <div id="options" class="options" runat="server">
        <asp:Button ID="ButtonHome" runat="server" Text="Home" OnClick="ButtonHome_Click" class="Button2"/>
    </div>
    
    <div id="title" class="title">
        <img 
            alt="BINGHAM" 
            src="../images/binghamLogoSmall.gif" 
            />
        <img 
            alt="Split Manager" 
            src="../images/SplitManagerTitle.gif" 
            class="titlePic"
            id ="titlePic"
            style="border-style: none"
            />
        <img id="ImageAdminTitle"  alt="Administrator Options" src="../images/AdminTitle.gif" class="titlePic" />
    </div>
    
    <asp:Panel ID="PanelAdmin" runat="server" DefaultButton="ButtonSave" Visible="true">
        <div id="DivCopy" runat="server" class="headerDiv">
        <asp:UpdatePanel ID="UpdatePanelHeader" runat="server">
            <ContentTemplate>
            <table border="0" >
                <tr><td colspan="2">
                    <asp:Label ID="LabelHeading" runat="server" Text="Create A Master Matter Copy" CssClass="headerTitleCell" Width="100%"></asp:Label>
                </td></tr>
                <tr>
                    <td style="width:100px">
                        <asp:Label ID="LabelMasterMatter" runat="server" Text="Master Matter"></asp:Label>
                    </td><td>
                        <asp:TextBox ID="TextBoxMasterMatter" runat="server"  Text="" Width="75px" OnTextChanged="TextBoxMasterMatter_TextChanged" /><asp:ImageButton ID="ImageButtonMasterMatter" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="ImageButtonMasterMatter_Click"/>
                        <asp:Button ID="ButtonGetStartDates" runat="server" Text="Get Start Dates" OnClick="ButtonGetStartDates_Click" class="Button2"/>
                    </td>
                </tr><tr>
                    <td>
                    </td><td>
                        <asp:Label ID="LabelMasterMatterDescription" runat="server" Text="" />
                    </td>
                </tr><tr>
                    <td>
                        <asp:Label ID="LabelStartDates" runat="server" Text="Start Date<br />(To Copy From)" Visible="false"/>
                    </td><td>
                        <asp:DropDownList ID="DropDownListStartDates" runat="server" Visible="false" OnSelectedIndexChanged="DropDownListStartDates_SelectedIndexChanged" AutoPostBack="true"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LabelNewStartDate" runat="server" Text="New Start Date" Visible="false"></asp:Label>
                        <br />
                        <asp:Label ID="LabelNewStartDateInstructions" runat="server" Text="(yyyy-mm-dd or <br/> &nbsp; mm-dd-yyyy)" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <br />
                        <asp:TextBox ID="TextBoxNewStartDate" runat="server" Text="" Visible="false" Width="75px"></asp:TextBox>
                        <br /><br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="ButtonSave" runat="server" Text="Make Copy" class="Button2" Visible="false" OnClick="ButtonSave_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" class="Button2" Visible="false" OnClick="ButtonCancel_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="LabelAdminMessage" runat="server" Text=""></asp:Label>
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
    
    
    <asp:SqlDataSource ID="SqlDataSource_MasterMatter" runat="server" 
        ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
        SelectCommand="uspBMcBEARSearchMasterMatter" 
        SelectCommandType="StoredProcedure"
        OnSelecting="SqlDataSource_MasterMatter_EscapeSingleQuote">
     </asp:SqlDataSource>
     
    
    </form>
</body>
</html>
