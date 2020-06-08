
function validate(validationgroup) {

    var isValid = Page_ClientValidate(validationgroup);
    var validationclass = "." + validationgroup;

    if (!isValid) {                
        $(validationclass + " .validation").show();
        var catTopPosition = jQuery(validationclass + ' .validation').offset().top;
        // Scroll down to 'catTopPosition'
        jQuery('html, body').animate({ scrollTop: catTopPosition }, 'slow');
    }
    else {
        //hide validation summary
        $(validationclass + " .validation").hide();
    }

}

function SetUpValidation(validationgroup) {
    var validationclass = "." + validationgroup;

    //************* Form validation
    var submit;
    $(validationclass + " .btnsubmitform").on("click", function () {
        submit = true;
        $(validationclass + " .validateinput").each(function (e) {
            validate(this);
        });
    });

    //blur on text boxes
    $(validationclass + " .validateinput input[type=text]").on("blur", function () {
        if (submit) {
            validate($(this).parents(".validateinput"));
        }
    });

    function validate(obj) {
        //get validators
        var validator = $(obj).find(".val");
        var valreg = $(obj).find(".valreg");
        //both req and reg validation on input
        if ($(validator).length && $(valreg).length) {
            var regVisiblity = $(validator).css("visibility");
            var reqVisiblity = $(valreg).css("visibility");
            if (regVisiblity == "hidden" && reqVisiblity == "hidden") {
                setClasses(obj, "hidden");
            }
            else {
                setClasses(obj, "visible");
            }
            return true;
        }
        //only req validation on input
        if ($(validator).length) {
            var validatorVisiblity = $(validator).css("visibility");
            setClasses(obj, validatorVisiblity);
            return true;
        }
        //only reg validation on input
        if ($(valreg).length) {
            var validatorVisiblity = $(valreg).css("visibility");
            setClasses(obj, validatorVisiblity);
            return true;
        }
    }

    function setClasses(obj, validatorVisiblity) {
        if (validatorVisiblity == "hidden") {
            $(obj).removeClass("errorval");
            $(obj).addClass("okval");
        }
        else {
            $(obj).addClass("errorval");
            $(obj).removeClass("okval");
        }

    }
    //************* end validation
}