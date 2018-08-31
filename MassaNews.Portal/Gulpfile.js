'use strict';

// Include Gulp & tools we'll use
var gulp = require('gulp');
var $ = require('gulp-load-plugins')();
var del = require('del');
var runSequence = require('run-sequence');
var browsersync = require('browser-sync');
var concat = require('gulp-concat');
var watch = require('gulp-watch');
var jshint = require('gulp-jshint');
var svgo = require('gulp-svgo');
var cleanCSS = require('gulp-clean-css');

var paths = {
  scripts        : ['Assets/scripts/**/*.js'],
  scriptsmodules : ['Assets/scripts/modules/*.js'],
  styles         : ['Assets/styles/*.{scss,css}', 'Assets/styles/**/*.{scss,css}', 'Assets/styles/**/*.css']
};

var AUTOPREFIXER_BROWSERS = [
  'ie >= 9',
  'ie_mob >= 10',
  'ff >= 30',
  'chrome >= 34',
  'safari >= 7',
  'opera >= 23',
  'ios >= 7',
  'android >= 4.0',
  'bb >= 10'
];

// Clean files
gulp.task('clean', function() {
  return del(['Content/*']);
});

// BrowserSync proxy
gulp.task('browser-sync', function() {
  browsersync({
    proxy: 'localhost',
    port: 51175
  });
});

// BrowserSync reload all Browsers
gulp.task('browsersync-reload', function () {
  browsersync.reload();
});

// Copy all files at the root level (app)
gulp.task('copy', function () {
  return gulp.src([
    // Ignore
    '!Assets/styles/*',
    '!Assets/styles/**/*',
    '!Assets/scripts/*',
    '!Assets/scripts/**/*',
    '!Assets/images/*',
    '!Assets/images/**/*',

    'Assets/*',
    'Assets/**/*'
  ], {
    buffer: false,
    dot: true
  }).pipe(gulp.dest('Content'));
});

// Compile and automatically prefix stylesheets
gulp.task('styles', function () {
  return gulp.src(paths.styles)
  .pipe($.plumber({ errorHandler: $.notify.onError('Error: <%= error.message %>') }))
  //.pipe($.sourcemaps.init())
  .pipe($.changed('Content/styles'))
  .pipe($.sass({ precision: 10 }))
  .pipe($.cssimport())
  .pipe($.autoprefixer({ browsers: AUTOPREFIXER_BROWSERS }))
  //.pipe($.if('*.css', $.minifyCss()))
  .pipe($.if('*.css', cleanCSS({compatibility: AUTOPREFIXER_BROWSERS, keepSpecialComments: false})))
  //.pipe($.sourcemaps.write())
  .pipe(gulp.dest('Content/styles'))
  .pipe($.size({title: 'styles'}));
});

// Images
gulp.task('images', function() {
  gulp.src('Assets/images/**/*')
    .pipe(svgo())
    .pipe(gulp.dest('Content/images/'));
});

// Lint JS task
gulp.task('jslint', function() {
  return gulp.src(['Assets/scripts/components/*.js', 'Assets/scripts/application/*.js', 'Assets/scripts/modules/*.js'])
    .pipe(jshint())
    .pipe(jshint.reporter('default'));
    //.pipe(jshint.reporter('fail'));
    //.pipe($.notify({ message: 'Lint task complete' }));
});

// Scripts JS
gulp.task('scripts', ['scriptsmodules'/*, 'jslint'*/], function() {
  return gulp.src(['Assets/scripts/vendor/*.js', 'Assets/scripts/components/*.js', 'Assets/scripts/application/*.js'])
  .pipe(concat('application.js'))
  .pipe($.uglify())
  .pipe(gulp.dest('Content/scripts/'));
});

// JS (Modules)
gulp.task('scriptsmodules', function() {
  return gulp.src(paths.scriptsmodules)
  .pipe($.uglify())
  .pipe(gulp.dest('Content/scripts/modules'));
});

// Watch
gulp.task('watch', function() {
  //gulp.watch(paths.scripts, ['scripts']);
  gulp.watch(paths.styles, ['styles']);
});

// Watch with sync
gulp.task('watchsync', ['browser-sync'], function() {
  //gulp.watch(paths.scripts, ['scripts', 'browsersync-reload']);
  gulp.watch(paths.styles, ['styles', 'browsersync-reload']);
});

// Build production files, the default task
gulp.task('default', function (cb) {
  //runSequence('clean', ['styles'], ['scripts', 'copy'], cb);
  runSequence('clean', ['styles'], ['images', 'copy'], cb);
});
