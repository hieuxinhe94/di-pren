payment = {
    methods: {
        card: {
            _tabSelector: "#card-tab",
            _contentSelector: "#card"
        },
        invoice: {
            _tabSelector: "#invoice-tab",
            _contentSelector: "#invoice"
        },
        autogiro: {
            _tabSelector: "#autogiro-tab",
            _contentSelector: "#autogiro"
        }
    },
    updateAvailableMethods : function(selectedCampaign) {
        payment.disableAll();

        if (selectedCampaign.campIdAutogiro != '') {
            $(payment.methods.autogiro._tabSelector).data("campid", selectedCampaign.campIdAutogiro);
            payment.enable(payment.methods.autogiro);
            payment.setActive(payment.methods.autogiro);
        }
        
        if (selectedCampaign.campIdCardAndInvoice != '') {
            // Check if card payment should be hidden
            if (!selectedCampaign.hideCardPayment) {
                $(payment.methods.card._tabSelector).data("campid", selectedCampaign.campIdCardAndInvoice);
                payment.enable(payment.methods.card);
            }

            $(payment.methods.invoice._tabSelector).data("campid", selectedCampaign.campIdCardAndInvoice);
            payment.enable(payment.methods.invoice);
            payment.setActive(payment.methods.invoice);
        }

        var campaign = selectedCampaign.periodButtonElement.parents(".pren-selection");
        $("#invoice-pay-text").html(campaign.data("invoicepaytext"));
        $("#card-pay-text").html(campaign.data("cardpaytext"));
        $("#autogiro-pay-text").html(campaign.data("autogiropaytext"));
    },
    enable: function(paymentmethod) {
        $(paymentmethod._tabSelector).show();
        //$(paymentmethod._contentSelector).show();
    },
    disable: function (paymentmethod) {
        $(paymentmethod._tabSelector).hide();
        //$(paymentmethod._contentSelector).hide();
    },
    setActive: function (paymentmethod) {
        $(paymentmethod._tabSelector).addClass("active")
            .siblings().removeClass("active");
        $(paymentmethod._contentSelector).addClass("active")
            .siblings().removeClass("active");

        payment.setCampNo($(paymentmethod._tabSelector).data("campid"));
        payment.setPaymentMethod($(paymentmethod._tabSelector).data("method"));
        payment.setIsDigital(selectedCampaign.properties.isDigital);
        payment.setIsStudent(selectedCampaign.properties.isStudent);
        payment.setIsTrial(selectedCampaign.properties.isTrial);
        payment.setIsTrialFree(selectedCampaign.properties.isTrialFree);
        payment.setIsPayWall(selectedCampaign.properties.isPayWall);
        payment.setId(selectedCampaign.properties.id);        
    },
    disableAll : function() {
        $.each(payment.methods, function(index, method) {
            payment.disable(method);
            $(method._tabSelector).data("campid", "");
        });
    },
    setId: function(id) {
        $("#campaigncontentidinput").val(id);
    },
    setCampNo: function (campId) {
        //campId can contain both campId and campNo (separated by |)
        try {
            if (campId.indexOf("|") > -1) {
                var campArray = campId.split("|");
                campId = campArray[0];
                $("#campnoinput").val(campArray[1]);
            } else {
                $("#campnoinput").val(""); //reset
            }
        } catch(err) {}

        $("#campidinput").val(campId);        
        $("#loginlink").attr("href", payment.updateQueryString("selectedcamp", campId, $("#loginlink").attr("href")));
        $("#logoutlink").attr("href", payment.updateQueryString("selectedcamp", campId, $("#logoutlink").attr("href")));
    },
    setPaymentMethod : function(paymentMethod) {
        $("#paymentmethodinput").val(paymentMethod);
    },
    setIsDigital: function (isDigital) {        
        $("#isdigitalinput").val(isDigital);
    },
    setIsStudent: function(isStudent) {
        $("#isstudentinput").val(isStudent);
    },
    setIsTrial: function(isTrial) {
        $("#istrialinput").val(isTrial);
    },
    setIsTrialFree: function (isTrialFree) {
        $("#istrialfreeinput").val(isTrialFree);
    },
    setIsPayWall: function (isPayWall) {
        $("#ispaywallinput").val(isPayWall);
    },
    updateQueryString : function(key, value, url) {
        if (!url) url = window.location.href;
        var re = new RegExp("([?&])" + key + "=.*?(&|#|$)(.*)", "gi"),
            hash;

        if (re.test(url)) {
            if (typeof value !== 'undefined' && value !== null)
                return url.replace(re, '$1' + key + "=" + value + '$2$3');
            else {
                hash = url.split('#');
                url = hash[0].replace(re, '$1$3').replace(/(&|\?)$/, '');
                if (typeof hash[1] !== 'undefined' && hash[1] !== null) 
                    url += '#' + hash[1];
                return url;
            }
        }
        else {
            if (typeof value !== 'undefined' && value !== null) {
                var separator = url.indexOf('?') !== -1 ? '&' : '?';
                hash = url.split('#');
                url = hash[0] + separator + key + '=' + value;
                if (typeof hash[1] !== 'undefined' && hash[1] !== null) 
                    url += '#' + hash[1];
                return url;
            }
            else
                return url;
        }
    }
};