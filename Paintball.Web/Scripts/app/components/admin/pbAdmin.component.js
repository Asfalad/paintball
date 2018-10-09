angular.module('pbAdmin').
    component('pbAdmin', {
        templateUrl: '/app/admin',
        controller: ['$rootScope', function pbAdminController($rootScope) {
            $rootScope.cleanTabFlags();
            $rootScope.adminTab = true;
            $rootScope.isLoaded = false;

            $rootScope.isLoaded = true;
        }]
    });