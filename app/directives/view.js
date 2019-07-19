dnnWebAnalytics.directive('view', function () {
    return {
        templateUrl: '/DesktopModules/Dnn.WebAnalytics/app/views/view.html?v=' + Date.now(),
        controller: 'viewController'
    };
});