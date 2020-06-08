<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfPdfDownloadBox.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes.ConfPdfDownloadBox" %>
<%@ Register TagPrefix="di" TagName="Input" Src="~/Tools/Classes/WebControls/InputWithValidation.ascx" %>
<div class="infobox">
	<div class="wrapper">
        
        
        <h3>
            <a id="pdf_request_form_show" href="javascript:void(0);">Programmet via e-post</a>
        </h3>
        
        <div class="content">
            <asp:Literal ID="MainBodyLiteral" runat="server" />
        </div>
        
        <script type="text/javascript">
            $(function () {
                <%if(!ShowForm){ %>
                    $('#pdf_request_form').hide();
                <%} %>
                $('#pdf_request_form_show').click(function () {
                    if (!$('#pdf_request_form').is(":visible")) {
                        $('#pdf_request_form').show("blind", {}, 1000);
                    } else {
                        $('#pdf_request_form').hide("blind", {}, 1000);
                    }
                });
            });
        </script>
    
    
        <div id="pdf_request_form">
            <asp:PlaceHolder ID="FormPlaceHolder" runat="server">
            <div>
                <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Visible="false" />
            </div>
            <div class="form-box">
                
                <div class="row">
                    <div class="col">
		                <div class="medium">
                            <DI:Input ID="NameInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="name" TypeOfInput="Text" Title='<%# Translate("/conference/forms/common/name") %>' DisplayMessage='<%# Translate("/conference/forms/common/name.message") %>' runat="server" />
                        </div>
                    </div>
                </div>

                <div class="row">
		            <div class="col">
                        <div class="medium">
                            <DI:Input ID="TelephoneInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="phone" TypeOfInput="Telephone" Title='<%# Translate("/conference/forms/common/phone") %>' DisplayMessage='<%# Translate("/conference/forms/common/phone.message") %>' runat="server" />
                        </div>
		            </div>						
	            </div>

                <div class="row">
		            <div class="col">
                        <div class="medium">
                            <DI:Input ID="EmailInput" CssClass="text" Required="true" StripHtml="true" AutoComplete="true" Name="mail" TypeOfInput="Email" Title='<%# Translate("/conference/forms/common/mail") %>' DisplayMessage='<%# Translate("/conference/forms/common/mail.message") %>' runat="server" />
                        </div>
		            </div>				
	            </div>
   		
                <div style="padding:10px 0px 0px 20px;">
                    <asp:LinkButton ID="PDFFormButton" Text="Skicka programmet" runat="server" OnClick="PDFFormButton_Click" />
                </div>

            </div>

            </asp:PlaceHolder>
            <asp:PlaceHolder ID="SuccessPlaceHolder" runat="server" Visible="false">
                PDF-broschyren har nu skickats till e-postadressen
            </asp:PlaceHolder>
        </div>

    </div>
</div> 