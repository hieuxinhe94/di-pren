<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellIframe.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellIframe" %>
<%@ Register TagPrefix="DI" TagName="GasellRoot" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellRoot.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="ContainerMain">    
        <div id="MainContentArea">            
            <iframe 
                src='<%=GetURL() %>' 
                height='<%=CurrentPage["IframeHeight"] as string ?? "1000" %>px' 
                width='<%=CurrentPage["IframeWidth"] as string ?? "475" %>px' 
                frameborder="0" 
                scrolling="no">
            </iframe>           
        </div> 
    </div>    
    
    <div id="ContainerRight">
        <DI:GasellRoot runat="server" />
    </div>
</asp:Content>

