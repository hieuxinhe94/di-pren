function CompanyInfoStep($containerElement, showOnEventName) {
    'use strict';
    StepListener.call(this, $containerElement, showOnEventName);
    var self = this; // Reference the this object to self
    // Init variabels

    self.firstNameInput = $containerElement.find("#firstnameinput");
    self.lastNameInput = $containerElement.find("#lastnameinput");
    self.phoneInput = $containerElement.find("#phoneinput");
    self.companyInput = $containerElement.find("#companyinput");
    self.streetAddressInput = $containerElement.find("#streetaddressinput");
    self.streetNoInput = $containerElement.find("#streetnoinput");
    self.zipInput = $containerElement.find("#zipinput");
    self.cityInput = $containerElement.find("#cityinput");

    self.zipInput.bind("change keyup", function (event) {
        var zip = $(this).val();
        //todo: kj refactor postNameHandler to object
        postNameHandler.setPostName(zip, self.cityInput);
    });

    self.phoneInput.on("blur", function () {
        self.phoneInput.val(self.phoneInput.val()
            .split(' ').join('') // Trim spaces from value
            .split('-').join('') // Trim - from value
            .split('+').join('')); // Trim + from value

        self.phoneInput.valid(); // Triggers validation on this field
    });

    $.subscribe('company-info', function (_, companyInfo) {
        if (companyInfo === undefined || companyInfo === null) {
            companyInfo = {};
            companyInfo.Name = '';
            companyInfo.StreetAddress = '';
            companyInfo.HouseNumber = '';
            companyInfo.StairCase = '';
            companyInfo.ZipCode = '';
            companyInfo.City = '';
        }
        self.companyInput.val(companyInfo.Name);
        self.streetAddressInput.val(companyInfo.StreetAddress);
        self.streetNoInput.val(companyInfo.HouseNumber + companyInfo.StairCase);
        self.zipInput.val(companyInfo.ZipCode);
        self.cityInput.val(companyInfo.City);
    });
}

//CompanyInfoStep.prototype = Object.create(StepListener.prototype);