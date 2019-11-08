(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('CalendarController', Controller);

    Controller.$inject = ['$rootScope', 'calendarService', 'holidayService', 'commonsService', 'DaterangepickerOptions', 'fullcalendarConfig'];
    function Controller($rootScope, itemService, holidayService, commonsService, DaterangepickerOptions, fullcalendarConfig) {
        var BUSINESS_HOUR = { start: '08:00', end: '18:00' };

        var viewModel = this;

        var itemDefault = {
            action: 'new',
            Id: 0,
            Description: '',
            Date: moment(),
            Start: '',
            End: '',
            Color: ''
        };

        viewModel.itemEditing = null;
        viewModel.listItems = [];
        viewModel.dateConfig = DaterangepickerOptions.singleConfig;

        viewModel.isBusinessHour = true;
        viewModel.calendarConfig = {};
        viewModel.calendarLegend = [];

        viewModel.functions = {
            newItem: _NewItem,
            saveItem: _SaveItem,
            removeItem: _RemoveItem,
            toggleBusinessHour: _toggleBusinessHour
        };

        var sourceEvent = { bd: 0 };
        var holidays = [];

        (function initializePage() {
            _ListHolidays();

            _SetFullcalendarConfig();
            _SetFullcalendarLegendData()
        })();

        function _ListItems(request) {
            itemService.list(request).then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage(response.data.Error);
                    return;
                }
                viewModel.listItems[sourceEvent.bd] = response.data;
            });
        };
        function _NewItem() {
            viewModel.itemEditing = angular.copy(itemDefault);
        };
        function _SaveItem() {
            if (!_FormIsValid(viewModel.itemEditing))
                return false;

            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            var request = angular.copy(viewModel.itemEditing);
            request.Date = request.Date.format();
            request.Start = moment(request.Start, 'H:m').format('HH:mm');
            if (request.End) request.End = moment(request.End, 'H:m').format('HH:mm');

            if (viewModel.itemEditing.action == 'new') {
                _CreateInDatabase(request);
            } else {
                _ChangeInDatabase(request);
            }
        };
        function _SelectItem(item) {
            //item is an event fullcalendar in this case
            viewModel.itemEditing = {
                action: 'edit',
                Id: item.id,
                Description: item.title,
                Date: item.start,
                Start: item.start.format('HH:mm'),
                End: item.end && item.end.format('HH:mm'),
                Color: item.color
            };
        };
        function _RemoveItem(id) {
            itemService.delete(id).then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage(response.data.message);
                    return;
                }
                commonsService.showMessage('alerts:success.delete_generic');

                var _source = angular.copy(viewModel.listItems[sourceEvent.bd]);
                var indexEditing = _source.findIndex(function (item) { return item.Id == id; });
                if (indexEditing == -1) return;
                _source.splice(indexEditing, 1);

                viewModel.listItems[sourceEvent.bd] = _source;
                $('.modal').modal('hide');
            })
        };

        function _ChangeInDatabase(request) {
            itemService.put(request).then(function (response) {
                if (viewModel.itemEditing.action != 'dragndrop') {
                    // força o source a ser atualizado para renderizar novamente os events do fullcalendar
                    var _source = angular.copy(viewModel.listItems[sourceEvent.bd]);

                    var indexEditing = _source.findIndex(function (item) { return item.Id == request.Id; });
                    if (indexEditing == -1) return;
                    _source.splice(indexEditing, 1);
                    _source.push(response.data);
                    viewModel.listItems[sourceEvent.bd] = _source;
                };
                _FinalizeSave(response);
            });
            return false;
        };
        function _CreateInDatabase(request) {
            itemService.post(request).then(function (response) {
                var _source = angular.copy(viewModel.listItems[sourceEvent.bd]);
                _source.push(response.data);
                viewModel.listItems[sourceEvent.bd] = _source;

                _FinalizeSave(response);
            });
            return false;
        };

        function _FinalizeSave(response) {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
            $rootScope.spinnerFullPageShow = false;

            if (response.status != 200) {
                commonsService.showMessage(response.data.message);
                return;
            }
            commonsService.showMessage('alerts:success.save_generic');

            $('#edit').modal('hide');
            $(".colorpicker").removeClass('colorpicker-visible');
        };
        function _FormIsValid(item) {
            var valid = true;
            var msgs = [];
            if (!item.Start) {
                msgs.push('alerts:warning.invalid_time_start');
                valid = false;
            }
            if (!item.Date || !item.Date.isValid()) {
                msgs.push('alerts:warning.invalid_date');
                valid = false;
            }

            if (!valid)
                commonsService.showMessages(msgs);
            return valid;
        };

        //
        function _ListHolidays() {
            holidayService.list().then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage(response.data.Error);
                    return;
                }
                response.data.forEach(function (item) {
                    holidays.push(item.Date.format('YYYY-MM-DD'));
                });
                _putHolidaysInCalendar();
            });
        };
        function _SetFullcalendarConfig() {
            var functionsConfig = {
                viewRender: _viewRender,
                eventDrop: _onEventDropOrResize,
                eventResize: _onEventDropOrResize,
                eventClick: _eventClick
            };

            _addWatchBusinessHour();
            angular.extend(viewModel.calendarConfig, functionsConfig, fullcalendarConfig());


            function _viewRender(view) {
                _ListItems({ since: view.start.format(), until: view.end.format() });
                _putHolidaysInCalendar();
            };
            function _onEventDropOrResize(event, jsEvent, ui, view) {
                _SelectItem(event);
                //viewModel.itemEditing.action = 'dragndrop';
                _SaveItem();
            };
            function _eventClick(event) {
                $('#edit').modal('show');
                _SelectItem(event);
            };

            function _addWatchBusinessHour() {
                $rootScope.$watch(
                    function () { return viewModel.isBusinessHour; },
                    watchBusinessHour);

                var first = true;
                function watchBusinessHour(newValue, oldValue) {
                    if (!first && newValue == oldValue) return;
                    first = false;

                    var hourToShow, hideDays;
                    if (newValue == true) {
                        hourToShow = BUSINESS_HOUR;
                        hideDays = [0, 6];
                    } else {
                        hourToShow = { start: '00:00', end: '23:59' };
                        hideDays = false;
                    }

                    var elCalendar = fullcalendarConfig.uiCalendars['calendar'];
                    if (elCalendar) {
                        elCalendar.fullCalendar('option', 'minTime', hourToShow.start);
                        elCalendar.fullCalendar('option', 'maxTime', hourToShow.end);
                        elCalendar.fullCalendar('option', 'hiddenDays', hideDays);
                    } else {
                        viewModel.calendarConfig.minTime = hourToShow.start;
                        viewModel.calendarConfig.maxTime = hourToShow.end;
                        viewModel.calendarConfig.hiddenDays = hideDays;
                    }
                };
            };
        };
        function _SetFullcalendarLegendData() {
            viewModel.calendarLegend = angular.copy(fullcalendarConfig('legend'));
        };
        function _putHolidaysInCalendar() {
            $(".fc-day").each(function () {
                var cell = $(this);
                var date = cell.attr('data-date');
                if (holidays && holidays.any(function (item) { return item == date; }))
                    cell.addClass('holiday');
            });
        };

        function _toggleBusinessHour() {
            viewModel.isBusinessHour = !viewModel.isBusinessHour;
        };
    };
})();