(function () {
    'use strict';

    var generalConfig = {
        templateVersion: 'v4.2',                           //Não alterar o número de versão do template!
        //baseAddressApi: '/api/',                         //Quando a API estiver no mesmo endereço do Front-End Web
        baseAddressApi: 'http://localhost:8081/api/',      //Quando a API estiver em endereço diferente do Front-End Web
        apiClientId: 'WebAngularAppAuth',
        language: {
            languageDefault: 'pt-BR',
            availableLanguages: [
                { code: 'pt-BR', name: 'Português - Brasil' },
                { code: 'en', name: 'English' },
                { code: 'es', name: 'Español' }
            ]
        },
        defaultPage: 'home',
        cacheFrontEndFiles: false,
        defaultRefreshNotificationTimeInSeconds: 10,
        logoImg: {
            login: 'public/images/logo/logo-lg.png',
            menu: 'public/images/logo/logo-md.png',
            miniMenu: 'public/images/logo/logo-sm.png'
        }
    };

    angular.module('itlabtreinamento')
        .config(templateRequestProvider)
        .config(interceptorsConfig)
        .config(toastrOptions)
        .config(ocLazyLoadConfig)
        .run(initializeApp)
        .constant('generalConfig', generalConfig)
        .factory('spinnerFullPage', spinnerFullPage)
        .run(globalize);

    templateRequestProvider.$inject = ['$templateRequestProvider'];
    function templateRequestProvider($templateRequestProvider) {
        $templateRequestProvider.httpOptions({ isTemplate: true });
    }

    function toastrOptions() {
        // opções globais para os toastr
        toastr.options.closeButton = true;
    };

    initializeApp.$inject = ['$rootScope', 'commonsService'];
    function initializeApp($rootScope, commonsService) {
        commonsService.getLayoutSkin().then(function (response) { $rootScope.layoutSkin = response.data; });
    };

    interceptorsConfig.$inject = ['$httpProvider'];
    function interceptorsConfig($httpProvider) {
        $httpProvider.interceptors.push('apiInterceptorService');
    };

    ocLazyLoadConfig.$inject = ['$ocLazyLoadProvider'];
    function ocLazyLoadConfig($ocLazyLoadProvider) {
        $ocLazyLoadProvider.config({
            debug: false
        });
    };

    spinnerFullPage.$inject = ['$rootScope'];
    function spinnerFullPage($rootScope) {
        var config = {
            show: show,
            hide: hide
        };
        return config;

        $rootScope.spinnerFullPageShow = false;
        function show(bool) {
            if (typeof bool === 'undefined') bool = true;
            $rootScope.spinnerFullPageShow = bool;
        };
        function hide() {
            $rootScope.spinnerFullPageShow = false;
        };

        /*
         ***********************
         * USAGE EXEMPLE SIMPLE:
         * 
         *  function ExempleController(spinnerFullPage, $timeout){
         *      spinnerFullPage.show();
         *      $timeout(function () {
         *          spinnerFullPage.hide();
         *      }, 3000);
         *  }
         * 
         ****************************
         * USAGE EXEMPLE BY VARIABLE:
         * 
         *  function ExempleController(spinnerFullPage, $timeout){
         *      var show = true;
         *      spinnerFullPage.show(show);
         *      $timeout(function () {
         *          show = false;
         *          spinnerFullPage.show(show);
         *      }, 3000);
         *  }
         * 
         */
    };

    globalize.$inject = ['$rootScope', 'principal'];
    function globalize($rootScope, principal) {
        $rootScope.generalConfig = generalConfig;
        var user = principal.getUserNameLogged();
        if (user)
            $rootScope.loggedUser = user;
    };
})();