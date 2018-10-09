angular.module('pbResources').
    factory('Equipment', ['$resource',
        function ($resource) {
            return $resource('/papi/equipment/:id', { id: '@id' }, {
                "search": {
                    method: "GET",
                    params: { pageNumber: 1, pageSize: 15, descending: false }
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);