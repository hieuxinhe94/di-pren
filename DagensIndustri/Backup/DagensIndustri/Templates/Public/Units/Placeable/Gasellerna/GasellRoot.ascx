<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellRoot.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Gasellerna.GasellRoot" %>
<%@ Register TagPrefix="DI" TagName="GasellContainer" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellContainer.ascx" %>
<%@ Register TagPrefix="DI" TagName="GasellAdContainer" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellAdContainer.ascx" %>


<div id="GasellContainer">
    <DI:GasellContainer runat="server" />        
</div>
<div id="GasellAdsContainer">
    <DI:GasellAdContainer runat="server" />
</div>