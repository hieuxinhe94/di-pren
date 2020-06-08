<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellListBox.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.GasellListBox" %>

<!-- Gasellmeetings -->		
	<div class="infobox">
		<div class="wrapper">
			<h2>
                <asp:Literal ID="HeadingLiteral" runat="server" />
            </h2>
			<div class="content">
						

                <asp:Literal ID="MainBodyLiteral" runat="server" />

				
                <EPiServer:PageList ID="GasellListBoxPageList" SortBy="Date" SortDirection="Ascending" runat="server">
                    
                    <HeaderTemplate>
                        <dl class="gasellist">
                    </HeaderTemplate>
                    
                    <ItemTemplate>
                        <dt>
                            <%# Container.CurrentPage["GasellCity"] %>
                        </dt>
					    <dd>
						    <span class="date">
                                <%# EPiFunctions.GetDate(Container.CurrentPage, "Date")%>
                           </span>
                            <a href='<%# Container.CurrentPage.LinkURL %>'>
                                <EPiServer:Translate ID="Translate2" Text="/common/readmore" runat="server" />
                            </a>
					    </dd>
                    </ItemTemplate>
                    
                    <FooterTemplate>
                        </dl>
                    </FooterTemplate>

                </EPiServer:PageList>				
			</div>
		</div>
	</div>	
	<!-- // Gasellmeetings -->