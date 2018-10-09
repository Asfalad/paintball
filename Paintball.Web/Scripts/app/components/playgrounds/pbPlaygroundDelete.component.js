angular.module('pbPlaygrounds').
    component('pbPlaygroundDelete', {
        templateUrl: '/app/playground/delete',
        controller: ['$rootScope', '$routeParams', '$location', 'Playgrounds', 'Notification',
            function ($rootScope, $routeParams, $location, Playgrounds, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.playground = Playgrounds.get({ id: self.id }, function () { },
                    function (response) {
                        $location.url('/company/playgrounds');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.playground.$delete(function () {
                        $location.url('/company/playgrounds');
                        Notification.success({ message: 'Площадка успешно удалена', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/playgrounds');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                };
                $rootScope.isLoaded = true;
            }]
    });