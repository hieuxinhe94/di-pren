<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ShopProductList.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Shop.ShopProductList" %>
<%@ Register TagPrefix="di" TagName="ShopProductList" Src="~/Templates/Public/Units/Placeable/Shop/ShopProductList.ascx" %>
<%@ Register TagPrefix="di" TagName="TopImage" Src="~/Templates/Public/Units/Placeable/TopImage.ascx" %>
<%@ Register TagPrefix="di" TagName="DiGoldMembershipPopup" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipPopup/DiGoldMembershipPopup.ascx" %>

<asp:Content ID="TopImageContent" ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
    <asp:PlaceHolder ID="TopProductPlaceHolder" runat="server">
        <asp:HyperLink ID="TopProductHyperLink" runat="server">
            <div id="image-viewer">
                <asp:Image ID="TopProductImage" runat="server" />
            </div>
        </asp:HyperLink>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="TopImagePlaceHolder" runat="server">
        <di:TopImage runat="server" />
    </asp:PlaceHolder>
    
</asp:Content>

<asp:Content ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server">    
    <di:ShopProductList runat="server" />
</asp:Content>