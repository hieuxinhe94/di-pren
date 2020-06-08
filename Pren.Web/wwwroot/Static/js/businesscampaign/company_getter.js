function CompanyGetter() {
    function getCompany(companyNumber, callBack, callBackError) {
        var generalErrorMsg = "Ett fel uppstod, vänligen försök igen.";
        companyNumber = addHypen(companyNumber);
        jQuery.ajax({
            url: '/api/address/' + companyNumber,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result != null) {
                    result.Error != null ? callBackError(result.Error) : callBack(result);
                } else {
                    callBackError(generalErrorMsg);
                }
            },
            error: function (xhr, textStatus, error) {
                callBackError(generalErrorMsg);
            },
            timeout: 5000,
            async: false
        });
    }

    function publishCompanyInfo(companyInfo) {
        $.publish('company-info', [companyInfo]);
    }

    function addSubscription(eventName) {
        $.subscribe(eventName, function (_, companyNumber) {
            getCompany(companyNumber, function (companyInfo) {
                publishCompanyInfo(companyInfo);
            }, function () {
                publishCompanyInfo(undefined);
            });
        });
    }

    function addHypen(companyNumber) {
        if (companyNumber.indexOf("-") > -1) {
            return companyNumber;
        }
        return companyNumber.substr(0, 6) + "-" + companyNumber.substr(6);
    }

    return {
        subscribe: function (eventName) {
            addSubscription(eventName);
        }
    }
}