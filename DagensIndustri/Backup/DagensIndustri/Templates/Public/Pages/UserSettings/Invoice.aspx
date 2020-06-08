<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.Invoice" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="MySettingsMenu" src="~/Templates/Public/Pages/UserSettings/MySettingsMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>


<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server" ></asp:Content>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:MySettingsMenu ID="MySettingsMenu1" runat="server" />

    <h2><%= CurrentPage["Heading"] ?? CurrentPage.PageName %></h2>
    <di:Mainbody ID="Mainbody1" runat="server" />

    <di:UserMessage ID="UserMessageControl" runat="server" />

   
    <asp:PlaceHolder ID="MessagePlaceHolder" Visible="false" runat="server" >
        <div class="row">
            <p class="description"><b><EPiServer:Translate ID="Translate1" Text="/mysettings/invoice/nounpaid" runat="server" /></b></p>
        </div>
    </asp:PlaceHolder>


    <asp:PlaceHolder ID="SearchResultPlaceHolder" Visible="false" runat="server">
		<div class="row"> <!--class="row" searchlist-->
            <asp:Repeater ID="InvoiceRepeater" runat="server">
                <HeaderTemplate>
                </HeaderTemplate>

                <ItemTemplate>
                    <div class="form-box">
                        <div class="row">
                        <p>
						    <EPiServer:Translate ID="Translate1" Text="/payinvoice/invoicenumber" runat="server" />
						    <%# DataBinder.Eval(Container.DataItem, "INVNO")%><br />
						    <EPiServer:Translate ID="Translate2" Text="/payinvoice/topay" runat="server" />
						    <%# string.Format(Translate("/mysettings/invoice/invoicelist/priceandvat" ), DataBinder.Eval(Container.DataItem, "OPENAMOUNT"),  DataBinder.Eval(Container.DataItem, "VATAMOUNT")) %>
                        </p>
                        </div>
					    <div class="button-wrapper">
                            <asp:LinkButton ID="PayInvoiceLinkButton" CssClass="btn" OnClick="PayInvoiceLinkButton_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OPENAMOUNT") + "|" + DataBinder.Eval(Container.DataItem, "INVAMOUNT") + "|" + DataBinder.Eval(Container.DataItem, "VATAMOUNT") + "|" + DataBinder.Eval(Container.DataItem, "INVNO") + "|" + Subscriber.Cusno + "|" + Subscriber.Email %>' runat="server">
                                <span><EPiServer:Translate ID="Translate3" Text="Betala med kort" runat="server" /></span>
                            </asp:LinkButton>
                        </div>
                        <br />
                    </div>
                </ItemTemplate>

                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
		</div>
	</asp:PlaceHolder>


</asp:Content>
