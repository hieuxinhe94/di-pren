jQuery.fn.extend({
    loadingIndicatorLoad: function (condition) {

        this.on("click", function() {
            if (condition !== undefined && !condition()) {
                return;
            }

            $(this).button("loading");
        });

    },
    loadingIndicatorReset: function () {
        this.each(function () {
            $(this).button("reset");
        });
    }

});