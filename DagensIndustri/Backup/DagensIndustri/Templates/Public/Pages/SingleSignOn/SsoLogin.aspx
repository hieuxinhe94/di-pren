﻿<%--@ Page Language="C#" AutoEventWireup="true" CodeBehind="SsoLogin.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SingleSignOn.SsoLogin" --%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="SsoLogin.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SingleSignOn.SsoLogin" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarsubmenu" Src="~/Templates/Public/Units/Placeable/SidebarSubMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarboxlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register Tagprefix="di" Tagname="SendCodeToEmail" src="~/Templates/Public/Units/Placeable/SingleSingOn/SendCodeToEmail.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading ID="Heading1" runat="server" />

    <style type="text/css">
        .idHeader
        {
            font-size:1.1em;
            margin-bottom:7px;
            font-weight:bold;    
        }
    
        .grayInfo
        {
            font-size:1em;
            line-height:1.3em;
            color:#999999;
            margin-top:5px;
            float:left;
        }
    
        .borderBox
        {
            border: 1px solid #cccccc; 
            padding:10px;
            background-color:#ffffff;    
            overflow:auto;
            vertical-align:text-top;
            position:relative;
            width:170px;
        }
    
        .spaceTop
        {
            margin-top:5px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">    

    <di:mainintro ID="Mainintro1" runat="server" />
    <di:mainbody ID="Mainbody1" runat="server" />		
      
    <di:usermessage ID="UserMessageControl1" runat="server" />
    
    <asp:Label ID="LabelMess" runat="server" Visible="false"></asp:Label>


    <asp:PlaceHolder ID="PlaceHolderSsoLinks" runat="server">
        <table border="0" cellspacing="0" cellpaddning="0">
        <tr>
        <td>
            <p>
                <asp:Button ID="ButtonToLogin" runat="server" Text="Logga in med ditt Di-konto" OnClick="ButtonToLogin_Click" />
            </p>
        </td>
        <td width="20"></td>
        <td>
            <p>
                <asp:Button ID="Button2" runat="server" Text="Skapa nytt Di-konto" OnClick="ButtonToNewAccount_Click" />
            </p>
        </td>
        </tr>
        </table>
    </asp:PlaceHolder>


    
    <asp:PlaceHolder ID="PlaceHolderCode" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
        <td>
            <div class="borderBox">
                <div class="idHeader">Identifiera dig via kod</div>
                <div>
                    <asp:TextBox ID="TextBoxCode" Width="140" runat="server" /><br />
                    <asp:Button ID="ButtonCode" runat="server" CssClass="spaceTop" Text="OK" onclick="ButtonCode_Click" />
                </div>
            </div>
        </td>
        <td width="30"></td>
        <td>
            <di:SendCodeToEmail ID="SendCodeToEmail1" runat="server" />
        </td>
        </tr>
        </table>
     
        <br />
        <br />
    </asp:PlaceHolder>

        
        

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <di:sidebarboxlist ID="Sidebarboxlist1" runat="server" />
</asp:Content>

