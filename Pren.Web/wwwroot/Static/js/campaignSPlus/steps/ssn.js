var stepSsn = {
    init: function (disableScroll) {
        
        stepActivator.setActiveStep(stepSsn.elements.getStepSsn());
        
        if (disableScroll == undefined || !disableScroll) {
            scroller.scrollTo(stepSsn.elements.getStepSsn());
            stepSsn.elements.getSsnInput().focus();
        }

        // Show prenstart datepicker
        //stepSsn.elements.getPrenStartContainer().show();
    },
    submit: function () {
        validationHandler.validateSingleElement(function () {

            stepSsn.elements.getSsnError().hide();

            addressHandler.getAddress(stepSsn.elements.getSsnInput().val(),
                function(result) {
                    stepAddress.populateAddressFields(result);
                },
                function (errorMessage) {
                    stepSsn.elements.getSsnError().show();
                    stepSsn.elements.getSsnErrorAlert().text(errorMessage);
                }
            );

            if (stepSsn.elements.getSsnError().is(":visible")) {
                return;
            }

            stepSsn.elements.getBtnManualAddress().hide();

            customEventHandler.trigger(stepSsn.events.triggers.onSubmitEvent, "init address");

            // If all address fields are set, jump over address and go to payment            
            toggler.toggleAndValidateCompleteArea(true, function () {
                customEventHandler.trigger(stepSsn.events.triggers.onSubmitEventComplete, "init payment");
            }, null);
        }, "#ssninput");
    },
    events: {
        listeners: {
            initEvent: "stepPhoneSubmitted",
            submitAddressEvent: "stepSsnSubmit",
        },
        triggers: {
            onSubmitEvent: "stepSsnSubmitted",
            onSubmitEventComplete: "stepAddressSubmitted",
        },
    },
    setUpEvents: function () {
        customEventHandler.subscribe(stepSsn.events.listeners.submitAddressEvent, function () {
            stepSsn.submit();
        });
        customEventHandler.subscribe(stepSsn.events.listeners.initEvent, function () {
            stepSsn.init();
        });

        // Attach event to getaddress button        
        $(stepSsn.elements.getBtnGetAddress()).click(function (e) {
            e.preventDefault();
            var btn = $(this);
            btn.button('loading');
            // Timeout to get loading state to work with ajax
            window.setTimeout(
                function() {
                    stepSsn.submit();
                    btn.button('reset');
                }, 300);           
        });

        // Attach event to manual form    
        $(stepSsn.elements.getBtnManualAddress()).click(function (e) {
            e.preventDefault();
            $(this).hide();
            customEventHandler.trigger(stepSsn.events.triggers.onSubmitEvent, "init address");
            scroller.scrollTo(stepSsn.elements.getStepAddress());
            stepSsn.elements.getStepAddress().find("input").first().focus();
        });

        $(stepSsn.elements.getSsnInput()).blur(function (e) {
            stepSsn.checkStudent($(this));
        });
    },
    checkStudent: function (ssnInput) {
        if (selectedCampaign.properties.isStudent && ssnInput.val().length) {
            studentChecker.checkStudent(ssnInput.val(), function(isStudent) {
                if (!isStudent) {
                    stepSsn.elements.getStudentConfirmContainer().fadeIn("slow");
                    disabler.disableSubmitButtons(true);
                    scroller.scrollTo(ssnInput);
                } else {
                    stepSsn.elements.getStudentConfirmContainer().fadeOut("slow");
                    disabler.disableSubmitButtons(false);
                }
            });
        } else {
            stepSsn.elements.getStudentConfirmContainer().fadeOut("slow");
            disabler.disableSubmitButtons(false);
        }
    },
    deactivate: function () {
        stepActivator.deactivateStep(stepSsn.elements.getStepSsn());
        //stepSsn.elements.getPrenStartContainer().hide();
    },
    elements: {
        getStepSsn: function () { return elementFactory.getElement(elementFactory.elements.stepSsn); },
        getStepAddress: function () { return elementFactory.getElement(elementFactory.elements.stepAddress); },
        //getPrenStartContainer: function() { return elementFactory.getElement(elementFactory.elements.getStepsContainer).find("#prenStartArea"); },
        getStudentConfirmContainer: function() { return elementFactory.getElement(elementFactory.elements.studentConfirmContainer); },
        getSsnInput: function() { return elementFactory.getElement(elementFactory.elements.stepSsn).find("input"); },
        getSsnError: function () { return elementFactory.getElement(elementFactory.elements.stepSsn).find("#ssnerror"); },
        getSsnErrorAlert: function () { return elementFactory.getElement(elementFactory.elements.stepSsn).find("#ssnerror .alert"); },
        getBtnGetAddress: function() { return elementFactory.getElement(elementFactory.elements.stepSsn).find("#btngetaddress"); },
        getBtnManualAddress: function() { return elementFactory.getElement(elementFactory.elements.stepSsn).find("#btnmanualaddress"); },
    }
}