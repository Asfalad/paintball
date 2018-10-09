angular.module('pbUsers').
    component('pbUserDelete', {
        templateUrl: '/app/staff/delete',
        controller: ['$rootScope', '$routeParams', '$location', 'Users', 'Notification' ,
            function ($rootScope, $routeParams, $location, Users, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.user = Users.get({ id: self.id }, function () {
                }, function (response) {
                    $location.url('/company/users');
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                });

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.user.$delete(function () {
                        $location.url('/company/users');
                        Notification.success({ message: 'Сотрудник успешно исключен из компании', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/users');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                };
                $rootScope.isLoaded = true;
            }]
    });