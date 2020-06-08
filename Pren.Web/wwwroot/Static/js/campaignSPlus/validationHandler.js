

var validationHandler = {

    init: function () {
        // Set up jqueryvalidation
        elementFactory.getElement(elementFactory.elements.getStepsContainer).validate({
            ignore: ":hidden, .ignore",
            onsubmit: false,
            rules: {
                'termscheck': {
                    required: true
                }
            },
            messages: {
                'termscheck': {
                    required: "Vänligen godkänn prenumerationsvillkoren"
                },
                'prenstart': {
                    date: "Vänligen ange ett korrekt datum"
                }
            },
            errorPlacement: function (error, element) {
                if (element.is(':checkbox')) {
                    error.insertAfter(element.parent().parent()); 
                } else {
                    error.insertAfter(element); // <- the default
                }

            },

        });

        // Add validation
        validationHandler.addEmailValidation();
        validationHandler.addPhoneValidation();
        validationHandler.addSsnValidation();
        validationHandler.addAddressValidation();
        validationHandler.addAddressInvoiceValidation();
    },
    getFormToValidate: function () {

        if (this.fields._form == null) {
            this.fields._form = elementFactory.getElement(elementFactory.elements.getStepsContainer).validate();
        }

        return this.fields._form;
    },
    addEmailValidation: function () {

        $('#emailinput').rules('add', {
            required: true,
            email: true,
            messages: {
                required: "Fältet är tomt. Var god ange din e-postadress",
                email: "Du måste skriva in en korrekt e-postadress"
            }
        });
    },
    addPhoneValidation: function () {
        $('#phoneinput').rules('add', {
            required: true,
            number: true,
            minlength: 8,
            maxlength: 16,
            messages: {
                required: "Var god ange ditt personliga mobilnummer",
                number: "Var god ange ditt personliga mobilnummer",
                minlength: "Var god ange ditt personliga mobilnummer",
                maxlength: "Var god ange ditt personliga mobilnummer"
            }
        });
    },
    addSsnValidation: function () {
        $.validator.addMethod("regexp", function (value, element) {
            return /(^[\d]{10,12}$)|(^[\d]{6}(-)[\d]{4}$)/.test(value);
        }, 'Var god ange personnummer (ååmmddxxxx) eller organisationsnummer (xxxxxx-xxxx).');

        $('#ssninput').rules('add', {
            required: true,
            regexp: true,
                messages: {
                    required: 'Var god ange personnummer (ååmmddxxxx) eller organisationsnummer (xxxxxx-xxxx).'
                }
        });
    },
    addAddressValidation: function () {

        if ($('#extrainfoinput').data("mandatory") == "True") {
            $('#extrainfoinput').rules('add', {
                required: true,
                messages: {
                    required: "Detta fält är obligatoriskt",
                }
            });
        }

        $('#firstnameinput').rules('add', {
            required: true,
            messages: {
                required: "Var god ange ditt förnamn",
            }
        });
        $('#lastnameinput').rules('add', {
            required: true,
            messages: {
                required: "Var god ange ditt efternamn",
            }
        });
        $('#streetaddressinput').rules('add', {
            required: true,
            messages: {
                required: "Var god ange ditt efternamn",
            }
        });

        $.validator.addMethod("zip", function (value, element) {
            return $('#cityinput').val().length > 0;
        }, 'Vi kan inte hitta postnumret du har angivit.');

        $.validator.addMethod("regexpzip", function (value, element) {
            return /(^[\d]{5}$)|(^\d[/*]{4}$)/.test(value);
        }, 'Var god ange ditt femsiffriga postnummer');


        $('#zipinput').rules('add', {
            required: true,
            //number: true,
            minlength: 5,
            maxlength: 5,
            zip: true,
            regexpzip: true,
            messages: {
                required: "Var god ange ditt postnummer",
                number: "Var god ange ditt postnummer",
                minlength: "Var god ange ditt femsiffriga postnummer",
                maxlength: "Var god ange ditt femsiffriga postnummer"
            }
        });

        $('#firstnamedigitalinput').rules('add', {
            required: true,
            messages: {
                required: "Var god ange ditt förnamn",
            }
        });
        $('#lastnamedigitalinput').rules('add', {
            required: true,
            messages: {
                required: "Var god ange ditt efternamn",
            }
        });
        
    },
    addAddressInvoiceValidation: function () {

        $('#companyinputinvoice').rules('add', {
            required: true,
            messages: {
                required: "Var god ange företagsnamn/namn",
            }
        });
        $('#attentioninputinvoice').rules('add', {
            required: true,
            messages: {
                required: "Var god ange attention",
            }
        });
        $('#phoneinputinvoice').rules('add', {
            required: true,
            number: true,
            minlength: 8,
            maxlength: 16,
            messages: {
                required: "Var god ange ditt personliga mobilnummer",
                number: "Var god ange ditt personliga mobilnummer",
                minlength: "Var god ange ditt tiosiffriga mobilnummer",
                maxlength: "Var god ange ditt tiosiffriga mobilnummer"
            }
        });        
        $('#streetaddressinputinvoice').rules('add', {
            required: true,
            messages: {
                required: "Var god ange ditt efternamn",
            }
        });

        $.validator.addMethod("invoicezip", function (value, element) {
            return $('#cityinputinvoice').val().length > 0;
        }, 'Vi kan inte hitta postnumret du har angivit.');

        $.validator.addMethod("regexpzipinvoice", function (value, element) {
            return /(^[\d]{5}$)|(^\d[/*]{4}$)/.test(value);
        }, 'Var god ange ditt femsiffriga postnummer');

        $('#zipinputinvoice').rules('add', {
            required: true,
            //number: true,
            minlength: 5,
            maxlength: 5,
            invoicezip: true,
            regexpzipinvoice: true,
            messages: {
                required: "Var god ange ditt postnummer",
                number: "Var god ange ditt postnummer",
                minlength: "Var god ange ditt femsiffriga postnummer",
                maxlength: "Var god ange ditt femsiffriga postnummer"
            }
        });
    },
    validate: function (callback, ignoreContainerElements) {
        // Add temporary ignore class. This means that jqueryvalidator will exclude inputs in container
        if (ignoreContainerElements != null) {
            $.each(ignoreContainerElements, function () {
                $(this).find("input").addClass("ignore");
            });
        }

        var isValid = this.getFormToValidate().form();

        if (isValid) {
            if(callback != null) callback();
        } else {
            var firstInvalidInput = elementFactory.getElement(elementFactory.elements.getStepsContainer).find("input.error").first();
            scroller.scrollTo(firstInvalidInput);
            firstInvalidInput.focus();
        }

        // Remove ignore class. 
        if (ignoreContainerElements != null) {
            $.each(ignoreContainerElements, function () {
                $(this).find("input").removeClass("ignore");
            });
        }

        return isValid;
    },
    validateSingleElement: function (callback, selector) {
        var val = validationHandler.getFormToValidate();

        if (val.element(selector)) {
            callback();
        }
    },
    fields: {
        _form: null,
    },
};