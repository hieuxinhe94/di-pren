<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopImage.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.TopImage" %>

  <asp:PlaceHolder runat="server" ID="ImageViewerPlaceHolder">
    <div id="image-viewer">
        <asp:Repeater runat="server" ID="ImageViewerRepeater">
            <HeaderTemplate>
                <ul class="images">
            </HeaderTemplate>

            <ItemTemplate>
                <li><%# GetItem(Container.DataItem) %></li>
            </ItemTemplate>

            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>

    </div>
</asp:PlaceHolder>  