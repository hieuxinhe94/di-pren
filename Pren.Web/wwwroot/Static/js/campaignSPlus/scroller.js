
var scroller = {

    scrollTo: function (element) {
        $('html, body').animate({ scrollTop: element.offset().top }, 'slow');
    }
};
