/**
 * @by          IT Lab
 * @author      camilla.vianna
 *
 * @desc        filtro para converter data em formato apresentável
 * @params      format: string          formato que deverá ser exibido
 * 
 * @example     {{ viewModel.date | moment: "L" }}
 * 
 * ver mais conversões em directives/moment.directive.js
 * tipos de string válidas para format -> http://momentjs.com/
 */

(function () {
    angular.module('itlabtreinamento').filter('moment', momentFilter);

    function momentFilter() {
        return function (date, format) {
            return moment(date).format(format);
        };
    };
})();
