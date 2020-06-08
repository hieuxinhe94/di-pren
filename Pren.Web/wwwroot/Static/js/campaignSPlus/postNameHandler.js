var postNameHandler = {
    setPostName: function (zipCode, inputElement) {
        if (!jQuery.isNumeric(zipCode) || zipCode.length != 5) {
            inputElement.val('');
            return;
        }

        postNameHandler.getPostName(zipCode, function (city) {
            inputElement.val(city);
        },
        function() {
            inputElement.removeAttr("readonly");
        });
    },
    getPostName: function (zipCode, callback, callBackError) {
        jQuery.ajax({
            url: '/api/postname/' + zipCode,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                callback(result);
            },
            error: function (xhr, textStatus, error) {
                callBackError();
            },
            timeout: 5000,
            async: false
        });
    }
}