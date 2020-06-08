<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Bookshelf.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Bookshelf" %>
<%@ Import Namespace="DagensIndustri.Tools.Classes.Shop" %>

<asp:PlaceHolder ID="BookShelfPlaceHolder" runat="server">
    <!-- Bookshelf -->			
    <div class="bookshelf banner">
	    <div class="books">
    
            <asp:Repeater ID="BookShelfRepeater" runat="server">
                <HeaderTemplate>
                    <ul>                
                </HeaderTemplate>
                <ItemTemplate>
                    <!--Book -->
			        <li>
                        <asp:PlaceHolder ID="ImagePlaceHolder" Visible="<%# !string.IsNullOrEmpty(((Product)Container.DataItem).ImageUrl) %>" runat="server">
				            <div class="image">
					            <img src="<%# ((Product)Container.DataItem).ImageUrl %>" alt="<%# ((Product)Container.DataItem).Heading %>" />
				            </div>
                        </asp:PlaceHolder>

				        <h3>
                            <asp:HyperLink NavigateUrl="<%# ((Product)Container.DataItem).ProductPageUrl %>" Text="<%# ((Product)Container.DataItem).Heading %>" runat="server" />                        
                        </h3>

				        <p class="author"><%# ((Product)Container.DataItem).Subtitle %></p>
				        <p class="price"><%# ((Product)Container.DataItem).PriceWithCurrency %></p>
				    
                        <a href="<%# ((Product)Container.DataItem).ProductPageUrl %>" class="btn newwin">
                            <span><EPiServer:Translate Text="/common/buy" runat="server" /></span>
                        </a>
			        </li>
			        <!--// Book -->
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>

	    </div>

	    <ul class="nav">
		    <li class="prev"><span>Föregående</span></li>
		    <li class="next"><span>Nästa</span></li>
	    </ul>
    </div>
    <!-- // Bookshelf -->
</asp:PlaceHolder>