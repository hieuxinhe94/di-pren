<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppendicesSearch.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.AppendicesSearch" %>
<%@ Register TagPrefix="di" TagName="input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<div class="search-form">
    <di:input ID="SearchInput" CssClass="text" Name="search" TypeOfInput="Text" runat="server" /> 
    <asp:Button CssClass="btn" Text="Sök" OnClick="SearchAppendix" runat="server" />
</div> 
		
<div id="search-results">
    <asp:PlaceHolder ID="ResultsCounterPlaceHolder" runat="server">
	    <h2 class="n">Visar <span class="b"><%= FromItem.ToString() %> - <%= ToItem.ToString() %></span> senaste bilagor av <%= TotalItems.ToString() %></h2> 
  	</asp:PlaceHolder>				
    
    <asp:Repeater ID="AppendixRepeater" runat="server">
  	    <HeaderTemplate>
            <ul class="papers-inserts">
        </HeaderTemplate>

        <ItemTemplate>
            <li> 
			    <a href="javascript:void(0);" onClick='javascript:downloadPDF(<%# DataBinder.Eval(Container.DataItem, "ID")%>,true)'>
                    <img src='/Tools/Operations/Stream/ShowImage.aspx?what=appendix&imgname=Bilagor\<%# DataBinder.Eval(Container.DataItem, "ID")%>' alt='<%# DataBinder.Eval(Container.DataItem, "headLine")%>' />
                </a>
			    <h3><%# EPiServer.Core.Html.TextIndexer.StripHtml(DataBinder.Eval(Container.DataItem, "headLine").ToString(), 12)%></h3> 
			    <p class="date"><%# EPiFunctions.ToShortDateString(DataBinder.Eval(Container.DataItem, "created"))%>, <%# DataBinder.Eval(Container.DataItem, "size")%> <EPiServer:Translate ID="Translate1" runat="server" Text="/appendix/sizesuffix" /></p> 
			    <a href="javascript:void(0);" onClick='javascript:downloadPDF(<%# DataBinder.Eval(Container.DataItem, "ID")%>,true)' class="btn">
                    <span>Läs</span>
                </a> 
		    </li> 
        </ItemTemplate>
        
        <FooterTemplate>
            </ul> 
        </FooterTemplate>																																										
					
	</asp:Repeater>			
</div> 
		

<asp:Repeater ID="PagingRepeater" runat="server">
    <HeaderTemplate>
        <div class="pagination">
		    <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <asp:PlaceHolder Visible='<%# Container.ItemIndex.ToString() == PageNumber.ToString() %>' runat="server">
            <li class="current">
                <asp:LinkButton ID="btnPage" CommandName="Page" CommandArgument="<%#Container.DataItem %>" runat="server">
                    <%# Container.DataItem %>
                </asp:LinkButton>
            </li>
        </asp:PlaceHolder>
        <asp:PlaceHolder Visible='<%# Container.ItemIndex.ToString() != PageNumber.ToString() %>' runat="server">
            <li>
                <asp:LinkButton ID="LinkButton1" CommandName="Page" CommandArgument="<%#Container.DataItem %>" runat="server">
                    <%# Container.DataItem %>
                </asp:LinkButton>
            </li>
        </asp:PlaceHolder>
    </ItemTemplate>
    <FooterTemplate>
            </ul>            
	    </div>
    </FooterTemplate>
</asp:Repeater>
	