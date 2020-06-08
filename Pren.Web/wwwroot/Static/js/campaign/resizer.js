var resizer = {
    resizeStepInput: function (reset, container) {
        container.find("input:not(.norezise)").each(function () {
            if ($(this).val().length) {                
                var width = reset ? "" : $(this).textWidth();
                $(this).css("width", width);

                // Protect element
                if (reset && $(this).hasClass("readonly")) {
                    // only remove readonly attribute if element has class readonly
                    // this is to prevent removal of readonly elements that is set by autoPopulateHandler
                    $(this).removeAttr("readonly");
                } else if ($(this).attr("readonly") == undefined) {
                    // only add readonly if not already there
                    $(this).addClass("readonly");
                    $(this).attr("readonly", "readonly");
                }
            } else {
                reset ? $(this).show() : $(this).hide();
            }
        });
    },
}