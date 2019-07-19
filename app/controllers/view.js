dnnWebAnalytics.controller('viewController', ['$scope', '$q', 'toastr', '$uibModal', '$filter', 'visitService', 'visitorService', function ($scope, $q, toastr, $uibModal, $filter, visitService, visitorService) {

    $scope.period_start;
    $scope.period_end;

    $scope.view_count = 0;
    $scope.visit_count = 0;
    $scope.visitor_count = 0;
    $scope.user_count = 0;

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
    };
    $scope.lastMonth = function () {
        $scope.period_end = new Date();
        $scope.period_start = new Date();
        $scope.period_start.setDate($scope.period_end.getDate() - 30);

        $scope.getDashboard();
    };
    $scope.last3Months = function () {
        $scope.period_end = new Date();
        $scope.period_start = new Date();
        $scope.period_start.setDate($scope.period_end.getDate() - 90);

        $scope.getDashboard();
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

                $scope.chart_series = [
                    'Views (' + $scope.view_count + ')',
                    'Visits (' + $scope.visit_count + ')',
                    'Visitors (' + $scope.visitor_count + ')',
                    'Users (' + $scope.user_count + ')'
                ];

                var current_date = new Date($scope.period_start.getFullYear(), $scope.period_start.getMonth(), $scope.period_start.getDate());
                var end_date = new Date($scope.period_end.getFullYear(), $scope.period_end.getMonth(), $scope.period_end.getDate());

                $scope.chart_labels = [];

                var views = [];
                var visits = [];
                var visitors = [];
                var users = [];

                while (current_date <= end_date) {
                    $scope.chart_labels.push($filter('date')(current_date, 'shortDate'));

                    var view_data = null;
                    for (var view_index = 0; view_index < dashboardDTO.views.length; view_index++) {
                        var view = dashboardDTO.views[view_index];
                        var view_date = new Date(view.date);
                        if (+view_date === +current_date) {
                            view_data = view;
                        }
                    }
                    if (view_data) {
                        views.push(view_data.count);
                    }
                    else {
                        views.push(0);
                    }

                    var visit_data = null;
                    for (var visit_index = 0; visit_index < dashboardDTO.visits.length; visit_index++) {
                        var visit = dashboardDTO.visits[visit_index];
                        var visit_date = new Date(visit.date);
                        if (+visit_date === +current_date) {
                            visit_data = visit;
                        }
                    }
                    if (visit_data) {
                        visits.push(visit_data.count);
                    }
                    else {
                        visits.push(0);
                    }

                    var visitor_data = null;
                    for (var visitor_index = 0; visitor_index < dashboardDTO.visitors.length; visitor_index++) {
                        var visitor = dashboardDTO.visitors[visitor_index];
                        var visitor_date = new Date(visitor.date);
                        if (+visitor_date === +current_date) {
                            visitor_data = visitor;
                        }
                    }
                    if (visitor_data) {
                        visitors.push(visitor_data.count);
                    }
                    else {
                        visitors.push(0);
                    }

                    var user_data = null;
                    for (var user_index = 0; user_index < dashboardDTO.users.length; user_index++) {
                        var user = dashboardDTO.users[user_index];
                        var user_date = new Date(user.date);
                        if (+user_date === +current_date) {
                            user_data = user;
                        }
                    }
                    if (user_data) {
                        users.push(user_data.count);
                    }
                    else {
                        users.push(0);
                    }

                    current_date.setDate(current_date.getDate() + 1);
                }
                $scope.chart_data = [views, visits, visitors, users];

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

                console.log($scope.report_rows);

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
        if (chart.id === 'line') {
            $scope.chart = chart;
            $scope.chart.update();
        }
    });

    init = function () {
        var promises = [];
        return $q.all(promises);
    };
    init();
    $scope.lastWeek();
    $scope.getReport();

}]);

