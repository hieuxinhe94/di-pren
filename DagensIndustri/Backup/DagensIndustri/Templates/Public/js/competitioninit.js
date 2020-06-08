$(document).ready(function () {

    SetUpImageSlider();

    SetUpDialogEvents();

    SetUpValidation();

});

function SetUpImageSlider() 
{

    $("#showcase").awShowcase(
	        {
	            content_width: 968,
	            fit_to_parent: false,
	            auto: true,
	            interval: 5000,
	            continuous: true,
	            loading: true,
	            arrows: false,
	            buttons: false,
	            btn_numbers: true,
	            keybord_keys: true,
	            mousetrace: false, /* Trace x and y coordinates for the mouse */
	            pauseonover: true,
	            stoponclick: false,
	            transition: 'fade', /* hslide/vslide/fade */
	            transition_delay: 0,
	            transition_speed: 1000,
	            show_caption: 'onload', /* onload/onhover/show */
	            thumbnails: false,
	            dynamic_height: true, /* For dynamic height to work in webkit you need to set the width and height of images in the source. Usually works to only set the dimension of the first slide in the showcase. */
	            speed_change: true, /* Set to true to prevent users from swithing more then one slide at once. */
	            viewline: false, /* If set to true content_width, thumbnails, transition and dynamic_height will be disabled. As for dynamic height you need to set the width and height of images in the source. */
	            custom_function: null /* Define a custom function that runs on content change */
	        });

}


function SetUpValidation() 
{

    //************* Form validation
    var submit;
    $(".submitbtn").live("click", function () {
        submit = true;
        $(".inputfield").each(function (e) {
            validate(this);
        });

        if (!Page_IsValid) {
            $("#notvalid").show("fast");
        }
    });

    //blur on text boxes
    $(".inputfield input[type=text]").live("blur", function () {
        if (submit) {
            validate($(this).parents(".inputfield"));
        }
    });
    //click on radio buttons
    $(".inputfield input[type=radio]").live("click", function () {
        if (submit) {
            validate($(this).parents(".inputfield"));
        }
    });

    function validate(obj) {
        var validator = $(obj).find(".val");
        if ($(validator).length) {
            var validatorVisiblity = $(validator).css("visibility");

            if (validatorVisiblity == "hidden") {
                $(obj).removeClass("errorval");
                $(obj).addClass("okval");
            }
            else {
                $(obj).addClass("errorval");
                $(obj).removeClass("okval");
            }
        }
    }
    //************* end validation

}

function SetUpDialogEvents() {
    $(".about").click(function () {
        $("#aboutdialog").dialog({ modal: true, width: 'auto' });
    });

    $(".rules").click(function () {
        $("#rulesdialog").dialog({ modal: true, width: 'auto' });
    });
}

//Used by inputfield to handle default text inside textbox
function check(obj) {

    if (obj.value == "") {
        obj.value = obj.title;
    }
    else if (obj.value == obj.title) {
        obj.value = "";
    }
}

//Shows progress bar
function showProgress(formid) {

    Page_ClientValidate();

    if (Page_IsValid) {
        //must reset src in image after postback, otherwise IE will stop animation
        ProgressImg = document.getElementById('progressimg');
        setTimeout("ProgressImg.src = ProgressImg.src", 100);

        $("#progress").dialog({ width: 200, height: 120, modal: true });
        //hide title bar
        $(".ui-dialog-titlebar").hide();
    }
    else {

        //var catTopPosition = jQuery('#leftarea').offset().top;
        $(formid).dialog({ width: 'auto', modal: true });
        //, position: ['top', catTopPosition] }
        // Scroll down to 'catTopPosition'
        //jQuery('html, body').animate({ scrollTop: catTopPosition }, 'slow');                                                
        // Stop the link from acting like a normal anchor link                
    }
}	 