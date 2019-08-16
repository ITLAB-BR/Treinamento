angular
    .module('itlabtreinamento')
    .config(['$navigationProvider', function (nav) {
        nav.map([
            {
                name: 'events',
                title: 'nav.events',
                icon: 'calendar',
                nav: 'root',
                order: 3
            },
            {
                name: 'calendar',
                title: 'nav.calendar',
                route: '/events/calendar',
                template: 'app/pages/events/calendar.html',
                controller: 'CalendarController',
                bundle: [
                    //'app/pages/events/holidays.service.js',
                    //'app/pages/events/calendar.service.js',
                    'app/pages/events/holidays.service.prototype.js',
                    'app/pages/events/calendar.service.prototype.js',
                    'app/pages/events/calendar.controller.js'
                ],
                nav: 'events'
            },
            {
                name: 'agenda',
                title: 'nav.agenda',
                route: '/events/agenda',
                template: 'app/pages/events/agenda.html',
                controller: 'AgendaController',
                bundle: [
                    //'app/pages/events/calendar.service.js',
                    'app/pages/events/calendar.service.prototype.js',
                    'app/pages/events/agenda.controller.js'
                ],
                nav: 'events'
            },
            {
                name: 'holidays',
                title: 'nav.holidays',
                route: '/events/holidays',
                template: 'app/pages/events/holidays.html',
                controller: 'HolidayController',
                bundle: [
                    //'app/pages/events/holidays.service.js',
                    'app/pages/events/holidays.service.prototype.js',
                    'app/pages/events/holidays.controller.js'
                ],
                nav: 'events'
            }
        ]);
    }]);