define([
    "jquery",
    "pubsub",
    "subdom/subscriptionDom",
    "underscore",
    "classes/subscriber",
    "text!subscriptionTemplates/activeSubsTabs.html"],
    function ($, pubsub, dom, _, subscriber, activeSubsTemplate) {
        return new ActiveSubscriptions(dom, subscriber, activeSubsTemplate);
    });


function ActiveSubscriptions(dom, subscriber, activeSubsTemplate) {
    this.dom = dom;
    this.subscriber = subscriber;
    this.activeSubsTemplate = _.template(activeSubsTemplate);
}

ActiveSubscriptions.prototype.init = function () {
    var self = this;
    // Trigger event on subscription changed
    self.dom.tabContainer.on("click", "a", function () {
        self.subscriber.setSelectedSubscription($(this).data("subsno"), $(this).data("generation"));
    });

    // Subscribe to subscriptionChanged
    $.subscribe("subscriptionChanged", function () {
        var subscription = self.subscriber.selectedSubscription;
        
        self.dom.subscriptionInfo.subscriptionNumber.text(subscription.subscriptionNumber);
        self.dom.subscriptionInfo.startDate.text(subscription.startDate);
        self.dom.subscriptionInfo.endDate.text(subscription.endDate.indexOf("2078") == 0 ? "Tillsvidare" : subscription.endDate);
    });
}

ActiveSubscriptions.prototype.setUpActiveSubscriptionTabs = function () {

    if (this.subscriber.activeSubscriptions.length) {
        var activeSubs = $(this.activeSubsTemplate({ items: this.subscriber.activeSubscriptions }));
        activeSubs.appendTo(this.dom.tabContainer);

        // Set first subscription as active
        $(this.dom.selectors.tabs).first().trigger("click");
    }
}