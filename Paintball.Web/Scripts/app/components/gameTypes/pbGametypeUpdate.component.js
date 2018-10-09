angular.module('pbGametypes').
    component('pbGametypeUpdate', {
        templateUrl: '/app/gametype/modify',
        controller: ['$rootScope', '$routeParams', '$location', 'GameTypes', 'Notification',
            function ($rootScope, $routeParams, $location, GameTypes, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.gametype = GameTypes.get({ id: self.id }, function () {
                }, function (response) {
                    $location.url('/company/gametypes');
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                });

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.gametype.$update(function () {
                        $location.url('/company/gametypes');
                        Notification.success({ message: 'Тип игры успешно обновлен', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/gametypes');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                };
                $rootScope.isLoaded = true;
            }]
    });