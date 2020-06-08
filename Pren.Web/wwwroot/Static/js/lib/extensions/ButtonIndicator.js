define(["jquery", "bootstrap"], function ($, bootstrap) {

    $.fn.extend({
        loadingIndicatorLoad: function(condition) {
            if (condition !== undefined && !condition()) {
                return;
            }

            $(this).button("loading");
        },
        loadingIndicatorReset: function() {
            this.each(function() {
                $(this).button("reset");
            });
        }

    });
});