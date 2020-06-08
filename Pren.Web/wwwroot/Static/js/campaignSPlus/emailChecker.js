var emailChecker = {
    events: {

    },
    checkIfEmailExist: function (email, callback) {

        var isLoggedIn = elementFactory.getElement(elementFactory.elements.isServicePlusUser).val() == "true";

        jQuery.ajax({
            method: "POST",
            url: '/api/emailexists/?isLoggedIn=' + isLoggedIn,
            data: JSON.stringify({ email: email }),
            contentType: 'application/json; charset=utf-8',
            success: function(result) {
                callback(result);
            },
            async: true
        });
    },
    checkEmailInput: function(emailInput) {

        if (emailInput.attr("readonly") != "readonly" && emailInput.val().length) {
            emailChecker.checkIfEmailExist(emailInput.val(), function (user) {

                emailChecker.elements.getEmailExistsContainer().show();

                if (user == null) {
                    emailChecker.elements.getEmailExistsContainerAll().fadeOut("slow");
                    disabler.disableSubmitButtons(false);
                    return false;
                }

                if (user.IsBip) {
                    if (user.Header) {
                        $(".emaillabel").hide();
                        if (user.ForceLogin) {
                            $("#emaillabelbip").html(user.Header).show();
                        } else {
                            $("#loggedinemaillabel span").html(user.Header).show();
                        }
                    } else {
                        $(".emaillabel").hide();
                        $("#notloggedinemaillabel").show();
                    }

                    if (user.Message) {
                        emailChecker.elements.getEmailExistsContainerOrg().hide();
                        var message = user.Message.replace("LOGIN_LINK", emailChecker.elements.getLoginLink().attr("href"));

                        emailChecker.elements.getEmailExistsContainerBip().html(message).fadeIn("slow");
                    }
                }

                if (user.ForceLogin) {
                    if (!user.IsBip) {
                        emailChecker.elements.getEmailExistsContainerBip().hide();
                        emailChecker.elements.getEmailExistsContainerOrg().fadeIn("slow");
                    }

                    disabler.disableSubmitButtons(true);
                    scroller.scrollTo(emailInput);
                    emailChecker.appendEmailToLink(emailChecker.elements.getLoginLink(), emailInput.val());
                    customEventHandler.trigger("emailExists", "email exists");
                    return true;
                } else {
                    if (!user.Message) {
                        emailChecker.elements.getEmailExistsContainerAll().fadeOut("slow");
                    }                    
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
        getEmailExistsContainerOrg: function () { return elementFactory.getElement(elementFactory.elements.emailExistsContainer).find(".alert-reg"); },
        getEmailExistsContainerBip: function () { return elementFactory.getElement(elementFactory.elements.emailExistsContainer).find(".alert-bip"); },
        getEmailExistsContainerAll: function () { return elementFactory.getElement(elementFactory.elements.emailExistsContainer).find(".alert"); },
    },
    texts: {
        emailExistsOrgText: ""
    }
};