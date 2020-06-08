<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GasellMovie.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.Gasellerna.GasellMovie" %>

<a href='<%= ActualCurrentPage["MovieLink"]%>' target="_blank" title='<%=ActualCurrentPage["MovieHeading"] + " " + ActualCurrentPage["MovieIntroText"]%>'>
   <strong><%= HttpUtility.HtmlEncode(ActualCurrentPage["MovieHeading"] as string)%></strong><br />
   <%= HttpUtility.HtmlEncode(ActualCurrentPage["MovieIntroText"] as string)%> 
</a>	