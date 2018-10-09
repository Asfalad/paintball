angular.module('pbMain')
    .component('pbLogin', {
        templateUrl: '/app/login',
        controller: ['$rootScope', '$location', 'authService', '$window', '$routeParams',
            function PbLoginController($rootScope, $location, authService, $window, $routeParams) {
                var self = this;
                self.savedSuccessfully = false;
                self.message = "";

                self.url = $location.search().returnUrl ? $location.search().returnUrl : '/app/#!/';

                self.loginData = {
                    userName: "",
                    password: ""
                };

                self.login = function () {
                    authService.login(self.loginData).then(function (response) {
                        self.message = 'Вход выполнен';
                        self.savedSuccessfully = true;
                        $rootScope.isLoaded = false;
                        var host = $location.host();
                        var port = $location.port();
                        $window.location.href = 'http://'+ host + ':' + port + '/app/#!' + self.url;
                        $window.location.reload();
                    }, function (err) {
                        self.message = err.data.error_description;
                        self.$apply();
                    });
                };

                $rootScope.isLoaded = true;
        }]});