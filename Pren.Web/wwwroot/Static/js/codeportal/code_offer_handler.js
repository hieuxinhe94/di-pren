function CodeOfferHandler(
    userCodeRepository,
    codeDisplayer,
    getCodeButtonGenerator,
    $codeOffer,
    $codeOfferFallbackContainer) {

    var self = this;
    self.userCodeRepository = userCodeRepository;
    self.codeDisplayer = codeDisplayer;
    self.getCodeButtonGenerator = getCodeButtonGenerator;
    self.codeOffer = $codeOffer;
    self.codeOfferFallbackContainer = $codeOfferFallbackContainer;

    function setUpCodeOffers() {
        self.codeOffer.each(function () {
            var userCodeContainer = $(this);
            var listId = userCodeContainer.data("listid");
            var codeInfo = userCodeContainer.data("codeinfo");
            var buttonText = userCodeContainer.data("buttontext");
            var notAvailableText = userCodeContainer.data("notavailabletext");
            self.userCodeRepository.getExistingCode(listId, function (code) {
                if (code === '') {
                    self.getCodeButtonGenerator.createGetCodeButton(listId, codeInfo, buttonText, notAvailableText, userCodeContainer);
                } else {
                    self.codeDisplayer.displayCode(code, codeInfo, userCodeContainer);
                }
            });
        });
    }

    function setUpFallback() {
        if (!$(self.codeOfferFallbackContainer.data("lookfor")).length) {
            self.codeOfferFallbackContainer.show();
        }
    }

    setUpCodeOffers();
    setUpFallback();
}