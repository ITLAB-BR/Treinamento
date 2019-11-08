/*
 * @by          IT Lab
 * @author      camilla.vianna
 *
 * @desc        gera o componente picklist
 * @param       list:           array[object]                   lista com todos os itens que vão compor o picklist
 *              rightList:      array[number]                   lista de IDs dos itens selecionados
 *              heightList:     string/number, default '100%'   altura do componente em pixels
 *              description:    string, default 'description'   nome do parâmetro do item que será exibido
 * @example     <picklist list="ctrl.list" right-list="ctrl.itens"
                          description="Name" height-list="200px"></picklist>
 */

(function () {
    angular.module("picklist", []);
    angular.module("picklist")
        .factory("listFactory", listFactory)
        .factory("picklistFactory", picklistFactory)
        .directive("picklist", picklistDirective)
        .directive("picklistList", picklistListDirective)
        .directive("picklistButtons", picklistButtonsDirective)
        .directive("picklistFilter", picklistFilterDirective);

    function listFactory() {
        var factory = {
            find: find,
            findById: findById,
            remove: remove,
            removeById: removeById
        };
        return factory;

        function find(obj, array) {
            for (var i = 0; i < array.length; i++) {
                var item = array[i];
                if (item == obj)
                    return i;
            }
            return false;
        };
        function findById(id, array) {
            for (var i = 0; i < array.length; i++) {
                var item = array[i];
                if (item.Id == id)
                    return i;
            }
            return false;
        };
        function remove(item, array) {
            var i = this.find(item, array);
            if (i !== false) {
                array.splice(i, 1);
                return true;
            }
            return false;
        };
        function removeById(id, array) {
            var i = this.findById(id, array);
            if (i) {
                array.splice(i, 1);
                return true;
            }
            return false;
        }
    };
    picklistFactory.$inject = ['listFactory'];
    function picklistFactory(listFactory) {
        var factory = {
            clearSelecteds: clearSelecteds,
            getSelecteds: getSelecteds,
            anySelected: anySelected
        };
        return factory;

        function clearSelecteds(list) {
            for (var i; i < list.length; i++) {
                list[i].selected = false;
            }
        };
        function getSelecteds(list) {
            var sel = [];
            for (var i = 0; i < list.length; i++) {
                if (list[i].selected) {
                    list[i].selected = false;
                    sel.push(list[i]);
                }
            }
            return sel;
        };
        function anySelected(list) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].selected)
                    return true;
            }
        };
    };
    function picklistDirective() {
        var directive = {
            restrict: 'E',
            scope: {
                listWithAllItems: '=list',
                rightList: '=',
                heightList: '@',
                description: '@'
            },
            template: '<picklist-filter></picklist-filter>' +
                '<div class="picklist" ng-style="{\'height\': heightList}">' +
                    '<div class="picklist-list" list="list.left" move="moveToRight" filter="{{picklistFilter}}" description="{{description}}"></div>' +
                    '<picklist-buttons></picklist-buttons>' +
                    '<div class="picklist-list" list="list.right" move="moveToLeft" filter="{{picklistFilter}}" description="{{description}}"></div>' +
                    '<div style="clear: both"></div>' +
                '</div>',
            controller: controllerPicklist
        };
        return directive;
           };
    function picklistListDirective() {
        var directive = {
            restrict: 'C',
            scope: { list: '=', move: '&', filter: '@', description: '@' },
            template: '<div ng-slimscroll height="100%">' +
                        '<ul class="list-group">' +
                            '<li class="list-group-item" ' +
                                'ng-repeat="item in list | orderBy: description | filter: filterOption"' +
                                'ng-click="select(item)" ng-dblclick="move(item)"' +
                                'ng-class="{active: item.selected}"' +
                                '>{{item[description]}}</li>' +
                        '</ul>' +
                      '</div>',
            link: link
        };
        return directive;

        function link(scope) {
            scope.move = scope.move();

            scope.filterOption = {};
            scope.filterOption[scope.description] = scope.filter;
            scope.$watch('filter',
                function () {
                    scope.filterOption[scope.description] = scope.filter;
                });

            scope.select = function (item) {
                item.selected = !item.selected;
            };
        }
    };
    function picklistButtonsDirective() {
        var directive = {
            restrict: 'E',
            replace: true,
            template: '<div class="picklist-buttons">' +
                        '<div>' +
                            '<button type="button" class="btn btn-primary btn-sm" ng-click="allToRight()"><i class="fa fa-angle-double-right"></i></button>' +
                            '<button type="button" class="btn btn-primary btn-sm" ng-click="moveToRight()"><i class="fa fa-angle-right"></i></button>' +
                            '<button type="button" class="btn btn-primary btn-sm" ng-click="moveToLeft()"><i class="fa fa-angle-left"></i></button>' +
                            '<button type="button" class="btn btn-primary btn-sm" ng-click="allToLeft()"><i class="fa fa-angle-double-left"></i></button>' +
                        '</div>' +
                      '</div>'
        };
        return directive;
    };
    function picklistFilterDirective() {
        var directive = {
            restrict: 'E',
            replace: true,
            template: '<div class="input-group">' +
                        '<input type="text" ng-i18n-placeholder="label.filter" class="input form-control" ng-model="picklistFilter">' +
                        '<span class="input-group-btn"><button type="button" class="btn btn-white"> <i class="fa fa-search"></i></button></span>' +
                      '</div>'
        };
        return directive;
    };

    controllerPicklist.$inject = ['$scope', 'listFactory', 'picklistFactory'];
    function controllerPicklist($scope, listFactory, picklistFactory) {
        if (!$scope.description) $scope.description = 'description';
        if (!$scope.heightList) $scope.heightList = '100%';
        var list = { right: [], left: [] };

        separateLists();

        $scope.allToLeft = allToLeft;
        $scope.allToRight = allToRight;
        $scope.moveToRight = moveToRight;
        $scope.moveToLeft = moveToLeft;
        $scope.getIdsRight = getIdsRight;

        $scope.$watch('rightList', onChangeList);
        $scope.$watch('listWithAllItems', onChangeList);

        function separateLists() {
            var listWithAllItems = $scope.listWithAllItems || [];
            list = { right: [], left: [] };
            for (var conter = 0; conter < listWithAllItems.length; conter++) {
                var itemId = listWithAllItems[conter].Id;
                var itemFoundInRightList = listFactory.find(itemId, $scope.rightList || []);
                if (itemFoundInRightList !== false)
                    list.right.push(listWithAllItems[conter]);
                else
                    list.left.push(listWithAllItems[conter]);
            }
            $scope.list = list;
        };
        function getOriginalList() { return $.extend([], $scope.listWithAllItems); };
        function getIdsRight() {
            var right = [];
            for (var i = 0; i < $scope.list.right.length; i++) {
                var item = $scope.list.right[i];
                right.push(item.Id);
            }
            $scope.rightList = right;
        };

        function allToLeft() {
            $scope.list.left = getOriginalList();
            $scope.list.right = [];
            picklistFactory.clearSelecteds($scope.list.right);
            $scope.getIdsRight();
        };
        function allToRight() {
            $scope.list.left = [];
            $scope.list.right = getOriginalList();
            picklistFactory.clearSelecteds($scope.list.right);
            $scope.getIdsRight();
        };
        function moveToRight(item) {
            if (item) item.selected = true;
            var _from = $scope.list.left;
            var _to = $scope.list.right;
            move(_from, _to);
            $scope.getIdsRight();
        };
        function moveToLeft(item) {
            if (item) item.selected = true;
            var _from = $scope.list.right;
            var _to = $scope.list.left;
            move(_from, _to);
            $scope.getIdsRight();
        };

        function move(_from, _to) {
            var _in = picklistFactory.getSelecteds(_from);

            while (_in.length) {
                moveOne(_from, _in[0], _to);
                _in.splice(0, 1);
            }
            picklistFactory.clearSelecteds(_to);
        };
        function moveOne(_from, one, _to) {
            one.selected = false;
            _to.push(one);
            listFactory.remove(one, _from);
        };
        function onChangeList(newValue, oldValue) {
            if (newValue && oldValue != newValue) {
                separateLists();
            }
        }
    };
})();