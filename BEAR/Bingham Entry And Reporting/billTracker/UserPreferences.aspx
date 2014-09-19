<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPreferences.aspx.cs" Inherits="BEAR.billTracker.UserPreferences" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <title>BillTracker User Column Preferences</title>
    <link href="../jQuery/css/ui-lightness/jquery-ui-1.7.2.custom.css" rel="stylesheet" type="text/css" />
    
     <link href="../style/billTracker.css" rel="stylesheet" type="text/css" media="all" />
     
    <script src="../jQuery/js/jquery-1.3.2.min.js" type="text/javascript"></script>    
    <script src="../jQuery/js/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>    
    <style type="text/css">
	    #sortable { list-style-type: none; margin: 100; padding: 0; width: 200px; }
	    #sortable li { margin: 0 5px 5px 5px; padding: 5px; font-size: .75em; height: 1.0em; cursor:n-resize;}
	    html>body #sortable li { height: 1.0em; line-height: .75em;}
	    .ui-state-highlight { height: 1.0em; line-height: .75em; }
    
        .hidden
        {
	        display:none;
	        visibility:hidden;
        } 
        
        body  
        {
        	font-family: arial, sans-serif; 
        }

	</style>
    <script type="text/javascript">
	    $(function() {
		    $("#sortable").sortable({ placeholder: 'ui-state-highlight' });
		    $("#sortable").disableSelection();
	    });
	    
        function Save()
        {
        var items = $(".ui-state-default");
        var textbox1 = document.getElementById("TextBox1");
        var textbox2 = document.getElementById("TextBox2");
        var textbox3 = document.getElementById("TextBox3");
        var textbox4 = document.getElementById("TextBox4");
        var textbox5 = document.getElementById("TextBox5");
        var textbox6 = document.getElementById("TextBox6");
        var textbox7 = document.getElementById("TextBox7");
        var textbox8 = document.getElementById("TextBox8");
        var textbox9 = document.getElementById("TextBox9");
        var textbox10 = document.getElementById("TextBox10");
        var textbox11 = document.getElementById("TextBox11");
        var textbox12 = document.getElementById("TextBox12");
        var textbox13 = document.getElementById("TextBox13");
        
     
        for(var x=0; x<items.length; x++)
        {
            //alert(x + "\n" + items[x].id);
            
            switch(x)
            {
            case 0:
              textbox1.value = items[x].id;
              break;
            case 1:
              textbox2.value = items[x].id;
              break;
            case 2:
              textbox3.value = items[x].id;
              break;
            case 3:
              textbox4.value = items[x].id;
              break;
            case 4:
              textbox5.value = items[x].id;
              break;
            case 5:
              textbox6.value = items[x].id;
              break;
            case 6:
              textbox7.value = items[x].id;
              break;
            case 7:
              textbox8.value = items[x].id;
              break;
            case 8:
              textbox9.value = items[x].id;
              break;
            case 9:
              textbox10.value = items[x].id;
              break;
            case 10:
              textbox11.value = items[x].id;
              break;
            case 11:
              textbox12.value = items[x].id;
              break;
            case 12:
              textbox13.value = items[x].id;
              break;
            default:
              break;
            }
            
            //alert(document.getElementbyID("TextBox"+ (x+1) ).value);
        }
  
 }  
	    
	    
	    
    </script>

    

</head>

<body>
    <form id="form1" runat="server">
    
        <div id="title" class="titleBT">
            <img id="binghamLogo" alt="BINGHAM" src="../images/binghamLogoSmall.gif" />
            <img id="titlePic"  alt="Bill Tracker" src="../images/BillTrackerTitle.gif" class="titlePic" />
        </div>

        <div id="instructions" class="instructionsDiv">
            <p class="instructionsText">
            <span class="additionalTitle">Column Order</span> - Use this screen to set the column display order in the BillTracker online application.
            <ul class="instructionsText">
            <li class="liHeading">INSTRUCTIONS:</li>
            <li>Click and drag the column headings below.</li>
            <li>The order of the columns from top to bottom will be the order of the columns on the application from left to right.</li>
            <li>Click the Save button</li>
            </ul>
            </p>
                
            <ul id="sortable" runat="server">
            </ul>
            <ul class="additionalNotes">
                <li class="liHeading">ADDITIONAL NOTES:</li>
                <li>The Undo Button, will undo the changes made since the last save point.</li>
                <li>The Default Order Button, will erase your custom column ordering.</li>
                <li>The first two columns: "Draft Sent" and "Ready to Bill" must remain at the front of the list.</li>
                <li>The last columns: "Reversal Code", "Notes", "Exception", and "Exception Reason" are not movable and must be at the end.</li>
                <li>Column Ordering does not change the column order on the report.  This only affects the application.</li>
                <li>The column order will be unique to you (the person logged into the network on PC being used).</li>
            </ul>

            
         </div>   
         
         <div id="saveOptions" class="saveOptions">
            <asp:Button ID="ButtonSave" runat="server" Text="Save" OnClientClick="Save()" OnClick="ButtonSave_Click" CssClass="OrangeButton widthSaveButton"/>
            <br /><br />
            <asp:Button ID="ButtonRestore" runat="server" Text="Undo Changes" OnClick="ButtonRestore_Click" CssClass="OrangeButton widthSaveButton"/>
            <br /><br />
            <asp:Button ID="ButtonReset" runat="server" Text="Default Order" OnClick="ButtonReset_Click" CssClass="OrangeButton widthSaveButton"/>
         </div>
        
        <div id="options" class="optionsColumnOrder">
            <asp:Button ID="ButtonBillTracker" runat="server" Text="Parameters" OnClick="ButtonButtonBillTracker_Click" CssClass="OrangeButton widthOptionsButton"/>
         </div>
       
        <asp:TextBox ID="TextBox1" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox2" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox3" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox4" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox5" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox6" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox7" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox8" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox9" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox10" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox11" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox12" runat="server" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="TextBox13" runat="server" CssClass="hidden"></asp:TextBox>
        
    </form>
</body>
</html>
