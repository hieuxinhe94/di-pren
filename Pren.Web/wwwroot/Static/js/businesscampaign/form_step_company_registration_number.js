function CompanyRegistrationNumberStep($rootEl) {
    'use strict';
    var self = this; // Reference the this object to self
    // Init variabels
    self.$rootEl = $rootEl;
    self.$input = self.$rootEl.find('#companyregistrationnumberinput');
    self.$submitButton = self.$rootEl.find('#companyregistrationnumber-submit');
    self.$nocompanyfound = self.$rootEl.find("#nocompanyfound");

    function publishCompanyRegistrationNumberSubmitted() {
        $.publish('company-reg-nb-submitted', [self.$input.val()]);
    }

    self.$submitButton.on('click', function (e) {
        e.preventDefault();

        self.$input.val(self.$input.val().split(' ').join('')); // Trim spaces from value
        self.$input.valid(); // Validate the field

        // Validate complete form
        var isValid = $("#company-campaign-form").validate().form();

        // Only publish company reg nb submitted if form is valid
        if (isValid) {
            publishCompanyRegistrationNumberSubmitted();
        }
    });

    self.$input.on("blur", function () {
        self.$input.val(self.$input.val().split(' ').join('')); // Trim spaces from value
        var isValid = self.$input.valid();

        // Only publish reg nb submitted if form is valid
        if (isValid) {
            publishCompanyRegistrationNumberSubmitted();
        }
    });

    $.subscribe('company-info', function (_, companyInfo) {
        if (companyInfo === undefined) {
            self.$nocompanyfound.show();
        } else {
            self.$nocompanyfound.hide();
        }
    });

    $.subscribe("company-reg-nb-submitted", function () {
        dataLayer.push({ "event": "company-reg-nb-submitted" });
        self.$submitButton.hide();
    });

    $.subscribe("email-ok", function () {
        self.$rootEl.show();
    });
};
