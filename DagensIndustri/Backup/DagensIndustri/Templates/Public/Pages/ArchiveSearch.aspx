<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="ArchiveSearch.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.ArchiveSearch" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="MainIntro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="DateSearch" Src="~/Templates/Public/Units/Placeable/ArchiveSearch/DateArchiveSearch.ascx" %>
<%@ Register TagPrefix="di" TagName="TextSearch" Src="~/Templates/Public/Units/Placeable/ArchiveSearch/TextArchiveSearch.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="ConcUsers" Src="~/Templates/Public/Units/Placeable/ConcurrentUsers.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:Heading runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:ConcUsers ID="ConcUsers1" runat="server" />

    <di:MainIntro runat="server" />
    <di:UserMessage ID="UserMessageControl" runat="server" />


    <asp:PlaceHolder ID="PlaceHolderNotLoggedIn" runat="server">

         <!-- Hidden Fields -->
        <asp:HiddenField ID="SelectedTabHiddenField" runat="server" />
        <!-- // Hidden Fields -->

        <div class="form-nav"> 
  	        <ul> 
		        <li class="current">
                    <asp:HyperLink ID="DateSearchHyperLink" NavigateUrl="#archive-search_date" runat="server"><EPiServer:Translate Text="/archive/paper/datesearch/searchondate" runat="server" /></asp:HyperLink>
                </li>

                <asp:PlaceHolder ID="PlaceHolderTextSearchHeader" runat="server">
                    <li>
                        <asp:HyperLink ID="TextSearchHyperLink" NavigateUrl="#archive-search_text" runat="server"><EPiServer:Translate Text="/archive/article/textsearch/searchontext" runat="server" /></asp:HyperLink>
                    </li>
                </asp:PlaceHolder>

  	        </ul>
        </div>

        <div class="form-box"> 	
            <!-- Date search -->
	        <di:DateSearch runat="server" />
            <!-- //Date search -->

            <!-- Text search -->
	        <di:TextSearch ID="TextSearch1" runat="server" />
            <!-- //Text search -->
        </div>

    </asp:PlaceHolder>

</asp:Content>
