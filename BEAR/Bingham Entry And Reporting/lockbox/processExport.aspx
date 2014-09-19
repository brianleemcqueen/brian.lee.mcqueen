<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="processExport.aspx.cs" Inherits="BEAR.lockbox.processExport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Lockbox: Processing Export File</title>
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/pageClose.js" type="text/javascript"></script>
    <script src="../scripts/general.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="title" class="title">
        <img 
            alt="BINGHAM" 
            src="../images/binghamLogoSmall.gif" 
            />
        <img 
            alt="Lockbox Processing" 
            src="../images/LockboxTitle.gif" 
            class="titlePic"
            id ="titlePic"
            />
    </div>
    
    
    <div id="picture" class="picture" runat="server">
        <script type="text/javascript" language="JavaScript">
            loadPoster();
        </script>

    </div>
        
    
    <div id="options" class="options" runat="server">
        <asp:Button ID="ButtonHome" runat="server" Text="Home" CssClass="OptionButtonStandard" OnClick="ButtonHome_Click" />
        <asp:Button ID="ButtonExit" runat="server" Text="Exit" CssClass="OptionButtonStandard" OnClientClick="ExitApplication()" /> 
        <br />
    </div>
    
    <div>
    <div id="messages" class="messages" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label ID="LabelProcessingMessage" runat="server" Text=""/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelProcessingOutput" runat="server" Text=""/>
                    <asp:ImageButton ID="ImageButtonOpenExportLocation" runat="server" ImageUrl="~/images/controls/Folder-Open-16x16.gif" Visible="false" onclick="ImageButtonOpenExportLocation_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelProcessingOutput2" runat="server" Text=""/>
                </td>
            </tr>


        </table>
        
        
        
    </div>
    </div>
    
    </form>
</body>
</html>
