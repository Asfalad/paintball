angular.module('pbPlaygrounds').
    component('pbPlaygroundUpdate', {
        templateUrl: '/app/playground/modify',
        controller: ['$rootScope', '$routeParams', '$location', 'Playgrounds', 'Notification', 'Upload', '$q',
            function ($rootScope, $routeParams, $location, Playgrounds, Notification, Upload, $q) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.id = $routeParams.id;

                self.playground = Playgrounds.get({ id: self.id }, function () {
                }, function (response) {
                    $location.url('/company/playgrounds');
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                });

                self.oldPlayground = self.playground;

                self.upload = function (file) {
                    return Upload.upload({
                        url: '/papi/images',
                        data: { file: file }
                    }, function () {
                    }, function (response) {
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                };

                self.savePlayground = function () {
                    self.playground.$update(function () {
                        $location.url('/company/playgrounds');
                        Notification.success({ message: 'Площадка успешно сохранена', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/playgrounds');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    var arr = [];
                    if (typeof self.playground.firstImage !== 'string') {
                        if (self.playground.firstImage) {
                            arr.push(self.upload(self.playground.firstImage));
                        }
                    }
                    if (typeof self.playground.secondImage !== 'string') {
                        if (self.playground.secondImage) {
                            arr.push(self.upload(self.playground.secondImage));
                        }
                    }
                    if (typeof self.playground.thirdImage !== 'string') {
                        if (self.playground.thirdImage) {
                            arr.push(self.upload(self.playground.thirdImage));
                        }
                    }
                    if (typeof self.playground.fourthImage !== 'string') {
                        if (self.playground.fourthImage) {
                            arr.push(self.upload(self.playground.fourthImage));
                        }
                    }
                    if (arr.length === 0) {
                        self.savePlayground();
                    } else {
                        $q.all(arr).then(function (responses) {
                            var i = 0;
                            if (typeof self.playground.firstImage !== 'string') {
                                if (self.playground.firstImage) {
                                    self.playground.firstImage = responses[i].data.name;
                                    i++;
                                }
                            }
                            if (typeof self.playground.secondImage !== 'string') {
                                if (self.playground.secondImage) {
                                    self.playground.secondImage = responses[i].data.name;
                                    i++;
                                }
                            }
                            if (typeof self.playground.thirdImage !== 'string') {
                                if (self.playground.thirdImage) {
                                    self.playground.thirdImage = responses[i].data.name;
                                    i++;
                                }
                            }
                            if (self.playground.fourthImage !== 'string') {
                                if (self.playground.fourthImage) {
                                    self.playground.fourthImage = responses[i].data.name;
                                }
                            }
                            self.savePlayground();
                        });
                    }
                    
                };
                $rootScope.isLoaded = true;
            }]
    });