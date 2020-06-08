var Faq = function (options) {
    var self = this;

    if (options != undefined) {
        if (options.topicsElement != undefined) {
            this.topicsElement = options.topicsElement;
            this.topicsElement.on('change', function (e) { self._handleChange(e, this); });
        }

        this.onTopicsLoaded = options.onTopicsLoaded;
        this.onTopicsChanged = options.onTopicsChanged;
        this.onItemsLoaded = options.onItemsLoaded;
    }
}

Faq.prototype._handleChange = function (e, element) {
    this.populateFaqItemsByTopic(element.value);
}

Faq.prototype.populateFaqItemsByTopic = function (topic) {
    var self = this;    
    jQuery.ajax({
        url: '/api/faq/getitemsbytopics/' + topic + '/5/-num_comments',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            self.onTopicsChanged(result);
        },
        async: true
    });
}

Faq.prototype.populateFaqTopics = function () {
    var self = this;
    jQuery.ajax({
        url: '/api/faq/gettopics/5/-num_comments',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            self.onTopicsLoaded(result);
        },
        async: true
    });
}

Faq.prototype.populateFaqItems = function () {
    var self = this;
    jQuery.ajax({
        url: '/api/faq/getitems/5/-num_comments',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            self.onItemsLoaded(result);
        },
        async: true
    });
}




