var elementFactory = {
    getElement: function (element) {
        if (element._field == null) {
            element._field = $(element._selector);
        }

        return element._field;
    },
    elements: {
        sliderTopContainer: {
            _field: null,
            _selector: ".slider.container"
        },
        sliderContainer: {
            _field: null,
            _selector: ".slider-container"
        },
        campaigns: {
            _field: null,
            _selector: ".pren-selection"
        },
        prenContainer: {
            _field: null,
            _selector: "#pren-userinfo-container"
        },
        getStepsContainer: {
            _field: null,
            _selector: "#steps" // the form
        },
        stepsWrapper: {
            _field: null,
            _selector: "#step-wrapper" // the steps, excluding payment
        },
        stepEmail: {
            _field: null,
            _selector: "#stepEmail"
        },
        stepPhone: {
            _field: null,
            _selector: "#stepPhone"
        },
        stepSsn: {
            _field: null,
            _selector: "#stepSsn"
        },
        stepAddress: {
            _field: null,
            _selector: "#stepAddress"
        },
        stepPay: {
            _field: null,
            _selector: "#pay-wrapper"
        },
        summary: {
            _field: null,
            _selector: "#summary"
        },
        submitAddress: {
            _field: null,
            _selector: "#submitaddress"
        },
        submitForm: {
            _field: null,
            _selector: "#submitform button"
        },
        summarySelectedTitle: {
            _field: null,
            _selector: ".selected-title"
        },
        summarySelectedPrice: {
            _field: null,
            _selector: ".selected-price"
        },
        summaryPrenRange: {
            _field: null,
            _selector: "#pren-range-selector"
        },
        startDate: {
            _field: null,
            _selector: "#prenstartinput"
        },
        progressBar: {
            _field: null,
            _selector: ".progressbar-container"
        },
        submitFormMobile: {
            _field: null,
            _selector: "#submitformmobile button"
        },
        emailExistsContainer: {
            _field: null,
            _selector: "#emailexists"
        },
        studentConfirmContainer: {
            _field: null,
            _selector: "#notstudent"
        },
        invoiceAddressDigital: {
            _field: null,
            _selector: "#address-digital"
        },
        isServicePlusUser: {
            _field: null,
            _selector: "#isserviceplususerinput"
        },
        digitalNameArea: {
            _field: null,
            _selector: "#digitalnamearea"
        },

    }
}
