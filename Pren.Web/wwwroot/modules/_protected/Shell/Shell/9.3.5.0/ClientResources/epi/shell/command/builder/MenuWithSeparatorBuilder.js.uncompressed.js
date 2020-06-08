define("epi/shell/command/builder/MenuWithSeparatorBuilder", [
    "dojo/_base/array",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/on",

    "dijit/MenuSeparator",
    "./MenuBuilder"
], function (array, declare, lang, on, MenuSeparator, MenuBuilder) {

    return declare([MenuBuilder], {
        // summary:
        //      Builds a context menu with separator.
        //
        // tags:
        //      internal

        _addToContainer: function (widget, container) {
            // summary:
            //		Adds the widget to the container.
            // tags:
            //		protected

            var separator = new MenuSeparator();
            container.addChild(widget);
            container.addChild(separator);
        }
    });
});
