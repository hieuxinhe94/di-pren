$(document).ready(function() { goldCalendar.loadFiles(); });

var goldCalendar = {
	
	// File URL's
	files : {
		addPost : '',
		data : '/Templates/Public/js/DiGuldCalendarData.js'		
	}, 
	companies : {},
	categories : {},	
	searchField : false,
	searchList : false,
	searchContainer : false,
	noResults : false,
	timer: false,	
	
	// Load the list of companies from the JSON
	loadFiles : function() {
		$.ajax({
			url: goldCalendar.files.data,
			dataType: 'json',
			success: function(data) {
				goldCalendar.companies = data.companies;
				goldCalendar.categories = data.categories;
				goldCalendar.init();
			}
		});
	},
	
	// Set the handler for the searchfield and define tile searchlist container	
	init : function() {
		goldCalendar.searchField = $('#calendar-ordinary input:first');
		$(goldCalendar.searchField).keyup( goldCalendar.preSearch );		
		goldCalendar.searchListContainer = $('#calendar-ordinary div.searchlist:first');
		goldCalendar.noResults = $('#calendar-ordinary div.noresults:first');		
		goldCalendar.searchList = $(goldCalendar.searchListContainer).find('> ul');
		$(goldCalendar.searchList).find('a.more').live('click', goldCalendar.addCompany);
		$(goldCalendar.searchList).find('input[type="checkbox"]').live('click', goldCalendar.checkForOne);		
		$(goldCalendar.searchListContainer).hide();
		$('#my-calendar input.text.large').bind('focus mouseup', function() { this.select(); return false; });
		//$('#my-calendar input.text.large').bind('keydown', function() { return false; });
		
		$('#btn-showurl').click(function() { $('#input-url').show(); return false; });
		
	},
	
	preSearch : function(e) { 
		clearTimeout(goldCalendar.timer);
		goldCalendar.timer = false;			
		goldCalendar.timer = setTimeout( 'goldCalendar.search()', 40);		
	},
	
	// Search the object of companies for the user input
	search : function() {
		var val = $(goldCalendar.searchField).val();
		if( val != '' ) {
			var searchPar = new RegExp('^' + val, "i");
			var results = _.select(goldCalendar.companies, function(company) {
				if( company.name.match( searchPar ) ) {
					return true;
				}
			});		
		} else {
			results = false;
		}
		goldCalendar.populateList(results);
	},
	
	// Update the search result
	populateList : function(results) {
	
		// Remove the old data
		$(goldCalendar.searchList).empty();
			
		// Check if we have data else display no result
		if( results.length > 0 ) {
			
			// Add the new
			var HTML = '';
			$.each(results, function(i) {
				HTML += '<li>';
				HTML += '<h5>' + this.name + '';
				HTML += '<a href="#" class="more add">Lägg till</a>';
				HTML += '</h5>';				
				HTML += goldCalendar.buildCBs(this.id);
				HTML += '<input type="hidden" name="id" value="' + this.id + '" />';
				HTML += '</li>';			
			});
			$(goldCalendar.searchList).append(HTML);
	
			// Show results if hidden
			if(	$(goldCalendar.searchListContainer).css('display') == 'none' ) {
				$(goldCalendar.searchListContainer).show();
				$(goldCalendar.noResults).hide();				
			}
			
		} else if( !results ) {
			$(goldCalendar.searchListContainer).hide();
			$(goldCalendar.noResults).hide();			
		} else {
			$(goldCalendar.searchListContainer).hide();
			$(goldCalendar.noResults).show();
		}	
	},
	
	buildCBs : function(id) {
		var companyId = id;
		var HTML = '';
		$.each(goldCalendar.categories, function(i) {
			catId = this.id;
			checked = ( this.checked ) ? 'checked="checked"' : '';
			//disabled = ( this.disabled ) ? 'disabled="disabled"' : '';		
			HTML += '<span class="checkbox">';
			HTML += '<input type="checkbox" name="cat_' + catId + '" id="c_ordinary_' + companyId + '_' + catId + '" ' + checked + ' />';
			HTML += '<label for="c_ordinary_' + companyId + '_' + catId + '">' + this.name + '</label>';
			HTML += '</span>';
		});
		return HTML;
	},
	
	addCompany : function() {
		var cbs = $(this).parents('li:first').find('input[type=checkbox]:checked');
		var data = '';
		$(cbs).each(function() {
			data+= $(this).attr('name') + '=on&';
		});
		//data+= 'id=' + $(this).parents('li:first').find('input[name=id]').val();
        data+= 'insert=1&compId=' + $(this).parents('li:first').find('input[name=id]').val();
		window.location.href = goldCalendar.files.addPost + '?' + data;
		return false;		
	},
	
	checkForOne : function() {
		var cbs = $(this).parents('li').find('input[type="checkbox"]');
		var checked = $(cbs).filter(':checked');
		var disabled = $(cbs).filter(':disabled');
		if( checked.length == 1 ) {
			$(checked).attr('disabled', 'disabled');
		} else {
			$(disabled)	.removeAttr('disabled');
		}
	}
}