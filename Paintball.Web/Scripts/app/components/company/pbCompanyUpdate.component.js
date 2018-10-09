angular.module('pbCompany').
    component('pbCompanyUpdate', {
        templateUrl: '/app/company/modify',
        controller: ['$scope','$rootScope', 'Companies', 'Upload', 'Notification', '$location',
            function pbCompanyController($scope, $rootScope, Companies, Upload, Notification, $location) {
                
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;
                
                $scope.upload = function (file) {
                    return Upload.upload({
                        url: '/papi/images',
                        data: { file: file }
                    }, function () {
                    }, function (response) {
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                };
                $scope.proceed = function () {
                    $rootScope.isLoaded = false;
                    if (typeof $scope.company.logoImage !== 'string') {
                        if ($scope.company.logoImage) {
                            $scope.upload($scope.company.logoImage).then(function (response) {
                                $scope.company.logoImage = response.data.name;
                                $scope.company.$update(function () {
                                    $location.url("/company");
                                    Notification.success({ message: 'Компания обновлена', positionY: 'bottom', positionX: 'right' });
                                }, function (response) {
                                    $rootScope.isLoaded = true;
                                    Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                                });
                            });
                        }
                    }
                    else {
                        $scope.company.$update(function () {
                            $location.url("/company");
                            Notification.success({ message: 'Компания успешно обновлена', positionY: 'bottom', positionX: 'right' });
                        }, function () {
                            Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                        });
                    }
                };

                $scope.company = Companies.get({ id: $rootScope.authentication.companyId }, function () {
                }, function (response) {
                    $location.url('/company');
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                });
                $rootScope.isLoaded = true;
        }]
    });