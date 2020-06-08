<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Autowithdrawal.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.Autowithdrawal" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="MySettingsMenu" src="~/Templates/Public/Pages/UserSettings/MySettingsMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <%--<di:heading ID="Heading1" runat="server" />--%>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="TopContentPlaceHolder" runat="server" ></asp:Content>


<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:MySettingsMenu ID="MySettingsMenu1" runat="server" />

    <h2><%= CurrentPage["Heading"] ?? CurrentPage.PageName %></h2>
    <di:Mainbody ID="Mainbody1" runat="server" />

    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:Button ID="ButtonQuitAwd" CssClass="btn" Text="Avsluta prenumeration" OnClick="ButtonQuitAwd_Click" runat="server" />

    <br />

    <asp:Repeater ID="PaymentRepeater" runat="server">
        <HeaderTemplate>
            <p>
                <b>Betalningshistorik</b><br />
        </HeaderTemplate>

        <ItemTemplate>
            Datum: <%# DataBinder.Eval(Container.DataItem, "Purchase_date")%><br />
			Pris inkl moms: <%# DataBinder.Eval(Container.DataItem, "Amount").ToString() %> kr<br />
            Moms: <%# DataBinder.Eval(Container.DataItem, "VAT").ToString() %> kr<br />
            Betalningsstatus: <i><%# DataBinder.Eval(Container.DataItem, "PaymentStatus")%></i>
            <br />
            <br />
        </ItemTemplate>

        <FooterTemplate>
            </p>
        </FooterTemplate>
    </asp:Repeater>

</asp:Content>
