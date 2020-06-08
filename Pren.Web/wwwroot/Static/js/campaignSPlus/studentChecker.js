var studentChecker = {
    checkStudent: function (socialSecurityNumber, callback) {
        jQuery.ajax({
            url: '/api/studentcheck/' + socialSecurityNumber,
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                callback(result);
            },
            async: true
        });
    }
};