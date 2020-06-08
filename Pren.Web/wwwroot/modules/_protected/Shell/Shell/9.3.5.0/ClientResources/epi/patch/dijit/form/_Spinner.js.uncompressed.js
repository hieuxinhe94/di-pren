define("epi/patch/dijit/form/_Spinner", [
    "dojo/_base/lang",
    "dijit/form/_Spinner"
], function (lang, _Spinner) {
    // summary:
    //      Monkey patch mouse wheel scrolling so it doesn't grab focus when scrolling over the input field,
    //      otherwise the spinner value starts scrolling instead of the document.
    //
    //      https://bugs.dojotoolkit.org/ticket/18300

    var base = _Spinner.prototype._mouseWheeled;

    lang.mixin(_Spinner.prototype, {

        _mouseWheeled: function (/*Event*/ evt) {
            // summary:
            //      Do an early exit if spinner is not focused, otherwise call the base implementation.

            if (!this.get("focused")) {
                return;
            }

            return base.apply(this, arguments);
        }

    });

    _Spinner.prototype._mouseWheeled.nom = "_mouseWheeled";
});
