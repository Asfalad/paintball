angular.module('pbCertificates').
    component('pbCertificateDelete', {
        templateUrl: '/app/certificate/delete',
        controller: ['$rootScope', '$routeParams', '$location', 'Certificates', 'Notification',
            function ($rootScope, $routeParams, $location, Certificates, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.certificate = Certificates.get({ id: self.id }, function () { }, function (response) {
                    $location.url('/company/certificates');
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                });

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.certificate.$delete(function () {
                        $location.url('/company/certificates');
                        Notification.success({ message: 'Сертификат успешно удален', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/certificates');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                };
                $rootScope.isLoaded = true;
            }]
    });