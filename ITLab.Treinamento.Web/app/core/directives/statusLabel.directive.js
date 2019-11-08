(function () {
    "use strict";

    angular.module('itlabtreinamento')
        .directive("statusLabel", statusLabel)
        .directive("vibilityEye", vibilityEye);

    function statusLabel() {
        return {
            restrict: "AE",
            scope: { active: "@", status: "=", addClass: "@" },
            replace: true,
            template: '<span class="label {{color}}-bg label-status {{addClass}}">{{statusText}}</span>',
            controller: function ($scope, $element) {
                setStatus($scope.status || $scope.active === "A");

                $scope.$watch("active",
                    function (oldValue, newValue) {
                        setStatus(newValue === "true");
                    });
                $scope.$watch("status",
                    function (oldValue, newValue) {
                        if (typeof newValue === "boolean")
                            setStatus(newValue);
                    });

                function setStatus(bool) {
                    var status = bool === true;
                    if (status) {
                        $scope.color = "navy";
                        $scope.statusText = i18n.t("label.active");
                    } else {
                        $scope.color = "red";
                        $scope.statusText = i18n.t("label.inactive");
                    }
                };
            }
        };
    };
    function vibilityEye() {
        return {
            restrict: "AE",
            scope: { vibility: "=", addClass: "@" },
            replace: true,
            template: '<i class="fa {{icon}} {{color}}"></i>',
            link: fn_link
        };

        function fn_link(scope, asd, zxc) {
            setStatus(scope.vibility);

            scope.$watch("vibility",
                function (oldValue, newValue) {
                    setStatus(newValue);
                });

            function setStatus(bool) {
                if (bool) {
                    scope.icon = "fa-eye";
                    scope.color = "";
                } else {
                    scope.icon = "fa-eye-slash";
                    scope.color = "text-muted";
                }
            };

        };
    };
})();