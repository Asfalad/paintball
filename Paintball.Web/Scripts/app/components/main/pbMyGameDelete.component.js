angular.module('pbMain').
    component('pbMyGameDelete', {
        templateUrl: '/app/mygame/delete',
        controller: ['$rootScope', '$routeParams', '$location', 'MyGames', 'Notification',
            function ($rootScope, $routeParams, $location, MyGames, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.accountTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    MyGames.delete({ id:self.id }, function () {
                        $location.url('/mygames');
                        Notification.success({ message: 'Игра успешно удалена', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/mygames');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                };
                $rootScope.isLoaded = true;
            }]
    });