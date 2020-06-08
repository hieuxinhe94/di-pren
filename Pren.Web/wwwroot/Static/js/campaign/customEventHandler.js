var customEventHandler = {
    trigger: function (eventType, eventMessage) {
        $.event.trigger({
            type: eventType,
            message: eventMessage,
            time: new Date()
        });
    },
    subscribe: function (eventType, callback) {
        $(document).on(eventType, function (e) {
            callback(e.message);
        });
    }
};