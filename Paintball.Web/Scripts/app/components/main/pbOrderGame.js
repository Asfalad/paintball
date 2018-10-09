angular.module('pbMain').
    component('pbOrderGame', {
        templateUrl: '/app/order',
        controller: ['$rootScope', '$location', '$anchorScroll', 'Notification',
             '$routeParams', 'Order', 'MyGames', 'EquipmentOrders', 
            function pbIndexController($rootScope, $location,
                $anchorScroll, Notification,  $routeParams, Order, MyGames, EquipmentOrders) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.accountTab = true;
                $rootScope.isLoaded = false;

                self.game = new MyGames();
                self.creatingEquipment = new EquipmentOrders();
                self.equipmentOrders = [];

                self.dateBeginOptions = {
                    showWeeks: false,
                    startingDay: 0
                }
                
                self.id = $routeParams.id;

                var orderResponse = Order.get({ id: self.id }, function (response) {
                    self.playgrounds = orderResponse.playgrounds;
                    self.gameTypes = orderResponse.gameTypes;
                    self.equipment = orderResponse.equipment;

                    self.currentPlayground = self.playgrounds[0];
                    self.currentGameType = self.gameTypes[0];
                }, function (response) {
                    $location.url('/mygames');
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                });

                self.selectPlayground = function (playground) {
                    self.currentPlayground = playground;
                };

                self.proceed = function () {
                    if (self.game.beginDate) {
                        if (self.game.playerCount) {
                            self.game.companyId = self.id;
                            self.game.playground = self.currentPlayground.id;
                            self.game.gameType = self.currentGameType.id;
                            self.game.gamePrice = self.currentPlayground.price + self.currentGameType.price;
                            self.game.$save(function (response) {
                                self.game = response.game;
                                self.gameCreated = true;
                                
                                Notification.success({ message: 'Выберите оборудование для игры', title: 'Игра создана', positionY: 'bottom', positionX: 'right' });
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
                        if (item.id === order.equipmentId) {
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
                        self.creatingEquipment.$save(function (response) {
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
                        self.creatingEquipment.$update(function () {
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
                    order.$delete(function (response) {
                        var index = self.equipmentOrders.indexOf(order);
                        self.equipmentOrders.splice(index, 1);
                        Notification.success({ message: 'Оборудование удалено', title: 'Удалено успешно', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                }

                $rootScope.isLoaded = true;
            }]
    });