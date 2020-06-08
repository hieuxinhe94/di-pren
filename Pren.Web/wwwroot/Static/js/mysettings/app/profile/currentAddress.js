define([
    "jquery",
    "underscore",
    "classes/subscriber",
    "profdom/profileDom",
    "text!profileTemplates/currentAddress.html"],
    function ($, _, subscriber, dom, addressTmpl) {
        return new CurrentAddressHandler(dom, subscriber, addressTmpl);
    });

function CurrentAddressHandler(dom, subscriber, addressTmpl) {
    this.dom = dom;
    this.subscriber = subscriber;
    this.addressTemplate = _.template(addressTmpl);
}

CurrentAddressHandler.prototype.setCurrentAddress = function () {
    // Append address to template
    var addressChanges = $(this.addressTemplate({ address: this.subscriber }));
    addressChanges.appendTo(this.dom.currentAddressPh);
}

CurrentAddressHandler.prototype.hidePermAddressShortCut = function() {
    this.dom.currentAddressPh.find(".shortcut").hide();
}