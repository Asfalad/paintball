angular.module('pbNews').
    component('pbNewsUpdate', {
        templateUrl: '/app/news/modify',
        controller: ['$rootScope', '$routeParams', '$location', 'News', 'Notification', 'Upload',
            function ($rootScope, $routeParams, $location, News, Notification, Upload) {
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

                self.upload = function (file) {
                    return Upload.upload({
                        url: '/papi/images',
                        data: { file: file }
                    }, function () {
                    }, function (response) {
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                };

                self.newsSave = function () {
                    self.news.$update(function () {
                        $location.url('/news');
                        Notification.success({ message: 'Новость успешно обновлена', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/news');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    if (typeof self.news.titleImage !== 'string') {
                        self.upload(self.news.titleImage).then(function (response) {
                            self.news.titleImage = response.data.name;
                            self.newsSave();
                        }, function (response) {
                            $location.url('/news');
                            Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                        });
                    } else {
                        self.newsSave();
                    }
                };

                $rootScope.isLoaded = true;
            }]
    });