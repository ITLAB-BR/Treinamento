angular
    .module('itlabtreinamento')
    .directive('footer', function () {
        return {
            restrict: 'E',
            templateUrl: '/app/directives/elements/footer.html'
        };
    });