<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CampaignForm.ascx.cs" Inherits="DagensIndustri.Templates.Public.Pages.bscampaign.CampaignForm" %>

<script type="text/javascript">

    $(document).ready(function () {        
        //Set up validation.js
        SetUpValidation("<%#ValidationGroup %>");

        //Disable all validators on hidden rows
        $(".address.hidden .val").each(function (e) {
            ValidatorEnable(this, false);
        });

        //Reset form submit button on blur, only if page is not valid
        $(".campaignform .controls input").click(function () {
            //if (!Page_IsValid)
            $(".campaignform .btn").button('reset');
        });

        //Show help text for default payment method
        ShowPaymentHelpText($('.payment input:checked').val());

        //Set up datepicker
        var startDate = '<%= TxtStartDate.Text %>';
        $('#dpStartDate').attr("data-date", startDate);
        $('#dpStartDate').datepicker({
            onRender: function (date) {
                return date.valueOf() < new Date(startDate.replace(/-/g, "/")).valueOf() ? 'disabled' : '';
            }
        }).on('changeDate', function (ev) {
            $('#dpStartDate').datepicker('hide');
        })

    });

    function chkTermsValidation(sender, args) {
        args.IsValid = $(".chkTerms").is(":checked");
    }

    function ShowPaymentHelpText(method) {
        $("#PaymentHelpText").children().hide();
        var elementid = "#" + method + "HelpText";
        if ($(elementid).length) {
            $("#" + method + "HelpText").show();
        }
    }

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

    //Function used to show/hide areas in form. It also enables/disables validator controls
    //Used to disable/enable validators when you show/hide areas in form
    function FormHandler(selector, show)
    {
        var element = $(selector);

        if (show) {
            $(element).removeClass("hidden");         
            $(element).show();
            //Disable all validators on hidden rows
            $(element).find(".val").each(function (e) {
                ValidatorEnable(this, true);
            });
        }
        else {
            $(element).addClass("hidden");
            $(element).hide();
            //Disable all validators on hidden rows
            $(element).find(".val").each(function (e) {
                ValidatorEnable(this, false);
            });        
        }
    }

</script>

<div class="campaignform <%#ValidationGroup %>">
    <div class="borderbottom">        
        <h4><%=FormHeading%></h4>
    </div>
    <asp:PlaceHolder runat="server" ID="PhGetParInfo">
        <div class="control-row">
            <div class="control-group">
                <asp:Label class="control-label" runat="server" AssociatedControlID="TxtPnoGet">
                    Vi fyller i åt dig om du ...
                    <em><small>(ÅÅÅÅMMDDXXXX eller XXXXXXXXXX)</small></em>
                </asp:Label>
                <%--If value starts with '55', it's a company, fire LbPopulateCompany as defaultbutton, otherwise LbPopulatePerson--%>
                <div onkeypress="javascript:return WebForm_FireDefaultButton(event,  $(this).find('#UcCampaignForm_TxtPnoGet').val().indexOf('55') == 0 ? 'UcCampaignForm_LbPopulateCompany' : 'UcCampaignForm_LbPopulatePerson')">
                    <div class="controls input-append">
                        <asp:TextBox class="input-large"  runat="server" ID="TxtPnoGet" Text="... skriver persnr eller orgnr här" onfocus="if(this.value.indexOf('...') == 0){ this.value='';}"></asp:TextBox>                                        
                        <asp:LinkButton ID="LbPopulatePerson" CssClass="btn btn-medium" runat="server" ValidationGroup="Private" OnClick="LbPopulatePersonFormOnClick" data-loading-text="Hämtar..." ToolTip="Privatperson">
                            Privatperson
                        </asp:LinkButton>
                        <asp:LinkButton ID="LbPopulateCompany" CssClass="btn btn-medium" runat="server" ValidationGroup="Company" OnClick="LbPopulateCompanyFormOnClick" data-loading-text="Hämtar..." ToolTip="Företag">
                            Företag
                        </asp:LinkButton>
                    </div>   
                </div>
                <asp:PlaceHolder runat="server" ID="PhParError" Visible="false" EnableViewState="false">                    
                    <div class="alert alert-error">
                        <button type="button" class="close" data-dismiss="alert">&times;</button>                            
                        <asp:Label ID="LblParError" runat="server"></asp:Label>
                    </div>                    
                </asp:PlaceHolder>
            </div>
        </div>
    </asp:PlaceHolder>

    <asp:Panel runat="server" DefaultButton="BtnSubmitForm">

    <asp:PlaceHolder runat="server" Visible="<%# !IsOtherPayerForm %>">
        <div class="control-row borderbottom">
            <div class="control-group validateinput">
                <asp:Label CssClass="control-label" runat="server" AssociatedControlID="TxtFirstName">
                    Förnamn                                                    
                    <asp:RequiredFieldValidator runat="server" CssClass="val" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtFirstName" ErrorMessage="Du måste ange förnamn"><span class="status">&nbsp;</span></asp:RequiredFieldValidator>                                                                                                        
                </asp:Label>
                <div class="controls">
                    <asp:TextBox class="input-large" runat="server" ID="TxtFirstName"></asp:TextBox>                                                                         
                </div>                                                
            </div>
            <div class="control-group validateinput">
                <asp:Label CssClass="control-label" runat="server" AssociatedControlID="TxtLastName">
                    Efternamn
                    <asp:RequiredFieldValidator runat="server" CssClass="val" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtLastName" ErrorMessage="Du måste ange efternamn"><span class="status">&nbsp;</span></asp:RequiredFieldValidator>
                </asp:Label>
                <div class="controls">
                    <asp:TextBox class="input-large" runat="server" ID="TxtLastName"></asp:TextBox>
                </div>
            </div>
            <asp:PlaceHolder runat="server" Visible="<%# IsStudentCampaign && !IsOtherPayerForm %>">
                <div class="control-row">
                    <div class="control-group validateinput">
                        <asp:Label runat="server" AssociatedControlID="TxtPno">
                            Personnummer <i>(ÅÅÅÅMMDDXXXX)</i>
                            <asp:RequiredFieldValidator runat="server" CssClass="val" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtPno" ErrorMessage="Du måste ange personnummer"><span class="status">&nbsp;</span></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtPno" ErrorMessage="Du måste ange ett korrekt personnummer" CssClass="valreg" ValidationExpression="(^[\d]{12}$)">&nbsp;</asp:RegularExpressionValidator>
                        </asp:Label>
                        <div class="controls">
                            <asp:TextBox class="input-large" runat="server" ID="TxtPno"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
        </div>
    </asp:PlaceHolder>
    <div class="control-row borderbottom">
        <div class="control-group validateinput">
            <asp:Label CssClass="control-label" runat="server" AssociatedControlID="TxtMobilePhone">                
                <%= IsOtherPayerForm ? "Telefonnummer" : "Mobiltelefon"%>                
                <asp:RequiredFieldValidator runat="server" CssClass="val" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtMobilePhone" ErrorMessage='<%# IsOtherPayerForm ? "Du måste ange telefonnummer" : "Du måste ange mobilnummer"%>'><span class="status">&nbsp;</span></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtMobilePhone" ErrorMessage="Du måste ange ett korrekt mobilnummer" CssClass="valreg" ValidationExpression="(^0([0-9-\s\+]){7,}$)">&nbsp;</asp:RegularExpressionValidator>
            </asp:Label>
            <div class="controls">
                <asp:TextBox class="input-large" runat="server" ID="TxtMobilePhone"></asp:TextBox>
            </div>
        </div>
        <asp:PlaceHolder runat="server" Visible="<%#!IsOtherPayerForm %>">
            <div class="control-group validateinput">
                <asp:Label CssClass="control-label" runat="server" AssociatedControlID="TxtEmail">
                    Personlig e-postadress
                    <asp:RequiredFieldValidator runat="server" CssClass="val" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtEmail" ErrorMessage="Du måste ange e-postadress"><span class="status">&nbsp;</span></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtEmail" ErrorMessage="Du måste ange en korrekt e-postadress" CssClass="valreg" ValidationExpression="^([_A-Za-z0-9-])+(\.[_A-Za-z0-9-]+)*@(([A-Za-z0-9]){1,}([-.])?([A-Za-z0-9]){1,})+((\.)?([A-Za-z0-9]){1,}([-])?([A-Za-z0-9]){1,})*(\.[A-Za-z]{2,4})$">&nbsp;</asp:RegularExpressionValidator>
                </asp:Label>
                <div class="controls">
                    <asp:TextBox class="input-large" runat="server" ID="TxtEmail"></asp:TextBox>
                    <label><em><small>Ditt användarnamn till Di:s digitala värld</small></em></label>
                </div>
            </div>
        </asp:PlaceHolder>
    </div>   
                                                                                                                                                                                                                                                                                                                                            
    <div class="control-row">
        <asp:PlaceHolder runat="server" Visible="<%#!IsStudentCampaign %>">
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="TxtCompany">Företag</asp:Label>
                <div class="controls">
                    <asp:TextBox class="input-large" runat="server" ID="TxtCompany"></asp:TextBox>
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="control-group">
            <asp:Label runat="server" AssociatedControlID="TxtCo">C/O</asp:Label>
            <div class="controls">
                <asp:TextBox class="input-large" runat="server" ID="TxtCo"></asp:TextBox>
            </div>
        </div>
    </div>

    <asp:PlaceHolder runat="server" Visible="<%#IsOtherPayerForm %>">
        <div class="control-row borderbottom">
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="TxtAttention">Attention</asp:Label>
                <div class="controls">
                    <asp:TextBox class="input-large" runat="server" ID="TxtAttention"></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="TxtOrgNo">Organisationsnummer</asp:Label>
                <div class="controls">
                    <asp:TextBox class="input-large" runat="server" ID="TxtOrgNo"></asp:TextBox>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
    
    <%--If a digital campaign, hide address fields (if not invoice or other payer though--%>
    <div class='<%= IsDigitalCampaign && !IsOtherPayerForm ? (RbInvoice.Checked ? "address" : "address hidden") : "address" %>'>
      
        <div class="control-row">
            <%--Street address is not mandatory on digital campaign and other payer form--%>
            <div class='control-group <%= (IsDigitalCampaign || IsOtherPayerForm ) ? string.Empty : "validateinput" %>'>
                <asp:Label runat="server" AssociatedControlID="TxtStreetAddress">
                    Gatuadress
                    <%--Disable validation --%>
                    <asp:RequiredFieldValidator runat="server" Visible='<%# !IsDigitalCampaign && !IsOtherPayerForm %>' ID="ReqValStreetAddress" CssClass="val" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtStreetAddress" ErrorMessage="Du måste ange gatuadress"><span class="status">&nbsp;</span></asp:RequiredFieldValidator>
                </asp:Label>
                <div class="controls">
                    <asp:TextBox CssClass="input-large" runat="server" ID="TxtStreetAddress"></asp:TextBox>
                </div>
            </div>
            <div class="control-group">
                <asp:Label runat="server" AssociatedControlID="TxtHouseNo">
                    Nr
                </asp:Label>
                <div class="controls">
                    <asp:TextBox CssClass="input-mini" runat="server" ID="TxtHouseNo"></asp:TextBox>
                </div>
            </div>
            <asp:PlaceHolder runat="server" Visible="<%# !IsOtherPayerForm %>">
                <div class="control-group">
                    <asp:Label runat="server" AssociatedControlID="TxtStairCase">Uppgång</asp:Label>
                    <div class="controls">
                        <asp:TextBox CssClass="input-mini" runat="server" ID="TxtStairCase"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group validateinput optional">
                    <asp:Label runat="server" AssociatedControlID="TxtStairs">
                        Tr
                        <asp:RegularExpressionValidator runat="server" ID="RegValStairs" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtStairs" ErrorMessage="Du har angett för många tecken i Tr" CssClass="valreg" ValidationExpression="^.{0,3}$"><span class="status">&nbsp;</span></asp:RegularExpressionValidator>
                    </asp:Label>
                    <div class="controls">
                        <asp:TextBox CssClass="input-mini" runat="server" ID="TxtStairs"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group">
                    <asp:Label runat="server" AssociatedControlID="TxtAppNo">Lgh nr</asp:Label>
                    <div class="controls">
                        <asp:TextBox CssClass="input-mini" runat="server" ID="TxtAppNo"></asp:TextBox>
                    </div>
                </div>
            </asp:PlaceHolder>
        </div>
        <div class="control-row">
            <div class="control-group validateinput">
                <asp:Label runat="server" AssociatedControlID="TxtPostalCode">
                    Postnr
                    <asp:RequiredFieldValidator runat="server" ID="ReqValPostalCode" CssClass="val" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtPostalCode" ErrorMessage="Du måste ange postnummer"><span class="status">&nbsp;</span></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="RegValPostalCode" CssClass="valreg" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtPostalCode" ErrorMessage="Du måste ange ett korrekt postnummer (xxxxxx)" ValidationExpression="(^[\d]{5}$)">&nbsp;</asp:RegularExpressionValidator>
                </asp:Label>
                <div class="controls">
                    <asp:TextBox CssClass="input-mini" MaxLength="5" runat="server" ID="TxtPostalCode"></asp:TextBox>
                </div>
            </div>
        </div>       
    </div>

    <asp:PlaceHolder runat="server" Visible="<%# !IsOtherPayerForm %>">
        <asp:PlaceHolder runat="server" Visible='<%# !IsValue("HidePrenDate") %>'>        
            <div class="control-row">
                <div class="control-group validateinput optional">
                    <asp:Label runat="server" AssociatedControlID="TxtStartDate">
                        Prenumerationstart
                        <asp:RegularExpressionValidator runat="server" ID="RegValStartDate" CssClass="valreg" ValidationGroup="<%#ValidationGroup %>" ControlToValidate="TxtStartDate" ErrorMessage="Felaktigt format på datum (åååå-dd-mm)" ValidationExpression="(^[\d]{4}-[\d]{2}-[\d]{2}$)"><span class="status">&nbsp;</span></asp:RegularExpressionValidator>
                    </asp:Label>
                    <div class="controls">
                        <div class="input-append date" id="dpStartDate" data-date="" data-date-weekstart="1" data-date-format="yyyy-mm-dd">
                            <asp:TextBox runat="server" ID="TxtStartDate" CssClass="input-medium"></asp:TextBox>
                            <span class="add-on"><i class="icon-th"></i></span>
                        </div>                
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>
        <div class="control-row">
            <div class="control-group validateinput">                
                <div class="controls">
                    <input type="checkbox" id="ChkTerms" runat="server" class="chkTerms" /> <span>Jag godkänner <a href="/System/Footer/Prenumerationsvillkor/" target="_blank">prenumerationsvillkoren</a> och samtycker till däri beskriven personuppgiftsbehandling inom Bonnierkoncernen</span>
                    <%--<asp:CustomValidator runat="server" ValidationGroup="<%#ValidationGroup %>" ClientValidationFunction="chkTermsValidation" CssClass="val" ErrorMessage="Du måste godkänna prenumerationsvillkoren"><span class="status">&nbsp;</span></asp:CustomValidator>--%>
                </div>
            </div>
        </div> 
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" Visible='<%# !IsOtherPayerForm && (!IsValue(PropertyPrefix + "HideCard") || !IsValue(PropertyPrefix + "HideInvoice") || !IsValue(PropertyPrefix + "HideInvoiceOther") || !IsValue(PropertyPrefix + "HideAutopay") || !IsValue(PropertyPrefix + "HideAutoWithdrawal")) %>'>

        <div class="control-row bordertop">
            <div class="size4">
                Jag vill betala med
                <asp:Image runat="server" ID="ImgCreditCards" ImageUrl="/templates/public/images/ills/creditcards.png" AlternateText="Vi accepterar: MasterCard, Visa och American Express" class="creditcards" />
            </div>
            <div class="control-group payment">
                <div class="controls">
                    
                    <label class="radio" runat="server" id="LblInvoice" onclick="ShowPaymentHelpText('RbInvoice');">
                        <asp:RadioButton ID="RbInvoice" Text="Faktura" GroupName="PaymentMethod" runat="server" />
                    </label>
                    <label class="radio" runat="server" id="LblInvoiceOtherPayer" onclick="ShowPaymentHelpText('RbInvoiceOtherPayer');">
                        <asp:RadioButton ID="RbInvoiceOtherPayer" Text="Faktura, annan betalare" GroupName="PaymentMethod" runat="server" />
                    </label>
                    
                    <label class="radio" runat="server" id="LblCard" onclick="ShowPaymentHelpText('RbCard');">
                        <asp:RadioButton ID="RbCard"  Text="Kort" GroupName="PaymentMethod" runat="server" />
                    </label>
                    
                    <label class="radio" runat="server" id="LblAutoPayment" onclick="ShowPaymentHelpText('RbAutoPayment');">
                        <asp:RadioButton ID="RbAutoPayment" Text="Autogiro" GroupName="PaymentMethod" runat="server" />
                    </label>
                    <label class="radio" runat="server" id="LblAutoWithdrawal" onclick="ShowPaymentHelpText('RbAutoWithdrawal');">
                        <asp:RadioButton ID="RbAutoWithdrawal" Text="Autodragning" GroupName="PaymentMethod" runat="server" />
                    </label>
                </div>
            </div>
        </div>
        <div id="PaymentHelpText">
            <div id="RbCardHelpText<%=CurrentPage[PropertyPrefix + "CardHelpText"] != null ? string.Empty : "Hide" %>" class="hide alert alert-diinfo">
                <strong>Kort: </strong><%=CurrentPage[PropertyPrefix + "CardHelpText"] %>
            </div>
            <div id="RbInvoiceHelpText<%=CurrentPage[PropertyPrefix + "InvoiceHelpText"] != null ? string.Empty : "Hide" %>" class="hide alert alert-diinfo">
                <strong>Faktura: </strong><%=CurrentPage[PropertyPrefix + "InvoiceHelpText"]%>
            </div>
            <div id="RbInvoiceOtherPayerHelpText<%=CurrentPage[PropertyPrefix + "InvoiceOtherHelpText"] != null ? string.Empty : "Hide" %>" class="hide alert alert-diinfo">
                <strong>Faktura annan betalare: </strong><%=CurrentPage[PropertyPrefix + "InvoiceOtherHelpText"]%>
            </div>
            <div id="RbAutoPaymentHelpText<%=CurrentPage[PropertyPrefix + "AutopayHelpText"] != null ? string.Empty : "Hide" %>" class="hide alert alert-diinfo">
                <strong>Autogiro: </strong><%=CurrentPage[PropertyPrefix + "AutopayHelpText"]%>
            </div>
            <div id="RbAutoWithdrawalHelpText<%=CurrentPage[PropertyPrefix + "AutowithdrawalHelpText"] != null ? string.Empty : "Hide" %>" class="hide alert alert-diinfo">
                <strong>Autodragning: </strong><%=CurrentPage[PropertyPrefix + "AutowithdrawalHelpText"]%>
            </div>
        </div>
    </asp:PlaceHolder>

    <div class="control-row validation">
        <div id="validationsummary" class="alert alert-error">
            <button type="button" class="close" data-dismiss="alert">&times;</button>
            <asp:ValidationSummary runat="server" ValidationGroup="<%#ValidationGroup %>" />
        </div>
    </div>

    <div class="control-row erroralert">
        <div id="validationsummaryt" class="alert alert-error">
            <button type="button" class="close" data-dismiss="alert">&times;</button>
            <h4>Felmeddelande!</h4>
            <asp:Label ID="LblError" runat="server"></asp:Label>
        </div>
    </div>

    <asp:Button runat="server" ID="BtnBack" OnClick="BtnBackOnClick" CssClass="btn btn-large"  data-loading-text="Laddar steg" Text="TILLBAKA" />
    <%--Do not remove class btnsubmitform, it's used by jquery during validation--%>
    <asp:Button runat="server" ID="BtnSubmitForm" OnClick="BtnSubmitFormOnClick" ValidationGroup="<%#ValidationGroup %>" OnClientClick="<%#GetValidationScript() %>" CssClass="btn btn-large btn-success disabled btnsubmitform"  data-loading-text="Skickar beställning" Text="BESTÄLL" />

    </asp:Panel>
</div>