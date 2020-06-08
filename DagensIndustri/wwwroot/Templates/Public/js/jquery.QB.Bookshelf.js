/*!
 * Queensbridge jQuery-plugin
 * http://queensbridge.se/
 */
(function($) {
$.fn.QBbookshelf = function(options) {
	options = $.extend({
		speed: 500,
		fadeSpeed: 500,
		numOfPages: 1
	}, options);
 	
	$(this).each(function() {
		options.fadeSpeed = ( !$.support.opacity ) ? 0 : 250;
		var a = $(this).find('div.books:first');
		var b = $(a).find('li').length;
		var	rest = b % 4 ;
		if( rest != 0 )	{
			b = b + ( 4 - ( b % 4 ) );
		}
		var c = b / 4; // Num of pages, hide nav if we only have one page
		options.numOfPages = c;
		if( c == 1 ) { 
			$(this).find('ul.nav').hide(); 
		} 
		else {
			$(a).find('ul:first').width( c * 620 );
		}

		
		$(a).find('li').each(function(i) {
			if( i % 4 == 0 ) { $(this).addClass('first'); }
			else if( i % 4 == 3 ) { $(this).addClass('last'); }
		});
		
		// Activate click on image
		$(a).find('img').bind('click', function() {
			window.location = $(this).parents('li').find('h3 a').attr('href');
		});
		
		// Browse
		$(this).find('ul.nav li').bind('click', function() {
			
			if( $(this).hasClass('next') ) {
				var x = $(a).scrollLeft() + 620;
			} else {
				var x = $(a).scrollLeft() - 620;			
			}
			
			$(a).animate( { scrollLeft: x + 'px' }, options.speed);
	  	if( x >= 620 * ( options.numOfPages - 1 ) ) {
	  		$(this).parents('div.bookshelf:first').find('ul.nav li.next').fadeOut(options.fadeSpeed);
	  	} else {
	  		$(this).parents('div.bookshelf:first').find('ul.nav li.next:not(visible)').fadeIn(options.fadeSpeed);
	  	}
	  	
	  	if( x <= 0 ) {
	  		$(this).parents('div.bookshelf:first').find('ul.nav li.prev').fadeOut(options.fadeSpeed);
	  	} else {
	  		$(this).parents('div.bookshelf:first').find('ul.nav li.prev:not(visible)').fadeIn(options.fadeSpeed);
	  	}		
			
			return false;
			
		});
		$(this).find('ul.nav li.prev').hide();
		
		
		});
	}
})(jQuery);
