define([
    "jquery",
    "func/ajax",
    "profdom/invoiceDom",
    "underscore",
    "classes/subscriber",
    "text!profileTemplates/myInvoices.html"],
    function ($, ajax, dom, _, subscriber, invoiceTemplate) {
        return new Invoices(ajax, dom, subscriber, invoiceTemplate);
    });

function Invoices(ajax, dom, subscriber, invoiceTemplate) {
    this.ajax = ajax;
    this.dom = dom;
    this.subscriber = subscriber;
    this.template = _.template(invoiceTemplate);
    // Set default text in placeholder
    this.dom.ph.text(this.dom.ph.data("defaulttext"));
}

Invoices.prototype.populateInvoices = function () {
    var self = this;

    var successCallback = function (data) {
        var invoices = $(self.template({ customerNumber: self.subscriber.customerNumber, invoices: data }));
        self.dom.ph.html(invoices);
    };

    self.ajax.makeAjaxRequest("/api/mysettings/profile/getinvoices", 'GET', successCallback);
}
