var campaignSelectionHandler = {
    setUpCampaignSelection: function (callback) {

        var campaigns = campaignSelectionHandler.elements.getCampaigns();
        var defaultCampaign = campaignSelectionHandler.getDefaultCampaign();

        // If only one campaign
        if (campaigns.length == 1) {
            campaignSelectionHandler.setSelectedCampaign(defaultCampaign);
            campaignSelectionHandler.setUpSingleCampaign(defaultCampaign);
            //No slider needed
            return;
        }

        // Attach click event to campaigns
        campaigns.click(campaignSelectionHandler.prenSelectionClick);

        if (callback != null) {
            callback();
        }

        campaignSelectionHandler.setSelectedCampaign(defaultCampaign);
    },
    setUpSingleCampaign: function (campaign) {
        campaign.wrapInner("<div class='col-md-4'></div>");
        $(campaign).prepend("<div class='col-md-8'></div>");
        campaign.find(".pren-selection-image-and-text-container").appendTo(campaign.find("div").first());

        //Let css know it's only one campaign
        campaignSelectionHandler.elements.getSliderTopContainer().addClass("single");
    },
    prenSelectionClick: function () {
        campaignSelectionHandler.setSelectedCampaign($(this));
    },
    setSelectedCampaign: function (campaign) {
        // Remove selected on all campaigns
        campaignSelectionHandler.elements.getCampaigns().removeClass("selected");
        // Set clicked campaign as selected
        campaign.addClass("selected");
        // Change period to clicked campaign and its selected campaign period
        // changePeriod will trigger event selectedCampaignChanged
        // There is a listener in campaignPeriodHandler:onPeriodChanged
        // There is also a listener in stepPayment:resetDigitalAddressForm
        campaignPeriodHandler.changePeriod(campaign, campaign.find(".pren-range-button.selected"));
    },
    getDefaultCampaign: function () {

        // Try to get a selected periodbutton by campno in input and return parent (campaign) if found
        var periodButtonElement = campaignPeriodHandler.getPeriodButtonElementByCampNo(campaignSelectionHandler.elements.getCampaigns(), campaignSelectionHandler.elements.getCampIdInput().val());

        if (periodButtonElement != null) {
            return periodButtonElement.parents(".pren-selection");
        }

        var campaigns = campaignSelectionHandler.elements.getCampaigns();
        // DEFAULT, If more than 1 campaign, select the second, otherwise the first
        return $(campaigns[campaigns.length > 2 ? 1 : 0]);
    },
    elements: {
        getSliderTopContainer: function () { return elementFactory.getElement(elementFactory.elements.sliderTopContainer); },
        getCampaigns: function () { return elementFactory.getElement(elementFactory.elements.campaigns); },
        getCampIdInput: function () { return $("#campidinput"); },
    },
}