<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="CampaignPaperList.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.bscampaign.CampaignPaperList" %>

<%@ Register TagPrefix="di" TagName="papersidebar" Src="~/Templates/Public/Pages/bscampaign/CampaignPaperSideBar.ascx" %>

<script type="text/javascript">

    $(document).ready(function () {

        $(".diProductImg").click(function () {
            $(this).siblings(".readmore").click();
        });

        //Attach slideToggle to papers readmore
        $("#PaperList .paper .readmore").click(function () {
            var val = $(this).children("span").text();
            //Only track on show
            if (val == "+") {
                Track('Readmore', $(this).prevAll('.heading').find('.size3').text(), 'Step1')
            }
            $(this).children("span").html(val == "+" ? "−" : "+");
            $(this).prev().slideToggle('fast');
        });

        //Attach slideToggle to sidebar paper list
        $(".sidebarpapers.visible-phone p").click(function () {
            //Only track on show
            if (!$(this).next().is(":visible")) {
                Track('Readmore', $(this).find('.hd').text(), 'Step1');
            }
            $(this).next().slideToggle('fast');
        });

        $(".sidebarpapers.visible-phone .closex").click(function () {
            $(this).parent().slideUp('fast');
        });
    });

</script>

<div class="sidebarpapers visible-phone">
    <EPiServer:PageList ID="PlSideBar" runat="server">
        <HeaderTemplate>
            <div class="size4"><%= CurrentPage[Campaign + "SideBarMainHeading"]%></div>                                            
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
</div>

<div class="hidden-phone">
    <EPiServer:PageList ID="PlPaperList" runat="server">
        <HeaderTemplate>
            <div class="borderbottom">
	            <div class="size2"><%= CurrentPage[Campaign + "SideBarMainHeading"] %></div>                                                            
            </div>
            <div class="row">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="span3">            
                <div class="paper">
                    
                    <div class="heading">
                        <span class="size3"><%# Container.CurrentPage.Property["CampaignSideBarHeading"] %></span> 
                        <small class="date"><%# Container.CurrentPage.Property["CampaignSideBarWeekdays"]%></small>
                    </div>
                    
                    <img class="diProductImg" style="cursor:pointer;" src='<%# Container.CurrentPage.Property["CampaignSideBarImage"] %>' alt="<%# Container.CurrentPage.PageName %>"  /> 
                    
                    <div class="text white" style="display:none;">		         		    
		                <%# Container.CurrentPage.Property["CampaignSideBarDescription"]%>    
	                </div>   
                    
                    <div class="readmore white">
                        <strong>Läs mer</strong>
                        <span>+</span>
                    </div>                           

                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </EPiServer:PageList>
</div>