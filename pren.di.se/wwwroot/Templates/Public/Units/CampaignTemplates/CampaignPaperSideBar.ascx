<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CampaignPaperSideBar.ascx.cs" Inherits="PrenDiSe.Templates.Public.Units.CampaignTemplates.CampaignPaperSideBar" %>
<%@ Import Namespace="PrenDiSe.Tools.Classes" %>



<EPiServer:PageList ID="PlSideBar" runat="server">
    <HeaderTemplate>
        <div class="size4"><%= CurrentPage[PropertyPrefix + "SideBarMainHeading"]%></div>                                            
    </HeaderTemplate>
    <ItemTemplate>
        <asp:PlaceHolder Visible='<%# EPiFunctions.HasValue(Container.CurrentPage, "CampaignSideBarIsBanner") ? false : true %>' runat="server">
            <p>
                <span class="hd"><%# Container.CurrentPage.Property["CampaignSideBarHeading"] %> </span>
                <small class="date"><%# Container.CurrentPage.Property["CampaignSideBarWeekdays"]%></small>
            </p>
            <div class="readmoretext" style="display:none;">
                <div class="closex">&#10006;</div>		         		    
		        <div class="rmt"><%# Container.CurrentPage.Property["CampaignSideBarDescription"]%></div>
	        </div> 
        </asp:PlaceHolder>
    </ItemTemplate>
</EPiServer:PageList>