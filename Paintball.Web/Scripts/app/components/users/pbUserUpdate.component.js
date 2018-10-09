angular.module('pbUsers').
    component('pbUserUpdate', {
        templateUrl: '/app/staff/modify',
        controller: ['$rootScope', '$routeParams', '$location', 'Users', 'Notification',
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
                    self.user.$update(function () {
                        $location.url('/company/users');
                        Notification.success({ message: 'Данные сотрудника успешно обновлены', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/users');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                };

                $rootScope.isLoaded = true;
            }]
    });