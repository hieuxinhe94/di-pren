function InviteHandler(ajaxHandler, bizSubscriptionId, pagerHandler, tabs, inviteByEmailForm) {
    var self = this;
    self.ajaxHandler = ajaxHandler;
    self.bizSubscriptionId = bizSubscriptionId;
    self.pagerHandler = pagerHandler;
    self.tabs = tabs;

    self.inviteByEmailForm = inviteByEmailForm;
    self.sendInvitesButtonEl = inviteByEmailForm.find("#send-mail-invites");
    self.inviteEmailInputContainerEl = inviteByEmailForm.find("#email-input-container");
    self.addMoreEmailsButtonEl = inviteByEmailForm.find("#add-more-emails");
    self.inviteMessageBox = inviteByEmailForm.find("#invite-message");

    self.numberOfInvites = 1; // Default there is 1 input field for email invites
    self.numberOfHandledInvites = 0;
    self.invitedEmails = [];

    function addValidationRules() {
        $("#invite-by-email").validate({
            rules: {
                email: {
                    required: true,
                    email: true
                }
            },
            messages: {
                email: {
                    required: "Var god ange en e-postadress",
                    email: "Du måste skriva in en korrekt e-postadress"
                }
            }
        });
    }

    self.sendInvitesButtonEl.on("click", function (e) {
        self.sendInvitesClick(e);
    });

    self.sendInvitesButtonEl.loadingIndicatorLoad(
        function () {
            // Only indicate loading if form is valid
            return self.inviteByEmailForm.valid();
        }
    );

    self.addMoreEmailsButtonEl.on("click", function(e) {
        self.addMoreEmailsClick(e);
    });

    // Fires on every invite sent to S+
    $.subscribe('biz-invite-successfull', function (notUsedElem, result, inputElement) {
        self.singleInviteSuccessfull(inputElement);
    });

    $.subscribe('biz-invite-failed', function (notUsedElem) {
        self.showInviteFailed();
        self.sendInvitesButtonEl.loadingIndicatorReset();
    });

    // Fires everytime a get of pending subscriber is made
    $.subscribe('biz-get-pending-successfull-after-add', function (notUsedElem, pendingSubscribers) {
        self.publishPendingInvites(pendingSubscribers);
        self.sendInvitesButtonEl.loadingIndicatorReset();
        self.tabs.setTabAsActive("invites");
    });

    // Fires when all invites are handled
    $.subscribe('biz-invited-subscribers-success', function (notUsedElem, invitedEmails) {
        self.showInviteConfirm(invitedEmails);
    });

    addValidationRules();
};

InviteHandler.prototype.sendInvitesClick = function(e) {
    var self = this;
    e.preventDefault();

    // Check if form is valid
    if (!self.inviteByEmailForm.validate().form()) {
        return;
    }

    self.numberOfInvites = self.inviteEmailInputContainerEl.find("input").length;

    // Iterate over the input fields and make api request for each one
    self.inviteEmailInputContainerEl.find("input").each(function() {
        var inputElement = $(this);
        self.ajaxHandler.makeAjaxRequest(
            "/api/biz/invitebizsubscriber/" + self.bizSubscriptionId + "/" + inputElement.val(),
            "biz-invite-successfull",
            inputElement, // On successfull invite we make the ajaxhandler include our input element in the puslish parameters
            "biz-invite-failed");
    });
};

InviteHandler.prototype.addMoreEmailsClick = function (e) {
    var self = this;
    e.preventDefault();

    // Check if form is valid
    if (!self.inviteByEmailForm.validate().form()) {
        return;
    }

    // Input count is used to set proper id and name
    var inputCount = self.inviteEmailInputContainerEl.data("inputcount");
    var identifier = "email" + inputCount;
    var cssClass = self.inviteEmailInputContainerEl.find("input").first().attr("class");

    // Add new input
    var input = $("<input>")
        .attr("type", "email")
        .attr("id", identifier)
        .attr("name", identifier)
        .addClass(cssClass);

    self.inviteEmailInputContainerEl.append(input);

    var removeBtn = $("<a>")
        .css("width", "100%")
        .css("display", "block")
        .attr("href", "#")
        .text("Ta bort fält")
        .on("click", function (ev) {
            ev.preventDefault();
            input.remove();
            $("#" + identifier + "-error").remove();
            $(this).remove();
        });

    self.inviteEmailInputContainerEl.append(removeBtn);

    // Add validation rules to new input
    input.rules("add", {
        required: true,
        email: true,
        messages: {
            required: "Var god ange en e-postadress",
            email: "Du måste skriva in en korrekt e-postadress"
        }
    });

    // Update input count
    self.inviteEmailInputContainerEl.data("inputcount", (inputCount + 1));
};

InviteHandler.prototype.singleInviteSuccessfull = function(inputElement) {
    var self = this;
    //save invited email
    var invitedEmail = inputElement.val();

    // clear input
    inputElement.val('');

    // remove inputs except the "master one"
    if (inputElement.attr("id") !== "inviteemailinput") {
        inputElement.remove();
    }

    // Remove all "remove field btns"
    self.inviteEmailInputContainerEl.find("a").remove();

    // Push the invited email to the array
    self.invitedEmails.push(invitedEmail);

    // Increase handled invite count
    self.numberOfHandledInvites++;

    // If numberOfHandledInvites === numberOfInvites we have handled all invites and reload the pending list
    if (self.numberOfHandledInvites === self.numberOfInvites) {
        ajaxHandler.makeAjaxRequest(
            "/api/biz/getpendingbizsubscribers/" + self.bizSubscriptionId + self.pagerHandler.GetAllInCurrentScopeAsRouteParameters(),
            "biz-get-pending-successfull-after-add",
            invitedEmail, // On successfull get we make the ajaxhandler include the invited email in the publish parameters
            "biz-get-pending-failed-after-add");

        // Reset invite numbers
        self.numberOfHandledInvites = 0;
        self.numberOfInvites = 1;
    }
};

InviteHandler.prototype.publishPendingInvites = function (pendingSubscribers) {
    var self = this;
    
    $.each(self.invitedEmails, function (i, email) {
        var addedEmailIsIncluded = false;
        $.each(pendingSubscribers, function (ii, subscriber) {
            console.log(subscriber.Email);
            if (subscriber.Email === email) {
                addedEmailIsIncluded = true;
                return false;
            }
        });

        if (!addedEmailIsIncluded) {
            console.log("Manually added " + email);
            pendingSubscribers.push({ Email: email, Status: "GUIADDED" });
        }
    });

    $.publish("biz-invited-subscribers-success", [self.invitedEmails]);

    // Reset invitedEmails array
    self.invitedEmails = [];

    $.publish("biz-pending-subscribers-get-success", [pendingSubscribers, true]);
};

InviteHandler.prototype.showInviteConfirm = function (invitedEmails) {
    var self = this;
    self.inviteMessageBox.removeClass("alert-danger");
    self.inviteMessageBox.html("");
    self.inviteMessageBox.append($("<div>").text("Du har bjudit in:"));
    $.each(invitedEmails, function (i, email) {
        var emailDiv = $("<div>");
        emailDiv.text(email);
        self.inviteMessageBox.append(emailDiv);
    });

    self.inviteMessageBox.fadeIn("slow");
    setTimeout(function () {
        self.inviteMessageBox.fadeOut("slow");
    }, 4000);
};

InviteHandler.prototype.showInviteFailed = function () {
    var self = this;
    self.inviteMessageBox.addClass("alert-danger");
    self.inviteMessageBox.html("");
    self.inviteMessageBox.append($("<div>").text("Ett fel uppstod med din inbjudan. Kontrollera att e-posten inte redan är inbjuden."));

    self.inviteMessageBox.fadeIn("slow");
    setTimeout(function () {
        self.inviteMessageBox.fadeOut("slow");
    }, 4000);
};