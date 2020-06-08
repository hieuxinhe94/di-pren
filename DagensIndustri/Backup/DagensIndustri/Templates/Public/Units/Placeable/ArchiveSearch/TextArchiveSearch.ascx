<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextArchiveSearch.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.ArchiveSearch.TextArchiveSearch" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="Paging" Src="~/Templates/Public/Units/Placeable/Paging.ascx" %>

<div class="section" id="archive-search_text">
	<div class="row">
		<div class="col">
            <di:Input ID="SearchInput" TypeOfInput="Text" CssClass="text medium" Name="searchstring" AutoComplete="true" Title="<%$ Resources: EPiServer, archive.article.textsearch.searchfor %>" runat="server" />
            <asp:Button ID="SearchButton" CssClass="btn" OnClick="SearchButton_Click" Text="<%$ Resources: EPiServer, common.search %>" runat="server" />
		</div>
	</div>

    <asp:PlaceHolder ID="SearchResultPlaceHolder" Visible="false" runat="server">
	    <div class="row searchlist">
		    <h4><asp:Literal ID="SearchResultLiteral" runat="server" /></h4>

            <asp:Repeater ID="SearchResultRepeater" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
            
                <ItemTemplate>
                    <li>
				        <h5>
                            <a href="<%# GetLink((string)DataBinder.Eval(Container.DataItem, "filename"))%>">
                                <%# GetTitle((string)DataBinder.Eval(Container.DataItem, "DocTitle")) %>
                            </a>
                            <span class="date">
                                <%# string.Format("{0} {1}", Translate("/archive/article/textsearch/published"), DataBinder.Eval(Container.DataItem, "publishdate")) %>
                            </span>
                        </h5>
			        </li>
                </ItemTemplate>
            
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
	    </div>
		
        <di:Paging ID="PagingControl" runat="server" />

    </asp:PlaceHolder>
</div>