angular.module('pbResources').
    factory('Games', ['$resource',
        function ($resource) {
            return $resource('/papi/games/:id', { id: '@id' }, {
                "search": {
                    method: "GET",
                    params: { pageNumber: 1, pageSize: 15, descending: false }
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);