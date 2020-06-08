<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageTextListBox.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.ImageTextListBox" %>

<div class="banner banner-conference">
	<div class="wrapper">
		<h2>
             <asp:Literal ID="HeadingLiteral" runat="server" />
        </h2>
		<div class="content">
            <EPiServer:PageList ID="ImageTextPageListBox" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
				  		<a href='<%# Container.CurrentPage.LinkURL %>' class="content">
                            <asp:PlaceHolder ID="ImagePlaceHolder" Visible='<%# EPiFunctions.HasValue(Container.CurrentPage, "MenuImage") %>' runat="server">
				  			    <img src='<%# Container.CurrentPage.Property["MenuImage"] %>' alt='<%# Container.CurrentPage.PageName %>' />
                            </asp:PlaceHolder>
				  			<span>
				  				<strong><%# Container.CurrentPage.PageName %></strong>
                                <asp:PlaceHolder ID="DatePlaceHolder" Visible='<%# EPiFunctions.HasValue(Container.CurrentPage, "Date") %>'  runat="server">
				  				    <i><%# EPiFunctions.HasValue(Container.CurrentPage, "Date") != true ? "" : ((DateTime)Container.CurrentPage["Date"]).ToString("d MMMM") %></i> 
                                </asp:PlaceHolder>
                                <%# Container.CurrentPage.Property["MenuText"] %>
				  			</span>
				  		</a>
				  	</li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>	
                </FooterTemplate>
            </EPiServer:PageList>	  		
		</div>
	</div>
</div>    