(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .directive('totalizer', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: { listLength: '=length' },
                template: '<p class="m-sm table-totalizer" ng-bind-html="\'commons.table_totalizer\' | ngI18n: { X: \'<b>\' + listLength + \'</b>\' }"></p>'
            };
        });
})();