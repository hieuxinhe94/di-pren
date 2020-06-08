define("epi/shell/command/withConfirmation", [
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/_base/Deferred",
    "epi/shell/widget/dialog/Confirmation"
],
function (declare, lang, Deferred, Confirmation) {

    function defaultConfirmationHandler(settings) {

        var deferred = new Deferred(),
            dialog = new Confirmation(settings);

        dialog.connect(dialog, "onAction", function (confirm) {
            if (confirm) {
                deferred.resolve();
            } else {
                deferred.cancel();
            }
        });

        dialog.show();

        return deferred;
    }

    return function (command, /* function */confirmationHandler, /* Object */settings) {
        // summary:
        //      Adds confirmation before executing a command.
        //
        // tags:
        //      internal xproduct

        // keep original method.
        var originalExecute = command._execute;

        // wrap with a confirmation dialog and return a deferred.
        command._execute = function () {

            function executioner() {
                Deferred.when(originalExecute.apply(command, arguments),
                    commandDeferred.resolve,
                    commandDeferred.cancel);
            }

            var commandDeferred = new Deferred();

            confirmationHandler = confirmationHandler || defaultConfirmationHandler;
            Deferred.when(confirmationHandler(settings), executioner, commandDeferred.cancel);
            return commandDeferred;
        };

        // return the wrapped command.
        return command;
    };
});
