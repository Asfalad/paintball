angular.module('pbEvents').
    component('pbEventDelete', {
        templateUrl: '/app/event/delete',
        controller: ['$rootScope', '$routeParams', '$location', 'Events', 'Notification',
            function ($rootScope, $routeParams, $location, Events, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.eventTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.event = Events.get({ id: self.id }, function () {
                }, function (response) {
                    $location.url('/company/events');
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' })
                });

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.event.$delete(function () {
                        $location.url('/events');
                        Notification.success({ message: 'Событие успешно удалено', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/events');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                    
                };

                $rootScope.isLoaded = true;
            }]
    });