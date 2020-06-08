<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StartPagePuffList.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.StartPagePuffList" %>

<asp:Repeater ID="PuffListRepeater" OnItemDataBound="PuffListRepeater_ItemDataBound" runat="server">
    <ItemTemplate>
        <asp:PlaceHolder ID="PuffWithTextPlaceHolder" Visible='<%# EPiFunctions.HasValue((EPiServer.Core.PageData)Container.DataItem, "PuffText") && ShowPuff((EPiServer.Core.PageData)Container.DataItem) %>' runat="server">
            <div class="banner">
		        <div class="content">
			        <img src='<%# ((EPiServer.Core.PageData)Container.DataItem)["PuffImage"] as string %>' alt='<%# GetPuffHeading((EPiServer.Core.PageData)Container.DataItem) %>' />
			        <h2><%# GetPuffHeading((EPiServer.Core.PageData)Container.DataItem) %></h2>
			        <%# ((EPiServer.Core.PageData)Container.DataItem)["PuffText"]%>
                    
                    <asp:PlaceHolder ID="HasAccessTextPlaceHolder" runat="server" Visible='<%# EPiFunctions.UserHasPageAccess((EPiServer.Core.PageData)Container.DataItem, EPiServer.Security.AccessLevel.Read)%>'>
                        <a href='<%# GetPuffUrl((EPiServer.Core.PageData)Container.DataItem, "PuffUrl") %>' class="more"><%# EPiFunctions.HasValue((EPiServer.Core.PageData)Container.DataItem, "PuffReadMoreText") ? ((EPiServer.Core.PageData)Container.DataItem)["PuffReadMoreText"].ToString() : Translate("/common/readmore") %></a>
                    </asp:PlaceHolder>
                    
                    <asp:PlaceHolder ID="NoAccessTextPlaceHolder" runat="server" Visible='<%# !EPiFunctions.UserHasPageAccess((EPiServer.Core.PageData)Container.DataItem, EPiServer.Security.AccessLevel.Read)%>'>
                        <asp:HyperLink ID="MembershipRequiredWithTextHyperLink" NavigateUrl="#membership-required" CssClass="ajax more" runat="server"><EPiServer:Translate Text="/common/readmore" runat="server" /></asp:HyperLink>
                    </asp:PlaceHolder>
		        </div>
            </div>
        </asp:PlaceHolder>
            
        <asp:PlaceHolder ID="PuffWithoutTextPlaceHolder"  Visible='<%# !EPiFunctions.HasValue((EPiServer.Core.PageData)Container.DataItem, "PuffText") && ShowPuff((EPiServer.Core.PageData)Container.DataItem) %>' runat="server">
    	    <div class="banner banner-free">
                <asp:PlaceHolder ID="HasAccessNoTextPlaceHolder" runat="server" Visible='<%# EPiFunctions.UserHasPageAccess((EPiServer.Core.PageData)Container.DataItem, EPiServer.Security.AccessLevel.Read)%>'>
                    <a href='<%# GetPuffUrl((EPiServer.Core.PageData)Container.DataItem, "PuffUrl") %>' ><img src='<%# ((EPiServer.Core.PageData)Container.DataItem)["PuffImage"] as string %>' alt='<%# GetPuffHeading((EPiServer.Core.PageData)Container.DataItem) %>' width="620" height="202" /></a>
                </asp:PlaceHolder>
                    
                <asp:PlaceHolder ID="NoAccessNoTextPlaceHolder" runat="server" Visible='<%# !EPiFunctions.UserHasPageAccess((EPiServer.Core.PageData)Container.DataItem, EPiServer.Security.AccessLevel.Read)%>'>
                    <asp:HyperLink ID="MembershipRequiredWithoutTextHyperLink" NavigateUrl="#membership-required" CssClass="ajax" runat="server"><img src='<%# ((EPiServer.Core.PageData)Container.DataItem)["PuffImage"] as string %>' alt='<%# GetPuffHeading((EPiServer.Core.PageData)Container.DataItem) %>' width="620" height="202" /></asp:HyperLink>
                </asp:PlaceHolder>
	        </div>
        </asp:PlaceHolder>
		
    </ItemTemplate>
</asp:Repeater>