angular.module('pbResources').
    factory('Users', ['$resource',
        function ($resource) {
            return $resource('/papi/users/:id', { id: '@userName' }, {
                "search": {
                    method: "GET",
                    params: { pageNumber: 1, pageSize: 15, descending: false }
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);