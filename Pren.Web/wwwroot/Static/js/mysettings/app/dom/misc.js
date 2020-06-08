define(["jquery"], function ($) {
    return {
        dateElements: $(".dateinput"),
        loadModal: $("#load-modal"),
        forbiddenModal: $("#forbidden-modal"),
        errorModal: $("#error-modal"),
        navigationcontainer: $("#navcontainer"),
        toggleblocks: $(".textwidgetblock h3, .shortcutwidgetblock h3"),
        dateAndTimeWidget: $(".date-and-time-widget"),
        widgetTimeSelector: ".widget-time",
        widgetDateSelector: ".widget-date",
        subscriptionFunctionArea: $("#accordion"),
        shortCuts: {
            selector: "a.shortcut", 
            reclaim: $("#reclaimHeading a"),
            subssleep: $("#sleepHeading a"),
            tmpaddress: $("#tmpaddressHeading a"),
            permaddress: $("#permaddressHeading a")
        }
    }
});