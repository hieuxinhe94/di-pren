function GetCodeButtonGenerator(userCodeRepository, codeDisplayer) {
    var self = this;
    self.userCodeRepository = userCodeRepository;
    self.codeDisplayer = codeDisplayer;
}

GetCodeButtonGenerator.prototype.createGetCodeButton = function (listId, codeInfo, buttonText, notAvailableText, $codeButtonContainer) {
    $("<button>")
        .addClass("btn btn-primary")
        .text(buttonText)
        .on("click", function () {
            var btn = $(this);
            self.userCodeRepository.getNewCode(listId, function (newCode) {
                if (newCode != '') {
                    self.codeDisplayer.displayCode(newCode, codeInfo, $codeButtonContainer);
                    btn.remove();
                } else {
                    self.codeDisplayer.displayCodeNotAvailable(notAvailableText, $codeButtonContainer);
                    btn.remove();
                }
            });
        })
        .appendTo($codeButtonContainer);
}