function Tracking() {
    this.subscibeEvent = 'subscribe';
    this.stepSelectCampaign = 'välj prenumeration';
    this.stepFillInForm = 'fyll i uppgifter';    
};

Tracking.prototype.addListeners = function () {
    var self = this;

    customEventHandler.subscribe(selectedCampaign.events.selectedCampaignChanged, function () {
        self.push(self.subscibeEvent, selectedCampaign.properties.title, self.stepSelectCampaign);
    });

    customEventHandler.subscribe(stepEmail.events.triggers.onSubmitEvent, function () {
        if (!elementFactory.getElement(elementFactory.elements.stepEmail).find("#emailinput").val().length) {
            self.push(self.subscibeEvent, selectedCampaign.properties.title, 'kom igång med din prenumeration');
        }        
    });

    customEventHandler.subscribe(stepPhone.events.listeners.initEvent, function () {
        if (!elementFactory.getElement(elementFactory.elements.stepPhone).find("#phoneinput").val().length) {
            //self.push(self.subscibeEvent, selectedCampaign.properties.title, self.stepFillInForm + ' - ange telefonnummer');
            self.push(self.subscibeEvent, selectedCampaign.properties.title, self.stepFillInForm + ' - angett e-postadress');
        }        
    });

    customEventHandler.subscribe(stepSsn.events.listeners.initEvent, function () {
        //self.push(self.subscibeEvent, selectedCampaign.properties.title, self.stepFillInForm + ' - ange personnummer/org.nr');
        self.push(self.subscibeEvent, selectedCampaign.properties.title, self.stepFillInForm + ' - angett telefonnummer');
    });

    customEventHandler.subscribe(stepAddress.events.listeners.initEvent, function () {
        //self.push(self.subscibeEvent, selectedCampaign.properties.title, self.stepFillInForm + ' - ange adress');
        self.push(self.subscibeEvent, selectedCampaign.properties.title, self.stepFillInForm + ' - angett personnummer/org.nr');
    });

    customEventHandler.subscribe(stepPayment.events.listeners.initEvent, function () {
        //self.push(self.subscibeEvent, selectedCampaign.properties.title, self.stepFillInForm + ' - välj betalsätt');
        self.push(self.subscibeEvent, selectedCampaign.properties.title, self.stepFillInForm + ' - angett adress');
    });

    customEventHandler.subscribe("emailExists", function () {
        self.push(self.subscibeEvent, selectedCampaign.properties.title, 'logga in');
    });

    customEventHandler.subscribe("loginClicked", function () {
        self.push(self.subscibeEvent, selectedCampaign.properties.title, 'logga in klick');
    });    
};

Tracking.prototype.isLoggedIn = function() {
    return (elementFactory.getElement(elementFactory.elements.isServicePlusUser).val() == "true");
};

Tracking.prototype.getTargetGroup = function () {
    return $("#targetgroupinput").val();
};

Tracking.prototype.getCampaignId = function() {
    return $("#campidinput").val();
}

Tracking.prototype.push = function (event, subscription, subscriptionStep, loggedIn, targetGroup, campaignId) {

    if (loggedIn === undefined) {
        loggedIn = this.isLoggedIn();
    }
    if (targetGroup === undefined) {
        targetGroup = this.getTargetGroup();
    }
    if (campaignId === undefined) {
        campaignId = this.getCampaignId();
    }

    dataLayer.push({
        'product': 'PREN',
        'order': subscription,
        'event': event,
        'subscription': subscription,
        'subscription_step': subscriptionStep,
        'loggedin': loggedIn,
        'targetGroup': targetGroup,
        'campaignId' : campaignId
    });
};