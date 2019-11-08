/**
 * @by          IT Lab
 * @author      camilla.vianna
 *
 * @desc        filtro para converter valor booleano em texto 'sim' ou 'não' traduzindo com o i18n
 * @example     <span ng-bind="viewModel.boolValue | yesno"></span>
 * @example     <span ng-bind="viewModel.boolValue | yesno: true"></span>
 */

(function () {
    angular.module('itlabtreinamento').filter('yesno', yesnoFilter);

    function yesnoFilter() {
        return function (bool, force) {
            if (!force && typeof bool !== 'boolean') return '';
            var text = bool ? 'label.yes' : 'label.no';
            return i18n.t(text);
        };
    }
})();
