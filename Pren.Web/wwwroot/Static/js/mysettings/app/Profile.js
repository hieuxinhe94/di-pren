define([
    "jquery",
    "app/profile/currentAddress",
    "app/profile/contactInfo",
    "app/profile/invoices",
    "app/profile/comingEvents",
    "profdom/profileDom"],
    function ($, currentAddress, contactInfo, invoices, comingEvents, dom) {
        return new ProfileHandler(currentAddress, contactInfo, invoices, comingEvents, dom);
    });

function ProfileHandler(currentAddress, contactInfo, invoices, comingEvents, dom) {
    this.currentAddress = currentAddress;
    this.contactInfo = contactInfo;
    this.invoices = invoices;
    this.comingEvents = comingEvents;
    this.exists = dom.block.length;
} 

ProfileHandler.prototype.setUpProfile = function () {
    // Set contact info
    this.contactInfo.init();
    this.contactInfo.setContactInfo();
    // Set events
    this.comingEvents.setComingEvents();
    // Set address
    this.currentAddress.setCurrentAddress();
    // Get invoices
    this.invoices.populateInvoices();
}

ProfileHandler.prototype.hideSubscriptionsInfo = function () {
    this.currentAddress.hidePermAddressShortCut();
}