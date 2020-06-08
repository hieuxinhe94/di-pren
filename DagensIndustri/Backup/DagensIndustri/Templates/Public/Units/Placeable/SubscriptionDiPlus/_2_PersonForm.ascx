<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_2_PersonForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SubscriptionDiPlus._2_PersonForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register src="Footer.ascx" tagname="Footer" tagprefix="uc1" %>


<div id="main" role="main">
			
	<%--<a href="prenumerera-ftg.html" class="link-back">Föregående steg</a>--%>
			
	<h1><asp:Literal ID="LiteralHeader" runat="server" /></h1>
    
	<p class="form-description">
		<span>Fyll i dina uppgifter</span>
		<span class="required">= Obligatoriska uppgifter</span>
	</p>
			
	<div class="form-wrapper">
        

        <asp:PlaceHolder ID="PlaceHolderCusno" runat="server">
            <div class="form-field">
                <DI:Input ID="InputCusno" Title="Kundnummer" DisplayMessage="" MaxValue="10" TypeOfInput="Text" Required="true" CssClass="text" StripHtml="true" AutoComplete="true" Name="" runat="server" />
		    </div>
		</asp:PlaceHolder>
        
	
        <asp:PlaceHolder ID="PlaceHolderRegularFields" runat="server">
            
            <asp:PlaceHolder ID="PlaceHolderPersonFields" runat="server">
                <div class="form-field">
                    <DI:Input ID="InputCompany" Title="Företag" DisplayMessage="" MaxValue="50" TypeOfInput="Text" Required="false" CssClass="text" StripHtml="true" AutoComplete="true" Name="" runat="server" />
		        </div>

                <div class="form-field">
                    <DI:Input ID="InputFirstName" Title="Förnamn" DisplayMessage="" MaxValue="50" TypeOfInput="Text" Required="true" CssClass="text" StripHtml="true" AutoComplete="true" Name="" runat="server" />
                </div>
				
		        <div class="form-field">
                    <DI:Input ID="InputLastName" Title="Efternamn" DisplayMessage="" MaxValue="50" TypeOfInput="Text" Required="true" CssClass="text" StripHtml="true" AutoComplete="true" Name="" runat="server" />
                </div>
				
		        <div class="form-field">
                    <DI:Input ID="InputPhone" Title="Mobiltelefonnummer" DisplayMessage="070XXXXXXX" MinValue="7" MaxValue="20" TypeOfInput="Telephone" Required="true" CssClass="text" StripHtml="true" AutoComplete="true" Name="" runat="server" />
                </div>
            </asp:PlaceHolder>
				
		    <div class="form-field form-divider form-description">
                <DI:Input ID="InputEmail" Title="E-post" DisplayMessage="" MinValue="6" TypeOfInput="Email" Required="true" CssClass="text" StripHtml="true" AutoComplete="true" Name="" runat="server" />
        	    <p class="description">(Används som användarnamn för appen)</p>
		    </div>				
				
		    <div class="form-field">
                <DI:Input ID="InputPasswd1" Title="Lösenord till app" DisplayMessage="Minst 6 tecken" MinValue="6" TypeOfInput="Password" Required="true" CssClass="text" StripHtml="true" AutoComplete="true" Name="" runat="server" />
            </div>
				
		    <div class="form-field">
                <DI:Input ID="InputPasswd2" Title="Bekräfta lösenord till app" DisplayMessage="Minst 6 tecken" MinValue="6" TypeOfInput="Password" Required="true" CssClass="text" StripHtml="true" AutoComplete="true" Name="" runat="server" />
            </div>
            
            
            <div class="form-field">
                <div class="oneRowCheckBox">
                    <DI:Input ID="TermsAcceptedInput" TypeOfInput="CheckBox" Required="true" Name="terms" Title="Jag godkänner <a href='#' data-popup='#terms-popup'>prenumerationsvillkoren</a>" DisplayMessage="" runat="server" />
	            </div>
            </div>
            

		</asp:PlaceHolder>


        <asp:Literal ID="LiteralErr" runat="server"></asp:Literal>


		<div class="btns-wrapper">
					
            <asp:PlaceHolder ID="PlaceHolderButtonCalcPrice" runat="server">	
                <div class="payment-method">
                    <%--<h2>Test</h2>--%>
				    <p class="price">Priset varierar beroende på vilken typ av prenumeration du har</p>
                    <div class="btn-wrapper">
                        <asp:Button ID="Button3" Text="Beräkna pris" OnClick="ButtonCusno_Click" runat="server"/>
                    </div>
                </div>
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="PlaceHolderButtonPayCreditCard" runat="server">	
			    <div class="payment-method">
				    <h2>Betala med kreditkort</h2>

                    <p>Avgiften dras månadsvis via automatisk kontokortsdragning.</p>

				    <p class="price"><asp:Literal ID="LiteralPriceCardBig" runat="server" />:- / mån</p>
				    <p class="price-small"><asp:Literal ID="LiteralPriceCardSmall" runat="server" />:- / mån <em>(ex. moms)</em></p>
				    <div class="btn-wrapper">
                        <asp:Button ID="Button1" Text="Betala" OnClick="ButtonPayCreditCard_Click" runat="server"/>
				    </div>
			    </div>
			</asp:PlaceHolder>

            <%--<asp:PlaceHolder ID="PlaceHolderButtonPayInvoice" runat="server">	
			    <div class="payment-method">
				    <h2>Betala mot faktura</h2>
				    <p class="price"><asp:Literal ID="LiteralPriceInvoiceBig" runat="server" />:- / kvartal</p>
				    <p class="price-small"><asp:Literal ID="LiteralPriceInvoiceSmall" runat="server" />:- / kvartal <em>(ink. moms)</em></p>
				    <div class="btn-wrapper">
                        <asp:Button ID="Button2" Text="Betala" OnClick="ButtonPayInvoice_Click" runat="server"/>
				    </div>
			    </div>
            </asp:PlaceHolder>--%>
				
		</div>
	</div>
 
	<uc1:Footer ID="Footer1" runat="server" />
						
</div>
