<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Heading.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Heading" %>

<h1>
    <%= CurrentPage["Heading"] ?? CurrentPage.PageName %>
</h1>