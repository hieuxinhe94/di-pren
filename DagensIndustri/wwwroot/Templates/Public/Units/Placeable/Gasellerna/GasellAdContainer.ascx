<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellAdContainer.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Gasellerna.GasellAdContainer" %>
<%@ Register TagPrefix="DI" TagName="GasellAdList" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellAdList.ascx" %>
<%@ Register TagPrefix="DI" TagName="GasellAdEditor" Src="~/Templates/Public/Units/Placeable/Gasellerna/GasellAdEditor.ascx" %>
<%@ Register TagPrefix="DI" Namespace="DagensIndustri.Tools.Classes.WebControls" assembly="DagensIndustri" %>

<DI:GasellContainerPageList runat="server" ID="PlAdContainer">
    <GasellAdPuffTemplate>
        <DI:GasellAdEditor runat="server" />
    </GasellAdPuffTemplate>
    <GasellAdListTemplate>
        <DI:GasellAdList runat="server" />
    </GasellAdListTemplate>  
</DI:GasellContainerPageList>