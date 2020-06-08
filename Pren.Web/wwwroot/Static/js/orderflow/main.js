$(".package-view-all__link").click(function () {
    $(this).children('.icon').toggleClass('icon-up');
    $(this).parent().parent().children('.package-listing').slideToggle("slow");
});

$(".js-package-expand").click(function () {
    if (isMobile()) {
        $(this).toggleClass('opened');
        $(this).parent().children('.package-listing').slideToggle("slow");
    }
});

$(".compare-package-link").on("click",
    function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: $("#compare-package").offset().top }, 'slow');
    });

$(".package").on("click",
    function () {
        if (!isMobile()) {
            var href = $(this).find(".package__button a").attr("href");
            if (href) {
                window.location = href;
            }
        }
    });

function isMobile() {
    var hiddenXs = $(".hidden-xs");

    return !hiddenXs.is(":visible");
}