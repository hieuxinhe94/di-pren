define("epi/shell/command/_Command", [
// dojo
    "dojo/_base/declare",

    "dojo/Stateful"
],

function (
// dojo
    declare,

    Stateful
) {

    return declare([Stateful], {
        // summary:
        //      A base class implementation for commands.
        // tags:
        //      public abstract

        // label: [public] String
        //      The action text of the command to be used in visual elements.
        label: null,

        // tooltip: [public] String
        //      The description text of the command to be used in visual elements.
        tooltip: null,

        // iconClass: [public] String
        //      The icon class of the command to be used in visual elements.
        iconClass: null,

        // category: [readonly] String
        //      A category which provides a hint about how the command could be displayed, e.g. "setting".
        category: null,

        // order: [readonly] Integer
        //      An ordering indication used when generating a ui for this command.
        //      Commands with order indication will be placed before commands with no order indication.
        order: null,

        // model: [public] Object
        //      The model which this command will use to determine if it can execute; this may
        //      also be the subject of the execute action.
        model: null,

        // canExecute: [readonly] Boolean
        //      Flag which indicates whether this command is able to be executed.
        canExecute: false,

        // isAvailable: [readonly] Boolean
        //      Flag which indicates whether this command is available in the current context.
        isAvailable: true,

        // _modelChangeWatcher: [private] Object
        //      Watch handler that looking for model value changes
        _modelChangeWatcher: [],

        // =======================================================================
        // Public overrided functions

        postscript: function () {
            // summary:
            //      Watch model and initialize model dependent properties.
            // tags:
            //      public

            this.inherited(arguments);

            this.watchModelChange();

            // Call intial on model change if a model was injected in the constructor.
            if (this.model) {
                this._onModelChange();
            }
        },

        // =======================================================================
        // Public functions

        destroy: function () {
            this.unwatchModelChange();
        },

        execute: function () {
            // summary:
            //      Executes this command if canExecute is true; otherwise do nothing.
            // tags:
            //      public

            if (this.isAvailable && this.canExecute) {
                return this._execute();
            }
        },

        watchModelChange: function () {
            // summary:
            //      Register a watch hanlder when the command change its model
            // tags:
            //      public

            this._modelChangeWatcher = this.watch("model", this._onModelChange);
        },

        unwatchModelChange: function () {
            // summary:
            //      Clear the registered watch handler for command model change
            // tags:
            //      public

            this._modelChangeWatcher && this._modelChangeWatcher.unwatch();
        },

        // =======================================================================
        // Protected functions

        _execute: function () {
            // summary:
            //      Executes this command assuming canExecute has been checked. Subclasses should override this method.
            // tags:
            //      protected
        },

        _onModelChange: function () {
            // summary:
            //      Updates canExecute after the model has been updated. Subclasses should override this method.
            // tags:
            //      protected
        }

    });

});
