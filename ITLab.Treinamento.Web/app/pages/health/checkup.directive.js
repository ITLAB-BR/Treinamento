(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .directive('resultWidget', resultWidget)
        .directive('emailResultWidget', emailResultWidget);

    function resultWidget() {
        return {
            responsetrict: 'E',
            scope: {
                successText: '@',
                failText: '@',
                failDesc: '@',
                result: '='
            },
            template: template(),
            controller: controller
        };


        function template() {
            return '<div class="result {{resultClass}}">\
                           <div class="widget navy-bg p-lg text-center animated fadeInUp success">\
                               <div class="row">\
                                   <div class="col-xs-3"><i class="fa fa-5x m-t-n-xs fa-check-circle"></i></div>\
                                   <div class="col-xs-9"><h1>{{i18n_successText}}</h1></div>\
                               </div>\
                           </div>\
                           <div class="widget red-bg p-lg text-center animated fadeInDown fail">\
                               <div class="row">\
                                   <div class="col-xs-3"><i class="fa fa-5x fa-warning {{failDesc?\'m-t-xs\':\'m-t-n-xs\'}}"></i></div>\
                                   <div class="col-xs-9">\
                                       <h1>{{i18n_failText}}</h1>\
                                       <p>{{i18n_failDesc}}</p>\
                                   </div>\
                               </div>\
                           </div>\
                       </div>';
        };

        function controller($scope) {
            $scope.i18n_successText = i18n.t($scope.successText);
            $scope.i18n_failText = i18n.t($scope.failText);
            $scope.i18n_failDesc = i18n.t($scope.failDesc);

            $scope.$watch('result', function (newVal, oldVal) {
                if (newVal == oldVal) return;
                if (newVal == undefined) $scope.resultClass = '';
                else if (newVal) $scope.resultClass = 'success'
                else $scope.resultClass = 'fail'
            });
        };


    };
    function emailResultWidget() {
        return {
            responsetrict: 'E',
            scope: {
                successText: '@',
                successDesc: '@',
                failText: '@',
                failDesc: '@',
                result: '='
            },
            template: template,
            controller: controller
        };

        function template() {
            return '<div class="result {{resultClass}}">\
                           <div class="widget lazur-bg animated fadeInUp success">\
                               <div class="row">\
                                   <div class="col-xs-4">\
                                       <i class="fa fa-5x fa-envelope-square {{successDesc?\'m-t-xs\':\'m-t-n-xs\'}}"></i>\
                                   </div>\
                                   <div class="col-xs-8 text-right">\
                                       <h3>{{i18n_successText}}</h3>\
                                       <span> {{i18n_successDesc}} </span>\
                                   </div>\
                               </div>\
                           </div>\
                           <div class="widget red-bg p-lg text-center animated fadeInDown fail">\
                               <div class="row">\
                                   <div class="col-xs-3"><i class="fa fa-5x fa-warning {{failDesc?\'m-t-xs\':\'m-t-n-xs\'}}"></i></div>\
                                   <div class="col-xs-9">\
                                       <h1>{{i18n_failText}}</h1>\
                                       <p>{{i18n_failDesc}}</p>\
                                   </div>\
                               </div>\
                           </div>\
                       </div>';
        };

        controller.$inject = ['$scope'];
        function controller($scope) {
            $scope.i18n_successText = i18n.t($scope.successText) || i18n.t('alerts:success.success');
            $scope.i18n_successDesc = i18n.t($scope.successDesc);
            $scope.i18n_failText = i18n.t($scope.failText) || i18n.t('alerts:error.fail');
            $scope.i18n_failDesc = i18n.t($scope.failDesc);

            $scope.$watch('result', function (newVal, oldVal) {
                if (newVal == oldVal) return;
                if (newVal == undefined) $scope.resultClass = '';
                else if (newVal) $scope.resultClass = 'success'
                else $scope.resultClass = 'fail'
            });
        }
    };
})();