angular.module('pbNews').
    component('pbNewsRead', {
        templateUrl: '/app/news/read',
        controller: ['$rootScope', '$routeParams', '$location', 'News', 'Notification',
            function ($rootScope, $routeParams, $location, News, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.newsTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.news = News.get({ id: self.id }, function () {
                }, function (response) {
                    $location.url('/news');
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                });
                $rootScope.isLoaded = true;
            }]
    });