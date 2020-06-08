var sliderHandler =
{
    setUp: function() {

        // Setup BX Sslider
        var setupBXSlider = function () {
            this.active = false;
            this.setupListeners();
        };

        setupBXSlider.prototype.setupListeners = function () {
            var self = this;

            if ($(window).width() < 990 && !self.active) {
                self.createSlider();
            }

            // Check if we should create or destroy slider
            $(window).resize(function () {
                if ($(window).width() < 990 && !self.active) {
                    // Create slider if mobile
                    self.createSlider();
                } else if ($(window).width() >= 990 && self.active) {
                    // Destroy slider if desktop
                    self.destroySlider();
                }
            });
        };

        // Function for creating slider
        setupBXSlider.prototype.createSlider = function () {
            // Unbind events on campaign. Let slider decide which campaign that is selected
            var campaigns = sliderHandler.elements.getCampaigns();
            campaigns.unbind();

            this.slider = sliderHandler.elements.getPrenSelectContainer().bxSlider({
                controls: true,
                nextSelector: "#slider-next",
                prevSelector: "#slider-prev",
                startSlide: campaigns.index(campaignSelectionHandler.getDefaultCampaign()),
                onSlideAfter: function (slide) {
                    campaignSelectionHandler.setSelectedCampaign($(slide).find('.pren-selection'));
                }
            });

            this.active = true;
        };


        // Function for destroying slider
        setupBXSlider.prototype.destroySlider = function () {
            // Add event on campaign
            sliderHandler.elements.getCampaigns().click(campaignSelectionHandler.prenSelectionClick);
            this.slider.destroySlider();
            this.active = false;
        };

        new setupBXSlider();
    },
    elements: {
        getCampaigns: function () { return elementFactory.getElement(elementFactory.elements.campaigns); },
        getPrenSelectContainer: function () { return $('.pren-select-container'); },
        getSliderDirection: function () { return $("#slider-direction"); },
    },
}