function BizActivationFormValidator($activationForm) {
    'use strict';
    console.log('ff');
    function addValidationRules() {
        console.log('sa');
        $activationForm.validate({
            rules: {
                email: {
                    required: true,
                    email: true
                },
                firstname: "required",
                lastname: "required",
                phone: {
                    required: true,
                    number: true,
                    minlength: 8,
                    maxlength: 16,
                },
                termscheck: "required"
            },
            messages: {
                email: {
                    required: "Var god ange din e-postadress",
                    email: "Du måste skriva in en korrekt e-postadress"
                },
                firstname: {
                    required: "Var god ange förnamn"
                },
                lastname: {
                    required: "Var god ange efternamn"
                },
                phone: {
                    required: "Var god ange telefonnummer",
                    number: "Var god ange telefonnummer",
                    minlength: "Var god ange telefonnummer",
                    maxlength: "Var god ange telefonnummer"
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
    }

    addValidationRules();
}