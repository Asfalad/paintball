angular.module('pbResources').
    factory('EquipmentOrders', ['$resource',
        function ($resource) {
            return $resource('/papi/equipmentorders/:id', { id: '@id' }, {
                "search": {
                    method: "GET",
                    params: { pageNumber: 1, pageSize: 15, descending: false }
                },
                "update": {
                    method: "PUT"
                }
            });
        }]);