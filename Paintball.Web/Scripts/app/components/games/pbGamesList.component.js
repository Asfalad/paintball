angular.module('pbGames').
    component('pbGamesList', {
        templateUrl: '/app/games',
        controller: ['$rootScope', 'Games', '$location', '$anchorScroll', 'Notification', 'Playgrounds', 'GameTypes', '$filter', '$scope',
            function pbCompanyGames($rootScope, Games, $location, $anchorScroll, Notification, Playgrounds, GameTypes, $filter, $scope) {
                
            $rootScope.cleanTabFlags();
            $rootScope.companyTab = true;
            $rootScope.isLoaded = false;

            $scope.gameLoaded = false;

            $scope.today = new Date();
            $scope.pageNumber = 1;
            $scope.pageSize = 15;
            $scope.descending = false;
            $scope.count = 0;
            $scope.getPages = function () {
                var input = [];
                var pagesCount = $scope.count / $scope.pageSize;
                for (var i = 0; i < pagesCount ; i++) {
                    input.push(i + 1);
                }
                return input;
            };

            var playgroundResponse = Playgrounds.search({
                pageNumber: 1,
                pageSize: 100,
                descending: false
            }, function () {
                $scope.playgrounds = playgroundResponse.data;
                if ($scope.gameTypes && $scope.gameLoaded == false) {
                    $scope.gameLoaded = true;
                    $scope.loadData();
                }
            });
            var gametypesReponse = GameTypes.search({
                pageNumber: 1,
                pageSize: 100,
                descending: false
            }, function () {
                $scope.gameTypes = gametypesReponse.data;
                if ($scope.playgrounds && $scope.gameLoaded == false) {
                    $scope.gameLoaded = true;
                    $scope.loadData();
                }
            });

            $scope.dateOptions = {
                showWeeks: false,
                startingDay: 1
            };

            $scope.$watch('today', function () {
                $scope.loadData();
            });

            $scope.loadData = function () {
                $rootScope.isLoaded = false;
                var response = Games.search({
                    date: $filter('date')($scope.today, 'yyyy-MM-dd')
                }, function () {
                    // UPDATE VIEW
                    $scope.games = response.data;
                    $scope.count = response.count;
                    $location.hash('games');
                    $anchorScroll();
                    $rootScope.isLoaded = true;
                }, function (response) {
                    // SHOW ERROR
                    $rootScope.isLoaded = true;
                    Notification.error({ message: response.data.message, title: 'Произошла ошибка' , positionY: 'bottom', positionX: 'right' });
                });
            };

            $scope.getPage = function (page) {
                if ($scope.pageNumber != page) {
                    $scope.pageNumber = page;

                    $scope.loadData();
                }
            };
    }]});