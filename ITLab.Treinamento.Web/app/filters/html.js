/**
 * @by          IT Lab
 * @author      camilla.vianna
 *
 * @desc        filtro para converter texto recebido do serviço em texto html
 *              * substituição de quebras de linha para tag <br>
 *              * substituição de espaços consecutivos para a entidade de espaço &ensp;
 * @example     <span ng-bind="viewModel.text | html"></span>
 */

angular.module('itlabtreinamento')
  .filter('html', function () {
      return function (text) {
          var html = text.replace(/\r\n/g, '<br>')
                         .replace(/\s\s/g, '&ensp;&ensp;')

          return html;
      };
  });

