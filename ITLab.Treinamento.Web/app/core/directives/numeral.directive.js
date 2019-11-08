/**
 * @by          IT Lab
 * @author      camilla.vianna
 *
 * @desc        diretivas para formatar valores numéricos de forma internacionalizada
 * @example     <input ng-model="viewModel.numberValue" input-numeral format="0,0.0" />
 * @example     <span ng-numeral="viewModel.numberValue" format="0,0.0"></span>
 */

(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .directive('inputNumeral', inputNumeralDirective)
        .directive('ngNumeral', numeralDirective)
        .filter('numeral', numeralFilter);

    inputNumeralDirective.$inject = ['$window']
    function inputNumeralDirective($window) {
        var directive = {
            require: 'ngModel',
            restrict: 'A',
            scope: {
                ngModel: '=', //String (formatted)
                format: '@'
            },
            link: link
        };
        return directive;

        function link(scope, element, attrs, modelCtrl) {
            var $el = $(element);
            var format = scope.format || '0,0.00';

            modelCtrl.$render = function () {
                modelCtrl.$viewValue = numeral(modelCtrl.$viewValue).format(format);
                modelCtrl.$modelValue = numeral(modelCtrl.$viewValue).value();

                $el.val(modelCtrl.$viewValue)
            };

            element.bind('blur', modelCtrl.$render);

            element.on('click', function () {
                $el.val(numeral(modelCtrl.$viewValue).format(format.replace('0,', '')));

                //// seleciona todo o texto do input quando clica
                //if (!$window.getSelection().toString()) {
                //    // Required for mobile Safari
                //    this.setSelectionRange(0, this.value.length);
                //}
            });
        };
    };

    function numeralDirective() {
        var directive = {
            restrict: 'A',
            scope: {
                ngNumeral: '=',
                format: '@'
            },
            template: '{{numberFormatted()}}',
            link: link
        };
        return directive;

        function link(scope) {
            scope.numberFormatted = function () {
                return numeral(scope.ngNumeral).format(scope.format);
            };

            //scope.$watch('ngNumeral', function (newValue, oldValue) {
            //    if (newValue == oldValue) return;
            //    numberFormat();
            //});
        };
    };

    function numeralFilter() {
        return function (number, option) {
            var format = option && option.format,
                emptyIfZero = option && option.emptyIfZero;

            var formatted = numeral(number).format(format);
            if (emptyIfZero)
                return formatted == "0" ? "" : formatted;
            return formatted;
        };
    }
})();