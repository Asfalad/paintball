angular.module('pbOperations').
    component('pbOperationDelete', {
        templateUrl: '/app/operation/delete',
        controller: ['$rootScope', '$routeParams', '$location', 'Operations', 'Notification',
            function ($rootScope, $routeParams, $location, Operations, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.operation = Operations.get({ id: self.id }, function () {
                }, function (response) {
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                });

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.operation.$delete(function () {
                        $location.url('/company/operations');
                        Notification.success({ message: 'Операция успешно удалена', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/operations');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                    
                };
                $rootScope.isLoaded = true;
            }]
    });