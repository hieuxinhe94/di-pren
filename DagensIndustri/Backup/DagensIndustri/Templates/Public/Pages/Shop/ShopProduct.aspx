<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ShopProduct.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Shop.ShopProduct" %>
<%@ Register  TagPrefix="di" TagName="ShopProductInfo" Src="~/Templates/Public/Units/Placeable/Shop/ShopProductInfo.ascx"%>
<%@ Register  TagPrefix="di" TagName="ShopProductDescription" Src="~/Templates/Public/Units/Placeable/Shop/ShopProductDescription.ascx"%>
<%@ Register  TagPrefix="di" TagName="ShopProductSideBar" Src="~/Templates/Public/Units/Placeable/Shop/ShopProductSideBar.ascx"%>
<%@ Register TagPrefix="di" TagName="SideBar" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>
<%@ Register TagPrefix="di" TagName="DiGoldMembershipPopup" Src="~/Templates/Public/Units/Placeable/DiGoldMembershipPopup/DiGoldMembershipPopup.ascx" %>

<asp:Content ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
    <!-- Top info -->
    <di:ShopProductInfo ID="ShopProductControl" runat="server" />
    <!-- Top info -->
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:ShopProductDescription ID="ShopProductDescrControl" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <asp:PlaceHolder ID="ShopProductSideBarPlaceHolder" Visible="false" runat="server">
        <di:ShopProductSideBar ID="ShopProductSideBarControl" runat="server" />
    </asp:PlaceHolder>

    <di:sidebar runat="server" />
</asp:Content>