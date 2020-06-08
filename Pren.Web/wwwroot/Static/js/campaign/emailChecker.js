var emailChecker = {
    events : {
          
    },
    checkIfEmailExist:function(email, callback) {
        jQuery.ajax({
            url: '/api/emailcheck/' + email,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                callback(result);
            },
            async: true
        });
    },
    checkEmailInput: function (emailInput) {

        if (emailInput.attr("readonly") != "readonly" && emailInput.val().length) {
            emailChecker.checkIfEmailExist(emailInput.val(), function (exists) {
                if (exists) {
                    emailChecker.elements.getEmailExistsContainer().fadeIn("slow");
                    disabler.disableSubmitButtons(true);
                    scroller.scrollTo(emailInput);
                    emailChecker.appendEmailToLink(emailChecker.elements.getLoginLink(), emailInput.val());
                    customEventHandler.trigger("emailExists", "email exists");
                    return true;
                } else {
                    emailChecker.elements.getEmailExistsContainer().fadeOut("slow");
                    disabler.disableSubmitButtons(false);
                    return false;
                }
            });
        }
    },
    appendEmailToLink: function(link, email) {        
        var currentHref = link.attr("href");
        var loginHref = currentHref.indexOf("email") > -1 ? currentHref : currentHref + "&email=" + email;
        link.attr("href", loginHref);
    },
    elements: {
        getLoginLink: function () { return elementFactory.getElement(elementFactory.elements.emailExistsContainer).find("#loginlink"); },
        getEmailExistsContainer: function () { return elementFactory.getElement(elementFactory.elements.emailExistsContainer); },
    }
};