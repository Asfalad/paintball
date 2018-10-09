angular.module('pbMain')
    .component('pbLogOut', {
        template: '',
        controller: ['$location', 'authService',
            function PbLoginController($location, authService) {
                authService.logOut();
                alert('success');
                $location.path('/');
            }]
    });