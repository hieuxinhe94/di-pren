<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CampaignSidebar.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.DiCampaign.CampaignSidebar" %>

<div id="campaign-package" class="small"> 
		
	<h2><%= CurrentPage.Property["CampaignSideBarMainHeading"] %></h2> 
	
    <script language="javascript" type="text/javascript">
        function JsShowHideText(id) {
            if (document.getElementById(id).style.visibility == 'hidden' || document.getElementById(id).style.display == 'none') {
                document.getElementById(id).style.visibility = 'visible';
                document.getElementById(id).style.display = 'block';
            }
            else {
                document.getElementById(id).style.visibility = 'hidden';
                document.getElementById(id).style.display = 'none';
            }
        }
    </script>

    <EPiServer:PageList ID="CampaignSideBarPageList" runat="server">
        <HeaderTemplate> 
        </HeaderTemplate>
        
        <ItemTemplate>
            <asp:PlaceHolder ID="PaperPlaceHolder" Visible='<%# EPiFunctions.HasValue(Container.CurrentPage, "CampaignSideBarIsBanner") ? false : true %>' runat="server">
                <div class="campaign-paper"> 
		            <img src='<%# Container.CurrentPage.Property["CampaignSideBarImage"] %>' class="no-shadow" alt="" /> 
		            <h3><%# Container.CurrentPage.Property["CampaignSideBarHeading"] %>
		            <small class="date"><%# Container.CurrentPage.Property["CampaignSideBarWeekdays"]%></small></h3>  
		            <p><%# Container.CurrentPage.Property["CampaignSideBarDescription"]%></p>
                    
                    <asp:PlaceHolder ID="PlaceHolderMoreInfo" Visible='<%# !string.IsNullOrEmpty(Container.CurrentPage.Property["CampaignSideBarMetaText"].ToString()) %>' runat="server">
                        <p>
                            <a href="javascript:void(0);" onclick="JsShowHideText('a_<%# Container.CurrentPage.PageLink.ID %>');"><EPiServer:Translate ID="Translate1" runat="server" text="/dicampaign/campaignsidebar/readmore" /></a>
                            <div id="a_<%# Container.CurrentPage.PageLink.ID %>" style="display:none; visibility:hidden;">
                                <p><%# Container.CurrentPage.Property["CampaignSideBarMetaText"]%></p>
                            </div>
                        </p>
                    </asp:PlaceHolder>

	            </div>			
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="BannerPlaceHolder" Visible='<%# EPiFunctions.HasValue(Container.CurrentPage, "CampaignSideBarIsBanner") %>' runat="server">
        	    <img src="/templates/public/images/images-campaign/diguld.png" alt="Du får även tillgång till Di Guld" />
            </asp:PlaceHolder>
        </ItemTemplate>
        
        <FooterTemplate>
        
        </FooterTemplate>
    </EPiServer:PageList>
</div>