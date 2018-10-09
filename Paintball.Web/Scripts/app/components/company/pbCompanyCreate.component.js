angular.module('pbCompany').
    component('pbCompanyCreate', {
        templateUrl: '/app/company/create',
        controller: ['$rootScope', '$http', '$location', 'Companies', 'authService', 'Upload', 'Notification',
            function ($rootScope, $http, $location, Companies, authService, Upload, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.company = new Companies();

                self.upload = function (file) {
                    return Upload.upload({
                        url: '/papi/images',
                        data: { file: file }
                    }, function () {
                    }, function (response) {
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                };
                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    if (self.company.logoImage) {
                        self.upload(self.company.logoImage).then(function (response) {
                            self.company.logoImage = response.data.name;
                            self.company.$save(function () {
                                authService.logOut();
                                location.url("/login");
                                Notification.success({ title: 'Компания успешно создана', message: 'Войдите в свой аккаунт, чтобы просмотреть данные о компании', positionY: 'bottom', positionX: 'right' });
                            }, function (response) {
                                $rootScope.isLoaded = true;
                                Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                            });
                        });
                    }
                    else {
                        self.company.$save();
                        authService.logOut(function () {
                            location.url("/login");
                            Notification.success({ title: 'Компания успешно создана', message: 'Войдите в свой аккаунт, чтобы просмотреть данные о компании', positionY: 'bottom', positionX: 'right' });
                        }, function (response) {
                            $rootScope.isLoaded = true;
                            Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                        });
                    }
                };
                $rootScope.isLoaded = true;
            }]
    });