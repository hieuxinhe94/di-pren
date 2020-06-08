<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CampaignHeader.ascx.cs" Inherits="PrenDiSe.Templates.Public.Units.CampaignTemplates.CampaignHeader" %>

<asp:PlaceHolder ID="MetaDataAreaPlaceHolder" runat="server" />
<asp:PlaceHolder ID="SimpleAddressPlaceHolder" runat="server" />

<meta content="width=device-width, initial-scale=1.0" name="viewport" />
<meta property="og:image" content="<%= CurrentPage["FaceBookImage"] != null ? EPiServer.Configuration.Settings.Instance.SiteUrl.ToString() + CurrentPage["FaceBookImage"] :  EPiServer.Configuration.Settings.Instance.SiteUrl.ToString() + CurrentPage["Campaign1TopImage"]%>" />
<meta property="og:title" content="<%= CurrentPage["FaceBookTitle"] != null ? CurrentPage["FaceBookTitle"] : CurrentPage.PageName  %>" />
<meta property="og:description" content="<%= CurrentPage["FaceBookDescription"] != null ? CurrentPage["FaceBookDescription"] : CurrentPage.PageName  %>" />

<link href="/bootstrapDi/css/bootstrap.min.css" rel="stylesheet" media="screen" />
<link href="/bootstrapDi/css/bootstrap-responsive.min.css" rel="stylesheet" media="screen" />
<link href="/bootstrapDi/css/shared.css?v=2" rel="stylesheet" media="screen" />
<link href="/bootstrapDi/css/datepicker.css?v=1" rel="stylesheet" media="screen" />
<% if (CurrentPage["CampaignTheme"]!=null && CurrentPage["CampaignTheme"].ToString().ToLower() == "campthemeagenda")
   { %>
   <link href="/bootstrapDi/css/agenda.css?v=1" rel="stylesheet" media="screen" />
<% } %>

<script type="text/javascript" src="/bootstrapDi/js/jquery.1.9.1.js"></script>
<script type="text/javascript" src="/bootstrapDi/js/bootstrap.min.js"></script>
<script type="text/javascript" src="/bootstrapDi/js/bootstrap-datepicker.js"></script>
<script type="text/javascript" src="/bootstrapDi/js/validation.js"></script>
<script type="text/javascript" src="/bootstrapDi/js/selectivizr-min.js"></script>

<script type="text/javascript">
    var pageId = <%=CurrentPage.PageLink.ID %>;
    var fixedflag = false;

    //attach ajax event
    $(document).ajaxComplete(function () {
        $('#loading-indicator').hide();
        //wait while so papers is fully loaded
        window.setTimeout(ShowFixedReadMore, 500);   
        ResetFixedReadMore();       
    });
    $(document).ajaxError(function () {
        $("#PaperList").text("Ett fel har uppstått.");
    });
    $.ajaxSetup({
        cache: false
    });

    
    $(document).ready(function () {
        //Load default papers
        LoadPapers("Campaign1");    
        //Attach slideToggle to sidebar paper list
        $(".sidebarpapers p").click(function () {
            //Only track on show
            if (!$(this).next().is(":visible")) {
                Track('Readmore', $(this).find('.hd').text(), 'Step2');
            }
            $(this).next().slideToggle('fast');            
        });
        $(".sidebarpapers .closex").click(function () {
            $(this).parent().slideUp('fast');
        });    
        //Reset all buttons from loading events
        $(".btn").button('reset');
        //Attach loading events to all buttons
        $(".btn").click(function () {
            $(this).button('loading');
        });
        //Attach event to checkboxes on step 1
        $('.enable #RbPrimaryCampaignStep1').click(function () {
            if ($(this).prop("checked") == true) {
                LoadPapers("Campaign1");
                Track('ChooseOffer', 'Campaign1', 'Step1');
            }
        });
        $('.enable #RbSecondaryCampaignStep1').click(function () {
            if ($(this).prop("checked") == true) {
                LoadPapers("Campaign2");
                Track('ChooseOffer', 'Campaign2', 'Step1');
            }
        });
        //Attach click event to read more
        $("#posreadmore0 .rm").click(function () {            
            var rml = $(this).find(".rml");
            if(rml.hasClass("top")){
                ResetFixedReadMore();
                //Scroll to top
                $('html, body').animate({ scrollTop: $(".container").offset().top }, 'slow');     
            }
            else{
                fixedflag = true;
              rml.addClass("top");
              rml.html("<i class='icon-chevron-up'></i><br>Toppen");
                // Scroll down to 'paperlistPosition'
                $('html, body').animate({ scrollTop: $("#PaperList").offset().top }, 'slow');            
            }

        });        
        //Attach event on window scroll
        $(window).scroll(function () {
            ShowFixedReadMore();
        });
    });

    function ResetFixedReadMore()
    {
        fixedflag = false;
        var rml = $("#posreadmore0 .rml");
      rml.removeClass("top");
      rml.html("Läs mer<br><i class='icon-chevron-down'></i>");
    }

    function ShowFixedReadMore()
    {
        //Only on step 1
        if($("#posreadmore0").length == 0 || fixedflag)
            return;

        var viewportHeight = $(window).height();       
        var paperListHeight = $("#PaperList").height();      
        var scrollPosition = $(window).scrollTop();       
        var paperlistPosition = $("#PaperList").offset().top;

        //If bottom is reached - hide readmore
        if(scrollPosition + viewportHeight == $(document).height()){      
            $("#posreadmore0").slideUp(); //hide                
        }
        else if((paperListHeight + paperlistPosition) > viewportHeight && scrollPosition < paperlistPosition){
            $("#posreadmore0").slideDown(); //show
        }
        else{
            $("#posreadmore0").slideUp(); //hide    
        }
    }

    function LoadPapers(campaign) {
        //Only load papers if placeholder exist
        if ($("#PaperList").length) {
            $("#loading-indicator").show();

            //You can't use delay before load, cause load ignores the que
            $("#PaperList").delay(500).queue(function (nxt) {
                $(this).load("/Templates/Public/Pages/CampaignTemplates/CampaignPaper/CampaignPaperList.aspx?c=" + campaign + "&id=" + pageId);
                nxt(); //only used to clear que                
            });
        }        
    }

    function Track(context, name, step) {
        _gaq.push(['_trackEvent', context, name, step]);        
    }
    function TrackSocial(network, action) {
        _gaq.push(['_trackSocial', 'sharing', network, action]);
    }
    function TrackPageview(url){        
        _gaq.push(['_trackPageview', url]);
    }

    function ResetStartDate()
    {
        $("#dpStartDate input:text").val("");
    }

</script>

<script type="text/javascript" src="http://ll.lp4.io/app/53/26/18/532618ace45a1dec4dc5e7d2.js"></script>