var startDateHandler = {
    isMobileDevice : false,
    setUp: function (isMobileDevice) {
        this.isMobileDevice = isMobileDevice;
        // If not mobile device, init datepicker
        // We change type attribute to text, this is because some browsers (chrome) have
        // crappy built in functionality for handling input type=date. We don't want to use that shit.
        if (!this.isMobileDevice) {
            var startDateInput = campaignHandler.elements.getStartDateInput();

            startDateInput.attr("type", "text");

            startDateInput.datepicker({
                format: "yyyy-mm-dd",
                weekStart: 1,
                language: "sv",
            });
        }
    },
    getStartDate: function (campId, callback) {
        jQuery.ajax({
            url: '/api/startdate/' + campId,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                callback(result);
            },
            async: true
        });
    },
    setStartDate: function (startDateInput, campId) {
        if (campId == undefined || !campId.length) { return; }

        this.getStartDate(campId, function (startDate) {
            startDateInput.val(startDate);
            if (!startDateHandler.isMobileDevice) {
                startDateInput.datepicker('setStartDate', startDate);
            }            
        });
    }
};