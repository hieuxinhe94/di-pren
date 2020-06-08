define([
    "jquery",
    "underscore",
    "pubsub",
    "func/ajax",
    "profdom/eventsDom",
    "subdom/subscriptionDom",
    "classes/subscriber",
    "text!profileTemplates/comingEvents.html"],
    function ($, _, pubsub, ajax, dom, subdom, subscriber, eventsTmpl) {
        return new ComingEvents(ajax, dom, subdom, subscriber, eventsTmpl);
    });

function ComingEvents(ajax, dom, subdom, subscriber, eventsTmpl) {
    this.ajax = ajax;
    this.dom = dom;
    this.subdom = subdom;
    this.subscriber = subscriber;
    this.eventsTemplate = _.template(eventsTmpl);
}

ComingEvents.prototype.setComingEvents = function () {

    var self = this;

    var successCallback = function (data) {
        var events = $(self.eventsTemplate({ profileEvents: data }));
        self.dom.eventsContainer.html(events);
    };

    var errorCallback = function () {
        // Do nothing
    }

    self.ajax.makeAjaxRequest("/api/mysettings/profile/events", 'GET', successCallback, errorCallback);
}
