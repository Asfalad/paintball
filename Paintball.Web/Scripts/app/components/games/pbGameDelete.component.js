angular.module('pbGames').
    component('pbGameDelete', {
        templateUrl: '/app/game/delete',
        controller: ['$rootScope', '$routeParams', '$location', 'Games', 'Notification',
            function ($rootScope, $routeParams, $location, Games, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.game = Games.get({ id: self.id }, function () { },
                    function (response) {
                        $location.url('/company/games');
                        Notification.error(response.message);
                    });

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.game.$delete(function () {
                        $location.url('/company/games');
                        Notification.success({ message: 'Игра успешно удалена', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/games');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                };
                $rootScope.isLoaded = true;
            }]
    });