/*
 * @by          IT Lab
 * @author      camilla.vianna
 *
 * @desc        diretiva para o componente spinner (loader)
 * @params      shape¹  string, default 'rotating-plane'    
 *              show    bool        
 *              ¹ opções de shape: [ rotating-plane | double-bounce | wave | wandering-cubes | pulse |
 *                                  chasing-dots | three-bounce | circle | cube-grid | fading-circle ]
 *              class   spinner-*   define a cor do spinner
 *                      [ spinner-primary | spinner-success | spinner-warning | spinner-danger | spinner-info ] 
 * @example     <spinner class="spinner-primary" shape="double-bounce" show="viewModel.spinnerShow" />
 *              
 * veja as animações de cada forma em -> http://webapplayers.com/inspinia_admin-v2.5/spinners.html
 */

(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .directive('spinner', function () {
            var shapesHtml = _shapesHtml();

            var directive = {
                restrict: 'E',
                scope: { shape: '@', show: '=' },
                template: '<div class="bg-loader"><div class="loader" ng-bind-html="shapeHtml"></div></div>',
                link: link
            };
            return directive;

            function link(scope, el) {
                var shapeDefault = 'rotating-plane';
                if (!scope.shape) scope.shape = shapeDefault;
                scope.shapeHtml = shapesHtml[scope.shape] || shapesHtml[shapeDefault];

                scope.$watch('show', function (newVal) {
                    if (newVal)
                        $(el).show();
                    else
                        $(el).hide();
                })
            };
            function _shapesHtml() {
                return {
                    'rotating-plane': '<div class="sk-spinner sk-spinner-rotating-plane sk-shape"></div>',

                    'double-bounce': '<div class="sk-spinner sk-spinner-double-bounce">\
                                <div class="sk-double-bounce1 sk-shape"></div>\
                                <div class="sk-double-bounce2 sk-shape"></div>\
                              </div>',

                    'wave': '<div class="sk-spinner sk-spinner-wave">\
                        <div class="sk-rect1 sk-shape"></div>\
                        <div class="sk-rect2 sk-shape"></div>\
                        <div class="sk-rect3 sk-shape"></div>\
                        <div class="sk-rect4 sk-shape"></div>\
                        <div class="sk-rect5 sk-shape"></div>\
                     </div>',

                    'wandering-cubes': '<div class="sk-spinner sk-spinner-wandering-cubes">\
                                    <div class="sk-cube1 sk-shape"></div>\
                                    <div class="sk-cube2 sk-shape"></div>\
                                </div>',

                    'pulse': ' <div class="sk-spinner sk-spinner-pulse sk-shape"></div>',

                    'chasing-dots': '<div class="sk-spinner sk-spinner-chasing-dots">\
                                <div class="sk-dot1 sk-shape"></div>\
                                <div class="sk-dot2 sk-shape"></div>\
                             </div>',

                    'three-bounce': '<div class="sk-spinner sk-spinner-three-bounce">\
                                <div class="sk-bounce1 sk-shape"></div>\
                                <div class="sk-bounce2 sk-shape"></div>\
                                <div class="sk-bounce3 sk-shape"></div>\
                             </div>',
                    'circle': '<div class="sk-spinner sk-spinner-circle">\
                        <div class="sk-circle1 sk-circle"></div>\
                        <div class="sk-circle2 sk-circle"></div>\
                        <div class="sk-circle3 sk-circle"></div>\
                        <div class="sk-circle4 sk-circle"></div>\
                        <div class="sk-circle5 sk-circle"></div>\
                        <div class="sk-circle6 sk-circle"></div>\
                        <div class="sk-circle7 sk-circle"></div>\
                        <div class="sk-circle8 sk-circle"></div>\
                        <div class="sk-circle9 sk-circle"></div>\
                        <div class="sk-circle10 sk-circle"></div>\
                        <div class="sk-circle11 sk-circle"></div>\
                        <div class="sk-circle12 sk-circle"></div>\
                       </div>',

                    'cube-grid': '<div class="sk-spinner sk-spinner-cube-grid">\
                            <div class="sk-cube sk-shape"></div>\
                            <div class="sk-cube sk-shape"></div>\
                            <div class="sk-cube sk-shape"></div>\
                            <div class="sk-cube sk-shape"></div>\
                            <div class="sk-cube sk-shape"></div>\
                            <div class="sk-cube sk-shape"></div>\
                            <div class="sk-cube sk-shape"></div>\
                            <div class="sk-cube sk-shape"></div>\
                            <div class="sk-cube sk-shape"></div>\
                          </div>',

                    'fading-circle': '<div class="sk-spinner sk-spinner-fading-circle">\
                                    <div class="sk-circle1 sk-circle"></div>\
                                    <div class="sk-circle2 sk-circle"></div>\
                                    <div class="sk-circle3 sk-circle"></div>\
                                    <div class="sk-circle4 sk-circle"></div>\
                                    <div class="sk-circle5 sk-circle"></div>\
                                    <div class="sk-circle6 sk-circle"></div>\
                                    <div class="sk-circle7 sk-circle"></div>\
                                    <div class="sk-circle8 sk-circle"></div>\
                                    <div class="sk-circle9 sk-circle"></div>\
                                    <div class="sk-circle10 sk-circle"></div>\
                                    <div class="sk-circle11 sk-circle"></div>\
                                    <div class="sk-circle12 sk-circle"></div>\
                                </div>'
                }
            };
        })
})();