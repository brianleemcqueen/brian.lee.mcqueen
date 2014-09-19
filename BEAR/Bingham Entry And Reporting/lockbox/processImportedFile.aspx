<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="processImportedFile.aspx.cs" Inherits="BEAR.lockbox.processImportedFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server">
    <title>Lockbox: Processing Imported File</title>
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <link href="../style/uploadControl.css" rel="stylesheet" type="text/css" />
    <link href="../style/lockedHeader.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/general.js" type="text/javascript"></script>
    <script src="../scripts/grid.js" type="text/javascript"></script>
    <script src="../scripts/div.js" type="text/javascript"></script>
    <script src="../scripts/pageClose.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        window.onload = CreateDataDivCookie;  //see div.js
    </script>
    
    
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
    
    <div id="options" class="options" runat="server">
        <asp:Button ID="ButtonSave" runat="server" Text="Update" CssClass="OptionButtonStandard"/>
        <asp:Button ID="ButtonHome" runat="server" Text="Home" CssClass="OptionButtonStandard" OnClick="ButtonHome_Click" />
        <asp:Button ID="ButtonExit" runat="server" Text="Exit" CssClass="OptionButtonStandard" OnClientClick="ExitApplication()" /> 
        <br />
    </div>
    
    <div id="messages" class="messages" runat="server">
        <asp:Label ID="LabelProcessingMessage" runat="server" Text=""></asp:Label>
        <asp:Label ID="LabelProcessingOutput" runat="server" Text=""></asp:Label>

    </div>
    
    <div id="data" runat="server" class="importData" onscroll="SetDataDivPosition()">
        <asp:GridView 
            ID="GridViewImportedData" 
            runat="server"
            AutoGenerateColumns="false"
            CellPadding="2" 
            ForeColor="#333333" 
            GridLines="Both" 
            RowStyle-Height="16px"
            HeaderStyle-Height="20px"
            HeaderStyle-Wrap="false"
            ShowFooter="false"
            OnRowDataBound="GridViewImportedData_RowDataBound"
            >
            <EmptyDataRowStyle
                BackColor="#2a76b2" 
                Font-Bold="true" 
                ForeColor="#FFFFFF" 
                />
            <RowStyle CssClass="GridViewRow" />
            <HeaderStyle 
                BackColor="#2a76b2" 
                Font-Bold="true" 
                ForeColor="#FFFFFF" 
                HorizontalAlign="Right"   
                />
            <AlternatingRowStyle 
                BackColor="White" 
                ForeColor="#284775" 
                />
            <SelectedRowStyle CssClass="GridViewSelectedRow"/>
            <Columns>
            
                <asp:BoundField 
                    DataField="ID" 
                    HeaderText="ID" 
                    SortExpression="ID" 
                    ItemStyle-Wrap="false"
                    ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width = "40"
                    HeaderStyle-HorizontalAlign="Center"
                    HeaderStyle-VerticalAlign="Bottom"
                    HtmlEncode="false"
                    />
                <asp:BoundField 
                    DataField="BatchID" 
                    HeaderText="Batch ID" 
                    SortExpression="BatchID" 
                    ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width = "50"
                    HeaderStyle-HorizontalAlign="Center"
                    HeaderStyle-VerticalAlign="Bottom"
                    HtmlEncode="false"
                    >
                </asp:BoundField>
                <asp:BoundField 
                    DataField="CheckSeq" 
                    HeaderText="Sequence" 
                    SortExpression="CheckSeq" 
                    ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width = "50"
                    HeaderStyle-HorizontalAlign="Center"
                    HeaderStyle-VerticalAlign="Bottom"
                    HtmlEncode="false"
                    >
                </asp:BoundField>
                <asp:TemplateField
                    HeaderText="Check Amt"
                    SortExpression="CheckAmt" 
                    HeaderStyle-VerticalAlign="Bottom"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="false"
                    ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width = "75"
                    ControlStyle-CssClass="GridViewTemplateFieldDataEntry right"
                    >
                    <ItemTemplate>
                        <asp:TextBox
                            ID="TextBoxCheckAmt"
                            onChange="ChangeRow('ChangedRow')"
                            runat="server"
                            Text='<%# Eval("[CheckAmt]", "{0:C2}") %>'
                            OnTextChanged="TextBoxTextChanged"
                            BorderStyle="None"
                            Width="75"
                            >
                          </asp:TextBox>
                     </ItemTemplate>
                     <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>   

                <asp:TemplateField
                    HeaderText="Doc #"
                    SortExpression="DocNo" 
                    HeaderStyle-VerticalAlign="Bottom"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="false"
                    ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width = "55"
                    ControlStyle-CssClass="GridViewTemplateFieldDataEntry right"
                    >
                    <ItemTemplate>
                        <asp:TextBox
                            ID="TextBoxDocNo"
                            onChange="ChangeRow('ChangedRow')"
                            runat="server"
                            Text='<%# Bind("[DocNo]") %>'
                            OnTextChanged="TextBoxTextChanged"
                            BorderStyle="None"
                            Width="55"
                            >
                          </asp:TextBox>
                     </ItemTemplate>
                     <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField> 
                <asp:TemplateField
                    HeaderText="Invoice #"
                    SortExpression="InvoiceNo" 
                    HeaderStyle-VerticalAlign="Bottom"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="false"
                    ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width = "55"
                    ControlStyle-CssClass="GridViewTemplateFieldDataEntry center"
                    >
                    <ItemTemplate>
                        <asp:TextBox
                            ID="TextBoxInvoiceNo"
                            onChange="ChangeRow('ChangedRow')"
                            runat="server"
                            Text='<%# Bind("[InvoiceNo]") %>'
                            OnTextChanged="TextBoxTextChanged"
                            BorderStyle="None"
                            Width="55"
                            >
                          </asp:TextBox>
                     </ItemTemplate>
                     <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField> 
                <asp:TemplateField
                    HeaderText="Invoice Amt"
                    SortExpression="InvoiceAmount" 
                    HeaderStyle-VerticalAlign="Bottom"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="false"
                    ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width = "75"
                    ControlStyle-CssClass="GridViewTemplateFieldDataEntry right"
                    >
                    <ItemTemplate>
                        <asp:TextBox
                            ID="TextBoxInvoiceAmt"
                            onChange="ChangeRow('ChangedRow')"
                            runat="server"
                            Text='<%# Eval("[InvoiceAmount]", "{0:C2}") %>'
                            OnTextChanged="TextBoxTextChanged"
                            BorderStyle="None"
                            Width="75"
                            >
                          </asp:TextBox>
                     </ItemTemplate>
                     <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>   
                <asp:TemplateField
                    HeaderText="Invoice Description"
                    SortExpression="InvoiceDesc" 
                    HeaderStyle-VerticalAlign="Bottom"
                    HeaderStyle-HorizontalAlign="Left"
                    ItemStyle-Wrap="false"
                    ItemStyle-HorizontalAlign="Center"
                    ControlStyle-CssClass="GridViewTemplateFieldDataEntry"
                    ItemStyle-Width="170"
                    >
                    <ItemTemplate>
                        <asp:TextBox
                            ID="TextBoxInvoiceDesc"
                            onChange="ChangeRow('ChangedRow')"
                            runat="server"
                            Text='<%# Bind("[InvoiceDesc]") %>'
                            OnTextChanged="TextBoxTextChanged"
                            BorderStyle="None"
                            Width="170"
                            >
                          </asp:TextBox>
                     </ItemTemplate>
                     <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>   
                <asp:TemplateField
                    HeaderText="Notes"
                    SortExpression="Notes" 
                    HeaderStyle-VerticalAlign="Bottom"
                    ItemStyle-Wrap="false"
                    ItemStyle-Width="170"
                    ItemStyle-HorizontalAlign="Center"
                    ControlStyle-CssClass="GridViewTemplateFieldDataEntry"
                    >
                    <ItemTemplate>
                        <asp:TextBox
                            ID="TextBoxNotes"
                            onChange="ChangeRow('ChangedRow')"
                            runat="server"
                            Text='<%# Bind("[Notes]") %>'
                            OnTextChanged="TextBoxTextChanged"
                            BorderStyle="None"
                            Width="170"
                            >
                          </asp:TextBox>
                     </ItemTemplate>
                     <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>   

                
            
            </Columns>
            
        </asp:GridView>
    </div>

    </form>
</body>
</html>
