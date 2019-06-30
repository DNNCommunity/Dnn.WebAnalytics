dnnWebAnalytics.controller('mapController', ['$scope', '$q', 'toastr', 'mapService', function ($scope, $q, toastr, mapService) {

    $scope.minutes = "10";
    var markersArray = [];

    $scope.visitors_online = 0;
    $scope.users_online = 0;

    var red_flag = "http://maps.google.com/mapfiles/ms/icons/red.png";
    var blue_flag = "http://maps.google.com/mapfiles/ms/icons/blue.png";

    function clearOverlays() {
        for (var i = 0; i < markersArray.length; i++) {
            markersArray[i].setMap(null);
        }
        markersArray = new Array();
    }

    $scope.getMapPoints = function () {
        var deferred = $q.defer();

        var icon;
        clearOverlays();
        $scope.visitors_online = 0;
        $scope.users_online = 0;

        mapService.getMapPoints($scope.minutes).then(
            function (response) {
                $scope.map_points = response.data;

                for (var i = 0; i < $scope.map_points.length; i++) {
                    var map_point = $scope.map_points[i];

                    if (map_point.user_id) {
                        $scope.users_online++;
                        icon = blue_flag;
                    }
                    else {
                        $scope.visitors_online++;
                        icon = red_flag;
                    }

                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(map_point.latitude, map_point.longitude),
                        map: $scope.map,
                        icon: icon
                    });
                    markersArray.push(marker);
                }

                deferred.resolve();
            },
            function (response) {
                console.log('getMapPoints failed', response);
                toastr.error("There was a problem loading the map points", "Error");
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.getMapCenter = function () {
        var deferred = $q.defer();

        mapService.getMapCenter().then(
            function (response) {
                $scope.map_center = response.data;

                deferred.resolve();
            },
            function (response) {
                console.log('getMapCenter failed', response);
                toastr.error("There was a problem loading the map center", "Error");
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.initMap = function () {
        $scope.map = new google.maps.Map(document.getElementById('map'), {
            zoom: 2,
            center: new google.maps.LatLng($scope.map_center.latitude, $scope.map_center.longitude),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });
        $scope.getMapPoints();
    };

    init = function () {
        var promises = [];
        promises.push($scope.getMapCenter());
        promises.push($scope.getMapPoints());
        return $q.all(promises);
    };
    init().then(function () {
        $scope.initMap();
    });
}]);
