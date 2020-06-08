requirejs([
    "jquery",
    "domReady",    
    "func/load",
    "func/menu",
    "func/shortcut",
    "func/dateAndTime",
    "func/mobile",
    "app/overview",
    "app/profile",
    "app/subscriptions",
    "app/contactForm",
    "app/faq",
    "app/mycodes",
    "classes/subscriber"],
function ($, domReady, load, menu, shortcut, dateAndTime, mobile, overview, profile, subscriptions, contactForm, faq, mycodes, subscriber) {
    
    domReady(function () {

        // PreventDefault on all form submits  
        $("form").submit(function (event) {
            event.preventDefault();
        });

        // PreventDefault on all links except target blank
        $("body").on("click", ".contentblocks a[href='#']", function (event) {
             event.preventDefault();
        });

        setUpAnchor();
        init();
    });

    function init() {
        load.closeLoader(); //close load-modal
        mobile.setUpMobile();
        faq.populateFaqTopics();
        
        if (profile.exists) {
            shortcut.init();
            overview.setNameInHeader();
            profile.setUpProfile();

            if (subscriber.activeSubscriptions.length) {
                subscriptions.init();
                subscriptions.setUpSubscriptions();
                subscriptions.setUpCancelForm();
            } else {
                subscriptions.hideSubscriptionsInfo();
                profile.hideSubscriptionsInfo();
            }

            contactForm.setUpForm();
            mycodes.setUpMyCodes();
            shortcut.triggerShortcutFromHash();
        }
    }

    function setUpAnchor() {
        var anchor = $("#anchor").val();
        if (anchor) {
            window.location.hash = anchor;
        }
    }
});
