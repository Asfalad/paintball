angular.module('pbService').
    factory('authService', ['$http', '$q', '$cookies', '$window',
        function ($http, $q, $cookies, $window) {
        var serviceBase = 'http://localhost:59497/';
        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: "",
            companyId: 0,
            isAdmin: false,
            isUser: false,
            isCompanyOwner: false,
            isCompanyStaff: false,
            isCertificatesModify: false,
            isCompanyModify: false,
            isEquipmentModify: false,
            isEventsModify: false,
            isGameTypesModify: false,
            isNewsModify: false,
            isOperationsRead: false,
            isOperationsModify: false,
            isPlaygroundsModify: false
        };

        var _saveRegistration = function (registration) {
            _logOut();

            return $http.post(serviceBase+'papi/account/register', registration).
                then(function (response) {
                    return response;
                });
        }

        var _login = function (loginData) {
            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            var deferred = $q.defer();

            $http({method: 'POST', url: serviceBase + "token", data: data, headers: { 'Content-Type': 'application/x-www-form-urlencoded' }}).
                then(function (response) {
                    $cookies.putObject('authorizationData', {
                        token: response.data.access_token,
                        companyId: response.data.companyId,
                        userName: response.data.userName,
                        isAdmin: response.data.roles.includes('Admin'),
                        isUser: response.data.roles.includes('User'),
                        isCompanyOwner: response.data.roles.includes('CompanyOwner'),
                        isCompanyStaff: response.data.roles.includes('CompanyStaff'),
                        isCertificatesModify: response.data.roles.includes('CertificatesModify'),
                        isCompanyModify: response.data.roles.includes('CompanyModify'),
                        isEquipmentModify: response.data.roles.includes('EquipmentModify'),
                        isEventsModify: response.data.roles.includes('EventsModify'),
                        isGameTypesModify: response.data.roles.includes('GameTypesModify'),
                        isNewsModify: response.data.roles.includes('NewsModify'),
                        isOperationsRead: response.data.roles.includes('OperationsRead'),
                        isOperationsModify: response.data.roles.includes('OperationsModify'),
                        isPlaygroundsModify: response.data.roles.includes('PlaygroundsModify')
                    });
                    deferred.resolve(response);
                }, function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });

            return deferred.promise;
        }

        var _logOut = function () {
            $cookies.remove('authorizationData');
            _authentication.isAuth = false;
            _authentication.userName = "";
            _authentication.companyId = 0;
            _authentication.isAdmin= false;
            _authentication.isUser= false;
            _authentication.isCompanyOwner= false;
            _authentication.isCompanyStaff= false;
            _authentication.isCertificatesModify= false;
            _authentication.isCompanyModify= false;
            _authentication.isEquipmentModify= false;
            _authentication.isEventsModify= false;
            _authentication.isGameTypesModify= false;
            _authentication.isNewsModify= false;
            _authentication.isOperationsRead= false;
            _authentication.isOperationsModify= false;
            _authentication.isPlaygroundsModify = false

            $window.location.reload();
        }

        var _fillAuthData = function () {
            var authData = $cookies.getObject('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.userName = authData.userName;
                _authentication.companyId = authData.companyId;
                _authentication.isAdmin = authData.isAdmin;
                _authentication.isUser = authData.isUser;
                _authentication.isCompanyOwner = authData.isCompanyOwner;
                _authentication.isCompanyStaff = authData.isCompanyStaff;
                _authentication.isCertificatesModify = authData.isCertificatesModify;
                _authentication.isCompanyModify = authData.isCompanyModify;
                _authentication.isEquipmentModify = authData.isEquipmentModify;
                _authentication.isEventsModify = authData.isEventsModify;
                _authentication.isGameTypesModify = authData.isGameTypesModify;
                _authentication.isNewsModify = authData.isNewsModify;
                _authentication.isOperationsRead = authData.isOperationsRead;
                _authentication.isOperationsModify = authData.isOperationsModify;
                _authentication.isPlaygroundsModify = authData.isPlaygroundsModify;
            }
        }

        authServiceFactory.saveRegistration = _saveRegistration;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;

        return authServiceFactory;
    }]);