/*!
 * Queensbridge jQuery-plugin
 * http://queensbridge.se/
 */
(function($) {
$.fn.QBslideShow = function(options) {
	options = $.extend({
		speed: 750,
		timeout : false,
		nextBtn : false
	}, options);
 
	$(this).each(function() {
		
			var a = $(this).find('li');
			if( $(a).length > 1 ) {
				
				HTML = '<div class="nav">';
				HTML += ' <ul>';
				HTML += '  <li class="prev">Föregående</li>';
				HTML += '  <li class="page">Bild <span class="current">1</span> av <span class="total">' + a.length + '</span></li>';
				HTML += '  <li class="next">Nästa</li>';
				HTML += '</ul>';
				HTML += '</div>';
				
				$(this).append(HTML);
				
				if( $(this).hasClass('nonav') ) {
					$(this).find('div.nav').hide();
				} else {
					$(this).addClass('hasnav');
				}
				
				var animating = false;
				var me = $(this);
				var nav = $(this).find('div.nav:first ul:first');
				var images = $(this).find('ul.images:first');
				$(images).find('li:not(:first)').hide();
				$(images).find('li').css({
					'position': 'absolute',
					'top': 0,
					'left': 0
				});
				$(me).css({ 'overflow': 'visible'});
				
				$(this).find('img').each(function(i) {
					if( $(this).height() > $(me).height() ) { $(me).height( $(this).height() ); }	
				});
				
				
				$(this).find('img').load(function() {
					if( $(this).height() > $('#image-viewer').height() ) { $('#image-viewer').height( $(this).height() ); }	
				});
				
				
				// Browse
				$(nav).find('li.prev, li.next').bind('click', function(e) {			
					
					// If this was a real mouse-click we turn off the automated slideshow
					if( typeof(e.pageX) != 'undefined' ) {
						clearInterval(options.timeout);					
					}
					
					if( animating ) { return false; } else { animating = true; }
					
					var c = parseInt( $(nav).find('span.current:first').text() );
					var t = parseInt( $(nav).find('span.total:first').text() );
					
					if( $(this).hasClass('prev') ) {
						if( c == 1 ) {
							n = t-1;
						} else {
							n = c-2;
						}
					} else {
						if( c == t ) {
							n = 0;
						} else {
							n = c;
						}					
					}

					$(nav).find('span.current:first').html(n + 1);
					
					c = $(images).find('li:eq('+ ( c - 1 ) +')');
					n = $(images).find('li:eq('+n+')');
					
					$(c).css('zIndex', 10);
					$(n).css('zIndex', 5);
					
					$(n).show();
					var p = $(n).find('p');
					$(p).css({'top': '-' + ( $(p).height() + 10 ) + 'px'});
					
					$(c).fadeOut(1000, function() {
						$(n).find('p').animate({
							top: 0
						}, 500);
						animating = false;
					});
				});
				
				// Add automated scrolling of slideshow
				options.nextBtn = $(nav).find('li.next');
				options.timeout = setInterval(
					function() { 
						$(options.nextBtn).triggerHandler('click')
					}, 5000);
			}
		});
	}
})(jQuery);