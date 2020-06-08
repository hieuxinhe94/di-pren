define(["jquery", "pubsub"], function ($, pubsub) {
    return new AjaxHandler();        
});

function AjaxHandler() {}

AjaxHandler.prototype.makeAjaxRequest = function (apiUrl, method, callbackOnSuccess, callbackOnError, callbackGeneral, data) {
    if (apiUrl === undefined || apiUrl === null) {
        return;
    }

    $.ajax({
        url: apiUrl,
        method: method,
        data: data,
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            //general fail if result is false
            if (result === false) {
                doCallbackWithResult(callbackOnError, result);
                doCallback(callbackGeneral);
                return;
            }
            callbackOnSuccess(result);
            doCallback(callbackGeneral);
        },
        error: function (xhr, textStatus, error) {
            if (xhr.status === 403) {
                $.publish('forbidden');
            } else {
                doCallbackWithResult(callbackOnError, xhr.responseText);
                doCallback(callbackGeneral);
            }
        },
        async: true
    });

    function doCallback(callback) {
        if (callback !== undefined && callback !== null) {
            callback();
        }
    }

    function doCallbackWithResult(callback, result) {
        if (callback !== undefined && callback !== null) {
            callback(result);
        }
    }

};

