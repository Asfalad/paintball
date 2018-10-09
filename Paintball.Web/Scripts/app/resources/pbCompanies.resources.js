angular.module('pbResources').
    factory('Companies', ['$resource',
        function ($resource) {
            return $resource('/papi/companies/:id', { id: '@id' }, {
                "search":{
                    method: "GET",
                    params: {pageNumber: 1, pageSize: 15, descending: false}
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);