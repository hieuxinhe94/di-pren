<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubscriptionDiPlus.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus" %>
<%--<%@ Register tagprefix="uc1" tagname="_1_Start" src="../Units/Placeable/SubscriptionDiPlus/_1_Start.ascx" %>--%>
<%@ Register tagprefix="uc1" tagname="_1_SelectSubsType" src="../Units/Placeable/SubscriptionDiPlus/_1_SelectSubsType.ascx" %>
<%@ Register tagprefix="uc1" tagname="_2_PersonForm" src="../Units/Placeable/SubscriptionDiPlus/_2_PersonForm.ascx" %>
<%@ Register tagprefix="uc1" tagname="_3_ThankYou" src="../Units/Placeable/SubscriptionDiPlus/_3_ThankYou.ascx" %>
<%@ Register tagprefix="uc1" tagname="_4_Error" src="../Units/Placeable/SubscriptionDiPlus/_4_Error.ascx" %>
<%@ Register TagPrefix="di" TagName="googleanalytics" Src="~/Templates/Public/Units/Static/GoogleAnalytics.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <title>Dagens industri i läsplatta</title>
    <meta name="description" content=""/>
    <meta name="author" content=""/>
    <meta name="viewport" content="width=device-width,initial-scale=1"/>
    <link rel="stylesheet" href="../Styles/subscriptionDiPlus.css"/>
    <script src="../js/jquery-1.5.1.min.js" type="text/javascript"></script>
    <script src="../js/cufon-yui.js" type="text/javascript"></script>
    <script src="../js/jquery.QB.DiSeRSS.js" type="text/javascript"></script>
    <script src="../js/jquery.QB.SlideShow.js" type="text/javascript"></script>
    <script src="../js/master.js" type="text/javascript"></script>
    <script src="../js/subscriptionDiPlus.js" type="text/javascript"></script>
    <script src="../js/modernizr-2.0.6.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" novalidate="novalidate">
    
        <asp:Literal ID="LiteralPlusSub" runat="server"></asp:Literal>

        <div id="container">
  
            <asp:PlaceHolder ID="PlaceHolderIpad" runat="server">
		        <div class="surfboard">
		            <img src="../images/subscriptionDiPlus/ipad-content-2013.jpg" alt="Dagens industri i läsplatta" width="394" height="525" />
		            <div class="glare"></div>
		        </div>  
            </asp:PlaceHolder>
  
            <asp:PlaceHolder ID="PlaceHolderInvoice" runat="server">
                <div class="illustration">
                    <h2>Så hittar du ditt kundnummer</h2>
                    <img src="../images/subscriptionDiPlus/invoice.jpg" alt="" width="533" height="598" />
                    <p>Ditt kundnummer hittar du på din faktura. Om du inte har ditt kundnummer tillgängligt är du välkommen att ringa kundtjänst på <strong>08-573 651 00</strong></p>
                </div>
            </asp:PlaceHolder>


            <%--<uc1:_1_Start ID="_1_Start1" runat="server" />--%>
            <uc1:_1_SelectSubsType ID="_1_SelectSubsType1" runat="server" />
            <uc1:_2_PersonForm ID="_2_PersonForm1" runat="server" />
            <uc1:_3_ThankYou ID="_3_ThankYou1" runat="server" />
            <uc1:_4_Error ID="_4_Error1" runat="server" />
 
        

      </div> 
      <!--! end of #container -->


      <!-- JavaScript at the bottom for fast page loading -->
      <!-- Grab Google CDN's jQuery, with a protocol relative URL; fall back to local if offline -->
      <!--<script src="//ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>-->
      <!--<script>      window.jQuery || document.write('<script src="js/libs/jquery-1.6.2.min.js"><\/script>')</script>-->
      <!--<script defer src="js/plugins.js"></script>-->
      <!--<script defer src="js/script.js"></script>-->
      <!-- end scripts-->

    
        <di:googleanalytics ID="Googleanalytics1" runat="server" />

    </form>

</body>
</html>
