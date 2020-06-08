var addressHandler = {
    getAddress: function (identity, callback, callbackError) {
        var generalErrorMsg = "Ett fel uppstod, vänligen försök igen.";

        jQuery.ajax({
            url: '/api/address/' + identity,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result != null) {
                    result.Error != null ? callbackError(result.Error) : callback(result);
                } else {
                    callbackError(generalErrorMsg);
                }
            },
            error: function (xhr, textStatus, error) {
                callbackError(generalErrorMsg);
            },
            timeout: 5000,
            async: false
        });
    },
};