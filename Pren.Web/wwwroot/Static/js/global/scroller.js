function Scroller(options) {
    this.scrollOnLoad = options.scrollOnLoad;
    this.scrollToThis = options.scrollToThis;
    if (this.scrollOnLoad && this.scrollToThis.length) {
        this.scrollToElement(this.scrollToThis);
    }
}

Scroller.prototype.scrollToElement = function (scrollToThis) {
    
    $('html, body').animate({ scrollTop: scrollToThis.offset().top }, 'slow');
    
};

