<%@ Page Language="C#" MasterPageFile="~/Templates/Public/MasterPages/MasterPage.Master" AutoEventWireup="false" CodeBehind="InviteFriends.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.InviteFriend.InviteFriends" %>
<%@ Register TagPrefix="di" TagName="heading" Src="~/Templates/Public/Units/Placeable/Heading.ascx" %>
<%@ Register TagPrefix="di" TagName="mainintro" Src="~/Templates/Public/Units/Placeable/MainIntro.ascx" %>
<%@ Register TagPrefix="di" TagName="mainbody" Src="~/Templates/Public/Units/Placeable/MainBody.ascx" %>
<%@ Register TagPrefix="di" TagName="sidebarboxlist" Src="~/Templates/Public/Units/Placeable/SideBarBoxes/SideBar.ascx" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>

<asp:Content ContentPlaceHolderID="HeadingContentPlaceHolder" runat="server">
    <di:heading runat="server" />

    <script type="text/javascript">

        $(document).ready(function () {
            //Reset form submit button on blur, only if page is not valid
            $(".form-box input").click(function () {
                $(".form-box .success").hide();
            });
            //Modify some html
            $("#header .logo").replaceWith("<div class='logo'>" + $("#header .logo").html() + "</div>"); //remove clickable logo
            $("#header #nav").remove(); //remove navigation
            $("#footer").remove(); //remove footer
        });

        //Show specific (hidden) area in form and scroll down to it
        function displayArea(area) {
            var areaclass = "." + area;
            if ($(areaclass).length) {
                $(areaclass).show();
                var areaPosition = jQuery(areaclass).offset().top;
                // Scroll down to area position
                jQuery("html, body").animate({ scrollTop: areaPosition }, "slow");
            }
        }

        function showProgress() {
            $(".form-box .success").hide();
            $(".form-box .error").hide();            
            //must reset src in image after postback, otherwise IE will stop animation
            ProgressImg = document.getElementById('progressimg');
            setTimeout("ProgressImg.src = ProgressImg.src", 100);
            $("#progress").show();            
        }

    </script>

</asp:Content>

<asp:Content  ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

    <asp:Label runat="server" EnableViewState="false" ID="LblFormError"></asp:Label>

    <asp:PlaceHolder runat="server" ID="PhForm">
        
    <!-- Page primary content -->
    <di:mainbody ID="MainBody" runat="server" />
    
    <div class="content50">
        <div class="form-nav"> 
  	        <ul><li class="current"><a href="#you">Dina uppgifter</a></li></ul>   			            
        </div>         
        <div class="form-box">
            <div id="you" class="section"> 
                <div class="row">
			        <div class="col">
                        <di:Input CssClass="text" ID="TxtSenderFirstName" Required="true" Name="firstname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.firstname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.firstnamerequired %>" runat="server" />
			        </div>
			        <div class="col">
                        <di:Input  CssClass="text" ID="TxtSenderLastName" Required="true" Name="lastname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.lastname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.lastnamerequired %>" runat="server" />
			        </div>
		        </div>
	            <div class="row textarea">
                    <DI:Input ID="TxtSenderMessage" Name="message" IsTextArea="true" Required="false" StripHtml="true" MaxValue="450" Title="Meddelande" runat="server" />										
	            </div>				                
            </div>
        </div>
    </div>

    <div class="content50">
        <div class="form-nav friend" > 
  	        <ul><li class="current"><a href="#friend">Din väns uppgifter</a></li></ul>   		
            <p class="required">= obligatoriska uppgifter</p> 	
        </div>         
        <div id="friend" class="form-box">
            <div class="section"> 
                <div class="row">
			        <div class="col">
                        <di:Input ID="TxtFriendFirstName" CssClass="text" Required="true" Name="firstname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.firstname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.firstnamerequired %>" runat="server" />
			        </div>
			        <div class="col">
                        <di:Input ID="TxtFriendLastName" CssClass="text" Required="true" Name="lastname" TypeOfInput="Text" MaxValue="28" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.lastname %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.lastnamerequired %>" runat="server" />
			        </div>
		        </div>
		        <div class="row">
			        <div class="col">
                        <di:Input ID="TxtFriendEmail" CssClass="text" Required="true" Name="email" TypeOfInput="Email" MinValue="6" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.personalemailaddress %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.emailrequired %>" runat="server" />
			        </div>
			        <div class="col">
                        <di:Input ID="TxtFriendPhone" CssClass="text" Required="true" Name="phone" TypeOfInput="Telephone" MinValue="7" MaxValue="20" AutoComplete="true" Title="<%$ Resources: EPiServer, common.user.mobilephone %>" DisplayMessage="<%$ Resources: EPiServer, common.validation.mobilephonenumberrequired %>" runat="server" />
			        </div>
		        </div>						                
            </div>
            <div class="button-wrapper">	
                <asp:Label runat="server" CssClass="success hidden" ID="LblMsg" EnableViewState="false"></asp:Label>
                <asp:Label runat="server" CssClass="error hidden" ID="LblError" EnableViewState="false"></asp:Label>
                    <%-- Progress area, jquery popup --%>
                    <div id="progress" class="hidden">                            
                        <img id="progressimg" src="/templates/public/images/loader.gif" alt="Skickar ..."  />
                        <span style="">Ditt tips skickas</span>
                    </div>	
                <asp:Button Text="Skicka" CssClass="btn" runat="server" OnClick="BtnSubmitClick" OnClientClick="showProgress()" />             
            </div>	
        </div>
    </div>
    </asp:PlaceHolder>        
</asp:Content>