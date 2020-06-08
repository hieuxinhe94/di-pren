<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" CodeBehind="GasellVip.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.DiGasell.GasellVip" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="DI" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarsubmenu" Src="~/Templates/Public/Units/Placeable/SidebarSubMenu.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarboxlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>


<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:Heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:Mainbody ID="Mainbody1" runat="server" />
	<!-- // Page primary content goes here -->


    <asp:PlaceHolder ID="PlaceHolderForm" runat="server">
        <div class="form-box">

			<div class="row">
			    <div class="col">
                    <DI:Input ID="firstName" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="firstname" Title='<%# Translate("/common/user/firstname") %>' DisplayMessage='<%# Translate("/common/validation/firstnamerequired") %>' runat="server" />
			    </div>

			    <div class="col">
                    <DI:Input ID="lastName" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="lastname" Title='<%# Translate("/common/user/lastname") %>' DisplayMessage='<%# Translate("/common/validation/lastnamerequired") %>' runat="server" />
			    </div>						
		    </div>

			<div class="row">
			    <div class="col">
                    <DI:Input ID="company" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="company" Title='<%# Translate("/common/company/company") %>'  DisplayMessage='<%# Translate("/common/validation/companyrequired") %>' runat="server" />
			    </div>
                <div class="col">
				    <DI:Input ID="email" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Email" Name="email" Title='<%# Translate("/common/user/emailaddress") %>' DisplayMessage='<%# Translate("/common/validation/emailrequired") %>' runat="server" />
			    </div>
		    </div>	
	
            <div class="section" style="display: block;">
                <p>Uppgifter för medföljande gäst</p>
            </div>

            <div class="row">
			    <div class="col">
                    <DI:Input ID="firstName2" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="firstname2" Title='<%# Translate("/common/user/firstname") %>' runat="server" />
			    </div>

			    <div class="col">
                    <DI:Input ID="lastName2" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="lastname2" Title='<%# Translate("/common/user/lastname") %>' runat="server" />
			    </div>						
		    </div>

			<div class="row">
			    <div class="col">
                    <DI:Input ID="company2" CssClass="text" StripHtml="true" AutoComplete="true" TypeOfInput="Text" Name="company2" Title='<%# Translate("/common/company/company") %>' runat="server" />
			    </div>
		    </div>
	
            <div class="button-wrapper">
                <asp:Button ID="ButtonSend" CssClass="btn" Text="<%$ Resources: EPiServer, common.apply %>" OnClick="Send_Click" runat="server" />
            </div>

	    </div>    
    </asp:PlaceHolder>


</asp:Content>

<asp:Content ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <asp:PlaceHolder ID="ConferenceSubMenuPlaceHolder" Visible="false" runat="server">
        <di:sidebarsubmenu ID="Sidebarsubmenu" runat="server" />
    </asp:PlaceHolder>
    <di:sidebarboxlist ID="Sidebarboxlist1" runat="server" />
</asp:Content>