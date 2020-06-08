<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="PersonInfo.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.UserSettings.PersonInfo" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="MySettingsMenu" src="~/Templates/Public/Pages/UserSettings/MySettingsMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <%--<di:heading ID="Heading1" runat="server" />--%>
    <script type="text/javascript" src="/Templates/Public/js/PreventDoublePost.js"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="TopContentPlaceHolder" runat="server" ></asp:Content>


<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:MySettingsMenu ID="MySettingsMenu1" runat="server" />

    <h2><%= CurrentPage["Heading"] ?? CurrentPage.PageName %></h2>
    <di:Mainbody ID="Mainbody1" runat="server" />


    <di:UserMessage ID="UserMessageControl" runat="server" />

    <asp:PlaceHolder ID="PlaceHolderForm" runat="server">
        <div class="form-box">
            <div class="row">
                <div class="col">
                    <di:input ID="InputEmail" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MaxValue="60" StripHtml="true" AutoComplete="true" Title="E-postadress" DisplayMessage="<%$ Resources: EPiServer, common.validation.emailrequired %>" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <di:input ID="InputPhone" CssClass="text" Required="false" Name="phoneMob" TypeOfInput="Telephone" MinValue="7" MaxValue="20" StripHtml="true" AutoComplete="true" Title="Mobilnummer" DisplayMessage="Ange korrekt formaterat mobilnummer" runat="server" />
                </div>
            </div>
            
            <div class="button-wrapper">

                <div id="divSubmitBtn">
                    <asp:Button ID="ButtonSave" CssClass="btn" Text="<%$ Resources: EPiServer, common.save %>" OnClick="ButtonSave_Click" OnClientClick="return jsPreventDoublePost('divSubmitBtn', 'divFormSent');" runat="server" />
	            </div>
            
                <div id="divFormSent" style="float:right; visibility:hidden;">
                    <img src="/Templates/Public/Images/loader.gif" alt="" />
                    <i>&nbsp;<asp:Literal ID="Literal1" Text="<%$ Resources: EPiServer, common.sendingform %>" runat="server" /></i>
                </div>

            </div>

        </div>
    </asp:PlaceHolder>

</asp:Content>
