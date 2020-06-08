define([
    "jquery",
    "pubsub",
    "bootstrap",
    "dom/misc",
    "func/ajax"], function ($, pubsub, bootstrap, dom, ajax) {
    return new ErrorHandler(dom, ajax);
});

function ErrorHandler(dom, ajax) {
    this.dom = dom;
    this.ajax = ajax;

    this.init();
}

ErrorHandler.prototype.init = function () {
    var self = this;
    // Handling for sudden session death
    $.subscribe("forbidden", function () {
        self.dom.forbiddenModal.modal();
    });

    $.subscribe("error", function () {
        self.dom.errorModal.modal();
    });

    $.subscribe("feedback", function (_, feedbackText) {
        $("#feedback-modal .modal-body").html(feedbackText);
        $("#feedback-modal").modal();
    });



    // Handling for require errors
    requirejs.onError = function (err) {        
        $.publish('error');
        throw err;
    }

    // Handling for all js errors
    window.onerror = function (msg, url, line, col, error) {
        // Note that col & error are new to the HTML 5 spec and may not be supported in every browser.
        var extra = !col ? '' : '\ncolumn: ' + col;
        extra += !error ? '' : '\nerror: ' + error;

        var errorObj = {
            error: msg,
            url: url,
            line: line,
            col: col,
            extra: extra
        };

        self.ajax.makeAjaxRequest("/api/mysettings/logging/log", "GET", function(){}, function (){}, function(){}, errorObj);
    };

}