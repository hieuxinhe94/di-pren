function WindowResizeEventHandler() {

    function setUpTriggers(maxWidth, triggerMinEventName, triggerMaxEventName) {

        var active = false;

        if ($(window).width() < maxWidth && !active) {
            //console.log(triggerMinEventName);
            $.publish(triggerMinEventName);
            active = true;
        }
           
        
        $(window).resize(function () {
            if ($(window).width() < maxWidth && !active) {                
                //console.log(triggerMinEventName);
                $.publish(triggerMinEventName);
                active = true;
            } else if ($(window).width() >= maxWidth && active) {                
                //console.log(triggerMaxEventName);
                $.publish(triggerMaxEventName);
                active = false;
            }
        });
    };

    return {
        trigger: function (maxWidth, triggerMinEventName, triggerMaxEventName) {
            /// <summary>
            /// Public method that calls internal function to set up triggers for provided events
            /// </summary>
            /// <param name="maxWidth">Width i pixels</param>
            /// <param name="triggerMinEventName">Event to trigger if sreensize is lower than maxWidth</param>
            /// <param name="triggerMaxEventName">Event to trigger if sreensize is higher than maxWidth</param>
            setUpTriggers(maxWidth, triggerMinEventName, triggerMaxEventName);
        }
    }
}

