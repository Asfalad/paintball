angular.module('pbMain').
    component('pbIndex', {
        templateUrl: '/app/index',
        controller: ['$rootScope', '$location',
            function pbIndexController($rootScope, $location) {
                $rootScope.cleanTabFlags();
                $rootScope.accountTab = true;
                $rootScope.isLoaded = true;
        }]
    });