<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tokyoclientmatter.aspx.cs" Inherits="BEAR.search.tokyoclientmatter" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Tokyo Client / Matter Search</title>
    <link href="../style/global.css" rel="stylesheet" type="text/css" />
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <link href="../style/search.css" rel="stylesheet" type="text/css" />
    <link href="../style/progress.css" rel="stylesheet" type="text/css" />

    <script src="../scripts/resizeColumns.js" type="text/javascript"></script>
    <script src="../scripts/general.js" type="text/javascript"></script>
    
    <script language="javascript" type="text/javascript">
        var oldgridSelectedColor;

        function setMouseOverColor(element)
        {
            oldgridSelectedColor = element.style.backgroundColor;
            element.style.backgroundColor='#EB8123';
            element.style.cursor='hand';
            element.style.textDecoration='underline';
        }

        function setMouseOutColor(element)
        {
            element.style.backgroundColor=oldgridSelectedColor;
            element.style.textDecoration='none';
        }
        
    </script>

    
    
</head>
<body>

    <script src="../scripts/wz_tooltip.js" type="text/javascript"></script>
        
    <form id="form1" runat="server" defaultbutton="ImageButtonSearch">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="0">
    </asp:ScriptManager>
    <input id="Hidden0" runat="server" type="hidden" value="20px" />
    <input id="Hidden1" runat="server" type="hidden" value="30px" />
    <input id="Hidden2" runat="server" type="hidden" value="30px" />
    <input id="Hidden3" runat="server" type="hidden" value="145px" />
    <input id="Hidden4" runat="server" type="hidden" value="33px" />
    <input id="Hidden5" runat="server" type="hidden" value="33px" />
    <input id="Hidden6" runat="server" type="hidden" value="145px" />
    <input id="Hidden7" runat="server" type="hidden" value="50px" />
    <input id="Hidden8" runat="server" type="hidden" value="145px" />
    <input id="Hidden9" runat="server" type="hidden" value="33px" />
    <input id="Hidden10" runat="server" type="hidden" value="33px" />
    <input id="Hidden11" runat="server" type="hidden" value="145px" />
    <input id="Hidden12" runat="server" type="hidden" value="20px" />
    <input id="Hidden13" runat="server" type="hidden" value="25px" />
    <input id="Hidden14" runat="server" type="hidden" value="34px" />
    <input id="Hidden15" runat="server" type="hidden" value="40px" />
        

    <script type="text/javascript">

        function resetColumnWidths() {

            document.getElementById('Hidden0').value = "20px";
            document.getElementById('Hidden1').value = "30px";
            document.getElementById('Hidden2').value = "30px";
            document.getElementById('Hidden3').value = "150px";
            document.getElementById('Hidden4').value = "33px";
            document.getElementById('Hidden5').value = "33px";
            document.getElementById('Hidden6').value = "150px";
            document.getElementById('Hidden7').value = "50px";
            document.getElementById('Hidden8').value = "150px";
            document.getElementById('Hidden9').value = "33px";
            document.getElementById('Hidden10').value = "33px";
            document.getElementById('Hidden11').value = "150px";
            document.getElementById('Hidden12').value = "20px";
            document.getElementById('Hidden13').value = "25px";
            document.getElementById('Hidden14').value = "34px";
            document.getElementById('Hidden15').value = "40px";

        }

        //  true when a header is currently being resized
        var _isResizing;
        //  a reference to the header column that is being resized
        var _element;
        //  an array of all of the tables header cells
        var _ths;

        function pageLoad(args){
            //  get all of the th elements from the gridview
            _ths = $get('searchResults').getElementsByTagName('TH');
                    
            //  if the grid has at least one th element
            if(_ths.length > 1){
            
                for(i = 0; i < _ths.length; i++){
                    //  determine the widths
                    _ths[i].style.width = Sys.UI.DomElement.getBounds(_ths[i]).width + 'px';
                    
                    //  attach the mousemove and mousedown events
                    if(i < _ths.length - 1){
                        $addHandler(_ths[i], 'mousemove', _onMouseMove);
                        $addHandler(_ths[i], 'mousedown', _onMouseDown);
                    }
                }

                //  add a global mouseup handler            
                $addHandler(document, 'mouseup', _onMouseUp);
                //  add a global selectstart handler
                $addHandler(document, 'selectstart', _onSelectStart);
            }       
        }
        

        function _onMouseMove(args){    
            if(_isResizing){
                
                //  determine the new width of the header
                var bounds = Sys.UI.DomElement.getBounds(_element); 
                var width = args.clientX - bounds.x;
             
                //  we set the minimum width to 1 px, so make
                //  sure it is at least this before bothering to
                //  calculate the new width
                if(width > 1){
                
                    //  get the next th element so we can adjust its size as well
                    var nextColumn = _element.nextSibling;
                    var nextColumnWidth;
                    if(width < _toNumber(_element.style.width)){
                        //  make the next column bigger
                        nextColumnWidth = _toNumber(nextColumn.style.width) + _toNumber(_element.style.width) - width;
                    }
                    else if(width > _toNumber(_element.style.width)){
                        //  make the next column smaller
                        nextColumnWidth = _toNumber(nextColumn.style.width) - (width - _toNumber(_element.style.width));
                    }   
                    
                    //  we also don't want to shrink this width to less than one pixel,
                    //  so make sure of this before resizing ...
                    if(nextColumnWidth > 1){
                        _element.style.width = width + 'px';
                        nextColumn.style.width = nextColumnWidth + 'px';
                    }
                }
            }   
            else{
                //  get the bounds of the element.  If the mouse cursor is within
                //  2px of the border, display the e-cursor -> cursor:e-resize
                var bounds = Sys.UI.DomElement.getBounds(args.target);
                if(Math.abs((bounds.x + bounds.width) - (args.clientX)) <= 2) {
                    args.target.style.cursor = 'e-resize';
                }  
                else{
                    args.target.style.cursor = '';
                }          
            }         
        }

        function _onMouseDown(args){
            //  if the user clicks the mouse button while
            //  the cursor is in the resize position, it means
            //  they want to start resizing.  Set _isResizing to true
            //  and grab the th element that is being resized
            if(args.target.style.cursor == 'e-resize') {
                _isResizing = true;
                _element = args.target;               
            }                    
        } 

        function _onMouseUp(args){
            //  the user let go of the mouse - so
            //  they are done resizing the header.  Reset
            //  everything back
            if(_isResizing){
                
                //  set back to default values
                _isResizing = false;
                _element = null;
                
                //  make sure the cursor is set back to default
                for(i = 0; i < _ths.length; i++){   
                    _ths[i].style.cursor = '';
                }
                document.getElementById('Hidden0').value = (_toNumber(_ths[0].style.width) - 8) + 'px';
                document.getElementById('Hidden1').value = (_toNumber(_ths[1].style.width) - 9) + 'px';
                document.getElementById('Hidden2').value = (_toNumber(_ths[2].style.width) - 9) + 'px';
                document.getElementById('Hidden3').value = (_toNumber(_ths[3].style.width) - 9) + 'px';
                document.getElementById('Hidden4').value = (_toNumber(_ths[4].style.width) - 9) + 'px';
                document.getElementById('Hidden5').value = (_toNumber(_ths[5].style.width) - 9) + 'px';
                document.getElementById('Hidden6').value = (_toNumber(_ths[6].style.width) - 9) + 'px';
                document.getElementById('Hidden7').value = (_toNumber(_ths[7].style.width) - 9) + 'px';
                document.getElementById('Hidden8').value = (_toNumber(_ths[8].style.width) - 9) + 'px';
                document.getElementById('Hidden9').value = (_toNumber(_ths[9].style.width) - 9) + 'px';
                document.getElementById('Hidden10').value = (_toNumber(_ths[10].style.width) - 9) + 'px';
                document.getElementById('Hidden11').value = (_toNumber(_ths[11].style.width) - 9) + 'px';
                document.getElementById('Hidden12').value = (_toNumber(_ths[12].style.width) - 9) + 'px';
                document.getElementById('Hidden13').value = (_toNumber(_ths[13].style.width) - 9) + 'px';
                document.getElementById('Hidden14').value = (_toNumber(_ths[14].style.width) - 9) + 'px';
                document.getElementById('Hidden15').value = (_toNumber(_ths[15].style.width) - 9) + 'px';
                
            }
        } 

        function _onSelectStart(args){
            // Don't allow selection during drag
            if(_isResizing){
                args.preventDefault();
                return false;
            }
        }    

        function _toNumber(m) {
            //  helper function to peel the px off of the widths
            return new Number(m.replace('px', ''));
        }

    </script>

    <script type="text/javascript">
        function onUpdating(){
            // get the update progress div
            var pnlPopup = $get('<%= this.pnlPopup.ClientID %>'); 

            //  get the gridview element        
            var gridView = $get('<%= this.search.ClientID %>');
            
            // get the bounds of both the gridview and the progress div
            var gridViewBounds = Sys.UI.DomElement.getBounds(gridView);
            var pnlPopupBounds = Sys.UI.DomElement.getBounds(pnlPopup);
            
            //  center of gridview
            var x = Math.round(document.body.offsetWidth / 2) - (110); //Math.round(pnlPopupBounds.width / 2); //hardcoded 110 to match the width of the activity bar.  pnlpopupbounds was not working.
            var y = Math.round(gridViewBounds.height / 2) - Math.round(pnlPopupBounds.height / 2);	    

            //document.title = 'x = ' + x + '; offsetWidth = ' + document.body.offsetWidth + '; pnlPopupBounds.width = ' + pnlPopupBounds.width;
            //	set the progress element to this position
            Sys.UI.DomElement.setLocation(pnlPopup, x, y);           
            
            // make it visible
            pnlPopup.style.display = '';
            
        }

        function onUpdated() {
            // get the update progress div
            var pnlPopup = $get('<%= this.pnlPopup.ClientID %>'); 
            // make it invisible
            pnlPopup.style.display = 'none';
        } 
        
        function ToggleOffices() {
            var checkBox = document.getElementById('CheckBoxAllOffices');
            if(checkBox.checked) {
                document.getElementById('TextBoxOffices').disabled = true;
                document.getElementById('TextBoxOffices').style.background='#C8C8C8';
                document.getElementById('RadioButtonListOffices').style.display='inline';
                document.getElementById('RadioButtonListOffices').style.visibility='hidden';
                document.getElementById('OfficeListing').style.overflow='hidden';
            } else {
                document.getElementById('TextBoxOffices').disabled = false;
                document.getElementById('TextBoxOffices').style.background='White';
                document.getElementById('RadioButtonListOffices').style.display='inline';
                document.getElementById('RadioButtonListOffices').style.visibility='visible';
                document.getElementById('OfficeListing').style.overflow='auto';
            }
        }
        
            
    </script>

    
    <div id="title" class="title">
        <img 
            alt="BINGHAM" 
            src="../images/binghamLogoSmall.gif" 
            />
        <img 
            alt="Tokyo Client Matter Search" 
            src="../images/TokyoClientMatterSearchTitle.gif" 
            class="titlePic"
            id ="titlePic"
            />
    </div>
    <asp:Panel ID="SearchSelectedPanel" runat="server" DefaultButton="LinkButtonUpdate" >
    
        <div id="searchSelectedItems" runat="server" class="searchSelected">
            <asp:UpdatePanel ID="UpdatePanelSelectedItem" runat="server">
                <ContentTemplate>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr><td colspan="2">
                            <b><asp:Label ID="LabelSelectedMessage" runat="server" Text="" ></asp:Label></b>
                            <asp:ImageButton ID="ImageButtonUpdate" runat="server" OnClick="LinkButtonUpdate_Click" ToolTip="Save Japanese Client Name and Matter Description" ImageUrl="~/images/controls/smallwhitedot.gif" />
                            <asp:LinkButton ID="LinkButtonUpdate" runat="server" OnClick="LinkButtonUpdate_Click" Text = "" ToolTip="Save Japanese Client Name and Matter Description"></asp:LinkButton>
                            <br />
                         </td></tr><tr><td nowrap="nowrap" width="25%">
                            <asp:Label ID="LabelSelectedClientNumberLabel" runat="server" Text=""></asp:Label><br />
                            </td><td>
                            <asp:Label ID="LabelSelectedClientNumber" runat="server" Text=""></asp:Label><br />
                        </td></tr><tr><td nowrap="nowrap">
                            <asp:Label ID="LabelSelectedNTIClientNumber" runat="server" Text=""></asp:Label><br />
                        </td></tr><tr><td nowrap="nowrap">
                            <asp:Label ID="LabelSelectedBSMClientNumber" runat="server" Text=""></asp:Label><br />
                        </td></tr><tr><td nowrap="nowrap" valign="top">
                            <asp:Label ID="LabelSelectedClientNameLabel" runat="server" Text=""></asp:Label><br />
                            </td><td>
                            <asp:Label ID="LabelSelectedClientName" runat="server" Text=""></asp:Label><br />
                        </td></tr><tr><td nowrap="nowrap" valign="top">
                            <asp:Label ID="LabelSelectedJapaneseClientNameLabel" runat="server" Text=""></asp:Label>
                            </td><td>
                            <asp:TextBox ID="TextBoxSelectedJapaneseClientName" runat="server" Text="" ToolTip="Japanese Client Name" BorderStyle="None" CssClass="TextBoxEditTokyo" TabIndex="-1"></asp:TextBox>
                            <br />
                        </td></tr>
                        <tr>
                            <td nowrap="nowrap" valign="top">
                                <asp:Label ID="LabelOverseasMatter" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBoxOverseasMatter" runat="server" style="display:none" Text="" />
                            </td>
                        </tr>
                        <tr><td nowrap="nowrap">
                            <asp:Label ID="LabelSelectedMatterNumberLabel" runat="server" Text=""></asp:Label><br />
                            </td><td>
                            <asp:Label ID="LabelSelectedMatterNumber" runat="server" Text=""></asp:Label><br />
                        </td></tr><tr><td nowrap="nowrap">
                            <asp:Label ID="LabelSelectedNTIMatterNumber" runat="server" Text=""></asp:Label><br />
                        </td></tr><tr><td nowrap="nowrap">
                            <asp:Label ID="LabelSelectedBSMMatterNumber" runat="server" Text=""></asp:Label><br />
                       </td></tr><tr><td nowrap="nowrap" valign="top">
                            <asp:Label ID="LabelSelectedMatterDescription" runat="server" Text=""></asp:Label><br />
                        </td></tr><tr><td nowrap="nowrap" valign="top">
                            <asp:Label ID="LabelSelectedJapaneseMatterDescriptionLabel" runat="server" Text=""></asp:Label>
                            </td><td>
                            <asp:TextBox ID="TextBoxSelectedJapaneseMatterDescription" runat="server" Text="" ToolTip="Japanese Matter Description" BorderStyle="None" CssClass="TextBoxEditTokyo" TabIndex="-1"></asp:TextBox>
                        </td></tr><tr><td nowrap="nowrap" valign="top">
                            <asp:Label ID="LabelOffice" runat="server" Text=""></asp:Label><br />
                        </td></tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewSearchResults" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    

    <div id="search" runat="server" class="searchDivGridView">
        
        <span class="searchTitle">Tokyo Client / Matter Search 東京クライアントと案件の検索</span><br />
        <span class="searchInstructions">Searches Status, Client Number, Client Name, Client Address, Matter Number, Matter Description</span>
        <table cellpadding="2" border="0">
            <tr>
                <td valign="top">
                    <asp:RadioButtonList ID="RadioButtonListSearchFrom" runat="server" 
                        RepeatDirection="Vertical" Width="202px">
                        <asp:ListItem Selected="False" Text="Search Client Data Only" Value="C"></asp:ListItem>
                        <asp:ListItem Selected="False" Text="Search Matter Data Only" Value="M"></asp:ListItem>
                        <asp:ListItem Selected="True" Text="Search Both Client & Matter Data" Value="B"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td valign="top">
                    <asp:CheckBox ID="CheckBoxAllOffices" runat="server" Checked="true" Text="Search All Offices" />
                    <br />
                    <asp:TextBox ID="TextBoxOffices" runat="server" Enabled="false" CssClass="disabledTextBox" />
                    <br />
                    <asp:CheckBox ID="CheckBoxClosedMatters" runat="server" Checked="false" Text="Include Closed Matters" />
                </td>
                <td valign="top" rowspan="2">
                    <div id="OfficeListing">
                        <asp:RadioButtonList ID="RadioButtonListOffices" runat="server" CssClass="displayNone" >
                        </asp:RadioButtonList>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="labelSearch" runat="server" Text="Enter Search Criteria" CssClass="parameterLabel" /><br />
                    <asp:TextBox ID="textBoxSearch" runat="server" Width="271px"/>
                    <asp:ImageButton ID="ImageButtonSearch" runat="server" ImageUrl="~/images/controls/search.gif" OnClick="imageButtonSearchClick" AlternateText="Search" ToolTip="Search" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:ImageButton ID="ImageButtonResetColumnWidths" 
                                runat="server" 
                                ImageUrl="../images/controls/resetButton.gif" 
                                OnClick="imageButtonResetColumnWidthsClick" 
                                AlternateText="Reset Column Widths" 
                                ToolTip="Reset Column Widths" 
                                Visible="false"
                                OnClientClick="resetColumnWidths()" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ImageButtonSearch" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <asp:Label ID="labelSearchResults" runat="server" Text="" CssClass="leftBold" Visible="false"/>
        <br />
    </div>
    
    
    <div id="searchResults" class="searchResultsGridView">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView 
                    ID="GridViewSearchResults" 
                    runat="server" 
                    AllowSorting="True" 
                    AllowPaging="True"
                    DataSourceID="SqlDataSourceSearchTokyoClientMatter" 
                    Visible="False" 
                    ForeColor="#333333" 
                    CellPadding="4"
                    GridLines="Vertical" 
                    AutoGenerateColumns="False"
                    OnRowDataBound="GridViewSearchResults_RowDataBound"
                    OnSelectedIndexChanged="GridViewSearchResults_SelectedIndexChanged"
                    PageSize="20"
                    EmptyDataText='No Matching Results Found.'
                    CssClass="GridViewSearchResults"
                    >
                    <FooterStyle CssClass="GridViewFooter" />
                    <RowStyle CssClass="GridViewRow" />
                    <PagerStyle CssClass="GridViewPager" ForeColor="#FFFFFF"/>
                    <HeaderStyle BackColor="#2a76b2" Font-Bold="True" ForeColor="#FFFFFF" VerticalAlign="Bottom" Wrap="False" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <SelectedRowStyle CssClass="GridViewSelectedRow"/>
                    <PagerSettings  Mode="NextPreviousFirstLast" 
                                    FirstPageText="First"
                                    LastPageText="Last"
                                    NextPageText="Next"
                                    PreviousPageText="Previous"
                                    Position="Bottom"
                                    />
                    
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" Visible="False" />
                        <asp:BoundField 
                            DataField="mcurrency" 
                            HeaderText="Cur." 
                            SortExpression="mcurrency"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="mlang" 
                            HeaderText="Lang." 
                            SortExpression="mlang"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="ClientNum" 
                            HeaderText="Client" 
                            SortExpression="ClientNum"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="ClientName" 
                            HeaderText="Client Name" 
                            SortExpression="ClientName"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Left"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="nti_client" 
                            HeaderText="NTI<br />Client" 
                            SortExpression="nti_client"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="bsm_client" 
                            HeaderText="BSM<br />Client" 
                            SortExpression="bsm_client"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="JapaneseClientName" 
                            HeaderText="Japanese<br />Client_Name" 
                            SortExpression="JapaneseClientName"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Left"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="MatterNum" 
                            HeaderText="Matter" 
                            SortExpression="MatterNum"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="MatterDesc" 
                            HeaderText="Matter Description"
                            SortExpression="MatterDesc"
                            HtmlEncode="false"
                            ItemStyle-HorizontalAlign="Left"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="nti_matter"
                            HeaderText="NTI<br />Matter"
                            SortExpression="nti_matter"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="bsm_matter" 
                            HeaderText="BSM<br />Matter" 
                            SortExpression="bsm_matter"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="JapaneseMatterName" 
                            HeaderText="Japanese<br />Matter_Description"
                            SortExpression="JapaneseMatterName"
                            ItemStyle-HorizontalAlign="Left"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="mloc" 
                            HeaderText="Office<br />Code" 
                            SortExpression="mloc"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="MatterStatus" 
                            HeaderText="Matter<br />Status" 
                            SortExpression="MatterStatus"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="bsm_or_nti" 
                            HeaderText="BSM<br />NTI" 
                            SortExpression="bsm_or_nti"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField 
                            DataField="overseas_matter" 
                            HeaderText="Overseas<br />Matter?" 
                            SortExpression="overseas_matter"
                            HtmlEncode="false"
                            >
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>

                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ImageButtonSearch" />
            </Triggers>
        </asp:UpdatePanel>
        <cc1:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server" TargetControlID="UpdatePanel1" >
            <Animations>
                <OnUpdating>
                    <Parallel duration="0">
                        <%-- place the update progress div over the gridview control --%>
                        <ScriptAction Script="onUpdating();" /> 
                    </Parallel>
                </OnUpdating>
                <OnUpdated>
                    <Parallel duration="0">
                        <%--find the update progress div and place it over the gridview control--%>
                        <ScriptAction Script="onUpdated();" /> 
                    </Parallel> 
                </OnUpdated>
            </Animations>
        </cc1:UpdatePanelAnimationExtender>
        <asp:Panel ID="pnlPopup" runat="server" CssClass="progress" style="display:none;">
            <div class="progressContainer">
                <div class="progressHeader">Loading...</div>
                <div class="progressBody">
                    <img src="../images/activity.gif" alt="" />
                </div>
            </div>
        </asp:Panel>     
    </div>


    <asp:SqlDataSource ID="SqlDataSourceSearchTokyoClientMatter" runat="server" 
                ConnectionString="<%$ ConnectionStrings:eliteConnectionString %>" 
                SelectCommand="uspBMcBEARSearchTokyoClientMatter" 
                SelectCommandType="StoredProcedure"
                OnSelecting="SqlDataSource_OnSelecting">
            </asp:SqlDataSource>
        
    </form>
</body>
</html>
