angular.module('pbFilters')
    .filter('length', function () {
        return function (input, length) {
            return new String(input).substr(0, length);
        }
    });