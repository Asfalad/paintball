angular.module('pbResources').
    factory('MyGames', ['$resource',
        function ($resource) {
            return $resource('/papi/mygames/:id', { id: '@id' }, {
                "search": {
                    method: "GET",
                    params: { pageNumber: 1, pageSize: 15, descending: false }
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);