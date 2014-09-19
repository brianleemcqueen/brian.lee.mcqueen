<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="BEAR.exceptionRates.admin.admin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ListBox ID="ListBoxEmailBody" runat="server" Height="100px">
            <asp:ListItem Text="Default Initial email" Value="Default" Enabled="false"></asp:ListItem>
            <asp:ListItem Text="Alternate Initial email" Value="Alternate" Enabled="false"></asp:ListItem>
            <asp:ListItem Text="Default REMINDER email" Value="DefaultRemind"></asp:ListItem>
            <asp:ListItem Text="Alternate REMINDER email" Value="AlternateRemind"></asp:ListItem>
            <asp:ListItem Text="Blank Body" Value="Blank" Selected="True"></asp:ListItem>
        </asp:ListBox>
        <br/>        
        <asp:Button ID="ButtonEmail" runat="server" Text="Email Attorneys" OnClick="LaunchEmail"/>
    </div>
    </form>
</body>
</html>
