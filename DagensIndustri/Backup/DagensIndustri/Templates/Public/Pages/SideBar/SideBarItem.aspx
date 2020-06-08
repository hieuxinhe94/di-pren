<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="SideBarItem.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SideBar.SideBarItem" %>
<%@ Register TagPrefix="di" TagName="htmlbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/HtmlBox.ascx" %>
<%@ Register TagPrefix="di" TagName="imagelistbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/ImageListBox.ascx" %>
<%@ Register TagPrefix="di" TagName="pagelistbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/PageListBox.ascx" %>
<%@ Register TagPrefix="di" TagName="imagetextlistbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/ImageTextListBox.ascx" %>
<%@ Register TagPrefix="di" TagName="gaselllistbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/GasellListBox.ascx" %>
<%@ Register TagPrefix="di" TagName="pagebox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/PageBox.ascx" %>
<%@ Register TagPrefix="di" TagName="imagebox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/ImageBox.ascx" %>
<%@ Register TagPrefix="di" TagName="winelist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/WineList.ascx" %>
<%@ Register TagPrefix="di" TagName="twitterbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/TwitterBox.ascx" %>
<%@ Register TagPrefix="di" TagName="googletranslatebox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/GoogleTranslateBox.ascx" %>
<%@ Register TagPrefix="di" TagName="addthis" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/addthis.ascx" %>
<%@ Register TagPrefix="di" TagName="onlinesupportbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/OnlineSupportBox.ascx" %>


<asp:Content  ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <asp:PlaceHolder ID="HtmlBox" Visible='false' runat="server">
        <di:htmlbox runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="ImageListBox" Visible='false' runat="server">
        <di:imagelistbox runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="TextListBox" Visible='false' runat="server">
        <di:pagelistbox runat="server" />    
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="ImageTextListBox" Visible='false' runat="server">
        <di:imagetextlistbox  runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="GasellListBox" Visible='false' runat="server">
        <di:gaselllistbox runat="server" />    
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="PageBox" Visible='false' runat="server">
        <di:pagebox  runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="ImageBox" Visible='false' runat="server">
        <di:imagebox runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="WineList" Visible='false' runat="server">
        <di:winelist runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="TwitterBox" Visible='false' runat="server">
        <di:twitterbox runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="GoogleTranslateBox" Visible='false' runat="server">
        <di:googletranslatebox runat="server" />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="AddThis" Visible='false' runat="server">
        <di:addthis runat="server" />
    </asp:PlaceHolder>
    
    <asp:PlaceHolder ID="OnlineSupportBox" Visible='false' runat="server">
        <di:onlinesupportbox runat="server" />
    </asp:PlaceHolder>

</asp:Content>
