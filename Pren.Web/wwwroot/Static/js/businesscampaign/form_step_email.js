function EmailStep($rootEl, $emailSubmitBtn) {
    'use strict';
    var self = this;
    // Init variabels
    self.$rootEl = $rootEl;
    self.$input = self.$rootEl.find('#emailinput');
    self.$emailExistsEl = self.$rootEl.find('#emailexists');
    self.$emailExistsLoginLink = self.$emailExistsEl.find("a#loginlink");
    self.$hasbizsubscriptionEl = self.$rootEl.find('#hasbizsubscription');
    self.$loggedInLabel = self.$rootEl.find('#loggedinemaillabel');
    self.$notLoggedInLabel = self.$rootEl.find('#notloggedinemaillabel');
    self.$submitButton = $emailSubmitBtn;

    self.changeLoggedInState = function (loggedIn) {
        if (loggedIn) {
            // Make input field readonly when loggen in
            self.$input.attr('readonly', 'readonly');
            // Toggle labels
            self.$loggedInLabel.show();
            self.$notLoggedInLabel.hide();

            $.publish("check-biz-subscription", $("#serviceplususeridinput").val());
        } else {
            // Toggle labels
            self.$loggedInLabel.hide();
            self.$notLoggedInLabel.show();
            // Add blur event when not logged in
            self.$input.on('blur', function () {
                var isValid = self.$input.valid();
                if (!isValid) {
                    return;
                }
                $.publish('email-input-blurred', [self.$input.val()]);
            });
        }
    }

    // Change state depending on if user is logged in or not
    function changeLoggedInState(loggedIn) {
        if (loggedIn) {
            // Make input field readonly when loggen in
            self.$input.attr('readonly', 'readonly');
            // Toggle labels
            self.$loggedInLabel.show();
            self.$notLoggedInLabel.hide();

            $.publish("check-biz-subscription", $("#serviceplususeridinput").val());
        } else {
            // Toggle labels
            self.$loggedInLabel.hide();
            self.$notLoggedInLabel.show();
            // Add blur event when not logged in
            self.$input.on('blur', function () {
                var isValid = self.$input.valid();
                if (!isValid) {
                    return;
                }
                $.publish('email-input-blurred', [self.$input.val()]);
            });
        }
    }

    // Subscribe to checked.email event and toggle the email exists info box depending on if email exists
    $.subscribe('checked-email', function (_, emailExists) {
        console.log("checked-email" + " " + emailExists);
        if (emailExists) {

            // Update loginLink with the provided email
            var currentHref = self.$emailExistsLoginLink.attr("href");
            var loginHref = currentHref.indexOf("?email") > -1 ? currentHref.substring(0, currentHref.indexOf("?email")) + "?email=" + self.$input.val() : currentHref + "?email=" + self.$input.val();
            self.$emailExistsLoginLink.attr("href", loginHref);

            self.$emailExistsEl.fadeIn('slow');
            $.publish('email-not-ok');
        } else {
            self.$emailExistsEl.fadeOut('slow');
            $.publish('email-ok');
        }
    });

    $.subscribe('checked-biz-subscription', function (_, hasBizSubscription) {
        if (hasBizSubscription) {
            self.$hasbizsubscriptionEl.fadeIn('slow');
            self.$submitButton.hide();
        } else {
            self.$hasbizsubscriptionEl.fadeOut('slow');
            $.publish('email-ok');
        }
    });

    $.subscribe('email-ok', function (_, email) {
        self.$submitButton.hide();
    });

    // If there is a value in the input when we initialize the object we know it is populated - change the state accordingly
    changeLoggedInState(self.$input.val().length);

    self.$submitButton.on('click', function (e) {
        e.preventDefault();

        var isValid = $("#company-campaign-form").validate().form();
        if (!isValid) {
            return;
        }
        $.publish('email-input-blurred', [self.$input.val()]);
    });

    self.$rootEl.bind("keydown", function (event) {
        var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
        if (keycode == 13) {
            event.preventDefault();
            var isValid = $("#company-campaign-form").validate().form();
            if (!isValid) {
                return;
            }
            $.publish('email-input-blurred', [self.$input.val()]);
        }
    });
}