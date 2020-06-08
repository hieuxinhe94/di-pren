<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WineList.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.WineList" %>

<!-- Infobox --> 
<div class="infobox"> 
	<div class="wrapper"> 
		<h2>
            <asp:Literal ID="HeadingLiteral" runat="server" />
        </h2> 
		<div class="content">
            <asp:Repeater ID="WineListRepeater" runat="server">
                <HeaderTemplate>
                    <ul class="winelist">            
                </HeaderTemplate>

                <ItemTemplate>
                    <li> 
					    <div class="image-wrapper"> 
						    <img src='<%# DataBinder.Eval(Container.DataItem, "ImagePath") %>' /> 
					    </div> 
					    <h3>
                            <%# DataBinder.Eval(Container.DataItem, "Name")%>
                        </h3> 
					    <p class="region">
                            <%# DataBinder.Eval(Container.DataItem, "Country")%> <%# DataBinder.Eval(Container.DataItem, "Region")%>
                        </p> 
					    <p class="price">
                            <%# DataBinder.Eval(Container.DataItem, "Type")%>
                        </p> 
					    <p class="quantity">
                            <%# DataBinder.Eval(Container.DataItem, "BottleSize")%><%#DataBinder.Eval(Container.DataItem, "Unit") %>
                        </p> 
					    <a class="btn" href='<%= WineURL() %>' target="_blank">
                            <span>
                                <EPiServer:Translate Text="/common/readmore" runat="server" />
                            </span>
                        </a> 
			        </li> 	                
                </ItemTemplate>

                <FooterTemplate>    											
			        </ul>
                </FooterTemplate> 
            </asp:Repeater> 
		</div> 
	</div> 
</div> 
<!-- // Infobox -->	