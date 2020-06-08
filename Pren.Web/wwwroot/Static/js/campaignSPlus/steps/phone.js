/*
Dependencies:

stepHelper
elementFactory
validationHandler
customEventHandler
*/



var stepPhone = {    
    init: function (disableScroll) {

        stepActivator.setActiveStep(stepPhone.elements.getStepPhone());

        if (disableScroll == undefined || !disableScroll) {
            scroller.scrollTo(stepPhone.elements.getStepPhone());
            stepPhone.elements.getPhoneInput().focus();
        }                

        // Show summary
        stepPhone.elements.getSummary().addClass("active");
    },
    submit: function() {

        //if (selectedCampaign.properties.isDigital && !selectedCampaign.properties.isStudent) {
        //    toggler.toggleAndValidateCompleteArea(true,
        //        function () {
        //            customEventHandler.trigger(stepPhone.events.triggers.onSubmitEventDigital, "init payment");
        //        }, null);
        //}
        //else {
            validationHandler.validate(
                function() {
                    customEventHandler.trigger(stepPhone.events.triggers.onSubmitEvent, "init ssn");
                }, null);
        //}

    },
    events: {
        listeners: {
            initEvent: "stepEmailSubmitted",
            submitAddressEvent: "stepPhoneSubmit",            
        },
        triggers: {
            onSubmitEvent: "stepPhoneSubmitted",
            onSubmitEventDigital: "stepAddressSubmitted",
        },        
    },
    setUpEvents: function() {
        customEventHandler.subscribe(stepPhone.events.listeners.submitAddressEvent, function() {
            stepPhone.submit();
        });
        customEventHandler.subscribe(stepPhone.events.listeners.initEvent, function () {
            stepPhone.init();
        });
    },
    deactivate: function() {
        stepActivator.deactivateStep(stepPhone.elements.getStepPhone());        
    },
    elements: {        
        getStepPhone: function () { return elementFactory.getElement(elementFactory.elements.stepPhone); },
        getSummary: function() { return elementFactory.getElement(elementFactory.elements.summary); },
        getPhoneInput: function () { return elementFactory.getElement(elementFactory.elements.stepPhone).find("input"); },
    }
}