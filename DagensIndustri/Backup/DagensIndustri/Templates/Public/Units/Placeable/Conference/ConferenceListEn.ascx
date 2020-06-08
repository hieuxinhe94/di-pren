<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConferenceListEn.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Conference.ConferenceListEn" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>

<h2>
    <EPiServer:Translate Text="/conference/current" runat="server" />
</h2>

<EPiServer:PageList ID="ConferenceEnPageList" runat="server">
    <HeaderTemplate>
        <dl class="conferencelist"> 
    </HeaderTemplate>
    <ItemTemplate>
        <dt><%# Container.CurrentPage.PageName %></dt> 
	    <dd> 
		    <span class="date"><%# EPiFunctions.GetConferenceDateAndPlace(Container.CurrentPage, false, "en-US") %></span> 
		    <a href='<%# Container.CurrentPage.LinkURL %>'>
                <%# Container.CurrentPage["PuffReadMoreText"] ?? "Read more" %>
            </a> 
	    </dd> 
    </ItemTemplate>
    <FooterTemplate>
        </dl> 
    </FooterTemplate>
</EPiServer:PageList> 