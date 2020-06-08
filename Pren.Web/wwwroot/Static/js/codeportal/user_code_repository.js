function UserCodeRepository(userId, token) {
    this.userId = userId;
    this.token = token;
}

UserCodeRepository.prototype.getExistingCode = function (listId, callBack) {
    var self = this;

    jQuery.ajax({
        url: '/api/codeportal/getexistingcode/' + self.userId + '/' + listId + '/',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            callBack(result);
        },
        async: true
    });
}

UserCodeRepository.prototype.getNewCode = function (listId, callBack) {
    var self = this;

    jQuery.ajax({
        url: '/api/codeportal/getnewcode/' + self.token + '/' + listId + '/',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            callBack(result);
        },
        async: true
    });
}