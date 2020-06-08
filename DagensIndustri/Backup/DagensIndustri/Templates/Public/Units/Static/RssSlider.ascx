<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RssSlider.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Static.RssSlider" %>
<asp:PlaceHolder ID="PlaceHolderRssWrapper" runat="server" Visible='<%#DIClassLib.ServiceVerifier.ServiceVerifier.DiRssIsValid %>'>
<div id="di_se_rss">
	<a href="http://www.di.se" target="_blank"><h2>Nyheter från di.se</h2></a>

    <asp:Repeater ID="RepRSS" runat="server">
        <ItemTemplate>
            <div class="rss-item-<%# ((RSSItem)Container.DataItem).ID %>">
                <small><%# ((RSSItem)Container.DataItem).FormattedPublishedDate %></small>
                <h3><a href="<%# ((RSSItem)Container.DataItem).Link %>" target="diSe"><%# ((RSSItem)Container.DataItem).Title %></a></h3>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
</asp:PlaceHolder>