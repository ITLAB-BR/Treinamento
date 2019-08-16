(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('orderServicePrototype', Service);

    Service.$inject = ['$http', '$q'];
    function Service($http, $q) {
        var listItensPrototype = [
            {
                Id: 1, Date: moment().subtract(1, 'd').toDate(), ClientId: 1, Client: { Id: 1, Name: 'Cliente ABC', Telephone: 1212345678, Email: 'cliente@abc.com' },
                OrderItens: [
                    { Product: { Id: 1, Name: 'Produto A', Price: 12.90 }, Amount: 5 },
                    { Product: { Id: 4, Name: 'Produto D', Price: 51.95 }, Amount: 2 }
                ]
            },
            {
                Id: 2, Date: moment().toDate(), ClientId: 2, Client: { Id: 2, Name: 'Cliente BCD', Telephone: 1212345678, Email: 'cliente@bcd.com' },
                OrderItens: [
                    { Product: { Id: 1, Name: 'Produto A', Price: 12.90 }, Amount: 1 },
                    { Product: { Id: 2, Name: 'Produto B', Price: 15.99 }, Amount: 1 },
                    { Product: { Id: 3, Name: 'Produto C', Price: 17.00 }, Amount: 2 },
                    { Product: { Id: 5, Name: 'Produto E', Price: 99.90 }, Amount: 10 }
                ]
            }
        ];
        calcTotal();

        var service = {
            list: _List,
            get: _Get,
            post: _Post,
            put: _Put,

            getClients: _GetClients,
            getProducts: _GetProducts
        };
        return service;

        function _List(request) {
            var result = listItensPrototype;

            if (request.Id)
                result = result.where(function (item) { return item.Id == request.Id; });
            if (request.Client)
                result = result.where(function (item) { return item.ClientId == request.Client; });

            var response = {
                data: result,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _Get(id) {
            var response = {
                data: listItensPrototype.where(function (item) { return item.Id == id; }),
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;

        };
        function _Post(request) {
            request.Id = listItensPrototype.last().Id + 1;
            var response = {
                data: request,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;

        };
        function _Put(request) {
            var response = {
                data: request,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;

        };

        function _GetClients(request) {
            var listItensPrototype = [
                { Id: 1, Name: 'Cliente A', Telephone: 1212345678, Email: 'cliente@a.com' },
                { Id: 2, Name: 'Cliente B', Telephone: 1212345678, Email: 'cliente@b.com' },
                { Id: 3, Name: 'Cliente C', Telephone: 1212345678, Email: 'cliente@c.com' },
                { Id: 4, Name: 'Cliente D', Telephone: 1212345678, Email: 'cliente@d.com' },
                { Id: 5, Name: 'Cliente E', Telephone: 1212345678, Email: 'cliente@e.com' },
                { Id: 6, Name: 'Cliente EFG', Telephone: 1212345678, Email: 'cliente@efg.com' },
                { Id: 7, Name: 'Cliente FGH', Telephone: 1212345678, Email: 'cliente@fgh.com' },
                { Id: 8, Name: 'Cliente GHI', Telephone: 1212345678, Email: 'cliente@ghi.com' },
                { Id: 9, Name: 'Cliente HIJ', Telephone: 1212345678, Email: 'cliente@hij.com' },
                { Id: 10, Name: 'Cliente IJK', Telephone: 1212345678, Email: 'cliente@ijk.com' },
            ];
            var response = {
                data: listItensPrototype.where(function (item) {
                    return item.Name.toLowerCase().indexOf(request.toLowerCase()) !== -1;
                }),
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _GetProducts(request) {
            var listItensPrototype = [
                { Id: 1, Name: 'Produto A', Price: 12.90 },
                { Id: 2, Name: 'Produto B', Price: 15.99 },
                { Id: 3, Name: 'Produto C', Price: 17.00 },
                { Id: 4, Name: 'Produto D', Price: 51.95 },
                { Id: 5, Name: 'Produto E', Price: 99.90 },
                { Id: 1, Name: 'Produto ABC', Price: 12.90 },
                { Id: 2, Name: 'Produto BCD', Price: 15.99 },
                { Id: 3, Name: 'Produto CDE', Price: 17.00 },
                { Id: 4, Name: 'Produto DEF', Price: 51.95 },
                { Id: 5, Name: 'Produto EFG', Price: 99.90 },
                { Id: 1, Name: 'Produto GHI', Price: 12.90 },
                { Id: 2, Name: 'Produto IJK', Price: 15.99 },
                { Id: 3, Name: 'Produto KLM', Price: 17.00 },
                { Id: 4, Name: 'Produto MNO', Price: 51.95 },
                { Id: 5, Name: 'Produto OPQ', Price: 99.90 },
                { Id: 1, Name: 'Produto RST', Price: 12.90 },
                { Id: 2, Name: 'Produto STU', Price: 15.99 },
                { Id: 3, Name: 'Produto TUV', Price: 17.00 },
                { Id: 4, Name: 'Produto UVW', Price: 51.95 },
                { Id: 5, Name: 'Produto VWX', Price: 99.90 },
            ];
            var response = {
                data: listItensPrototype.where(function (item) {
                    return item.Name.toLowerCase().indexOf(request.toLowerCase()) !== -1;
                }),
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };

        function calcTotal() {
            for (var j = 0; j < listItensPrototype.length; j++) {
                var order = listItensPrototype[j];
                order.Total = 0;

                for (var i = 0; i < order.OrderItens.length; i++) {
                    var item = order.OrderItens[i];
                    order.Total += item.Amount * item.Product.Price;
                }
            }
        };
    };
})();