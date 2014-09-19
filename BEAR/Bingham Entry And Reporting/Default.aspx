<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BEAR._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Bingham Entry And Rreporting (B.E.A.R.)</title>
    <link href="style/style.css" rel="stylesheet" type="text/css" media="all" />
    <link href="style/applications.css" rel="stylesheet" type="text/css" />

    <script src="scripts/general.js" type="text/javascript"></script>
        
</head>
<body>
    <form id="form1" runat="server">
        
        <div id="title" class="title">
            <img 
                alt="BINGHAM" 
                src="images/binghamLogoSmall.gif" 
                />
            <img 
                alt="Bingham Entry and Reporting" 
                src="images/bearLogo.gif" 
                class="titlePic"
                id ="titlePic"
                />
            &nbsp;
        </div>

       <div id="picture"
        class="pictureHome"
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
           <img src="images/posters/bingham1.jpg" alt="B.E.A.R."/><br />
        <div id="applications">
            <p class="housebuttonTitle">
            Select an Application:
            </p>
            <p class="housebutton">
                <a href="billTracker/Default.aspx">Bill Tracker</a>
            </p>
            <p class="housebutton">
                <a href="cas/Default.aspx">CAS (Cost Allocation System)</a>
            </p>
            <p class="housebutton">
                <a href="cashflow/searchCashFlow.aspx">Cash Flow Manager (Filter Page)</a>
            </p>
            <p class="housebutton">
                <a href="search/cashreceipts.aspx">Cash Receipts Client Matter Search</a>
            </p>
            <p class="housebutton">
                <a href="splitManager/Default.aspx">Split Manager</a>
            </p>
            <p class="housebutton">
                <a href="search/tokyoclientmatter.aspx">Tokyo Client Matter Search</a>
            </p>
            <p class="housebutton">
                <a href="exceptionRates/parameters.aspx">Exception Rates</a>
            </p>
            <p class="housebutton">
                <a href="4qcashforecast/attorneyforecast.aspx">4th Quarter Attorney Cash Forecast</a>
            </p>
<%--            <p class="housebutton">
                <a href="lockbox/Default.aspx">Lockbox Processing</a>
            </p>
--%>
        </div>
    </div>
    

    
    </form>
</body>
</html>
