$(document).ready( function() { campaignTool.init(); } );

var campaignTool = {

	init: function() {
		
		// Fix Cufón headers
		Cufon.replace('#campaign-package h2');
		Cufon.replace('#content h2');		
		Cufon.replace('#content h3');
		
		
		// Add some CSS3 for IE <= 8
		if( !$.support.opacity ) {
			$('body.campaign-tool #campaign-package .campaign-paper:last-child').addClass('last-child');
		}
		
	}
	
}