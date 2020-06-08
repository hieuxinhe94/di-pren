define(["jquery"],
    function ($) {
        return new CodeDisplayer();
    });

function CodeDisplayer() {

}

CodeDisplayer.prototype.displayCode = function (code, codeInfo, $codeContainer) {
    $("<div>")
        .addClass("code-info")
        .text(codeInfo)
        .appendTo($codeContainer);
    $("<div>")
        .addClass("code-presentation")
        .text(code)
        .appendTo($codeContainer);
}

CodeDisplayer.prototype.displayCodeNotAvailable = function (notAvailableText, $codeContainer) {
    $("<div>")
        .addClass("code-not-available")
        .html(notAvailableText)
        .appendTo($codeContainer);
}

CodeDisplayer.prototype.displayGiveAway = function (giveAwayTo, giveAwayInfo, $codeContainer) {
    $("<div>")
        .addClass("code-info")
        .text(giveAwayInfo)
        .appendTo($codeContainer);
    $("<div>")
        .addClass("code-presentation")
        .text(giveAwayTo)
        .appendTo($codeContainer);
}
