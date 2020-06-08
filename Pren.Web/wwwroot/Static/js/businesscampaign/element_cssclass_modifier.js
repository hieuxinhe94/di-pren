function ElementCssClassModifier() {

    function addSubscription(element, eventName, cssClassToRemove, cssClassToAdd) {

        $.subscribe(eventName, function () {
            $(element).removeClass(cssClassToRemove);
            $(element).addClass(cssClassToAdd);
        });
    }

    return {
        subscribe: function (element, eventName, cssClassToRemove, cssClassToAdd) {
            addSubscription(element, eventName, cssClassToRemove, cssClassToAdd);
        }
    }
}