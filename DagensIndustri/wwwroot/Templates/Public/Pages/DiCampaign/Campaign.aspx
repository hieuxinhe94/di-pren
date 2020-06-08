<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/templates/public/MasterPages/MasterPage.Master" CodeBehind="Campaign.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiCampaign.Campaign" %>
<%@ Register TagPrefix="di" TagName="sidebar" Src="~/Templates/Public/Units/Placeable/DiCampaign/CampaignSidebar.ascx" %>
<%@ Register TagPrefix="di" TagName="form" Src="~/Templates/Public/Units/Placeable/DiCampaign/CampaignForm.ascx" %>
<%@ Register TagPrefix="di" TagName="otherpayerform" Src="~/Templates/Public/Units/Placeable/DiCampaign/CampaignOtherPayerForm.ascx" %>
<%@ Register TagPrefix="di" TagName="footer" Src="~/Templates/Public/Units/Static/DiCampaign/CampaignFooter.ascx" %>
<%@ Register TagPrefix="di" TagName="thankyou" Src="~/Templates/Public/Units/Placeable/DiCampaign/CampaignThankYou.ascx" %>
<%--<%@ Register TagPrefix="di" TagName="codeform" Src="~/Templates/Public/Units/Placeable/DiCampaign/CampaignCodeForm.ascx" %>--%>
<%--<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>--%>


<asp:Content ContentPlaceHolderID="NavigationPlaceHolder" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server">

    <di:sidebar runat="server" />

    <%--<asp:PlaceHolder ID="CodeFormPlaceHolder" Visible="false" runat="server">
	    <di:codeform runat="server" />
    </asp:PlaceHolder>--%>

    <%--<div id="content">
        <di:UserMessage ID="UserMessageControl" runat="server" />
    </div>--%>

    <asp:Label ID="LabelErr" Visible="false" style="padding-left:20px; float:left;" runat="server"></asp:Label>
    

    <asp:PlaceHolder ID="MainFormPlaceHolder" Visible="false" runat="server">
        <di:form ID="CampForm" runat="server" />	 
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="OtherPayerPlaceHolder" Visible="false" runat="server">
        <di:otherpayerform ID="OtherPayerForm" runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="ThankYouPlaceholder" runat="server" Visible="false">
        <di:thankyou ID="CampThankYou" runat="server" />
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <di:footer runat="server" />

    <asp:Literal ID="LiteralAdWordsScriptOnLoad" Visible="false" runat="server">
    </asp:Literal>

    <asp:Literal ID="LiteralAdWordsScriptOnThankYou" Visible="false" runat="server">
    </asp:Literal>
</asp:Content>