var AddressDropDown = function (dropDownElement) {
    var self = this;
    this.dropDownElement = dropDownElement;
    this.dropDownElement.on("change", function () { self._handleChange(this); });
};

AddressDropDown.prototype._handleChange = function (element) {
    $(document).trigger('addressDropdownChange', [element.value]);
};

AddressDropDown.prototype.setSelectedIndex = function(index) {
    this.dropDownElement[0].selectedIndex = index;
};

var AddressData = function () { };

AddressData.prototype.get = function (url, addressId, callback) {
    jQuery.ajax({
        url: url,
        data: { addressId: addressId },
        contentType: 'application/json; charset=utf-8',
        success: function (address) {
            callback(address);
        },
        async: true
    });
};

var AddressForm = function (formContainer) {
    this.formContainer = formContainer;

    this.streetForm = {
        coInput: formContainer.find("#coinput"),
        streetAddressInput: formContainer.find("#streetaddressinput"),
        streetNoInput: formContainer.find("#streetnoinput"),
        staircaseInput: formContainer.find("#staircaseinput"),
        stairsInput: formContainer.find("#stairsinput"),
        apartmentNumberInput: formContainer.find("#apartmentnumberinput"),
        zipInput: formContainer.find("#zipinput"),
        cityInput: formContainer.find("#cityinput"),
        fromdateInput: formContainer.find("#fromdateinput"),
        toDateInput: formContainer.find("#todateinput")
    };
};

AddressForm.prototype.populate = function (formData) {
    this.streetForm.coInput.val(formData.StreetAddressForm.Co);
    this.streetForm.streetAddressInput.val(formData.StreetAddressForm.StreetAddress);
    this.streetForm.streetNoInput.val(formData.StreetAddressForm.StreetNo);
    this.streetForm.staircaseInput.val(formData.StreetAddressForm.StairCase);
    this.streetForm.stairsInput.val(formData.StreetAddressForm.Stairs);
    this.streetForm.apartmentNumberInput.val(formData.StreetAddressForm.ApartmentNumber);
    this.streetForm.zipInput.val(formData.StreetAddressForm.Zip);
    this.streetForm.cityInput.val(formData.StreetAddressForm.City);
    this.streetForm.fromdateInput.val('');
    this.streetForm.toDateInput.val('');
};

AddressForm.prototype.show = function () {
    this.formContainer.show();
};

AddressForm.prototype.clearForm = function () {
    $.each(this.streetForm, function (i, param) {
        param.val('');
    });
};

var AddressFormInitButton = function (buttonElement) {
    var self = this;
    this.buttonElement = buttonElement;
    this.buttonElement.on("click", function (e) { self._handleClick(e); });
};

AddressFormInitButton.prototype._handleClick = function (event) {
    event.preventDefault();
    $(document).trigger('initAddressFormClick');
};

var AddressList = function (listElement) {
    this.listElement = listElement;
};

AddressList.prototype.init = function () {
    if (this.listElement === undefined) {
        return;
    }

    this.listElement.find(".delete-address-link").click(function () {
        return confirm('Är du säker på att du vill radera adressen?');
    });
};