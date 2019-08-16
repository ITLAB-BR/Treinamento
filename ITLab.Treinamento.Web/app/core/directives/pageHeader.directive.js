/*
 * @by: IT Lab
 * @author: camilla.vianna
 *
 * @desc        diretiva para auxiliar na criação do cabeçalho e breadcrumb de cada página
 * @example     <page-header page-title="page_head.users" breadcrumb="['nav.security', 'nav.users']"></page-header>
 */

(function () {
    'use strict';

    angular.module('itlabtreinamento').directive('pageHeader', pageHeaderDirective);

    function pageHeaderDirective() {
        var directive = {
            restrict: 'E',
            scope: {
                pageTitle: '@',
                breadcrumb: '='
            },
            template: '<div class="row wrapper border-bottom white-bg page-heading">\
                        <div class="col-lg-10">\
                            <h2 class="m-b-md" ng-i18n="{{pageTitle}}"></h2>\
                            <ol class="breadcrumb" ng-show="breadcrumb">\
                                <li><a href="index.html">Home</a></li>\
                                <li ng-repeat="bc in breadcrumb" ng-i18n="{{bc}}"></li>\
                            </ol>\
                        </div>\
                        <div class="col-lg-2"></div>\
                      </div>'
        };

        return directive;
    };
})();