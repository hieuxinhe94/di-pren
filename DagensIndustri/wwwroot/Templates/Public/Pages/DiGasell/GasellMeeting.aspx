﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GasellMeeting.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiGasell.GasellMeeting" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="gasellmeetingtop" Src="~/Templates/Public/Units/Placeable/DiGasell/GasellMeetingTop.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
    <!-- Top info -->
    <di:gasellMeetingtop runat="server" />
    <!-- Top info -->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <di:mainbody ID="MainBody1" runat="server" />
</asp:Content>