angular.module('pbCompany').
    component('pbCompanyDelete', {
        templateUrl: '/app/company/delete',
        controller: ['$rootScope', function pbCompanyController($rootScope) {
            $rootScope.cleanTabFlags();
            $rootScope.companyTab = true;
            $rootScope.isLoaded = false;
            $rootScope.isLoaded = true;
        }]
    });