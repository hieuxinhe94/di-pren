define(["dom/overview", "classes/subscriber"],
    function(dom, subscriber) {
        return new Overview(dom, subscriber);
    });

function Overview(dom, subscriber) {
    this.dom = dom;
    this.subscriber = subscriber;

    this.init();
}

Overview.prototype.init = function () {
    var self = this;

    // Set name if subscriber is changed
    $.subscribe("subscriberChanged", function () {
        self.setNameInHeader();
    });
}

Overview.prototype.setNameInHeader = function() {
    this.dom.nameElement.text(this.subscriber.firstName);
}