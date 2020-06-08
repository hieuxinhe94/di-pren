
requirejs.config({
    baseUrl: '/static/js',
    paths: {
        app: 'mysettings/app',
        func: 'mysettings/functions',
        classes: 'mysettings/classes',

        dom: 'mysettings/app/dom',
        faqdom:'mysettings/app/faq/dom',
        profdom: 'mysettings/app/profile/dom',
        subdom: 'mysettings/app/subscription/dom',
        mycodesdom: 'mysettings/app/mycodes/dom',

        profileTemplates: 'mysettings/app/profile/templates',
        subscriptionTemplates: 'mysettings/app/subscription/templates',
        faqTemplates: 'mysettings/app/faq/templates',
        myCodesTemplates: 'mysettings/app/mycodes/templates',
        
        ext: 'lib/extensions',
        jquery: 'lib/jquery-1.11.2.min',
        bootstrap: 'lib/bootstrap.min',
        jqueryvalidate: 'lib/jquery.validate.min',
        datepicker: 'lib/datepicker/bootstrap-datepicker',
        datepickerSv: 'lib/datepicker/locales/bootstrap-datepicker.sv',
        underscore: 'lib/underscore-min',
        pubsub: 'lib/tiny-pubsub',

        domReady: 'lib/require/plugins/domReady',
        text: 'lib/require/plugins/text',
        json: 'lib/require/plugins/json'

    },
    shim: {
        'bootstrap': {
            deps: ['jquery'],
        },
        'datepicker': {
            deps: ['jquery', 'bootstrap']
        },
        'datepickerSv': {
            deps: ['jquery', 'bootstrap', 'datepicker']
        },
        'pubsub':{
            deps: ['jquery'],
        },
        // Make sure components for loading and errorhandling is loaded before loading of main.
        'app/main': {
            deps: ['pubsub', 'func/load', 'func/error', 'func/tracking']
        }
    }
});

requirejs(['app/main']);



