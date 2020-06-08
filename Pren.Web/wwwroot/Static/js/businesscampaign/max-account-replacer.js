function MaxAccountReplacer(maxAccountElems, accountDelimiterSelector) {
    function replaceMaxAccount() {
        try {
            maxAccountElems.each(function () {
                var maxAccountElem = $(this);
                if (Number(maxAccountElem.text()) >= 9999999 || maxAccountElem.text() === "+") {
                    maxAccountElem.text("+");
                    maxAccountElem.prev(accountDelimiterSelector).hide();
                } else {
                    maxAccountElem.prev(accountDelimiterSelector).show();
                }
            });

        } catch (e) {

        }
    }

    function addSubscription(eventName) {
        /// <summary>
        /// Adds subscription to provided event and perfoms replaceMaxAccount when that event is published. 
        /// </summary>
        /// <param name="eventName">The event name to subscribe to.</param>
        $.subscribe(eventName, function () {
            replaceMaxAccount();
        });
    }

    replaceMaxAccount();

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