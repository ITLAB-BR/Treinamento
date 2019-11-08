angular
    .module('itlabtreinamento')
    .provider('$navigation', ['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        var self = this;
        var nav = { items: {} };
        var layouts = {};

        var getLayoutStateName = function (settings) {
            var name = settings.anonymous ? 'anonymous' : 'authorized';
            name += settings.layout;

            return name;
        };

        var mapLayoutState = function (settings) {
            settings.layout = settings.layout || 'content';

            var name = getLayoutStateName(settings);

            if (!layouts[name]) {
                layouts[name] = {
                    name: name,
                    templateUrl: 'app/layouts/' + settings.layout + '.html'
                };

                if (!settings.anonymous)
                    layouts[name].resolve = {
                        authorize: ['authorization', function (authorization) { return authorization.authorize(); }]
                    };

                $stateProvider.state(layouts[name]);
            }
        };

        var mapState = function (settings) {
            mapLayoutState(settings);

            var state = {
                name: settings.name,
                parent: getLayoutStateName(settings),
                url: settings.route,
                templateUrl: settings.template,
                controller: settings.controller,
                controllerAs: 'viewModel',
                resolve: {
                    deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'atlastca',
                            insertBefore: '#ng_load_plugins_before',
                            files: settings.bundle
                        });
                    }]
                },
                data: {
                    pageTitle: settings.title,
                    roles: settings.roles
                }
            }

            $stateProvider.state(state);
        };

        var mapNav = function (settings) {
            var item = nav.items[settings.name] || {};
            item.name = settings.name;
            item.title = settings.title;
            item.icon = settings.icon;
            item.roles = settings.roles;
            item.order = settings.order || 0;

            if (settings.nav == 'root')
                nav.items[settings.name] = item;
            else {
                var parent = nav.items[settings.nav] || {};
                parent.items = parent.items || {};
                parent.items[settings.name] = item;
            }
        };

        var toArray = function (items) {
            var result = [];

            for (var i in items) {
                var child = angular.copy(items[i]);
                child.items = toArray(child.items);
                result.push(child);
            }

            return result.sort(function (a, b) {
                var n = a.order - b.order;
                if (n !== 0) return n;

                if (a.name < b.name)
                    return -1;

                if (a.name > b.name)
                    return 1;

                return 0;
            });
        }

        this.map = function (settings) {
            var _settings = angular.isArray(settings) ? settings : [settings];

            settings.forEach(function (s) {
                if (s.route) mapState(s);
                if (s.nav) mapNav(s);
            });

            return self;
        };

        this.$get = function () {
            return {
                getTree: function () {
                    return toArray(nav.items);
                }
            };
        };

        $urlRouterProvider.otherwise('/home');
    }]);