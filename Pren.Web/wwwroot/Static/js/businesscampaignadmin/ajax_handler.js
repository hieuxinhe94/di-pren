function AjaxHandler() { }

AjaxHandler.prototype.makeAjaxRequest = function (apiUrl, pubNameOnSuccess, pubValueOnSuccess, pubNameOnError, pubValueOnError) {
    if (apiUrl === undefined || apiUrl === null) {
        return;
    }

    jQuery.ajax({
        url: apiUrl,
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //general fail if result is false
            if (result === false) {
                $.publish(pubNameOnError, pubValueOnError);
                return;
            }
            $.publish(pubNameOnSuccess, [result, pubValueOnSuccess]);
        },
        error: function (xhr, textStatus, error) {
            $.publish(pubNameOnError, pubValueOnError);
        },
        async: true
    });
};