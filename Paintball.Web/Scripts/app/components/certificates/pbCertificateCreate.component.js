angular.module('pbCertificates').
    component('pbCertificateCreate', {
        templateUrl: '/app/certificate/modify',
        controller: ['$rootScope', '$location', 'Certificates', 'Notification',
            function ($rootScope, $location, Certificates, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.certificate = new Certificates();
                self.certificate.startDate = new Date();
                self.isCreating = true;

                self.dateOptions = {
                    showWeeks: false,
                    startingDay: 1,
                    minDate: self.certificate.startDate
                };

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.certificate.$save(function () {
                        $location.url('/company/certificates');
                        Notification.success({ message: 'Сертификат создан успешно', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/certificates');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                };

                $rootScope.isLoaded = true;
            }]
        });