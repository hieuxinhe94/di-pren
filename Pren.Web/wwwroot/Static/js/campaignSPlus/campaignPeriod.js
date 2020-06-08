var campaignPeriodHandler = {
    onPeriodChanged: null,
    init: function (onPeriodChanged) {
        this.onPeriodChanged = onPeriodChanged;
        this.setUpEvents();
        this.setUpPeriods();
    },
    setUpPeriods: function () {
        elementFactory.getElement(elementFactory.elements.campaigns).each(function () {
            var periodContainer = $(this);

            //set default period
            var defaultPeriodButton = campaignPeriodHandler.getDefaultPeriodButton(periodContainer);
            campaignPeriodHandler.setPeriodAsSelected(periodContainer, defaultPeriodButton);

            //add click event for periodbuttons
            periodContainer.find(".pren-range-button").click(function (e) {
                e.preventDefault();
                campaignPeriodHandler.changePeriod(periodContainer, $(this));
            });
        });
    },
    setPeriodAsSelected : function(periodContainer, periodElement) {
        //set selected class on period button
        periodElement.siblings().removeClass("selected");
        periodElement.addClass("selected");

        //set price on container
        periodContainer.find(".pren-selection-price .price-text").html(periodElement.data("price"));

        //set total price on container
        periodContainer.find(".pren-selection-price-subtext").html(periodElement.data("totalpricetext"));
    },
    setUpEvents: function () {
        //if provided - set up subscriber to event when selected campaign is changed
        if (campaignPeriodHandler.onPeriodChanged != undefined) {
            customEventHandler.subscribe(selectedCampaign.events.selectedCampaignChanged, function (selectedCampaign) {
                campaignPeriodHandler.onPeriodChanged(selectedCampaign);
            });
        }
    },
    changePeriod: function (periodContainer, periodElement) {

        //compare current selected campaign with the one clicked and do nothing if same
        if (selectedCampaign.get().periodButtonElement != null &&
            selectedCampaign.get().periodButtonElement[0] == periodElement[0]) {
            return;
        }

        campaignPeriodHandler.setPeriodAsSelected(periodContainer, periodElement);

        //set selected campaign (will trigger the onPeriodChanged event)         
        selectedCampaign.set(
            periodElement.data("campidcardandinvoice"),
            periodElement.data("campidautogiro"),
            periodContainer.find(".pren-selection-title").text(),
            periodElement.data("price"),
            periodContainer.data("campisdigital"),
            periodContainer.data("campisstudent"),
            periodElement.data("istrial"),
            periodElement.data("istrialfree"),
            periodContainer.data("id"),
            periodElement,
            periodContainer.data("campispaywall"),
            periodElement.data("hidecardpayment"),
            periodElement.data("hideinvoicepayment"));
    },
    getDefaultPeriodButton: function (periodContainer) {

        var periodButtonElement = campaignPeriodHandler.getPeriodButtonElementByCampNo(periodContainer, $("#campidinput").val());

        if (periodButtonElement != null) {
            return periodButtonElement;
        }

        var periodButtons = periodContainer.find(".pren-range-button");
        return $(periodButtons[periodButtons.length > 2 ? 2 : 0]);
    },
    getPeriodButtonElementByCampNo: function (periodContainerElement, campNo) {
        if (campNo.length == 0) {
            return null;
        }

        var campidcardandinvoice = periodContainerElement.find('.pren-range-button[data-campidcardandinvoice="' + campNo + '"]');
        if (campidcardandinvoice.length) {
            return campidcardandinvoice;
        }

        var campidautogiro = periodContainerElement.find('.pren-range-button[data-campidautogiro="' + campNo + '"]');
        if (campidautogiro.length) {
            return campidautogiro;
        }

        return null;
    }
};