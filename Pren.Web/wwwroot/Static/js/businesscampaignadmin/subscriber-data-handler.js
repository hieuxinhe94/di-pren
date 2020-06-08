function SubscriberDataHandler(ajaxHandler, apiBaseUrl, subEventNameForFetching, pubEventNameForFetching) {
    var self = this;
    self.ajaxHandler = ajaxHandler;
    self.apiBaseUrl = apiBaseUrl;
    self.subEventNameForFetching = subEventNameForFetching;
    self.pubEventNameForFetching = pubEventNameForFetching;

    $.subscribe(self.subEventNameForFetching, function (notUsedElem, bizSubscriptionId, parameters) {
        self.fetch(bizSubscriptionId, parameters);
    });
};

SubscriberDataHandler.prototype.fetch = function(bizSubscriptionId, parameters) {
    var self = this;

    ajaxHandler.makeAjaxRequest(
        self.apiBaseUrl + bizSubscriptionId + (parameters != null ? parameters : "") ,
        self.pubEventNameForFetching,
        null,
        self.pubEventNameForFetching + "-failed");
};
