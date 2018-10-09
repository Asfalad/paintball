angular.module('pbResources').
    factory('News', ['$resource',
        function ($resource) {
            return $resource('/papi/news/:id', { id: '@id' }, {
                "search": {
                    method: "GET",
                    params: { pageNumber: 1, pageSize: 15, descending: false }
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);