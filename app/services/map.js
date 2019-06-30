dnnWebAnalytics.factory('mapService', ['$http', function mapService($http) {

    var base_path = "/api/Dnn.WebAnalytics/map";

    // interface
    var service = {
        getMapPoints: getMapPoints,
        getMapCenter: getMapCenter
    };
    return service;

    // implementation    

    // get Map Points
    function getMapPoints(minutes) {
        var request = $http({
            method: "get",
            url: base_path + "?minutes=" + minutes
        });
        return request;
    }

    // get Map Center
    function getMapCenter() {
        var request = $http({
            method: "get",
            url: base_path
        });
        return request;
    }
}]);
