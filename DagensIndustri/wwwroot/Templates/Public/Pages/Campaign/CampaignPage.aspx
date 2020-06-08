<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Public/MasterPages/CampaignMaster.Master" AutoEventWireup="true" CodeBehind="CampaignPage.aspx.cs" Inherits="DagensIndustri.Templates.Public.Pages.Campaign.CampaignPage" %>
<%@ Register TagPrefix="DI" TagName="MainBody" Src="~/Templates/Public/Units/Placeable/OldMainBody.ascx" %>
<%@ Register TagPrefix="DI" TagName="Input" Src="~/Templates/Public/Units/Placeable/InputWithValidation.ascx" %>
<%@ Register TagPrefix="DI" TagName="Form" Src="~/Templates/Public/Units/Placeable/Campaign/CampaignForm.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="CampaignRegion" runat="server">
    <script type="text/javascript">

        function validatePage() {
            var flag = Page_ClientValidate('form');

            if (!flag) {
                showmodal("#validation", "350px", "auto", true);
            }
            else {
                //must reset src in image after postback, otherwise IE will stop animation
                ProgressImg = document.getElementById('progressimg');
                setTimeout("ProgressImg.src = ProgressImg.src", 100);
                showmodal("#progress", "200px", "100", false);
            }
        }

        function showmodal(id, modalwidth, modalheight, showbutton) {

            $(id).dialog("destroy");

            if (showbutton) {
                $(id).dialog({
                    width: modalwidth, height: modalheight, modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog('close');
                        }
                    }
                });
            }
            else {
                $(id).dialog({ width: modalwidth, height: modalheight, modal: true });
            }

        }

        function ValidateChkBox(source, arguments) {
            var checkbox = document.getElementById('<%= CbAutogiroDownload.ClientID %>');
            arguments.IsValid = checkbox.checked;
        }	    
	    
	</script>

	<div id="CampaignArea">
	    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
	    
	    <div id="CampaignHeader"><asp:Image runat="server" ID="TopImage" /></div>	   
	    
	    <div id="CampaignMainArea">
	        <div id="CampaignFormArea">
    	        <asp:UpdatePanel runat="server" ID="UpdPanelFormArea" UpdateMode="Conditional">
                    <ContentTemplate>                                        
                        <%-- Offercode area --%>
                        <asp:PlaceHolder runat="server" ID="PhOfferCodeArea">
                            <div class="bggrey offercodes">
                                <asp:PlaceHolder runat="server" ID="PhOfferCodes">
                                    <img src="/Templates/Public/Styles/Campaign/Images/offerheading.jpg" alt="Tillgängliga erbjudanden" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="form" ControlToValidate="RblOfferCodes" ErrorMessage="Erbjudande:* Obligatorisk uppgift" Display="Dynamic">
                                        <img src="/Templates/Public/Styles/Campaign/Images/mandatory.png" title='<%= Translate("/various/validators/mandatory") %>' alt='<%= Translate("/various/validators/mandatory") %>' />
                                    </asp:RequiredFieldValidator>                                                                
                                    <asp:RadioButtonList runat="server" AutoPostBack="true" OnSelectedIndexChanged="RblOfferCodesIndexChanged" ID="RblOfferCodes"></asp:RadioButtonList>	                    	                    
                                </asp:PlaceHolder>
        	                    
                                <asp:PlaceHolder runat="server" ID="PhPaymentOptions" Visible="false">
                                    <div class="infoBox">
                                        <strong><EPiServer:Translate ID="Translate1" runat="server" Text="/campaigns/various/pay" /></strong><br />
                                        <asp:RadioButtonList ID="RblPayMethod" runat="server">
                                            <asp:ListItem Value="invMe" Text="Faktura" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="invOther" Text="Faktura, annan betalare"></asp:ListItem>
                                            <asp:ListItem Value="card" Text="Kort"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </asp:PlaceHolder>
        	                    
                                <asp:PlaceHolder runat="server" ID="PhAutogiroOption" Visible="false">
                                    <div class="infoBox underline">
                                        <div class="blkArr paddingB10"><a href="javascript:showmodal('#autogiroterms','500px', 'auto', true);">Ladda ner medgivandeblankett</a></div>
                                        <asp:CheckBox runat="server" ID="CbAutogiroDownload" Text="Ja, jag har laddat hem medgivande-blanketten för autogiro" />
                                        <asp:CustomValidator ID="CustomValidator1" ValidationGroup="form" ClientValidationFunction="ValidateChkBox" ErrorMessage="Autogiro-medgivande:* Obligatorisk uppgift" Display="Dynamic" runat="server" >
                                            <img src="/Templates/Public/Styles/Campaign/Images/mandatory.png" title='<%= Translate("/various/validators/mandatory") %>' alt='<%= Translate("/various/validators/mandatory") %>' />
                                        </asp:CustomValidator>                                
                                    </div>	                    
                                </asp:PlaceHolder>
                            </div>
                        </asp:PlaceHolder>
                        <%-- Properties for thank you text after submitting campaign --%>
	                    <EPiServer:Property runat="server" CssClass="paddingT10 paddingL10" ID="ThanksDi" PageLinkProperty="CampaignRootPage" PropertyName="ThanksDi" Visible="false" />
	                    <EPiServer:Property runat="server" CssClass="paddingT10 paddingL10" ID="ThanksWeekend" PageLinkProperty="CampaignRootPage" PropertyName="ThanksWeekend" Visible="false" />
                        <%-- Campaign forms --%>
	                    <DI:Form runat="server" ID="CampaignForm" HideHeading="true" />
	                    <DI:Form runat="server" ID="CampaignFormOtherPayer" FormHeading="Betalarens uppgifter" Visible="false" />
	                </ContentTemplate>
	            </asp:UpdatePanel>  
    	        <%-- Submit/message area --%>	
	            <div class="paddingL10 paddingT10B10">
	                <asp:Button runat="server" Text="Gå vidare" ID="BtnSubmit"  ValidationGroup="form" OnClick="BtnSubmitOnClick" OnClientClick="javascript:validatePage();" />	                
	                <asp:Label runat="server" ID="LblMessage" EnableViewState="false"></asp:Label>
    	        </div>    	        
	        </div>
    	    <%-- Image/flash area --%>
	        <div id="CampaignImageArea">
	            <div id="CampaignImage">
	                <asp:Image runat="server" ID="ImgCampaign" Visible="false" />
	            </div>            
	        </div>     	        
	    </div>
	    <%-- Campaign footer --%>
        <div id="CampaignFooter" class="underline">
            <a href="javascript:showmodal('#pul','500px', 'auto', true);"><EPiServer:Translate ID="Translate2" runat="server" Text="/campaigns/various/pul" /></a>
            <a href="javascript:showmodal('#prenterms','650px', '500', true);" class="paddingL10"><EPiServer:Translate ID="Translate3" runat="server" Text="/campaigns/various/prenterms" /></a>
            <EPiServer:Property runat="server" ID="FooterDi" PageLinkProperty="CampaignRootPage" PropertyName="FooterTextDi" Visible="false" />
            <EPiServer:Property runat="server" ID="FooterWeekend" PageLinkProperty="CampaignRootPage" PropertyName="FooterTextWeekend" Visible="false" />
            <EPiServer:Property runat="server" ID="FooterDimension" PageLinkProperty="CampaignRootPage" PropertyName="FooterTextDimension" Visible="false" />
        </div>		    
    </div>        
    <%-- Validation area, jquery popup --%>
    <div id="validation" class="hidden" title="Obligatoriska fält">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="form" />
    </div>
    <%-- Pul area, jquery popup --%>
    <div id="pul" class="hidden" title="Så behandlar vi dina personuppgifter">
        <EPiServer:Property ID="Property1" runat="server" PageLinkProperty="CampaignRootPage" PropertyName="pulText" />
    </div>	        
    <%-- Prenterms area, jquery popup --%>
    <div id="prenterms" class="hidden" title="Prenumerationsvillkor">
        <EPiServer:Property ID="Property2" runat="server" PageLinkProperty="CampaignRootPage" PropertyName="prenTerms" />
    </div>	  
    <%-- Autogiro area, jquery popup --%>
    <div id="autogiroterms" class="hidden" title="Ladda ner medgivandeblankett">
        <EPiServer:Property ID="Property3" runat="server" PageLinkProperty="CampaignRootPage" PropertyName="AutoGiroText" />                           
    </div>      	                    
    <div id="nostudent" class="hidden underline" title="Studentkontroll">
        Vi kan inte verifiera att du är en student. 
        <p>Var vänlig kontakta kundtjänst.</p>
        <strong>Telefon:</strong> 08-573 651 00<br />
        <strong>E-post:</strong>
        <a href='mailto:pren@di.se'>pren@di.se</a>
    </div>     
    <div id="error" class="hidden">
        <asp:Label runat="server" ID="LblError" EnableViewState="false" CssClass="error"></asp:Label>
    </div>
    <%-- Progress area, jquery popup --%>
    <div id="progress" class="hidden bold">
        <EPiServer:Translate ID="Translate4" runat="server" Text="/campaigns/various/progress" />&nbsp;
        <img id="progressimg" src="/Templates/Public/Styles/Campaign/Images/loader.gif" alt="Skickar ..." />
    </div>	                
    <%-- Flash functionality --%>
    <asp:PlaceHolder runat="server" ID="PhFlashScript" Visible="false">
        <script type="text/javascript">
            // <![CDATA[
            var so = new SWFObject('<%=CurrentPage["FlashMovie"]%>', 'campaignflash', '<%=CurrentPage["FlashWidth"] ?? 590%>px', '<%=CurrentPage["FlashHeight"] ?? 550%>px', '<%=CurrentPage["FlashVersion"] ?? 7%>', '#FFFFFF');
            so.addParam("scale", "exactfit");
            so.addParam("wmode", "transparent");
            so.write("CampaignImage");
            // ]]>
        </script> 
    </asp:PlaceHolder>
</asp:Content>

