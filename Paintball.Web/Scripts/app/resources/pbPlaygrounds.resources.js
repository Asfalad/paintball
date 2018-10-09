angular.module('pbResources').
    factory('Playgrounds', ['$resource',
        function ($resource) {
            return $resource('/papi/playgrounds/:id', { id: '@id' }, {
                "search": {
                    method: "GET",
                    params: { pageNumber: 1, pageSize: 15, descending: false }
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);