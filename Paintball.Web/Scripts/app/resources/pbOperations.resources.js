angular.module('pbResources').
    factory('Operations', ['$resource',
        function ($resource) {
            return $resource('/papi/operations/:id', { id: '@id' }, {
                "search": {
                    method: "GET",
                    params: { pageNumber: 1, pageSize: 15, descending: false }
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);