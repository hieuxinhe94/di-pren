define([
    "jquery",    
    "jqueryvalidate",
    "pubsub",
    "ext/buttonIndicator",
    "func/ajax",
    "profdom/contactDom",
    "classes/subscriber"],
    function ($, jqueryvalidate, pubsub, btnIndicator, ajax, dom, subscriber) {
        return new ContactInfo(ajax, dom, subscriber);
    });

function ContactInfo(ajax, dom, subscriber) {
    this.ajax = ajax;
    this.dom = dom;
    this.subscriber = subscriber;
}

ContactInfo.prototype.init = function () {
    var self = this;

    $(self.dom.togglebtns).on("click", function () {
        $(this).parent().siblings(".edit").slideToggle();
    });

    // Set up events
    self.dom.name.form.submit(function () {
        var firstName = self.dom.name.elementFirstName.val().trim();
        var lastName = self.dom.name.elementLastName.val().trim();
        var fullName = firstName + " " + lastName;

        var onSuccess = function () {
            self.subscriber.changeProperty("firstName", firstName);
            self.subscriber.changeProperty("lastName", lastName);
        }

        self.update(self.dom.name, '/api/mysettings/profile/updatename/' + encodeURIComponent(firstName) + "/" + encodeURIComponent(lastName), fullName, onSuccess);
    });

    self.dom.email.form.submit(function () {       
        var email = self.dom.email.elementEmail.val().trim();

        var onSuccess = function () {
            self.subscriber.changeProperty("email", email);
            $.publish("mypage-track-event", 'ändrat e-postadress');
        }

        self.update(self.dom.email, '/api/mysettings/profile/updateemail/' + encodeURIComponent(email), email, onSuccess);
    });

    self.dom.phone.form.submit(function () {
        var phone = self.dom.phone.elementPhone.val().trim();

        var onSuccess = function() {
            self.subscriber.changeProperty("phone", phone);
            $.publish("mypage-track-event", 'ändrat telefonnummer');
        }

        self.update(self.dom.phone, '/api/mysettings/profile/updatephone?phone=' + encodeURIComponent(phone), phone, onSuccess);
    });


    // Set up validation
    self.dom.name.form.validate({
        rules: {
            firstName: {
                required: true
            },
            lastName: {
                required: true
            }
        },
        messages: {
            firstName: {
                required: "Vänligen ange förnamn"
            },
            lastName: {
                required: "Vänligen ange efternamn"
            }
        }
    });

    self.dom.phone.form.validate({
        rules: {
            phone: {
                required: true
            }
        },
        messages: {
            phone: {
                required: "Vänligen ange telefonnummer"
            }
        }
    });

    self.dom.email.form.validate({
        rules: {
            email: {
                required: true,
                email: true
            }
        },
        messages: {
            email: {
                required: "Vänligen ange e-postadress",
                email: "Vänligen ange en korrekt e-postadress"
            }
        }
    });
}

ContactInfo.prototype.setComingEvents = function () {

    var self = this;

    var successCallback = function (data) {
        alert(data);
    };

    var errorCallback = function () {
        $.publish("feedback", "Dä bidde fel hä");
    }

    self.ajax.makeAjaxRequest("/api/mysettings/profile/events", 'GET', successCallback, errorCallback);
}

ContactInfo.prototype.setContactInfo = function() {
    // Set contact info
    this.dom.elementCusno.text(this.subscriber.customerNumber);
    this.dom.name.element.text(this.subscriber.firstName + " " + this.subscriber.lastName);
    this.dom.phone.element.text(this.subscriber.phone);
    this.dom.email.element.text(this.subscriber.email);
}

ContactInfo.prototype.update = function (root, apiUrl, value, onSuccess) {
    var self = this;

    if (!root.form.valid()) return;

    root.submit.loadingIndicatorLoad(function () { return true; });

    var successCallback = function () {
        root.element.text(value);
        onSuccess(value);
        root.edit.fadeOut();
        root.elementInputs.val("");
    };

    var errorCallback = function () {
        $.publish("feedback", root.errors.update);
    }

    var generalCallback = function () {
        root.submit.loadingIndicatorReset();
    }

    self.ajax.makeAjaxRequest(apiUrl, 'POST', successCallback, errorCallback, generalCallback);
}
