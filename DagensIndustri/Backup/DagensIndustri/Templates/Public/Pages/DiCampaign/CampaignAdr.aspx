<%@ Page Title="" Language="C#" MasterPageFile="~/templates/public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="CampaignAdr.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiCampaign.CampaignAdr" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>


<%--<di:sidebar runat="server" />--%>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="NavigationPlaceHolder" runat="server">
</asp:Content>--%>


<asp:Content ID="Content2" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:mainintro ID="Mainintro1" runat="server" />
    <di:mainbody ID="Mainbody1" runat="server" />	

    <p style="margin-bottom:0px;">Ange kod</p>
    <asp:TextBox ID="TextBoxCode" runat="server"></asp:TextBox>
    <div style="margin-top:5px;">
        <asp:button ID="ButtonRedir" runat="server" text="Ta del av erbjudandet" onclick="ButtonRedir_Click" />
    </div>
    
    <di:UserMessage ID="UserMessageControl" runat="server" />
</asp:Content>


<%--<asp:Content ID="Content1" ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server">
    WideMainContentPlaceHolder
    <di:UserMessage ID="UserMessage1" runat="server" />
</asp:Content>--%>

<%--<asp:Content ID="Content4" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <di:footer ID="Footer1" runat="server" />
</asp:Content>--%>




    
