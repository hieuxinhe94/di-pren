<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubscriptionPaymentMethod.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Subscription.SubscriptionPaymentMethod" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<asp:PlaceHolder ID="InvoiceCreditCardPlaceHolder" runat="server">
    <div class="form-box">

	    <h2><EPiServer:Translate Text="/subscription/subscriberdetails/wanttopaywith" runat="server" /></h2>
	    <div class="radiolist">
	        <ul>
		        <li>
                    <span class="label">
                        <asp:RadioButton ID="InvoiceRadioButton" Text="<%$ Resources: EPiServer, subscription.paymentmethod.invoice %>" GroupName="PaymentMethod" Checked="true" runat="server" />
                    </span>
		        </li>
                <li>
                    <span class="label">
                        <asp:RadioButton ID="InvoiceAnotherAddresseeRadioButton" Text="<%$ Resources: EPiServer, subscription.paymentmethod.invoiceanotheraddressee %>" GroupName="PaymentMethod" runat="server" />
                    </span>
		        </li>
		        <li>
                    <span class="label">
                        <asp:RadioButton ID="CardPaymentRadioButton" Text="<%$ Resources: EPiServer, subscription.paymentmethod.cardpayment %>" GroupName="PaymentMethod" runat="server" />
                        <img src="../images/ills/creditcards.png" alt='<%= Translate("/subscription/paymentmethod/acceptedcreditcards") %>' class="creditcards" />
                    </span>                
		        </li>
	        </ul>
        </div>
    </div>
</asp:PlaceHolder>


<asp:PlaceHolder ID="DirectDebitPlaceHolder" runat="server">
    			
	<!-- Autogiro -->
	<div class="form-box">
		<h2><EPiServer:Translate Text="/subscription/paymentmethod/directdebit" runat="server" /></h2>
		<div class="row row-checkbox">
  		<span class="checkbox">
            <DI:Input ID="DirectDebitInput" TypeOfInput="CheckBox" Required="true" Title='<%# string.Format(Translate("/subscription/directdebit/havedownloadedform"), (string)CurrentPage["DirectDebitForm"])%>' DisplayMessage="<%$ Resources: EPiServer, common.validation.formdownloadrequired %>" runat="server"/>
  		</span>
  	</div>
    
  	<a href="#personalinfo" class="more togglecontent"><EPiServer:Translate Text="/subscription/directdebit/howhandlepersonalinfo" runat="server" /></a>				
 		<div id="personalinfo" class="expandable">
            <%= (string)CurrentPage["directdebitpersonaldetails"] %>
 		</div>
	</div>
	<!-- // Autogiro -->	
</asp:PlaceHolder>