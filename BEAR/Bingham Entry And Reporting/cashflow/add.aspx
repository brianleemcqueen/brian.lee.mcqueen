<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="add.aspx.cs" Inherits="BEAR.cashflow.add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Cash Flow Manager Add Manual Entry</title>
    <link href="../style/style.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/cashFlowManager.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/pageClose.js" type="text/javascript"></script>
    
    <script type="text/javascript">
    function ButtonReset_Click() {
        document.getElementById("TextBoxGlCode").value="";
        document.getElementById("TextBoxVendorID").value="";
        document.getElementById("TextBoxVendorName").value="";
        document.getElementById("TextBoxBarcode").value="";
        document.getElementById("TextBoxInvoiceNumber").value="";
        document.getElementById("TextBoxDate").value="";
        document.getElementById("TextBoxAmount").value="";
        document.getElementById("TextBoxDescription").value="";
        
        var tags = document.getElementsByTagName('input');
        
        for(i=0;i<tags.length;i++)
        {
            name = tags[i].name;
            if(name.indexOf('RadioButtonList') > -1)
            {                   
                document.getElementById(tags[i].id).checked = false;
            }
        }
    }    
    </script>
    
</head>
<body>
    <form id="form1" runat="server" defaultbutton="ButtonSave">
    
    <div id="title" class="title">
        <img alt="BINGHAM" src="../images/binghamLogoSmall.gif" />
        <img id="titlePic"  alt="Bill Tracker" src="../images/CashFlowManagerTitle.gif" class="titlePic" />
        <br />
         <img id="ImageAddTitle"  alt="AddManualRecord" src="../images/AddManualRecordSubTitle.gif" class="titlePic" />
    </div>
    
    <div id="options" class="options" runat="server" visible="true">
        <asp:Button ID="ButtonAdminOptions" runat="server" Text="Admin" Width="70" onclick="ButtonAdminOptionsClick" Height="20px" Font-Size="8"/>
        <asp:Button ID="ButtonFilter" runat="server" Text="Filter" Width="50" Height="20px" Font-Size="8" OnClick="ButtonFilter_Click"/>
        <input type="button" id="exitButton" name="exitButton" class="exitButton" runat="server" onclick="ExitApplication()" value="Exit"/>
    </div>

    
    <div id="data" class="data">
        <table>
            <tr>
                <td>
                    <asp:Label ID="LabelLocation" runat="server" Text="Location: " CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:RadioButtonList ID="RadioButtonListLocation" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="False" Text="1-US" Value="1" />
                        <asp:ListItem Selected="False" Text="2-UK" Value="2" />
                        <asp:ListItem Selected="False" Text="3-HK" Value="3" />
                        <asp:ListItem Selected="False" Text="7-HK" Value="7" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelBarcode" runat="server" Text="Barcode: " CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:TextBox ID="TextBoxBarcode" runat="server" width="70px"/><asp:ImageButton ID="ImageButtonSearch" runat="server" ImageUrl="../images/controls/search.gif" OnClick="ImageButtonSearch_Click"/>&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ImageButtonDelete" runat="server" ImageUrl="../images/controls/smallwhitedot.gif" OnClick="ImageButtonDelete_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelOffice" runat="server" Text="Office:" CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:DropDownList ID="DropDownListOffice" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelGlCode" runat="server" Text="GL Code: " CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:TextBox ID="TextBoxGlCode" runat="server" width="70px"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelVendorID" runat="server" Text="Vendor ID: " CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:TextBox ID="TextBoxVendorID" runat="server" width="70px"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelVendorName" runat="server" Text="Vendor Name: " CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:TextBox ID="TextBoxVendorName" runat="server" width="250px"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelInvoiceNumber" runat="server" Text="Invoice #: " CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:TextBox ID="TextBoxInvoiceNumber" runat="server" width="120px"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelInvoiceDate" runat="server" Text="Invoice Date: " CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:TextBox ID="TextBoxDate" runat="server" width="70px"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelAmount" runat="server" Text="Amount: " CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:TextBox ID="TextBoxAmount" runat="server" width="120px"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelCurrency" runat="server" Text="Currency: " CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:RadioButtonList ID="RadioButtonListCurrency" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="False" Text="USD" Value="USD" />
                        <asp:ListItem Selected="False" Text="GBP" Value="GBP" />
                        <asp:ListItem Selected="False" Text="HKD" Value="HKD" />
                    </asp:RadioButtonList>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LabelDescription" runat="server" Text="Description: " CssClass="parameterLabel" />
                </td>
                <td>
                    <asp:TextBox ID="TextBoxDescription" runat="server" width="250px"/>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="ButtonSave" runat="server" Text="Save" OnClick="ButtonSave_Click" Width="50px" />
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:Button ID="ButtonReset" runat="server" Text="Reset" OnClick="ButtonReset_Click" Width="50px" />
                            </td>
                        </tr>
                    </table>    
                </td>
            </tr>
        </table>    
    </div>
    <asp:Panel ID="PanelMessage" runat="server">
        <div id = "message" class="cashFlowManagerMessage">
            <asp:Label ID="LabelMessage" runat="server" Text="" />
        </div>
    </asp:Panel>
    
   <asp:Panel ID="PanelDelete" runat="server" Visible="false">
        <div id = "DivDelete" class="cashFlowManagerMessage">
            <asp:Label ID="LabelDelete" runat="server" Text="Delete this record?"></asp:Label>
            <br />
            <asp:Button ID="ButtonDelete" runat="server" Text="Delete" OnClick="ButtonDelete_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="ButtonCancelDelete" runat="server" Text="Cancel" OnClick="ButtonCancel_Click"/>
        </div>
    </asp:Panel>

    </form>
</body>
</html>
