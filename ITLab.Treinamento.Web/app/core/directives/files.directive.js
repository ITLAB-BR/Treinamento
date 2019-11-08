/**
 * @by          IT Lab
 * @author      marcelo.neias
 * @author      camilla.vianna
 *
 * @desc        diretivas para formatar valores numéricos de forma internacionalizada
 *
 * @example     <input type="file" class="form-control hide" ng-files="viewModel.file" />
 *              * controller.js 
                function Controller(itemService, getModelAsFormData) {
                    // ... controller code ...
                    function submit() {
                        var request = getModelAsFormData(viewModel.edit, 'image');
                        // where viewModel.edit have the models of the form

                        itemService.set(request).then(function (response) {
                            // ... code ...
                        });
                    };
                };
 */

(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .directive('ngFiles', ngFilesDirective)
        .constant('getModelAsFormData', getModelAsFormData);

    ngFilesDirective.$inject = ['$parse'];
    function ngFilesDirective($parse) {
        var directive = {
            link: fn_link
        };
        return directive;

        function fn_link(scope, element, attrs) {
            var onChange = $parse(attrs.ngFiles);
            var modelSetter = onChange.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files);
                });
            });
        };
    };


    function getModelAsFormData(data, nameModel) {
        var dataAsFormData = new FormData();
        nameModel = nameModel || 'filesToUpload';

        angular.forEach(data, function (value, key) {
            if (key == nameModel) {
                for (var i = 0; i < value.length; i++) {
                    dataAsFormData.append(value[i].name, value[i]);
                }
            } else {
                dataAsFormData.append(key, value);
            }
        });
        return dataAsFormData;
    };
})();