angular.module('pbStorage').
    component('pbStorageCreate', {
        templateUrl: '/app/storage/modify',
        controller: ['$rootScope', '$location', 'Equipment', 'Notification',
            function ($rootScope, $location, Equipment, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.equipment = new Equipment();

                self.isCreating = true;

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.equipment.$save(function () {
                        $location.url('/company/storage');
                        Notification.success({ message: 'Оборудование добавлено', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/storage');
                        Notification.error({ message: response.data.message, title: 'Оборудование не создано' , positionY: 'bottom', positionX: 'right' });
                    });
                };
                $rootScope.isLoaded = true;
            }]
    });