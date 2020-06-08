<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Static.Footer" %>

<!-- Footer DagensIdustri -->
<div id="footer">
	<div class="content">
        <asp:PlaceHolder ID="DIGoldFooterFirstColumn" Visible="false" runat="server">
            <!-- DagensIndustri.se -->
		    <div class="section di">
			    <h5><a href='<%= EPiFunctions.StartPage().LinkURL %>'><%= EPiFunctions.StartPage().PageName %></a></h5>
                <EPiServer:PageList ID="DagensIndustriMenu" runat="server">
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
		    <!-- // DagensIndustri.se -->
        </asp:PlaceHolder>	

        <asp:PlaceHolder ID="FooterLinksPlaceHolder" Visible="false" runat="server">
		    <!-- Links -->
		    <div class="section links">
                <EPiServer:PageList ID="FooterLinks" runat="server">
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
        </asp:PlaceHolder>
        
        <asp:PlaceHolder ID="DiGoldFooterLinksPlaceHolder" Visible="false" runat="server">
            <!-- Di Gold Links -->
		    <div class="section links">
                <EPiServer:PageList ID="DiGoldFooterLinks" runat="server">
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
		    <!-- // Di Gold Links -->
        </asp:PlaceHolder>		

		<!-- Customer service -->
		<div class="section support">
	        <asp:Literal ID="CustomerHelp" runat="server" />
        </div>
		<!-- // Customer service -->
		
        <asp:PlaceHolder ID="DagensIndustryFooterAd" Visible="false" runat="server">
		    <!-- Banner advertising -->
		    <div class="section advertising">
                <asp:Literal ID="DagensIndustriRightColumn" runat="server" />
            </div>
		    <!-- // Banner advertising -->
        </asp:PlaceHolder> 	
        
        <asp:PlaceHolder ID="DIGoldFooterLogo" Visible="false" runat="server">
            <!-- Back to DagensIndustri -->
                <asp:Literal ID="DIGoldRightColumn" runat="server" />
		    <!-- // Back to DagensIndustri -->
        </asp:PlaceHolder>			
	</div>
</div>
<!--// Footer DagensIdustri -->