<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Login" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%--<%@ Register TagPrefix="di" TagName="LoginControl" Src="~/Templates/Public/Units/Placeable/Login/LoginControl.ascx" %>--%>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">    
    <di:Heading ID="HeadingControl" runat="server"/>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:UserMessage ID="UserMessageControl" runat="server" />
   
    <!-- Page primary content goes here -->
    <div class="form-box login">
        <asp:LoginView ID="LoginView" runat="server">
            <AnonymousTemplate>
                [LoginControl]
                <%--<di:LoginControl ID="LoginCtrl" ShowForgotPassword="true" runat="server" />--%>
            </AnonymousTemplate>
        </asp:LoginView>
    </div>  					
	<!-- // Page primary content goes here -->
</asp:Content>
