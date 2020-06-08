function InformationBox($containerElement, showOnEventName) {
    'use strict';
    var self = this;
    StepListener.call(self, $containerElement, showOnEventName);

    $.subscribe('business-offer-changed', function (_, selectedOffer) {
        self.updateOfferInfo(selectedOffer);
    });
};



//InformationBox.prototype = Object.create(StepListener.prototype);