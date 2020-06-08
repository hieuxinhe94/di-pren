
define(["jquery", "pubsub", "dom/misc", "classes/subscriber"], function ($, pubsub, dom, subscriber) {
    return new Shortcut(dom, subscriber);
});

function Shortcut(dom, subscriber) {
    this.dom = dom;
    this.subscriber = subscriber;
}

Shortcut.prototype.init = function () {
    var self = this;

    // Trigger shourtcut on click
    $("body").on("click", self.dom.shortCuts.selector, function () {
        var shortcut = $(this).data("shortcut");
        self.triggerShortcut(shortcut);
    });

    // Disable shortcuts if no subscriptions
    if (self.subscriber.activeSubscriptions == null || !self.subscriber.activeSubscriptions.length) {
        $(self.dom.shortCuts.selector).addClass("disabled");
    }

    // Subscribe to event subscriptionChanged. 
    // If a digital subscription is selected, disable shortcut buttons
    // If a not active subscription is selected, disable only reclaim
    $.subscribe("subscriptionChanged", function () {        
        if (self.subscriber.selectedSubscription.isDigital) {
            $(self.dom.shortCuts.selector).not('[data-shortcut="permaddresschange"]').addClass("disabled");
        }
        else if (!self.subscriber.selectedSubscription.isActive) {
            $(self.dom.shortCuts.selector).filter('[data-shortcut="reclaim"]').addClass("disabled");
        } else {
            $(self.dom.shortCuts.selector).removeClass("disabled");
        }
    });
}

Shortcut.prototype.triggerShortcutFromHash = function () {
    // If shortcut provided as hash. Trigger shortcut.
    var shortcutHash = location.hash;
    this.triggerShortcut(shortcutHash.substring(1));
}

Shortcut.prototype.triggerShortcut = function (shortcut) {
    var self = this;
    var element = null;

    switch (shortcut) {
        case "subssleep":
            element = self.dom.shortCuts.subssleep;
            break;
        case "tmpaddresschange":
            element = self.dom.shortCuts.tmpaddress;
            break;
        case "reclaim":
            element = self.dom.shortCuts.reclaim;
            break;
        case "permaddresschange":
            element = self.dom.shortCuts.permaddress;
            break;
    }

    if (element != null) {

        $('html, body').animate({
            scrollTop: self.dom.subscriptionFunctionArea.offset().top - (self.dom.navigationcontainer.height() + 10)
        }, 'slow');

        element.trigger("click");
    }
}
