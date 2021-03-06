﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GasellFlow.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiGasell.GasellFlow" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="participants" Src="~/Templates/Public/Units/Placeable/GasellFlow/RegisterParticipants.ascx" %>
<%@ Register TagPrefix="di" TagName="placeorder" Src="~/Templates/Public/Units/Placeable/GasellFlow/PlaceOrder.ascx" %>
<%@ Register TagPrefix="di" TagName="stepbox" Src="~/Templates/Public/Units/Placeable/GasellFlow/SideBarStepBox.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
	<!-- Page header -->
	<h1>
        <%= GetHeaderForStep() %>
    </h1>
	<!-- // Page header -->
</asp:Content>

<asp:Content ID="MainContentPlaceHolder" EnableViewState="true" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <!-- Errors -->
    <di:UserMessage ID="UserMessageControl" runat="server" />
	<!-- // Errors -->

    <asp:MultiView ID="FlowMultiView" runat="server">
    </asp:MultiView>

    <div class="button-wrapper">
        <asp:LinkButton CssClass="back" Text='<%$ Resources: EPiServer,  gasell.backbutton %>' OnClick="BackButtonLinkButton_Click" ID="BackButtonLinkButton" runat="server" />
        <asp:Button ID="NextButton" Text='<%$ Resources: EPiServer,  gasell.nextbutton %>' CssClass="btn" OnClick="NextButton_Click" runat="server" /> 
    </div>
    
<script type="text/javascript">
  $(document).ready(function () {
    _gaq.push(['_trackPageview', '<%=GoogleVirtualUrl%>']);
  });
</script>
</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">  
    <asp:PlaceHolder ID="StepBoxPlaceHolder" runat="server">
    </asp:PlaceHolder>
</asp:Content>
