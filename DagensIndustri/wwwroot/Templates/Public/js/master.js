$(document).ready( function(){ q.init(); });

var q = {

	/**************************************************************/
	/*                                                            */
	/* Cache                                                      */
	/*                                                            */			
	/**************************************************************/
	
	cache : [],
	
	globals : {
		animationTime : ( !$.support.opacity ) ? 0 : 250
	},
	
	/**************************************************************/
	/*                                                            */
	/* Text-strings                                               */
	/*                                                            */			
	/**************************************************************/

	text : {
		'Close'				: 'Stäng',
		'Edit'				: 'Ändra',
		'Loading'			: 'Laddar…',
		'Required'		: 'Obligatorisk',
		'Valid'				: 'Giltig',				
		'Unvalid'			: 'Ogiltig',
		'Buy'					: 'Köp',
		'Book'				: 'Boka'
	},	
	
	init : function() {

		// Use Cufón on startpage banners
		Cufon.replace('#content div.banner h2');
		//Cufon.replace('#right div.banner-read-paper h2');		
		Cufon.replace('#footer .advertising strong.header');		
		
		// Init CSS3 support for IE6-8
		q.seletivizr();
		
		// Init nav
		q.nav.init();
		
		// Init page specifik functions
		q.pages.init();
		
		// Init payment bubble
		q.payment.init();
		
		// Init forms
		q.forms.init();		
		
		// Init expandable divs
		q.expandable.init();
		
		// Init slideshow
		$('body:not(.matrix) #image-viewer').QBslideShow();
		
		// Set global variables
		if( $('#nav > div.login').is('div') ) { q.login.init(); }
			
		// Check for iOS
		if( navigator.platform == 'iPad' || navigator.platform == 'iPhone' || navigator.platform == 'iPod' ) {
			q.iOS.init();
		}	
		
		// Add print-event to print-btns
		$('#content a.print').click(function() {
			window.print();
			return false;
		});
		
		// Set target to windows that should open in new window
		$('a.newwin').attr('target','_blank');
		
		// AJAX setup
		$.ajaxSetup ({
    	cache: false,
    	dataType: 'html',
    	complete: function() {
    		$('#loader-ajax').fadeOut(100, function() { $(this).remove(); });
    	}
		});
		
		// Init AJAX-popup
		q.popup.init();		
		
		// Fix some design stuff that CSS wont do
		q.design.init();
		
		// If we have a server-error let's scroll down to it
		if( $('#content div.server-error').is('div') ) {
			$('html, body').stop().delay(250).animate({scrollTop: $('#content div.server-error').offset().top - 30 + 'px'}, 250, 'easeOutCirc');
		}
		
		// Make banners clickable anywhere
	  $('#content .banner:not(.bookshelf)').each(function() {
	  	a = $(this).find('a:first');
	  	if( $(a).hasClass('ajax') ) {
	  		$(this).bind('click', function() {
	  			$(this).find('a:first').triggerHandler('click');
	  			return false;
	  		});
	  		$(this).css('cursor','pointer');
	  	} else if($(a).is('a') ) {
	  		$(this).bind('click', function() {
	  			window.location = $(this).find('a:first').attr('href');
	  			return false;	  			
	  		});
	  		$(this).css('cursor','pointer');	  		
	  	}	  	
	  });
		
		// RSS from di.se
		$('#di_se_rss').QBDiSeRSS();
		
		// Bookshelf
		if( $('#content div.bookshelf:first').is('div') ) {
			$('#content div.bookshelf:first').QBbookshelf();
		}

	},
		
	/**************************************************************/
	/*                                                            */
	/* Common help functions                                      */
	/*                                                            */			
	/**************************************************************/
	
	c : {
		
		isIE6 : navigator.userAgent.toLowerCase().indexOf('msie 6') != -1,
		
		getBackgroundPos : function(a) {
			var b = $(a).css('backgroundPosition').split(' ');
			return {x : parseFloat(b[0].substring(0, b[0].length-2)), y : parseFloat(b[1].substring(0, b[1].length-2)) };
		},
		
		getIdFromHref : function(a) {
			return a.split('#')[1];
		},
		
		getSelectorFromHref : function(a) {
			return '#' + a.split('#')[1];
		},
		
		getQueryString : function(a,b) {
			var a = escape( unescape(a) );
			var regex = new RegExp("[?&]" + a + "(?:=([^&]*))?","i");
			var match = regex.exec(b);
			var value = null;
			if( match != null ) { value = match[1]; }
			return value;
		},
		
		getDocumentScrollTop : function() {
			var a = $('html').scrollTop();
			if( a == 0 ) {
				a = $('body').scrollTop();
			}
			return a; 
		},
		
		getAjax : function(a, b) {
			if( q.cache[a.url] ) {
				a.success(q.cache[a.url]);
			} else {
			$.ajax({
				url: a.url,
				success: function(data) {
					a.success(data);
					q.cache[a.url] = data;
  			}
			});			
			
			}
		}
	},

	/**************************************************************/
	/*                                                            */
	/* Functions to fix position fixed on iOS                     */
	/*                                                            */			
	/**************************************************************/

	iOS : {
		
		is : false,
	
		init : function() {
			q.iOS.is = true;
		}
		
	},

	/**************************************************************/
	/*                                                            */
	/* Selectivizr                                                */
	/*                                                            */
	/**************************************************************/
	
	seletivizr : function() {
	
		if( !$.support.opacity ) {
			
			// Turn off jQuery animations since opacity animations don't work
			//jQuery.fx.off = true;
		
			// First children
			$('#nav > ul > li:first-child').addClass('first-child');
			$('#content div.form-nav ul li:first-child').addClass('first-child');
			$('#right > div.infobox .newslist li:first-child').addClass('first-child');			
			$('#content div.my-calendar table tr td:first-child').addClass('first-child');


			// Last children
			//$('#nav .subnav ul li:last-child').addClass('last-child');
			//$('#nav > ul > li:last-child').addClass('last-child');
			$('#content div.banner:last-child').addClass('last-child');
			$('#content div.form-nav ul li:last-child').addClass('last-child');
			$('#right > div.banner-conference ul > li:last-child').addClass('last-child');
			$('#content ul.previous-editions li:last-child').addClass('last-child');			
			$('#right > div.infobox .newslist li:last-child').addClass('last-child');
			$('#search-results > ul > li:last-child').addClass('last-child');
			$('#right > div.infobox .winelist li:last-child').addClass('last-child');
			$('#content div.my-calendar:first table tr td:last-child').addClass('last-child');
			
			// Other children			
			$('#content > div.product-list:first div.product:nth-child(3n)').addClass('nth-child-3');
			$('#content div.form-box > div.section:not(:first-child)').addClass('not-first-child');
			$('#content > ul.contactlist > li:nth-child(4n)').addClass('nth-child-4n');
			$('#search-results > ul.papers-inserts > li:nth-child(5n)').addClass('nth-child-5n');
			$('#search-results > ul.papers-inserts > li:nth-child(5n)').addClass('nth-child-5n');
			$('#content div.form-box div.searchlist > ul > li:even').addClass('nth-child-even');
		}	
	},

	/**************************************************************/
	/*                                                            */
	/* Navigation                                                 */
	/*                                                            */
	/**************************************************************/
	
	nav : {
		
		container : false,
		
		init : function() {
			q.nav.container = $('#nav');
			
			$('#nav:not(.gold) > ul > li:first').addClass('magazines');
			$('#nav:not(.gold) > ul > li:eq(2)').addClass('conference');			
				
			// Menu hover
			
  		$(q.nav.container).find('ul:first > li,div.account').mouseover(
	  		function() {
	  			$('#nav li.active,#nav div.active').removeClass('active');				
					$(this).addClass('active');
					$(q.nav.container).find('div.login').triggerHandler('mouseout');
	  		}
  		);
  		
  		$(q.nav.container).find('ul:first > li,div.account').mouseout(
	  		function() {
	  			$(this).removeClass('active');	
				}
  		);  		
  		
  		
  		$(q.nav.container).find('div.login').mouseover( 
  			function(e) {
  				if( !$('#header-form-login').hasClass('in-content') ) {
  					//q.login.hide(this);										
  					$(this).addClass('active');
  					$('#header-form-login').addClass('active');
  				}	
  			});
  		$(q.nav.container).find('div.login').mouseout(
  			function(e) {
  				if( !$(e.target).parents('div.login:first').is('div') ) {
  					$(this).find('input').trigger('blur');
  					$(this).removeClass('active');
  				  $('#header-form-login').removeClass('active');
  				}
  			}
  		);
  		
  		$('#header-form-login').mouseover(
  			function() {
  				if( !$(this).hasClass('in-content') ) {
  					$(this).addClass('active');
  					$(q.nav.container).find('div.login').addClass('active');
  				}
  			}
  		);
  		$('#header-form-login').mouseout(  		
  			function(e) {
  				if( !$(this).hasClass('in-content') && ( e.target == this || $(e.target).hasClass('wrapper') ) ) {
	  				$(this).find('input').trigger('blur');
  					$(this).removeClass('active');  					
  					$(q.nav.container).find('div.login').removeClass('active');
  				}
  			}
  		);
  		
  	}
		
	},

	/**************************************************************/
	/*                                                            */
	/* Expandables                                                */
	/*                                                            */
	/**************************************************************/	
	
	expandable : {
	
		init : function() {
			$('#content a.togglecontent').click(q.expandable.toggle);
		},
		
		toggle : function() {
			var a = $(q.c.getSelectorFromHref($(this).attr('href')));
			if( $(a).css('display') == 'none' ) {
				$(a).show();
			} else {
				$(a).hide();			
			}
			return false;
		}
	},
	
	/**************************************************************/
	/*                                                            */
	/* Forms                                                      */
	/*                                                            */
	/**************************************************************/
	
	forms : {
	
		init : function() {
			
			// Activate form navigations
			$('#content div.form-nav:first a').click( q.forms.switchSection );

			// Activate form edit-links			
			$('#content div.form-box:first a.edit').click(q.forms.showEdit);
			
			// Style login-button in header and add enter-key to input fields
			if( $('#header-form-login').is('div') ) {
				var loginBtn = $('#header-form-login input[type="submit"]');
				$(loginBtn).addClass('removed');
				$(loginBtn).before($('<a/>', {
					className: 'btn ' + $(loginBtn).attr('class'),
					data: {'submit' : loginBtn},
					click: q.forms.validate.all,
					innerHTML: '<span>' + $(loginBtn).val() + '</span>'
				}));
				
				// Make login-form submit on enter
				$('#header-form-login input[type="text"],#header-form-login input[type="password"]').keypress(function(e) {
					if( e.keyCode == 13 ) {
						$(this).parents('div.form:first').find('input[type="submit"]:first').trigger('click');
						//$(this).parents('div.form:first').find('input[type="submit"]:first').focus();
						return false;
					}
				});
				
			}	
			
			$('#content-wrapper input[type="submit"],#wrapper .ajax-popup input[type="submit"]').each(function(i) {
				if( !$('#content > div.button-wrapper a.btn').is('a') ) {
					$(this).before($('<a/>', {
						className: 'btn ' + $(this).attr('class'),
						data: {'submit' : this},
						click: q.forms.validate.all,
						innerHTML: '<span>' + $(this).val() + '</span>'
					}));
				}
				$(this).addClass('removed');				
			});
			
			$('#content > div.button-wrapper a.btn').live('click', q.forms.validate.all);
			q.forms.initFields();
			
			// Activate Date-picker
			if( $('#content input.date').is('input') ) {
				$('#content input.date').datepicker({ dateFormat: 'yy-mm-dd', showOn: 'both', buttonImage: 'images/elements/btn-calendar.png', buttonImageOnly: false });
				$('#content input.date[min]').each(function() {
					$(this).datepicker( "option", "minDate", $(this).attr('min') );
				});
				$('#content input.date[max]').each(function() {
					$(this).datepicker( "option", "maxDate", $(this).attr('max') );
				});				
			}
			
			if( $('#content div.input.date-inline').is('div') ) {
				var defDate = new Date();
				var dateVal = $('#input-datepicker').val();
						dateVal = dateVal.split('-');
						defDate.setFullYear(
							parseInt(dateVal[0]),
							parseInt(dateVal[1]) - 1,
							parseInt(dateVal[2])							
						);
				$('#content div.input.date-inline').datepicker({
					altFormat: 'yy-mm-dd', 
					altField: '#input-datepicker', 
					maxDate: new Date(),
					defaultDate: defDate,
					onSelect: function() {
						$(this).siblings('input[type=submit]').trigger('click');
					}
				});
			}			
			
			// Check for the Golden popup and make sure the btn is disabled if the cb ain't checked
			/*
			if( $('#membership-required').is('div') ) {
				if( !$('#membership-required input[type="checkbox"]').is(':checked') ) {
					$('#membership-required .btn').addClass('disabled');
				}
				$('#membership-required input[type="checkbox"]').click(function() {
					if( $(this).is(':checked') ) {
						$('#membership-required .btn').removeClass('disabled');
						$('#membership-required input[type="submit"]').removeAttr('disabled');
					} else {
						$('#membership-required .btn').addClass('disabled');
						$('#membership-required input[type="submit"]').attr('disabled', 'disabled');
					}
				});
			}
			*/
		},
		
		initFields : function() {
			//$('#wrapper div.form-box input[required=required],#header-form-login input[required=required]').each(function(i) {
			$('#wrapper div.form-box input[required]:not([pattern])').attr('pattern', '(^.+$)');
			$('#wrapper div.form-box input[pattern],#header-form-login input[required]').each(function(i) {
				if( $(this).attr('required') ) {
					$(this).after('<span class="status required input-' + $(this).attr('type') + '">' + q.text['Required'] + '</span>');
				} else {
					$(this).after('<span class="status optional input-' + $(this).attr('type') + '">' + q.text['Required'] + '</span>');
				}
				$(this).data('errorText', $(this).attr('title'));
				$(this).removeAttr('title');
				if( $(this).hasClass('date') ) {
					$(this).bind('change', q.forms.validate.one);
				} else {
					$(this).one('blur', q.forms.validate.one);
				}
				
				// Set RegEx for field
				if( $(this).attr('pattern') ) {
					var regEx = new RegExp( $(this).attr('pattern') );
				} else {
					var regEx = new RegExp(/\S/);
				}				
				$(this).data('pattern', regEx);
				$(this).focus( q.forms.validate.hideErrorTexts );
				if( $(this).is('input[type=date]') ) {
					$(this).change( q.forms.validate.one );
				}
			});
		},
		
		validate : {
			
			one : function(e) {
		  	if( e.type == 'blur' ) {
		  		if( $(this).val() == '' ) {
						$(this).one('blur', q.forms.validate.one);
						if( $(this).parents('#header-form-login').is('div') ) {
							return true;
						} else {
							return false; 
						}
		  		} else {
		  			$(this).keyup(q.forms.validate.one);
		  		}
		  	}			
			
				if( $(this).val().search( $(this).data('pattern') ) == -1 ) {
					$(this).data('valid', false);
				} else {
					$(this).data('valid', true);				
				}

				// Update status
				if( !$(this).data('valid') ) {
		  		$(this).siblings('span.status:not(.unvalid)').addClass('unvalid');			
		  	} else {
		  		$(this).siblings('span.status:not(.valid)').addClass('valid');
		  		$(this).siblings('span.status.unvalid').removeClass('unvalid');
		  	}
				
			},
			
			all : function() {
				if( $(this).hasClass('disabled') ) { return false; }
				q.forms.validate.hideErrorTexts();
				$('body').bind('click', q.forms.validate.hideErrorTexts);
				var formBox = $('#content .form-box');
				if( $(this).parents('div.section.form-box:first').is('div') ) {
					formBox = $(this).parents('div.section.form-box:first');
				} else if( $(this).parents('#header-form-login').is('div') ) {
					formBox = $(this).parents('#header-form-login');
				}
				var flds = $(formBox).find('input[pattern]');
				var valid = true;	
				$(flds).each(function(i) {
					if( $(this).is(':visible') && ( $(this).attr('required') || $(this).val() != '' ) ) {
						if( $(this).attr('pattern') ) {
							var regEx = new RegExp( $(this).attr('pattern') );
						} else {
							var regEx = new RegExp(/\S/);
						}
						
						if( ( $(this).not('[type="checkbox"]') && $(this).val().search( regEx ) == -1 ) || ( $(this).is('[type="checkbox"]') && !$(this).is(':checked') ) ) {
							$(this).data('valid', false);
							$(this).siblings('span.status:not(.unvalid)').addClass('unvalid');
							q.forms.validate.showErrorText(this);
							valid = false;
						} else {
							$(this).data('valid', true);
				  		$(this).siblings('span.status:not(.valid)').addClass('valid');
				  		$(this).siblings('span.status.unvalid').removeClass('unvalid');			
						}	
					}				
				});
				
				if( valid ) {
					if( typeof( $(this).data('submit') ) != 'undefined' ) {
						$( $(this).data('submit') ).trigger('click');
						return false;
                    } else {
						$('#content .form-box input[type=submit]').trigger('click');
						return false;
					}
				} else {
					
					if( $('#wrapper div.form-error:first').offset().top < q.c.getDocumentScrollTop() ) {
						$('html, body').stop().animate({scrollTop: ( $('#wrapper div.form-error:first').offset().top - 50 ) + 'px'}, 250, 'easeOutCirc');
					}
					return false;					
				}
				
			},
			
			showErrorText : function(obj) {
				$(obj).parent().find('div.form-error').remove();
				$(obj).siblings('.status:first').append('<div class="form-error"><p>' + $(obj).data('errorText') + '</p><div class="right-side"></div><div class="left-side"></div></div>');
				var e = $(obj).parent().find('div.form-error');
				$(e).css({
					left: -( $(e).width() / 2 ) + 'px'
				});
			},
			
			hideErrorTexts : function() {
				$('#wrapper div.form-error').remove();
				$('body').unbind('click', q.forms.validate.hideErrorTexts);				
			}
		},
		
		showEdit : function( obj ) {
			if( typeof(obj) == 'string' ) {
				obj = $(obj).prevAll('a.edit:first');
			} else {
				obj = this;
			}
			var f = $(obj).siblings('div.form-edit:first');
			if( $(f).is(':visible') ) {
				$(obj).text(q.text['Edit']);
				$(f).hide(0, function() {
					$(obj).siblings('p.description:first, p.value:first').show();
				});			
			} else {
				$(obj).text(q.text['Close']);
				$('#content div.form-box div.form-edit:visible').siblings('p.description:first, p.value:first').show();
				$('#content div.form-box div.form-edit:visible').siblings('a.edit').text(q.text['Edit']);
				$('#content div.form-box div.form-edit:visible').hide();
				$(f).siblings('p.description:first, p.value:first').hide();
				$(f).show();
			}
			return false;
		},
		
		switchSection : function(obj) {
			if( typeof(obj) == 'string' ) {
				obj = $(obj).parents('div.form-box:first').prev('div.form-nav:first').find('a[href$="'+ obj +'"]');
			} else {
				obj = this;			
			}
			$(obj).parents('div.form-nav:first li.current').removeClass('current');
			$(obj).parent().addClass('current');
			var formBox = $(obj).parents('div.form-nav:first').next('div.form-box');				
			var url = $(obj).attr('href');							
			if( url.indexOf('#') != -1 ) {
				$(formBox).find('.section:visible').hide();
				$( q.c.getSelectorFromHref(url) ).show();			
			} else {
				$(formBox).load( url, q.forms.initFields);
			}
			
			return false;
		}
	},

	/**************************************************************/
	/*                                                            */
	/* Payment specifik functions                                 */
	/*                                                            */			
	/**************************************************************/

	popup : {
		
		init : function() {
			$('body a.ajax').click( q.popup.open );
			
			// Add closing functionality via LIVE
			$('.ajax-popup a.btn-close,.ajax-popup a.cancel,#content .form-box.login .request-form a.cancel').live('click', q.popup.close );
			
		},
		
		open : function() {
			
			var URL = $(this).attr('href');			
			
			if( URL.indexOf('#') == - 1 ) {
				
				q.popup.showOverlay();

				if( $(this).hasClass('iframe') ) {

					// iFrame popup
					q.popup.appendiFrame( $(this).attr('href') );
					
					
				} else {

					// Real AJAX
					$.ajax({
		  			url: URL,
			  		data: { ajax: true },
			  		success: q.popup.appendAJAX
			  	});				
				}
		  	
		  } else {
				
				var obj = q.c.getSelectorFromHref(URL);
				
				// Exceptions
				if( !$(obj).hasClass('no-popup') ) {
					q.popup.showOverlay();
				}
				
		  	// Fake AJAX
		  	$( obj ).fadeIn(q.globals.animationTime);
		  	
		  	// If iOS or IE6 position:fixed won't work
		  	if( q.iOS.is || q.c.isIE6 ) {
		  	  $( q.c.getSelectorFromHref(URL) ).css({
		  	  	position: 'absolute',
		  	  	top: ( $('html').scrollTop() + $('body').scrollTop() + 100 ) + 'px'
		  	  });
		  	}		  	
		  			
		  }
			return false;
		},
		
		appendiFrame : function(URL) {
			
			var w = 500;
			var h = 350;
			var content = 'unknown';	
			
			if( URL.indexOf('player.vimeo.com') != -1 ) {
				w = 640; h = 360; content = 'movie';
			}
			
			var	HTML = '<div class="wrapper ' + content + '"><div class="content">';
					HTML+= ' <iframe src="' + URL + '" width="' + w + '" height="' + h + '" frameborder="0"></iframe>';
					HTML+= ' <a href="#" class="btn-close">Stäng</a>';
					HTML+= '</div></div>';
			
			$('#wrapper').append($('<div />', {
				id : 'ajax-popup-iframe',
				css : {display:'block'},
				className : 'ajax-popup real-ajax'
			}).append(HTML).hide().fadeIn(q.globals.animationTime));
			$('#ajax-popup-iframe > .wrapper').width(w);
			
		},
		
		appendAJAX : function(data) {
			$('#wrapper').append($('<div />'	, {
				id : 'ajax-popup',
				className : 'ajax-popup real-ajax'
			}).hide().fadeIn(q.globals.animationTime));
		  			
			// If iOS position:fixed won't work
			if( q.iOS.is ) { $('#ajax-popup').css({ position: 'absolute', top: ( $('html').scrollTop() + $('body').scrollTop() + 200 ) + 'px' }); }
		  			
			$('#ajax-popup').html(data);
		},
		
		close : function() {
			var obj = $('#wrapper div.ajax-popup:visible');		
			if( $('#wrapper div.no-popup:visible').is('div') ) {
				obj = $('#wrapper div.no-popup:visible');
			}
			$(obj).fadeOut(q.globals.animationTime, function() {
				if( $(this).hasClass('real-ajax') ) { $(this).remove(); }
			});						
			q.popup.hideOverlay();
			$(window).unbind('keyup', q.popup.closeByKey);
			
			return false;
		},
		
		closeByKey : function(e) {
			if( e.keyCode == 27 ) { q.popup.close(); }
		},
		
		showOverlay : function() {
			$('#wrapper').append($('<div />', {
				id : 'overlay',
				click : q.popup.close,
				css : { height: $('#wrapper').height() + 'px'}
			}).hide().fadeTo(q.globals.animationTime, 0.8));
			$(window).bind('keyup', q.popup.closeByKey);
		},
		
		hideOverlay : function() {		
			$('#overlay').fadeOut(q.globals.animationTime, function() {
				$(this).remove();
			});
		}
		
	},

	/**************************************************************/
	/*                                                            */
	/* Payment specifik functions                                 */
	/*                                                            */			
	/**************************************************************/
	
	
	payment : {
		
		init : function() {
			
			//$('#shop-top div.paymentoptions').hide();
			//$('#shop-top div.payment a.btn').click( q.payment.showBubble );
			
			var paymentBubbles = $('#shop-top div.payment');
			$('#shop-top div.paymentoptions').each(function(i) {
				$(this).hide();
				$(this).next('a.btn').click( q.payment.showBubble );
				$(this).next('a.btn').data('bubble', this);
			});			
			
			/*
			var paymentBubbles = $('#shop-top div.payment');
			$('#shop-top div.paymentoptions').each(function(i) {
				$(this).hide();
				$(this).after(
					$('<a/>', {
			  		href: '#',
				  	data: {'bubble': this},
				  	className: 'btn',
				  	click : q.payment.showBubble
				  }).append('<span>' + q.text["Buy"] + '</span>')
				);
			});
			*/
		},
		
		showBubble : function() {		
			var a = $(this).data('bubble');
			if( $(a).is(':visible') ) {
				q.payment.hideBubble();
				return false;
			} else {
				$(a).fadeIn( q.globals.animationTime, function() {
					$('body').bind('click', q.payment.hideBubble);				
				});
				return false;				
			}
		},
		
		hideBubble : function() {
			$('#wrapper div.paymentoptions').fadeOut( q.globals.animationTime );
			$('body').unbind('click', q.payment.hideBubble);			
		}
	},

	/**************************************************************/
	/*                                                            */
	/* Google maps via gMap.js                                  	*/
	/*                                                            */			
	/**************************************************************/
	
	map : {
		
		zoom: 15,
		icon: {
			image: '/templates/public/images/elements/map-marker.png',
			iconsize: [53,67],
			iconanchor: [26,63]
		}, 
		
		set : function( selector, address ) {
			$(selector).gMap({
				address: address,
				zoom: q.map.zoom,
				markers: [
					{ address: address }
				],
				icon: q.map.icon
			});
			$(selector).after('<a href="http://maps.google.com/maps?f=q&source=s_q&hl=sv&geocode=&q='+ address + '" target="gmap" class="more">Visa på Google Maps</a>'); 
		}
	},
	
	/**************************************************************/
	/*                                                            */
	/* Design specifik functions                                  */
	/*                                                            */			
	/**************************************************************/
	
	design : {
		
		init: function() {
			
			$('#right .infobox .content > a.btn').each(function(i) {
				$(this).wrap('<div class="button-wrapper" />');				
				$(this).css({
					'width': $(this).width(),
					'float': 'none'
				});
			});
			
			// Fix size of caption text box
			$('#content div.caption p.caption-text').each(function(i) {
				$(this).width( $(this).siblings('img').width() );
			});
			
			if( q.c.isIE6 ) { q.design.ie6() }
			
		},
		
		ie6 : function() {
			
			// Fix PNG's
			$('body img[src$=".png"]').each(function(i) {
				$(this).hide();
				$(this).after(
					$('<div />',{
						className: $(this).attr('class') + ' img',
						css: {
							width: $(this).outerWidth() + 'px',
							height: $(this).outerWidth() + 'px',
							filter: 'progid:DXImageTransform.Microsoft.AlphaImageLoader(src="' + $(this).attr("src") + '")'
						}
					})
				);
			});				
		
		}
	},

	/**************************************************************/
	/*                                                            */
	/* Login specifik functions                                   */
	/*                                                            */			
	/**************************************************************/		
	
	login : {
		
		offsetChords : {
			'default' : { top: 0, left: -17 },
			'banner-read-paper' : { top: 495, left: 660 }			
		},
		
		init : function() {
			$('#content-wrapper a.login-required').click( q.login.show );
		},
		
		show : function() {
			if( $(this).hasClass('login-bubble') ) {
				// If I am open - hide me
			  q.login.hide(this);
			} else {
			
				// Remove old one if exists
			  $('#content-wrapper .login-bubble').removeClass('login-bubble');
			  $('#login-popup').remove();
	
				// Which offset should we use
				var o = q.login.offsetChords['default'];
				if( $(this).parents('div.banner-read-paper').is('div') ) {
					o = q.login.offsetChords['banner-read-paper'];
				}
			  
			  // Get the postion
			  var p = $(this).position();
			  var pt = ( p.top - 215 ) + o.top ;
			  var pl = p.left + o.left;
				

			  var	aniTime = 250;			  
			  if( !$.support.opacity ) {
			  	aniTime = 0;
			  }
			  
			  // Create the bubble
			  $('#wrapper').append(
			  	$('<div/>', {
			  		id: 'login-popup',
			  		css: {
			  			top: pt + 'px',
			  			left: pl + 'px'
			  		}
			  	}).append('<div class="wrapper" />').fadeIn(aniTime)
			  );
			  
			  // Move the login-form to the bubble			  
			  $('#header-form-login').addClass('in-content');
			  $('#header-form-login').hide().css(
			  	{
			  		top: pt + 'px',
			  		left: ( pl - 870 ) + 'px'
			  	}
			  ).fadeIn(aniTime);
			  $(this).addClass('login-bubble');
			  $('#login-landing_page').val($(this).find('a').attr('href'));
			  			  
			  $('body').bind('click', q.login.hide);		
			}
			return false;		
		},
		
		hide : function(e) {
			if( !$(e.target).parents('#header-form-login:first').is('div') && !$(e.target).parents('#login-popup:first').is('div') ) {
			  $('#header-form-login').removeClass('in-content');
			  $('#header-form-login').css({
			  	top: '', left: ''
			  });
			  $('#header-form-login').css('display', '');
			  $('#content-wrapper .login-bubble').removeClass('login-bubble');
			  $('#login-popup').remove();
			  $('#login-landing_page').val('');
			  $('body').unbind('click', q.login.hide);
			}
		}
	
	},
	
	/**************************************************************/
	/*                                                            */
	/* Page specifik functions                                    */
	/*                                                            */			
	/**************************************************************/	
	
	pages : {
		
		init : function() {
			
			if( $('body').hasClass('matrix') ) {
				q.pages.matrix.init();
			}

			if( $('body').hasClass('bizbook') ) {
				q.pages.bizbook();
			}	
			
			if( $('#page-preferences').is('div') ) {
				q.pages.preferences.init();
			}
			
		},
		
		/**************************************************************/
		/*                                                            */
		/* The matrix for subscription                                */
		/*                                                            */			
		/**************************************************************/
		
		preferences : {
			
			init : function() {
				$('#page-preferences select[name=previousadresses]').change(q.pages.preferences.updateAddress);
			},
			
			updateAddress : function() {
				var address = $(this).find('option:selected').val();
				var p = $(this).parents('div.form-edit:first');				
				if( address == '' ) {
					// New address -> empty fields (not dates)
					$(p).find('input:not(.date)').val('');
				} else {
					// Use old address -> populate fields (not dates)
					address = eval(address);
					var flds = $(p).find('input:not(.date)')
					$(flds).each(function(i) {
						$(this).val( address[i] );
					});
				}
			}
		},		
		
		/**************************************************************/
		/*                                                            */
		/* BizBook                                                    */
		/*                                                            */			
		/**************************************************************/		
		
		bizbook : function() {
			
			$("#company-members ul li h3 a").click(function() {
				$(this).parents('li:first').addClass('ajax-loader');
			});
			
			$("#content .section h2").click(function(e) {
				$(this).parent().toggleClass("open");
				return false;
			});
			
			$("#show-more-result-balance").click(function() {
				$(this).remove();
				$("#more-result-balance").toggleClass("hidden");
				return false;
			});
			
			// Add even class to tables 
			$("table tr:nth-child(even)").addClass("even");
			
			// Scroll to selected member
			var member_el = $("#company-members ul>li.open");
			if (member_el.is("*") != false) {
				member_el.get(0).scrollIntoView();
			}
		},
		
		/**************************************************************/
		/*                                                            */
		/* The matrix for subscription                                */
		/*                                                            */			
		/**************************************************************/
		
		matrix : {
			
			me : false,
			timer : false,
			
			init : function() {
				
				q.pages.matrix.me = $('#content-wrapper table')
				
				$(q.pages.matrix.me).find('th:not(.col_1),td:not(.col_1):not(.col_all)').each(function() {
					if( q.iOS.is ) {
						$(this).click(function() {
							q.pages.matrix.change( $(this).attr('class') );
						});
					}	else {
						$(this).mouseover(q.pages.matrix.preChange);
						$(this).mouseout(q.pages.matrix.clearTimer);					
					}
				});					
				
				$(q.pages.matrix.me).find('input[type=checkbox]').click(q.pages.matrix.cb);
				
			},
			
			cb : function() {
				var p = $(this).parents('td:first,th:first');
				var colClass = $(p).attr('class');
				$(q.pages.matrix.me).find('input[type=checkbox]').attr('checked', $(this).attr('checked'));
				q.pages.matrix.changeImage(colClass);
				
			},
			
			preChange : function() {
				q.pages.matrix.clearTimer();
				q.pages.matrix.timer = setTimeout("q.pages.matrix.change('" + $(this).attr('class') + "')", 250);
			},
			
			clearTimer : function() {
				clearTimeout(q.pages.matrix.timer);
				q.pages.matrix.timer = false;			
			},
			
			change : function(colClass) {
				var active = $(q.pages.matrix.me).find('div.wrapper.active:first');
				if( !$(active).parent().hasClass(colClass) ) {
				  $(q.pages.matrix.me).find('div.wrapper.active').removeClass('active');
				  $(q.pages.matrix.me).find('div.wrapper.afterActive').removeClass('afterActive');
				  
				  var newActive = $(q.pages.matrix.me).find('thead th.' + colClass + ' div.wrapper,tfoot td.' + colClass + ' div.wrapper');
				  $(newActive).addClass('active');
				  $(q.pages.matrix.me).find('thead th.' + colClass).next('th').find('div.wrapper').addClass('afterActive');
				  
				  // Change top image
				  q.pages.matrix.changeImage(colClass);
				  
				}			
			},
			
			changeImage : function(colClass) {
			  var gold = $(q.pages.matrix.me).find('thead th.' + colClass + ' input[type=checkbox]:checked').is('input');
			  if( gold ) {
					var nextImg = $('#image-viewer').find('li.' + colClass + '.gold'); 
			  } else {
				  var nextImg = $('#image-viewer').find('li.' + colClass + ':not(.gold)'); 
			  }
			 	
			 	var prevImg = $('#image-viewer').find('li.active');
				
			 	$(prevImg).css({ zIndex: 4})
			 	$(nextImg).hide();
		    $(nextImg).css({ zIndex: 5});
		    $(nextImg).fadeIn(500, function() {
		    	
		    	$('#image-viewer li.active').hide().removeClass('active');
		    	$(this).addClass('active')
		    	
		   });			
			
			}
		}
	}
}