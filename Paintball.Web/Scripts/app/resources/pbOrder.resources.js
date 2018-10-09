angular.module('pbResources').
    factory('Order', ['$resource',
        function ($resource) {
            return $resource('/papi/order/:id', {}, {
                "search": {
                    method: "GET"
                },
                "post": {
                    method: "POST",
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }
            });
        }]);