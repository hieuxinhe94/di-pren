define([
    "dojo/_base/connect",
    "dojo/_base/declare",
    "dojo/dom-construct",
    "dojo/on",

    "dijit/_CssStateMixin",
    "dijit/_Widget",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",

    "epi/dependency",
    "epi/epi",
    "epi/shell/widget/_ValueRequiredMixin",

    "dojo/text!./templates/CampaignSelectorDialog.html"
],
function (
    connect,
    declare,
    domconstruct,
    on,

    _CssStateMixin,
    _Widget,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,

    dependency,
    epi,
    _ValueRequiredMixin,

    template
) {
    return declare("pren.editors.CampaignSelectorDialog", [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin, _CssStateMixin, _ValueRequiredMixin], {

        templateString: template,
        intermediateChanges: true,
        postCreate: function () {
            this.inherited(arguments);

            this.txtSelectedCampaign.set("value", this.selectedCampaign);

            this.getCampaigns(false);

            this.connect(this.txtFilterCampaigns, "onKeyDown", this.filterCampaigns);
            this.connect(this.txtFilterCampaigns, "onKeyUp", this.filterCampaigns);
            this.connect(this.btnUpdateCampaigns, "onClick", this.updateCampaigns);                
        },
        updateCampaigns: function () {
            $("#campaignContainer div.campaign-item").remove();
            $("#campaignContainer div.loading-campaigns").show();
            
            this.getCampaigns(true);
        },
        getCampaigns: function (updateCache) {            
            var that = this;
            var campaignContainer = this.campaignContainer;

            jQuery.ajax({
                url: '/api/campaign/' + updateCache,
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    $("#campaignContainer div.loading-campaigns").hide();
                    result = JSON.parse(result);
                    $.each(result, function (index, value) {
                        that.createCampaignItem(value, campaignContainer);
                    });
                },
                async: true
            });
        },
        createCampaignItem: function (campaign, campaignContainer) {

            var that = this;

            var content = campaign.CampId + " " + campaign.CampNo + " " + campaign.CampName;
            var className = "campaign-item";

            if (that.selectedCampaign == campaign.CampId | that.selectedCampaign == (campaign.CampId + "|" + campaign.CampNo)) {
                className += " selected";
                this.createSelectedCampaignItem(content);
            }

            var campaignItem = domconstruct.create("div", {
                className: className,
                innerHTML: content,
            }, campaignContainer);

            $(campaignItem).attr("data-campaignnumber", campaign.CampId);

            on(campaignItem, "click", function (event) {
                that.txtSelectedCampaign.set("value", campaign.CampId + "|" + campaign.CampNo);
                that.createSelectedCampaignItem(content);

                $(".selected").removeClass("selected");
                $(this).addClass("selected");
            });
        },
        createSelectedCampaignItem: function (content) {
            this.selectedCampaignContainer.innerHTML = '';
            domconstruct.create("div", {
                className: "selected-campaign",
                innerHTML: content,
            }, this.selectedCampaignContainer);
        },
        filterCampaigns :  function() {
            var filterBy = this.txtFilterCampaigns.get("value");
            $("#campaignContainer div.campaign-item").each(function () {
                if ($(this).attr("data-campaignnumber").toLowerCase().substring(0, filterBy.length) === filterBy.toLowerCase()) {
                    $(this).show();
                } else {
                    $(this).hide();
                }                
            });
        },
        _getValueAttr: function () {
            return this.txtSelectedCampaign.get("value");
        }
    });
});