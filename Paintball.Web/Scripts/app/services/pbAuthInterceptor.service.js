angular.module('pbService').
    factory('authInterceptorService', ['$q', '$location', '$cookies',
        function ($q, $location, $cookies) {
            var authInterceptorServiceFactory = {};

            var _request = function (config) {
                config.headers = config.headers || {};

                var authData = $cookies.getObject('authorizationData');
                if (authData) {
                    config.headers.Authorization = 'Bearer ' + authData.token;
                }

                return config;
            }

            var _responseError = function (rejection) {
                if (rejection.status === 401) {
                    $location.url('/login?returnUrl='+$location.url());
                }
                return $q.reject(rejection);
            }

            authInterceptorServiceFactory.request = _request;
            authInterceptorServiceFactory.responseError = _responseError;

            return authInterceptorServiceFactory;
        }]);