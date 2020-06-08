
define(["jquery", "dom/tabs"], function ($, dom) {
    return new Menu(dom);
});


function Menu(dom) {
    var self = this;
    self.dom = dom;
    self.tabs = dom.tabElements;
    self.anchors = dom.anchorElements;
    self.activeAnchor = self.anchors.first();
    
    var hash = location.hash;
    if (hash != "") {
        self.setActiveTab(hash);
    } else {
        self.setActiveTab("#" + this.activeAnchor.attr("id"));
    }

    self.tabs.on("click", function () {
        var toggleBtn = self.dom.toggleBtn;
        if (toggleBtn.is(":visible")) {
            toggleBtn.trigger("click");
        }
    });

    self.init();
}

Menu.prototype.init = function () {
    var self = this;

    $(window).on("scroll", function() {
        if (self.bottomIsReached()) {
            self.activeAnchor = self.anchors.last();
        } else {
            $.each(self.anchors, function(index, element) {
                var anchor = $(element);
                var anchorPosition = self.getAnchorPosition(anchor);
                var activeAnchorPosition = self.getAnchorPosition(self.activeAnchor);
                // Closest to 0 wins
                if (Math.abs(activeAnchorPosition) > Math.abs(anchorPosition)) {
                    self.activeAnchor = anchor;
                }
            });
        }
        var anchorHash = "#" + self.activeAnchor.attr("id");
        self.setActiveTab(anchorHash);
        //location.hash = anchorHash;
    });
}

Menu.prototype.setActiveTab = function (anchor) {
    this.tabs.removeClass("active");
    this.tabs.find("a[href='" + anchor + "']").parent().addClass("active");
}

Menu.prototype.getAnchorPosition = function (anchor) {
    var scrollTop = parseInt($(window).scrollTop());

    return (parseInt(anchor.offset().top) - scrollTop);
}

Menu.prototype.bottomIsReached = function () {
    var scrollHeight = $(document).height();
    var scrollPosition = $(window).height() + $(window).scrollTop();

    return Math.round(((scrollHeight - scrollPosition) / scrollHeight) * 100) / 100 === 0;
    
}
