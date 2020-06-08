define("epi/shell/widget/TimeSpinner", [
    "dojo",
    "epi/datetime",
    "dojox/form/TimeSpinner"
], function (dojo, epiDateTime, TimeSpinner) {

    return dojo.declare('epi.shell.widget.TimeSpinner', [TimeSpinner], {
        // tags:
        //      public

        // smallDelta: [public] Integer
        //      It will cause the arrow clicks, up and down keys to shift 1 hours.
        smallDelta: 60,

        // largeDelta: [public] Integer
        //      It will cause the page up and page down keys to shift 6 hours.
        largeDelta: 360,

        _arrowPressed: function (/*Node*/nodePressed, /*Number*/direction, /*Number*/increment) {
            this.inherited(arguments);
            this.onChange(this.get('value'));
        }
    });
});
