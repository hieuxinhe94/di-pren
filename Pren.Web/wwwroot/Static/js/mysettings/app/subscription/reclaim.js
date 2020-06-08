define([
    "jquery",
    "bootstrap",
    "pubsub",
    "ext/serializeObject",
    "func/ajax",
    "subdom/subscriptionDom",
    "underscore",
    "classes/subscriber",
    "text!subscriptionTemplates/reclaimDetails.html"],
    function ($, bootstrap, pubsub, serializer, ajax, dom, _, subscriber, reclaimDetailsTemplate) {
        return new Reclaim(ajax, dom, subscriber, reclaimDetailsTemplate);
    });


function Reclaim(ajax, dom, subscriber, reclaimDetailsTemplate) {
    this.ajax = ajax;
    this.dom = dom;
    this.subscriber = subscriber;
    this.reclaimDetailsTemplate = _.template(reclaimDetailsTemplate);
}

Reclaim.prototype.init = function () {
    var self = this;

    // Subscribe to subscriptionChanged
    $.subscribe("subscriptionChanged", function () {

        var subscription = self.subscriber.selectedSubscription;
        
        if (subscription.isDigital || !subscription.isActive) {
            self.dom.reclaim.collapseHeading.hide();            
        } else {
            self.dom.reclaim.collapseHeading.show();
            // Set default text in placeholder
            self.dom.reclaim.ph.text(self.dom.reclaim.ph.data("defaulttext"));
        }
        //Always collapse panel when switching subscriptions
        self.dom.reclaim.collapsePanel.collapse("hide");
        //Set hidden input that will be serialized when posting form
        self.dom.reclaim.elementSubsNoInput.val(subscription.subscriptionNumber);
    });

    // Init collapseable
    self.dom.reclaim.collapsePanel.on('show.bs.collapse', function () {
        if (!self.dom.reclaim.ph.children().length) {
            self.populateReclaimDetails(self.subscriber.selectedSubscription.subscriptionNumber);
        }
    });

    // Submit form
    self.dom.reclaim.form.submit(function () {
        if (!self.dom.reclaim.form.valid()) return;
        self.dom.reclaim.submit.loadingIndicatorLoad(function () { return true; });
        self.sendReclaim(this);
    });

    // Set up validation
    self.dom.reclaim.form.validate({
        errorPlacement: function (error, element) {
            if (element.is(':checkbox')) {
                error.insertAfter(element.parent().parent().parent());
            } else {
                error.insertAfter(element); // <- the default
            }
        },
        rules: {
            daystoreclaim: {
                required: true
            }
        },
        messages: {
            daystoreclaim: {
                required: "Vänligen ange dag att reklamera"
            }
        }
    });
}

Reclaim.prototype.sendReclaim = function (form) {
    var self = this;

    var successCallback = function () {
        // Disable selected checkboxes
        self.dom.reclaim.form.find("input[type=checkbox]:checked").prop("disabled", true).parent().addClass("disabled");
        // Uncheck selected checkboxes
        self.dom.reclaim.form.find("input[type=checkbox]").prop("checked", false);
        $.publish("feedback", self.dom.reclaim.feedback.save);
        $.publish("mypage-track-event", 'skickat reklamation');
    };

    var errorCallback = function () {
         $.publish("feedback", self.dom.reclaim.errors.save);
    }

    var generalCallback = function () {
        self.dom.reclaim.submit.loadingIndicatorReset();
    }

    var json = JSON.stringify($(form).serializeObject("daystoreclaim"));
    self.ajax.makeAjaxRequest("/api/mysettings/subscription/savereclaim", "POST", successCallback, errorCallback, generalCallback, json);
}

Reclaim.prototype.populateReclaimDetails = function(subscriptionNumber) {
    var self = this;

    var successCallback = function (data) {
        if (data != null) {
            var details = $(self.reclaimDetailsTemplate({ data: data }));
            self.dom.reclaim.ph.html(details);
        }
    };

    var errorCallback = function () {
        $.publish("feedback", self.dom.reclaim.errors.load);
    }
    
    self.ajax.makeAjaxRequest('/api/mysettings/subscription/getreclaimdetails/' + subscriptionNumber, 'GET', successCallback, errorCallback);
}

