/**
 * Object responsible for client validation of biz subscription form
 * @param {jQuery object} bizSubscriptionForm - the viz subscription form element
 */
function BizSubscriptionFormvalidator(bizSubscriptionForm) {
    'use strict';

    /**
     * Adds validation rules to the provided form
     */
    function addValidationRules() {
        $(bizSubscriptionForm).validate({
            rules: {
                email: {
                    required: true,
                    email: true
                },
                companyregistrationnumber: {
                    required: true,
                    regexp: true
                },
                firstname: "required",
                lastname: "required",
                phone: {
                    required: true,
                    number: true,
                    minlength: 8,
                    maxlength: 16
                },
                company: "required",
                streetaddress: "required",
                streetno: "required",
                zip: {
                    required: true,
                    number: true,
                    minlength: 5,
                    maxlength: 5,
                    zip: true
                },
                city: "required",
                termscheck: "required"
            },
            messages: {
                email: {
                    required: "Var god ange din e-postadress",
                    email: "Du måste skriva in en korrekt e-postadress"
                },
                companyregistrationnumber: {
                    required: "Var god ange organisationsnummer (xxxxxx-xxxx)"
                },
                firstname: {
                    required: "Var god ange förnamn"
                },
                lastname: {
                    required: "Var god ange efternamn"
                },
                phone: {
                    required: "Var god ange telefonnummer",
                    number: "Var god ange ett korrekt telefonnummer",
                    minlength: "Var god ange ett korrekt telefonnummer",
                    maxlength: "Var god ange ett korrekt telefonnummer"
                },
                company: {
                    required: "Var god ange förtagsnamn"
                },
                streetaddress: {
                    required: "Var god ange gatuadress"
                },
                streetno: {
                    required: "Var god ange gatunummer"
                },
                zip: {
                    required: "Var god ange ditt postnummer",
                    number: "Var god ange ditt postnummer",
                    minlength: "Var god ange ditt femsiffriga postnummer",
                    maxlength: "Var god ange ditt femsiffriga postnummer"
                },
                city: {
                    required: "Var god ange ort"
                },
                termscheck: {
                    required: "Vänligen godkänn prenumerationsvillkoren"
                }
            },
            errorPlacement: function (error, element) {
                if (element.is(':checkbox')) {
                    error.insertAfter(element.parent().parent());
                } else {
                    error.insertAfter(element); // <- the default
                }
            }
        });

        $.validator.addMethod("regexp", function (value) {
            return /(^[\d]{10}$)|(^[\d]{6}(-)[\d]{4}$)/.test(value);
        }, 'Var god ange organisationsnummer (xxxxxx-xxxx).');

        $.validator.addMethod("zip", function () {
            return $('#cityinput').attr("readonly") !== "readonly" || $('#cityinput').val().length > 0;
        }, 'Vi kan inte hitta postnumret du har angivit.');
    }

    addValidationRules();
}