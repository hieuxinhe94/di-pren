var debugHandler = {
    init: function () {
        if (this.active && !campaignHandler.isMobileDevice) {
            $(this.selector).show();
        }
    },
    active: false,
    showCampaignInfo: function () {
        if (this.active && !campaignHandler.isMobileDevice) {

            var debug = $(this.selector);
            debug.remove();

            var campaignDetails = $("#campidinput").val() +
                ($("#campnoinput").val().length ? "(" + $("#campnoinput").val() + ")" : "");

            debug = $("<div id='debug'></div>");
            debug.append("<div><strong>Kampanj</strong>: " + campaignDetails + "</div>");
            debug.append("<div><strong>Betalmetod</strong>: " + $("#paymentmethodinput").val() + "</div>");
            debug.append("<div><strong>Målgrupp</strong>: " + $("#targetgroupinput").val() + "</div>");
            debug.append("<div><strong>Digital</strong>: " + $("#isdigitalinput").val() + "</div>");
            debug.append("<div><strong>Student</strong>: " + $("#isstudentinput").val() + "</div>");
            debug.append("<div><strong>Trial</strong>: " + $("#istrialinput").val() + "</div>");
            debug.append("<div><strong>Trial free</strong>: " + $("#istrialfreeinput").val() + "</div>");
            debug.append("<div><strong>IsServicePlusUser</strong>: " + $("#isserviceplususerinput").val() + "</div>");
            debug.append("<div><strong>InvoiceOtherPayer</strong>: " + $("#invoiceotherpayerinput").val() + "</div>");

            debug.show();
            $("body").append(debug);

            console.log('debug is active');
        }
    },
    selector : "#debug"
}