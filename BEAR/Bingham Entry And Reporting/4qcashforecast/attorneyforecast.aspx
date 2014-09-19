<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="attorneyforecast.aspx.cs" Inherits="BEAR.attorneyforecast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>4th Quarter Attorney Cash Forecast</title>
    <link href="../style/global.css" rel="stylesheet" type="text/css" />
    <link href="../style/style.css" rel="stylesheet" type="text/css" media="all" />
    <link href="../style/lockedHeader.css" rel="stylesheet" type="text/css" />
        
    <script src="../scripts/div.js" type="text/javascript"></script>
    <script src="../scripts/grid.js" type="text/javascript"></script>
    <script src="../scripts/general.js" type="text/javascript"></script>
    <script src="../scripts/pageClose.js" type="text/javascript"></script>


    <script type="text/javascript">
        window.onload = CreateDataDivCookie;  //see div.js
      
        function   textbox_tkid_onFocus()  
        {
            document.getElementById("textbox_tkid").value="";
        }
        
        
        
        var oldgridSelectedColor;

        function setMouseOverColor(element, color)
        {
            oldgridSelectedColor = element.style.backgroundColor;
            element.style.cursor='hand';
            element.style.textDecoration='underline';
            
            if(color) 
            {
                element.style.backgroundColor='#EFC7A4';
            }
            
        }
        
        function setDetailMouseOverColor(element, color)
        {
            oldgridSelectedColor = element.style.backgroundColor;
            element.style.cursor='default';
            
            if(color) 
            {
                element.style.backgroundColor='#EFC7A4';
            }
            
        }

        function setMouseOutColor(element)
        {
            element.style.backgroundColor=oldgridSelectedColor;
            element.style.textDecoration='none';
        }
        
        
        
       
    </script>

</head>
<body onbeforeunload="closeIt()">

    <script src="../scripts/wz_tooltip.js" type="text/javascript"></script>
    
    <form id="form1" runat="server">
    
    <div id="title" class="title">
        <img 
            alt="BINGHAM" 
            src="../images/binghamLogoSmall.gif" 
            />
        <img 
            alt="4th Quarter Attorney Cash Forecast" 
            src="../images/4QCashForecastTitle.gif" 
            class="titlePic"
            id ="titlePic"
            />
        &nbsp;
    </div>
    
    <div id="asOfDate" class="asOfDate">
        <asp:GridView 
            ID="GridView4" 
            runat="server" 
            AutoGenerateColumns="False" 
            DataSourceID="SqlDataSource_Elite_WPReports_AttorneyInfo" 
            CellPadding="0" 
            ForeColor="#333333" 
            GridLines="None"
            Font-Names="Arial"
            Font-Size="10"
            ShowHeader="false"
            HeaderStyle-CssClass=""
            >
            <RowStyle 
                BackColor="White" 
                ForeColor="#EB8123"
                />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        As Of:&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField 
                    DataField="asOfDate" 
                    DataFormatString="{0:MMM dd, yyyy}"
                    HtmlEncode="false"
                    />
                <asp:TemplateField>
                    <ItemTemplate>
                        &nbsp;/&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField 
                    DataField="asOfDate" 
                    DataFormatString="{0:T}"
                    HtmlEncode="false"
                    />
            </Columns>
        </asp:GridView>
    </div>
    
    <div 
        id="login" 
        class="login"
        onclick="ResetDataDivPosition()">
       <asp:TextBox 
            ID="textbox_tkid" 
            runat="server"
            Text="Enter TKID"
            Visible="true" 
            Width="99px"
            />
        <asp:TextBox
            ID="InvisibleTextBoxToFixIEIssueWithOnlyOneTxtBox"
            runat="server"
            style="visibility:hidden;display:none;"
            />
        <asp:Button 
            ID="button_submitTkid" 
            runat="server" 
            Visible="true"
            Text="Submit" 
            Width="50"
            onclick="button_submitTkid_Click" 
            />
    </div>
    
    <div 
        id="options" 
        class="options"
        runat="server"
        visible="false">
        <asp:Button 
            ID="buttonSave" 
            runat="server" 
            Text="Update" 
            Width="50"
            Height="20px"
            Font-Size="9"
            />
        <asp:Button 
            ID="buttonPrint" 
            runat="server" 
            Text="Print" 
            Width="50"
            onclick="buttonPrint_Click" 
            Height="20px"
            Font-Size="9"
            />
        <asp:Button 
            ID="buttonReport" 
            runat="server" 
            Text="Report" 
            Width="50"
            onclick="buttonReport_Click" 
            Height="20px"
            Font-Size="9"
            />
        <asp:Button 
            ID="button_changeTkid" 
            runat="server" 
            Text="TKID" 
            Width="50"
            onclick="button_changeTkid_Click" 
            Height="20px"
            Font-Size="9"
            />
        <input  
            type="button" 
            id="exitButton" 
            name="exitButton"
            class="exitButton"
            runat="server" 
            onclick="ExitApplication()" 
            value="Exit"
            />
        <br />
      
    </div>
    
    <div 
        id="picture"
        class="picture"
        runat = "server" >
        <asp:Label 
            ID="LabelServerMessage" 
            CssClass="LabelServerMessage"
            runat="server"
            Visible="False" 
            Font-Bold="True" 
            Font-Names="Arial" 
            ForeColor="#EB8123"
            />
        <br /><br />
        <script type="text/javascript" language="JavaScript">
            loadPoster();
        </script>
    </div>

    <div 
        id="header"
        class="header">
        <asp:GridView 
            ID="GridView1" 
            runat="server" 
            AutoGenerateColumns="False" 
            DataSourceID="SqlDataSource_Elite_WPReports_AttorneyInfo" 
            CellPadding="1" 
            ForeColor="#333333" 
            GridLines="Vertical"
            Font-Names="Arial"
            Font-Size="Small"
            OnRowDataBound="GridView_HeaderDiv_RowBindEvent"
            HeaderStyle-Wrap="false"
            >
            <FooterStyle CssClass="GridViewFooter" />
            <RowStyle CssClass="GridViewRow" Wrap="false" />
            <Columns>
                <asp:BoundField 
                    DataField="Tkinit" 
                    HeaderText="TK #" 
                    SortExpression="Tkinit" 
                    ItemStyle-Width = "70"
                    ItemStyle-HorizontalAlign="Center"
                    />
                <asp:BoundField 
                    DataField="AttyName" 
                    HeaderText="Attorney Name" 
                    SortExpression="AttyName" 
                    ItemStyle-Width = "190"
                    ItemStyle-Wrap = "false"
                    ItemStyle-HorizontalAlign="Center"
                    />
                <asp:BoundField 
                    DataField="tktitle" 
                    HeaderText="Title" 
                    SortExpression="tktitle" 
                    ItemStyle-Width = "130"
                    ItemStyle-Wrap = "false"
                    ItemStyle-HorizontalAlign="Center"
                    />
                <asp:BoundField 
                    DataField="Location" 
                    HeaderText="Location" 
                    SortExpression="Location" 
                    ItemStyle-Width = "130"
                    ItemStyle-Wrap = "false"
                    ItemStyle-HorizontalAlign="Center"
                    />
                <asp:BoundField 
                    DataField="Head1" 
                    HeaderText="Practice Area" 
                    SortExpression="Head1" 
                    ItemStyle-Width = "130"
                    ItemStyle-Wrap = "false"
                    ItemStyle-HorizontalAlign="Center"
                   />
            </Columns>
            <PagerStyle CssClass="GridViewPager" ForeColor="#FFFFFF" />
            <SelectedRowStyle 
                BackColor="#E2DED6" 
                Font-Bold="True" 
                ForeColor="#333333" 
                />
            <HeaderStyle 
                BackColor="#2a76b2" 
                Font-Bold="True" 
                ForeColor="#FFFFFF" 
                />
            <EditRowStyle 
                BackColor="#999999" 
                />
            <AlternatingRowStyle 
                BackColor="White" 
                ForeColor="#284775" 
                />
        </asp:GridView>
        <asp:SqlDataSource 
            ID="SqlDataSource_Elite_WPReports_AttorneyInfo" 
            runat="server" 
            ConnectionString="<%$ ConnectionStrings:WPReportsConnectionString %>" 
            SelectCommand="SELECT DISTINCT [Tkinit], [AttyName], [tktitle], [Location], [Head1], MAX([asOfDate]) AS asOfDate
                           FROM [Forecast_Atty] 
                           WHERE ([Tkinit] = @Tkinit)
                           GROUP BY [Tkinit], [AttyName], [tktitle], [Location], [Head1]">
            <SelectParameters>
                <asp:QueryStringParameter 
                    Name="Tkinit" 
                    QueryStringField="tkid" 
                    Type="String" 
                    />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

    <div 
        id="data"
        onscroll="SetDataDivPosition()"        
        runat="server"
        class="data">

        <asp:GridView 
            ID="GridView2" 
            runat="server" 
            AutoGenerateColumns="False" 
            CellPadding="2" 
            DataSourceID="SqlDataSource_Elite_WPReports_AttorneyForecastData" 
            ForeColor="#333333" 
            GridLines="Both" 
            AllowSorting="True"
            Font-Names="Arial"
            Font-Size="Small"
            RowStyle-Height="20px"
            HeaderStyle-Height="20px"
            HeaderStyle-Wrap="false"
            ShowFooter="False"
            CssClass="linkNoUnderline"
            DataKeyNames="clnum"
            OnRowDataBound="GridView_DataDiv_RowBindEvent"
            OnSelectedIndexChanged="GridView_IndexChanged"
            OnSorting="Grid_Data_Sorting"
            >
            <EmptyDataRowStyle
                BackColor="#2a76b2" 
                Font-Bold="True" 
                ForeColor="#FFFFFF" 
                />
            <RowStyle CssClass="GridViewRow" />
            <HeaderStyle 
                BackColor="#2a76b2" 
                Font-Bold="True" 
                ForeColor="#FFFFFF" 
                HorizontalAlign="Right"   
                />
            <AlternatingRowStyle 
                BackColor="White" 
                ForeColor="#284775" 
                />
            <SelectedRowStyle 
                BackColor="#EB8123" 
                />
                
            <Columns>
			    <asp:CommandField 
				    ShowSelectButton="True" 
				    ButtonType="Image"
                    SelectImageUrl="~/images/controls/magnifier.png"
				    SelectText="Matter Details"
				    ItemStyle-HorizontalAlign="Center"
				    />


                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField 
                    DataField="clnum" 
                    HeaderText="<u>Client #</u>" 
                    HeaderStyle-VerticalAlign="Top"
                    SortExpression="clnum"
                    HtmlEncode="false"
                    />
                <asp:BoundField 
                    DataField="clname1" 
                    HeaderText="<span style='float:center'><u>Client Name</u></span><br /><span style='float:right'>Totals:</span>" 
                    SortExpression="clname1" 
                    ItemStyle-Wrap="false"
                    HtmlEncode="false"
                    />
                <asp:BoundField 
                    DataField="Net_Investment" 
                    HeaderText="Net Inv" 
                    SortExpression="Net_Investment" 
                    DataFormatString="{0:N0}"
                    ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Wrap="false"
                    ItemStyle-Width = "75"
                    HtmlEncode="false"
                    >
                </asp:BoundField>
                <asp:BoundField 
                    DataField="AR" 
                    HeaderText="AR" 
                    SortExpression="AR" 
                    DataFormatString="{0:N0}"
                    ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Wrap="false"
                    ItemStyle-Width = "75"
                    HtmlEncode="false"
                    >
                </asp:BoundField>
                <asp:BoundField 
                    DataField="WIP" 
                    HeaderText="WIP" 
                    SortExpression="WIP" 
                    DataFormatString="{0:N0}"
                    ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Wrap="false"
                    ItemStyle-Width = "75"
                    HtmlEncode="false"
                    >
                </asp:BoundField>
                <asp:BoundField 
                    DataField="CurrentCR" 
                    HeaderText="Receipts" 
                    SortExpression="CurrentCR" 
                    DataFormatString="{0:N0}"
                    ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Wrap="false"
                    ItemStyle-Width = "75"
                    HtmlEncode="false"
                    >
                </asp:BoundField>
                <asp:BoundField 
                    DataField="Variance" 
                    HeaderText="Variance" 
                    SortExpression="Variance" 
                    DataFormatString="{0:N0}"
                    ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Wrap="false"
                    ItemStyle-Width = "75"
                    HtmlEncode="false"
                    >
                </asp:BoundField>
               <asp:TemplateField
                    HeaderText="Forecast"
                    SortExpression="Forecast" 
                    ItemStyle-Wrap="false"
                    ItemStyle-HorizontalAlign="Right"
                    ControlStyle-Width="75"
                    >
                    <ItemTemplate>
                        <asp:TextBox
                            ID="forecastTB"
                            onChange="ChangeRow('ChangedRow')"
                            runat="server"
                            Text='<%# Bind("[Forecast]") %>'
                            OnTextChanged="TextBox_TextChanged"
                            BorderStyle="None"
                            CssClass="right ShadedDataEntryColumn"
                            >
                          </asp:TextBox>
                     </ItemTemplate>
                </asp:TemplateField>   

                <asp:TemplateField
                    HeaderText="<u>Notes</u>"
                    SortExpression="Notes" 
                    HeaderStyle-VerticalAlign="Top"
                    ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:TextBox
                            ID="NotesTB"
                            onChange="ChangeRow('ChangedRow')"
                            runat="server"
                            Text='<%# Bind("[Notes]") %>'
                            OnTextChanged="TextBox_TextChanged"
                            BorderStyle="None"
                            CssClass="ShadedDataEntryColumn"
                            >
                          </asp:TextBox>
                     </ItemTemplate>
                     <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>   

            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource_Elite_WPReports_AttorneyForecastData" 
            runat="server" 
            ConnectionString="<%$ ConnectionStrings:WPReportsConnectionString %>" 
            SelectCommand="[uspBMcBEARFourthQCashForecastSelect]"
            SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:QueryStringParameter 
                    Name="Tkinit" 
                    QueryStringField="tkid" 
                    Type="String" 
                    />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

    <div 
        id="details"
        class="details"
        runat="server">
                <asp:GridView 
                    ID="GridView3" 
                    runat="server" 
                    AutoGenerateColumns="False" 
                    CellPadding="0" 
                    DataSourceID="SqlDataSource_Elite_WPReports_AttnyForecastDetails" 
                    ForeColor="#333333" 
                    Font-Names="Arial"
                    Font-Size="Small"
                    GridLines="Both"
                    AllowSorting="True"
                    Visible="False" 
                    EmptyDataText="There are No Matter Details for this Line."
                    OnRowDataBound="GridView_DetailDiv_RowBindEvent"
                    ShowFooter="True"
                    HeaderStyle-Wrap="false"
                    >
                    <EmptyDataRowStyle
                        BackColor="#2a76b2" 
                        Font-Bold="True" 
                        ForeColor="#FFFFFF" 
                        />
                    <FooterStyle CssClass="GridViewFooter right" />
                    <RowStyle CssClass="GridViewRow" Height="15px"/>
                    <Columns>
                         <asp:BoundField 
                            DataField="mmatter" 
                            HeaderText="Matter #" 
                            SortExpression="mmatter" 
                            ItemStyle-Wrap="false"
                            ItemStyle-Width="90"
                            ItemStyle-HorizontalAlign="Center"
                            />
                        <asp:BoundField 
                            DataField="mdesc1" 
                            HeaderText="Matter" 
                            FooterText="Totals:"
                            SortExpression="mdesc1" 
                            ItemStyle-Wrap="false"
                            ItemStyle-Width="300px"
                            ItemStyle-HorizontalAlign="Left"
                            />
                        <asp:BoundField 
                            DataField="Net_Investment" 
                            HeaderText="Net Inv" 
                            SortExpression="Net_Investment" 
                            DataFormatString="{0:N0}"
                            ItemStyle-Wrap="false"
                            ItemStyle-Width="80"
                            ItemStyle-HorizontalAlign="Right"
                            />
                        <asp:BoundField 
                            DataField="AR" 
                            HeaderText="AR" 
                            SortExpression="AR" 
                            DataFormatString="{0:N0}"
                            ItemStyle-Wrap="false"
                            ItemStyle-Width="80"
                            ItemStyle-HorizontalAlign="Right"
                            />
                        <asp:BoundField 
                            DataField="WIP" 
                            HeaderText="WIP" 
                            SortExpression="WIP" 
                            DataFormatString="{0:N0}"
                            ItemStyle-Wrap="false"
                            ItemStyle-Width="80"
                            ItemStyle-HorizontalAlign="Right"
                            />
                        <asp:BoundField 
                            DataField="CurrentCR" 
                            HeaderText="Receipts" 
                            SortExpression="CurrentCR" 
                            DataFormatString="{0:N0}"
                            ItemStyle-Wrap="false"
                            ItemStyle-Width="80"
                            ItemStyle-HorizontalAlign="Right"
                            />
                    </Columns>
                    <HeaderStyle 
                            BackColor="#2a76b2" 
                            Font-Bold="True" 
                            ForeColor="#FFFFFF" 
                            />
                    <AlternatingRowStyle 
                            BackColor="White" 
                            ForeColor="#284775" 
                            />
                </asp:GridView>

                <asp:SqlDataSource 
                    ID="SqlDataSource_Elite_WPReports_AttnyForecastDetails" 
                    runat="server" 
                    ConnectionString="<%$ ConnectionStrings:WPReportsConnectionString %>" 
                    
                    SelectCommand="SELECT [mmatter], [mdesc1], isNull([Net_Investment],0) AS Net_Investment, isNull([AR],0) AS AR, isNull([WIP],0) AS WIP, isNull([CurrentCR],0) AS CurrentCR 
                                    FROM [Forecast_Atty_Detail]
                                    WHERE (([tkinit] = @tkinit) AND ([clnum] = @clnum))">
                    <SelectParameters>
                        <asp:QueryStringParameter 
                            Name="tkinit" 
                            QueryStringField="tkid" 
                            Type="String" 
                            />
                        <asp:Parameter 
                            Name="clnum" 
                            Type="String" 
                            />
                    </SelectParameters>
                </asp:SqlDataSource>
            
    </div>

    </form>
</body>
</html>
