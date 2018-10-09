angular.module('pbResources').
    factory('GameTypes', ['$resource',
        function ($resource) {
            return $resource('/papi/gametypes/:id', { id: '@id' }, {
                "search": {
                    method: "GET",
                    params: { pageNumber: 1, pageSize: 15, descending: false }
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);