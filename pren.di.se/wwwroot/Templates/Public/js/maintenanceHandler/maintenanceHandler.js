var maintenanceHandler = {
    mySettingsPageTypeNames: [
    ],
    campaignPageTypeNames: [
        "[DI] Campaign Paper 2014", //http://lokalpren.di.se/Kampanj/C-prototyp/
        "[DI] Campaign Paper", //http://lokalpren.di.se/Kampanj/papperstidning/
        "[DI] Campaign Digital" //http://lokalpren.di.se/agenda/utrbis/?token=&id=&remembered=false
    ],
    textConstants: {
        bodyCampaign: "<p><strong>Hej!</strong><p>Tack för visat intresse för Dagens industri. Mellan den 31/1 - 1/2 genomför vi systembyten hos oss för att du som kund ska få bättre service. Under denna period kan du inte teckna din prenumeration direkt här på sajten.</p><p>Fyll i formuläret nedan så kontaktar vi dig snarast för att hjälpa dig att komma igång med tidningen. </p><p>Vänligen,<br/>Dagens industris prenumerationsavdelning</p>",
        bodyMySettings: "<p style=\"padding:50px;\"><strong>Under perioden 31/1-1/2 genomför vi systembyten hos oss för att du som kund ska få bättre service.<br />Vänligen maila ditt ärende till oss på <a href=\"mailto:pren@di.se\">pren@di.se.</a></strong></p>",
        bodyMailResponse: "<p>Tack, vi kontaktar dig.</p>"
    },
    init: function() {
        var currentPageTypeName = $("body").data("pagetypename");

        var kcookieVal = maintenanceHandler.parseBool(maintenanceHandler.getCookie('disableMaintPopup'));
        if (kcookieVal) {
            return; //Disable this whole handler for testing purpose
        }

        if (maintenanceHandler.checkPageTypeNameMatch(maintenanceHandler.mySettingsPageTypeNames, currentPageTypeName)) {
            maintenanceHandler.handleMySettingsPages();
        }
        if (maintenanceHandler.checkPageTypeNameMatch(maintenanceHandler.campaignPageTypeNames, currentPageTypeName)) {
            maintenanceHandler.handleCampaignPages();
        }
    },
    checkPageTypeNameMatch: function(pageTypeNamesArray, currentPageTypeNames) {
        var pageTypeNameMatch = false;
        $.each(pageTypeNamesArray, function(index, value) {
            if (value == currentPageTypeNames) {
                pageTypeNameMatch = true;
                return false;
            }
        });
        return pageTypeNameMatch;
    },
    handleMySettingsPages: function() {
        maintenanceHandler.createModalCover();

        var popup = $("<div>");
        var left = (window.innerWidth * 0.5) - 250;
        popup.css({ 'position': 'absolute', 'top': '100px', 'left': left + 'px', 'height': '200px', 'width': '500px', 'background-color': '#fff', 'display': 'block', 'z-index': '99999', 'box-shadow': '5px 5px 25px rgba(55, 55, 55, 0.5)', 'border-radius': '15px' });
        popup.html(maintenanceHandler.textConstants.bodyMySettings);
        $("body").append(popup);
    },
    handleCampaignPages: function() {
        maintenanceHandler.createModalCover();

        var popup = $("<div id=\"maintenancepopup\">");
        var left = (window.innerWidth * 0.5) - 250;
        //popup.css({ 'position': 'absolute', 'top': '100px', 'left': left + 'px', 'height': 'auto', 'width': '400px', 'background-color': '#fff', 'display': 'block', 'z-index': '99999', 'box-shadow': '5px 5px 25px rgba(55, 55, 55, 0.5)', 'border-radius': '15px', 'padding': '50px', 'box-sizing': 'unset' });
        popup.css({ 'position': 'relative', 'margin':'30px auto',  'top': '100px', 'height': 'auto', 'max-width': '500px', 'background-color': '#fff', 'display': 'block', 'z-index': '99999', 'box-shadow': '5px 5px 25px rgba(55, 55, 55, 0.5)', 'border-radius': '15px', 'padding': '50px', 'box-sizing': 'unset' });
        popup.append(maintenanceHandler.textConstants.bodyCampaign);

        var nameLabel = $("<label for=\"name\">").text("Namn");
        var nameInput = $("<input id=\"name\" type=\"text\">").css("width", "97%");
        popup.append(nameLabel);
        popup.append(nameInput);

        var phoneLabel = $("<label for=\"phone\">").text("Telefonnummer");
        var phoneInput = $("<input id=\"phone\" type=\"text\">").css("width", "97%");
        popup.append(phoneLabel);
        popup.append(phoneInput);

        var emailLabel = $("<label for=\"email\">").text("E-postadress");
        var emailInput = $("<input data-validate=\"email\" id=\"email\" type=\"text\">").css("width", "97%");
        popup.append(emailLabel);
        popup.append(emailInput);

        // Set focus on document ready. Otherwise the email-input in campaign will have focus.
        $(document).ready(function () {
            nameInput.focus();
            $("html, body").animate({ scrollTop: 0 }, "fast"); //if name input is far down the page, scroll to top just in case
        });        

        var howToContactLabel = $("<label for=\"howToContact\">").text("Hur vill du bli kontaktad?");
        var howToContactSelect = $("<select>").append("<option>Telefonnummer</option><option>E-postadress</option>").css("width", "100%");
        popup.append(howToContactLabel);
        popup.append(howToContactSelect);

        var sendBtn = $("<input type=\"submit\">")
            .addClass("btn-large btn-success margintop20")
            .attr("value", "Skicka")
            .css("width", "100%")
            .click(function (e) {
                var okToSend = true;
                $("#maintenancepopup input[type=text]").each(function () {
                    if ($(this).val() == "" || ($(this).data("validate") == "email" && !maintenanceHandler.isValidEmailAddress($(this).val()))) {
                        $(this).css("border", "2px solid red");
                        okToSend = false;
                    } else {
                        $(this).css("border", "1px solid #ccc");
                    }
                });
                if (okToSend) {
                    sendBtn.addClass("btn disabled");
                    sendBtn.attr("disabled", "disabled");
                    maintenanceHandler.sendContactMeMail(nameInput.val(), phoneInput.val(), emailInput.val(), howToContactSelect.val(), function() {
                        popup.html(maintenanceHandler.textConstants.bodyMailResponse);
                    });
                } 
            });
        popup.append(sendBtn);


        //add bootstrap look and feel
        popup.find("label").css({
            'margin-bottom': '5px',
            'display': 'block',
            'font-size': '14px',
            'line-height': '20px'
        });

        popup.find("input[type=text], select").css({
            'border': '1px solid #ccc',
            'box-shadow': '0 1px 1px rgba(0, 0, 0, 0.075) inset',
            'border-radius': '4px',
            'color': '#555',
            'font-size': '14px',
            //'height': '20px',
            'margin-bottom': '10px',
            'padding': '4px 6px',
            'box-sizing': 'unset'
        });

        popup.find("select").css({
            'border': '1px solid #ccc',
            //'height': '30px',
            'font-size': '14px',
            'line-height': '30px',
            'box-sizing': 'border-box'
        });

        popup.find("input[type=submit]").css({
            'background-color': '#5bb75b',
            'background-image': 'linear-gradient(to bottom, #62c462, #51a351)',
            'color': "#fff",
            'border-radius': '6px',
            'border-color': 'rgba(0, 0, 0, 0.1) rgba(0, 0, 0, 0.1) rgba(0, 0, 0, 0.25)',
            'font-size': '17.5px',
            'text-shadow': '0 -1px 0 rgba(0, 0, 0, 0.25)',
            'padding': '11px 19px',
            'margin-top': '20px'
        });

        var wrapper = $("<div>").css({ 'position': 'absolute', 'top': '0', 'width': '100%' });
        wrapper.append(popup);
        $("body").append(wrapper);
        
    },
    sendContactMeMail: function (name, phone, email, prefferedContactedBy, callBack) {

        $.ajax({
            type: "POST",
            url: "/Templates/Public/js/maintenanceHandler/MailService.ashx?name=" + name + "&phone=" + phone + "&email=" + email + "&prefferedContactedBy=" + prefferedContactedBy,
            timeout: 20000,
            dataType: "text",
            success: function (msg) {
                if (msg == "ok") {
                    callBack();
                }
            },
            error: function (xhr, textStatus, error) {
                alert('Ett fel har uppstått, var god försök igen....');
            }
        });        
    },
    createModalCover: function () {
        var modalCover = $("<div id='modalcover'>");
        modalCover.css({ 'position': 'fixed', 'top': '0', 'left': '0', 'height': '100%', 'width': '100%', 'background-color': 'rgba(0,0,0,0.5)', 'z-index': '1000', 'padding': '100px' });
        $("body").append(modalCover);
    },
    isValidEmailAddress : function(emailAddress) {
        var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
        return pattern.test(emailAddress);
    },
    parseBool: function (value) {
        if (typeof value !== 'string') return false;
        return (value.toLowerCase() === 'true');
    },
    getCookie: function (cookieName) {
        var name,
        cookie,
        cookies = document.cookie.split(';');
        for (var i = 0, len = cookies.length; i != len; i += 1) {
            cookie = cookies[i];

            name = $.trim(cookie.substr(0, cookie.indexOf('=')));
            if (name === cookieName) {
                return $.trim(unescape(cookie.substr(cookie.indexOf('=') + 1)));
            }
        }
        return null;
    }
};