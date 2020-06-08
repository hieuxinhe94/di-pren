<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CampaignHeader.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Static.Campaign.CampaignHeader" %>

<link rel="stylesheet" type="text/css" media="screen" href="<%=ResolveUrl("~/Templates/Public/Styles/campaign/style.css")%>"  />
<link rel="stylesheet" type="text/css" media="print" href="<%=ResolveUrl("~/Templates/Public/Styles/campaign/print.css")%>"  />

<!--[if lte IE 6]>
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Templates/Public/Styles/campaign/ie6.css")%>" />
<![endif]-->

<%if (CurrentPage["CampaignTheme"] != null)  {%>
    <link rel="stylesheet" type="text/css" media="screen" href="<%=ResolveUrl("~/Templates/Public/Styles/campaign/" + CurrentPage["CampaignTheme"] + ".css")%>"  />
<%} %>
