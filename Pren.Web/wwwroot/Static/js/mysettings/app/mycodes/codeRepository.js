define([
    "jquery",
    "func/ajax",
    "classes/subscriber"],
    function ($, ajax, subscriber) {
        return new CodeRepository(ajax, subscriber);
    });


function CodeRepository(ajax, subscriber) {
    this.ajax = ajax;
    this.subscriber = subscriber;
}

CodeRepository.prototype.getExistingCode = function (listId, callBack, errorCallBack) {
    var self = this;

    this.ajax.makeAjaxRequest('/api/codeportal/getexistingcode/' + self.subscriber.myCodesSettings.userId + '/' + listId + '/', 'GET', callBack, errorCallBack);
}

CodeRepository.prototype.getNewCode = function (listId, callBack, errorCallBack) {
    var self = this;
    this.ajax.makeAjaxRequest('/api/codeportal/getnewcode/' + self.subscriber.myCodesSettings.token + '/' + listId + '/', 'GET', callBack, errorCallBack);
}

CodeRepository.prototype.getExistingGiveAway = function (listId, callBack, errorCallBack) {
    var self = this;
    this.ajax.makeAjaxRequest('/api/codeportal/getexistinggiveaway/' + self.subscriber.myCodesSettings.userId + '/' + listId + '/', 'GET', callBack, errorCallBack);
}

CodeRepository.prototype.createNewGiveAway = function (listId, giveToEmail, callBack, errorCallBack) {
    var self = this;
    this.ajax.makeAjaxRequest('/api/codeportal/createnewgiveaway/' + self.subscriber.myCodesSettings.token + '/' + listId + '/' + giveToEmail + '/', 'GET', callBack, errorCallBack);
}