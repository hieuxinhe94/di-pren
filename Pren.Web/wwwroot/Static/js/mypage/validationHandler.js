function ValidationHandler(validationRootElement) {
    // Note: root must be a form-element to work on submit.
    // Otherwise you need to validate manually.
    this.validationRootElement = validationRootElement;
}

ValidationHandler.prototype.init = function () {
    $(this.validationRootElement).each(function () {
        if ($(this).hasClass("customvalidation")) {
            return true;
        }
        $(this).validate({
            ignore: ":hidden, .ignore",
            errorPlacement: function (error, element) {
                if (element.is(':checkbox')) {
                    error.insertAfter(element.parent().parent().parent());
                } else {
                    error.insertAfter(element); // <- the default
                }
            },
            rules: {
                message: {
                    required: true
                },
                customernumber: {
                    required: true
                },
                name: {
                    required: true
                },
                email: {
                    required: true,
                    email: true
                },
                daystoreclaim: {
                    required: true
                },
                phone: {
                    required: true
                },
                streetaddress: {
                    required: true
                },
                zip: {
                    required: true,
                    number: true,
                    maxlength: 5
                },
                city: {
                    required: true
                },
                fromdate: {
                    required: true
                },
                todate: {
                    required: true
                },
                stoporboxaddress: {
                    required: true
                },
                stoporboxnumber: {
                    required: true
                },
                stoporboxzip: {
                    required: true,
                    number: true,
                    maxlength: 5
                },
                stoporboxcity: {
                    required: true
                },
                stoporboxfromdate: {
                    required: true
                },
                stoporboxtodate: {
                    required: true
                },
                code: {
                    required: true
                },
                emailOrCusno: {
                    required: true
                }
            },
            messages: {
                message: {
                    required: "Vänligen ange ämne"
                },
                customernumber: {
                    required: "Vänligen ange ditt kundnummer"
                },
                name: {
                    required: "Vänligen ange ditt namn",
                },
                email: {
                    required: "Vänligen ange e-post",
                    email: "Felaktigt format på din e-post"
                },
                daystoreclaim: {
                    required: "Vänligen ange dag att reklamera",
                },
                phone: {
                    required: "Vänligen ange ditt telefonnummer"
                },
                streetaddress: {
                    required: "Vänligen ange din gatuadress"
                },
                zip: {
                    required: "Vänligen ange ditt postnummer XXXXX",
                    number: "Endast siffror, utan mellanslag"
                },
                city: {
                    required: "Vänligen ange din ort"
                },
                fromdate: {
                    required: "Vänligen ange fråndatum",
                    date: "Vänligen ange ett korrekt datum"
                },
                todate: {
                    required: "Vänligen ange tilldatum",
                    date: "Vänligen ange ett korrekt datum"
                },
                stoporboxaddress: {
                    required: "Vänligen ange stoppställe eller box"
                },
                stoporboxnumber: {
                    required: "Vänligen ange nummer"
                },
                stoporboxzip: {
                    required: "Vänligen ange postnummer",
                    number: "Endast siffror, utan mellanslag"
                },
                stoporboxcity: {
                    required: "Vänligen ange ort"
                },
                stoporboxfromdate: {
                    required: "Vänligen ange fråndatum",
                    date: "Vänligen ange ett korrekt datum"
                },
                stoporboxtodate: {
                    required: "Vänligen ange tilldatum",
                    date: "Vänligen ange ett korrekt datum"
                },
                code: {
                    required: "Vänligen ange din kod"
                },
                emailOrCusno: {
                    required: "Vänligen ange epost eller kundnummer"
                }
        }
        });
    });
}

ValidationHandler.isValid = function(element) {
    return $(element).validate().form();
}
