define(["jquery", "dom/misc"], function ($, dom) {
    return new DateAndTime(dom);
});

function DateAndTime(dom) {
    this.dom = dom;
    if (dom.dateAndTimeWidget.length) {
        loadDatesForWidget(dom.dateAndTimeWidget.find(dom.widgetTimeSelector), dom.dateAndTimeWidget.find(dom.widgetDateSelector));
    }
}

var loadDatesForWidget = function(timeSelector, dateSelector) {

    var date = new Date;

    if (dateSelector) {
        var year = date.getFullYear();
        var month = date.getMonth();
        var months = new Array('Januari', 'Februari', 'Mars', 'April', 'Maj', 'Juni', 'Juli', 'Augusti', 'September', 'Oktober', 'November', 'December');
        var d = date.getDate();

        var resultDate = '' + d + ' ' + months[month] + ' ' + year;
        dateSelector.text(resultDate);
    }

    if (timeSelector) {
        var h = date.getHours();
        if (h < 10) {
            h = "0" + h;
        }
        var m = date.getMinutes();
        if (m < 10) {
            m = "0" + m;
        }

        var resultTime = '' + h + ':' + m;
        timeSelector.text(resultTime);
    }

    setTimeout(function () {
        loadDatesForWidget(timeSelector);
    }, 1000);

    return true;
}