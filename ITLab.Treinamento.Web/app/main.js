// Angular module
angular.module('itlab.datatables', ['datatables', 'datatables.buttons', 'datatables.options']);

angular
    .module('itlabtreinamento', ['oc.lazyLoad', 'ngAnimate', 'ngCookies', 'ngTouch', 'ngSanitize', 'ngResource',
        'ngMask', 'ui.router', 'ui.bootstrap', 'ui.calendar', 'jkuri.slimscroll', 'ui.grid', 
        'ui.grid.pagination', 'itlab.datatables', 'ui.select', 'daterangepicker', 'picklist', 'colorpicker.module', 'tree-table',
        'angular-ladda', 'xeditable'])
    .run(function () {
        var fixHeight = function () {
            var heightWithoutNavbar = $("body > #wrapper").height() - 61;
            $(".sidebard-panel").css("min-height", heightWithoutNavbar + "px");

            var navbarHeigh = $("nav.navbar-default").height();
            var wrapperHeigh = $("#page-wrapper").height();

            if (navbarHeigh > wrapperHeigh) {
                $("#page-wrapper").css("min-height", navbarHeigh + "px");
            }

            if (navbarHeigh < wrapperHeigh) {
                $("#page-wrapper").css("min-height", $(window).height() + "px");
            }

            if ($("body").hasClass("fixed-nav")) {
                $("#page-wrapper").css("min-height", $(window).height() - 60 + "px");
            }
        };

        $(window).bind("load resize scroll", function () {
            if (!$("body").hasClass("body-small")) {
                fixHeight();
            }
        });

        $(window).bind("load resize", function () {
            if ($(this).width() < 769)
                $("body").addClass("body-small");
            else
                $("body").removeClass("body-small");
        });

        setTimeout(fixHeight);
    });

// Prototypes
Array.prototype.all = function (predicate) {
    var i, result;
    i = 0;
    result = true;
    while (i < this.length) {
        if (!predicate(this[i])) {
            result = false;
            break;
        }
        i++;
    }
    return result;
};

Array.prototype.any = function (predicate) {
    var i, result;
    result = false;
    if (!predicate) {
        result = this.length > 0;
    } else {
        i = 0;
        while (i < this.length) {
            if (predicate(this[i])) {
                result = true;
                break;
            }
            i++;
        }
    }
    return result;
};

Array.prototype.first = function (predicate) {
    var i, result;
    result = null;
    if (!predicate && this.any()) {
        result = this[0];
    } else {
        i = 0;
        while (i < this.length) {
            if (predicate(this[i])) {
                result = this[i];
                break;
            }
            i++;
        }
    }
    return result;
};

Array.prototype.last = function (predicate) {
    var i, result;
    result = null;
    if (!predicate && this.any()) {
        result = this[this.length - 1];
    } else {
        i = this.length;
        while (i >= 0) {
            if (predicate(this[i])) {
                result = this[i];
                break;
            }
            i--;
        }
    }
    return result;
};

Array.prototype.forEach = function (action) {
    var arg, j, len;
    for (j = 0, len = this.length; j < len; j++) {
        arg = this[j];
        action(arg);
    }
};

Array.prototype.select = function (func) {
    var i, j, len, results;
    results = [];
    for (j = 0, len = this.length; j < len; j++) {
        i = this[j];
        results.push(func(i));
    }
    return results;
};

Array.prototype.where = function (predicate) {
    var i, result;
    result = [];
    i = 0;
    while (i < this.length) {
        if (predicate(this[i])) {
            result.push(this[i]);
        }
        i++;
    }
    return result;
};

Array.prototype.count = function (predicate) {
    return this.where(predicate).length;
};

Array.prototype.selectMany = function (func) {
    var results = [];
    for (j = 0, len = this.length; j < len; j++) {
        i = this[j];
        func(i).forEach(function (ii) { results.push(ii); });
    }

    return results;
};

Array.prototype.min = function (func) {
    var result = undefined;

    for (j = 0, len = this.length; j < len; j++) {
        i = this[j];
        var currentValue = func(i);

        if (result == undefined || currentValue < result)
            result = currentValue;
    }

    return result;
};

Array.prototype.max = function (func) {
    var result = undefined;

    for (j = 0, len = this.length; j < len; j++) {
        i = this[j];
        var currentValue = func(i);

        if (result == undefined || currentValue > result)
            result = currentValue;
    }

    return result;
};

Array.prototype.remove = function (item) {
    for (i = 0, len = this.length; i < len; i++) {
        if (item === this[i]) {
            this.splice(i, 1);
            break;
        }
    }
};