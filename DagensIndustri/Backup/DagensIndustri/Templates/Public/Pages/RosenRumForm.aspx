<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="RosenRumForm.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.RosenRumForm" %>
<%@ Register TagPrefix="di" TagName="Heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="Mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<%@ Register TagPrefix="DI" TagName="UserMessage" Src="~/Templates/Public/Units/Placeable/UserMessage.ascx" %>

<asp:Content ID="Content11" ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:Heading ID="Heading1" runat="server" />
</asp:Content>

<asp:Content ID="Content12" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    
    <di:Mainbody ID="Mainbody1" runat="server" />

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
                    <DI:Input ID="email" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" TypeOfInput="Email" Name="email" Title='<%# Translate("/common/user/emailaddress") %>' DisplayMessage='<%# Translate("/common/validation/emailrequired") %>' runat="server" />
			    </div>
                <div class="col">
                    <di:Input ID="phone" CssClass="text" Required="true" Name="phone" TypeOfInput="Telephone" MinValue="7" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.mobilephone %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.mobilephonenumberrequired %>" runat="server" />
			    </div>
		    </div>	
	
            <div class="row">
			    <div class="col">
				    <div class="medium">
					    <DI:Input ID="date" CssClass="text date" Required="true" StripHtml="true" AutoComplete="true" Name="date" TypeOfInput="Date" Title='<%# Translate("/confroom/selectdate") %>' DisplayMessage='<%# Translate("/confroom/selectdatemess") %>' runat="server" />
				    </div>
			    </div>
			    <div class="col">
				    <div class="medium">
					    <DI:Input ID="time" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="time" TypeOfInput="Time" Title='<%# Translate("/confroom/selecttime") %>'  DisplayMessage='<%# Translate("/confroom/selecttimemess") %>' runat="server" />
				    </div>
			    </div>						
		    </div>
					
			<div class="row">
			    <div class="col">
				    <div class="small">
                        <DI:Input ID="numPersons" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="numPersons" TypeOfInput="Numeric" Title='<%# Translate("/confroom/numpers") %>'  DisplayMessage='<%# Translate("/confroom/numpersmess") %>' MinValue="1" MaxValue="999" runat="server" />
				    </div>
			      </div>
			      <div class="col">	
                <asp:RadioButton ID="rbSthlm" GroupName="rblCity" Checked="true" runat="server"></asp:RadioButton>Stockholm<br />
                <%--<asp:RadioButton ID="rbGbg" GroupName="rblCity" runat="server"></asp:RadioButton>Göteborg--%>
			      </div>		
		    </div>
	
            <div class="button-wrapper">
                <asp:Button ID="ButtonSend" CssClass="btn" Text="<%$ Resources: EPiServer, common.send %>" OnClick="Send_Click" runat="server" />
            </div>

	    </div>    
    </asp:PlaceHolder>


</asp:Content>

<asp:Content ID="Content13" ContentPlaceHolderID="RightColumnPlaceHolder" runat="server">
    <%--<asp:PlaceHolder ID="ConferenceSubMenuPlaceHolder" Visible="false" runat="server">
        <di:sidebarsubmenu ID="Sidebarsubmenu" runat="server" />
    </asp:PlaceHolder>
    <di:sidebarboxlist ID="Sidebarboxlist1" runat="server" />--%>
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
</asp:Content>--%>
