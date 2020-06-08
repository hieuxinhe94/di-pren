<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GasellWinners.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiGasell.GasellWinners" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <h1>
        <%= CurrentPage.PageName %>
    </h1>

    <EPiServer:PageList ID="GasellWinnersPageList" runat="server">
        <HeaderTemplate>
            <!-- Winners -->
			<dl class="list">
        </HeaderTemplate>
        <ItemTemplate>
            <dt>
                <%# Container.CurrentPage["GasellWinnerPlace"] %>
            </dt>
			<dd>
				<span class="description">
                    <%# Container.CurrentPage["GasellWinnerText"]%>
                </span>
                <asp:PlaceHolder ID="VideoLink" Visible='<%# EPiFunctions.HasValue(Container.CurrentPage, "GasellVideoLink") %>' runat="server">
                    <a href='<%#  Container.CurrentPage["GasellVideoLink"] %>' class="more">Se film</a>
                </asp:PlaceHolder>
				<asp:PlaceHolder ID="VimeoVideoLink" Visible='<%# EPiFunctions.HasValue(Container.CurrentPage, "GasellVimeoLink") %>' runat="server">
                    <a href='<%# Container.CurrentPage["GasellVimeoLink"] %>' class="more ajax iframe">Se film</a>
                </asp:PlaceHolder>
			</dd>
        </ItemTemplate>
        <FooterTemplate>
            </dl>
			<!-- // Winners -->	
        </FooterTemplate>
    </EPiServer:PageList>
</asp:Content>
