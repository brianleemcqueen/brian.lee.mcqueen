<%@ Control Language="C#" Description="ASP.NET file upload user control" AutoEventWireup="true" CodeBehind="Upload.ascx.cs" Inherits="BEAR.controls.Upload" %>

    <table id="uploadTable">
        <tr>
            <td valign="top" width="100">
                <asp:Label ID="LabelUp" runat="server" />
            </td>
            <td valign="top" colspan="2">
                <input id="fileName" type="file" runat="server" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="LabelStatus" runat="server" Visible="false" />
            </td>
            <td valign="top" colspan="2">
                <asp:Label ID="LabelStatusMessage" runat="server" Visible="false" />
            </td>
        </tr>
        <tr>
            <td valign="top">
            </td>
            <td valign="top" width="33%">
                <asp:Button ID="ButtonUpload" CssClass="ButtonStandard" runat="server" OnClick="ButtonUpload_Click" />
                <asp:Button ID="ButtonNext" CssClass="ButtonStandard" runat="server" Text="Next" OnClick="ButtonNext_Click" Visible="false" />
            </td>
            <td valign="top">
                <asp:Button ID="ButtonCancel" CssClass="ButtonStandard" runat="server" Text="Cancel" OnClick="ButtonCancel_Click" Visible="false" />
            </td>
        </tr>
    </table>
