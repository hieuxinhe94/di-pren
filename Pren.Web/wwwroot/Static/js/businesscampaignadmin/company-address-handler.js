function CompanyAddressHandler(ajaxHandler, companyInfoBoxEl, customerNumber, templateString) {
    var self = this;
    self.ajaxHandler = ajaxHandler;
    self.companyInfoBoxEl = companyInfoBoxEl;
    self.customerNumber = customerNumber;
    self.template = _.template(templateString);
    self.companyInfoContentContainer = self.companyInfoBoxEl.find(".content");

    function fetch() {
        if (self.customerNumber === undefined) {
            return;
        }

        ajaxHandler.makeAjaxRequest(
            "/api/biz/getbizsubscriberaddressinfo/" + self.customerNumber,
            "biz-get-company-info-successfull",
            null,
            "biz-get-company-info-failed");
    };

    function populateAddress(companyInfo) {
        // Populate container with template and subscribers
        self.companyInfoContentContainer.html(self.template({ data: companyInfo }));
        self.companyInfoBoxEl.show();
    }

    $.subscribe('biz-get-company-info-successfull', function (notUsedElem, companyInfo) {
        populateAddress(companyInfo);
    });
    
    fetch();
};
