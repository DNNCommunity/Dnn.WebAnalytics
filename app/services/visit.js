dnnWebAnalytics.factory('visitService', ['$http', '$filter', function visitService($http, $filter) {

    var base_path = apiUrlBase + "/visit";

    // interface
    var service = {
        list: list,
        get: get,
        insert: insert,
        update: update,
        remove: remove,
        save: save,
        getDashboard: getDashboard,
        getReport: getReport
    };
    return service;

    // implementation    

    // list
    function list() {
        var request = $http({
            method: "get",
            url: base_path
        });
        return request;
    }

    // get 
    function get(id) {
        var request = $http({
            method: "get",
            url: base_path + '/' + id
        });
        return request;
    }

    // insert
    function insert(item) {
        var request = $http({
            method: "post",
            url: base_path,
            data: item
        });
        return request;
    }

    // update 
    function update(item) {
        var request = $http({
            method: "put",
            url: base_path,
            data: item
        });
        return request;
    }

    // delete
    function remove(id) {
        var request = $http({
            method: "delete",
            url: base_path + '/' + id
        });
        return request;
    }

    // save
    function save(item) {
        if (item.id > 0) {
            return update(item);
        }
        else {
            return insert(item);
        }
    }

    // get dashboard
    function getDashboard(portal_id, period_start = null, period_end = null) {

        if (period_start) {
            period_start = $filter('date')(period_start, 'MM/dd/yyyy');
        }
        if (period_end) {
            period_end = $filter('date')(period_end, 'MM/dd/yyyy');
        }

        var request = $http({
            method: "get",
            url: base_path + '?portal_id=' + portal_id + '&period_start=' + period_start + '&period_end=' + period_end
        });
        return request;
    }

    // get report
    function getReport(field, portal_id, period_start = null, period_end = null, rows) {

        if (period_start) {
            period_start = $filter('date')(period_start, 'MM/dd/yyyy');
        }
        if (period_end) {
            period_end = $filter('date')(period_end, 'MM/dd/yyyy');
        }

        var request = $http({
            method: "get",
            url: base_path + '?field=' + field + '&portal_id=' + portal_id + '&period_start=' + period_start + '&period_end=' + period_end + '&rows=' + rows
        });
        return request;
    }

}]);
