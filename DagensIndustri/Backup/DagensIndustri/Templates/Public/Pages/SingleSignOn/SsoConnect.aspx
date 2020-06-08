<%--@ Page Language="C#" AutoEventWireup="true" CodeBehind="SsoConnect.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SingleSignOn.SsoConnect" --%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="SsoConnect.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.SingleSignOn.SsoConnect" %>
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
            font-size:1.0em;
            line-height:1.3em;
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

    <asp:PlaceHolder ID="PlaceHolderCode" runat="server">
            
        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
        <td valign="top">
            <div class="borderBox">
                <div class="idHeader">Identifiera dig via kod</div>
                <div>
                    Ange koden du har fått i vårt brev<br />
                    <asp:TextBox ID="TextBoxCode" Width="170" CssClass="spaceTop" runat="server" /><br />
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

        
    <asp:PlaceHolder ID="PlaceHolderSso" runat="server">
        <p>
            <asp:Literal ID="LiteralSsoFirst" runat="server" />
            <asp:Button ID="ButtonSsoFirst" runat="server" OnClick="ButtonSsoFirst_Click" />
            
            <asp:PlaceHolder ID="PlaceHolderSsoNew" runat="server">
                <br />
                <br />
                <asp:Button ID="ButtonSsoLinkToNewAccount" runat="server" Text="Skapa nytt Di-konto" OnClick="ButtonSsoLinkToNewAccount_Click" /><br />
            </asp:PlaceHolder>
        </p>
    </asp:PlaceHolder>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    
    <asp:PlaceHolder ID="PlaceHolderAccountBox" runat="server">
        <div class="infobox">
            <div class="wrapper">
                <h2>Dina uppgifter</h2>
                <div class="content">
                    <p><asp:Literal ID="LiteralAccountInfoBox" runat="server"></asp:Literal></p>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>

    <di:sidebarboxlist ID="Sidebarboxlist1" runat="server" />
    
</asp:Content>

