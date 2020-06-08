List.prototype.plugins.paging = function (locals, options) {
    var list = this;
    var pagingList;
    var init = function () {
        options = options || {};
        pagingList = new List(list.listContainer.id, {
            listClass: options.pagingClass || 'paging',
            item: "<li><div class='page'></div></li>", // Have to contain something, can't set valueName at root element
            valueNames: ['page', 'dotted'],
            searchClass: 'nosearchclass',
            sortClass: 'nosortclass'
        });
        list.on('updated', refresh);
        refresh();
    };

    var refresh = function () {
        var l = list.matchingItems.length,
            index = list.i,
            page = list.page,
            pages = Math.ceil(l / page),
            currentPage = Math.ceil((index / page)),
            innerWindow = options.innerWindow || 2,
            left = options.left || options.outerWindow || 0,
            right = options.right || options.outerWindow || 0,
            right = pages - right;

        pagingList.clear();

        var prev = pagingList.add({
            page: "<a class='prev' href='javascript:function Z(){Z=\"\"}Z()'>Föregående</a>",
            dotted: false
        })[0];
        addEvent(prev.elm, (currentPage == 1 ? 1 : currentPage - 1), page);

        for (var i = 1; i <= pages; i++) {
            var className = (currentPage === i) ? "current" : "";

            if (is.number(i, left, right, currentPage, innerWindow)) {
                var item = pagingList.add({
                    page: "<a class='" + className + "' href='javascript:function Z(){Z=\"\"}Z()'>" + (i) + "</a>",
                    dotted: false
                })[0];
                addEvent(item.elm, i, page);
            } else if (is.dotted(i, left, right, currentPage, innerWindow, pagingList.size())) {
                pagingList.add({
                    page: "...",
                    dotted: true
                });
            }
        }

        var next = pagingList.add({
            page: "<a class='next' href='javascript:function Z(){Z=\"\"}Z()'>Nästa</a>",
            dotted: false
        })[0];
        addEvent(next.elm, (currentPage == pages ? pages : currentPage + 1), page);
    };

    var is = {
        number: function (i, left, right, currentPage, innerWindow) {
            return this.left(i, left) || this.right(i, right) || this.innerWindow(i, currentPage, innerWindow);
        },
        left: function (i, left) {
            return (i <= left);
        },
        right: function (i, right) {
            return (i > right);
        },
        innerWindow: function (i, currentPage, innerWindow) {
            return (i >= (currentPage - innerWindow) && i <= (currentPage + innerWindow));
        },
        dotted: function (i, left, right, currentPage, innerWindow, currentPageItem) {
            return this.dottedLeft(i, left, right, currentPage, innerWindow)
            || (this.dottedRight(i, left, right, currentPage, innerWindow, currentPageItem));
        },
        dottedLeft: function (i, left, right, currentPage, innerWindow) {
            return ((i == (left + 1)) && !this.innerWindow(i, currentPage, innerWindow) && !this.right(i, right))
        },
        dottedRight: function (i, left, right, currentPage, innerWindow, currentPageItem) {
            if (pagingList.items[currentPageItem - 1].values().dotted) {
                return false
            } else {
                return ((i == (right)) && !this.innerWindow(i, currentPage, innerWindow) && !this.right(i, right))
            }
        }
    };

    var addEvent = function (elm, i, page) {
        ListJsHelpers.addEvent(elm, 'click', function () {
            list.show((i - 1) * page + 1, page);
            $(window).scrollTop($('#wine-container').offset().top);
        });
    };

    init();
    return this;
};