var toggler = {
    toggleAndValidateCompleteArea: function (isSaveAction, callback, ignoreContainerElements) {

        var stepContainer = toggler.elements.getStepContainer();
        // Before toogle, check that form is ok
        validationHandler.validate(function () {

            // Add style to input
            isSaveAction ? stepContainer.addClass("complete") : stepContainer.removeClass("complete");
            resizer.resizeStepInput(!isSaveAction, toggler.elements.getStepWrapper());

            // Show edit form button
            toggler.elements.getEditLink().show().text(isSaveAction ? "Ändra" : "Spara");
            
            // Hide confirm address button
            toggler.elements.getSubmitAddressBtn().hide();

            if (callback != null) {
                callback();
            }

            progressBarHelper.activateStep(progressBarHelper.steps.step3);

        }, ignoreContainerElements);
    },
    elements: {
        getStepContainer: function () { return elementFactory.getElement(elementFactory.elements.getStepsContainer); },
        getEditLink: function () { return elementFactory.getElement(elementFactory.elements.getStepsContainer).find(".edit-link"); },
        getStepWrapper: function () { return elementFactory.getElement(elementFactory.elements.stepsWrapper); },
        getSubmitAddressBtn: function () { return elementFactory.getElement(elementFactory.elements.submitAddress); },                       
    }
}