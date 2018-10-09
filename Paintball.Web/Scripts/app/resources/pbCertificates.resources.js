angular.module('pbResources').
    factory('Certificates', ['$resource',
        function ($resource) {
            return $resource('/papi/certificates/:id', { id: '@id' }, {
                "search":{
                    method: "GET",
                    params: {pageNumber: 1, pageSize: 15, descending: false}
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);