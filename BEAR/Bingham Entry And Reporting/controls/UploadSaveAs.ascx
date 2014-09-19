<%@ Control Language="C#" Description="ASP.NET file upload user control with Save As functionality" AutoEventWireup="true" CodeBehind="UploadSaveAs.ascx.cs" Inherits="BEAR.controls.UploadSaveAs" %>

<form id="Form1" enctype="multipart/form-data" runat="server">
    <table width="400" cellpadding="4" bgcolor="silver">
        <tr>
            <td valign="top" width="100">
                <asp:Label ID="LabelUp" runat="server" />
            </td>
            <td valign="top">
                <input id="filename" type="file" runat="server" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="LabelSave" runat="server" />
            </td>
            <td valign="top">
                <input id="savename" type="text" runat="server" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="LabelStatus" runat="server" />
            </td>
            <td valign="top">
                <asp:Label ID="LabelStatusMessage" runat="server" />
            </td>
        </tr>
        <tr>
            <td valign="top">
            </td>
            <td valign="top">
                <asp:Button ID="uploadButton" runat="server" OnClick="uploadButton_Click" />
            </td>
        </tr>
    </table>
</form>