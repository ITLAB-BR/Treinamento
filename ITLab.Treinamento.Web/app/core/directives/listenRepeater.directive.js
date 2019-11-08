/*
 * @solutionFrom    https://www.linkedin.com/pulse/20140822090331-106754325-execute-code-after-ng-repeat-renders-the-items
 */

(function () {
    angular.module('itlabtreinamento').directive('emitLastRepeaterElement', diretive);

    diretive.$inject = ['$timeout'];
    function diretive($timeout) {
        return function (scope) {
            if (scope.$last) {
                $timeout(function () {
                    scope.$emit('LastRepeaterElement');
                });
            }
        };
    }
})();