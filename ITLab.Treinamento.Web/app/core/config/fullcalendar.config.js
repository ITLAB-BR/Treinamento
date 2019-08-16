/**
 * @by          IT Lab
 * @author      camilla.vianna
 * @desc        configuração comum do fullcalendar
 * 
 */

(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('fullcalendarConfig', Config)

    Config.$inject = ['$rootScope', 'multilanguage', 'uiCalendarConfig'];
    function Config($rootScope, multilanguage, uiCalendarConfig) {
        var locale = multilanguage.currentLanguage.toLowerCase();

        var calendar = {
            locale: locale,
            eventDataTransform: eventDataTransform,
            header: { left: 'month,agendaWeek,agendaDay', center: 'title', right: 'prev,next today' },
            defaultView: ($(window).width() < 650) ? 'agendaDay' : 'month',
            //minTime: '08:00',
            //maxTime: '20:00',
            timeFormat: 'LT',
            weekNumberTitle: 'L',
            slotLabelFormat: 'LT',
            slotDuration: '00:30:00',
            snapDuration: '00:15:00',
            height: 'auto',
            lazyFetching: false,
            allDaySlot: false,
            editable: true,
            selectable: true,
            disableDragging: true
        };
        var agenda = {
            locale: locale,
            eventDataTransform: eventDataTransform,
            defaultView: 'listMonth',
            height: 'auto'
        };

        var legend = [
            { classColor: 'holiday', description: 'page.calendar.holiday' },
            { classColor: 'today', description: 'date_options.today' }
        ];
        var config = {
            calendar: calendar,
            agenda: agenda,

            legend: legend
        };

        _config.uiCalendars = uiCalendarConfig.calendars || false;
        function _config(type, _currentConfig) {
            return type && config.hasOwnProperty(type) ? config[type] : config.calendar;
        };

        activateWatchLanguage();

        return _config;

        function eventDataTransform(eventData) {
            return {
                id: eventData.Id,
                title: eventData.Description,
                start: moment(eventData.Date).startOf('d').add(eventData.Start),
                end: moment(eventData.Date).startOf('d').add(eventData.End),
                color: eventData.Color,
                allday: eventData.AllDay
            };
        };

        function activateWatchLanguage() {
            $rootScope.$watch(function () { return multilanguage.currentLanguage; },
                              function (newValue, oldValue) {
                                  if (newValue == oldValue) return;

                                  newValue = newValue.toLowerCase();
                                  updateConfigLanguage(newValue);

                                  for (var cal in _config.uiCalendars)
                                      _config.uiCalendars[cal].fullCalendar('option', 'locale', newValue);
                                  $('[ui-calendar]').fullCalendar('option', 'locale', newValue);
                              });

            function updateConfigLanguage(newValue) {
                for (var i in config)
                    config[i].locale = newValue;
            }
        };
    };
})();
