var DateHandler = function (dateElements, startDate, endDate) {
    this.dateElements = dateElements;
    this.startDate = startDate;
    this.endDate = endDate;
}

DateHandler.prototype.init = function() {
    this.dateElements.datepicker({
        format: "yyyy-mm-dd",
        weekStart: 1,
        language: "sv",
    });

    if (this.startDate != null && this.startDate != undefined) {
        this.setFunction("setStartDate", this.startDate);
    }

    if (this.endDate != null && this.endDate != undefined) {
        this.setFunction("setEndDate", this.endDate);
    }
}

DateHandler.prototype.setFunction = function (functionName, startDate, dateInput) {
    var elements = dateInput != null ? dateInput : this.dateElements;

    if (elements != null) {
        elements.datepicker(functionName, startDate);
    }
}


