function RemindPendingHandler(ajaxHandler, bizSubscriptionId) {
    'use strict';
    var self = this;
    self.ajaxHandler = ajaxHandler;
    self.bizSubscriptionId = bizSubscriptionId;

    $.subscribe('biz-pending-subscriber-remind', function (notUsedElem, code, email, parentElem) {
        self.remindPendingSubscriber(code, email, parentElem);
    });

    $.subscribe('biz-pending-subscriber-remind-success', function (notUsedElem, result, publishedParams) {
        self.remindPendingSubscriberSuccessfull(publishedParams.email, publishedParams.parentElem);
    });

    $.subscribe('biz-pending-subscriber-remind-fail', function (notUsedElem, email) {
        alert('Det gick inte att ta påminna ' + email + ' testa att ladda om sidan och försök igen.');
    });
}

RemindPendingHandler.prototype.remindPendingSubscriber = function (code, email, parentElem) {
    'use strict';
    var self = this;
    self.ajaxHandler.makeAjaxRequest(
        '/api/biz/remindbizsubscriber/' + self.bizSubscriptionId + "/" + code,
        "biz-pending-subscriber-remind-success",
        { email: email, parentElem: parentElem },
        "biz-pending-subscriber-remind-fail",
        email);    
};

RemindPendingHandler.prototype.remindPendingSubscriberSuccessfull = function (email, parentElem) {
    'use strict';
    var confirmBox = $("<div class='col-md-8 alert alert-info pull-right dipsplaynone'>").text("En påminnelse har skickats till " + email);
    parentElem.append(confirmBox);

    confirmBox.fadeIn("slow");
    setTimeout(function () {
        confirmBox.fadeOut("slow");        
    }, 4000);

    // Remove the confirmBox after it has faded out
    setTimeout(function () {
        confirmBox.remove();
    }, 5000);
};