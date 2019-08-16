(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('AgendaController', Controller);

    Controller.$inject = ['$rootScope', '$compile', 'calendarService', 'commonsService', 'DaterangepickerOptions', 'fullcalendarConfig'];
    function Controller($rootScope, $compile, itemService, commonsService, DaterangepickerOptions, fullcalendarConfig) {
        var viewModel = this;

        var itemDefault = {
            action: 'new',
            Id: 0,
            Description: '',
            Date: null,
            Start: '',
            End: '',
            Color: ''
        };
        viewModel.itemEditing = null;
        viewModel.listItems = [];
        viewModel.dateConfig = DaterangepickerOptions.singleConfig;

        viewModel.calendarConfig = _SetFullcalendarConfig();

        viewModel.functions = {
            newItem: _NewItem,
            saveItem: _SaveItem,
            removeItem: _RemoveItem
        };

        var sourceEvent = { bd: 0 };

        (function initializePage() {
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
        function _SetFullcalendarConfig() {
            var functionsConfig = {
                viewRender: viewRender,
                eventDrop: onEventDropOrResize,
                eventResize: onEventDropOrResize,
                eventClick: eventClick
            };
            return angular.extend(functionsConfig, fullcalendarConfig('agenda'));

            function viewRender(view) {
                _ListItems({ since: view.start.format(), until: view.end.format() });
            };
            function onEventDropOrResize(event, jsEvent, ui, view) {
                _SelectItem(event);
                //viewModel.itemEditing.action = 'dragndrop';
                _SaveItem();
            };
            function eventClick(event) {
                $('#edit').modal('show');
                _SelectItem(event);
            };
        };
    };
})();