/**
 * Object responsible for handlig click events on the business subscription offers
 * @param {jQuery object} businessOffersContainer - the element that wraps the business offers as a jQuery object
 */
function BusinessOffers(businessOffersContainer) {
    'use strict';
    var self = this;
    self.businessOffersContainer = businessOffersContainer;

    self.businessOffersContainer.find("a.tabaction").on("click", function (e) {
        e.preventDefault();
        self.businessOfferChanged($(this));
    });
}

/**
 * Function that handles when a business offer is changed. It creates a selectedOffer object with the offers parameters and fires an event
 * @param {jQuery object} businessOfferElem - the business offer element as a jQuey object
 */
BusinessOffers.prototype.businessOfferChanged = function (businessOfferElem) {
    'use strict';
    var selectedOffer = {
        id: businessOfferElem.data("id"),
        price: businessOfferElem.data("price"),
        campaignNumber: businessOfferElem.data("campaignnumber"),
        minAccounts: businessOfferElem.data("minaccounts"),
        maxAccounts: businessOfferElem.data("maxaccounts")
    };

    $.publish('business-offer-changed', [selectedOffer]);
};