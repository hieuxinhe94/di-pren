<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayMethodsForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiCampaign.PayMethodsForm" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>


<asp:PlaceHolder ID="PlaceHolderMultipePayMethods" runat="server">
    <div class="row radiolist">
	    <h4>Jag vill betala med</h4>
    
        <table border="0" cellpadding="0" cellspacing="0" style="margin:7px;">
        <tr>
            <asp:PlaceHolder ID="PlaceHolderCard" runat="server">
                <td><asp:RadioButton ID="RbCard" GroupName="rbl" runat="server" /></td>
                <td style="padding-right:20px;">Kort</td>
            </asp:PlaceHolder>
            
            <asp:PlaceHolder ID="PlaceHolderInvoice" runat="server">
                <td><asp:RadioButton ID="RbInvoice" GroupName="rbl" runat="server" /></td>   <%--added by code behind: onclick="javascript:jsShowAddressFields('true/false');"--%>
                <td style="padding-right:20px;">Faktura</td>
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="PlaceHolderInvoiceOtherPayer" runat="server">
                <td><asp:RadioButton ID="RbInvoiceOtherPayer" GroupName="rbl" runat="server" /></td>
                <td>Faktura, annan betalare</td>
            </asp:PlaceHolder>

            <%--<asp:PlaceHolder ID="PlaceHolderAutowithdrawal" runat="server">
                <td><asp:RadioButton ID="RbAutowithdrawal" GroupName="rbl" runat="server" /></td>
                <td>Autodragning på kort</td>
            </asp:PlaceHolder>--%>

        </tr>
        </table>
    
        <asp:PlaceHolder ID="CreditCardsPlaceHolder" runat="server">
		    <img src="/templates/public/images/ills/creditcards.png" alt="Vi accepterar: MasterCard, Visa och American Express" class="creditcards" />	
        </asp:PlaceHolder>					
    </div>
</asp:PlaceHolder>


<asp:PlaceHolder ID="PlaceHolderDirectDebit" Visible="false" runat="server">

	<div class="row radiolist">
		<h4><%= Translate("/dicampaign/directdebit/header") %></h4>
     
        <asp:PlaceHolder ID="PlaceHolderDirDebOtherPayer" runat="server">
            <div style="margin:7px;" class="checkbox">
  		        <span class="checkbox">
                    <di:Input ID="DirectDebitOtherPayerInput" TypeOfInput="CheckBox" Name="directdebitotherpayer" Title="Annan betalare" runat="server" />
		        </span>
	        </div>
        </asp:PlaceHolder>
	
<%--
		<div style="margin:7px;" class="checkbox">
  		    <span class="checkbox">
                <DI:Input ID="DirectDebitInput" TypeOfInput="CheckBox" Required="true" Title="<%$ Resources: EPiServer, dicampaign.directdebit.checkboxtext %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.formdownloadrequired %>" runat="server"/>
            </span>
  	    </div>

        <a href="#agform_<%=ClientID %>" class="more togglecontent"><EPiServer:Translate ID="Translate1" Text="/dicampaign/directdebit/aboutform" runat="server" /></a>
 	    <div id="agform_<%=ClientID %>" class="expandable">
            <p><%= Translate("/dicampaign/directdebit/terms") %></p>
        </div>
	
  	    <a href="#personalinfo_<%=ClientID %>" class="more togglecontent"><EPiServer:Translate ID="Translate2" Text="/subscription/directdebit/howhandlepersonalinfo" runat="server" /></a>
 	    <div id="personalinfo_<%=ClientID %>" class="expandable">
            <p><%= Translate("/dicampaign/directdebit/personalinfo") %></p>
 	    </div>--%>
    </div>
</asp:PlaceHolder>


<asp:PlaceHolder ID="PlaceHolderAwd" Visible="false" runat="server">
	
    <div class="row radiolist">
		<h4>Betalas via autodragning på kort</h4>

        <div style="margin:7px; margin-bottom:-14px;">
            <p>
                Avgiften dras via automatisk kontokortsdragning vid varje betalningstillfälle.
            </p>
	    </div>
    </div>
</asp:PlaceHolder>
