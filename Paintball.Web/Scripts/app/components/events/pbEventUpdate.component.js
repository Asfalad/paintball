angular.module('pbEvents').
    component('pbEventUpdate', {
        templateUrl: '/app/event/modify',
        controller: ['$rootScope', '$routeParams', '$location', 'Events', 'Notification', 'Upload',
            'Games', 'GameTypes', 'Playgrounds',
            function ($rootScope, $routeParams, $location, Events, Notification, Upload,
                Games, GameTypes, Playgrounds) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.eventTab = true;
                $rootScope.isLoaded = false;
                self.gameLoaded = true;

                self.id = $routeParams.id;

                var arr = [];
                var playgroundResponse = Playgrounds.search({
                    pageNumber: 1,
                    pageSize: 100,
                    descending: false
                }, function () {
                    self.playgrounds = playgroundResponse.data;
                    if (self.gameTypes && self.gameLoaded == false) {
                        self.gameLoaded = true;
                        self.getEvent();
                    }
                });
                var gametypesReponse = GameTypes.search({
                    pageNumber: 1,
                    pageSize: 100,
                    descending: false
                }, function () {
                    self.gameTypes = gametypesReponse.data;
                    if (self.playgrounds && self.gameLoaded == false) {
                        self.gameLoaded = true;
                        self.getEvent();
                    }
                });

                self.getEvent = function () {
                    self.event = Events.get({ id: self.id }, function () {
                        self.game = Games.get({ id: self.event.gameId }, function () {
                            self.currentPlayground = self.playgrounds.find(function (element, index, array) {
                                if (element.id === self.game.playground) {
                                    return element;
                                }
                            });
                            self.currentGameType = self.gameTypes.find(function (element, index, array) {
                                if (element.id === self.game.gameType) {
                                    return element;
                                }
                            })

                            self.dateStartOptions = {
                                showWeeks: false,
                                startingDay: 1,
                                maxDate: self.event.endDate
                            };
                            self.dateEndOptions = {
                                showWeeks: false,
                                startingDay: 1,
                                minDate: self.event.startDate
                            };
                            $rootScope.isLoaded = true;
                        }, function (response) {
                            $location.url('/events');
                            Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' })
                        });
                    }, function (response) {
                        $location.url('/events');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' })
                    });

                    self.oldImage = self.event.titleImage;
                };

                self.upload = function (file) {
                    return Upload.upload({
                        url: '/papi/images',
                        data: { file: file }
                    });
                };

                self.saveEvent = function () {
                    self.event.$update(function () {
                        $location.url('/events');
                        Notification.success({ message: 'Событие успешно обновлено', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/events');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.game.playground = self.currentPlayground.id;
                    self.game.gameType = self.currentGameType.id;
                    self.game.$update(function (response) {
                        if (typeof self.event.titleImage !== 'string') {
                            self.upload(self.event.titleImage).then(function (response) {
                                self.event.titleImage = response.data.name;
                                self.saveEvent();
                                return;
                            });
                        } else {
                            self.saveEvent();
                            return;
                        }
                    }, function (response) {
                        $location.url('/events');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                };
            }]
    });