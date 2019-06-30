
var dnnWebAnalytics = angular.module('DNN_WebAnalytics', ['ngMessages', 'ngAnimate', 'ui.bootstrap', 'toastr', 'ui.bootstrap.datetimepicker', 'chart.js'], function ($locationProvider) {
    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
});

dnnWebAnalytics.config(function ($httpProvider) {
    $httpProvider.defaults.withCredentials = true;

    var httpHeaders = {
        "ModuleId": sf.getModuleId(),
        "TabId": sf.getTabId(),
        "RequestVerificationToken": sf.getAntiForgeryValue()
    };
    angular.extend($httpProvider.defaults.headers.common, httpHeaders);
});

dnnWebAnalytics.config(function (toastrConfig) {
    angular.extend(toastrConfig, {
        positionClass: 'toast-top-right',
        timeOut: 3000,
        maxOpened: 1,
        progressBar: true,
        tapToDismiss: true,
        autoDismiss: true,
        toastClass: 'toastr'
    });
});


//dnnWebAnalytics.config(function (uiGmapGoogleMapApiProvider) {
//    uiGmapGoogleMapApiProvider.configure({
//        key: 'AIzaSyBo1a4oXxfBSaUM4yEMvpKWARqyMsA1vD0',
//        v: '3.20',
//        libraries: 'weather,geometry,visualization'
//    });
//});