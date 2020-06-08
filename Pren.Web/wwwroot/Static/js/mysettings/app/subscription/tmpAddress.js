define([
    "jquery",
    "jqueryvalidate",
    "ext/buttonIndicator",
    "pubsub",
    "ext/serializeObject",
    "func/ajax",
    "func/date",
    "subdom/subscriptionDom",
    "underscore",
    "app/profile/comingEvents",
    "classes/subscriber",
    "text!subscriptionTemplates/tmpAddressChanges.html",
    "text!subscriptionTemplates/tmpAddresses.html"],
    function ($, validate, indicator, pubsub, serializer, ajax, date, dom, _, comingEvents, subscriber, tmpAddressTemplate, tmpAddressesTemplate) {
        return new TemporaryAddressHandler(ajax, date, dom, comingEvents, subscriber, tmpAddressTemplate, tmpAddressesTemplate);
    });


function TemporaryAddressHandler(ajax, date, dom, comingEvents, subscriber, tmpAddressTemplate, tmpAddressesTemplate) {
    this.ajax = ajax;
    this.dom = dom;
    this.tmpAddressTemplate = _.template(tmpAddressTemplate);
    this.tmpAddressesTemplate = _.template(tmpAddressesTemplate);
    this.date = date;
    this.comingEvents = comingEvents;
    this.subscriber = subscriber;
}

TemporaryAddressHandler.prototype.init = function () {
    var self = this;

    // Subscribe to subscriptionChanged
    $.subscribe("subscriptionChanged", function () {
        var subscription = self.subscriber.selectedSubscription;

        if (subscription.isDigital) {
            self.dom.tmpaddress.collapseHeading.hide();
        } else {
            self.dom.tmpaddress.collapseHeading.show();
            // Set default text in placeholder
            self.dom.tmpaddress.ph.text(self.dom.tmpaddress.ph.data("defaulttext"));
            self.dom.tmpaddress.phAddresses.text(self.dom.tmpaddress.phAddresses.data("defaulttext"));
        }
        //Always collapse panel when switching subscriptions
        self.dom.tmpaddress.collapsePanel.collapse("hide");
        //Set hidden input that will be serialized when posting form
        self.dom.tmpaddress.elementSubsNoInput.val(subscription.subscriptionNumber);
        // Set up datepicker in form
        self.date.init(subscription.closestIssueDate, subscription.endDate, $(self.dom.tmpaddress.dateinputs));
    });

    self.dom.tmpaddress.collapsePanel.on('show.bs.collapse', function () {
        // Only set forthcoming changes if not already populated
        if (!self.dom.tmpaddress.ph.children().length) {
            self.populateTemporaryAddressChanges(self.subscriber.selectedSubscription.subscriptionNumber);
        }
    });

    // Toogle rows in table (edit/save)
    self.dom.tmpaddress.area.on("click", self.dom.tmpaddress.selectors.togglebtn, function () {
        toggleEditRow($(this));
    });

    // Update row
    self.dom.tmpaddress.area.on("click", self.dom.tmpaddress.selectors.savebtn, function () {
        self.changeTemporaryAddressChange($(this).parents("tr"), self.subscriber.selectedSubscription.subscriptionNumber);        
    });

    // Delete 
    self.dom.tmpaddress.area.on("click", self.dom.tmpaddress.selectors.deletebtn, function () {
        if (!confirm(self.dom.confirm.remove)) return;
        self.deleteTemporaryAddressChanges(self.subscriber.selectedSubscription.subscriptionNumber, $(this).data("id"));
    });

    // Use existing address
    self.dom.tmpaddress.area.on("change", self.dom.tmpaddress.selectors.tmpaddresses, function () {
        var selectedAddress = $(this).find("option:selected");
        self.populateForm(selectedAddress);
    });

    // Submit form
    self.dom.tmpaddress.form.submit(function () {
        if (!self.dom.tmpaddress.form.valid()) return;

        self.dom.tmpaddress.submit.loadingIndicatorLoad(function () { return true; });
        self.saveTemporaryAddress(this, self.subscriber.selectedSubscription.subscriptionNumber);
    });

    // Set up validation
    self.dom.tmpaddress.form.validate({
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
                required: true,
                date: true
            },
            todate: {
                required: true,
                date: true
            },
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
            },
            todate: {
                required: "Vänligen ange tilldatum",
                date: "Vänligen ange ett korrekt datum"
            }
        }
    });

    // Toggle rows in forthcoming addresschanges
    function toggleEditRow(btnElement) {
        var row = $(btnElement).parents("tr");
        var elementEdit = row.find(".displaynone, span, .editBtn");

        $.each(elementEdit, function (index, element) {
            $(element).is(":visible") ? $(element).hide() : $(element).show();
        });
    }
}

TemporaryAddressHandler.prototype.populateForm = function (address) {
    this.dom.tmpaddress.forminputs.streetaddress.val(address.data("streetaddress"));
    this.dom.tmpaddress.forminputs.streetnumber.val(address.data("streetnumber"));
    this.dom.tmpaddress.forminputs.zip.val(address.data("zip"));
    this.dom.tmpaddress.forminputs.city.val(address.data("city"));
}

TemporaryAddressHandler.prototype.populateTemporaryAddressChanges = function (subscriptionNumber) {
    var self = this;

    var successCallback = function (data) {
        var addressChanges = $(self.tmpAddressTemplate({ items: data.TmpChanges }));
        self.dom.tmpaddress.ph.html(addressChanges);

        if (data.TmpChanges.length) {
            // Set up datepicker in html-template
            self.date.init(self.subscriber.selectedSubscription.closestIssueDate, self.subscriber.selectedSubscription.endDate, self.dom.tmpaddress.ph.find(self.dom.tmpaddress.selectors.dateinput));
        }

        var addresses = $(self.tmpAddressesTemplate({ items: data.TmpAddresses }));
        self.dom.tmpaddress.phAddresses.html(addresses);
    }

    var errorCallback = function () {
        $.publish("feedback", self.dom.tmpaddress.errors.load);
    }

    // Get forthcoming address changes
    self.ajax.makeAjaxRequest('/api/mysettings/subscription/tmpaddresschanges/' + subscriptionNumber, 'GET', successCallback, errorCallback);
}

TemporaryAddressHandler.prototype.saveTemporaryAddress = function (form, subscriptionNumber) {
    var self = this;

    var successCallback = function () {
        self.populateTemporaryAddressChanges(subscriptionNumber);
        self.dom.tmpaddress.forminputs.all.val("");
        self.comingEvents.setComingEvents();
        $.publish("mypage-track-event", 'skickat in tillfällig adress');
    }

    var errorCallback = function () {
        $.publish("feedback", self.dom.tmpaddress.errors.save);
    }

    var generalCallback = function () {
        self.dom.tmpaddress.submit.loadingIndicatorReset();      
    }

    var json = JSON.stringify($(form).serializeObject());

    self.ajax.makeAjaxRequest("/api/mysettings/subscription/savetmpaddress", "POST", successCallback, errorCallback, generalCallback, json);
}

TemporaryAddressHandler.prototype.deleteTemporaryAddressChanges = function (subscriptionNumber, addressId) {
    var self = this;

    var successCallback = function () {
        self.populateTemporaryAddressChanges(subscriptionNumber);
        self.comingEvents.setComingEvents();
        $.publish("mypage-track-event", 'tagit bort tillfällig adress');
    }

    var errorCallback = function () {
        $.publish("feedback", self.dom.tmpaddress.errors.remove);
    }

    var data = { Id: addressId, SubscriptionId: subscriptionNumber };
    var json = JSON.stringify(data);
    self.ajax.makeAjaxRequest('/api/mysettings/subscription/deletetmpaddress', 'POST', successCallback, errorCallback, null, json);
}

TemporaryAddressHandler.prototype.changeTemporaryAddressChange = function (row, subscriptionNumber) {
    var self = this;

    var successCallback = function () {
        self.populateTemporaryAddressChanges(subscriptionNumber);
        self.comingEvents.setComingEvents();
        $.publish("mypage-track-event", 'ändrat tillfällig adress');
    };

    var errorCallback = function () {
        $.publish("feedback", self.dom.tmpaddress.errors.save);
    }

    var startDateInput = row.find("input.startDate");
    var startDateOrg = startDateInput.data("orgdate");
    var startDate = startDateInput.val();
    var endDateInput = row.find("input.endDate");
    var endDateOrg = endDateInput.data("orgdate");
    var endDate = endDateInput.val();
    var id = row.data("id");

    var data = { id: id, subscriptionId: subscriptionNumber, fromDateOrg: startDateOrg, fromDate: startDate, toDateOrg: endDateOrg, toDate: endDate };
    var json = JSON.stringify(data);
    self.ajax.makeAjaxRequest("/api/mysettings/subscription/changetmpaddress", 'POST', successCallback, errorCallback, null, json);
}

