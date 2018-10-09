angular.module('pbPlaygrounds').
    component('pbPlaygroundCreate', {
        templateUrl: '/app/playground/modify',
        controller: ['$rootScope', '$location', 'Playgrounds', 'Notification', 'Upload', '$q',
            function ($rootScope, $location, Playgrounds, Notification, Upload, $q) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.playground = new Playgrounds();

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

                self.savePlayground = function () {
                    self.playground.$save(function () {
                        $location.url('/company/playgrounds');
                        Notification.success({ message: 'Площадка успешно создана', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/company/playgrounds');
                        Notification.error({ message: response.data.message, title: 'Площадка не создана', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    var arr = [];
                    if (self.playground.firstImage) {
                        arr.push(self.upload(self.playground.firstImage));
                    }
                    if (self.playground.secondImage) {
                        arr.push(self.upload(self.playground.secondImage));
                    }
                    if (self.playground.thirdImage) {
                        arr.push(self.upload(self.playground.thirdImage));
                    }
                    if (self.playground.fourthImage) {
                        arr.push(self.upload(self.playground.fourthImage));
                    }
                    $q.all(arr).then(function (responses) {
                        var i = 0;
                        if (self.playground.firstImage) {
                            self.playground.firstImage = responses[i].data.name;
                            i++;
                        }
                        if (self.playground.secondImage) {
                            self.playground.secondImage = responses[i].data.name;
                            i++;
                        }
                        if (self.playground.thirdImage) {
                            self.playground.thirdImage = responses[i].data.name;
                            i++;
                        }
                        if (self.playground.fourthImage) {
                            self.playground.fourthImage = responses[i].data.name;
                        }
                        self.savePlayground();
                    })
                };
                $rootScope.isLoaded = true;
            }]
    });