define([
    "jquery",
    "jqueryvalidate",
    "pubsub",
    "classes/subscriber",
    "dom/contactFormDom",
    "func/ajax"],
    function ($, jqvalidate, pubsub, subscriber, dom, ajax) {
        return new ContactForm(subscriber, dom, ajax);
    });

function ContactForm(subscriber, dom, ajax) {
    this.subscriber = subscriber;
    this.dom = dom;
    this.ajax = ajax;
    this.shouldSendToCustomerService = $(this.dom.form).data("sendemail");
    this.init();
} 

ContactForm.prototype.init = function () {
    var self = this;

    // If subscriber object changes, setUp form again with updated subscriber object
    $.subscribe("subscriberChanged", function () {
        self.setUpForm();
    });

    // Handle click on receipt btn in profile area
    $("#order-receipt").on("click", function () {
        $("#receiptorderinput").prop("checked", true);
        $("#contactmessageinput").val("Hej, jag skulle vilja beställa kvitto på mina betalningar som gäller min prenumeration, sammanställda från följande tidsperiod:\n\nTidsperiod: 12 månader tillbaka\nLeveranssätt: Kvittona önskar jag att få digital via mail till min e-postadress som jag kopplat till mitt Di-konto.\n\nTack så mycket!");
        $("#contactmessageinput").prop("readonly", true);
    });

    $("#btn-reset-message").on("click", function (event) {
        event.preventDefault();
        $("#receiptorderinput").prop("checked", false);
        $("#contactmessageinput").prop("readonly", false);
        $("#contactmessageinput").val("");
    });

    $("#chatbtn").on("click", function (event) {
        event.preventDefault();

        $("#chat-modal").modal({
            keyboard: false,
            backdrop: 'static'
        });       
    });

    self.dom.form.submit(function () {
        if (!$(this).valid()) return;

        var json = JSON.stringify($(self.dom.form).serializeObject());

        self.ajax.makeAjaxRequest("/api/mysettings/contactcustomerservice/contact",
            "POST",
            function() {
                $(self.dom.form).hide();
                self.dom.thankyoumessage.removeClass("hidden").show();
            },
            function () { alert('Ett fel uppstod, var vänlig kontakta oss på kundservice@di.se'); },
            function() {},
            json);
    });

    // Set up validation
    self.dom.form.validate({
        rules: {
            name: {
                required: true
            },
            phone: {
                required: true
            },
            email: {
                required: true,
                email: true
            },
            customernumber: {
                required: true,
                number: true
            },
            message: {
                required: true
            }
        },
        messages: {
            name: {
                required: "Vänligen ange ditt namn"
            },
            phone: {
                required: "Vänligen ange ditt telefonnummer"
            },
            email: {
                required: "Vänligen ange din e-postadress",
                email: "Du har angivit en felaktig e-postadress"
            },
            customernumber: {
                required: "Vänligen ange ditt kundnummer",
                number: "Ogiltigt kundnummer"
            },
            message: {
                required: "Du måste ange ett meddelande"
            }
        }
    });
}

ContactForm.prototype.setUpForm = function () {

    this.dom.nameinput.val(this.subscriber.firstName + " " + this.subscriber.lastName);
    this.dom.phoneinput.val(this.subscriber.phone);
    this.dom.emailinput.val(this.subscriber.email);
    this.dom.cusnoinput.val(this.subscriber.customerNumber);

    if (this.shouldSendToCustomerService) {
        this.dom.messageinput.removeAttr("maxlength");
        this.dom.messagelabel.text("Ärende *");
    }

    // Hide inputs already populated??
    //$.each(self.dom.forminputs, function() {
    //    if ($(this).val().length) {
    //        $(this).parents(".form-group").hide();
    //    }
    //});
}

