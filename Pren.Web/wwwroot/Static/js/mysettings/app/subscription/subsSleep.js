define([
    "jquery",
    "pubsub",
    "jqueryvalidate",
    "ext/buttonIndicator",
    "func/ajax",
    "func/date",
    "subdom/subscriptionDom",
    "underscore",
    "app/profile/comingEvents",
    "classes/subscriber",
    "text!subscriptionTemplates/subsciptionSleeps.html"],
    function ($, pubsub, jvalidate, indicator, ajax, date, dom, _, comingEvents, subscriber, sleepTemplate) {
        return new SubscriptionSleepHandler(ajax, dom, date, comingEvents, subscriber, sleepTemplate);
    });


function SubscriptionSleepHandler(ajax, dom, date, comingEvents, subscriber, sleepTemplate) {
    this.ajax = ajax;
    this.dom = dom;
    this.date = date;
    this.comingEvents = comingEvents;
    this.subscriber = subscriber;
    this.sleepTemplate = _.template(sleepTemplate);
}

SubscriptionSleepHandler.prototype.init = function () {
    
    var self = this;

    // Subscribe to subscriptionChanged
    $.subscribe("subscriptionChanged", function () {
        var subscription = self.subscriber.selectedSubscription;

        if (subscription.isDigital) {
            self.dom.subssleep.collapseHeading.hide();
        } else {
            self.dom.subssleep.collapseHeading.show();
            // Set default text in placeholder
            self.dom.subssleep.ph.text(self.dom.subssleep.ph.data("defaulttext"));
        }
        //Always collapse panel when switching subscriptions
        self.dom.subssleep.collapsePanel.collapse("hide");
        //Set hidden input that will be serialized when posting form
        self.dom.subssleep.elementSubsNoInput.val(subscription.subscriptionNumber);
        // Set up datepicker
        self.date.init(subscription.closestIssueDate, subscription.endDate, self.dom.subssleep.area.find(self.dom.subssleep.selectors.dateinput));
    });

    // Init collapseable
    self.dom.subssleep.collapsePanel.on('show.bs.collapse', function () {
        if (!self.dom.subssleep.ph.children().length) {
            self.populateForthcomingSleeps(self.subscriber.selectedSubscription.subscriptionNumber);
        }
    });

    // Toogle rows in table (edit/save)
    self.dom.subssleep.area.on("click", self.dom.subssleep.selectors.togglebtn, function () {
        toggleEditRow($(this));
    });

    // Update row
    self.dom.subssleep.area.on("click", self.dom.subssleep.selectors.savebtn, function () {
        self.changeSubscriptionSleep($(this).parents("tr"), self.subscriber.selectedSubscription.subscriptionNumber);
    });

    // Delete row
    self.dom.subssleep.area.on("click", self.dom.subssleep.selectors.deletebtn, function () {
        if (!confirm(self.dom.confirm.remove)) return;
        self.deleteSubscriptionSleep(self.subscriber.selectedSubscription.subscriptionNumber, $(this).data("startdate"), $(this).data("enddate"));
    });

    // Form submit (create new sleep)
    self.dom.subssleep.form.submit(function () {
        if (!self.dom.subssleep.form.valid()) return;

        self.dom.subssleep.submit.loadingIndicatorLoad(function () { return true; });
        self.saveSubscriptionSleep(this, self.subscriber.selectedSubscription.subscriptionNumber);
    });

    // Set up validation
    self.dom.subssleep.form.validate({
        rules: {
            fromdate: {
                required: true
            },
            todate: {
                required: true
            }
        },
        messages: {
            fromdate: {
                required: "Vänligen ange fråndatum",
            },
            todate: {
                required: "Vänligen ange tilldatum",
            }
        }
    });

    // Toggle rows in forthcoming holidaysstops
    function toggleEditRow(btnElement) {
        var row = $(btnElement).parents("tr");
        var elementEdit = row.find(".displaynone, span, .editBtn");

        $.each(elementEdit, function (index, element) {
            $(element).is(":visible") ? $(element).hide() : $(element).show();
        });
    }
}

SubscriptionSleepHandler.prototype.populateForthcomingSleeps = function (subscriptionNumber) {
    var self = this;

    var successCallback = function (data) {

        var items = $(self.sleepTemplate({ items: data.SubscriptionSleeps }));
        self.dom.subssleep.ph.html(items);

        if (data.SubscriptionSleeps.length) {
            // Set up datepicker in html-template
            self.date.init(self.subscriber.selectedSubscription.closestIssueDate, self.subscriber.selectedSubscription.endDate, self.dom.subssleep.ph.find(self.dom.subssleep.selectors.dateinput));
        }
    };

    var errorCallback = function () {
        $.publish("feedback", self.dom.subssleep.errors.load);
    }

    self.ajax.makeAjaxRequest("/api/mysettings/subscription/getsubssleeps/" + subscriptionNumber, 'GET', successCallback, errorCallback);
}

SubscriptionSleepHandler.prototype.deleteSubscriptionSleep = function (subscriptionNumber, startDate, endDate) {
    var self = this;

    var successCallback = function () {
        self.populateForthcomingSleeps(subscriptionNumber);
        self.comingEvents.setComingEvents();
        $.publish("mypage-track-event", 'tagit bort uppehåll');
    };

    var errorCallback = function () {
        $.publish("feedback", self.dom.subssleep.errors.remove);
    }

    self.ajax.makeAjaxRequest("/api/mysettings/subscription/deletesubssleep/" + subscriptionNumber + "/" + startDate + "/" + endDate, 'POST', successCallback, errorCallback);
}

SubscriptionSleepHandler.prototype.saveSubscriptionSleep = function (form, subscriptionNumber) {
    var self = this;

    var successCallback = function () {
        self.populateForthcomingSleeps(subscriptionNumber);
        $(form).find(self.dom.subssleep.selectors.dateinput).val("");
        self.comingEvents.setComingEvents();
        $.publish("mypage-track-event", 'skickat in uppehåll');
    };

    var errorCallback = function () {
        $.publish("feedback", self.dom.subssleep.errors.save);
    }

    var generalCallback = function () {
        self.dom.subssleep.submit.loadingIndicatorReset();
    }

    var json = JSON.stringify($(form).serializeObject());
    self.ajax.makeAjaxRequest("/api/mysettings/subscription/savesubssleep", 'POST', successCallback, errorCallback, generalCallback, json);
}

SubscriptionSleepHandler.prototype.changeSubscriptionSleep = function (row, subscriptionNumber) {
    var self = this;

    var successCallback = function () {
        self.populateForthcomingSleeps(subscriptionNumber);
        self.comingEvents.setComingEvents();
        $.publish("mypage-track-event", 'ändrat uppehåll');
    };

    var errorCallback = function () {
        $.publish("feedback", self.dom.subssleep.errors.save);
    }

    var startDateInput = row.find("input.startDate");
    var startDateOrg = startDateInput.data("orgdate");
    var startDate = startDateInput.val();
    var endDateInput = row.find("input.endDate");
    var endDateOrg = endDateInput.data("orgdate");
    var endDate = endDateInput.val();
    var id = row.data("id");

    var data = { id : id, subscriptionId: subscriptionNumber, fromDateOrg: startDateOrg, fromDate: startDate, toDateOrg: endDateOrg, toDate: endDate };
    var json = JSON.stringify(data);
    self.ajax.makeAjaxRequest("/api/mysettings/subscription/changesubssleep", 'POST', successCallback, errorCallback, null, json);
}