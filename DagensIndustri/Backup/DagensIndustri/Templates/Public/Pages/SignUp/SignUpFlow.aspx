<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUpFlow.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SignUp.SignUpFlow" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="participants" Src="~/Templates/Public/Units/Placeable/SignUpFlow/RegisterParticipants.ascx" %>
<%@ Register TagPrefix="di" TagName="placeorder" Src="~/Templates/Public/Units/Placeable/SignUpFlow/PlaceOrder.ascx" %>
<%@ Register TagPrefix="di" TagName="stepbox" Src="~/Templates/Public/Units/Placeable/SignUpFlow/SideBarStepBox.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" ID="CphHeader" runat="server">
	<!-- Page header -->
        <%= GetHeaderForStep() %>
	<!-- // Page header -->
</asp:Content>

<asp:Content ID="MainContentPlaceHolder" EnableViewState="true" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
  
    <!-- Errors -->
    <di:UserMessage ID="UserMessageControl" runat="server" />
	<!-- // Errors -->

    <asp:MultiView ID="FlowMultiView" runat="server">
    </asp:MultiView>

    <div class="button-wrapper">
        <asp:LinkButton CssClass="back" Text='<%$ Resources: EPiServer,  signup.backbutton %>' OnClick="BackButtonLinkButton_Click" ID="BackButtonLinkButton" runat="server" />
        <asp:Button ID="NextButton" Text='<%$ Resources: EPiServer,  signup.nextbutton %>' CssClass="btn" OnClick="NextButton_Click" runat="server" /> 
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">  
    <asp:PlaceHolder ID="StepBoxPlaceHolder" runat="server">
    </asp:PlaceHolder>
</asp:Content>
