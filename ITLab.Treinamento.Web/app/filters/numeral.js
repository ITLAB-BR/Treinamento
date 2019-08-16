/**
 * @by          IT Lab
 * @author      camilla.vianna
 *
 * @desc        filtro para formatar valores numéricos de forma internacionalizada
 * @example     {{ viewModel.numberValue | numeral: "0,0.00" }}
 */

(function () {
    angular.module('itlabtreinamento').filter('numeral', numeralFilter);

    function numeralFilter() {
        return function (number, format) {
            return numeral(number).format(format);
        };
    };
})();
