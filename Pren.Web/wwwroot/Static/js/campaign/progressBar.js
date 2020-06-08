var progressBarHelper = {
    activateStep: function (step) {
        var progressBar = elementFactory.getElement(elementFactory.elements.progressBar);
        progressBar.removeClass(this.steps.step2);
        progressBar.removeClass(this.steps.step3);
        progressBar.addClass(step);
    },
    steps: {
        step2: "first-complete",
        step3: "second-complete"
    }
};