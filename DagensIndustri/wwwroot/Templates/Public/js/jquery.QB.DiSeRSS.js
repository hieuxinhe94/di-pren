/*!
 * Queensbridge jQuery-plugin
 * Queensbridge.se
 */
(function($) {
	$.fn.QBDiSeRSS = function( method ) {
		var defaults = { 
			timer : false,
			totalTime : 15000, // 15000 + 1850 = 16850
			aniTime : 8425
	  };
	 	var opts = $.extend(defaults);
 
		var methods = {
			
			init : function() {
			
				//this.css('top', ( $('#image-viewer img:first').height() + 33 ) + 'px'); // 418px-385px = 33px
				
				this.find('div').show();
				
				opts.timerStep = 15;
				opts.me = '#' + this.attr('id');
				
				opts.timer = setTimeout("$('"+opts.me+"').QBDiSeRSS('switchNews')", 1000);
				this.find('div').each(function(i) {
		  		h = $(this).find('h3');
		  		$(h).data('origWidth', $(h).width()*1);
		  	});
				
				this.find('div').QBDiSeRSS('scrollText');
		
		  	this.find('div.rss-item-4, div.rss-item-5, div.rss-item-6').hide();
		  	
		  	$(this).find('div').hover(
		  		function() {
		  			clearTimeout(opts.timer);
		  			opts.timer = false;
		  			$(this).find('h3').stop();
		  		},
		  		function() {
		  			opts.timer = setTimeout("$('"+opts.me+"').QBDiSeRSS('switchNews')", 1000);
		  			if( $(this).find('h3').css('left') != 0 && $(this).find('h3').css('left') != '0px' ) {
		  				$(this).find('h3').QBDiSeRSS('startScrolling')
		  				
		  				//return methods.startScrolling.apply( this, Array.prototype.slice.call( $(this).find('h3') ) );
		  			}
		  		}
		  	).click(
		  		function() {
		  			window.open( $(this).find('a').attr('href') );
		  			return false;
		  		}
		  	);
			},
			
			switchNews : function() {
				clearTimeout(opts.timer);
				opts.timer = false;
				if( opts.timerStep > 0 ) {
					--opts.timerStep;
					opts.timer = setTimeout("$('"+opts.me+"').QBDiSeRSS('switchNews')", 1000);
					//opts.timer = this.delay(1000).QBDiSeRSS('switchNews');
					return false;
				} else { 				
					opts.timerStep = 15;
				}		
				$(this).find('div:visible').fadeOut(900);
				$(this).find('div:not(:visible)').delay(850).fadeIn(1000, function() {
					if( !$.support.opacity ) { this.style.removeAttribute('filter'); }
					if( !opts.timer ) {
						opts.timer = setTimeout("$('"+opts.me+"').QBDiSeRSS('switchNews')", 1000);
						//opts.timer = this.delay(1000).QBDiSeRSS('switchNews');
					}
					
				});
			},
			
			startScrolling : function () {
				var t = opts.aniTime;
				if( $(this).css('left') != '280px' ) {
					var l = $(this).css('left');
							l = l.substring(0, l.length - 2) * 1;
					var d = ( $(this).data('origWidth') * 1)  + l;
					var td = ( $(this).data('origWidth') * 1) + 280;
					var p = d / td;
					t = Math.ceil( t * p);
				}
				$(this).animate({left: -1 * $(this).data('origWidth')}, t, 'linear', function() {
					$(this).css('left', '280px');
					$(this).QBDiSeRSS('startScrolling');
				});			
			},

			scrollText : function() {
				$(this).each(function(i) {
					header2 = $(this).find('h3');
					headerWidth = $(header2).data('origWidth');
					widthDiff = $(header2).outerWidth() - $(this).outerWidth();
					if( widthDiff > 0 ) {
						$(header2).css('left', '280px');
						$(header2).QBDiSeRSS('startScrolling');
					}
				});
			}
		};
 

 	// Method calling logic
 	if ( methods[method] ) {
 	  return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ));
 	} else if ( typeof method === 'object' || ! method ) {
 	  return methods.init.apply( this, arguments );
 	} else {
 		return false;
 	  //$.error( 'Method ' +  method + ' does not exist on jQuery.tooltip' );
 	}   

 
	//return this.each(function(){});
	
	}
})(jQuery);

