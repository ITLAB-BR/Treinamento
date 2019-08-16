(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .factory('TreeTableService', Service);

    Service.$inject = ['$http', '$q'];
    function Service($http, $q) {
        var listItensPrototype = [
            { id: 1, name: 'sodales mauris id nulla', col1: 'ultricies ante', col2: 'ABC', col3: 1, col4: 'Fulano', col5: 5.5 },
            { id: 2, name: 'elementum lobortis tincidunt', col1: 'ultricies ante', col2: 'ABC', col3: 2, col4: 'Fulano', col5: 5.5 },
            { id: 3, name: 'mattis ut vitae', col1: 'mollis odio', col2: 'ABC', col3: 3, col4: 'Ciclano', col5: 5.5 },
            { id: 4, name: 'maecenas vitae enim arcu', col1: 'tempor tortor', col2: 'DEF', col3: 1, col4: 'Fulano', col5: 5.5 },
            { id: 5, name: 'sapien elementum lobortis', col1: 'tempor tortor', col2: 'DEF', col3: 1, col4: 'Ciclano', col5: 5.5 },
            { id: 6, name: 'lorem pretium congue sit amet', col1: 'suspendisse', col2: 'GHI', col3: 2, col4: 'Fulano', col5: 5.5 },
            { id: 7, name: 'libero fermentum', col1: 'tempor tortor', col2: 'GHI', col3: 1, col4: 'Ciclano', col5: 5.5 },
            { id: 8, name: 'luctus placerat scelerisque', col1: 'ultricies ante', col2: 'DEF', col3: 3, col4: 'Ciclano', col5: 5.5 },
            { id: 9, name: 'sit amet proin sem sapien', col1: 'ultricies ante', col2: 'ABC', col3: 3, col4: 'Ciclano', col5: 5.5 },
            { id: 10, name: 'dolor pretium eget pellentesque', col1: 'suspendisse', col2: 'GHI', col3: 1, col4: 'Ciclano', col5: 5.5 },
            { id: 11, name: 'dapibus urna viverra', col1: 'mollis odio', col2: 'GHI', col3: 2, col4: 'Beltrano', col5: 5.5 },
            { id: 12, name: 'augue augue nunc elit', col1: 'mollis odio', col2: 'DEF', col3: 3, col4: 'Beltrano', col5: 5.5 },
        ];

        var service = {
            list: _List,
        };
        return service;

        function _List(request) {
            var response = {
                data: listItensPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }
    }
})();