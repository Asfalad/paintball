angular.module('pbResources')
    .factory('Events', ['$resource', function ($resource) {
        return $resource('/papi/events/:id', { id: '@id' }, {
            "search": {
                method: "GET",
                params: { pageNumber: 1, pageSize: 15, descending: false }
            },
            "update": {
                method: "PUT"
            }
        });
    }]);