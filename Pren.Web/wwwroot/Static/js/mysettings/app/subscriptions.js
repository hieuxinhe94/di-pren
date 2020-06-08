define([
    "jquery",
    "jqueryvalidate",
    "pubsub",
    "classes/subscriber",
    "app/subscription/activesubscriptions",
    "app/subscription/reclaim",
    "app/subscription/subssleep",
    "app/subscription/tmpAddress",
    "app/subscription/permAddress",
    "subdom/subscriptionDom",
    "func/ajax"],
    function ($, jqvalidate, pubsub, subscriber, activesubs, reclaim, subssleep, tmpaddress, permaddress, dom, ajax) {
        return new SubscriptionHandler(subscriber, activesubs, reclaim, subssleep, tmpaddress, permaddress, dom, ajax);
    });

function SubscriptionHandler(subscriber, activeSubscriptions, reclaim, subssleep, tmpaddress, permaddress, dom, ajax) {
    this.subscriber = subscriber;
    this.activeSubscriptions = activeSubscriptions;
    this.reclaim = reclaim;
    this.subssleep = subssleep;
    this.tmpaddress = tmpaddress;
    this.permaddress = permaddress;
    this.dom = dom;
    this.ajax = ajax;
}

SubscriptionHandler.prototype.init = function () {
    var self = this;

    $.subscribe("subscriptionChanged", function () {
        //Always show info as default when switching subscriptions
        self.dom.subscriptionInfo.collapseHeading.addClass("active");
        self.dom.subscriptionInfo.collapsePanel.collapse("show");
    });

    this.dom.collapsibles.on('show.bs.collapse', function () {
        // Hide all siblings
        $(this).siblings().collapse("hide");

        // Get heading element
        var headingId = $(this).attr("aria-labelledby");
        var headingElement = $("#" + headingId + " a");

        // Disable click by destroying href
        headingElement.attr("href", "#");

        // Add active class
        headingElement.parent().siblings().removeClass("active");
        headingElement.parent().addClass("active");
    });

    this.dom.collapsibles.on('hide.bs.collapse', function (e) {
        if (e.namespace) {
            var headingId = $(this).attr("aria-labelledby");
            var headingElement = $("#" + headingId + " a");
            headingElement.attr("href", "#" + $(this).attr("Id"));

            headingElement.parent().removeClass("active");
        }
    });

    $("#cancelForm").submit(function () {
        if (!$(this).valid()) return;

        var json = JSON.stringify($("#cancelForm").serializeObject());

        self.ajax.makeAjaxRequest("/api/mysettings/contactcustomerservice/cancel",
            "POST",
            function () {
                $("#cancelForm").hide();
                $("#cancel-thank-you-message").removeClass("hidden").show();
            },
            function () { alert('Ett fel uppstod, var vänlig kontakta oss på kundservice@di.se'); },
            function () { },
            json);
    });

    // Set up validation
    $("#cancelForm").validate({
        rules: {
            name: {
                required: true
            },
            phone: {
                required: true
            },
            email: {
                required: true,
                email: true
            },
            customernumber: {
                required: true,
                number: true
            }
        },
        messages: {
            name: {
                required: "Vänligen ange ditt namn"
            },
            phone: {
                required: "Vänligen ange ditt telefonnummer"
            },
            email: {
                required: "Vänligen ange din e-postadress",
                email: "Du har angivit en felaktig e-postadress"
            },
            customernumber: {
                required: "Vänligen ange ditt kundnummer",
                number: "Ogiltigt kundnummer"
            }
        }
    });
}

SubscriptionHandler.prototype.setUpSubscriptions = function () {
    this.reclaim.init();
    this.subssleep.init();
    this.tmpaddress.init();
    this.permaddress.init();
    this.activeSubscriptions.init();
    this.activeSubscriptions.setUpActiveSubscriptionTabs();
}

SubscriptionHandler.prototype.setUpCancelForm = function () {
    $("#cancelnameinput").val(this.subscriber.firstName + " " + this.subscriber.lastName);
    $("#cancelphoneinput").val(this.subscriber.phone);
    $("#cancelemailinput").val(this.subscriber.email);
    $("#cancelcusnoinput").val(this.subscriber.customerNumber);
}

SubscriptionHandler.prototype.hideSubscriptionsInfo = function () {    
    this.dom.subscriptionInfoContainer.hide();
}