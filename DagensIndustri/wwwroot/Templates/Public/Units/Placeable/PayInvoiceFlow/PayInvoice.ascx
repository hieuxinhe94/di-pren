<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayInvoice.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.PayInvoiceFlow.PayInvoice" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="form-nav">   
  	<ul> 
		<li class="current"><a href="#search"><EPiServer:Translate Text="/payinvoice/search" runat="server" /></a></li> 
  	</ul>   		
</div>

<div class="form-box">

	<!-- Invoice search -->
	<div class="section" id="invoice-search">
		<div class="row">
            <di:Input ID="CustomerNumberInput" Name="searchstring" TypeOfInput="Numeric" AutoComplete="true" CssClass="text medium" Title="<%$ Resources: EPiServer, payinvoice.customernumber %>" runat="server" />
            <asp:Button ID="GetInvoiceButton" Text="<%$ Resources: EPiserver, payinvoice.getinvoice %>" OnClick="GetInvoiceButton_Click" runat="server" />
		</div>
		
        <asp:PlaceHolder ID="MessagePlaceHolder" Visible="false" runat="server" >
            <div class="row">
                <h3>
				    <EPiServer:Translate Text="/payinvoice/noinvoicesfound" runat="server" />
			    </h3>
            </div>
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="SearchResultPlaceHolder" Visible="false" runat="server">
		    <div class="row searchlist">
			    <h4>
				    <EPiServer:Translate Text="/payinvoice/searchresult" runat="server" />
                    <span class="n"><asp:Literal ID="SearchResultLiteral" runat="server" /></span>
			    </h4>

                <asp:Repeater ID="InvoiceRepeater" runat="server">
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <li>
					        <dl>
						        <dt><EPiServer:Translate ID="Translate1" Text="/payinvoice/invoicenumber" runat="server" /></dt>
						        <dd><%# DataBinder.Eval(Container.DataItem, "INVNO")%></dd>
						        <dt><EPiServer:Translate ID="Translate2" Text="/payinvoice/topay" runat="server" /></dt>
						        <dd><%# string.Format(Translate("/mysettings/invoice/invoicelist/priceandvat" ), DataBinder.Eval(Container.DataItem, "OPENAMOUNT"),  DataBinder.Eval(Container.DataItem, "VATAMOUNT")) %></dd>
					        </dl>

					        <asp:LinkButton ID="PayInvoiceLinkButton" CssClass="btn" OnClick="PayInvoiceLinkButton_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OPENAMOUNT") + "|" + DataBinder.Eval(Container.DataItem, "INVAMOUNT") + "|" + DataBinder.Eval(Container.DataItem, "VATAMOUNT") + "|" + DataBinder.Eval(Container.DataItem, "INVNO") + "|" + Subscriber.Cusno + "|" + Subscriber.Email %>' runat="server">
                                <span><EPiServer:Translate ID="Translate3" Text="/payinvoice/pay" runat="server" /></span>
                            </asp:LinkButton>
				        </li>                      
                    </ItemTemplate>

                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
		    </div>
		</asp:PlaceHolder>
	</div>
	<!-- // Invoice search -->		
</div>