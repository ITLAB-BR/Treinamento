﻿(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('commonsService', service);

    service.$inject = ['$q'];
    function service($q) {
        var service = {
            getUserAvatar: getUserAvatar,
            getLayoutSkin: getLayoutSkin,

            showMessages: showMessages,
            showMessage: showMessage,

            setDisabledPropertyForAllButtonsFromScreen: setDisabledPropertyToAllButtonsFromScreen,
        };

        return service;

        function getUserAvatar() {
            var deferred = $q.defer();
            var response = {
                data: null,
                status: 204
            };

            deferred.resolve(response);
            return deferred.promise;
        }

        function getLayoutSkin() {
            var deferred = $q.defer();

            var response = {
                data: 'skin-grey',
                status: 200
            };

            deferred.resolve(response);

            return deferred.promise;
        }

        function showMessages(containerMessages, title) {
            if (containerMessages == undefined) { return false; }

            var messages = (containerMessages.hasOwnProperty('message')) ? containerMessages.message : containerMessages;
            if (typeof messages == 'string') messages = [messages];

            for (var i = 0; i < messages.length; i++) {
                if (isMinimumPasswordLengthMessage(messages[i])) {
                    var interpolation = getInterpolationMinimumLenghtForPassword(messages[i]);
                    showMessage('alerts:error.minimum_length', title, interpolation);
                    continue;
                }
                if (isNameIsAlreadyTakenMessage(messages[i])) {
                    messages[i] = 'alerts:error.login_already_exists';
                }
                showMessage(messages[i], title, interpolation);
            };
        }

        function showMessage(message, title, interpolation) {
            if (message == undefined) { return false; }

            var typeMessage = 'error';
            if (message.indexOf('warning.') != -1) {
                typeMessage = 'warning';
            } else if (message.indexOf('success.') != -1) {
                typeMessage = 'success';
            }

            if (!title || title.length == 0) {
                switch (typeMessage) {
                    case 'success':
                        title = 'alerts:success.success';
                        break;
                    case 'warning':
                        title = 'alerts:warning.warning';
                        break;
                    default:
                        title = 'alerts:error.error';
                }
            }

            var messagei18n = i18n.t(message, interpolation);
            var messageTitlei18n = i18n.t(title);

            toastr[typeMessage](messagei18n, messageTitlei18n);
        };

        function isMinimumPasswordLengthMessage(message) {
            return /passwords_must_be_at_least_(\d+)_characters/.test(message);
        }

        function getInterpolationMinimumLenghtForPassword(message) {
            var regex = /(^.*)(\d+)(.*$)/;
            var minLength = message.replace(regex, '$2');
            return { x: minLength };
        }

        function isNameIsAlreadyTakenMessage(message) {
            return /name_(.*)_is_already_taken/.test(message);
        }

        function setDisabledPropertyToAllButtonsFromScreen(disabledValue) {
            var elements = [
                'button',
                'a',
                'input[type=button]',
                'input[type=submit]',
                'input[type=reset]'
            ].join(', ');
            var allButtons = angular.element(elements);

            allButtons.each(function (k, el) {
                el.disabled = disabledValue;
            });
        }
    }

})();