var ConfirmHandler = function (elements, event) {
    var self = this;
    this.confirmElements = elements;
    this.event = event;
    this.confirmElements.on(this.event, function (e) { self._handleEvent(e, $(this).data("confirm")); });
}

ConfirmHandler.prototype._handleEvent = function (event, confirmText) {
    if (!confirm(confirmText)) {
        event.preventDefault();
    }
};
