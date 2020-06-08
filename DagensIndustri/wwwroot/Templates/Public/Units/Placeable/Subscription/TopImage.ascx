<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopImage.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Subscription.TopImage" %>

<div id="image-viewer">
    <asp:Repeater ID="ImageViewerRepeater" OnItemDataBound="ImageViewerRepeater_ItemDataBound" runat="server">
        <HeaderTemplate>
            <ul class="images">
        </HeaderTemplate>
            
        <ItemTemplate>
            <li id="ListItem" runat="server">
                <asp:Image ID="TopItemImage" width="978" height="385" runat="server" />
            </li>
        </ItemTemplate>
            
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>