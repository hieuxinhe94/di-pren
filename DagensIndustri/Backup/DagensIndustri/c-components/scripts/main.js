(function ($) {

    //Variables for ANIMATION 
    var current_fs, next_fs, previous_fs; //fieldsets
    var left, opacity, scale; //fieldset properties which we will animate
    var animating; //flag to prevent quick multi-click glitches

    //Some form logic
    $(document).ready(function () {
        CampaignHandler.init();
    });

    var CampaignHandler = {
        init: function () {
            //Set up all events for Campaign
            $(".next").click(function (event) {
                event.preventDefault();
                CampaignHandler.setActiveFieldset(this, true);
            });
            $(".previous").click(function (event) {
                event.preventDefault();
                CampaignHandler.setActiveFieldset(this, false);
            });
            $('input[type="radio"]').iCheck({
                checkboxClass: 'icheckbox_square-green',
                radioClass: 'icheckbox_square-green', //iradio_square-green
                increaseArea: '20%' // optional
            });
            $("#email").bind("keydown", function (event) { //Default submit
                var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
                if (keycode == 13) {
                    $("#step1-next").click();
                    return false;
                } else {
                    return true;
                }
            });
            //Event for datepicker
            $(".input-group-addon").bind("click", function (event) {
                $('.txtprenstart').datepicker('show');
            });

            //Event for getting city
            $(".txtaddresszipcode, .txtaddresszipcodeop").bind("change keyup", function (event) {
                var zip = $(this).val();
                AjaxHandler.setPostalCity(zip, $(this).hasClass("txtaddresszipcodeop") ? "op" : "");
            });

            //Set up datepicker control
            var startDate = $(".txtprenstart").val();
            $('.txtprenstart').attr("data-date", startDate);
            $('.txtprenstart').datepicker({
                onRender: function (date) {
                    return date.valueOf() < new Date(startDate.replace(/-/g, "/")).valueOf() ? 'disabled' : '';
                }
            }).on('changeDate', function (ev) {
                $('.txtprenstart').datepicker('hide');
            });

            $("#getAddress").click(function (event) {
                event.preventDefault();
                $("#ssnerror").hide();
                InputHelper.clearAllInputFields();
                AjaxHandler.populateParData($(".txtssn").val());
            });

            $("#step1-next").click(function (event) {
                CampaignHandler.setUpStudentCampaign();
                if (CampaignHandler.isValid("email")) {
                    $(".txtemail-ver").text($(".txtemail").val()); //copy entered email (step1) to verify area
                    AjaxHandler.saveEmailAddress($(".txtemail").val());
                    CampaignHandler.setActiveFieldset(this, true);
                    $(".txtssn").focus();
                }
            });
            $("#step2-next").click(function (event) {
                if (CampaignHandler.isValid("form")) {
                    InputHelper.copyAllInputToVerify();
                    CampaignHandler.setActiveFieldset(this, true);
                    CampaignHandler.setUpPaymentOptions();
                    CampaignHandler.scrollTo("#msform");
                }
                else {
                    $('.sidebar').css('top', '');
                    InputHelper.setFocusOnFirstVisibleTextBox();
                }
            });
            //Manual form, and Edit form
            $("#ssn > p.help-block, #showform").click(function (event) {
                event.preventDefault();
                $("#ssn").hide(); //hide ssn-textbox
                $("#auto").show(); //show "get automatic" link
                $("#wform").show();
                $("#name").show();
                $("#phone").show();
                $("#addCompany").show();
                $("#address").show();
                $("#address-extra").show();
                $("#zip").show();
                //$("#pren").show();
                $("#step2-next").show();
            });

            //Get automatic link on form
            $("#auto > p.help-block").click(function (event) {
                event.preventDefault();
                $("#extra-info").hide();
                $("#ssn").show(); //show ssn-textbox
                $("#auto").hide(); //hide "get automatic" link
                $("#wform").hide();
                $("#step2-next").hide();
            });

            $("#addCompany").click(function (event) {
                event.preventDefault();
                $("#company").toggle();
            });

            $("#addAddressCo").click(function (event) {
                event.preventDefault();
                $("#addressCo").toggle();
            });

            $(".submit").click(function () {
                return false;
            });

            //Checkboxes for active campaign
            $('.RbPrimary, .RbSecondary').on('ifChecked', function (event) {
                CampaignHandler.IcheckClick();
            });


            //if Campaign2 selected on load, i must be caused by postback with error. Check to trigger events        
            if (this.getSelectedCampaign() == "Campaign2") {
                CampaignHandler.IcheckClick();
            }

            $("[data-toggle='tooltip']").tooltip();

            //set focus
            $(".txtemail").focus();
        },
        IcheckClick: function () {
            CampaignHandler.setUpPaymentOptions();
            CampaignHandler.setUpStudentCampaign();
            var camp = CampaignHandler.getSelectedCampaign().toLowerCase();
            var seccamp = camp == "campaign1" ? "campaign2" : "campaign1";
            //Switch classes to add border and background
            $("." + seccamp + ".campw").removeClass("selected");
            $("." + camp + ".campw").addClass("selected");
        },
        getSelectedCampaign: function () {
            return $(".RbPrimary input[type=radio]").prop("checked") ? "Campaign1" : "Campaign2";
        },
        isStudentCampaign: function () {
            var camp = this.getSelectedCampaign();
            return camp == "Campaign1" ? this.constants.campaign1StudentCamp.length > 0 : this.constants.campaign2StudentCamp.length > 0;
        },
        isValid: function (validationgroup) {
            return Page_ClientValidate(validationgroup);
        },
        setUpPaymentOptions: function () {
            var camp = this.getSelectedCampaign().toLowerCase();

            this.constants[camp + "HideCard"].length ? $("#paycard").hide() : $("#paycard").show();
            this.constants[camp + "HideInvoice"].length ? $("#payinvoice").hide() : $("#payinvoice").show();
            this.constants[camp + "HideInvoiceOther"].length ? $("#payinvoiceother").hide() : $("#payinvoiceother").show();
            this.constants[camp + "HideAutoPay"].length ? $("#payautopay").hide() : $("#payautopay").show();
            this.constants[camp + "HideAutoWithdrawal"].length ? $("#payautowithdrawal").hide() : $("#payautowithdrawal").show();

            if (this.constants[camp + "FreeCamp"].length) {
                $("#payinvoice").show();
                $("#payinvoice a span").text("Prova på gratis");
            }
            else {
                $("#payinvoice a span").text("Betala med faktura");
            }
        },
        setUpStudentCampaign: function () {
            var camp = this.getSelectedCampaign();
            var student = this.isStudentCampaign(camp);

            //Disable student asp.net validators
            ValidatorEnable(document.getElementById($(".StudSsnVal").attr('id')), student)
            ValidatorEnable(document.getElementById($(".StudSsnRegVal").attr('id')), student)
            student ? $("#studssn").show() : $("#studssn").hide();
        },
        setActiveFieldset: function (element, next) {
            if (animating) return false;
            animating = true;
            current_fs = $(element).parent();
            next_fs = next ? $(element).parent().next() : $(element).parent().prev();
            if (next) {
                $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");
            }
            else {
                $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");
            }
            next_fs.show();
            current_fs.animate({ opacity: 0 }, {
                step: function (now, mx) {
                    opacity = 1 - now;
                    if (next) {
                        current_fs.css({ 'transform': 'scale(' + scale + ')' });
                        next_fs.css({ 'left': left, 'opacity': opacity });
                    }
                    else {
                        current_fs.css({ 'left': left });
                        next_fs.css({ 'transform': 'scale(' + scale + ')', 'opacity': opacity });
                    }
                },
                duration: 0,
                complete: function () {
                    current_fs.hide();
                    animating = false;
                },
                easing: ''
            });
        },
        scrollTo: function (selector) {
            $('html, body').animate({ scrollTop: $(selector).offset().top }, 'slow');
        },
        constants: {
            pageId: $('#pageid').val(),
            //Campaign 1
            campaign1StudentCamp: $('#campaign1StudentCamp').val(),
            campaign1FreeCamp: $('#campaign1FreeCamp').val(),
            campaign1HideCard: $('#campaign1HideCard').val(),
            campaign1HideInvoice: $('#campaign1HideInvoice').val(),
            campaign1HideInvoiceOther: $('#campaign1HideInvoiceOther').val(),
            campaign1HideAutoPay: $('#campaign1HideAutoPay').val(),
            campaign1HideAutoWithdrawal: $('#campaign1HideAutoWithdrawal').val(),
            //Campaign 2
            campaign2StudentCamp: $('#campaign2StudentCamp').val(),
            campaign2FreeCamp: $('#campaign2FreeCamp').val(),
            campaign2HideCard: $('#campaign2HideCard').val(),
            campaign2HideInvoice: $('#campaign2HideInvoice').val(),
            campaign2HideInvoiceOther: $('#campaign2HideInvoiceOther').val(),
            campaign2HideAutoPay: $('#campaign2HideAutoPay').val(),
            campaign2HideAutoWithdrawal: $('#campaign2HideAutoWithdrawal').val()
        }
    }

    var AjaxHandler = {
        saveEmailAddress: function (email) {
            $.ajax({
                type: "POST",
                url: "http://" + document.domain + "/Templates/Public/Pages/C-Campaign/GetJson.ashx?action=email&email=" + email,
                timeout: 20000,
                dataType: "text",
                success: function (msg) {
                    if (msg == "ok") {
                        //alert("saved");
                    }
                },
                error: function (xhr, textStatus, error) {
                    //Sshh ... be silent
                }
            });
        },
        populateParData: function (ssn) {
            $.ajax({
                type: "POST",
                url: "http://" + document.domain + "/Templates/Public/Pages/C-Campaign/GetJson.ashx?pno=" + ssn + "&pageId=" + CampaignHandler.constants.pageId + "&campaign=" + CampaignHandler.getSelectedCampaign(),
                timeout: 20000,
                dataType: "text",
                success: function (msg) {
                    AjaxHandler.processParData(jQuery.parseJSON(msg));
                },
                error: function (xhr, textStatus, error) {
                    $("#ssnerror").text("Ett fel uppstod").show();
                }
            });
        },
        processParData: function (parData) {
            var error = parData.Error;
            if (error != null) {
                $("#ssnerror").text("Vänligen ange ditt personnummer med ÅÅÅÅMMDDXXXX, eller organisationsnummer med XXXXXX-XXXX").show();
                return;
            }

            //Populate element with pardata
            InputHelper.setUpControl(".txtfname", "#name", parData.FirstNames, true);
            InputHelper.setUpControl(".txtlname", "#name", parData.LastNames, true);
            InputHelper.setUpControl(".txtphone", "#phone", parData.PhoneMobile, true);
            InputHelper.setUpControl(".txtcompany", "#company", parData.Name, false);
            InputHelper.setUpControl(".txtaddress", "#address", parData.StreetAddress, true);
            InputHelper.setUpControl(".txtaddresshouseno", "#address", parData.HouseNumber, false);
            InputHelper.setUpControl(".txtaddresstaircase", "#address-extra", parData.StairCase, false);
            InputHelper.setUpControl(".txtaddresstairs", "#address-extra", parData.Stairs, false);
            InputHelper.setUpControl(".txtaddressappno", "#address-extra", parData.AppartmentNumber, false);
            InputHelper.setUpControl(".txtaddresszipcode", "#zip", parData.ZipCode, true);

            if (CampaignHandler.isStudentCampaign()) {
                InputHelper.setUpControl(".txtstudssn", "#studssn", $(".txtssn").val(), true);
            }
            else {
                $("#studssn").hide();
            }

            AjaxHandler.setPostalCity(parData.ZipCode, "");

            //Copy all element to verify area
            InputHelper.copyAllInputToVerify();

            $("#addCompany").hide();
            $("#ssn").hide();
            $("#extra-info").show();
            $("#wform").show();
            $("#step2-next").show();

            InputHelper.setFocusOnFirstVisibleTextBox();
        },
        setPostalCity: function (zip, suffix) {
            if (!jQuery.isNumeric(zip) && zip.length != 5) {
                return false;
            }

            $.ajax({
                type: "POST",
                url: "http://" + document.domain + "/Templates/Public/Pages/C-Campaign/GetJson.ashx?action=zip&zip=" + zip,
                timeout: 20000,
                dataType: "text",
                success: function (msg) {
                    $(".txtcity" + suffix).val(msg);
                    InputHelper.copyInputToVerify(".txtcity" + suffix);
                }
            });
        }
    }

    var InputHelper = {
        setUpControl: function (selector, wrapper, value, mandatory) {
            if (value != null && value.length) {
                $(selector).val(value);
                $(wrapper).hide();
            }
            else if (mandatory) {
                //if mandatory input is empty, show area            
                $(wrapper).show();
            }
            else {
                $(wrapper).hide();
            }
        },
        copyAllInputToVerify: function () {
            this.copyInputToVerify(".txtfname");
            this.copyInputToVerify(".txtlname");
            this.copyInputToVerify(".txtphone");
            this.copyInputToVerify(".txtaddress");
            this.copyInputToVerify(".txtaddresshouseno");
            this.copyInputToVerify(".txtaddresstaircase");
            this.copyInputToVerify(".txtaddresstairs");
            this.copyInputToVerify(".txtaddresszipcode");
            this.copyInputToVerify(".txtcompany");
            this.copyInputToVerify(".txtcity");
        },
        clearAllInputFields: function () {
            //clearInputFields(".txtssn");
            this.clearInputFields(".txtfname");
            this.clearInputFields(".txtlname");
            this.clearInputFields(".txtphone");
            this.clearInputFields(".txtcompany");
            this.clearInputFields(".txtaddress");
            this.clearInputFields(".txtaddresshouseno");
            this.clearInputFields(".txtaddressco");
            this.clearInputFields(".txtaddresstaircase");
            this.clearInputFields(".txtaddresstairs");
            this.clearInputFields(".txtaddressappno");
            this.clearInputFields(".txtaddresszipcode");
            this.clearInputFields(".txtcity");
        },
        clearInputFields: function (selector) {
            $(selector).val("")
            $(selector + "-ver").each(function () {
                $(this).text("");
            });
        },
        copyInputToVerify: function (selector) {
            var value = $(selector).val();
            $(selector + "-ver").each(function () {
                $(this).text(value);
            });
        },
        setFocusOnFirstVisibleTextBox: function () {
            $("#wform").find(":input:enabled:visible:first").not(".txtprenstart").focus();
        }
    };


})(jQuery);