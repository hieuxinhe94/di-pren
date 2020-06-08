<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMainMenu.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Static.TopMainMenu" %>
<%@ Import Namespace="EPiServer.Core" %>

<EPiServer:MenuList ID="TopMenu" RequiredAccess="NoAccess" runat="server">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>
        <li>
            <asp:PlaceHolder ID="Linked" Visible='<%# Container.CurrentPage.PageTypeID == (int)EPiFunctions.SettingsPageSetting(Container.CurrentPage, "PageTypeNotLinkable") ? false : true %>' runat="server">
                <EPiServer:Property ID="Property2" PropertyName="PageLink" runat="server" />
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="NotLinked" Visible='<%# Container.CurrentPage.PageTypeID == (int)EPiFunctions.SettingsPageSetting(Container.CurrentPage, "PageTypeNotLinkable") ? true : false %>' runat="server">
                <a href="#"><%#Container.CurrentPage.PageName %></a>
            </asp:PlaceHolder>

            <asp:Repeater ID="SubMenuRepeater" DataSource="<%# GetChildrenForSecondLevel(Container.CurrentPage) %>" OnItemDataBound="SubMenuRepeater_ItemDataBound" Visible="<%# GetChildrenForSecondLevel(Container.CurrentPage).Count > 0 %>" runat="server">
                <HeaderTemplate>
                    <div class="subnav">
	                    <ul>
                </HeaderTemplate>
                
                <ItemTemplate>
                    <asp:PlaceHolder ID="MenuItemWithImage" Visible='<%# EPiFunctions.HasValue((PageData)Container.DataItem, "MenuImage") %>' runat="server">
                        <li>
                            <asp:HyperLink ID="MenuItemWithImageHyperLink" NavigateUrl='<%# ((PageData)Container.DataItem).LinkURL %>' CssClass="content" runat="server">
                                <img src='<%# ((PageData)Container.DataItem).Property["MenuImage"] %>' alt='<%# ((PageData)Container.DataItem).PageName %>'/>
				                <span>
					                <strong><%# ((PageData)Container.DataItem).PageName%></strong>
                                    <asp:PlaceHolder ID="DatePlaceHolder" Visible='<%# EPiFunctions.IsMatchingPageType((PageData)Container.DataItem, ((PageData)Container.DataItem).PageTypeID, "ConferencePageType") %>' runat="server">
                                        <i><%# EPiFunctions.HasValue((PageData)Container.DataItem, "Date") != true ? "" : ((DateTime)((PageData)Container.DataItem)["Date"]).ToString("d MMMM")%></i>
                                    </asp:PlaceHolder>
					                <%# ((PageData)Container.DataItem).Property["MenuText"]%>
				                </span>
			                </asp:HyperLink>
                        </li>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="MenuItemWithoutImage" Visible='<%# !EPiFunctions.HasValue((PageData)Container.DataItem, "MenuImage") %>' runat="server">
                        <li>
                            <asp:HyperLink ID="MenuItemWithoutImageHyperLink" NavigateUrl='<%# ((PageData)Container.DataItem).LinkURL %>' Text='<%# ((PageData)Container.DataItem).PageName %>' runat="server" />
                        </li>
                    </asp:PlaceHolder>
                </ItemTemplate>
                
                <FooterTemplate>
                        </ul>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </li>
    </ItemTemplate>
    <SelectedTemplate>
        <li class="current">
            <asp:PlaceHolder ID="Linked" Visible='<%# Container.CurrentPage.PageTypeID == (int)EPiFunctions.SettingsPageSetting(Container.CurrentPage, "PageTypeNotLinkable") ? false : true %>' runat="server">
                <EPiServer:Property ID="PageLink" PropertyName="PageLink" runat="server" />
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="NotLinked" Visible='<%# Container.CurrentPage.PageTypeID == (int)EPiFunctions.SettingsPageSetting(Container.CurrentPage, "PageTypeNotLinkable") ? true : false %>' runat="server">
                <a href="#"><%#Container.CurrentPage.PageName %></a>
            </asp:PlaceHolder>

            <asp:Repeater ID="SubMenuRepeater" DataSource="<%# GetChildrenForSecondLevel(Container.CurrentPage) %>" OnItemDataBound="SubMenuRepeater_ItemDataBound" Visible="<%# GetChildrenForSecondLevel(Container.CurrentPage).Count > 0 %>" runat="server">
                <HeaderTemplate>
                    <div class="subnav">
	                    <ul>
                </HeaderTemplate>
                
                <ItemTemplate>
                    <asp:PlaceHolder ID="MenuItemWithImage" Visible='<%# EPiFunctions.HasValue((PageData)Container.DataItem, "MenuImage") %>' runat="server">
                        <li>
                            <asp:HyperLink ID="MenuItemWithImageHyperLink" NavigateUrl='<%# ((PageData)Container.DataItem).LinkURL %>' CssClass="content" runat="server">
                                <img src='<%# ((PageData)Container.DataItem).Property["MenuImage"] %>' alt='<%# ((PageData)Container.DataItem).PageName %>'/>
				                <span>
					                <strong><%# ((PageData)Container.DataItem).PageName%></strong>
                                    <asp:PlaceHolder ID="DatePlaceHolder" Visible='<%# EPiFunctions.IsMatchingPageType((PageData)Container.DataItem, ((PageData)Container.DataItem).PageTypeID, "ConferencePageType") %>' runat="server">
                                        <i><%# EPiFunctions.HasValue((PageData)Container.DataItem, "Date") != true ? "" : ((DateTime)((PageData)Container.DataItem)["Date"]).ToString("d MMMM")%></i>
                                    </asp:PlaceHolder>
					                <%# ((PageData)Container.DataItem).Property["MenuText"]%>
				                </span>
			                </asp:HyperLink>
                        </li>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="MenuItemWithoutImage" Visible='<%# !EPiFunctions.HasValue((PageData)Container.DataItem, "MenuImage") %>' runat="server">
                        <li>
                            <asp:HyperLink ID="MenuItemWithoutImageHyperLink" NavigateUrl='<%# ((PageData)Container.DataItem).LinkURL %>' Text='<%# ((PageData)Container.DataItem).PageName %>' runat="server" />
                        </li>
                    </asp:PlaceHolder>
                </ItemTemplate>
                
                <FooterTemplate>
                        </ul>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </li>
    </SelectedTemplate>
    <FooterTemplate>
    </FooterTemplate>
</EPiServer:MenuList>