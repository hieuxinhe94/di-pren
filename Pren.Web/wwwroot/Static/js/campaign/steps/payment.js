/*
Dependencies:

stepHelper
elementFactory
validationHandler
customEventHandler
debugHandler
payment
*/

var stepPayment = {
    init: function (disableScroll) {

        stepActivator.setActiveStep(stepPayment.elements.getStepPayment());

        if (disableScroll == undefined || !disableScroll) {
            scroller.scrollTo(stepPayment.elements.getStepPayment());
            stepPayment.elements.getStepPayment().focus();
        }

        // Show mobile button (can still be hidden by media querys in css)
        stepPayment.elements.getSubmitFormMobileBtn().show();

        stepPayment.elements.getPaymentNav().equalChildWidth();

        stepPayment.setUpDigitalAddressForm(stepPayment.elements.getPaymentMethodInput().val());
        campaignHandler.stepsCompleted = true;
    },
    setUpDigitalAddressForm: function(paymethod) {

        // Flag to determine if we should show area with firtname and lastname
        // Used when digital campaign and not invoice payment
        // This because we always need firstname and lastname when saving a subscription
        var showDigitalNameArea = false;

        if (selectedCampaign.properties.isDigital) {
            if (paymethod == "invoice") {
                // If a digital campaign with invoice we move the ssn and address step to stepPayment (if not already moved)
                // This because we need address to send invoice
                if (!stepPayment.elements.getStepPayment().find(elementFactory.elements.stepAddress._selector).length) {
                    var addressDigitalPh = stepPayment.elements.getInvoiceDigitalArea();
                    addressDigitalPh.append(stepPayment.elements.getStepSsn());
                    addressDigitalPh.append(stepPayment.elements.getStepAddress());
                    resizer.resizeStepInput(true, addressDigitalPh);
                }

                // If otherpayer we need firstname and lastname on subscriber                
                showDigitalNameArea = stepPayment.elements.getInvoiceOtherPayerInput().val() == "true";

                stepPayment.isDigitalInvoice = true;
            } else {
                showDigitalNameArea = true;
            }
        } else {
            stepPayment.isDigitalInvoice = false;
        }

        showDigitalNameArea ? stepPayment.elements.getDigitalNameArea().show() : stepPayment.elements.getDigitalNameArea().hide();
    },
    resetAddressForm: function() {
        // Triggers on event selectedCampaignChanged
        // If stepAdress is located in paywrapper (can happen if digital campaign with invoice), move it back to origin.
        // Moving of steps is done in setUpDigitalAddressForm
        if (stepPayment.elements.getStepPayment().find(elementFactory.elements.stepAddress._selector).length) {
            stepPayment.elements.getStepSsn().insertAfter(stepPayment.elements.getStepPhone());
            stepPayment.elements.getStepAddress().insertAfter(stepPayment.elements.getStepSsn());
        }

        stepPayment.elements.getDigitalNameArea().hide();
        // Always reset other payer when changing campaigns       
        stepPayment.elements.getInvoiceForm().hide();
        stepPayment.elements.getShowInvoiceAddressLink().text("Ange annan fakturaadress");
        stepPayment.elements.getInvoiceOtherPayerInput().val(false);
    },
    isDigitalInvoice: false,
    submit: function() {
        // This class is the last step, let the form submit handle it
    },
    events: {
        listeners: {
            initEvent: "stepAddressSubmitted",
            resetAddressForm: "selectedCampaignChanged",
        }
    },
    setUpEvents: function() {

        customEventHandler.subscribe(stepPayment.events.listeners.resetAddressForm, function() {
            stepPayment.resetAddressForm();
        });
        customEventHandler.subscribe(stepPayment.events.listeners.initEvent, function() {
            stepPayment.init();
        });

        $(stepPayment.elements.getSubmitFormMobileBtn()).click(function(e) {
            e.preventDefault();
            // Submit desktop button
            stepPayment.elements.getSubmitFormBtn().click();
        });

        $(stepPayment.elements.getShowInvoiceAddressLink()).click(function(e) {
            e.preventDefault();
            stepPayment.elements.getInvoiceForm().toggle(function() {
                if ($(this).is(":visible")) {
                    stepPayment.elements.getShowInvoiceAddressLink().text("Ta bort annan fakturaadress");
                    stepPayment.elements.getInvoiceOtherPayerInput().val(true);

                    if (stepPayment.isDigitalInvoice) {
                        stepPayment.elements.getInvoiceDigitalArea().hide();
                        stepPayment.elements.getDigitalNameArea().show();
                    }
                } else {
                    stepPayment.elements.getShowInvoiceAddressLink().text("Ange annan fakturaadress");
                    stepPayment.elements.getInvoiceOtherPayerInput().val(false);

                    if (stepPayment.isDigitalInvoice) {
                        stepPayment.elements.getInvoiceDigitalArea().show();
                        stepPayment.elements.getDigitalNameArea().hide();
                    }
                }
                debugHandler.showCampaignInfo();
            });

        });

        $(stepPayment.elements.btnGetInvoiceAddressBtn()).click(function(e) {
            e.preventDefault();

            stepPayment.elements.getSsnInvoiceError().hide();

            var btn = $(this);
            btn.button('loading');
            // Timeout to get loading state to work with ajax
            window.setTimeout(
                function() {
                    addressHandler.getAddress(stepPayment.elements.getSsnInvoiceInput().val(),
                        function(result) {
                            stepPayment.elements.getCompanyInput().val(result.Name);
                            stepPayment.elements.getPhoneInput().val(result.PhoneMobile);
                            stepPayment.elements.getStreetAddressInput().val(result.StreetAddress);
                            stepPayment.elements.getStreetNoInput().val(result.HouseNumber);
                            stepPayment.elements.getZipInput().val(result.ZipCode);
                            stepPayment.elements.getCityInput().val(result.City);
                        },
                        function(errorMessage) {
                            stepPayment.elements.getSsnInvoiceError().show();
                            stepPayment.elements.getSsnInvoiceErrorAlert().text(errorMessage);
                        }
                    );
                    btn.button('reset');
                }, 300);
        });

        $(stepPayment.elements.getTabElements()).click(function(e) {
            e.preventDefault();
            // Update text on submitbutton depending on active tab
            stepPayment.elements.getSubmitFormBtn().text($(this).data("btntext"));
            // Set correct campno and payment method
            payment.setCampNo($(e.target).parent().data("campid"));

            var paymethod = $(e.target).parent().data("method");
            payment.setPaymentMethod(paymethod);

            stepPayment.setUpDigitalAddressForm(paymethod);
            debugHandler.showCampaignInfo();
        });

        //Event for getting city
        $(stepPayment.elements.getZipInput()).bind("change keyup", function(event) {
            var zip = $(this).val();
            postNameHandler.setPostName(zip, stepPayment.elements.getCityInput());
        });
    },
    deactivate: function() {
        stepActivator.deactivateStep(stepPayment.elements.getStepPayment());
    },
    elements: {
        getPaymentNav: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find(".nav.nav-pills"); },
        getStepPayment: function () { return elementFactory.getElement(elementFactory.elements.stepPay); },
        getStepPhone: function () { return elementFactory.getElement(elementFactory.elements.stepPhone); },
        getStepSsn: function () { return elementFactory.getElement(elementFactory.elements.stepSsn); },
        getStepAddress: function () { return elementFactory.getElement(elementFactory.elements.stepAddress); },
        getSubmitFormMobileBtn: function () { return elementFactory.getElement(elementFactory.elements.submitFormMobile); },
        getSubmitFormBtn: function () { return elementFactory.getElement(elementFactory.elements.submitForm); },
        getPaymentMethodInput: function () { return $("#paymentmethodinput"); },
        getInvoiceOtherPayerInput: function () { return $("#invoiceotherpayerinput"); },
        getInvoiceForm: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#invoice-form"); },
        getInvoiceDigitalArea: function() { return elementFactory.getElement(elementFactory.elements.invoiceAddressDigital); },
        getShowInvoiceAddressLink: function() { return elementFactory.getElement(elementFactory.elements.stepPay).find("#linkshowinvoiceaddress"); },        
        btnGetInvoiceAddressBtn: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#btngetinvoiceaddress"); },
        getTabElements: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find(".nav.nav-pills a"); },
        getSsnInvoiceError: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#ssninvoiceerror"); },
        getSsnInvoiceErrorAlert: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#ssninvoiceerror .alert"); },
        getSsnInvoiceInput: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#ssninvoiceinput"); },
        getCompanyInput: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#companyinputinvoice"); },
        getPhoneInput: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#phoneinputinvoice"); },
        getStreetAddressInput: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#streetaddressinputinvoice"); },
        getStreetNoInput: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#streetnoinputinvoice"); },
        getZipInput: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#zipinputinvoice"); },
        getCityInput: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find("#cityinputinvoice"); },
        getDigitalNameArea: function() { return elementFactory.getElement(elementFactory.elements.digitalNameArea); }
    }
}