angular.module('pbResources').
    factory('Tasks', ['$resource',
        function ($resource) {
            return $resource('/papi/tasks/:id', { id: '@id' }, {
                "search": {
                    method: "GET",
                    params: { pageNumber: 1, pageSize: 15, descending: false }
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);