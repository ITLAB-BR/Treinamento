/**
 * @by          IT Lab
 * @author      camilla.vianna
 * @desc        configuração comum do daterangepicker
 * 
 * ver exemplo se uso na página ->   #/index/form
 */

(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('DaterangepickerOptions', DaterangepickerOptions)

    function DaterangepickerOptions() {
        var configDefault = {
            locale: getLocale(),
            alwaysShowCalendars: true,
            linkedCalendars: false,
            showCustomRangeLabel: false
        };

        var daterangepicker = {
            ranges: getRanges,
            locale: getLocale,
            rangeConfig: configDefault,
            singleConfig: angular.extend({ singleDatePicker: true }, configDefault)
        };
        return daterangepicker;

        function getRanges(options) {
            var ranges = {};
            if (!options && options !== null) {
                options = ['last_7_days', 'this_week', 'last_week', 'last_30_days', 'this_month', 'last_month'];
            } else if (typeof options == 'string' && options.toLowerCase() == 'all') {
                options = false;
            }

            var _ranges = {
                'today': [moment().startOf('day'), moment().endOf('day')],
                'yesterday': [moment().subtract(1, 'day').startOf('day'), moment().subtract(1, 'day').endOf('day')],
                'last_7_days': [moment().subtract(7, 'day'), moment()],
                'this_week': [moment().startOf('week'), moment().endOf('week')],
                'last_week': [moment().subtract(1, 'week').startOf('week'), moment().subtract(1, 'week').endOf('week')],
                'last_30_days': [moment().subtract(30, 'day'), moment()],
                'this_month': [moment().startOf('month'), moment().endOf('month')],
                'last_month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                'this_year': [moment().startOf('year'), moment().endOf('year')],
                'last_year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')],
            };

            for (var key in _ranges) {
                if (options && !options.any(function (item) { return item == key; }))
                    continue;

                var keyName = i18n.t('date_options.' + key);
                ranges[keyName] = _ranges[key];
            }

            return ranges;
        };
        function getLocale() {
            return {
                'daysOfWeek': moment.weekdaysShort(),
                'monthNames': moment.months(),
                'format': moment.localeData()._longDateFormat.L,
                'separator': ' ~ ',
                'applyLabel': 'OK',
                'cancelLabel': i18n.t('label.cancel'),
                'fromLabel': i18n.t('label.from'),
                'toLabel': i18n.t('label.to'),
                'customRangeLabel': i18n.t('label.custom')
            }
        };
    };
}());