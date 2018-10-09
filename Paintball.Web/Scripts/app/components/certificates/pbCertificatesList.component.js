﻿angular.module('pbCertificates').
    component('pbCertificatesList', {
        templateUrl: '/app/certificates',
        controller: ['$rootScope', 'Certificates', '$location', '$anchorScroll', 'Notification',
function pbCompanyCertificatesController($rootScope, Certificates, 
    $location, $anchorScroll, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.pageNumber = 1;
                self.pageSize = 15;
                self.descending = false;
                self.count = 0;
                self.getPages = function () {
                    var input = [];
                    var pagesCount = self.count / self.pageSize;
                    for (var i = 0; i < pagesCount ; i++)
                    {
                        input.push(i + 1);
                    }
                    return input;
                };

                self.loadData = function () {
                    $rootScope.isLoaded = false;
                    var response = Certificates.search({
                        pageNumber: self.pageNumber,
                        pageSize: self.pageSize,
                        descending: self.descending
                    }, function () {
                        // UPDATE VIEW
                        self.certificates = response.data;
                        self.count = response.count;
                        $location.hash('certificates');
                        $anchorScroll();
                        $rootScope.isLoaded = true;
                    }, function (response) {
                        // SHOW ERROR
                        $rootScope.isLoaded = true;
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                };

                self.getPage = function (page) {
                    if (self.pageNumber != page) {
                        self.pageNumber = page;
                        self.loadData();
                    }
                };
                
                self.loadData();
    }]});