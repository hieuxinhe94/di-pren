var autoPopulateHandler = {
    initAutopopulatedForm: function() {
        // Steps can be autopopulated for logged in users or by guid in url
        // If required input is prepopulated we will step forward by magic     
        var emailInput = autoPopulateHandler.elements.getStepEmailInput();

        if (emailInput.val().length) {
           
            // If user is logged in, show another label with possiblity to log out
            if (autoPopulateHandler.elements.getIsServicePlutUserInput().val() == "true") {
                autoPopulateHandler.elements.getEmailLoggedInLabel().show();
                autoPopulateHandler.elements.getEmailNotLoggedInLabel().hide();
                // If email is populated we do not allow it to be changed                      
                emailInput.attr("readonly", "readonly");
            }

            if (autoPopulateHandler.elements.getIsServicePlutUserInput().val() == "false") {
                var valid = emailChecker.checkEmailInput(emailInput);
                if (!valid) {
                    return;
                }                
            }

            // Submit step email
            customEventHandler.trigger(autoPopulateHandler.events.triggers.emailSubmit, "email submit");

            if (autoPopulateHandler.elements.getPhoneInput().val().length) {
                // Submit step phone
                customEventHandler.trigger(autoPopulateHandler.events.triggers.phoneSubmit, "phone submit");

                if (!selectedCampaign.properties.isDigital && autoPopulateHandler.elements.getStreetAddressInput().val().length) {
                    customEventHandler.trigger(autoPopulateHandler.events.triggers.adressInit, "address init");
                    customEventHandler.trigger(autoPopulateHandler.events.triggers.adressSubmit, "address submit");                    
                }
            }
        }
    },
    events: {
        triggers: {
            emailSubmit: "stepEmailSubmit",
            phoneSubmit: "stepPhoneSubmit",
            adressInit: "stepSsnSubmitted",
            adressSubmit: "stepAddressSubmit"
        }
    },
    elements: {
        getEmailExistsContainer: function () { return elementFactory.getElement(elementFactory.elements.emailExistsContainer); },
        getStepEmailInput: function () { return elementFactory.getElement(elementFactory.elements.stepEmail).find("#emailinput"); },
        getEmailLoggedInLabel: function () { return elementFactory.getElement(elementFactory.elements.stepEmail).find("#loggedinemaillabel"); },
        getEmailNotLoggedInLabel: function () { return elementFactory.getElement(elementFactory.elements.stepEmail).find("#notloggedinemaillabel"); },
        getPhoneInput: function () { return elementFactory.getElement(elementFactory.elements.stepPhone).find("#phoneinput"); },
        getStreetAddressInput: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#streetaddressinput"); },
        getIsServicePlutUserInput: function () { return elementFactory.getElement(elementFactory.elements.isServicePlusUser); }
    }
}