(function(factory) {
    if (typeof define === "function" && define.amd) {
        define(["jquery"], factory);
    } else {
        factory(jQuery);
    }
}(function($) {

    $.extend($.fn, {

        display: function(options) {

            $(this).each(function () {
                var displayHandler = new DisplayHandler($(this), options.event, options.callback);
                displayHandler.init();
            });
        }
    });
}));


var DisplayHandler = function (element, event, callback) {    
    this.displayelement = element;  
    this.event = event;
    this.callback = callback;
}

DisplayHandler.prototype.init = function () {
    var self = this;
    this.displayelement.on(this.event, function (e) { self._handleEvent(e, $(this).data("targetselector"), $(this).data("targetevent")); });
    this.setUpListeners();
};

DisplayHandler.prototype._handleEvent = function (event, targetselector, targetevent) {
    event.preventDefault();
    this.displayelement.trigger(targetevent, targetselector);
    if (this.callback != null) {
        this.callback();
    }
};

DisplayHandler.prototype.setUpListeners = function () {
    this.displayelement.on("show", function (e, targetselector) {
        $(targetselector).show();
    });
    this.displayelement.on("hide", function (e, targetselector) {
        $(targetselector).hide();
    });
    this.displayelement.on("toggle", function (e, targetselector) {
        $(targetselector).toggle();
    });
};