
define([
    "jquery",
    "func/ajax",
    "faqdom/faqdom",
    "underscore",
    "text!faqTemplates/faqTopics.html",
    "text!faqTemplates/faqItems.html"],
    function ($, ajax, dom, _, topicsTemplate, itemsTemplate) {
        return new Faq(ajax, dom, topicsTemplate, itemsTemplate);
    });


var Faq = function (ajax, dom, topicsTemplate, itemsTemplate) {
    this.ajax = ajax;
    this.dom = dom;
    this.topicsTemplate = _.template(topicsTemplate);
    this.itemsTemplate = _.template(itemsTemplate);

    this.init();
}

Faq.prototype.init = function () {
    var self = this;

    this.dom.topicsPh.on("change", "select", function () {
        self.populateFaqItemsByTopic($(this).val());
    });    
}

Faq.prototype.populateFaqItemsByTopic = function (topic) {
    var self = this;

    var successCallback = function (data) {
        var items = $(self.itemsTemplate({ items: data }));
        self.dom.itemsPh.html(items);
    };

    var errorCallback = function () { }

    self.ajax.makeAjaxRequest("/api/faq/getitemsbytopics/" + topic + "/5/-num_comments", 'GET', successCallback, errorCallback);
}

Faq.prototype.populateFaqTopics = function () {
    var self = this;

    var successCallback = function (data) {
        var topics = $(self.topicsTemplate({ topics: data }));
        topics.appendTo(self.dom.topicsPh);
    };

    var errorCallback = function () {}

    self.ajax.makeAjaxRequest("/api/faq/gettopics/5/-num_comment" , 'GET', successCallback, errorCallback);
}


