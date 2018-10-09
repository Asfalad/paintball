angular.module('pbUsers').
    component('pbUserRead', {
        templateUrl: '/app/staff/read',
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
                $rootScope.isLoaded = true;
            }]
    });