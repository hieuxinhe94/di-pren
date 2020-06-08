<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellContainer.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Gasellerna.GasellContainer" %>
<%@ Register TagPrefix="DI" TagName="GasellPuff" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellPuff.ascx" %>
<%@ Register TagPrefix="DI" TagName="GasellMovieList" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellMovieList.ascx" %>
<%@ Register TagPrefix="DI" TagName="GasellSearch" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellSearch.ascx" %>
<%@ Register TagPrefix="DI" Namespace="DagensIndustri.Tools.Classes.WebControls" assembly="DagensIndustri" %>


<DI:GasellContainerPageList runat="server" ID="PlGasellContainer" >
    <GasellPuffTemplate>
        <DI:GasellPuff runat="server"  />
    </GasellPuffTemplate> 
    <GasellMovieListTemplate>
        <DI:GasellMovieList runat="server"  />
    </GasellMovieListTemplate>
    <GasellSearchTemplate>
        <DI:GasellSearch runat="server"  />
    </GasellSearchTemplate>        
</DI:GasellContainerPageList>     