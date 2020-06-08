<%@ Control Language="C#" AutoEventWireup="False" CodeBehind="PuffList.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.PuffList" %>

<style type="text/css">
    .pufflist {}
    .pufflist li {border: 1px solid black;float: left;margin: 10px;min-height: 140px;padding: 10px;width: 300px;}
    .pufflist li.wide {width:640px;}    
    .pufflist li img {padding:0 10px 10px 0;float:left;}    
    .pufflist li img.right {padding:0 0 10px 10px;float:right;}    
</style>

<asp:PlaceHolder runat="server" ID="PhPuffList">

	<asp:Repeater runat="server" ID="RepPuffList">
        <HeaderTemplate>
            <div class="pufflist">
                <ul>
        </HeaderTemplate>
        <ItemTemplate>			        			    
            <li class="<%#GetSizeClass((EPiServer.Core.PageData)Container.DataItem) %>">                
                <EPiServer:Property runat="server" CssClass="<%#GetAlignClass((EPiServer.Core.PageData)Container.DataItem) %>" PropertyName="PuffImgLeft" alt="<%# ((EPiServer.Core.PageData)Container.DataItem).PageName %>" RenderImage="true" />
                
                <strong><EPiServer:Property runat="server" PropertyName="PageName" /></strong>
                <div class="intro">
                    <EPiServer:Property runat="server" PropertyName="PuffText" />
                </div>
                <div class="link">
                    <%#GetLink((EPiServer.Core.PageData)Container.DataItem) %>
                </div>
            </li>
        </ItemTemplate>
        <FooterTemplate>
                </ul>
            </div>
        </FooterTemplate>
	</asp:Repeater>
	
</asp:PlaceHolder>