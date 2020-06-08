define([
    "jquery",
    "app/mycodes/codeRepository",
    "app/mycodes/codeDisplayer",
    "app/mycodes/codeGetterButtonGenerator",
    "mycodesdom/mycodesdom"],
    function ($, codeRepo, codeDisplayer, btnGenerator, dom) {
        return new MyCodes(codeRepo, codeDisplayer, btnGenerator, dom);
    });

function MyCodes(
    codeRepository,
    codeDisplayer,
    btnGenerator,
    dom) {

    var self = this;
    self.codeRepository = codeRepository;
    self.codeDisplayer = codeDisplayer;
    self.btnGenerator = btnGenerator;
    self.codeOffer = dom.mycodesUserCodeContainer;
    self.codeOfferFallbackContainer = dom.mycodesNoOfferFallback;

    function setUpCodeOffers() {
        self.codeOffer.each(function () {
            var userCodeContainer = $(this);
            var listId = userCodeContainer.data("listid");
            var codeInfo = userCodeContainer.data("codeinfo");
            var buttonText = userCodeContainer.data("buttontext");
            var notAvailableText = userCodeContainer.data("notavailabletext");
            var isGiveAway = userCodeContainer.data("isgiveaway");

            var errorCallBack = function () {
                userCodeContainer.find(dom.mycodesErrorClass).show();
            }

            // Separate handling for giveaway offers
            if (isGiveAway) {

                self.codeRepository.getExistingGiveAway(listId, function (gaveAwayTo) {
                    if (gaveAwayTo === '') {
                            self.btnGenerator.createGiveAwayForm(listId, codeInfo, buttonText, notAvailableText, userCodeContainer);
                        } else {
                        self.codeDisplayer.displayGiveAway(gaveAwayTo, codeInfo, userCodeContainer);
                        }
                    });

            } else {
                self.codeRepository.getExistingCode(listId, function (code) {
                    if (code === '') {
                        self.btnGenerator.createGetCodeButton(listId, codeInfo, buttonText, notAvailableText, userCodeContainer);
                    } else {
                        self.codeDisplayer.displayCode(code, codeInfo, userCodeContainer);
                    }
                }, errorCallBack);
            }
        });
    }

    function setUpFallback() {
        if (!self.codeOfferFallbackContainer.data("lookfor").length) {
            self.codeOfferFallbackContainer.show();
        }
    }

    MyCodes.prototype.setUpMyCodes = function () {
        if (this.codeOffer.length) {
            setUpCodeOffers();
            setUpFallback();
        }
    }
}