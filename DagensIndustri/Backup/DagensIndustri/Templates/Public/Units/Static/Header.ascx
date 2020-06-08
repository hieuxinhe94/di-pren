<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="DagensIndustri.Templates.Public.Units.Static.Header" %>

<title runat="server">
    <asp:Literal ID="PageTitleLiteral" runat="server" />
</title>

<meta name="google-translate-customization" content="7660c05ddb7537f7-337b6759ceb9c498-g60ccd5d6edca6eda-11" />

<asp:PlaceHolder ID="MetaDataAreaPlaceHolder" runat="server" />
<asp:PlaceHolder ID="SimpleAddressPlaceHolder" runat="server" />

<link media="all" rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Templates/Public/Styles/reset.css")%>" />
<link media="all" rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Templates/Public/Styles/shared.css")%>" />
<link media="all" rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Templates/Public/Styles/bizbook.css")%>" />
<link media="all" rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Templates/Public/Styles/jqueryui/jquery-ui-1.8.9.custom.css")%>" />
<%--<link media="all" rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Templates/Public/Styles/print.css")%>" /> TODO: Nazgol när ska print.css läggas till?--%>
<link media="all" rel="stylesheet" type="text/css" href='<%= cssUrl %>' />	

<asp:Literal ID="CampaignCssLiteral" runat="server" />
<asp:Literal ID="ModsLiteral" runat="server" />

<link media="all" rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Templates/Public/Styles/mods.css")%>" />

<!--[if lte IE 8]>
<link href="<%=ResolveUrl("~/Templates/Public/Styles/ie.css")%>" media="all" rel="stylesheet" type="text/css" />	
<![endif]-->
	
<!--[if lte IE 7]>
<link href="<%=ResolveUrl("~/Templates/Public/Styles/ie7.css")%>" media="all" rel="stylesheet" type="text/css" />	
<![endif]-->
	
<!--[if lte IE 6]>
<link href="<%=ResolveUrl("~/Templates/Public/Styles/ie6.css")%>" media="all" rel="stylesheet" type="text/css" />	
<![endif]-->

<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/DI.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/Functions.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery-1.5.1.min.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/cufon-yui.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/BentonCompBlack_900.font.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery-ui-1.8.9.custom.min.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery.QB.DiSeRSS.js")%>"></script>	
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery.QB.SlideShow.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery.tools.min.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery.ui.datepicker-sv.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/master.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/swfobject.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery.QB.Bookshelf.js")%>"></script>
<%--<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/DiGuldCalendar.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/DiGuldCalendarData.js")%>"></script>--%>
<%--<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery.gmap-1.1.0-min.js")%>"></script>--%>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/underscore-min.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery.jqEasyCharCounter.min.js")%>"></script>
<%--<script src=" http://maps.google.com/maps?file=api&amp;amp;v=2&amp;sensor=true&amp;key=ABQIAAAArWnBSdNA2jqPuJO1ESOq-BRWN3Qt2iNGUYt-5uhC6uL6YjxK4hTfS-jMpoLnCdGcbQF6H8hD58sq6A" type="text/javascript"></script>--%>
<%--<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/campaign-tool.js")%>"></script>--%>

<script type="text/javascript">
  var strKey = "";

  switch (window.location.host) {
  case 'www.dagensindustri.se':
    strKey = 'ABQIAAAArWnBSdNA2jqPuJO1ESOq-BRWN3Qt2iNGUYt-5uhC6uL6YjxK4hTfS-jMpoLnCdGcbQF6H8hD58sq6A';
    break;
  case 'dagensindustri.se':
    strKey = 'ABQIAAAArWnBSdNA2jqPuJO1ESOq-BRWN3Qt2iNGUYt-5uhC6uL6YjxK4hTfS-jMpoLnCdGcbQF6H8hD58sq6A';
    break;
  case 'test2.dagensindustri.episerverhosting.com':
    strKey = 'ABQIAAAArWnBSdNA2jqPuJO1ESOq-BSBgy-ahb3O0icQNvHQ4NBjgHfViRQfmuc5aeDM88Dtr7eJ5_8nyi_OZg';
    break;
  case 'tidningen.di.se':
    strKey = 'ABQIAAAArWnBSdNA2jqPuJO1ESOq-BRpFshY4tQNUUEWT0j9Xvi-LLTj-xQ4gWJYMziZFn4stfgsZx_QKCVdiA';
    break;

  }

  document.write('<' + 'script src="http://maps.google.com/maps?file=api&v=2&key='
    + strKey + '" type="text/javascript">' + '<' + '/script>');
</script>


<%--<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery-1.4.4.min.js")%>"></script> 
<script type="text/javascript" src="<%=ResolveUrl("~/Templates/Public/js/jquery-1.5.min.js")%>"></script> Behöver inte användas längre enligt Queensbridge--%>


<script type="text/javascript" src="http://m.burt.io/d/dagensindustri-se.js"></script>
<script>
  var byburt_fakeVariable = <%=ConfigurationManager.AppSettings["BurtFakeVariable"]%>;
  var byburt_customerNumber = <%=Cusno%>;

  window.byburt_segments = (function() {
    var matches = document.cookie.match(/burtSegments=([^;]+)/);
    if(matches && matches.length === 2) {
        return matches[1].split(',');
    }
    return [];
})();

window.burtApi = window.burtApi || [];
window.burtApi.push(function() {
    window.burtApi.startTracking(function (api) {
        
        api.setTrackingKey('DAGFNSMUPDNQ', 'dagensindustri.se');
        api.setDomain('dagensindustri.se');
        api.addCloudKey('INS8GFMRVMI1');

        api.setDemographicsProvider('cint');

        api.withUserSegments(function(segments) {
            var date = new Date();
            date.setTime(+date + 1 * 864e+5);
            document.cookie = 'burtSegments=' + segments.join(',') + '; expires=' + date.toUTCString() + '; path=/';
        });

        var authPostString = '';

        if(window.byburt_customerNumber && !window.byburt_fakeVariable) {
            authPostString = '-authenticated';
            api.connect('burt.beacon', 'di-customer-number', window.byburt_customerNumber);
        }

        if(document.location.pathname === '/default.aspx') {
            api.setCategory('frontpage'+authPostString);
        }
        else {
            var category = (function() {
                var parts = window.location.pathname.split('/');
                if (parts.length >= 2) {
                  if (parts[1] === '') {
                    return 'frontpage';
                  } else {
                    return parts[1];
                  }
                }
            })();
            api.setCategory(category+authPostString);
        }

        api.annotate('burt.content', 'tags', ['BI_Business']);
    	api.startUnitTracking();

    });
});
</script>