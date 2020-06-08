function Campaign($campaignElements, $campaignSubmitters, uspListsSelector) {
    var self = this;
    self.campaignElements = $campaignElements;
    self.campaignSubmitters = $campaignSubmitters;
    self.uspListsSelector = uspListsSelector;
    self.campaignChangedEventName = "campaignChanged";
    self.sliderCreatedEventName = "sliderCreated";
    self.sliderDestroyedEventName = "sliderDestroyed";
    self.campaignCssClass = self.campaignElements.first().attr("class");

    setUpCampaigns();
    setUpEvents();
    setUpEventListeners();

    function setUpCampaigns() {
        var noOfCampaigns = self.campaignElements.length;
        var defaultCampaign = $(self.campaignElements[noOfCampaigns > 2 ? 1 : 0]);
        setSelectedCampaign(defaultCampaign);        

        // Set up single campaign
        if (noOfCampaigns === 1) {
            defaultCampaign.wrapInner("<div class='col-md-4'></div>");
            defaultCampaign.prepend("<div class='col-md-8'></div>");
            defaultCampaign.find(".pren-selection-image-and-text-container").appendTo(defaultCampaign.find("div").first());
            defaultCampaign.parents(".container").addClass("single");
            self.campaignSubmitters.css("display", "block");
        } else {
            setUpUspLists();
        }
    }

    function setUpEvents() {
        self.campaignSubmitters.on("click", function (event) {
            //prevent event from bubbling, instead we trigger the event and then submits form
            event.preventDefault();

            $.publish(self.campaignChangedEventName, $(this).parents("." + self.campaignCssClass));

            $("form").submit();
        });
    }

    function setUpUspLists() {
        // Must use selector instead of element collection.
        // This is because slider can duplicate elements.
        $(self.uspListsSelector).css("height", "");
        $(self.uspListsSelector).equalElementHeight(10);
    }

    function setSelectedCampaign(campaign) {
        // Set hidden input fields
        $("#campaigncontentidinput").val(campaign.data("id"));
    }

    function setUpEventListeners() {
        $.subscribe(self.campaignChangedEventName, function (_, campaign) {
            setSelectedCampaign($(campaign));
        });

        $.subscribe(self.sliderCreatedEventName, function (_) {
            setUpUspLists();
        });

        $.subscribe(self.sliderDestroyedEventName, function (_) {
            setUpUspLists();
        });
    }
};

// Funtion to adopt iframe styling.
// Used because the block type is used on other pages.
Campaign.prototype.setUpElements = function () {
    this.campaignElements.each(function () {
        $(this).find("h2").appendTo($(this).find(".pren-selection-product-copy"));
    });
};