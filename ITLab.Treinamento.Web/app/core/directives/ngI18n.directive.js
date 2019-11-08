/**
 * @by          IT Lab
 * @author      camilla.vianna
 *
 * @desc        diretivas para formatar valores numéricos de forma internacionalizada
 * @example     <span ng-i18n="app.name"></span>
 * @example     <input ng-i18n-placeholder="app.name" />
 * @example     <span ng-i18n="app.name" ng-i18n-title="app.name"></span>
 * @example     <span ng-i18n-helper="app.name"></span>
 */

(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .directive('ngI18n', ngI18nDirective)
        .directive('ngI18nPlaceholder', ngI18nPlaceholderDirective)
        .directive('ngI18nTitle', ngI18nTitleDirective)
        .directive('ngI18nHelper', ngI18nHelperDirective)
        .filter("ngI18n", ngI18nFilter);

    function ngI18nDirective() {
        var directive = {
            scope: { ngI18n: '@', interpolation: '=', transpolation: '=' },
            template: '{{translation()}}',
            link: link
        };
        return directive;

        function link(scope) {
            scope.translation = function () {
                var interpolation = scope.interpolation || {};
                if (scope.transpolation) {
                    var trans = scope.transpolation
                    for (var i in trans) {
                        interpolation[i] = i18n.t(trans[i]);
                    }
                }

                return i18n.t(scope.ngI18n, interpolation);
            };
        }
    };

    ngI18nPlaceholderDirective.$inject = ['$cookies'];
    function ngI18nPlaceholderDirective($cookies) {
        var directive = {
            scope: { ngI18nPlaceholder: '@' },
            link: link
        };
        return directive;

        function link(scope, element) {
            var attr = scope.ngI18nPlaceholder;
            element.attr('placeholder', i18n.t(attr));

            scope.$watch(function () { return $cookies.get('i18next'); },
                function () {
                    element.attr('placeholder', i18n.t(attr));
                });
        };
    };

    ngI18nTitleDirective.$inject = ['$cookies'];
    function ngI18nTitleDirective($cookies) {
        var directive = {
            scope: { ngI18nTitle: '@' },
            link: link
        };
        return directive;

        function link(scope, element) {
            var attr = scope.ngI18nTitle;
            element.attr('title', i18n.t(attr));

            scope.$watch(function () { return $cookies.get('i18next'); },
                function () {
                    element.attr('title', i18n.t(attr));
                });
        }
    };

    // TODO: arrumar bug do tooltip
    // NOTE: tooltip substituído por title temporariamente até resolver o problema
    function ngI18nHelperDirective() {
        var directive = {
            scope: { ngI18nHelper: '@', placement: '@' },
            template: '<i class="fa fa-question-circle question-helper" title="{{translation()}}"></i>',
            //template: '<i class="fa fa-question-circle question-helper" tooltip-placement="{{placement}}" '
            //            + 'uib-tooltip="{{translation()}}"></i>',
            link: link
        };
        return directive;

        function link(scope) {
            scope.translation = function () {
                return i18n.t(scope.ngI18nHelper);
            };

            if (!scope.placement) scope.placement = 'bottom-right';
        }
    };

    function ngI18nFilter() {
        return function (key, interpolation) {
            return i18n.t(key, interpolation);
        };
    }
})();