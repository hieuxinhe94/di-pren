function ImportPendingHandler(importForm, fileUploadElement, bizSubscriptionId, tabs, ajaxHandler) {
    var self = this;
    self.importForm = importForm;
    self.fileUploadElement = fileUploadElement;
    self.bizSubscriptionId = bizSubscriptionId;
    self.tabs = tabs;
    self.ajaxHandler = ajaxHandler;
    self.fileToUpload = null;
    self.noOfRowsToUpdate = 0;
    self.nowImportingRow = 0;

    self.importButton = self.importForm.find("#import-invites");

    self.statusModal = $('#import-status-modal');
    self.statusContainer = self.statusModal.find("#import-message");
    self.progressBarElement = self.statusContainer.find("#ImportProgressbar");
    self.statusTotalCount = self.statusContainer.find(".import-total");
    self.statusImportCount = self.statusContainer.find(".import-count");
    self.statusImportSuccessfullCount = self.statusContainer.find(".import-successfull");
    self.statusImportErrorArea = self.statusContainer.find("#import-error");
    self.statusImportSuccessArea = self.statusContainer.find("#import-success");
    self.statusImportSuccessRows = self.statusContainer.find("#rows-imported");
    self.statusImportFailedRows = self.statusContainer.find("#rows-not-imported");

    self.acceptFileTypes = ['txt', 'csv'];
    self.emailRegexp = new RegExp(/^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/);

    self.init();
};

ImportPendingHandler.prototype.init = function () {
    var self = this;

    // Check for File API support.
    if (window.File && window.FileReader && window.FileList && window.Blob) {
        // Disable form submit
        self.importForm.submit(function (event) {
            event.preventDefault();
        });

        self.importButton.on("click", function() {
            self.resetStatus();
            self.importSelectedFile();
        });

        $("#id_file").on("change", function() {
            self.fileToUpload = this.files[0];
        });
    }

    // Add loading indicator
    self.importButton.loadingIndicatorLoad(
        function () {
            // Only indicate loading if form is valid
            return self.importForm.valid();
        }
    );

    // Add validation rules
    self.importForm.validate({
        rules: {
            inviteFile: {
                required: true
            },
            filetoimport: {
                required: true
            }
        },
        messages: {
            inviteFile: {
                required: "Var god välj fil att importera",
            },
            filetoimport: {
                required: "Var god välj fil att importera",
            }
        }
    });

    $.subscribe('biz-invite-fromfile-successfull', function (_, result, email) {
        self.importSuccess(email);
    });

    $.subscribe('biz-invite-fromfile-failed', function (_, email) {
        self.importFailed(email);
    });

};

ImportPendingHandler.prototype.fileTypeIsAccepted = function (fileName) {
    var extension = fileName.split('.').pop().toLowerCase();
    return this.acceptFileTypes.indexOf(extension) > -1;    
}

ImportPendingHandler.prototype.importSelectedFile = function () {
    var self = this;
    var file = self.fileToUpload;
    if (file) {

        if (!self.fileTypeIsAccepted(file.name)) {
            alert("Felaktig filtyp");
            self.importButton.loadingIndicatorReset();
            return;
        }

        // Set active tab
        self.tabs.setTabAsActive("invites");

        var reader = new FileReader();

        reader.onload = function(event) {
            var content = event.target.result;
            var emails = content.split('\n');

            self.statusModal.modal('show');

            self.statusTotalCount.text(emails.length);

            $.each(emails, function(index, email) {

                email = $.trim(email);

                if (!self.emailRegexp.test(email)) {
                    self.importFailed(email);
                    return;
                }

                self.ajaxHandler.makeAjaxRequest(
                    "/api/biz/invitebizsubscriber/" + self.bizSubscriptionId + "/" + email,
                    "biz-invite-fromfile-successfull",
                    email,
                    "biz-invite-fromfile-failed",
                    email);

            });
        };
        reader.readAsText(file);
        //reader.readAsText(file, 'ISO-8859-1'); //Can read åäö, but not tested
    } else {
        self.importButton.loadingIndicatorReset();
    }
};

ImportPendingHandler.prototype.importSuccess = function (email) {
    this.increaseCounter();
    this.statusImportSuccessfullCount.text(parseInt(this.statusImportSuccessfullCount.text()) + 1);
    var successElement = $("<div>").text(email);
    this.statusImportSuccessArea.show();
    successElement.appendTo(this.statusImportSuccessRows);
};

ImportPendingHandler.prototype.importFailed = function (email) {
    this.increaseCounter();
    var errorElement = $("<div>").text(email);
    this.statusImportErrorArea.show();
    errorElement.appendTo(this.statusImportFailedRows);
};

ImportPendingHandler.prototype.resetStatus = function () {
    this.statusImportCount.text(0);
    this.statusTotalCount.text(0);
    this.statusImportSuccessfullCount.text(0);
    this.progressBarElement.find("span").css("width", "0");
    
    this.statusImportFailedRows.html("");
    this.statusImportSuccessRows.html("");
    this.statusImportErrorArea.hide();
    this.statusImportSuccessArea.hide();
}

ImportPendingHandler.prototype.increaseCounter = function () {

    this.statusImportCount.text(parseInt(this.statusImportCount.text()) + 1);

    var activeRow = parseInt(this.statusImportCount.text());
    var totalRows = parseInt(this.statusTotalCount.text());

    // Handle progressbar
    var progressWidth = (activeRow / totalRows) * 100;
    this.progressBarElement.find("span").css("width", progressWidth + "%");

    if (progressWidth == 100) {
        this.importDone();
    }
}

ImportPendingHandler.prototype.importDone = function () {
    var self = this;
    self.importButton.loadingIndicatorReset();

    self.ajaxHandler.makeAjaxRequest(
        "/api/biz/getpendingbizsubscribers/" + self.bizSubscriptionId,
        "biz-pending-subscribers-get-success", // Will trigger list handler to update list
        true,
        "biz-invite-fromfile-getpending-failed"); // Not listening to this event
}
