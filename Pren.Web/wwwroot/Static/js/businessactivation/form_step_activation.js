function ActivationStep($rootEl) {
    'use strict';
    var self = this;
    self.$rootEl = $rootEl;
    self.$phoneInput = self.$rootEl.find("#phoneinput");

    self.$phoneInput.on("blur", function () {
        self.$phoneInput.val(self.$phoneInput.val()
            .split(' ').join('') // Trim spaces from value
            .split('-').join('') // Trim - from value
            .split('+').join('')); // Trim + from value

        self.$phoneInput.valid(); // Triggers validation on this field
    });
}