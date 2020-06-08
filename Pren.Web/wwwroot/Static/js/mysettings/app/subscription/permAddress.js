define([
    "jquery",
    "jqueryvalidate",
    "ext/buttonIndicator",
    "ext/serializeObject",
    "subdom/subscriptionDom",
    "func/ajax",
    "func/date",
    "underscore",
    "classes/subscriber",
    "app/profile/comingEvents",
    "text!subscriptionTemplates/permAddressChanges.html"],
    function ($, jvalidate, indicator, serializer, dom, ajax, date, _, subscriber, comingEvents, addressTmpl) {
        return new PermanenetAddressHandler(dom, ajax, date, subscriber, comingEvents, addressTmpl);
    });

function PermanenetAddressHandler(dom, ajax, date, subscriber, comingEvents, addressTmpl) {
    this.dom = dom;
    this.ajax = ajax;
    this.date = date;
    this.subscriber = subscriber;
    this.comingEvents = comingEvents;
    this.addressTemplate = _.template(addressTmpl);
}

PermanenetAddressHandler.prototype.init = function () {
    var self = this;

    $("#permaddressCollapse").on('show.bs.collapse', function () {
        // Only set forthcoming changes if not already populated
        if (!self.dom.permaddress.permAddressChangesPh.children().length) {
            self.populateForthcomingAddressChanges();
        }
    });

    $("#permaddressCollapse").on("click", self.dom.permaddress.selectors.editbtn, function () {
        self.editPermanentAddress();
    });

    $("#permaddressCollapse").on("click", self.dom.permaddress.selectors.deletebtn, function () {
        self.deletePermanentAddress();
    });

    self.dom.permaddress.newAddressForm.submit(function () {
        if (!self.dom.permaddress.newAddressForm.valid()) return;

        self.dom.permaddress.newAddressFormBtn.loadingIndicatorLoad(function () { return true; });
        self.savePermanentAddress(this);
    });

    // Set default text in placeholder
    this.dom.permaddress.permAddressChangesPh.text(this.dom.permaddress.permAddressChangesPh.data("defaulttext"));

    // Set up datepicker in form
    this.date.init(this.subscriber.closestIssueDate, this.subscriber.maxEndDate, this.dom.permaddress.newAddressFormDateInputs);

    // Set up validation
    self.dom.permaddress.newAddressForm.validate({
        rules: {
            streetaddress: {
                required: true
            },
            zip: {
                required: true,
                number: true,
                maxlength: 5
            },
            city: {
                required: true
            },
            fromdate: {
                required: true
            }
        },
        messages: {
            streetaddress: {
                required: "Vänligen ange gatuadress eller box"
            },
            zip: {
                required: "Vänligen ange ditt postnummer XXXXX",
                number: "Endast siffror, utan mellanslag"
            },
            city: {
                required: "Vänligen ange din ort"
            },
            fromdate: {
                required: "Vänligen ange fråndatum",
                date: "Vänligen ange ett korrekt datum"
            }
        }
    });    
}

PermanenetAddressHandler.prototype.populateForthcomingAddressChanges = function () {
    var self = this;

    var successCallback = function (data) {
        var addressChanges = $(self.addressTemplate({ items: data }));
        self.dom.permaddress.permAddressChangesPh.html(addressChanges);
    }

    var errorCallback = function () {
        $.publish("feedback", self.dom.permaddress.errors.load);
    }    

    // Get forthcoming address changes
    self.ajax.makeAjaxRequest('/api/mysettings/profile/addresschanges', 'GET', successCallback, errorCallback);
}

PermanenetAddressHandler.prototype.savePermanentAddress = function (form) {
    var self = this;
   
    var successCallback = function () {
        self.populateForthcomingAddressChanges();
        self.dom.permaddress.newAddressFormInputs.val(""); //clear all inputs
        self.comingEvents.setComingEvents();
        $.publish("mypage-track-event", 'skickat in permanent adress');
    }

    var errorCallback = function () {
        $.publish("feedback", self.dom.permaddress.errors.save);
    }

    var generalCallback = function() {
        self.dom.permaddress.newAddressFormBtn.loadingIndicatorReset();
    }

    var json = JSON.stringify($(form).serializeObject());
    self.ajax.makeAjaxRequest("/api/mysettings/profile/savepermaddress", "POST", successCallback, errorCallback, generalCallback, json);
}

PermanenetAddressHandler.prototype.deletePermanentAddress = function () {
    var self = this;

    if (!confirm(self.dom.permaddress.confirm.remove)) return;

    var successCallback = function () {
        self.populateForthcomingAddressChanges();
        self.comingEvents.setComingEvents();
        $.publish("mypage-track-event", 'tagit bort permanent adress');
    }

    var errorCallback = function () {
        $.publish("feedback", self.dom.permaddress.errors.remove);
    }

    self.ajax.makeAjaxRequest("/api/mysettings/profile/deletepermaddress", "POST", successCallback, errorCallback);
}

PermanenetAddressHandler.prototype.editPermanentAddress = function () {
    var self = this;

    var successCallback = function (data) {
        self.dom.permaddress.form.co.val(data.Co);
        self.dom.permaddress.form.streetAddress.val(data.StreetAddress);
        self.dom.permaddress.form.streetNo.val(data.StreetNo);
        self.dom.permaddress.form.stairCase.val(data.StairCase);
        self.dom.permaddress.form.stairs.val(data.Stairs);
        self.dom.permaddress.form.apartmentNo.val(data.ApartmentNumber);
        self.dom.permaddress.form.zip.val(data.Zip);
        self.dom.permaddress.form.city.val(data.City);
        self.dom.permaddress.form.fromDate.val(data.FromDate.substring(0, 10));
        $.publish("mypage-track-event", 'ändrat permanent adress');
    }

    var errorCallback = function () {
        $.publish("feedback", self.dom.permaddress.errors.edit);
    }

    // Get address change
    self.ajax.makeAjaxRequest('/api/mysettings/profile/editpermaddress', 'GET', successCallback, errorCallback);
}
