define("epi/shell/command/builder/_Builder", [
    "dojo/_base/array",
    "dojo/_base/declare",
    "dojo/_base/lang"
], function (array, declare, lang) {

    return declare(null, {
        // summary:
        //      Builds a widget and connects events based on a command object.
        //
        // tags:
        //      internal xproduct

        // settings: [public] Object
        //		An optional settings object that will be applied to all widgets created.
        settings: null,

        postscript: function (mixin) {
            // summary:
            //		Mix constructor arguments into the object after construction.
            // tags:
            //		protected

            if (mixin) {
                lang.mixin(this, mixin);
            }
        },

        create: function (command, container) {
            // summary:
            //		Creates a widget from the given command and adds it to the given container.
            // tags:
            //		public

            var widget = this._create(command);

            // Create back-reference to command so that we can identify this widget.
            widget._command = command;
            widget._commandCategory = command.category;
            this._addToContainer(widget, container);
        },


        remove: function (command, container) {
            // summary:
            //      Remove the ui representation of a specific command from a container
            // command: epi/shell/command/_Command
            //      The command to remove
            // container: dijit/_Widget
            //      The container displaying the command
            // tags:
            //      public

            var children = container.getChildren(),
                len = children.length,
                i = 0;

            for (; i < len; i++) {
                var child = children[i];
                if (child._command === command) {
                    container.removeChild(child);
                    child.destroy();
                    return true;
                }
            }
            return false;
        },

        _create: function (/*===== command =====*/) {
            // summary:
            //		Builds a widget from the given command. Subclasses should override this method.
            // tags:
            //		protected
        },

        _addToContainer: function (widget, container) {
            // summary:
            //		Adds the widget to the container.
            // tags:
            //		protected
            var insertIndex = null;

            if (container.getChildren && widget._command && widget._command.order) {
                var order = widget._command.order;
                array.some(container.getChildren(), function (child, index) {
                    var childOrder = child._command && child._command.order;
                    if (!childOrder || childOrder > order) {
                        insertIndex = index;
                        // Break out of some
                        return true;
                    }
                });
            }

            if (insertIndex !== null) { // May be 0
                container.addChild ? container.addChild(widget, insertIndex) : widget.placeAt(container);
            } else {
                container.addChild ? container.addChild(widget) : widget.placeAt(container);
            }

        }
    });
});
