<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageListBox.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.PageListBox" %>

<!-- Infobox -->
<div class="infobox">
	<div class="wrapper">
		<h2>
            <asp:Literal ID="HeadingLiteral" runat="server" />
        </h2>
		
        <EPiServer:PageList ID="PageListBoxPageList" runat="server">
            <HeaderTemplate>
                <div class="content">
			        <ul class="newslist">
            </HeaderTemplate>
            
            <ItemTemplate>
                <li>
					<h3><%# Container.CurrentPage["Heading"] ?? Container.CurrentPage.PageName  %></h3>
					<p><%# Container.CurrentPage["MainIntro"] %></p>
					<a href='<%# Container.CurrentPage.LinkURL %>'><EPiServer:Translate ID="Translate2" Text="/common/readmore" runat="server" /></a>
				</li>
            </ItemTemplate>
            
            <FooterTemplate>
            	    </ul>
		        </div>
            </FooterTemplate>
        </EPiServer:PageList>									
	</div>
</div>
<!-- // Infobox -->		