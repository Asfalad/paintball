angular.module('pbGames').
    component('pbGameUpdate', {
        templateUrl: '/app/game/modify',
        controller: ['$rootScope', '$routeParams', '$location', 'Games', 'Notification', 
        'Playgrounds', 'GameTypes', 'Equipment', 'EquipmentOrders', '$q',
            function ($rootScope, $routeParams, $location, Games, Notification,
                Playgrounds, GameTypes, Equipment, EquipmentOrders, $q) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;
                self.gameLoaded = false;
                self.gameCreated = false;
                self.updatingEquipment = false;
                self.equipmentCreating = false;
                self.creatingEquipment = new Equipment();
                self.newEquipmentOrder = new EquipmentOrders();

                self.id = $routeParams.id;

                var arr = [];
                arr.push(Playgrounds.search({
                    pageNumber: 1,
                    pageSize: 100,
                    descending: false
                }).$promise);

                arr.push(GameTypes.search({
                    pageNumber: 1,
                    pageSize: 100,
                    descending: false
                }).$promise);

                arr.push(Equipment.search({
                    pageNumber: 1,
                    pageSize: 1000,
                    descending: false
                }).$promise);

                arr.push(EquipmentOrders.search({
                    gameId: self.id
                }).$promise)

                arr.push(Games.get({ id: self.id }).$promise);

                $q.all(arr).then(function (responses) {
                    self.playgrounds = responses[0].data;
                    self.gameTypes = responses[1].data;
                    self.equipments = responses[2].data;
                    self.equipmentOrders = responses[3].data;
                    self.game = responses[4];

                    self.currentPlayground = self.playgrounds.find(function (item) {
                        if (item.id == self.game.playground) {
                            return item;
                        }
                    });

                    self.currentGameType = self.gameTypes.find(function (item) {
                        if (item.id == self.game.gameType) {
                            return item;
                        }
                    });
                    $rootScope.isLoaded = true;
                });

                self.saveOrder = function () {
                    self.newEquipmentOrder.equipmentId = self.creatingEquipment.id;
                    self.newEquipmentOrder.gameId = self.game.id;
                    EquipmentOrders.update(self.newEquipmentOrder, function (response) {
                        self.updateData();
                    }, function (response) {
                        self.equipmentCreating = false;
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.updateData = function () {
                    var equipmentResponse = EquipmentOrders.search({
                        gameId: self.id
                    }, function (response) {
                        self.equipmentOrders = equipmentResponse.data;
                        self.game = Games.get({ id: self.game.id }, function () {
                            self.equipmentCreating = false;
                            self.updatingEquipment = false;
                            self.creatingEquipment = false;
                            Notification.success({ message: 'Успешно выполнено', positionY: 'bottom', positionX: 'right' });
                        }, function (response) {
                            self.equipmentCreating = false;
                            Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                        });
                    }, function (response) {
                        self.equipmentCreating = false;
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.deleteOrder = function (order) {
                    EquipmentOrders.delete(order, function (response) {
                        self.updateData();
                        
                    }, function (response) {
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.addOrder = function () {
                    self.newEquipmentOrder.equipmentId = self.creatingEquipment.id;
                    self.newEquipmentOrder.gameId = self.game.id;
                    self.newEquipmentOrder.$save(function (response) {
                        self.updateData();
                        
                    }, function (response) {
                        self.equipmentCreating = false;
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.selectPlayground = function (pg) {
                    self.currentPlayground = pg;
                }

                self.createEquipment = function(){
                    self.equipmentCreating = true;
                    self.updatingEquipment = false;
                    self.creatingEquipment = false;
                    self.newEquipmentOrder = new EquipmentOrders();
                    self.newEquipmentOrder.count = 1;
                    self.creatingEquipment = self.equipments[0];
                }

                self.cancel = function () {
                    self.equipmentCreating = false;
                    self.updatingEquipment = false;
                    self.creatingEquipment = false;
                }

                self.editOrder = function (order) {
                    self.creatingEquipment = false;
                    self.updatingEquipment = true;
                    self.equipmentCreating = false;
                    self.newEquipmentOrder = order;
                    self.creatingEquipment = self.equipments.find(function (item) {
                        if (item.id == order.equipmentId){
                            return item;
                        }
                    });
                }

                self.updateGame = function () {
                    $rootScope.isLoaded = false;
                    self.game.playground = self.currentPlayground.id;
                    self.game.gameType = self.currentGameType.id;
                    self.game.$update(function () {
                        Notification.success({ message: 'Игра обновлена успешно', positionY: 'bottom', positionX: 'right' });
                        self.gameCreated = true;
                        $rootScope.isLoaded = true;
                    }, function (response) {
                        $rootScope.isLoaded = true;
                        $location.url('/company/games');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    self.updateGame();
                };
            }]
    });