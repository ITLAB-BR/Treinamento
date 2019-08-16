/**
 * @by          IT Lab
 * @author      camilla.vianna
 *
 * @desc        diretiva para converter data¹ em formato apresentável
 *              ¹ tipos de data: JS (new Date()),
 *                               JSON,
 *                               Server ("/Date(12345600000)/"),
 *                               string customizada,
 *                               momentJs
 * @params        ngMoment:     object      [Date | JSON | String...]
 *                format:       string      formato que deverá ser exibido²
 *                momentFormat: string      função moment de formato³ [calendar|fromNow]
 *                formatFrom:   string      formato do qual está sendo convertido 
 *                                          no caso o ngMoment seja uma string específica
 *                update:       object      
 * @example     <span ng-moment="viewModel.date" format="L"></span>
 * @example     <span ng-moment="viewModel.date" moment-format="calendar"></span>
 * @example     <span ng-moment="viewModel.stringDate" formatFrom="YYYY-MM-DD" format="L"></span>
 * 
 * ² ³ tipos de string válidas para format e funções calendar e fromNow -> http://momentjs.com/
 */
(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .directive('ngMoment', momentDirective)
        .run(momentGlobal);

    momentDirective.$inject = ['$interval'];
    function momentDirective($interval) {
        var updateOptions = {
            each: 1000, //ms
            stopAfterDate: function (dateTime, stopAfter) {
                return moment(dateTime).add(stopAfter.time, stopAfter.unit);
            },
            stopAfter: false // ex.: { time: 10, unit: 'm' }
        };

        var directive = {
            restrict: 'A',
            scope: {
                ngMoment: '=',
                format: '@',
                momentFormat: '@',
                update: '=',
                formatFrom: '@'
            },
            template: '<span ng-bind="dateFormatted()"></span>',
            link: link
        };
        return directive;

        function link(scope) {
            scope.dateFormatted = function () {
                var dateMoment;
                if (typeof scope.ngMoment == 'string' && typeof scope.formatFrom == 'string')
                    dateMoment = moment(scope.ngMoment, scope.formatFrom);
                else dateMoment = moment(scope.ngMoment);

                if (scope.format)
                    return dateMoment.format(scope.format);
                else if (scope.momentFormat)
                    return dateMoment[scope.momentFormat]();

                return dateMoment.format();
            };

            scope.$watch('update', function (value, old) {
                if (value) {
                    var update = angular.extend({}, updateOptions, value);
                    // TODO: ainda dá pra melhorar issso
                    if (update.stopAfter && typeof update.stopAfterDate == 'function')
                        update.stopAfterDate = update.stopAfterDate(scope.ngMoment, update.stopAfter);
                    else if (typeof update.stopAfterDate != 'function')
                        update.stopAfterDate = moment(update.stopAfterDate);
                    else
                        update.stopAfterDate = false;

                    var timer = $interval(function () {
                        if (update.stopAfterDate && update.stopAfterDate.isBefore())
                            $interval.cancel(timer);
                    }, update.each);
                }
            })
        }
    };

    momentGlobal.$inject = ['$rootScope'];
    function momentGlobal($rootScope) {
        /*
         isto é para formatar data de modo global
         usado para o datepicker
         TODO: melhorar descrição e add exemplo
         */

        var config = {
            getStringFormat: getStringFormat
        };
        $rootScope.moment = config;
        function getStringFormat(format) {
            return moment.localeData()._longDateFormat[format || 'L'].replace(/D/g, 'd').replace(/Y/g, 'y');
        }
    }
})();
