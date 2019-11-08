/**
 * @by          IT Lab
 * @author      camilla.vianna
 *
 * @desc        diretiva para formatar hora para HH:mm
 *              horário é validado e formatado no event blur no input
 *
 * @example     <span ng-moment="viewModel.date" ng-Time></span>
 * @example     <span ng-moment="viewModel.date" ng-Time clearable></span>
 * 
 */
(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .directive('ngTime', directive);

    function directive() {
        var directive = {
            require: 'ngModel',
            restrict: 'A',
            scope: { ngModel: '=' },
            link: link
        };
        return directive;

        function link(scope, element, attrs, modelCtrl) {
            var clearable = attrs.hasOwnProperty('clearable');
            if (!attrs.hasOwnProperty('mask'))
                attrs.mask = '2?9:59';

            element.on('blur', function () {
                var value = moment(scope.ngModel, 'H:m');
                if (value.isValid())
                    scope.ngModel = value.format('HH:mm');
                else if (!clearable)
                    scope.ngModel = '00:00';
            });
        };
    };

})();
