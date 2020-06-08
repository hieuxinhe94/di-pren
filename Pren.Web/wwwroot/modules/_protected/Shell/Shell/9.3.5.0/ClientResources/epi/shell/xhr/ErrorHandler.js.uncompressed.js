define("epi/shell/xhr/ErrorHandler", [
    "dojo/_base/declare",
    "dojo/_base/json",
    "dgrid/List",
    "dgrid/extensions/DijitRegistry",
    "../widget/dialog/Alert",
    "../widget/dialog/ErrorDialog"
], function (declare, json, List, DijitRegistry, Alert, ErrorDialog) {

    var _parseResponseText = function (responseData) {
        if (typeof responseData === "string") {
            try {
                return json.fromJson(responseData);
            } catch (e) {
                console.warn("Failed to parse responseText as json", e);
            }
        }
        return null;
    };

    var handler = {

        forXhr: function (response) {
            // summary:
            //      Shows an alert dialog with the response message from a failed xhr result.
            //      If it's a 5xx error then the entire body is shown, otherwise the message
            //      is shown in an Alert dialog.
            //
            // tags:
            //      private

            if (response.status >= 500) {
                ErrorDialog.showXmlHttpError(response, response);
            } else {
                var data = _parseResponseText(response.responseText) || {};
                var message = data.message || response.message;

                var settings = { description: message };

                // If there is additional information for the error then display it as a
                // list within the alert dialog.
                if (data.additionalInformation instanceof Array) {
                    var list = new (declare([List, DijitRegistry]))({ className: "epi-grid-max-height--300" });
                    list.renderArray(data.additionalInformation);
                    list.startup();

                    settings.content = list;
                }

                var dialog = new Alert(settings);

                dialog.show();
            }

            return response;
        }
    };

    return handler;

});
