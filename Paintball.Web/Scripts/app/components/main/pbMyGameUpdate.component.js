angular.module('pbMain').
    component('pbMyGameUpdate', {
        templateUrl: '/app/mygame/modify',
        controller: ['$rootScope', '$location', '$anchorScroll', 'Notification',
             '$routeParams', 'MyGames', 'EquipmentOrders', 'Order',
            function ($rootScope, $location,
                $anchorScroll, Notification, $routeParams, MyGames, EquipmentOrders, Order) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.accountTab = true;
                $rootScope.isLoaded = false;

                self.creatingEquipment = new EquipmentOrders();
                self.equipmentOrders = [];

                self.dateBeginOptions = {
                    showWeeks: false,
                    startingDay: 0
                }

                self.id = $routeParams.id;

                if (self.game === undefined) {
                    var gameReponse = MyGames.get({ id: self.id }, function (response) {
                        self.game = gameReponse.game;
                        self.currentPlayground = gameReponse.playground;
                        self.currentGameType = gameReponse.gameType;
                        self.equipmentOrders = response.orders;
                        var orderResponse = Order.get({ id: self.game.companyId }, function (response) {
                            self.playgrounds = orderResponse.playgrounds;
                            self.gameTypes = orderResponse.gameTypes;
                            self.equipment = orderResponse.equipment;
                            
                            $rootScope.isLoaded = true;
                        }, function (response) {
                            $location.url('/mygames');
                            Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                        });
                    }, function (response) {
                        $location.url('/mygames');
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                self.selectPlayground = function (playground) {
                    self.currentPlayground = playground;
                };

                self.proceed = function () {
                    if (self.game.beginDate) {
                        if (self.game.playerCount) {
                            self.game.playground = self.currentPlayground.id;
                            self.game.gameType = self.currentGameType.id;
                            self.game.gamePrice = self.currentPlayground.price + self.currentGameType.price;
                            MyGames.update(self.game, function (response) {
                                self.game = response.game;
                                self.gameCreated = true;
                                Notification.success({ message: 'Измените заказанное оборудование', title: 'Сохранено', positionY: 'bottom', positionX: 'right' });
                                
                            }, function (response) {
                                $location.url('/mygames');
                                Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                            })
                        } else {
                            Notification.error({ message: 'Укажите количество игроков', title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                        }
                    } else {
                        Notification.error({ message: 'Укажите дату начала игры', title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    }
                };

                self.createEquipment = function () {
                    self.updatingEquipment = false;
                    self.equipmentCreating = true;
                    self.selectedEquipment = self.equipment[0];

                    self.creatingEquipment = new EquipmentOrders();
                    self.creatingEquipment.gameId = self.game.id;
                    self.creatingEquipment.equipmentId = self.selectedEquipment.id;
                    self.creatingEquipment.count = 1;
                }

                self.cancel = function () {

                    self.updatingEquipment = false;
                    self.equipmentCreating = false;
                    self.selectedEquipment = self.equipment[0];

                    self.creatingEquipment = new EquipmentOrders();
                    self.creatingEquipment.gameId = self.game.id;
                    self.creatingEquipment.equipmentId = self.selectedEquipment.id;
                    self.creatingEquipment.count = 1;
                }

                self.editOrder = function (order) {
                    self.updatingEquipment = true;
                    self.equipmentCreating = true;
                    self.newEquipmentOrder = order;

                    self.selectedEquipment = self.equipment.find(function (item) {
                        if (item.id == order.equipmentId) {
                            return item;
                        }
                    });

                    self.creatingEquipment = order;

                    self.oldOrder = order;
                    self.oldSelectedEquipment = self.selectedEquipment;
                }

                self.addOrder = function () {
                    if (self.creatingEquipment.count > 0) {
                        self.creatingEquipment.equipmentId = self.selectedEquipment.id;
                        EquipmentOrders.save(self.creatingEquipment, function (response) {
                            self.updatingEquipment = false;
                            self.equipmentCreating = false;
                            self.creatingEquipment = response;
                            self.equipmentOrders.push(self.creatingEquipment);
                            self.game.gamePrice += self.selectedEquipment.price * self.creatingEquipment.count;
                            Notification.success({ message: 'Оборудование добавлено', title: 'Добавлено успешно', positionY: 'bottom', positionX: 'right' });
                        }, function (response) {
                            Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                        })
                    }
                    else {
                        Notification.error({ message: 'Нужно указать количество', title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    }
                }

                self.saveOrder = function () {
                    if (self.creatingEquipment.count > 0) {
                        self.creatingEquipment.equipmentId = self.selectedEquipment.id;
                        EquipmentOrders.update(self.creatingEquipment, function () {
                            self.updatingEquipment = false;
                            self.equipmentCreating = false;
                            self.game.gamePrice -= self.oldSelectedEquipment.price * self.oldOrder.count;
                            self.game.gamePrice += self.selectedEquipment.price * self.creatingEquipment.count;
                            Notification.success({ message: 'Оборудование сохранено', title: 'Обновлено успешно', positionY: 'bottom', positionX: 'right' });
                        }, function (response) {
                            Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                        });
                    }
                    else {
                        Notification.error({ message: 'Нужно указать количество', title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    }
                }

                self.deleteOrder = function (order) {
                    EquipmentOrders.delete(order, function (response) {
                        var index = self.equipmentOrders.indexOf(order);
                        self.equipmentOrders.splice(index, 1);
                        var equipment = self.equipment.find(function (item) {
                            if (order.equipmentId === item.id)
                                return item;
                        });

                        self.game.gamePrice -= equipment.price * order.count;
                        Notification.success({ message: 'Оборудование удалено', title: 'Удалено успешно', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }
            }]
    });