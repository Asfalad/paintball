angular.module('pbNews').
    component('pbNewsCreate', {
        templateUrl: '/app/news/modify',
        controller: ['$rootScope', '$location', 'News', 'Notification', 'Upload',
            function ($rootScope, $location, News, Notification, Upload) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.newsTab = true;
                $rootScope.isLoaded = false;

                self.news = new News();
                self.news.publishDate = new Date();
                self.isCreating = true;

                self.upload = function (file) {
                    return Upload.upload({
                        url: '/papi/images',
                        data: { file: file }
                    }, function () {
                    }, function (response) {
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                };

                self.saveNews = function () {
                    self.news.$save(function () {
                        $location.url('/news');
                        Notification.success({ message: 'Новость создана успешно', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/news');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    if (self.news.titleImage) {
                        self.upload(self.news.titleImage).then(function (response) {
                            self.news.titleImage = response.data.name;
                            self.saveNews();
                        }, function (response) {
                            $location.url('/news');
                            Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                        });
                    } else {
                        self.saveNews();
                    }
                };

                $rootScope.isLoaded = true;
            }]
    });