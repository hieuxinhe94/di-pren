function Slider($campaignElements, $campaignContainer, width) {
    /// <summary>
    /// Class for setting up slider. Slider will not publish or subscribe to events.
    /// </summary>
    /// <param name="$campaignElements">The campaign jQuery objects</param>
    /// <param name="$campaignContainer">The campaign container jQuery object</param>
    /// <param name="width">The width will determine when slider will be created/destroyed</param>
    var self = this;
    self.campaignElements = $campaignElements;
    self.campaignContainer = $campaignContainer;
    self.campaignChangedEventName = "campaignChanged";
    self.sliderCreatedEventName = "sliderCreated";
    self.sliderDestroyedEventName = "sliderDestroyed";
    self.width = width;
    self.active = false;

    setUpSlider();

    function setUpSlider() {

        // No slider if only one campaign
        if (self.campaignElements.length == 1)
            return;

        if ($(window).width() < self.width && !self.active) {
            createSlider();
        }

        // Check if we should create or destroy slider
        $(window).resize(function () {
            if ($(window).width() < self.width && !self.active) {
                // Create slider if mobile
                createSlider();
            } else if ($(window).width() >= self.width && self.active) {
                // Destroy slider if desktop
                destroySlider();
            }
        });
    }

    // Function for creating slider
    function createSlider() {

        self.slider = self.campaignContainer.bxSlider({
            controls: true,
            nextSelector: "#slider-next",
            prevSelector: "#slider-prev",
            onSlideAfter: function (slide) {
                //trigger event that campaign object listens to
                $.publish(self.campaignChangedEventName, $(slide).find('.pren-selection'));
            }
        });

        $.publish(self.sliderCreatedEventName);      
        self.active = true;
    };

    // Function for destroying slider
    function destroySlider() {        
        self.slider.destroySlider();
        self.active = false;
        $.publish(self.sliderDestroyedEventName);
    };
};
