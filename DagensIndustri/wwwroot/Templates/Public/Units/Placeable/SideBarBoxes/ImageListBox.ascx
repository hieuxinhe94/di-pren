<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageListBox.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.ImageListBox" %>

<!-- Logos : Sponsors -->
<div class="infobox">
	<div class="wrapper">
		<h2>
             <asp:Literal ID="HeadingLiteral" runat="server" />    
        </h2>
        <asp:Repeater ID="ImageListRepeater" runat="server">
            <HeaderTemplate>
                <div class="content">
            </HeaderTemplate>
            <ItemTemplate>
                <%# GetItem(Container.DataItem) %>
            </ItemTemplate>
            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
	</div>
</div>
<!-- // Logos : Sponsors -->