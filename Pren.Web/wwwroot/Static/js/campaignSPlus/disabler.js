var disabler = {
    disableSubmitButtons: function (disabled) {
        disabler.elements.getSubmitFormBtn().attr("disabled", disabled);
        disabler.elements.getSubmitAddressBtn().attr("disabled", disabled);
        disabler.elements.getSubmitFormMobileBtn().attr("disabled", disabled);
    },
    elements: {
        getSubmitFormBtn: function () { return elementFactory.getElement(elementFactory.elements.submitForm); },
        getSubmitAddressBtn: function () { return elementFactory.getElement(elementFactory.elements.submitAddress).find("button"); },
        getSubmitFormMobileBtn: function () { return elementFactory.getElement(elementFactory.elements.submitFormMobile); },
    }

}