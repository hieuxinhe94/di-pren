var selectedCampaign = {
    properties: {
        campIdCardAndInvoice: null,
        campIdAutogiro: null,
        title: null,
        price: null,
        periodButtonElement: null,
        isDigital: null,
        isTrial: null,
        isTrialFree: null,
        isStudent: null,
        isPayWall: null,
        hideCardPayment: null,
        hideInvoicePayment: null,
        id: null
    },
    events: {
        selectedCampaignChanged: "selectedCampaignChanged",
        beforeCampaignChanged: "beforeCampaignChanged" //Not in use
    },
    clear: function () {
        $.each(selectedCampaign.properties, function (i) {
            selectedCampaign.properties[i] = null;
        });
    },
    set: function (campIdCardAndInvoice, campIdAutogiro, title, price, isDigital, isStudent, isTrial, isTrialFree, id, periodButtonElement, isPayWall, hideCardPayment, hideInvoicePayment) {

        selectedCampaign.clear();

        this.properties.campIdCardAndInvoice = campIdCardAndInvoice;
        this.properties.campIdAutogiro = campIdAutogiro;
        this.properties.title = title;
        this.properties.price = price;
        this.properties.isDigital = isDigital;
        this.properties.isTrial = isTrial;
        this.properties.isTrialFree = isTrialFree;
        this.properties.isStudent = isStudent;
        this.properties.periodButtonElement = periodButtonElement;
        this.properties.id = id;
        this.properties.isPayWall = isPayWall;
        this.properties.hideCardPayment = hideCardPayment;
        this.properties.hideInvoicePayment = hideInvoicePayment;
        
        customEventHandler.trigger(selectedCampaign.events.selectedCampaignChanged, this.properties);

        debugHandler.showCampaignInfo();
    },
    get: function () {
        return this.properties;
    }
};