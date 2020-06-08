var BizSubscriptionCountHandler = function (ajaxHandler, bizSubscriptionId, invitedCountElem, activeCountElem, totalPriceElem, progressBarElem, mindebAccountElem, pricePerAccount, minNumberOfAllowedSubscribers) {
    var self = this;
    self.ajaxHandler = ajaxHandler;
    self.bizSubscriptionId = bizSubscriptionId;
    self.invitedCountElem = invitedCountElem;
    self.activeCountElem = activeCountElem;
    self.totalPriceElem = totalPriceElem;
    self.pricePerAccount = pricePerAccount;
    self.minNumberOfAllowedSubscribers = minNumberOfAllowedSubscribers;
    self.progressBarElem = progressBarElem;
    self.mindebAccountElem = mindebAccountElem;

    $.subscribe('biz-pending-subscribers-get-success', function () {
        self.fetch(self.bizSubscriptionId);
    });

    $.subscribe('biz-count-get-success', function (notUsedElem, countData) {
        self.updateCount(countData);
    });
};

BizSubscriptionCountHandler.prototype.fetch = function (bizSubscriptionId) {
    var self = this;
    self.ajaxHandler.makeAjaxRequest(
        "/api/biz/getbizsubscriptioncount/" + bizSubscriptionId,
        "biz-count-get-success",
        null,
        "biz-count-get-failed");   
}

BizSubscriptionCountHandler.prototype.updateCount = function (countData) {
    var self = this;

    // Number of subscribers to pay for is number of active subscribers or min number of allowed subscribers if active subscribers is less than that.
    var subscribersToPayFor = countData.ActiveSubscribers;
    if (subscribersToPayFor < self.minNumberOfAllowedSubscribers) {
        subscribersToPayFor = self.minNumberOfAllowedSubscribers;
    }

    self.mindebAccountElem.text(subscribersToPayFor);

    self.progressBarElem.data("maxvalue", countData.ActiveSubscribers + countData.PendingSubscribers);

    self.invitedCountElem.text(countData.ActiveSubscribers + countData.PendingSubscribers);
    self.activeCountElem.text(countData.ActiveSubscribers);
    self.totalPriceElem.text(subscribersToPayFor * self.pricePerAccount);

    var progressWidth = (countData.ActiveSubscribers / self.progressBarElem.data("maxvalue")) * 100;
    self.progressBarElem.find("span").css("width", progressWidth + "%");
}
