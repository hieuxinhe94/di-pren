<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_1_SelectSubsType.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SubscriptionDiPlus._1_SelectSubsType" %>
<%@ Register src="Footer.ascx" tagname="Footer" tagprefix="uc1" %>


<div id="main" role="main">
	<%--<a href="prenumerera.html" class="link-back">Föregående steg</a>--%>
			
    <h1 class="start">Mindre format – Större innehåll</h1>
    <p>
        En digital prenumeration på Dagens industri innebär att du får tillgång till tidningens innehåll som pdf men också Dagens industri och Di Weekend i din läsplatta.
        <br />
        <br />
        Dagens industri med de senaste Twitter kommentarerna från våra reportrar och analytiker samt ständigt uppdaterad marknadsinformation från världens börser.
        <br />
        <br />
        Di Weekend i läsplattan med de långa intervjuerna, de handfasta tipsen om krog, mat, kultur och livsstil.<br />
        Allt i ett snyggt, lätthanterligt format.
    </p>

	<%--<h1>Prenumerera</h1>--%>
    <h2 class="large white xtramargin">Prenumerera</h2>
 
	<h2>Ny prenumeration för läsplattan</h2>
	<div class="choice">
        <p>
            Jag är inte prenumerant på Dagens industri och vill prenumerera på läsplattan för 
            <asp:Literal ID="LiteralPriceNewIncVat" runat="server" /> kr i månaden 
        </p>
		
        <p class="price">
            (<asp:Literal ID="LiteralPriceNewExVat" runat="server" /> kr ex. moms)
        </p>
        
        <%--<p>Jag har ingen prenumeration på Di i pappersformat och vill ha prenumeration på läsplatta</p>
		<p class="price">Pris <asp:Literal ID="LiteralPriceNew" runat="server" />:-/mån <em>(ink. moms)</em></p>--%>
        <div class="btn-wrapper">
            <asp:Button ID="Button1" Text="Prenumerera" OnClick="ButtonNewSubs_Click" runat="server"/>
		</div>
	</div>

    <!--
    <h2>Ny prenumeration på läsplatta</h2>			
	<div class="choice">
        <p>
            Jag är inte prenumerant men vill prenumerera på Dagens industri i läsplatta för 
            <asp:Literal ID="LiteralPriceNewStandAloneIncVat" runat="server" /> kr i månaden 
        </p>
		
        <p class="price">
            Från <asp:Literal ID="LiteralPriceNewStandAloneExVat" runat="server" />:-/mån <em>(ink. moms)</em>
        </p>
		
        <div class="btn-wrapper">
            <asp:Button ID="Button3" Text="Prenumerera"    runat="server"/>
		</div>
	</div>
    -->
			
	<!--
    <h2>Uppgradera prenumeration</h2>			
	<div class="choice">
        <p>Uppgradera min prenumeration på Dagens industri med läsplattan</p>
		<div class="btn-wrapper">
            <asp:Button ID="Button2" Text="Uppgradera" OnClick="ButtonUpgradeSubs_Click" runat="server"/>
		</div>
	</div>
    -->
 
	<uc1:Footer ID="Footer1" runat="server" />

</div>
