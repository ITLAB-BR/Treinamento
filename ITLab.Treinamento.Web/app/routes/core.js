angular
    .module('itlabtreinamento')
    .config(['$navigationProvider', function (nav) {
        nav.map([
            {
                name: 'accessdenied',
                route: '/unauthorized',
                template: 'app/pages/errors/unauthorized.html'
            },
            {
                name: '404',
                route: '/404',
                template: 'app/pages/errors/not-found.html'
            },
            {
                name: '500',
                route: '/500',
                template: 'app/pages/errors/internal-server-error.html'
            },
            {
                name: 'notifications',
                route: '/notifications',
                template: 'app/pages/notifications/list.html',
                controller: 'NotificationsController',
                bundle: [
                    'app/pages/notifications/list.controller.js'
                ]
            },
            {
                name: 'home',
                route: '/home',
                title: 'Home',
                icon: 'home',
                template: 'app/pages/home/list.html',
                controller: 'homeController',
                bundle: ['app/pages/home/list.controller.js'],
                nav: 'root',
                order: 0
            }
        ]);
    }])
    .run(['$rootScope', '$state', 'principal', 'authorization', function ($rootScope, $state, principal, authorization) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toStateParams) {
            $rootScope.toState = toState;
            $rootScope.toStateParams = toStateParams;

            if (principal.isIdentityResolved())
                authorization.authorize();
        });

        $rootScope.$on('$stateNotFound', function (event, unfoundState, fromState, fromParams) {
            $state.go('404');
        });
        
        $rootScope.$on('$viewContentLoaded', function (event) {
            if (event.targetScope && event.targetScope.loaded) {
                event.targetScope.loaded();
            }
        });
    }]);