angular.module('pbUsers').
    component('pbUserCreate', {
        templateUrl: '/app/staff/modify',
        controller: ['$rootScope', '$location', 'Users', 'Notification',
            function ($rootScope, $location, Users, Notification) {
                var self = this;
                $rootScope.cleanTabFlags();
                $rootScope.companyTab = true;
                $rootScope.isLoaded = false;

                self.message = '';

                self.user = {
                    email: '',
                    password: '',
                    confirmPassword: '',
                    firstName: '',
                    lastName: '',
                    middleName: '',
                    certificatesModify: false,
                    companyModify: false,
                    equipmentModify: false,
                    eventsModify: false,
                    gameTypesModify: false,
                    newsModify: false,
                    operationsRead: false,
                    operationsModify: false,
                    playgroundsModify: false,
                    salary: 0
                };

                self.isCreating = true;

                self.proceed = function () {
                    $rootScope.isLoaded = false;
                    Users.save(self.user,function () {
                        $location.url('/company/users');
                        Notification.success({ message: 'Сотрудник успешно добавлен', positionY: 'bottom', positionX: 'right' });
                    }, function (response) {
                        if (response.data.modelState) {
                            var errors = [];
                            for (var key in response.data.modelState) {
                                for (var i = 0; i < response.data.modelState[key].length; i++) {
                                    errors.push(response.data.modelState[key][i]);
                                }
                            }
                            self.message = errors.join(' ');
                            $rootScope.isLoaded = true;
                            Notification.error({ message: self.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                        }
                        else {
                            $location.url('/company/users');
                            Notification.error({ message: response.data.message, title: 'Произошла ошибка', positionY: 'bottom', positionX: 'right' });
                        }
                    });
                };

                $rootScope.isLoaded = true;
            }]
    });