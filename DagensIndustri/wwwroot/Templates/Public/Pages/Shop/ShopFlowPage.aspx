<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShopFlowPage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Shop.ShopFlowPage" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="placeorder" Src="~/Templates/Public/Units/Placeable/ShopFlow/ShopPlaceOrder.ascx" %>
<%@ Register TagPrefix="di" TagName="paywithcard" Src="~/Templates/Public/Units/Placeable/ShopFlow/ShopPayWithCreditCard.ascx" %>
<%@ Register TagPrefix="di" TagName="paywithinvoice" Src="~/Templates/Public/Units/Placeable/ShopFlow/ShopPayWithInvoice.ascx" %>
<%@ Register TagPrefix="di" TagName="receipt" Src="~/Templates/Public/Units/Placeable/ShopFlow/ShopReceipt.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebar" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:MultiView ID="ShopFlowMultiview" runat="server">
        <asp:View ID="PlaceOrderView" runat="server">
            <di:placeorder runat="server" />
        </asp:View>
        <asp:View ID="PayWithCardView" runat="server">
            <di:paywithcard runat="server" />
        </asp:View>
        <asp:View ID="InvoiceCardView" runat="server">
            <di:paywithinvoice runat="server" />
        </asp:View>
        <asp:View ID="ReceiptView" runat="server">
            <di:receipt runat="server" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:sidebar runat="server" />
</asp:Content>