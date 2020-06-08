function DeletePendingHandler(ajaxHandler, bizSubscriptionId, pagerHandler) {
    var self = this;
    self.ajaxHandler = ajaxHandler;
    self.bizSubscriptionId = bizSubscriptionId;
    self.pagerHandler = pagerHandler;

    $.subscribe('biz-pending-subscriber-delete', function (notUsedElem, code, email) {
        self.deletePendingSubscriber(code, email);
    });

    $.subscribe('biz-pending-subscriber-delete-success', function (notUsedElem, result, deletedEmail) {
        self.deletePendingSubscriberSuccessfull(deletedEmail);
    });

    $.subscribe('biz-pending-subscriber-delete-fail', function (notUsedElem, deletedEmail) {
        alert('Det gick inte att ta bort ' + deletedEmail + ' testa att ladda om sidan och försök igen.');
    });

    $.subscribe('biz-get-pending-successfull-after-delete', function (notUsedElem, pendingSubscribers, deletedEmail) {
        self.publishPendingInvites(pendingSubscribers, deletedEmail);
    });
};

DeletePendingHandler.prototype.deletePendingSubscriber = function (code, email) {
    var self = this;
    var shouldDelete = confirm("Är du säker på att du vill ta bort " + email + " från din prenumeration?");
    if (shouldDelete) {

        self.ajaxHandler.makeAjaxRequest(
            '/api/biz/deletependingbizsubscriber/' + self.bizSubscriptionId + "/" + code,
            "biz-pending-subscriber-delete-success",
            email,
            "biz-pending-subscriber-delete-fail",
            email);
    }
};

DeletePendingHandler.prototype.deletePendingSubscriberSuccessfull = function (email) {
    var self = this;
    ajaxHandler.makeAjaxRequest(
        "/api/biz/getpendingbizsubscribers/" + self.bizSubscriptionId + self.pagerHandler.GetAllInCurrentScopeAsRouteParameters(), //Add parameters for paging
        "biz-get-pending-successfull-after-delete",
        email, // On successfull get we make the ajaxhandler include the deleted email in the publish parameters
        "biz-get-pending-failed-after-delete");
};

DeletePendingHandler.prototype.publishPendingInvites = function (pendingSubscribers, deletedEmail) {    
    $.each(pendingSubscribers, function (i, subscriber) {
        console.log(subscriber.email);
        if (subscriber.Email === deletedEmail) {
            console.log("Manually deleted " + subscriber.email);
            delete subscriber;
        }
    });

    $.publish("biz-pending-subscribers-get-success", [pendingSubscribers, true]);
};