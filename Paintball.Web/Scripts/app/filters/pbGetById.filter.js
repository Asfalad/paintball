angular.module('pbFilters')
    .filter('getById', function () {
        return function (array, id) {
            var i = 0; len = array.length;
            for (; i < len; i++) {
                if (+array[i].id == +id) {
                    return array[i].name;
                }
            }
            return null;
        }
    });