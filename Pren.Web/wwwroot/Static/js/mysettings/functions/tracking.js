define(["jquery", "pubsub", "func/error"], function ($, pubsub) {
    return new Tracking();
});

function Tracking() {    
    this.init();
}

Tracking.prototype.init = function () {
    $.subscribe("mypage-track-event", function (_, action) {
        dataLayer.push({
            'event' : 'customEvent',
            'eventCategory': 'Interaction',
            'eventLabel': 'DI MINA SIDOR',
            'eventAction': action,
        });
    });
}