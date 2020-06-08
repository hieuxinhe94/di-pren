function PagingHandler(pagingElement, listContainerElement, bizSubscriptionId, pubClickEventName, subResetEventName, subSetVisibilityEventName) {
    var self = this;
    self.pagingElement = pagingElement;
    self.listContainerElement = listContainerElement;
    self.bizSubscriptionId = bizSubscriptionId;
    self.pubClickEventName = pubClickEventName;
    self.subResetEventName = subResetEventName;
    self.subSetVisibilityEventName = subSetVisibilityEventName;    

    self.skip = self.pagingElement.data("skip");
    self.take = self.pagingElement.data("take");
    self.routeParameters = "/" + self.skip + "/" + self.take;

    // Get number of subscriber rows in container
    function getNoOfSubscriberItems() {
        return self.listContainerElement.find(".subscriber").length;
    }
    
    self.pagingElement.on("click", function () {
        // Update skip and routeParameters
        self.skip = self.skip + self.take;
        self.routeParameters = "/" + self.skip + "/" + self.take;
        $.publish(self.pubClickEventName, [self.bizSubscriptionId, self.routeParameters]);
    });

    $.subscribe(self.subResetEventName, function () {
        // Reset loading indicator
        self.pagingElement.loadingIndicatorReset();
    });

    $.subscribe(self.subSetVisibilityEventName, function () {
        // Hide pager element if paging no longer needed
        if(getNoOfSubscriberItems() < (self.skip + self.take)) {
            self.pagingElement.fadeOut();
        } else {
            self.pagingElement.fadeIn();
        }
    });    

    // Add loading indicator to paging element
    self.pagingElement.loadingIndicatorLoad(function () { return true; });
}

// Use if you want all in current scope as route parameters.
// Example: if take=10 and user paged 9 times (skip=90, take=10), 100 items will be visible in list and scope parameters will be "/0/100"
PagingHandler.prototype.GetAllInCurrentScopeAsRouteParameters = function () {
    return "/0/" + (this.skip + this.take);
}