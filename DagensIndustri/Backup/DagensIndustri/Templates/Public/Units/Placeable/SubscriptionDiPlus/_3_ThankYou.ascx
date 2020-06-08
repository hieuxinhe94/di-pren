<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_3_ThankYou.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SubscriptionDiPlus._3_ThankYou" %>
<%@ Register src="Footer.ascx" tagname="Footer" tagprefix="uc1" %>

 <div id="main" role="main">
	<h1>Tack!</h1>
	<p>Du är nu prenumerant på Dagens industri i läsplattan. Ladda ned appen och logga in med ditt angivna lösenord och epostadress för att starta den.</p>
     
    <p>
		Ditt angivna kontokort kommer löpande debiteras <strong><asp:Literal ID="LiteralPrice" runat="server" /> kr inkl. moms</strong> månadsvis.<br />
		<%--Logga in på <a href="http://www.dagensindustri.se" target="di">Dagensindustri.se</a> för att se en översikt av dina debiteringar.--%>
	</p>

    <%--<asp:PlaceHolder ID="PlaceHolderInvoice" runat="server">
        <h2 class="small">Faktura skickas löpande en gång per kvartal till:</h2>
        <p><asp:Literal ID="LiteralInvocieAddress" runat="server" /></p>
    </asp:PlaceHolder>--%>

    <h2 class="large">Ladda ned direkt</h2>
	<ul class="download">
		<li class="ios"><a href="http://itunes.apple.com/se/app/di+/id409124182?mt=8" target="_blank">Ladda ner för iPad</a></li>
		<li class="android"><a href="https://play.google.com/store/apps/developer?id=Dagens+industri+AB&hl=sv" target="_blank">Ladda ner för surfplatta</a></li>
	</ul>					
	
    <uc1:Footer ID="Footer1" runat="server" />

</div>


