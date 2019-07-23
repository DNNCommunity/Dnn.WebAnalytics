dnnWebAnalytics.controller('viewController', ['$scope', '$q', 'toastr', '$uibModal', '$filter', 'visitService', 'visitorService', function ($scope, $q, toastr, $uibModal, $filter, visitService, visitorService) {

    $scope.period_start;
    $scope.period_end;

    $scope.view_count = 0;
    $scope.visit_count = 0;
    $scope.visitor_count = 0;
    $scope.user_count = 0;

    $scope.chart_labels = [];
    $scope.chart;

    $scope.field = "date";
    $scope.rows = "10";

    $scope.periodStartDatePicker = {
        isOpen: false
    };
    $scope.periodEndDatePicker = {
        isOpen: false
    };

    $scope.chart_series = ['Views', 'Visits', 'Visitors', 'Users'];
    $scope.chart_labels = [];
    $scope.chart_data = [];
    $scope.chart_options = {
        scales: {
            yAxes: [
                {
                    id: 'y-axis-1',
                    type: 'linear',
                    display: true,
                    position: 'left'
                }
            ]
        },
        legend: {
            display: true,
            position: "right"
        }
    };

    $scope.pie_chart_labels = [];
    $scope.pie_chart_data = [];
    $scope.pie_chart_options = {
        legend: {
            display: true,
            position: "right"
        }
    };

    $scope.lastWeek = function () {
        $scope.period_end = new Date();
        $scope.period_start = new Date();
        $scope.period_start.setDate($scope.period_end.getDate() - 7);

        $scope.getDashboard();
        $scope.getReport();
    };
    $scope.lastMonth = function () {
        $scope.period_end = new Date();
        $scope.period_start = new Date();
        $scope.period_start.setDate($scope.period_end.getDate() - 30);

        $scope.getDashboard();
        $scope.getReport();
    };
    $scope.last3Months = function () {
        $scope.period_end = new Date();
        $scope.period_start = new Date();
        $scope.period_start.setDate($scope.period_end.getDate() - 90);

        $scope.getDashboard();
        $scope.getReport();
    };

    $scope.getDashboard = function () {
        var deferred = $q.defer();

        $scope.dashboard_loading = true;

        visitService.getDashboard(portal_id, $scope.period_start, $scope.period_end).then(
            function (response) {
                var dashboardDTO = response.data;

                console.log(dashboardDTO);

                $scope.view_count = dashboardDTO.view_count;
                $scope.visit_count = dashboardDTO.visit_count;
                $scope.visitor_count = dashboardDTO.visitor_count;
                $scope.user_count = dashboardDTO.user_count;

                $scope.views = dashboardDTO.views;
                $scope.visits = dashboardDTO.visits;
                $scope.visitors = dashboardDTO.visitors;
                $scope.users = dashboardDTO.users;

                var current_date = new Date($scope.period_start.getFullYear(), $scope.period_start.getMonth(), $scope.period_start.getDate());
                var end_date = new Date($scope.period_end.getFullYear(), $scope.period_end.getMonth(), $scope.period_end.getDate());
                var labels = [];
                while (current_date <= end_date) {
                    labels.push($filter('date')(current_date, 'shortDate'));
                    current_date.setDate(current_date.getDate() + 1);
                }

                $scope.chart_series = [
                    'Views (' + $scope.view_count + ')',
                    'Visits (' + $scope.visit_count + ')',
                    'Visitors (' + $scope.visitor_count + ')',
                    'Users (' + $scope.user_count + ')'
                ];

                $scope.chart_labels = labels;
                $scope.chart_data = [$scope.views, $scope.visits, $scope.visitors, $scope.users];
                
                $scope.dashboard_loading = false;
            },
            function (response) {
                $scope.dashboard_loading = false;
                console.log('getDashboard failed', response);
                toastr.error("There was a problem loading the dashboard", "Error");
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.getReport = function () {
        var deferred = $q.defer();

        $scope.report_loading = true;

        visitService.getReport($scope.field, portal_id, $scope.period_start, $scope.period_end, $scope.rows).then(
            function (response) {
                $scope.report_rows = response.data;

                //console.log('get report', $scope.report_rows);

                $scope.pie_chart_labels = [];
                $scope.pie_chart_data = [];

                for (var x = 0; x < $scope.report_rows.length; x++) {
                    var report_row = $scope.report_rows[x];

                    if (report_row.field.length > 30) {
                        $scope.pie_chart_labels.push(report_row.field.substring(0, 30) + "...");
                    }
                    else {
                        $scope.pie_chart_labels.push(report_row.field);
                    }

                    $scope.pie_chart_data.push(report_row.count);
                }

                $scope.report_loading = false;
            },
            function (response) {
                $scope.report_loading = false;
                console.log('getReport failed', response);
                toastr.error("There was a problem loading the report", "Error");
                deferred.reject();
            }
        );
        return deferred.promise;
    };

    $scope.$on('chart-create', function (evt, chart) {
        //console.log('chart-create', chart);
        if (chart.canvas.id === 'line') {
            //console.log('before create', $scope.report_rows);
            $scope.chart = chart;
            $scope.chart.update();
            //console.log('after create', $scope.report_rows);
        }
    });

    init = function () {
        var promises = [];
        return $q.all(promises);
    };
    init();
    $scope.lastWeek();
}]);

