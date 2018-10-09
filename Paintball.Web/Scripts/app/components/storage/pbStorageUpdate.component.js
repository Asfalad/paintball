angular.module('pbStorage').
    component('pbStorageUpdate', {
        templateUrl: '/app/storage/modify',
        controller: ['$rootScope', '$routeParams', '$location', 'Equipment', 'Notification',
            function ($rootScope, $routeParams, $location, Equipment, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.equipment = Equipment.get({ id: self.id }, function () {
                }, function (response) {
                    $location.url('/company/storage');
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                });

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.equipment.$update(function () {
                        $location.url('/company/storage');
                        Notification.success({ message: 'Оборудование успешно обновлено', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/storage');
                        Notification.error({ message: response.data.message, title: 'Оборудование не обновлено' , positionY: 'bottom', positionX: 'right' });
                    });
                };
                $rootScope.isLoaded = true;
            }]
    });