
/**************************************************************/
/*                                                            */
/* Form validator by Queensbridge                             */
/*                                                            */
/**************************************************************/
(function ($) {

    // Set default values
    var defaults = {};

    var methods = {

        // Init method builds the Tickster
        init: function (options) {
            var obj = $(this);
            //var settings = $.extend({}, defaults, options);
            var formElements = obj.find('input,select');

            if (formElements.length == 0) { return false; }

            formElements.each(function () {
                if ($(this).attr('required')) {
                    $(this).parent().append('<span class="field-status" />');
                }
            });

            obj.submit(function (e) {
                return methods.isValid.call(this);
            });

        },

        isValid: function () {
            var obj = $(this);
            var formElements = obj.find('input,select');
            var formValid = true;
            var firstInvalidField = null;
            obj.find('div.form-field-invalid').removeClass('form-field-invalid');
            obj.find('div.field-msg-error').remove();

            var pswd = obj.find('input[type=password]').eq(1);

            formElements.each(function () {

                // CHeck if field is visible
                if ($(this).is(':visible') && $(this).attr('required')) {

                    if ($(this).is(':checkbox')) {
                        if (!$(this).is(':checked')) {
                            $(this).parent().parent().addClass('form-field-invalid');
                            formValid = false;
                            firstInvalidField = $(this);
                        }
                    }

                    // Check Password validator
                    if ($(this).data('inputtype') == 'pswvalidator') {
                        var pswValue = obj.find('input[data-inputtype=psworiginal]').val();
                        if ($(this).val() != pswValue) {
                            $(this).parent().addClass('form-field-invalid');
                            formValid = false;
                            firstInvalidField = $(this);
                        }

                    } else if (this == pswd.get(0)) {
                        var pw1 = obj.find('input[type=password]').eq(0);
                        var pw2 = obj.find('input[type=password]').eq(1);

                        if (pw1.val() != pw2.val()) {
                            pw1.parent().addClass('form-field-invalid');
                            pw2.parent().addClass('form-field-invalid');
                            formValid = false;
                            firstInvalidField = pw2;
                        }
                    } else  {

                        // Check pattern	
                        if ($(this).attr('pattern')) {
                            var regEx = new RegExp($(this).attr('pattern'));
                            if ($(this).val().search(regEx) == -1) {
                                $(this).parent().addClass('form-field-invalid');
                                formValid = false;
                                firstInvalidField = $(this);
                            }
                        } else {

                            // Check required
                            if ($(this).attr('required') == true && $(this).val() == '') {
                                $(this).parent().addClass('form-field-invalid');
                                formValid = false;
                                firstInvalidField = $(this);
                            }
                        }
                    }

                    return formValid; // Break loop if false

                }
            });

            // Don't send if invalid
            if (formValid) { return true; }
            else {
                var errorTxt = firstInvalidField.data("error");
                if (typeof (errorTxt) === 'undefined') { errorTxt = 'Kontrollera fältet.'; }
                firstInvalidField.after('<div class="field-msg-error"><p>' + errorTxt + '</p></div>');
                $('html,body').animate({
                    scrollTop: $('#main div.form-field-invalid:first').position().top
                }, 250);
                return false;
            }
        }
    }

    $.fn.formValidator = function (method) {

        var args = arguments;

        if (method == 'isValid') {
            return methods[method].apply(this, Array.prototype.slice.call(args, 1));
        }

        return this.each(function () {

            // Method calling logic
            if (methods[method]) {
                return methods[method].apply(this, Array.prototype.slice.call(args, 1));
            } else if (typeof method === 'object' || !method) {
                return methods.init.apply(this, args);
                //return methods.init.apply( this, new Array(method) );
            } else {
                return false;
            }

        });

    }

})(jQuery);

/* Author: Queensbridge.se

*/

$(document).ready(function () { diPlus.init(); });

var diPlus = {

    init: function () {

        diPlus.forms.init();
        diPlus.selectivzr();
        diPlus.layers();

    },

    layers: function () {

        $('#main div.layer').each(function () {
            $(this).append($('<a/>', {
                'href': '#',
                'class': 'btn-close',
                'click': function () { $(this).parent().hide(); return false; }
            }));
            console.log(($(this).width() / 2) * -1);
            $(this).css({
                marginLeft: ($(this).width() / 2) * -1
            });
        });

        $('#main a.layer-opener').click(function () {
            var id = ($(this).attr('href').split('#'))[1];
            $('#' + id).show();
            return false;
        });

        $(document).click(function () { $('#main div.layer').hide(); });

    },

    selectivzr: function () {

        $('#main div.payment-method:first-child').addClass('first-child');

    },

    forms: {

        init: function () {
            diPlus.forms.styleSubmits();
            $('form').formValidator();
            diPlus.forms.interactiveElements();
        },

        styleSubmits: function () {
            $('#main input[type="submit"]').each(function (i) {
                var cssClass = ($(this).attr('class') != 'undefined') ? $(this).attr('class') : '';
                /*
                $(this).before($('<a />', {
                'class': 'btn ' + cssClass,
                'data': {'submit' : this},
                'click': function() { $(this).siblings('input[type="submit"]').trigger('click') },
                'html': '<span>' + $(this).val() + '</span>'
                }));
                */

                $(this).after('<a href="#" class="btn"><span>' + $(this).val() + '</span></a>');
                $(this).next('a:first').click(function () {
                    if ($('form').formValidator('isValid')) {
                        $(this).siblings('input[type="submit"]').trigger('click');
                    }
                    return false;
                });

                $(this).addClass('removed');
            });
        },

        interactiveElements: function () {
            $('#payment-time-radios input[type="radio"]').click(function () {
                var v = parseInt($('#payment-time-radios input[type="radio"]:checked').data('time'));
                if (v == 12) {
                    $('#payment-time-year').show();
                    $('#payment-time-month').hide();
                } else {
                    $('#payment-time-year').hide();
                    $('#payment-time-month').show();
                }
            });
        }
    }
}

$(function () {

    $('.info-links a, .form-field a').bind('click', function (e) {
        var popup = $(this).data('popup');

        $(popup).show().siblings().hide();
        $('.popups').fadeIn('fast');
        $('.popup-overlay').show();

        return false;
    });

    $('.popup').bind('click', function (e) {
        e.stopPropagation();
    });

    $('.popups').bind('click', function (e) {
        $(this).fadeOut('fast');
        $('.popup-overlay').hide();
    });

    $('.close').bind('click', function (e) {
        $('.popups').fadeOut('fast');
        $('.popup-overlay').hide();
    });
});
