<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="email.aspx.cs" Inherits="BEAR.exceptionRates.admin.email" ValidateRequest="false" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Exception Rate Admin Email Tool</title>
    <link href="../../style/style.css" rel="stylesheet" type="text/css" />
   
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0"></asp:ScriptManager>
    
    <div id="title" class="title" runat="server">
        <img id="binghamLogo" alt="BINGHAM" src="../../images/binghamLogoSmall.gif" />
        <img id="titlePic"  alt="Exception Rates" src="../../images/ExceptionRatesTitle.gif" class="titlePic" />

    </div>
    

    <div id="email" class="email">
        <div id="emailOptions" runat="server">
            <table border="0" cellpadding="2">
                <tr>
                    <td colspan="4" align="center">
                        <big><b>email Links to Attorneys</b></big>
                    </td>
                </tr>
                <tr>
                    <td>
                        Subject
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="TextBoxEmailSubject" 
                                     runat="server" 
                                     Width="500px"
                                     Text="2010 Exception Rate Application Link">
                        </asp:TextBox>
                    </td>
                </tr>
               <tr>
                    <td>
                        Body (before the Link)
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="TextBoxEmailBodyOpeningLine" runat="server" 
                            Width="500px" 
                            Height="65px" 
                            TextMode="MultiLine"
                            CssClass="Arial10Black"
                            Text=""
                         >
                         </asp:TextBox>
                           
                    </td>
                </tr>
               <tr>
                    <td>
                        Body (after the Link)
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="TextBoxEmailBody" runat="server" 
                            Width="500px" 
                            Height="194px" 
                            TextMode="MultiLine"
                            CssClass="Arial10Black"
                            Text=""
                         >
                         </asp:TextBox>
                           
                    </td>
                </tr>
                <tr>
                    <td>
                        Billing Attorney
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxBillingAttorney" runat="server" Text="All"></asp:TextBox>
                    </td>
   
                    <td rowspan="3">
                        Billing TK Office<br />
                    </td>
                    <td rowspan="3">
                        <asp:ListBox ID="listboxBillingTimekeeperOffice" 
                                    runat="server" 
                                    SelectionMode="Single" 
                                    CssClass="parameterControlWide">
                            <asp:ListItem Selected="True">All</asp:ListItem>
                        </asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Client
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxClient" runat="server" Text="All"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Billling Specialist
                    </td>
                    <td>
                        <asp:DropDownList   ID="dropDownListBillingSpecialist" runat="server"
                                        Font-Size="9pt">
                            <asp:ListItem Selected="True">All</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Exceptions
                    </td>
                    <td>
                        <asp:ListBox ID="TCB" runat="server" CssClass="parameterControl parameterControlThreeHigh">
                            <asp:ListItem Selected="True" Value="T" Text="Timekeeper" />
                            <asp:ListItem Selected="False" Value="C" Text="Costcode" />
                            <asp:ListItem Selected="False" Value="B" Text="Both" />
                        </asp:ListBox>
                    </td>
 
                    <td>
                        <asp:Label ID="labelAttorneyReviewed" runat="server" Text="Attorney Reviewed Filter:" />
                    </td>
                    <td>
                        <asp:ListBox ID="ListBoxAttorneyReviewed" runat="server" CssClass="parameterControl parameterControlThreeHigh">
                            <asp:ListItem Selected="True" Value="-1" Text="Show All" />
                            <asp:ListItem Selected="False" Value="1" Text="Reviewed" />
                            <asp:ListItem Selected="False" Value="0" Text="Not Reviewed" />
                        </asp:ListBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        Rates
                    </td>
                    <td>
                        <asp:ListBox ID="CMB" runat="server" CssClass="parameterControl parameterControlThreeHigh">
                            <asp:ListItem Selected="False" Value="C" Text="Client Rates" />
                            <asp:ListItem Selected="False" Value="M" Text="Matter Rates" />
                            <asp:ListItem Selected="True" Value="B" Text="Both" />
                        </asp:ListBox>
                    </td>

                    <td>
                        <asp:Label ID="labelBillingReviewed" runat="server" Text="Billing Reviewed Filter:" />
                    </td>
                    <td>
                        <asp:ListBox ID="ListBoxBillingReviewed" runat="server" CssClass="parameterControl parameterControlThreeHigh">
                            <asp:ListItem Selected="True" Value="-1" Text="Show All" />
                            <asp:ListItem Selected="False" Value="1" Text="Reviewed" />
                            <asp:ListItem Selected="False" Value="0" Text="Not Reviewed" />
                        </asp:ListBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        Year
                    </td>
                    <td>
                        <asp:ListBox ID="listboxCalendarYear" runat="server" CssClass="parameterControl parameterControlOneHigh">
                            <asp:ListItem Selected="False" Value="2009" Text="2009" Enabled="false" />
                            <asp:ListItem Selected="True" Value="2010" Text="2010" />
                        </asp:ListBox>
                    </td>
                    
                    <td>
                        <asp:Label ID="labelFinalized" runat="server" Text="Finalized Filter:" />
                    </td>
                    <td>
                        <asp:ListBox ID="ListBoxFinalized" runat="server" CssClass="parameterControl parameterControlThreeHigh">
                            <asp:ListItem Selected="True" Value="-1" Text="Show All" />
                            <asp:ListItem Selected="False" Value="1" Text="Reviewed" />
                            <asp:ListItem Selected="False" Value="0" Text="Not Reviewed" />
                        </asp:ListBox>
                    </td>
                
                
                </tr>
                
                <tr>
                    <td>
                        Attorney Udf
                    </td>
                    <td>
                        <asp:ListBox ID="ListBoxUdf" runat="server" CssClass="parameterControl parameterControlThreeHigh">
                            <asp:ListItem Selected="False" Value="-1" Text="All Attorneys" />
                            <asp:ListItem Selected="False" Value="Y" Text="McKee Only" />
                            <asp:ListItem Selected="False" Value="N" Text="Non-McKee" />
                        </asp:ListBox>
                    </td>
                    
                    <td colspan="2">
                        <asp:CheckBox ID="CheckBoxForceOfficeToAll" Text="Generate Email Link with 'All' for Office Parameter?" runat="server" checked="true"/>
                        <br />
                        <asp:CheckBox ID="CheckBoxSentToInBody" runat="server" Text="Add 'Sent To' Text in Body?" Checked="false"/>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="ButtonEmailClear" runat="server" Text="Clear Email" 
                            onclick="ButtonEmailClear_Click" />
                        <cc1:ConfirmButtonExtender ID="ButtonEmailClear_ConfirmButtonExtender" 
                            runat="server" ConfirmText="Clear email Form?" Enabled="True" 
                            TargetControlID="ButtonEmailClear">
                        </cc1:ConfirmButtonExtender>
                    </td>
                    <td colspan="2" align="center">
                        <asp:Button ID="ButtonEmailSubmit" runat="server" 
                            Text="Send Email to All Attorneys" onclick="ButtonEmailSubmit_Click" />
                        <cc1:ConfirmButtonExtender ID="ButtonEmailSubmit_ConfirmButtonExtender" 
                            runat="server" ConfirmText="SEND EMAIL TO ALL ATTORNEYS?" Enabled="True" 
                            TargetControlID="ButtonEmailSubmit">
                        </cc1:ConfirmButtonExtender>
                    </td>
                </tr>
            </table>
        </div>
        <div id="emailMessage" runat="server" Visible="false">
            <asp:Label ID="LabelMessage" runat="server" Font-Size="Large" Font-Bold="true" />
            <asp:Button ID="ButtonToggleMessage" runat="server" Text="Email Again" 
                onclick="ButtonToggleMessage_Click" />
        </div>
    </div>
    
    
    </form>
</body>
</html>

