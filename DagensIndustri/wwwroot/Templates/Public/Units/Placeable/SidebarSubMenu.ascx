<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SidebarSubMenu.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SidebarSubMenu" %>


<EPiServer:MenuList ID="SubMenuList"  runat="server">
    <HeaderTemplate>
        <div class="nav">
	        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li>
            <asp:PlaceHolder Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "ConferencePageType") %>' runat="server">
                <a href='<%# Container.CurrentPage.LinkURL %>'>
                    <%# Container.CurrentPage["LanguageEnglish"] != null ? "Overview" : "Översikt"%>
                </a>
            </asp:PlaceHolder>
             <asp:PlaceHolder Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "ConferencePageType") ? false : true %>' runat="server">
                <a href='<%#Container.CurrentPage.LinkURL %>'>
                    <%# Container.CurrentPage["Heading"] ?? Container.CurrentPage.PageName%>
                </a>
            </asp:PlaceHolder>
        </li>
    </ItemTemplate>
    <SelectedTemplate>
        <asp:PlaceHolder ID="PlaceHolder1" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "ConferencePageType") %>' runat="server">
            <li class="current">
                <a href='<%# Container.CurrentPage.LinkURL %>'>
                    <%# Container.CurrentPage["LanguageEnglish"] != null ? "Overview" : "Översikt"%>
                </a>
            </li>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="PlaceHolder2" Visible='<%# EPiFunctions.IsMatchingPageType(Container.CurrentPage, Container.CurrentPage.PageTypeID, "ConferencePageType") ? false : true %>' runat="server">
            <li class="current">
                <a href='<%#Container.CurrentPage.LinkURL %>'>
                    <%# Container.CurrentPage["Heading"] ?? Container.CurrentPage.PageName %>
                </a>
            </li>
        </asp:PlaceHolder>
    </SelectedTemplate>
    <FooterTemplate>	
            </ul>
        </div>
    </FooterTemplate>
</EPiServer:MenuList>

		
							
