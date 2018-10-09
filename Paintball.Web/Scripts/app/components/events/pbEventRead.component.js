angular.module('pbEvents').
    component('pbEventRead', {
        templateUrl: '/app/event/read',
        controller: ['$rootScope', '$routeParams', '$location', 'Events', 'Notification',
            function ($rootScope, $routeParams, $location, Events, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.eventTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.event = Events.get({ id: self.id }, function () {
                }, function (response) {
                    $location.url('/events');
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                });
                $rootScope.isLoaded = true;
            }]
    });