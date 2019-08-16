angular
    .module('itlabtreinamento')
    .config(['$navigationProvider', function (nav) {
        nav.map([
            {
                name: 'examples',
                title: 'nav.examples',
                icon: 'flag',
                nav: 'root',
                order: 2
            },
            {
                name: 'examples-form',
                title: 'nav.form',
                route: '/examples/form',
                template: 'app/pages/examples/form.html',
                controller: 'FormController',
                bundle: [
                    'app/pages/examples/form.controller.js'
                ],
                nav: 'examples'
            },
            {
                name: 'examples-tree-table',
                title: 'Tree Table',
                route: '/examples/tree-table',
                template: 'app/pages/examples/tree-table.html',
                controller: 'TreeTableController',
                bundle: [
                    'app/pages/examples/tree-table.controller.js',
                    'app/pages/examples/tree-table.service.js'
                ],
                nav: 'examples'
            }
        ]);
    }]);