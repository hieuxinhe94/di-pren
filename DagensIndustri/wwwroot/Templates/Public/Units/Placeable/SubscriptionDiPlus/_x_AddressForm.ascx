<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_4_AddressForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SubscriptionDiPlus._4_AddressForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>



<div id="main" role="main">
			
	<a href="subscribe-company.html" class="link-back">Föregående steg</a>
			
	<h1>Faktureringsadress</h1>
			
	<p class="required">= Obligatoriska uppgifter</p>
    
	<div class="form-wrapper" id="content">
		
        
        <div class="form-nav">
            <ul>
                <li class="current">
                    <a href="#form-street">Gata</a>
                </li>
                <li>
                    <a href="#form-box">Stoppställe/Postbox</a>
                </li>
            </ul>
        </div>

        <div class="form-box" id="content">
            <div class="section" id="form-street">
                <div class="form-field">
                    <DI:Input ID="InputCompany" Title="Företag" DisplayMessage="" MaxValue="28" TypeOfInput="Text" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>
            
                <div class="form-field">
                    <di:Input ID="InputCompanyNumber" Title="Organisationsnummer" DisplayMessage="" MinValue="10" MaxValue="10" TypeOfInput="OrgNumber" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
                </div>

		        <div class="form-field">
                    <DI:Input ID="InputStreetAdress" Title="Gatuadress" DisplayMessage="" MaxValue="27" TypeOfInput="Text" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>
				
		        <div class="form-field">
                    <DI:Input ID="InputStreetNum" Title="Gatunummer" DisplayMessage="" MaxValue="6" TypeOfInput="Text" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>
				
		        <div class="form-field">
                    <DI:Input ID="InputEntrance" Title="Uppgång" DisplayMessage="" MaxValue="11" TypeOfInput="Text" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>
		
                <div class="form-field">
                    <DI:Input ID="InputStairs" Title="Trappor" DisplayMessage="" MaxValue="3" TypeOfInput="Text" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>
        		
                <div class="form-field">
                    <DI:Input ID="InputAppartmentNum" Title="Lägenhetsnummer" DisplayMessage="" MaxValue="6" TypeOfInput="Text" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>

                <div class="form-field">
                    <DI:Input ID="InputZip" Title="Postnummer" DisplayMessage="" MinValue="5" MaxValue="5" TypeOfInput="ZipCode" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>

                <div class="form-field">
                    <DI:Input ID="InputCity" Title="Ort" DisplayMessage="" MaxValue="50" TypeOfInput="Text" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>

            </div>

            <div class="section" id="form-box" style="display: none;">
                <div class="form-field">
                    <DI:Input ID="Input_s_stopOrBox" Title="Stoppställe eller Box" DisplayMessage="" MaxValue="100" TypeOfInput="Text" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>				
		
                <div class="form-field">
                    <DI:Input ID="Input_s_num" Title="Nummer" DisplayMessage="" MaxValue="6" TypeOfInput="Text" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>
        
                <div class="form-field">
                    <DI:Input ID="Input_s_zip" Title="Postnummer" DisplayMessage="" MinValue="5" MaxValue="5" TypeOfInput="ZipCode" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>

                <div class="form-field">
                    <DI:Input ID="Input_s_city" Title="Ort" DisplayMessage="" MaxValue="50" TypeOfInput="Text" Required="true" Name="" StripHtml="true" AutoComplete="true" CssClass="text" runat="server" />
		        </div>
            </div>

        </div>


		<div class="btns-wrapper">
					
			<div class="payment-method">
				<h2>Betala mot faktura</h2>
				<p class="price"><asp:Literal ID="LiteralPriceExVat" runat="server" />:- / kvartal</p>
				<p class="price-small"><asp:Literal ID="LiteralPriceIncVat" runat="server" />:- / kvartal <em>(ink. moms)</em></p>
				<div class="btn-wrapper">
                    <asp:Button ID="Button1" Text="Skapa prenumeration" OnClick="ButtonCreateInvoiceSubs_Click" runat="server"/>
                </div>
			</div>
				
		</div>
	</div>
 
	<ul class="info-links">
		<li><a href="http://dagensindustri.se/System/Footer/Prenumerationsvillkor/">Prenumerationsvillkor</a></li>
		<li><a href="http://dagensindustri.se/Kontakta-oss/">Kundtjänst</a></li>				
	</ul>

    <!-- POPUPS -->
    <div class="popup-overlay"></div>
    <div class="popups">
        <div class="popup" id="terms-popup" style="display: block;">
            <div class="close">X</div>
            <div class="popup-content">
                <div class="popup-content-wrapper">
                    <h1>Prenumerationsvillkor</h1>
                    <p><strong>Priser</strong> <br>Prenumerationspriser finns på di.se/tidningen under rubriken Prenumerera. För ytterligare information kontakta vår kundtjänst per e-post <a href="mailto:pren@di.se">pren@di.se</a> eller telefon 08-573 651 00. <br><br><strong>Tillsvidareprenumeration</strong><br>För Dagens industri är prenumerationsformen huvudsakligen tillsvidareprenumeration. Sådan prenumeration löper under en viss tid och förlängs om uppsägning inte sker. Vid tillsvidareprenumeration som betalas på annat sätt än med autogiro motsvarar förlängningen i de flesta fall den ursprungliga tidsperioden. Vid tillsvidareprenumeration som betalas med autogiro löper prenumeration inledningsvis under tolv månader och förlängs därefter med en månad i taget om den inte sägs upp senast tre månader före utgången av tolvmånadersperioden. Utebliven betalning gäller ej som uppsägning.&nbsp;</p>
                    <p><strong>Fullständig prenumeration</strong><br>Vid fullständig prenumeration utkommer Dagens industri måndag till och med lördag med undantag för helgdagar och liknande. Sommartid utkommer tidningen inte på måndagar.&nbsp;<br><br><strong>Helgprenumeration</strong><br>Helgprenumeration på Dagens industri innebär att tidningen kommer fredagar och lördagar med undantag för helgdagar och liknande. Prenumeranterna får tidningen nämnda veckodagar oavsett när bilagan DI Weekend distribueras.</p>
                    <p><strong>Studerandeprenumeration</strong><br>Studerande kan få en tidsbestämd fullständig prenumeration på Dagens industri med särskild rabatt. Sådan prenumeration löper under en viss tid varefter den upphör utan uppsägning. För att ha rätt till studerandeprenumeration krävs att du är minst 18 år och kan uppvisa giltigt intyg som visar att du är studerande. Rabatten är personlig och både leverans av tidning och faktura måste gå till den person som har tecknat prenumerationen.<br><br><strong>Prenumeration för medlemmar i Unga Aktiesparare<br></strong>Medlemmar i Unga Aktiesparare kan få tidsbestämd fullständig prenumeration på Dagens industri med särskild rabatt. Sådan prenumeration löper under en viss tid varefter den upphör utan uppsägning. För att ha rätt till prenumeration av detta slag krävs att du är mellan 16 och 25 år och att du kan bevisa att du är medlem i Unga Aktiesparare. Rabatten är personlig och både leverans av tidning och faktura måste gå till den person som har tecknat prenumerationen.&nbsp;</p>
                    <p><strong>Pensionärsprenumeration</strong><br>Pensionärer har möjlighet till tillsvidareprenumeration på Dagens industri med särskild rabatt. För att ha rätt till sådan rabatt krävs att du kan dokumentera att du är pensionär. Rabatten är personlig och både leverans av tidning och faktura måste gå till den person som har tecknat prenumerationen.&nbsp;</p>
                    <p><strong>Betalning</strong> <br>Prenumeration betalas i förskott via bank-/postgiro eller med konto-/kreditkort, alternativt månadsvis med autogiro. Inbetald prenumerationsavgift återbetalas ej.&nbsp;</p>
                    <p><strong>Betalning med konto-/kreditkort</strong><br>Betalning med konto-/kreditkort görs på di.se/tidningen. Logga in och välj Mitt konto – Mina inställningar. Under fliken “fakturaöversikt” hittar du dina fakturor. Saknar du inloggningsuppgifter kontakta kundtjänst.</p>
                    <p><strong>Betalning med autogiro</strong><br>Betalning med autogiro innebär att angivet konto debiteras den 28:e varje månad och att prenumerationen löper enligt ovan under rubriken Tillsvidareprenumeration. Blankett för kontouppgifter samt godkännande av villkoren, inklusive att Dagens industri är berättigad att varje månad debitera avgiften, erhålls vid beställning av betalning med autogiro. Månadsvis faktura skickas inte ut vid betalning med autogiro. Om inte ditt och bankens medgivande erhållits inom sju dagar från beställning av betalning med autogiro får du en vanlig faktura med förfallotid om 20 dagar för den första månadsavgiften.&nbsp;</p>
                    <p><strong>Leverans </strong><br>Dagens industris ambition är att du ska få varje utgåva av tidningen som ingår prenumerationen, någon absolut leveransgaranti kan dock inte ges. Om tidningen uteblir ber vi dig kontakta kundtjänst. Dagens industris ansvar begränsas till av dig anmälda uteblivna exemplar och till prenumerationspriset för dessa. Prenumerationen förlängs därför efter anmälan med motsvarande antal dagar.&nbsp;</p>
                    <p><strong>Distribution </strong><br>Dagens industri distribueras huvudsakligen av tidningsbud. På de orter som saknar tidningsbud en eller flera dagar i veckan distribueras Dagens industri i stället via post. Det förekommer även på ett fåtal orter att lördagstidningen distribueras tillsammans med måndagstidningen. Vid adressändring till utlandet tillkommer särskild avgift för distribution och Dagens industri reserverar sig för störningar i transporterna och lokala distributionsproblem i mottagarlandet.</p>
                    <p><strong>Adressändring, uppehåll<br></strong>För att ändra utdelningsadress eller göra tillfälligt uppehåll i prenumerationen gå in på di.se/tidningen. Logga in och välj Mitt konto – Mina inställningar. Du sköter enkelt dina prenumerationsärenden under fliken Adressändring/uppehåll. Saknar du inloggningsuppgifter kontakta kundtjänst.</p>
                    <p><strong>Provprenumeration<br></strong>Dagens industri tillhandahåller i särskilda sammanhang rabatterade eller kostnadsfria provprenumerationer som är förenade med specifika villkor, exempelvis att de distribueras inom Sverige och endast kan tecknas av medlemmar i hushåll som inte redan prenumererar och inte heller har prenumererat på Dagens industri de senast sex månaderna. Sådana prenumerationer löper antingen tills vidare eller är tidsbestämda. En förutsättning för provprenumeration är att erforderliga uppgifter, som namn, adress, telefonnummer m.m., anges vid beställning. Dagens industri har rätt att omedelbart avsluta en provprenumeration utan att redovisa skälen för detta.</p>
                    <p><strong>Behandling av personuppgifter <br></strong>Dagens industri behandlar och lagrar de personuppgifter som du direkt eller indirekt lämnar i en elektronisk databas. Dagens industris ändamål med uppgiftsbearbetningen är framför allt att kunna uppfylla de åtaganden som vi som tidningsutgivare har gentemot dig som prenumerant. Dagens industri säljer inte, och lämnar inte ut, personuppgifter om sina prenumeranter till företag utanför Bonnier AB. Du kan som prenumerant bli erbjuden varor och tjänster från Dagens industri eller andra företag inom Bonnier AB. Om du inte vill ta del av eventuella erbjudanden kan du kontakta kundtjänst. Bonnier AB har en övergripande integritetsskyddspolicy som du kan ta del av på webbsidan <a href="http://www.kunddata.bonnier.se">www.kunddata.bonnier.se</a></p>
				
                </div>
            </div>
        </div>
        <div class="popup" id="support-popup" style="display: block;">
            <div class="close">X</div>
            <div class="popup-content">
                <div class="popup-content-wrapper">
                    <h1>Kundtjänst</h1>
                    <p><strong>Telefonnummer</strong><br>08-573 650 00<br>Växelns öppettider är: Må-Fre klockan 08.00-18.00.<br><br><strong>Faxnummer</strong><br>08-573 652 20<br><br><strong>Prenumerationsärenden </strong><br>Tel:08-573 651 00<br>Fax:08-31 74 08<br><a href="mailto:pren@di.se">pren@di.se</a></p>
                    <p>Öppettider är: Må-Fre klockan 06.00-18.00.<br>Lör klockan 07.00-12.00<br><br><strong>Teknisk support</strong><br>08-573 651 00<br><a href="mailto:pren@di.se">pren@di.se</a><br><br><strong>Besöksadress</strong><br><a href="http://goo.gl/TzxtB" target="_blank">Torsgatan 21, Stockholm</a><br>Närmaste T-banestation: St Eriksplan<br><br><strong>Göteborgskontor</strong><br>031-711 34 80<br><a href="http://goo.gl/cn0Cl" target="_blank">Västra Hamngatan 9, Göteborg</a><br><br>Anställda på Dagens Industris redaktion nås på e-post fornamn.efternamn@di.se</p>      
                </div>
            </div>
        </div>
    </div>
    <!-- POPUPS END -->
						
</div>
