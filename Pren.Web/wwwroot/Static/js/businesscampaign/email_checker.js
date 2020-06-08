function EmailChecker() {
    /// <summary>
    /// Object responsible for checking if email exists in S+.
    /// Provides a public subscribe method that should be used to set up which events the email checker subscribes to.
    /// When email is checked an event with name "checked-email" is published with a bool that indicates if email exists or not.
    /// </summary>
    function checkEmail(emailToCheck, callBack) {
        /// <summary>
        /// Private function that performs the actual ajax call to check the email.
        /// </summary>
        /// <param name="emailToCheck">The email that should be checked.</param>
        /// <param name="callBack">A callback function that should be called with the response of the email check.</param>

        jQuery.ajax({
            method: "POST",
            url: '/api/emailcheck',
            data: JSON.stringify({ email: emailToCheck }),
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                callBack(result);
            },
            async: true
        });
    }

    function publishEmailStatus(emailExists) {
        /// <summary>
        /// Publishes the checked-email event.
        /// </summary>
        /// <param name="emailExists">Boolean indicating if email exists or not.</param>
        $.publish('checked-email', [emailExists]);
    }

    function addSubscription(eventName) {
        /// <summary>
        /// Adds subscription to provided event and perfoms checkemail when that event is published. 
        /// </summary>
        /// <param name="eventName">The event name to subscribe to.</param>
        $.subscribe(eventName, function (_, email) {
            checkEmail(email, function (emailExists) {
                publishEmailStatus(emailExists);
            });
        });
    }

    return {
        subscribe: function (eventName) {
            /// <summary>
            /// Public method that calls internal functions to add subscription to provided event
            /// </summary>
            /// <param name="eventName">The event name to subscribe to.</param>
            addSubscription(eventName);
        }
    }
}