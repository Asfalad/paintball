angular.module('pbNews').
    component('pbNewsList', {
        templateUrl: '/app/news',
        controller: ['$rootScope', 'News', 'Notification', '$location', '$anchorScroll',
            function ($rootScope, News, Notification, $location, $anchorScroll) {
            var self = this;
            $rootScope.cleanTabFlags();
            $rootScope.newsTab = true;
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
                var response = News.search({
                    pageNumber: self.pageNumber,
                    pageSize: self.pageSize,
                    descending: self.descending
                }, function () {
                    // UPDATE VIEW
                    self.news = response.data;
                    self.count = response.count;
                    $location.hash('news');
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
            $rootScope.isLoaded = true;
        }]
    });