define([
    "jquery",
    "jqueryvalidate",
    "underscore",
    "app/mycodes/codeRepository",
    "app/mycodes/codeDisplayer",
    "mycodesdom/mycodesdom",
    "text!myCodesTemplates/giveAwayForm.html"],
    function ($, jqueryvalidate, _, codeRepo, codeDisplayer, dom, giveAwayCodeTemplate) {
        return new CodeGetterButtonGenerator(codeRepo, codeDisplayer, dom, giveAwayCodeTemplate);
    });


function CodeGetterButtonGenerator(codeRepository, codeDisplayer, dom, giveAwayCodeTemplate) {
    var self = this;
    self.codeRepository = codeRepository;
    self.codeDisplayer = codeDisplayer;
    self.dom = dom;
    self.giveAwayCodeTemplate = _.template(giveAwayCodeTemplate);
}

CodeGetterButtonGenerator.prototype.createGetCodeButton = function (listId, codeInfo, buttonText, notAvailableText, $codeButtonContainer) {
    var self = this;
    $("<button>")
        .addClass("btn btn-primary")
        .text(buttonText)
        .on("click", function () {
            var btn = $(this);
            self.codeRepository.getNewCode(listId, function (newCode) {
                if (newCode != '') {
                    self.codeDisplayer.displayCode(newCode, codeInfo, $codeButtonContainer);
                    btn.remove();
                } else {
                    self.codeDisplayer.displayCodeNotAvailable(notAvailableText, $codeButtonContainer);
                    btn.remove();
                }
            },
            function() {
                $codeButtonContainer.find(self.dom.mycodesErrorClass).show();
                btn.remove();
            });
        })
        .appendTo($codeButtonContainer);
}

CodeGetterButtonGenerator.prototype.createGiveAwayForm = function (listId, giveAwayInfo, buttonText, notAvailableText, $codeButtonContainer) {
    var self = this;

    var formDataObj = {
        formId: "form_" + listId,
        buttonText: buttonText
    }

    var form = $(self.giveAwayCodeTemplate({ formData: formDataObj }));

    // Set up validation
    form.validate({
        rules: {
            giveawayemail: {
                required: true,
                email: true
            }
        },
        messages: {
            giveawayemail: {
                required: "Vänligen ange e-postadress",
                email: "Vänligen ange en korrekt e-postadress"
            }
        }
    });

    form.find("button").on("click", function (e) {
        e.preventDefault();
        if (!form.valid()) return;

        console.log(form.find("input").val());
        console.log(notAvailableText);
        self.codeRepository.createNewGiveAway(listId, form.find("input").val(), function (gaveAwayTo) {
            if (gaveAwayTo !== '') {
                self.codeDisplayer.displayGiveAway(gaveAwayTo, giveAwayInfo, $codeButtonContainer);
                form.remove();
            } else {
                self.codeDisplayer.displayCodeNotAvailable(notAvailableText, $codeButtonContainer);
                form.remove();
            }
        },
        function () {
            $codeButtonContainer.find(self.dom.mycodesErrorClass).show();
            form.remove();
        });
    });

    $codeButtonContainer.prepend(form);
}