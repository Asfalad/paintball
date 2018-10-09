angular.module('pbCompany').
    component('pbCompany', {
        templateUrl: '/app/company/read',
        controller: ['$scope', '$rootScope', 'Notification', 'Companies', '$location', '$q',
            'GameTypes', 'Playgrounds',
            function pbCompanyController($scope, $rootScope, Notification, Companies, $location, $q,
                GameTypes, Playgrounds) {
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                var arr = [];
                arr.push(Companies.get({ id: $rootScope.authentication.companyId }).$promise);
                arr.push(Playgrounds.search({ pageNumber: 1, pageSize: 200 }).$promise);
                arr.push(GameTypes.search({ pageNumber: 1, pageSize: 200 }).$promise);
                $q.all(arr).then(function (responses) {
                    $scope.company = responses[0];
                    $scope.playgrounds = responses[1].data;
                    $scope.gameTypes = responses[2].data;

                    $rootScope.isLoaded = true;
                });
        }]
    });