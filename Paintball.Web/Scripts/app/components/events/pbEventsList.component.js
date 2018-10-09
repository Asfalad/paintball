angular.module('pbEvents').
    component('pbEventsList', {
        templateUrl: '/app/events',
        controller: ['$rootScope', 'Events', 'Notification', '$location', '$anchorScroll',
            function pbEventsController($rootScope, Events, Notification, $location, $anchorScroll) {
            var self = this;
            $rootScope.cleanTabFlags();
            $rootScope.eventTab = true;
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
                var response = Events.search({
                    pageNumber: self.pageNumber,
                    pageSize: self.pageSize,
                    descending: self.descending
                }, function () {
                    // UPDATE VIEW
                    self.events = response.data;
                    self.count = response.count;
                    $location.hash('events');
                    $anchorScroll();
                    $rootScope.isLoaded = true;
                }, function (response) {
                    $rootScope.isLoaded = true;
                    // SHOW ERROR
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
            $rootScope.isLoaded = true;
        }]
    });