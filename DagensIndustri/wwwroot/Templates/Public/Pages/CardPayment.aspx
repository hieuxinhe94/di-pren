<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="CardPayment.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.CardPayment" %>
<%@ MasterType virtualpath="~/Templates/Public/MasterPages/MasterPage.Master" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register tagprefix="di" tagname="SubscriberDetails" src="~/Templates/Public/Units/Placeable/CardPayment/SubscriberDetails.ascx" %>
<%@ Register TagPrefix="di" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ID="MainContentPlaceHolder1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <di:UserMessage ID="UserMessageControl" runat="server" />
    
    <di:mainintro ID="Mainintro1" runat="server" />
    <di:mainbody ID="Mainbody1" runat="server" />



    <asp:PlaceHolder ID="PlaceHolderForm" runat="server">
        <div style="margin-bottom:5px;">
            <h3>
                <!--Pris:&nbsp;<asp:Label ID="LabelPriceItem" runat="server"></asp:Label>&nbsp;kr/st&nbsp;<asp:Label ID="LabelIncOrExVat" runat="server">(inkl moms)</asp:Label> -->
                Pris:<br />
                <asp:Label ID="LabelExVatPrice" runat="server" /> kr/st ex. moms<br />
                <asp:Label ID="LabelIncVatPrice" runat="server" /> kr/st inkl. moms<br />

            </h3>
        </div>

        <asp:PlaceHolder ID="PlaceHolderNumItems" runat="server">
            Ange antal:
            <asp:TextBox ID="TextBoxNumItems" Width="30" runat="server"></asp:TextBox>&nbsp;
            <asp:Label ID="LabelMaxNumInfo" runat="server"></asp:Label>
        </asp:PlaceHolder>    
    
        <br />
        <br />
        <br />

        <div class="form-box" id="fb1">
            <div class="section" id="form-street">
	            <div class="row">
		            <div class="col">
                        <di:Input ID="FirstNameInput" CssClass="text" Required="true" Name="firstname" TypeOfInput="Text" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.firstname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.firstnamerequired %>" runat="server" />
		            </div>
		            <div class="col">
			            <di:Input ID="LastNameInput" CssClass="text" Required="true" Name="lastname" TypeOfInput="Text" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.lastname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.lastnamerequired %>" runat="server" />
		            </div>						
	            </div>
	            <div class="row">
		            <div class="col">
			            <di:Input ID="MobilePhoneInput" CssClass="text" Required="true" Name="mobile" TypeOfInput="Telephone" MinValue="7" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.mobiletelephone %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.mobilephonenumberrequired %>" runat="server" />
		            </div>
		            <div class="col">
			            <di:Input ID="EmailInput" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MinValue="6" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.emailaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.emailrequired %>" runat="server" />
		            </div>						
	            </div>
            </div>
        </div>

        <di:SubscriberDetails ID="SubscriberDetails1" runat="server" />

        <div class="button-wrapper">
            <asp:Button ID="ButtonBuy" CssClass="btn" Text="Genomför kortköp" OnClick="ButtonBuy_Click" runat="server" />
        </div>
        
        <div style="float:right;">
            <img src="/templates/public/images/ills/creditcards.png" alt="" border="0" />
        </div>

    </asp:PlaceHolder>
    

</asp:Content>


<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigationPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TopContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="RSSPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="WideMainContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderOnTopOfSidebarBoxes" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
</asp:Content>
--%>