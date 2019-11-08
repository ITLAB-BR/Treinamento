/**
 * @requires
 * angular-filter.js
 * 
 * @param
 * master-list = row[]
 * config = {
 *      columns: column[],
 *      groups: column[]?,
 *      panelHeader: headerPart[]?,
 *      loading: function(isLoading: bool)?
 * }
 * 
 * column = { param: string, label: string }
 *      param = nome do parâmetro de row
 *      label = texto que será exibido no head da tabela
 * 
 * headerPart = { fn: function(sublist), description: string, class: string?, style: string? }
 *      fn = função que retornará o valor a ser exibido [ex. subtotal]
 *      description = texto que será exibido antes do velor do resultado
 */

/**
 * TODO:
 * unagrupable colunms
 * hide on grouping
 */


(function () {
    'use strict';

    angular.module('tree-table-template', ['angular.filter']).run(_templates);
    angular.module('tree-table', ['tree-table-template'])
        .directive('treeTable', main)
        .directive('treeTableItem', item)
        .directive('treeTableTable', table)
        .directive('treeTablePanel', panel)
        .directive('partHeader', partHeader)
        .directive('listenRendering', listenRendering);
    //


    function main() {
        var directive = {
            restrict: 'E',
            templateUrl: 'tree-table.html',
            scope: {
                list: '=masterList',
                config: '='
            },
            link: _link
        };
        return directive;

        function _link(scope) {
            var config = scope.config;
            scope.columns = config.columns;
            scope.groups = config.groups || [];
            scope.panelHeader = config.panelHeader || [];
            scope.loading = config.loading || angular.noop;

            scope.loading(true);

            scope.showhide = _showhidePanel;
            scope.lastParent = true;

            scope.isInGroups = _isInGroupsFn(scope.groups);
            scope.addGrouping = _addGroupingFn(scope.groups, scope.loading);
            scope.removeGrouping = _removeGroupingFn(scope.groups, scope.loading);

            scope.calculator = _calculator;

            if (!Array.isArray(scope.panelHeader)) {
                scope.panelHeader = [scope.panelHeader];
            }

            scope.$on('LastTableFromTree', function () {
                scope.loading(false);
            });

            function _calculator(columnHeader, list) {
                return columnHeader.fn(list);
            }
        }
    }

    function table() {
        var directive = {
            restrict: 'E',
            scope: false,
            templateUrl: 'tree-table-table.html'
        };
        return directive;
    }

    function item() {
        var directive = {
            restrict: 'E',
            templateUrl: 'tree-table-item.html',
        };
        return directive;
    }

    function panel() {
        var directive = {
            restrict: 'E',
            templateUrl: 'tree-table-panel.html',
        };
        return directive;
    }

    function partHeader() {
        var directive = {
            restrict: 'E',
            templateUrl: 'part-header.html'
        };
        return directive;
    }

    function _showhidePanel(element) {
        var _element = angular.element(element);
        _element.slideToggle(200);
        setTimeout(function () {
            _element.resize();
            _element.find('[id^=map-]').resize();
        }, 50);
        return _element.innerHeight() > 0;
    }

    function _isInGroupsFn(groups) {
        return function _isInGroup(column) {
            return groups.any(function (i) { return i == column; });
        };
    }
    function _addGroupingFn(groups, loading) {
        return function (column) {
            groups.push(column);
            loading(true);
        };
    }
    function _removeGroupingFn(groups, loading) {
        return function (column) {
            groups.remove(column);
            loading(true);
        };
    }

    listenRendering.$inject = ['$timeout'];
    function listenRendering($timeout) {
        var def = {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var customCondition;
                if (attrs.listenRendering)
                    customCondition = scope.$eval(attrs.listenRendering);
                else
                    customCondition = scope.$last;

                if (customCondition) {
                    $timeout(function () {
                        scope.$emit('LastTableFromTree');
                    }, 50);
                }
            }
        }
        return def;
    }

    _templates.$inject = ['$templateCache'];
    function _templates($templateCache) {
        $templateCache.put('part-header.html', '({{$description}} = {{$result}})');
        $templateCache.put('tree-table.html',
            '<div class="tree-table">' +
            '   <div class="m-sm" ng-if="groups.length">' +
            '      Agrupado por:' +
            '      <span ng-repeat="g in groups">' +
            '          <span ng-if="!$first">&raquo;</span>' +
            '          <a class="btn-link text-green" ng-click="removeGrouping(g)" title="Remover agrupamento">{{g.label}}</a>' +
            '      </span>' +
            '   </div>' +
            '   <tree-table-item ng-init="level=0; hash=\'\'"></tree-table-item>' +
            '</div>' +
            '');
        $templateCache.put('tree-table-item.html',
            '<div ng-if="groups[level]">' +
            '   <tree-table-panel ng-repeat="(key, sublist) in list | groupBy: (group = groups[level]).param"></tree-table-panel>' +
            '</div>' +
            '<tree-table-table ng-if="!groups[level]"></tree-table-table>' +
            '');
        $templateCache.put('tree-table-panel.html',
            '<div class="panel panel-default" ng-init="hash = (hash||\'\')+\'-\'+$index; collapseId=\'content-collapse\'+hash; open[hash]=true">' +
            '   <div class="panel-heading">' +
            '           <h5 class="panel-title">' +
            '               <a ng-click="showhide(\'#\'+collapseId); open[hash]=!open[hash]">' + // 
            '                   <i class="fa fa-{{open[hash]? \'minus\': \'plus\'}} m-r-xs"></i>' +
            '                   {{ group.label }}:' +
            '                   {{ sublist[0][group.param] }}' +
            '               </a>' +
            '               <span class="pull-right">' +
            '                   <span ng-repeat="h in panelHeader" ng-init="$description = h.description; $result = calculator(h, list)">' +
            '                       <part-header class="inline" ng-class="h.class" style="{{h.style}}"></part-header>' +
            '                   </span>' +
            '               </span>' +
            '           </h5>' +
            '   </div>' +
            '   <div id="{{collapseId}}" class="content-collapse">' +
            '       <div class="panel-group">' +
            '           <tree-table-item ng-init="level = level+1; list = sublist; lastParent=(lastParent&&$last)"></tree-table-item>' +
            '       </div>' +
            '   </div>' +
            '</div>' +
            '');
        $templateCache.put('tree-table-table.html',
            '<div class="table-responsive">' +
            '   <table class="table no-margins">' +
            '       <thead>' +
            '           <tr>' +
            '               <th class="text-nowrap" ng-repeat="col in columns" ng-class="{\'hide\': isInGroups(col)}">' +
            '                   <a class="btn-link font-bold" ng-click="addGrouping(col)">' +
            '                       <span ng-i18n="{{col.label}}">{{col.label}}</span>' +
            '                       &Sigma;' +
            '                   </a>' +
            '               </th>' +
            '           </tr>' +
            '       </thead>' +
            '       <tbody>' +
            '           <tr ng-repeat="item in list">' +
            '               <td ng-repeat="col in columns" ng-class="{\'hide\': isInGroups(col)}">' +
            '                   {{item[col.param]}}' +
            '               </td>' +
            '           </tr>' +
            '       </tbody>' +
            '       <tfoot listen-rendering="lastParent"></tfoot>' +
            '   </table>' +
            '</div>' +
            '');
    }
})();
