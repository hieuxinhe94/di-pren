var campaignSummary = {
    updateSummaryInfo: function (selectedCampaign) {
        this.populatePrenRangeOptions(selectedCampaign.periodButtonElement);

        //remove old custom-summarys
        $(".order-table tbody .custom-summary").remove();

        //get selected periodbuttonelemts custom summary table
        var summaryTable = selectedCampaign.periodButtonElement.next(".summary-table");

        //add new custom summarys
        summaryTable.find("tr").each(function() {
            $(".order-table tbody").append($(this).clone().addClass("custom-summary"));
        });
        
        campaignSummary.elements.getSummarySelectedTitle().text(selectedCampaign.title);
        campaignSummary.elements.getSummarySelectedPrice().html(selectedCampaign.periodButtonElement.data("totalpricetext"));
    },
    populatePrenRangeOptions: function (periodButtonElement) {
        //clear options
        campaignSummary.elements.getPrenRangeSelect().html("");

        //add options
        periodButtonElement.siblings("a").andSelf().each(function () {
            var periodButton = $(this);
            var selectOption = $("<option value='" + periodButton.data("id") + "'>").text(periodButton.data("period"));

            if (periodButton.hasClass("selected")) {
                selectOption.attr("selected", "selected");
            }

            campaignSummary.elements.getPrenRangeSelect().append(selectOption);
        });
    
    },
    setUpEvents: function() {
        campaignSummary.elements.getPrenRangeSelect().change(function (e) {
            var periodBtn = selectedCampaign.properties.periodButtonElement.siblings().andSelf().filter("a[data-id='" + $(this).val() + "']");
            periodBtn.click();
        });
    },
    elements: {
        getPrenRangeSelect: function() { return elementFactory.getElement(elementFactory.elements.summaryPrenRange); },
        getSummarySelectedTitle: function() { return elementFactory.getElement(elementFactory.elements.summarySelectedTitle); },
        getSummarySelectedPrice: function() { return elementFactory.getElement(elementFactory.elements.summarySelectedPrice); },
    }
}