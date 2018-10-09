angular.module('pbGametypes').
    component('pbGametypeCreate', {
        templateUrl: '/app/gametype/modify',
        controller: ['$rootScope', '$location', 'GameTypes', 'Notification',
            function ($rootScope, $location, GameTypes, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.gametype = new GameTypes();

                self.isCreating = true;

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.gametype.$save(function () {
                        $location.url('/company/gametypes');
                        Notification.success({ message: 'Тип игры успешно создан', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/gametypes');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' })
                    });
                };
                $rootScope.isLoaded = true;
            }]
    });