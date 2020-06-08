var stepActivator = {
    getActiveStep: function () {
        return $("div[data-active='true']");
    },
    setActiveStep: function (stepToActivate) {
        stepActivator.getActiveStep().attr("data-active", "false");
        stepToActivate.attr("data-active", "true");
    },
    deactivateStep: function (stepToDeactivate) {
        stepToDeactivate.removeAttr("data-active");
    }
}