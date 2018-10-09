angular.module('pbUsers').
    component('pbUsersList', {
        templateUrl: '/app/staff',
        controller: ['$rootScope', 'Users', 'Notification', '$location', '$anchorScroll',
            function pbCompanyStorageController($rootScope, Users, Notification, $location, $anchorScroll) {
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
                    for (var i = 0; i < pagesCount ; i++) {
                        input.push(i + 1);
                    }
                    return input;
                };

                self.loadData = function () {
                    $rootScope.isLoaded = false;
                    var response = Users.search({
                        pageNumber: self.pageNumber,
                        pageSize: self.pageSize,
                        descending: self.descending
                    }, function () {
                        // UPDATE VIEW
                        self.users = response.data;
                        self.count = response.count;
                        $location.hash('users');
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
            }]
    });