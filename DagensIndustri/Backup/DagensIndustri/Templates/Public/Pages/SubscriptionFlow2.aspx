<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="SubscriptionFlow2.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SubscriptionFlow2" %>
<%@ Register src="../Units/Placeable/SideBarBoxes/OnlineSupportBox.ascx" tagname="OnlineSupportBox" tagprefix="uc1" %>


<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    
    <script type="text/javascript">
        $(document).ready(function () {
            $(".bsbtn").click(function () {
                $(this).addClass("disabled");
                $(this).html("Laddar steg 2");
            });
            $("#prenmatrix .paperlist p").click(function () {
                //Only track on show
                if (!$(this).next().is(":visible")) {
                    _gaq.push(['_trackEvent', 'Readmore', $(this).find('.hd').text(), 'PrenMatrix']);
                }
                $(this).next().slideToggle('fast');
            });
            $("#prenmatrix .paperlist .closex").click(function () {
                $(this).parent().slideUp('fast');
            });
        });
    </script>

</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <div class="progresstracker">
        <div class="step active">                        
            <div class="stepcontent">
                <span class="steptext">1. Välj erbjudande</span>
                <div class="arrow-right"></div>                                             
            </div>                        
        </div>
        <div class="step">                        
            <div class="stepcontent">
                <div class="arrow-in"></div>
                <span class="steptext">2. Dina uppgifter</span>
                <div class="arrow-right"></div>
            </div>                        
        </div>
        <div class="step">                                                                      
            <div class="stepcontent">
                <div class="arrow-in"></div>
                <span class="steptext">3. Klar</span>
            </div>                                              
        </div>
    </div>

    <EPiServer:PageList runat="server" ID="PlCampaigns">
        <HeaderTemplate><div id="prenmatrix"></HeaderTemplate>
        <ItemTemplate>
            <div class="matrix_col">
                <img src='<%# Container.CurrentPage["Campaign1TopImage"] %>' alt='<%# Container.CurrentPage["Campaign1PrimHeading1"] %>' />
                <div class="col_w">                    
                    <div class="centertext">
                        <div class="cheading"><EPiServer:Property runat="server" PropertyName="Campaign1Heading" DisplayMissingMessage="false" /></div>
                        <div class="price"><EPiServer:Property runat="server" PropertyName="Campaign1Price" /></div>                        
                        <a href='<%# EPiServer.UriSupport.AddQueryString(Container.CurrentPage.LinkURL, "pm", "true") %>' class="bsbtn btn-large btn-success">GÅ VIDARE TILL STEG 2</a>
                    </div>

                    <div class="paperheading"><EPiServer:Property runat="server" PropertyName="Campaign1SideBarMainHeading" /></div>
                    <EPiServer:PageList runat="server" DataSource="<%#GetPapers(Container.CurrentPage) %>">
                        <HeaderTemplate><div class="paperlist"></HeaderTemplate>
                        <ItemTemplate> 
                            <div class="pc">
                                <p>
                                    <span class="hd"><%# Container.CurrentPage.Property["CampaignSideBarHeading"] %></span> <small class="date"><%# Container.CurrentPage.Property["CampaignSideBarWeekdays"]%></small>
                                </p> 
                                <div class="readmoretext" style="display:none;">
                                    <div class="closex">&#10006;</div>		         		    
		                            <div class="rmt"><%# Container.CurrentPage.Property["CampaignSideBarDescription"]%></div>
	                            </div>  
                            </div>
                        </ItemTemplate>
                        <FooterTemplate></div></FooterTemplate>
                    </EPiServer:PageList>                               
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </EPiServer:PageList>

    <div class="text">
        <div class="t_col">
            <h4><EPiServer:Property runat="server" PropertyName="FooterLeftTitle" /></h4>
            <EPiServer:Property runat="server" PropertyName="FooterLeftText" />
            
            <h4><EPiServer:Property ID="Property1" runat="server" PropertyName="FooterRightTitle" /></h4>
            <EPiServer:Property ID="Property2" runat="server" PropertyName="FooterRightText" />
        </div>
        <div class="t_col">
            <h4><EPiServer:Property runat="server" PropertyName="FooterMidTitle" /></h4>
            <EPiServer:Property runat="server" PropertyName="FooterMidText" />
        </div>
        <div class="t_col">
            <%--<h4><EPiServer:Property runat="server" PropertyName="FooterRightTitle" /></h4>
            <EPiServer:Property runat="server" PropertyName="FooterRightText" />--%>
            
            <uc1:OnlineSupportBox ID="OnlineSupportBox1" runat="server" Visible="False" />

        </div>
    </div>
</asp:Content>