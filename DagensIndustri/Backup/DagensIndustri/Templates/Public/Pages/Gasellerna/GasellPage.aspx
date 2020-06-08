<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/GasellernaMasterPage.Master" AutoEventWireup="true" CodeBehind="GasellPage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Gasellerna.GasellPage" %>
<%@ Register TagPrefix="DI" TagName="GasellRoot" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellRoot.ascx" %>
<%@ Register TagPrefix="DI" TagName="MainBody" Src="~/Templates/Public/Units/Placeable/OldMainBody.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="ContainerMain">    
        <div id="MainContentArea">            
            <div id="MainBodyArea">
                <DI:MainBody runat="server" /> 
            </div>         
        </div>                                      
    </div>    
    
    <div id="ContainerRight">
        <DI:GasellRoot runat="server" />
    </div>
</asp:Content>
