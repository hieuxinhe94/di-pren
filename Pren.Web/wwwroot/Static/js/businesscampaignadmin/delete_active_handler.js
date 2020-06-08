function DeleteActiveHandler(ajaxHandler, bizSubscriptionId, pagerHandler) {
    var self = this;
    self.ajaxHandler = ajaxHandler;
    self.bizSubscriptionId = bizSubscriptionId;
    self.pagerHandler = pagerHandler;

    $.subscribe('biz-active-subscriber-delete', function (notUsedElem, subscriberId, email) {
        self.deleteActiveSubscriber(subscriberId, email);
    });

    $.subscribe('biz-active-subscriber-delete-success', function (notUsedElem, result, deletedEmail) {
        self.deleteActiveSubscriberSuccessfull(deletedEmail);
    });

    $.subscribe('biz-active-subscriber-delete-fail', function (notUsedElem, deletedEmail) {
        alert('Det gick inte markera ' + deletedEmail + ' för borttagning testa att ladda om sidan och försök igen.');
    });

    $.subscribe('biz-get-active-successfull-after-delete', function (notUsedElem, activeSubscribers, deletedEmail) {
        self.publishActiveSubscribers(activeSubscribers, deletedEmail);
    });

    $.subscribe('biz-pending-subscriber-regret-delete', function (notUsedElem, subscriberId, email) {
        self.regretDeleteActiveSubscriber(subscriberId, email);
    });

    $.subscribe('biz-active-subscriber-regret-delete-success', function (notUsedElem, result, email) {
        $.publish("biz-active-subscribers-get", self.bizSubscriptionId);
    });
};

DeleteActiveHandler.prototype.deleteActiveSubscriber = function (subscriberId, email) {
    var self = this;
    var shouldDelete = confirm("Är du säker på att du vill ta bort " + email + " från din prenumeration?");
    if (shouldDelete) {

        self.ajaxHandler.makeAjaxRequest(
            '/api/biz/markactivebizsubscriberforremoval/' + self.bizSubscriptionId + "/" + subscriberId + "/" + true,
            "biz-active-subscriber-delete-success",
            email,
            "biz-active-subscriber-delete-fail",
            email);
    }
};

DeleteActiveHandler.prototype.regretDeleteActiveSubscriber = function (subscriberId, email) {
    var self = this;
    var shouldDelete = confirm("Är du säker på att du vill ångra borttagningen av " + email + " från din prenumeration?");
    if (shouldDelete) {

        self.ajaxHandler.makeAjaxRequest(
            '/api/biz/markactivebizsubscriberforremoval/' + self.bizSubscriptionId + "/" + subscriberId + "/" + false,
            "biz-active-subscriber-regret-delete-success",
            email,
            "biz-active-subscriber-regret-delete-fail",
            email);
    }
};

DeleteActiveHandler.prototype.deleteActiveSubscriberSuccessfull = function (email) {
    var self = this;
    ajaxHandler.makeAjaxRequest(
        "/api/biz/getactivebizsubscribers/" + self.bizSubscriptionId + self.pagerHandler.GetAllInCurrentScopeAsRouteParameters(), //Add parameters for paging,
        "biz-get-active-successfull-after-delete",
        email, // On successfull get we make the ajaxhandler include the deleted email in the publish parameters
        "biz-get-active-failed-after-delete");
};

DeleteActiveHandler.prototype.publishActiveSubscribers = function (activeSubscribers, deletedEmail) {
    $.each(activeSubscribers, function (i, subscriber) {
        if (subscriber.Email === deletedEmail && subscriber.Status !== "ABORT") {
            subscriber.Status = "ABORT";
        }
    });

    $.publish("biz-active-subscribers-get-success", [activeSubscribers, true]);
};