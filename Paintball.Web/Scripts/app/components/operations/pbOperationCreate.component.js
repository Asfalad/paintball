angular.module('pbOperations').
    component('pbOperationCreate', {
        templateUrl: '/app/operation/modify',
        controller: ['$rootScope', '$location', 'Operations', 'Notification',
            function ($rootScope, $location, Operations, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.operation = new Operations();

                self.isCreating = true;

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.operation.$save(function () {
                        $location.url('/company/operations');
                        Notification.success({ message: 'Операция успешно добавлена', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/operations');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                };
                $rootScope.isLoaded = true;
            }]
    });