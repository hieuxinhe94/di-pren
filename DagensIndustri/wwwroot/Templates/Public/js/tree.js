$(function () {
  Cufon.replace('.tree-item strong');
  Cufon.replace('.tree-item p', { fontFamily: 'Benton Sans DI'});

  $('.tree-item a').bind('mouseover', function (event) {
    $(this).next('.tree-item-popup').stop().fadeTo('fast', 1, function () {
      $(this).removeAttr('style').show();
    });
  });

  $('.tree-item a').bind('mouseout', function (event) {
    $(this).next('.tree-item-popup').stop().fadeTo('fast', 0, function () {
      $(this).removeAttr('style').hide();
    });
  });
});
