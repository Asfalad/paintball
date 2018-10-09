angular.module('pbMain').
    component('pbRegister', {
        templateUrl: '/app/register',
        controller: ['$rootScope','$location', '$timeout', 'authService',
            function PbRegisterController($rootScope, $location, $timeout, authService) {
                var self = this;
                self.savedSuccessfully = false;
                self.message = "";

                self.registration = {
                    firstName: "",
                    lastName: "",
                    email: "",
                    password: "",
                    confirmPassword: ""
                };

                self.signUp = function () {
                    authService.saveRegistration(this.registration).
                        then(function (response) {
                            self.savedSuccessfully = true;
                            self.message = "Вы успешно зарегистрировались";
                            startTimer();
                        }, function (response) {
                            var errors = [];
                            for (var key in response.data.modelState) {
                                for (var i = 0; i < response.data.modelState[key].length; i++) {
                                    errors.push(response.data.modelState[key][i]);
                                }
                            }
                            self.message = "Регистрация неудалась: " + errors.join(' ');
                            self.$apply();
                        });
                }

                var startTimer = function () {
                    var timer = $timeout(function () {
                        $timeout.cancel(timer);
                        $location.path('/login');
                    }, 1500);
                };
                $rootScope.isLoaded = true;
                
        }]
    });