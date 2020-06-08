<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopProductList.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Shop.ShopProductList" %>
<%@ Register TagPrefix="di" TagName="Paging" Src="~/Templates/Public/Units/Placeable/Paging.ascx" %>

<%@ Import Namespace="DagensIndustri.Tools.Classes.Shop" %>

<div id="content" class="wide">
    <asp:Repeater ID="ProductsRepeater" runat="server">
        <HeaderTemplate>
            <div class="product-list">
        </HeaderTemplate>

        <ItemTemplate>
            <!-- A product -->
		    <div class="product">                
			    <img src='<%# ((Product)Container.DataItem).ImageUrl %>' alt='<%# ((Product)Container.DataItem).ImageDescription %>' />
			    <p class="category"><%# ((Product)Container.DataItem).ProductCategory %></p>
			    
                <h2><%# ((Product)Container.DataItem).Heading %></h2>
			    <p class="description"><%# ((Product)Container.DataItem).Subtitle %></p>

			    <div class="buy">
				    <p class="price"><EPiServer:Translate Text="/shop/price" runat="server" />: <%# ((Product)Container.DataItem).PriceWithCurrency %></p>				        

                    
                    <asp:LinkButton ID="BuyLinkButton" CssClass="btn" OnClick="BuyLinkButton_Click" CommandArgument="<%# ((Product)Container.DataItem).ProductPageUrl %>" runat="server">
                        <span><EPiServer:Translate Text="/common/buy" runat="server" /></span>
                    </asp:LinkButton>
                    <a href='<%# ((Product)Container.DataItem).ProductPageUrl %>' class="more"><EPiServer:Translate Text="/common/moreinfo" runat="server" /></a>
                    
			    </div>
		    </div>
		    <!-- // A product -->
        </ItemTemplate>

            <FooterTemplate>
            </div>
        </FooterTemplate>

    </asp:Repeater>

    <di:Paging ID="PagingControl" runat="server" />
</div>