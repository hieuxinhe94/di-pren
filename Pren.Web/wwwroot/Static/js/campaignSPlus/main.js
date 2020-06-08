
var campaignHandler = {
    stepsCompleted: false,
    isMobileDevice: false,
    init: function (isMobileDevice) {
        $(".pren-range-buttons").equalChildWidth();

        campaignHandler.isMobileDevice = isMobileDevice;
        campaignHandler.setUpEvents();
        //startDateHandler.setUp(campaignHandler.isMobileDevice);
        validationHandler.init();

        campaignPeriodHandler.init(
            // onPeriodChanged
            function(selectedCampaign) {
                //update summary
                campaignSummary.updateSummaryInfo(selectedCampaign);
                //update payments
                payment.updateAvailableMethods(selectedCampaign);
                campaignHandler.elements.getPaymentNav().equalChildWidth(); 
            });
        
        // Init the first step
        customEventHandler.trigger("initEmail", null);
        campaignSelectionHandler.setUpCampaignSelection(sliderHandler.setUp);

        // Create tracking object for GA tracking
        var tracking = new Tracking();
        // Add listeners to tracking object for various events that should be tracked
        // Make to set up listeners after the default campaign has been set since we are pushing that event on our own below
        tracking.addListeners();
        // Push default campaign selection event
        if (!tracking.isLoggedIn()) {
            tracking.push(tracking.subscibeEvent, selectedCampaign.properties.title + ' (default)', tracking.stepSelectCampaign);
        }

        // Redesign for dibs
        campaignHandler.elements.getExpandLinks().hide();
        campaignHandler.elements.getPrenSelectionProductList().show();
        if (campaignHandler.elements.getPrenSelectionProductList().length > 1) {
            campaignHandler.elements.getPrenSelectionProductList().equalElementHeight(10);
        }        
        $(".pren-selection").each(function () {
            $(this).find("h2").appendTo($(this).find(".pren-selection-product-copy"));
        });


        autoPopulateHandler.initAutopopulatedForm();
        debugHandler.init();
    },
    setUpEvents: function() {
        // Set up events for all the steps
        stepEmail.setUpEvents();
        stepPhone.setUpEvents();
        stepSsn.setUpEvents();
        stepAddress.setUpEvents();
        stepPayment.setUpEvents();
        campaignSummary.setUpEvents();

        // Submit address button will trigger events depending on the active steps dataattribute "data-event"
        $(campaignHandler.elements.getSubmitAddressBtn()).click(function(e) {
            e.preventDefault();
            var submitEventName = stepActivator.getActiveStep().data("event");
            if (submitEventName != undefined) {
                customEventHandler.trigger(submitEventName, "submitAddress");
            }            
        });

        // The form will not submit if not payment step is initialized
        // This is to prevent submit (on enter key for example) if not all steps are done
        // Validationhandler will not trigger on submit, instead validation on form submit is done here
        $(campaignHandler.elements.getStepsContainer()).submit(function(e) {
            if (!campaignHandler.stepsCompleted) {
                e.preventDefault();
            } else {
                if (!validationHandler.validate(null, [campaignHandler.elements.getStepSsn()])) {
                    e.preventDefault();
                    campaignHandler.elements.getSubmitFormBtn().button("reset");
                    campaignHandler.elements.getSubmitFormMobileBtn().button("reset");
                } else {
                    customEventHandler.trigger("formSubmitted", "form submitted");
                }
            }
        });

        // Set loading state on submit buttons
        $(campaignHandler.elements.getSubmitFormBtn()).click(function(e) {
            $(this).button("loading");
            campaignHandler.elements.getSubmitFormMobileBtn().button("loading");
        });


        // Default submit on enter key
        campaignHandler.elements.getStepWrapper().bind("keydown", function(event) {
            var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
            if (keycode == 13) {
                event.preventDefault();

                var isDisabled = campaignHandler.elements.getSubmitAddressBtn().attr("disabled") == "disabled";

                if (!isDisabled) {
                    campaignHandler.elements.getSubmitAddressBtn().trigger("click");
                }
            }
        });
        
        campaignHandler.elements.getPrenSelect().click(function(e) {
            e.preventDefault();
        });

        // Set event on login link
        elementFactory.getElement(elementFactory.elements.emailExistsContainer).find("#loginlink").on('click', function() {
            customEventHandler.trigger("loginClicked", "login clicked");
        });
    },
    elements: {
        getPaymentNav: function () { return elementFactory.getElement(elementFactory.elements.stepPay).find(".nav.nav-pills"); },
        getStepSsn: function () { return elementFactory.getElement(elementFactory.elements.stepSsn); },
        getStartDateInput: function () { return elementFactory.getElement(elementFactory.elements.startDate); },
        getSubmitFormBtn: function() { return elementFactory.getElement(elementFactory.elements.submitForm); },
        getSubmitFormMobileBtn: function() { return elementFactory.getElement(elementFactory.elements.submitFormMobile); },
        getSubmitAddressBtn: function() { return elementFactory.getElement(elementFactory.elements.submitAddress).find("button"); },
        getStepsContainer: function() { return elementFactory.getElement(elementFactory.elements.getStepsContainer); },
        getStepWrapper: function() { return elementFactory.getElement(elementFactory.elements.stepsWrapper); },
        getCampIdInput: function () { return $("#campidinput"); },
        getExpandLinks: function () { return $(".expand-link"); },
        getPrenSelect: function () { return $(".pren-select"); },
        getPrenSelectionProductList: function () { return $(".pren-selection-product-list"); },
        getEmailExistsContainerBip: function () { return elementFactory.getElement(elementFactory.elements.emailExistsContainer).find(".alert-bip"); },
    },
};