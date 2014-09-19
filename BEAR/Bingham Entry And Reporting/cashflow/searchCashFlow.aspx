<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="searchCashFlow.aspx.cs" Inherits="BEAR.cashflow.searchCashFlow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Cash Flow Manager Advanced Search</title>
    <link href="../style/style.css" rel="stylesheet" type="text/css" media="all" />
    <script src="../scripts/pageClose.js" type="text/javascript"></script>
    <script src="../scripts/general.js" type="text/javascript"></script>

    <style type="text/css">
        .paramTitle 
        {
        	font-weight:bold;
        	text-decoration:underline;
        }
    </style>


</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>
        <div id="title" class="title">
            <img alt="BINGHAM" src="../images/binghamLogoSmall.gif" />
            <img id="titlePic"  alt="Cash Flow Manager" src="../images/CashFlowManagerTitle.gif" class="titlePic" />
            <br />
             <img id="ImageAdminTitle"  alt="Administrator Options" src="../images/advSearchTitle.gif" class="titlePic" />
        </div>
        <div id="options" class="options" runat="server" visible="true">
            <asp:Button ID="ButtonRun" runat="server" Text="Run" Width="50" Height="20px" Font-Size="8" OnClick="ButtonRun_Click" />
            <asp:Button ID="ButtonClose" runat="server" Text="Close" Width="50" Height="20px" Font-Size="8" OnClick="ButtonClose_Click"/>
            <asp:Button ID="ButtonReport" runat="server" Text="Report" Width="50" Height="20px" Font-Size="8" OnClick="buttonReportClick"/>
            <input type="button" id="exitButton" name="exitButton" class="exitButton" runat="server" onclick="ExitApplication()" value="Exit"/>

        </div>
        
        <br /><br /><br />
        <table cellpadding="2">
            <tr>
                <td>
                    <asp:Label ID="LabelInvoiceNumber" runat="server" Text="Invoice Number:" CssClass="paramTitle"></asp:Label><br />
                    <asp:TextBox ID="TextBoxInvoiceNumber" runat="server" Text="All"></asp:TextBox>
                    <br />
                    <asp:Label ID="LabelBarcode" runat="server" Text="Barcode:" CssClass="paramTitle"></asp:Label><br />
                    <asp:TextBox ID="TextBoxBarcode" runat="server" Text="All"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="LabelVendorId" runat="server" Text="Vendor ID:" CssClass="paramTitle"></asp:Label><br />
                    <asp:TextBox ID="TextBoxVendorID" runat="server" Text="All"></asp:TextBox>
                    <br />
                    <asp:Label ID="LabelVendorName" runat="server" Text="Vendor Name:" CssClass="paramTitle"></asp:Label><br />
                    <asp:TextBox ID="TextBoxVendorName" runat="server" Text="All"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="LabelLocation" runat="server" Text="Location: " CssClass="paramTitle" visible="false"></asp:Label>
                    <br />
                    <asp:RadioButtonList ID="RadioButtonListLocation" runat="server" RepeatDirection="Horizontal" Visible = "false"> 
                        <asp:ListItem Selected="True" Text="My Default Location" Value="Def" />
                        <asp:ListItem Selected="False" Text="All" Value="All" />
                        <asp:ListItem Selected="False" Text="US" Value="1" />
                        <asp:ListItem Selected="False" Text="UK / HK" Value="2" />
                        <asp:ListItem Selected="False" Text="UK" Value="UK" />
                        <asp:ListItem Selected="False" Text="HK" Value="HK" />
                    </asp:RadioButtonList>
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="LabelDeptPriority" runat="server" Text="Dept Priority: " CssClass="paramTitle" ></asp:Label>
                    <br />
                    <asp:RadioButtonList ID="RadioButtonListDeptPriority" runat="server" RepeatDirection="Horizontal" >
                        <asp:ListItem Selected="True" Text="All" Value="All" />
                        <asp:ListItem Selected="False" Text="1" Value="1" />
                        <asp:ListItem Selected="False" Text="2" Value="2" />
<%--                        
                        <asp:ListItem Selected="False" Text="3" Value="3" />
                        <asp:ListItem Selected="False" Text="4" Value="4" />
                        <asp:ListItem Selected="False" Text="1-3" Value="1-3" />
--%>                        
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Label ID="LabelCMPriority" runat="server" Text="CM Priority: "  CssClass="paramTitle" />
                    <br />
                    <asp:RadioButtonList ID="RadioButtonListCMPriority" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Text="All" Value="All" />
                        <asp:ListItem Selected="False" Text="1" Value="1" />
                        <asp:ListItem Selected="False" Text="2" Value="2" />
<%--                        
                        <asp:ListItem Selected="False" Text="3" Value="3" />
                        <asp:ListItem Selected="False" Text="4" Value="4" />
                        <asp:ListItem Selected="False" Text="1-3" Value="1-3" />
--%>                        
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Label ID="LabelCurrency" runat="server" Text="Currency: "  CssClass="paramTitle" />
                    <br />
                    <asp:RadioButtonList ID="RadioButtonListCurrency" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Text="All" Value="All" />
                        <asp:ListItem Selected="False" Text="USD" Value="USD" />
                        <asp:ListItem Selected="False" Text="GBP" Value="GBP" />
                        <asp:ListItem Selected="False" Text="HKD" Value="HKD" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            
            <tr>
                <td>
                    <asp:Label ID="LabelSource" runat="server" Text="Source: "  CssClass="paramTitle" />
                    <br />
                    <asp:RadioButtonList ID="RadioButtonListSource" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Text="All" Value="All" />
                        <asp:ListItem Selected="False" Text="Elite" Value="ELITE" />
                        <asp:ListItem Selected="False" Text="EPS" Value="EPS" />
                        <asp:ListItem Selected="False" Text="CAS" Value="CAS" />
                        <asp:ListItem Selected="False" Text="Manual" Value="MANL" />
                    </asp:RadioButtonList>
                </td>
                <td>
                
                </td>
                <td>
                    <asp:Label ID="LabelOffice" runat="server" Text="Office: "  CssClass="paramTitle" />
                    <br />
                    <asp:DropDownList ID="DropDownListOffice" runat="server"></asp:DropDownList>
                </td>
            </tr>
            
            <tr>
                <td>
                    <asp:Label ID="LabelPayFlag" runat="server" Text="Pay Flag: " CssClass="paramTitle" ></asp:Label>
                    <br />
                    <asp:RadioButtonList ID="RadioButtonListPayFlag" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Text="All" Value="All" />
                        <asp:ListItem Selected="False" Text="Marked to Pay" Value="1" />
                        <asp:ListItem Selected="False" Text="Not Marked To Pay" Value="0" />
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Label ID="LabelPaymentMethod" runat="server" Text="Payment Method: " CssClass="paramTitle" ></asp:Label>
                    <br />
                    <asp:RadioButtonList ID="RadioButtonListPaymentMethod" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Text="All" Value="All" />
                        <asp:ListItem Selected="False" Text="Bank" Value="Bank" />
                        <asp:ListItem Selected="False" Text="PC-CY" Value="PC-CY" />
                        <asp:ListItem Selected="False" Text="PC-FY" Value="PC-FY" />
                        <asp:ListItem Selected="False" Text="PC*" Value="PC*" />
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>    
        <br />
        <asp:Label ID="LabelDepartments" runat="server" Text="Department(s): " CssClass="paramTitle" ></asp:Label><asp:CheckBox ID="CheckBoxDepartmentsAll" AutoPostBack="true" Text="All" runat="server" checked="True" OnCheckedChanged="CheckBoxDepartmentsAll_CheckChanged" />
        <br />
        <asp:UpdatePanel ID="UpdatePanelDepartments" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelDepartments" runat="server" Visible="false">
                <asp:CheckBoxList ID="CheckBoxListDepartments" runat="server" RepeatColumns="4" RepeatLayout="Table" RepeatDirection="Vertical" CellPadding="2" ></asp:CheckBoxList>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="CheckBoxDepartmentsAll" />
        </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
