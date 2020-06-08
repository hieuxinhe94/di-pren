var stepVisualizer = {
    setVisibleSteps: function (selectedCampaign) {
        // Function triggered when user switches between campaigns.
        // We must decide which steps that should be visible depending on type of campaign.
        var activeStep = stepActivator.getActiveStep();

        // Step email and phone requires no action
        if (!activeStep.is(stepVisualizer.elements.getStepEmail()) && !activeStep.is(stepVisualizer.elements.getStepPhone())) {

            if (selectedCampaign.isDigital) {
                stepSsn.deactivate();
                stepAddress.deactivate();
                stepPhone.init(true);
            } else {
                stepSsn.init(true);
                stepAddress.init();
            }

            stepPayment.deactivate();
            progressBarHelper.activateStep(progressBarHelper.steps.step2);
            // Reset inputs if they are minimized
            resizer.resizeStepInput(true, stepVisualizer.elements.getStepWrapper());
            var stepContainer = stepVisualizer.elements.getStepContainer();
            stepContainer.removeClass("complete");
            stepVisualizer.elements.getEditLink().hide();
            stepVisualizer.elements.getSubmitAddressBtn().show();
            stepVisualizer.elements.getSubmitFormMobileBtn().hide();
        }
    },
    elements: {
        getStepEmail: function () { return elementFactory.getElement(elementFactory.elements.stepEmail); },
        getStepPhone: function() { return elementFactory.getElement(elementFactory.elements.stepPhone); },
        getStepWrapper: function () { return elementFactory.getElement(elementFactory.elements.stepsWrapper); },
        getStepContainer: function () { return elementFactory.getElement(elementFactory.elements.getStepsContainer); },
        getSubmitAddressBtn: function () { return elementFactory.getElement(elementFactory.elements.submitAddress); },
        getSubmitFormMobileBtn: function () { return elementFactory.getElement(elementFactory.elements.submitFormMobile); },
        getEditLink: function () { return elementFactory.getElement(elementFactory.elements.getStepsContainer).find(".edit-link"); }
    }
}