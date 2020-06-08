function StepListener($containerElement, showOnEventName) {
    /// <summary>
    /// Base class for objects in campaign form that should be hidden until a special event is published 
    /// </summary>
    /// <param name="$containerElement">The container jQuery object</param>
    /// <param name="showOnEventName">The event on which the container element should be shown</param>
    'use strict';
    var self = this;
    self.containerElement = $containerElement;
    self.showOnEventName = showOnEventName;

    self.showOnEvent();
};

StepListener.prototype.showOnEvent = function () {
    /// <summary>
    /// Subscribes to the provided event and show the provided container when fired
    /// </summary>
    'use strict';
    var self = this;
    $.subscribe(self.showOnEventName, function () {
        self.containerElement.show();
    });
};