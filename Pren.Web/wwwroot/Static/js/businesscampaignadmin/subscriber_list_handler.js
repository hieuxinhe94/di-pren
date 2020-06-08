function SubscriberListHandler(subscriberListContainerEl, templateString, subEventNameForPopulate, pubEventNameOnDeleteClick, pubEventNameOnListPopulated) {
    var self = this;
    self.subscriberListContainerEl = subscriberListContainerEl;
    self.template = _.template(templateString);
    self.pubEventNameOnDeleteClick = pubEventNameOnDeleteClick;
    self.subEventNameForPopulate = subEventNameForPopulate;
    self.pubEventNameOnListPopulated = pubEventNameOnListPopulated;

    $.subscribe(self.subEventNameForPopulate, function (notUsedElem, subscribers, emptylist) {
        self.populateList(subscribers, emptylist);
    });
};

SubscriberListHandler.prototype.populateList = function (subscribers, clearList) {
    var self = this;

    // Hide and empty the current list
    if (clearList != null && clearList == true) {
        self.subscriberListContainerEl.empty();
    }

    var subscriberItems = $(self.template({ items: subscribers }));

    // Populate container with template and subscribers
    subscriberItems.appendTo(self.subscriberListContainerEl);

    // Add event on remove btn
    subscriberItems.find('a.remove').on('click', function (e) {
        e.preventDefault();
        var $this = $(this);
        $.publish(self.pubEventNameOnDeleteClick, [$this.data('removeidentifier'), $this.data('email')]);
    });

    // Add event on remind btn
    subscriberItems.find('a.remind').on('click', function (e) {
        e.preventDefault();
        var $this = $(this);
        $.publish("biz-pending-subscriber-remind", [$this.data('code'), $this.data('email'), $this.closest('.subscriber')]);
    });

    // Add event on regret remove btn
    subscriberItems.find('a.regret-remove').on('click', function (e) {
        e.preventDefault();
        var $this = $(this);
        $.publish("biz-pending-subscriber-regret-delete", [$this.data('removeidentifier'), $this.data('email')]);
    });

    self.subscriberListContainerEl.fadeIn();

    $.publish(self.pubEventNameOnListPopulated);
};

