<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SideBar.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.SideBar" %>
<%@ Register TagPrefix="di" TagName="htmlbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/HtmlBox.ascx" %>
<%@ Register TagPrefix="di" TagName="imagelistbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/ImageListBox.ascx" %>
<%@ Register TagPrefix="di" TagName="pagelistbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/PageListBox.ascx" %>
<%@ Register TagPrefix="di" TagName="imagetextlistbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/ImageTextListBox.ascx" %>
<%@ Register TagPrefix="di" TagName="gaselllistbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/GasellListBox.ascx" %>
<%@ Register TagPrefix="di" TagName="pagebox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/PageBox.ascx" %>
<%@ Register TagPrefix="di" TagName="imagebox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/ImageBox.ascx" %>
<%@ Register TagPrefix="di" TagName="twitterbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/TwitterBox.ascx" %>
<%@ Register TagPrefix="di" TagName="googletranslatebox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/GoogleTranslateBox.ascx" %>
<%--<%@ Register TagPrefix="di" TagName="winelist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/WineList.ascx" %>--%>
<%@ Register TagPrefix="di" TagName="addthis" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/addthis.ascx" %>
<%@ Register TagPrefix="di" TagName="onlinesupportbox" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/OnlineSupportBox.ascx" %>


<EPiServer:PageList ID="SideBarList" runat="server">
    <HeaderTemplate>
    </HeaderTemplate>

    <ItemTemplate>
        <asp:PlaceHolder ID="HtmlBox" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "HtmlBoxPageType") %>' runat="server">
            <di:htmlbox PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="ImageListBox" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "ImageListBoxPageType") %>' runat="server">
            <di:imagelistbox PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>
            
        <asp:PlaceHolder ID="TextListBox" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "PageListBoxPageType") %>' runat="server">
            <di:pagelistbox PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="ImageTextListBox" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "ImageTextListBoxPageType") %>' runat="server">
            <di:imagetextlistbox PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="GasellListBox" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "GasellListBoxPageType") %>' runat="server">
            <di:gaselllistbox PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="PageBox" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "PageBoxPageType") %>' runat="server">
            <di:pagebox PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="ImageBox" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "ImageBoxPageType") %>' runat="server">
            <di:imagebox PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="TwitterBox" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "TwitterBoxPageType") %>' runat="server">
            <di:twitterbox PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="GoogleTranslateBox" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "GoogleTranslateBoxPageType") %>' runat="server">
            <di:googletranslatebox PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>

        <asp:PlaceHolder ID="AddThis" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "AddThisPageType") %>' runat="server">
            <di:addthis PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>        

        <%--<asp:PlaceHolder ID="WineList" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "WineListPageType") %>' runat="server">
            <di:winelist PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>--%>
        
        <asp:PlaceHolder ID="OnlineSupportBox" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "OnlineSupportBoxPageType") %>' runat="server">
            <di:onlinesupportbox PageID='<%# Container.CurrentPage.PageLink.ID %>' runat="server" />
        </asp:PlaceHolder>

    </ItemTemplate>

    <FooterTemplate>
    </FooterTemplate>
</EPiServer:PageList>