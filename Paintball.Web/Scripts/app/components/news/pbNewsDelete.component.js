angular.module('pbNews').
    component('pbNewsDelete', {
        templateUrl: '/app/news/delete',
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

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.news.$delete(function () {
                        $location.url('/news');
                        Notification.success({ message: 'Новость успешно удалена', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/news');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                    });
                    
                };
                $rootScope.isLoaded = true;
            }]
    });