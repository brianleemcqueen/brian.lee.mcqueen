<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminOptions.aspx.cs" Inherits="BEAR.cashflow.adminOptions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Cash Flow Manager Admin Options</title>
    <link href="../style/style.css" rel="stylesheet" type="text/css" media="all" />

</head>
<body>
    <form id="form1" runat="server">
    
    <div id="title" class="title">
        <img alt="BINGHAM" src="../images/binghamLogoSmall.gif" />
        <img id="titlePic"  alt="Cash Flow Manager" src="../images/CashFlowManagerTitle.gif" class="titlePic" />
        <br />
         <img id="ImageAdminTitle"  alt="Administrator Options" src="../images/AdminTitle.gif" class="titlePic" />
    </div>
    <div id="options" class="options" runat="server" visible="true">
        <asp:Button ID="ButtonAdd" runat="server" Text="Manual Entries" Height="20px" Font-Size="8" OnClick="ButtonAdd_Click"/>
        <asp:Button ID="ButtonFilter" runat="server" Text="Filter" Width="50" Height="20px" Font-Size="8" OnClick="ButtonFilter_Click"/>
        <asp:Button ID="ButtonClose" runat="server" Text="Close" Width="50" Height="20px" Font-Size="8" OnClick="ButtonClose_Click"/>
    </div>
    <div class="data">
        <asp:Label ID="LabelAmountToPay" runat="server" Text="Goal Amount To Pay: "></asp:Label>
        <asp:TextBox ID="TextBoxAmountToPay" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonSave" runat="server" Text="Save" Width="50" Height="20px" Font-Size="8" />
        <br />
        <asp:Label ID="LabelMessage" runat="server" Text="" CssClass="Arial14GreenBold"></asp:Label>
    </div>
    </form>
</body>
</html>
