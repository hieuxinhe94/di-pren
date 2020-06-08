<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="PayInvoiceFlow.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.PayInvoiceFlow" %>
<%@ Register TagPrefix="di" TagName="MainIntro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="MainBody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="PayInvoice" Src="~/Templates/Public/Units/Placeable/PayInvoiceFlow/PayInvoice.ascx" %>
<%@ Register TagPrefix="di" TagName="InvoiceReceipt" Src="~/Templates/Public/Units/Placeable/PayInvoiceFlow/InvoiceReceipt.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <h1>
        <%= GetHeading() %>
    </h1>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:MainIntro ID="MainIntroControl" Visible="false" runat="server"/>
    <di:MainBody ID="MainBodyControl" Visible="false" runat="server" />
    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:MultiView ID="PayInvoiceMultiView" runat="server">
        <asp:View ID="PayInvoiceView" runat="server">
            <di:PayInvoice ID="PayInvoiceControl" runat="server" />
        </asp:View>

        <asp:View ID="ReceiptView" runat="server">
            <di:InvoiceReceipt ID="InvoiceReceiptControl" runat="server" />
        </asp:View>
    </asp:MultiView>

    <div class="button-wrapper">
        <asp:LinkButton ID="BackLinkButton" CssClass="more back" Text="<%$ Resources: EPiServer, common.back %>" OnClick="BackLinkButton_Click" Visible="false" runat="server" />
    </div>
</asp:Content>
