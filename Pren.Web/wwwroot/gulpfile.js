/// <binding AfterBuild='r-optimizeSurvey, r-optimizePren' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/


var gulp = require('gulp');
var rjs = require('requirejs');
var replace = require('gulp-replace');

gulp.task('r-optimizePren', function (cb) {

    var config = {
        baseUrl: "../wwwroot/Static/js",
        name: "mysettings/app",
        inlineJSON: false,
        mainConfigFile: "../wwwroot/Static/js/mysettings/app.js",
        out: "../wwwroot/Static/js/bundles/mysettings-min.js"
    };

    rjs.optimize(config, function (buildResponse) {
        // console.log('build response', buildResponse);
        cb();
    }, cb);
});

gulp.task('fix-orderflow-screen-css', function () {
    gulp.src(['../wwwroot/Static/css/orderflow/css/screen.css'])
        .pipe(replace('../images/', '/static/css/orderflow/images/'))
        .pipe(replace('../fonts/', '/static/css/orderflow/fonts/'))
        .pipe(gulp.dest('../wwwroot/Static/css/orderflow/css/'));
});

gulp.task('requireWatch', function () {
    gulp.watch(["../wwwroot/Static/js/mysettings/**/*.js"], ['r-optimizePren']);
});