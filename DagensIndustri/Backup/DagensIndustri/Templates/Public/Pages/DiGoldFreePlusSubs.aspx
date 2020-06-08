<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="DiGoldFreePlusSubs.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiGoldFreePlusSubs" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarboxlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content  ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    

    <!-- Page primary content goes here -->
    <di:mainintro ID="Mainintro1" runat="server" />
    <di:mainbody ID="Mainbody1" runat="server" />
    <!-- // Page primary content goes here -->


    <asp:Panel ID="PanelThanks" runat="server">
        <p><%= Translate("/freeplussubs/thankyou")%></p> 
    </asp:Panel>

    
    <!-- Errors -->
    <di:UserMessage ID="UserMessageControl" runat="server" />
    <!-- // Errors -->
        
            
    <%--<p>--%>
        <%--<asp:HyperLink ID="HyperLinkLogin" runat="server"><%= Translate("/freeplussubs/clickhere")%></asp:HyperLink>--%>
        <%--<asp:LinkButton ID="LinkButtonActivateDiPlus" runat="server" onclick="LinkButtonActivateDiPlus_Click"><%= Translate("/freeplussubs/clickhere") %></asp:LinkButton>--%>
    <%--</p>--%>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:sidebarboxlist ID="Sidebarboxlist1" runat="server" />
</asp:Content>