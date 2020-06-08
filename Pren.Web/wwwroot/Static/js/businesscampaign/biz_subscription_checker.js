/**
 * Object responsible for checking if email exists in S+.
 * Provides a public subscribe method that should be used to set up which events the email checker subscribes to.
 * When email is checked an event with name "checked-email" is published with a bool that indicates if email exists or not.
 */
function BizSubscriptionChecker() {
    'use strict';
    /**
     * Private function that performs the actual ajax call to check the email.
     * @param {string} userId - The userId that should be checked.
     * @param {function} callBack - A callback function that should be called with the response of the email check.
     */
    function checkBizSubscription(userId, callBack) {
        jQuery.ajax({
            url: '/api/biz/checkbizsubscription/' + userId,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                callBack(result);
            },
            async: true
        });
    }
    /**
     * Publishes the checked-email event.
     * @param {bool} emailExists - Boolean indicating if email exists or not.
     */
    function publishBizSubscriptionStatus(emailExists) {
        $.publish('checked-biz-subscription', [emailExists]);
    }

    /**
     * Adds subscription to provided event and perfoms checkemail when that event is published.
     * @param {string} eventName - The event name to subscribe to.
     */
    function addSubscription(eventName) {
        $.subscribe(eventName, function (_, userId) {
            checkBizSubscription(userId, function (hasBizSubscription) {
                publishBizSubscriptionStatus(hasBizSubscription);
            });
        });
    }

    return {
        /**
         * Public method that calls internal functions to add subscription to provided event
         * @param {string} eventName - The event name to subscribe to.
         */
        subscribe: function (eventName) {
            addSubscription(eventName);
        }
    };
}