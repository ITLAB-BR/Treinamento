/*
 * @author:         camilla.vianna
 * 
 * @description:    diretiva para pegar o conteúdo do menu atraves do arquivo
 *                  json, gerar o menu e aplicar o metis
 * 
 */

(function () {
    'use strict';

    //Directive used to set metisMenu and minimalize button
    angular.module('itlabtreinamento')
        .directive('sideNavigation', sideNavigationDirective);

    sideNavigationDirective.$inject = ['$http', '$timeout'];
    function sideNavigationDirective($http, $timeout) {
        return {
            restrict: 'A',
            link: function (scope, element) {
                $timeout(function () {
                    element.metisMenu();
                });
            }
        };
    };
})();