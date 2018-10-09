angular.module('pbMain').
    component('pbMyGamesList', {
        templateUrl: '/app/mygames',
        controller: ['$rootScope', 'MyGames', '$location', '$anchorScroll', 'Notification', 'Playgrounds', 'GameTypes', '$filter', '$scope',
            function pbIndexController($rootScope, MyGames, $location, $anchorScroll, Notification, Playgrounds, GameTypes, $filter, $scope) {
                $rootScope.cleanTabFlags();
                $rootScope.accountTab = true;
                $rootScope.isLoaded = true;
                self.myGames = true;
                $scope.gameLoaded = false;

                $scope.today = new Date();
                $scope.pageNumber = 1;
                $scope.pageSize = 15;
                $scope.descending = false;
                $scope.count = 0;
                $scope.getPages = function () {
                    var input = [];
                    var pagesCount = Math.ceil($scope.count / $scope.pageSize);
                    for (var i = 0; i < pagesCount ; i++) {
                        input.push(i + 1);
                    }
                    return input;
                };

                $scope.dateOptions = {
                    showWeeks: false,
                    startingDay: 1
                };

                $scope.$watch('today', function () {
                    $scope.loadData();
                });

                $scope.loadData = function () {
                    $rootScope.isLoaded = false;
                    var response = MyGames.search({
                        date: $filter('date')($scope.today, 'yyyy-MM-dd'),
                        pageNumber: $scope.pageNumber,
                        pageSize: $scope.pageSize,
                        descending: $scope.descending
                    }, function () {
                        // UPDATE VIEW
                        $scope.playgrounds = [];
                        $scope.gameTypes = [];
                        $scope.games = [];
                        $scope.count = response.count;
                        for (var i = 0; i < response.data.length; i++) {
                            var gameData = response.data[i];
                            $scope.games.push(gameData.game);
                            $scope.playgrounds.push(gameData.playground);
                            $scope.gameTypes.push(gameData.gameType);
                        }

                        $location.hash('header');
                        $anchorScroll();
                        $rootScope.isLoaded = true;
                    }, function (response) {
                        // SHOW ERROR
                        $rootScope.isLoaded = true;
                        Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                    });
                };

                $scope.getPage = function (page) {
                    if ($scope.pageNumber != page) {
                        $scope.pageNumber = page;

                        $scope.loadData();
                    }
                };
            }]
    });