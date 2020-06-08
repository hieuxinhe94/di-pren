/*
Dependencies:

stepSSn
stepPayment
stepHelper
elementFactory
validationHandler
customEventHandler
*/

var stepAddress = {    
    init: function () {
        stepActivator.setActiveStep(stepAddress.elements.getStepAddress());
    },
    submit: function () {
        // If student campaign, ssn is madatory if studencampaign
        var ignoreElements = selectedCampaign.properties.isStudent ? null : [stepAddress.elements.getStepSsn()];

        toggler.toggleAndValidateCompleteArea(true, function() {
            //stepPayment.init();
            customEventHandler.trigger(stepAddress.events.triggers.onSubmitEvent, "init payment");
        }, ignoreElements);
    },
    events: {
        //submitAddressEvent: "stepAddressSubmit"
        listeners: {
            initEvent: "stepSsnSubmitted",
            submitAddressEvent: "stepAddressSubmit",
        },
        triggers: {
            onSubmitEvent: "stepAddressSubmitted"
        },
    },
    setUpEvents: function () {
        customEventHandler.subscribe(stepAddress.events.listeners.submitAddressEvent, function () {
            stepAddress.submit();
        });
        customEventHandler.subscribe(stepAddress.events.listeners.initEvent, function () {
            stepAddress.init();
        });

        // Attach event to edit link            
        $(stepAddress.elements.getEditLink()).click(function (e) {
            e.preventDefault();
            toggler.toggleAndValidateCompleteArea(!stepAddress.elements.getStepsContainer().hasClass("complete"), null, [stepAddress.elements.getStepPay(), stepAddress.elements.getStepSsn()]);
        });

        // Attach event to add buttons      
        $(stepAddress.elements.getAddButtons()).click(function (e) {
            e.preventDefault();
            $(this).toggleClass("redhollow");
            stepAddress.elements.getStepAddress().find($(this).data("actionselector")).toggle();
            var btnText = $(this).hasClass("redhollow") ? "Ta bort " + $(this).data("textsuffix") : "Lägg till " + $(this).data("textsuffix");
            $(this).text(btnText);
        });

        //Event for getting city
        $(stepAddress.elements.getZipInput()).bind("change keyup", function (event) {
            var zip = $(this).val();
            postNameHandler.setPostName(zip, stepAddress.elements.getCityInput());
        });

    },
    populateAddressFields: function (result) {                                
        stepAddress.elements.getFirstNameInput().val(result.FirstNames);
        stepAddress.elements.getLastNameInput().val(result.LastNames);
        stepAddress.elements.getStreetAddressInput().val(result.StreetAddress);
        stepAddress.elements.getStreetNoInput().val(result.HouseNumber);
        stepAddress.elements.getStairCaseInput().val(result.StairCase);
        //stepAddress.elements.getStairsInput().val(result.Stairs);
        stepAddress.elements.getZipInput().val(result.ZipCode);
        stepAddress.elements.getCityInput().val(result.City);
        stepAddress.elements.getCompanyInput().val(result.Name);
        $("#originalinfoinput").val(result.OriginalInfo);

        if (result.Name != null && result.Name.length) {
            stepAddress.elements.getAddCompanyBtn().click();
        }
    },
    deactivate: function () {
        stepActivator.deactivateStep(stepAddress.elements.getStepAddress());
    },
    elements: {
        getStepAddress: function () { return elementFactory.getElement(elementFactory.elements.stepAddress); },
        getStepSsn: function () { return elementFactory.getElement(elementFactory.elements.stepSsn); },
        getStepPay: function () { return elementFactory.getElement(elementFactory.elements.stepPay); },
        getStepsContainer: function () { return elementFactory.getElement(elementFactory.elements.getStepsContainer); },
        getFirstNameInput: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#firstnameinput"); },
        getLastNameInput: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#lastnameinput"); },
        getStreetAddressInput: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#streetaddressinput"); },
        getStreetNoInput: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#streetnoinput"); },
        getStairCaseInput: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#staircaseinput"); },
        //getStairsInput: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#stairsinput"); },
        getZipInput: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#zipinput"); },
        getCityInput: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#cityinput"); },
        getCompanyInput: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#companyinput"); },
        getAddCompanyBtn: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#addcompany"); },
        getEditLink: function () { return elementFactory.getElement(elementFactory.elements.getStepsContainer).find(".edit-link"); },
        getAddButtons: function () { return elementFactory.getElement(elementFactory.elements.stepAddress).find("#addcompany, #addco"); }
    }
}