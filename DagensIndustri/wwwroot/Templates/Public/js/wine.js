$(function () {
    Cufon.replace('.infobox-dark h2, .infobox-dark h3', {
        fontFamily: 'Benton Sans DI'
    });

    if (!_.difference) {
        _.difference = function (array) {
            var rest = Array.prototype.concat.apply(Array.prototype, Array.prototype.slice.call(arguments, 1));
            return _.filter(array, function (value) { return !_.contains(rest, value); });
        };
    }

    if (!_.union) {
        _.union = function () {
            return _.uniq(Array.prototype.concat.apply(Array.prototype, arguments));
        };
    }

    var options = {
        item: 'wine-list-item',
        listClass: 'wine-list',
        page: 10,
        plugins: [
            ['paging']
        ]
    };

    _.each(wines || [], function (wine, index, list) {
        wine.Character = wine.CharacterNames.join(', ');
        wine.Origin = wine.Origin2 + ', ' + wine.Origin1;
        wine.Latitude = wine.Latitude == '' ? null : wine.Latitude;
        wine.Longitude = wine.Longitude == '' ? null : wine.Longitude;

        wine.Flag = '';

        if (wine.ThisWeek === true) {
            if (wine.Type == 'Rött vin') {
                wine.Flag = 'is-red';
            }

            if (wine.Type == 'Vitt vin') {
                wine.Flag = 'is-white';
            }

            if (wine.Type == 'Mousserande vin') {
                wine.Flag = 'is-sparkling';
            }

            if (wine.Type == 'Rosévin') {
                wine.Flag = 'is-rose';
            }
        }
    });

    var wineList = new List('wine-container', options, wines);

    var tagGroup1 = [
        'Mousserande vin',
        'Rosévin',
        'Rött vin',
        'Vitt vin'
    ];

    // Fisk, Lamm, Nöt, Fläsk, Fågel, Skaldjur, Aperitif, Buffémat, Dessert, Ost och Vilt

    //var tagGroup2 = [
    //    'Aperitif',
    //    'Apéritif',
    //    'Asiatiskt',
    //    'Fisk',
    //    'Fläsk',
    //    'Fågel',
    //    'Gris',
    //    'Kyckling',
    //    'Lamm',
    //    'Nöt',
    //    'Ost',
    //    'Skaldjur'
    //];

    var tagGroup2 = [
        'Aperitif',
        'Apéritif',
        'Fisk',
        'Lamm',
        'Nöt',
        'Fläsk',
        'Fågel',
        'Skaldjur',
        'Buffémat',
        'Dessert',
        'Ost',
        'Vilt'
    ];

    var wineTags = _.uniq(_.flatten(_.map(wineList.items, function (item) {
        return item.values().CharacterNames;
    })));

    var possibleTags = wineTags;

    var wineTypes = _.uniq(_.flatten(_.map(wineList.items, function (item) {
        return item.values().Type;
    })));

    var possibleTypes = wineTypes;

    var tagList = $('.tag-list');
    tagList.empty();

    _.each(_.intersect(wineTypes, tagGroup1), function (element, index, list) {
        tagList.filter('.tag-group-one').append('<li class="tag-list-item"><span>' + element + '</span></li>');
    });

    _.each(_.intersect(wineTags, tagGroup2), function (element, index, list) {
        tagList.filter('.tag-group-two').append('<li class="tag-list-item"><span>' + element + '</span></li>');
    });

    _.each(_.difference(wineTags, tagGroup1, tagGroup2), function (element, index, list) {
        tagList.filter('.tag-group-three').append('<li class="tag-list-item"><span>' + element + '</span></li>');
    });

    var wineContainer = $('#wine-container');

    var initItem = function (item, element) {
        if (!item.data('initialized')) {
            var img = item.find('img'),
                origin = element.values().Origin2,
                lat = element.values().Latitude,
                long = element.values().Longitude,
                link = item.find('.wine-stock-info a'),
                flag = item.find('.wine-corner'),
                checkbox = item.find('.wine-notify input');

            if (lat !== null && long !== null) {
                origin = lat + ',' + long;
            }

            flag.addClass(element.values().Flag);

            link.attr('href', 'http://www.systembolaget.se/' + element.values().Varnummer).attr('target', '_blank');

            img.attr('src', 'http://maps.googleapis.com/maps/api/staticmap?center=' + origin + '&zoom=4&markers=color:blue|' + origin + '&size=198x174&sensor=false');

            img.parent('a').attr('href', 'http://maps.google.com/?q=' + origin);

            checkbox.val(element.values().Varnummer);

            item.data('initialized', true);
        }
    };

    _.each(wineList.items, function (element) {
        var item = $(element.elm);
        initItem(item, element);
    });

    var selectedWines = [];

    $('.wine-container').delegate('input:checkbox', 'change', function (e) {
        if ($(this).is(':checked')) {
            selectedWines.push($(this).val());
        } else {
            selectedWines = _.without(selectedWines, $(this).val());
        }

        if (selectedWines.length > 0) {
          $('.bottom-send-bar .btn-send').removeClass('btn-disabled');
        } else {
          $('.bottom-send-bar .btn-send').addClass('btn-disabled');
        }
    });

    wineList.on('updated', function () {
        _.each(wineList.visibleItems, function (element, index, list) {
            var item = $(element.elm);
            initItem(item, element);
        });
    });

    var updateTags = function (items, selected) {
        if (selectedWines.length > 0) {
          $('.bottom-send-bar .btn-send').removeClass('btn-disabled');
        } else {
          $('.bottom-send-bar .btn-send').addClass('btn-disabled');
        }

        possibleTags = _.uniq(_.flatten(_.map(items, function (item) {
            return item.values().CharacterNames;
        })));

        possibleTypes = _.uniq(_.flatten(_.map(items, function (item) {
            return item.values().Type;
        })));

        var possibleValues = _.union(possibleTags, possibleTypes);

        $('.tag-list-item', tagList).each(function () {
            var item = $(this),
                label = item.text();

            if (_.indexOf(possibleValues, label) == -1 && selected.length > 0) {
                item.addClass('is-disabled');
            } else {
                item.removeClass('is-disabled');
            }
        });
    };

    $('.tag-cloud-reset').click(function (e) {
        e.preventDefault();

        var selected = $('.is-selected', tagList).map(function () {
            return $.trim($(this).text());
        }).get();

        wineList.filter(function (item) {
            return true;
        });

        updateTags(wineList.items, selected);

        var heading = $('.wine-container > h2');
        heading.html(heading.data('default'));

        $('.tag-list-item', tagList).each(function () {
            var item = $(this),
                label = item.text();

            item.removeClass('is-selected');
        });

        $(this).hide();
    });

    $('.tag-list-item', tagList).click(function (e) {
        e.preventDefault();

        if ($(this).hasClass('is-disabled')) return;

        $(this).toggleClass('is-selected');

        var selected = $('.is-selected', tagList).map(function () {
            return $.trim($(this).text());
        }).get();

        var heading = $('.wine-container > h2');

        wineList.filter(function (item) {
            if (selected.length === 0) return true;
            return _.intersect(selected, _.union(item.values().CharacterNames, item.values().Type)).length === selected.length;
        });

        if (selected.length > 0) {
            var selection;

            if (selected.length > 1) {
                var lastItem = selected.pop();

                selection = ' <strong>' + selected.join(', ') + '</strong> och <strong>' + lastItem + '</strong>';
            } else {
                selection = ' <strong>' + selected.join('') + '</strong>';
            }

            heading.html(heading.data('selected') + selection);
            $('.tag-cloud-reset').show();

        } else {
            heading.html(heading.data('default'));
            $('.tag-cloud-reset').hide();
        }

        updateTags(wineList.matchingItems, selected);
    });

    var formSubmitting = false;

    //TODO: This method makes a strange/confusing 2 way submit, prevents first one but then trigger a re-click btn-send?!
    $('.btn-send').click(function (e) {
      if ($(this).hasClass('btn-disabled')) {
            e.preventDefault();
        } else {
            if (formSubmitting === false) {

              $('input[name="wine-notify"]').remove();

              _.each(selectedWines, function(wine) {
                var input = $("<input>").attr("type", "hidden").attr("name", "wine-notify").val(wine);
                $('form').append(input);
              });

              e.preventDefault();
              formSubmitting = true;

              $('.btn-send').click(); //Triggers click again on same button

            } else { //Triggered click, then show disabled button while waiting for page to reload/submit
              $(this).addClass('btn-disabled'); //To make sure user do not click twice
            }
        }
    });
});