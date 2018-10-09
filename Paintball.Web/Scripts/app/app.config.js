angular.
    module('paintballApp').
    config(['$locationProvider', '$routeProvider', '$httpProvider',
        function ($locationProvider, $routeProvider, $httpProvider) {
            $locationProvider.hashPrefix('!');
            $httpProvider.interceptors.push('authInterceptorService');

            // main
            $routeProvider.when("/", {
                template: '<pb-Index></pb-Index>'
            }).when("/login", {
                template: '<pb-Login></pb-Login>'
            }).when("/register", {
                template: '<pb-Register></pb-Register>'
            }).when("/admin", {
                template: '<pb-Admin></pb-Admin>'
            }).when("/order/:id", {
                template: '<pb-Order-Game></pb-Order-Game>'
            })
            // mygames
            .when("/mygames", {
                template: '<pb-My-Games-List></pb-My-Games-List>'
            }).when("/mygames/update/:id", {
                template: '<pb-My-Game-Update></pb-My-Game-Update>'
            }).when("/mygames/delete/:id", {
                template: '<pb-My-Game-Delete></pb-My-Game-Delete>'
            })
            // company
            .when("/company", {
                template: '<pb-Company></pb-Company>'
            }).when("/company/create", {
                template: '<pb-Company-Create></pb-Company-Create>'
            }).when("/company/update", {
                template: '<pb-Company-Update></pb-Company-Update>'
            }).when("/company/delete", {
                template: '<pb-Company-Delete></pb-Company-Delete>'
            })

            // events
            .when("/events", {
                template: '<pb-Events-List></pb-Events-List>'
            }).when("/events/create", {
                template: '<pb-Event-Create></pb-Event-Create>'
            }).when("/events/update/:id", {
                template: '<pb-Event-Update></pb-Event-Update>'
            }).when("/event/delete/:id", {
                template: '<pb-Event-Delete></pb-Event-Delete>'
            }).when("/events/:id", {
                template: '<pb-Event-Read></pb-Event-Read>'
            })
            // news
            .when("/news", {
                template: '<pb-News-List></pb-News-List>'
            }).when("/news/create", {
                template: '<pb-News-Create></pb-News-Create>'
            }).when("/news/update/:id", {
                template: '<pb-News-Update></pb-News-Update>'
            }).when("/news/delete/:id", {
                template: '<pb-News-Delete></pb-News-Delete>'
            }).when("/news/:id", {
                template: '<pb-News-Read></pb-News-Read>'
            })
            // certificates
            .when("/company/certificates", {
                template: '<pb-Certificates-List></pb-Certificates-List>'
            }).when("/company/certificates/create", {
                template: '<pb-Certificate-Create></pb-Certificate-Create>'
            }).when("/company/certificates/update/:id", {
                template: '<pb-Certificate-Update></pb-Certificate-Update>'
            }).when("/company/certificates/delete/:id", {
                template: '<pb-Certificate-Delete></pb-Certificate-Delete>'
            })
            // games
            .when("/company/games", {
                template: '<pb-Games-List></pb-Games-List>'
            }).when("/company/games/create", {
                template: '<pb-Game-Create></pb-Game-Create>'
            }).when("/company/games/update/:id", {
                template: '<pb-Game-Update></pb-Game-Update>'
            }).when("/company/games/delete/:id", {
                template: '<pb-Game-Delete></pb-Game-Delete>'
            })
            // gametypes
            .when("/company/gametypes", {
                template: '<pb-Gametypes-List></pb-Gametypes-List>'
            }).when("/company/gametypes/create", {
                template: '<pb-Gametype-Create></pb-Gametype-Create>'
            }).when("/company/gametypes/update/:id", {
                template: '<pb-Gametype-Update></pb-Gametype-Update>'
            }).when("/company/gametypes/delete/:id", {
                template: '<pb-Gametype-Delete></pb-Gametype-Delete>'
            })
            // operations
            .when("/company/operations", {
                template: '<pb-Operations-List></pb-Operations-List>'
            }).when("/company/operations/create", {
                template: '<pb-Operation-Create></pb-Operation-Create>'
            }).when("/company/operations/update/:id", {
                template: '<pb-Operation-Update></pb-Operation-Update>'
            }).when("/company/operations/delete/:id", {
                template: '<pb-Operation-Delete></pb-Operation-Delete>'
            })
            // playgrounds
            .when("/company/playgrounds", {
                template: '<pb-Playgrounds-List></pb-Playgrounds-List>'
            }).when("/company/playgrounds/create", {
                template: '<pb-Playground-Create></pb-Playground-Create>'
            }).when("/company/playgrounds/update/:id", {
                template: '<pb-Playground-Update></pb-Playground-Update>'
            }).when("/company/playgrounds/delete/:id", {
                template: '<pb-Playground-Delete></pb-Playground-Delete>'
            })
            // storage
            .when("/company/storage", {
                template: '<pb-Storage-List></pb-Storage-List>'
            }).when("/company/storage/create", {
                template: '<pb-Storage-Create></pb-Storage-Create>'
            }).when("/company/storage/update/:id", {
                template: '<pb-Storage-Update></pb-Storage-Update>'
            }).when("/company/storage/delete/:id", {
                template: '<pb-Storage-Delete></pb-Storage-Delete>'
            })
            // users
            .when("/company/users", {
                template: '<pb-Users-List></pb-Users-List>'
            }).when("/company/users/create", {
                template: '<pb-User-Create></pb-User-Create>'
            }).when("/company/users/update/:id", {
                template: '<pb-User-Update></pb-User-Update>'
            }).when("/company/users/delete/:id", {
                template: '<pb-User-Delete></pb-User-Delete>'
            }).when("/company/users/:id", {
                template: '<pb-User-Read></pb-User-Read>'
            })
            // tasks
            .when("/tasks", {
                template: '<pb-Tasks-List></pb-Tasks-List>'
            })
            ;
            $routeProvider.otherwise("/");
        }]);