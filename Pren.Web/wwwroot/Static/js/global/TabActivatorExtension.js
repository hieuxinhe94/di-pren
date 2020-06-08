//Extension method for bootstrap nav-tabs

jQuery.fn.extend({
    setTabAsActive: function (tabToActivate) {

        var tabLink = this.filter(function () {
            return $(this).attr("href") === "#" + tabToActivate;
        });

        if (!tabLink.length) {
            // If no tab to activate, trigger click on first tab
            $(this).first().trigger("click");
            return;
        }

        $(tabLink).trigger("click");
    }
});