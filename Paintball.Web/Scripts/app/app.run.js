angular.module('paintballApp').
    run(['authService', '$location', '$rootScope',
        function (authService, $location, $rootScope) {
            authService.fillAuthData();

            $rootScope.logOut = function () {
                authService.logOut();
                $location.path('/login');
            }

            $rootScope.cleanTabFlags = function(){
                $rootScope.accountTab = false;
                $rootScope.companyTab = false;
                $rootScope.eventTab = false;
                $rootScope.newsTab = false;
                $rootScope.adminTab = false;
            }

            $rootScope.authentication = authService.authentication;

            $rootScope.isLoaded = true;
    }]);