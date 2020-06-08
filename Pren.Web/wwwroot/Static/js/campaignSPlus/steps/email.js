/*
Dependencies:

stepHelper
elementFactory
validationHandler
customEventHandler
progressBarHelper
*/

var stepEmail = {
    init: function () {
        stepActivator.setActiveStep(stepEmail.elements.getStepEmail());

        var emailInputValue = stepEmail.elements.getEmailInput().val();
        if (emailInputValue.length) {
            emailChecker.checkEmailInput(stepEmail.elements.getEmailInput());
        }
    },
    submit: function () {

        validationHandler.validate(function () {
            
            stepEmail.elements.getEmailExistsContainer().hide();

            // Adjust container
            stepEmail.elements.getStepsContainer().removeClass("init");
            stepEmail.elements.getStepsWrapper().addClass("border");
            stepEmail.elements.getSubmitAddressBtn().text("Bekräfta uppgifter");
            stepEmail.elements.getH2().show();

            progressBarHelper.activateStep(progressBarHelper.steps.step2);

            customEventHandler.trigger(stepEmail.events.triggers.onSubmitEvent, "init phone");
        }, null);
    },
    events: {
        listeners: {
            initEvent: "initEmail",
            submitAddressEvent: "stepEmailSubmit",            
        },
        triggers: {
            onSubmitEvent: "stepEmailSubmitted",
        }
    },
    setUpEvents: function () {
        customEventHandler.subscribe(stepEmail.events.listeners.submitAddressEvent, function () {
            stepEmail.submit();
        });

        customEventHandler.subscribe(stepEmail.events.listeners.initEvent, function () {
            stepEmail.init();
        });

        stepEmail.elements.getEmailInput().blur(function (e) {
            emailChecker.checkEmailInput($(this));
        });       
    },
    deactivate: function () {
        stepActivator.deactivateStep(stepEmail.elements.getStepEmail());
    },
    elements: {
        getStepEmail: function () { return elementFactory.getElement(elementFactory.elements.stepEmail); },
        getEmailExistsContainer: function() { return elementFactory.getElement(elementFactory.elements.emailExistsContainer); },
        getStepsContainer: function() { return elementFactory.getElement(elementFactory.elements.getStepsContainer); },
        getStepsWrapper: function() { return elementFactory.getElement(elementFactory.elements.stepsWrapper); },
        getSubmitAddressBtn: function() { return elementFactory.getElement(elementFactory.elements.submitAddress).find("button"); },
        getH2: function() { return elementFactory.getElement(elementFactory.elements.prenContainer).find("h2"); },
        getEmailInput: function() { return elementFactory.getElement(elementFactory.elements.stepEmail).find("#emailinput"); },
    }
}