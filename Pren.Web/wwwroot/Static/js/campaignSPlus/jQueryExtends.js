jQuery.fn.extend({
    equalChildWidth: function () {
        // Each of the specified elements in the dom
        return this.each(function () {
            // Get each of specified elements children on the first level
            var childElements = $(this).children().filter(":visible");
            // Calculate width
            var childElementCount = childElements.length;
            var width = 100 / childElementCount;
            // Set width
            childElements.width(width + "%");
        });
    },
    equalElementHeight: function (addheight) {

        if (addheight === undefined) {
            addheight = 0;
        }

        // Calculate highest hight
        var highestHeight = 0;
        $(this).each(function () {
            if ($(this)[0].scrollHeight > highestHeight) {
                highestHeight = $(this)[0].scrollHeight;
            }
        });

        // Each of the specified elements in the dom
        return this.each(function () {
            $(this).height(highestHeight + addheight);
        });
    },
    textWidth: function() {
        var that = jQuery(this);
        var htmlOrg = that.html();

        if (that[0].nodeName == 'INPUT') {
            htmlOrg = that.val();
        }

        var htmlCalcS = '<span>' + htmlOrg + '</span>';
        jQuery('body').append(htmlCalcS);
        var lastspan = jQuery('span').last();
        lastspan.css({
            'font-size': that.css('font-size'),
            'font-family': that.css('font-family')
        });
        var width = lastspan.width() + 12;
        lastspan.remove();
        return width;
    }
});
