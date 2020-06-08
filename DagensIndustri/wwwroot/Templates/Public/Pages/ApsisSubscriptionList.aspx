<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ApsisSubscriptionList.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.ApsisSubscriptionList" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:Heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:Mainintro ID="Mainintro1" runat="server" />
    <di:Mainbody ID="Mainbody1" runat="server" />

    <di:UserMessage ID="UserMessageControl1" runat="server" />

    <br />
    
    <asp:PlaceHolder ID="PlaceHolderSubscribe" runat="server">
        <br />
        <div class="form-box">  							
	        <div class="section" id="form-check">
		        <div class="row">
			        <div class="col">
                        <di:Input ID="InputEmail" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MinValue="6" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.personalemailaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.emailrequired %>" runat="server" />
                        <di:Input ID="InputPhone" CssClass="text" Required="true" Name="phone" TypeOfInput="Telephone" MinValue="7" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.mobilephone %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.mobilephonenumberrequired %>" runat="server" />
                    </div>
                </div>

                <div class="button-wrapper">
                    <asp:Button ID="ButtonSubscribe" runat="server" Text="Prenumerera" onclick="ButtonSubscribe_Click" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>


    <asp:PlaceHolder ID="PlaceHolderUnSubscribe" runat="server">
        <asp:Button ID="ButtonUnSubscribe" runat="server" Text="Avsluta prenumereration" onclick="ButtonUnSubscribe_Click" />
    </asp:PlaceHolder>


</asp:Content>

<%--<asp:Content ID="Content8" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
</asp:Content>--%>
