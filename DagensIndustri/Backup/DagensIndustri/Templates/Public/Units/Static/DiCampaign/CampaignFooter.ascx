<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CampaignFooter.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Static.DiCampaign.CampaignFooter" %>

<div id="footer"> 
	<div class="content"> 
		
	<!-- Links --> 
		<div class="section links">
            <EPiServer:PageList ID="CampaignFooterLinksPageList" runat="server">
			    <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><a href='<%# Container.CurrentPage.LinkURL %>'><%# Container.CurrentPage.PageName %></a></li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </EPiServer:PageList> 
		</div> 
		<!-- // Links -->		
 
		<!-- Disclaimer --> 
		<div class="section disclaimer"> 
			<asp:Literal ID="CampaignFooterTextLiteral" runat="server" />
		</div> 
		<!-- // Disclaimer --> 
		
	</div> 
</div> 