<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CampaignForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiCampaign.CampaignForm" %>
<%@ Register TagPrefix="di" TagName="streetform" Src="~/Templates/Public/Units/Placeable/DiCampaign/CampaignStreetForm.ascx" %>
<%@ Register TagPrefix="di" TagName="boxform" Src="~/Templates/Public/Units/Placeable/DiCampaign/CampaignBoxForm.ascx" %>
<%@ Register TagPrefix="DI" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>


<div id="content"> 
	<h2>
        <asp:Literal ID="AnswerCardLiteral" runat="server" />   
    </h2>

    <div class="campaign-intro">
        <%=(string)CurrentPage["CampaignIntroHtml"]%>
    </div>

	<h3 class="ill-checkbox">
        <asp:Literal ID="AnswerCheckBoxLiteral" runat="server" />
    </h3>
			
    <!-- Errors -->
    <DI:UserMessage ID="UserMessageControl" runat="server" />
    <!-- // Errors -->

	<div class="form-nav">
  	    <ul>
  		    <li class="current">
                <a href="#form-street">
                    <asp:Literal ID="LiteralTabHeaderStreet" runat="server"></asp:Literal>
                    <%--<%= Translate("/dicampaign/tabs/street") %>--%>
                </a>
            </li>

  		    <asp:PlaceHolder ID="PlaceHolder1" Visible='<%#!IsDigitalCampaign%>' runat="server">
                <li>
                    <a href="#form-box"><%= Translate("/dicampaign/tabs/box") %></a>
                </li>						
            </asp:PlaceHolder>

  	    </ul>
  	    <p class="required"><%= Translate("/dicampaign/forms/mandatoryinformation")%></p>
  	</div>
  		
	<div class="form-box">    				
        <di:streetform ID="StreetForm" runat="server" />
        <di:boxform ID="BoxForm" runat="server" />
  	</div>
</div>