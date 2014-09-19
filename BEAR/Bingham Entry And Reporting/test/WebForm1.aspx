<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="BEAR.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
   <meta http-equiv="Content-Type" content="text/html; charset=utf-8">

    <script src="bearbox/jqueryBillTracker.js" type="text/javascript"></script>
    <link href="bearbox/facebox.css" rel="stylesheet" type="text/css" />
    <script src="bearbox/facebox.js" type="text/javascript"></script>
    
    <script language="javascript" type="text/javascript">
        var controlVariable = "";
    
        jQuery(document).ready(function($) {
          $('a[rel*=facebox]').facebox()
        }) 
        
        function populateBox(textboxID) {
            controlVariable = textboxID;
            var textBoxValue = document.getElementById(textboxID).value;
            var htmlBeforeValue = "<textarea id='textArea1' name='comments' cols=50 rows=10 wrap=physical>";
            var htmlAfterValue = "</textarea>";
            var allPutTogether = htmlBeforeValue + textBoxValue + htmlAfterValue;
            jQuery.facebox(allPutTogether);
            

        }
    </script>

 </head>

<body>
    <form id="form1" runat="server">
    <div>
       <asp:TextBox ID="TextBox1" runat="server" Text=""></asp:TextBox>
        <input id="Button2" type="button" value="button" onclick="populateBox('TextBox1')" />
        <br />
       <asp:TextBox ID="TextBox2" runat="server" Text=""></asp:TextBox>
        <input id="Button1" type="button" value="button" onclick="populateBox('TextBox2')" /> 
    </div>
    </form>
</body>
</html>
