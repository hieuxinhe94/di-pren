<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Invoice.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.MySettings2.Invoice" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>


<!-- Invoices -->
<div class="section" id="invoice">
    
    <asp:PlaceHolder ID="MessagePlaceHolder" Visible="false" runat="server" >
        <div class="row">
            <p class="description"><EPiServer:Translate ID="Translate1" Text="/mysettings/invoice/nounpaid" runat="server" /></p>
        </div>
    </asp:PlaceHolder>

    <asp:Repeater ID="InvoiceRepeater" runat="server">   <%--OnItemDataBound="InvoiceRepeater_ItemDataBound"--%>
        <ItemTemplate>
            <div class="row">
		        <h4><EPiServer:Translate Text="/mysettings/invoice/invoicetitle" runat="server" /> <%# DataBinder.Eval(Container.DataItem, "INVNO")%></h4>
		        <a href="#" class="edit"><EPiServer:Translate Text="/common/show" runat="server" /></a>
		        <p class="description"><EPiServer:Translate Text="/mysettings/invoice/description" runat="server" /></p>
		        <p class="value"><asp:Label ID="InvoiceStatusLabel" runat="server" /></p>

		        <div id="InvoiceDiv" class="form-edit" runat="server">
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/price" runat="server" /></h5>
				        <p class="value"><%# string.Format(Translate("/mysettings/invoice/invoicelist/priceandvat" ), DataBinder.Eval(Container.DataItem, "OPENAMOUNT"),  DataBinder.Eval(Container.DataItem, "VATAMOUNT")) %></p>
			        </div>
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/period" runat="server" /></h5>

				        <p class="value"><%-- = Subscriber != null ? string.Format("{0} - {1}", Subscriber.SubStart.ToString("yyyy-MM-dd"), Subscriber.SubEnd.ToString("yyyy-MM-dd")) : string.Empty --%></p>
			        </div>
			        <div class="edit-row">
				        <h5><EPiServer:Translate Text="/mysettings/invoice/invoicelist/newspaperaddressee" runat="server" /></h5>
			        </div>							
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/address/name" runat="server" /></h5>
				        <p class="value">Subscriber.Name<%-- = Subscriber != null ? Subscriber.Name : string.Empty --%></p>

			        </div>
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/address/address" runat="server" /></h5>
				        <p class="value">Subscriber.Address<%-- = Subscriber != null ? Subscriber.Address : string.Empty --%></p>
			        </div>
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/address/careof" runat="server" /></h5>

				        <p class="value">Subscriber.Co<%-- = Subscriber != null ? Subscriber.Co : string.Empty --%></p>
			        </div>
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/address/zipcode" runat="server" /></h5>
				        <p class="value"><%= Subscriber != null ? Subscriber.Zip : string.Empty %></p>
			        </div>
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/address/city" runat="server" /></h5>

				        <p class="value"><%= Subscriber != null ? Subscriber.PostName : string.Empty %></p>
			        </div>
			        <div class="edit-row">
				        <h5><EPiServer:Translate Text="/mysettings/invoice/invoicelist/invoiceaddressee" runat="server" /></h5>
			        </div>																											
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/address/name" runat="server" /></h5>
				        <p class="value"><asp:Literal ID="AddresseeNameLiteral" runat="server" /></p>

			        </div>
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/address/address" runat="server" /></h5>
				        <p class="value"><asp:Literal ID="AddresseeAddressLiteral" runat="server" /></p>
			        </div>
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/address/careof" runat="server" /></h5>
				        <p class="value"><asp:Literal ID="AddresseeCareOfLiteral" runat="server" /></p>

			        </div>
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/address/zipcode" runat="server" /></h5>
				        <p class="value"><asp:Literal ID="AddresseeZipCodeLiteral" runat="server" /></p>
			        </div>
			        <div class="edit-row">
				        <h5 class="label"><EPiServer:Translate Text="/mysettings/invoice/invoicelist/address/city" runat="server" /></h5>
				        <p class="value"><asp:Literal ID="AddresseeCityLiteral" runat="server" /></p>
			        </div>
			        <div class="button-wrapper">
                        <asp:Button ID="PayInvoiceButton" CssClass="btn" Text="<%$ Resources: EPiServer, mysettings.invoice.invoicelist.pay %>" OnClick="PayInvoice_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OPENAMOUNT") + "|" + DataBinder.Eval(Container.DataItem, "INVAMOUNT") + "|" + DataBinder.Eval(Container.DataItem, "VATAMOUNT") + "|" + DataBinder.Eval(Container.DataItem, "INVNO") %>' runat="server" />
			        </div>											
		        </div>						
	        </div>
        </ItemTemplate>
    </asp:Repeater>


    <div class="row">
        <h4>Autodragning via kontokort</h4>
        <a href="#" class="edit"><EPiServer:Translate ID="Translate2" Text="/common/show" runat="server" /></a>
		<p class="description">För dig som betalar din prenumeration via autodragning på kontokort</p>
		<p class="value"></p>

        <div ID="AutowithdrawalDiv" class="form-edit" runat="server">

            <table border="0" cellpadding="0" cellspacing="0">
            <tr>
            <td valign="top">
                
                <asp:Literal ID="LiteralAwdTables" runat="server"></asp:Literal>

            </td>
            <td width="30">&nbsp;</td>
            <td valign="top">
                <%--<div>
                <asp:Button ID="Button1" CssClass="btn" Text="Byt kreditskortsnummer" runat="server" />
                <br />
                <br />
                <br />
                <asp:Button ID="ButtonCancelSubs" CssClass="btn" Text="Avsluta prenumeration" runat="server" onclick="ButtonCancelSubs_Click" />
                <div style="margin-top:5px;"><i>Vid avslut upphör prenumerationen efter <asp:Literal ID="LiteralSubsEndDate" runat="server" /></i></div>
                </div>--%>

                Prenumerationsärenden:<br />
                Kontakta kundtjänst på tel<br />
                08-573 651 00.

            </td>
            </tr>
            </table>

		</div>
    </div>

</div>
<!-- // Invoices -->	