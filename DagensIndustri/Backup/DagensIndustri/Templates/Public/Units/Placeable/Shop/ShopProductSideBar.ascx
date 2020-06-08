<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShopProductSideBar.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Shop.ShopProductSideBar" %>

<!-- Infobox -->
<div class="infobox">
	<div class="wrapper">
		<h2>
            <EPiServer:Translate Text="/shop/moreinformation" runat="server" />
        </h2>
		<div class="content">
			<asp:Repeater ID="InformationRepeater" runat="server">
                <ItemTemplate>
                    <p>
                        <%# Container.DataItem %>
                    </p>
                </ItemTemplate>
            </asp:Repeater>
		</div>
	</div>
</div>
<!-- // Infobox -->