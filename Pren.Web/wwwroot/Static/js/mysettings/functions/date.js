define(["jquery","datepicker", "datepickerSv", "dom/misc"], function ($, datepicker, datepickerSv, dom) {
    return new DateHandler(dom.dateElements); //default elements is all inputs with class .inputDate
});

function DateHandler (dateElements) {
    this.dateElements = dateElements;
}

DateHandler.prototype.init = function (startDate, endDate, dateElements) {
    if (dateElements != null) {
        //Override dateelements
        this.dateElements = dateElements;
    }

    this.dateElements.datepicker({
        format: "yyyy-mm-dd",
        weekStart: 1,
        language: "sv",
    });

    if (startDate != null) {
        this.setFunction("setStartDate", startDate);
    }

    if (endDate != null) {
        this.setFunction("setEndDate", endDate);
    }
}

DateHandler.prototype.setFunction = function (functionName, startDate, dateInput) {
    var elements = dateInput != null ? dateInput : this.dateElements;

    if (elements != null) {
        elements.datepicker(functionName, startDate);
    }
}


