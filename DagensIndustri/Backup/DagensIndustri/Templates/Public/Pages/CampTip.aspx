<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="CampTip.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.CampTip" %>
<%@ MasterType virtualpath="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register Tagprefix="uc1" Tagname="PersonForm" src="~/Templates/Public/Units/Placeable/CampTip/PersonForm.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ID="MainContentPlaceHolder1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:UserMessage ID="UserMessageControl" runat="server" />
    
    <di:mainintro ID="Mainintro1" runat="server" />
    <di:mainbody ID="Mainbody1" runat="server" />


    <asp:PlaceHolder ID="PlaceHolderForm" runat="server">
        <%--<p><asp:Label ID="LabelFormHeader" runat="server" Text="Label"></asp:Label></p>--%>
        
        <uc1:PersonForm ID="p1" Visible="false" runat="server" />
        <uc1:PersonForm ID="p2" Visible="false" runat="server" />
        <uc1:PersonForm ID="p3" Visible="false" runat="server" />
        <uc1:PersonForm ID="p4" Visible="false" runat="server" />
        <uc1:PersonForm ID="p5" Visible="false" runat="server" />
        <uc1:PersonForm ID="p6" Visible="false" runat="server" />
        <uc1:PersonForm ID="p7" Visible="false" runat="server" />
        <uc1:PersonForm ID="p8" Visible="false" runat="server" />
        <uc1:PersonForm ID="p9" Visible="false" runat="server" />
        <uc1:PersonForm ID="p10" Visible="false" runat="server" />

        <div class="button-wrapper">
            <asp:Button ID="ButtonSave" CssClass="btn" Text="Skicka"  OnClick="ButtonSave_Click" runat="server" />
	    </div>

    </asp:PlaceHolder>

</asp:Content>

<%--<asp:Content ID="Content8" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
</asp:Content>--%>
