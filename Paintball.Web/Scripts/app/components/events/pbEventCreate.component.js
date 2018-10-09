angular.module('pbEvents').
    component('pbEventCreate', {
        templateUrl: '/app/event/modify',
        controller: ['$rootScope', '$location', 'Events', 'Notification', 'Upload', 
            'Games', 'GameTypes', 'Playgrounds', 
            function ($rootScope, $location, Events, Notification, Upload, 
                Games, GameTypes, Playgrounds) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.eventTab = true;
                $rootScope.isLoaded = false;

                self.event = new Events();
                self.game = new Games();
                var arr = [];
                var playgroundResponse = Playgrounds.search({
                    pageNumber: 1,
                    pageSize: 100,
                    descending: false
                }, function () {
                    self.playgrounds = playgroundResponse.data;
                    if (self.gameTypes) {
                        $rootScope.isLoaded = true;
                        self.updateView();
                    }
                });
                var gametypesReponse = GameTypes.search({
                    pageNumber: 1,
                    pageSize: 100,
                    descending: false
                }, function () {
                    self.gameTypes = gametypesReponse.data;
                    if (self.playgrounds) {
                        $rootScope.isLoaded = true;
                        self.updateView();
                    }
                });
                self.isCreating = true;

                self.updateView = function () {
                    self.currentPlayground = self.playgrounds[0];
                    self.currentGameType = self.gameTypes[0];
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
                }

                self.upload = function (file) {
                    return Upload.upload({
                        url: '/papi/images',
                        data: { file: file }
                    }, function () {
                    }, function (response) {
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                };

                self.saveEvent = function () {
                    self.event.$save(function () {
                        $location.url('/events');
                        Notification.success({ message: 'Событие успешно добавлено', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        $location.url('/events');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.game.playerCount = 1;
                    self.game.playground = self.currentPlayground.id;
                    self.game.gameType = self.currentGameType.id;
                    self.game.$save(function (response) {
                        self.event.gameId = self.game.id;
                        if (self.event.titleImage) {
                            self.upload(self.event.titleImage).then(function (response) {
                                self.event.titleImage = response.data.name;
                                self.saveEvent();
                            });
                        } else {
                            self.saveEvent();
                        }
                    }, function (response) {
                        $location.url('/events');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                };
            }]
    });