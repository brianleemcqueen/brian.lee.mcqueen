<%@ Register TagPrefix="bearControl" TagName="upload" Src="~/controls/Upload.ascx" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BEAR.lockbox._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LockBox Processing</title>
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <link href="../style/uploadControl.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/general.js" type="text/javascript"></script>
    <script src="../scripts/pageClose.js" type="text/javascript"></script>
    
    <style type="text/css">
        .style2
        {
            width: 142px;
        }
        .style3
        {
            width: 195px;
        }
    </style>
    
</head>
<body>
    <form id="form1" enctype="multipart/form-data" runat="server">
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
        <div id="options" class="options">
            <asp:Button ID="ButtonImport" runat="server" cssClass="OptionButtonStandard" Text="Import" OnClick="ButtonImport_Click" />
            <asp:Button ID="ButtonExport" runat="server" cssClass="OptionButtonStandard" Text="Export" OnClick="ButtonExport_Click" />
            <asp:Button ID="ButtonEdit" runat="server" cssClass="OptionButtonStandard" Text="Edit" OnClick="ButtonEdit_Click" />
            <asp:Button ID="ButtonExit" runat="server" Text="Exit" CssClass="OptionButtonStandard" OnClientClick="ExitApplication()" /> 
        </div>
        
        <div id="picture" class="picture" runat="server">
            <bearControl:upload id="uploadControl" runat="server" Visible="false" />
            
            <div id="SelectGroup" runat="server" visible="false">
                <table id="DropDownEditTable" class="tablePictureWidth">
                    <tr>
                        <td class="style2">
                            <asp:Label ID="LabelGroup" runat="server" Text="Select Group To " /><asp:Label ID="LabelGroupType" runat="server" Text="" />
                        </td>
                        <td class="style3">
                            <asp:DropDownList ID="DropDownListEdit" 
                                              runat="server"
                                              />
                        </td>
                        <td align="center">
                            <asp:ImageButton ID="ImageButtonNext" 
                                             runat="server" 
                                             ImageUrl="~/images/controls/nextOrange.gif" 
                                             Height="20px" 
                                             Width="20px" 
                                             ToolTip="Next: Process the Selected Group"
                                             OnClick="ImageButtonEditNext_Click"
                                             />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:RadioButtonList ID="RadioButtonListTestOrFinal" runat="server" RepeatDirection="Horizontal" ToolTip="Select Test or Final Export" Visible="false">
                                <asp:ListItem Text="Test" Selected="True" Value="Test" />
                                <asp:ListItem Text="Final" Selected="False" Value="Final" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="LabelSelectGroupMessage" runat="server" Text=""></asp:Label>                            
                        </td>
                    </tr>
                </table>
            </div>
            <script type="text/javascript" language="JavaScript">
                loadPoster();
            </script>

        </div>
    </form>
</body>
</html>
