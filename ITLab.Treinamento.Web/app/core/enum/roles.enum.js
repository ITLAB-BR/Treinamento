(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .constant('rolesEnum', rolesEnum());

    //Configuração das permissões do lado do Front-End
    //Esta configuração DEVE refletir o enum Permissions que está na API \ITLabTemplate.Api\Core\Security\Authorization\Permissions.cs
    function rolesEnum() {
        return {
            manageApiClients: -4,
            manageGeneralSettings: -3,
            userChangePassword: -2,
            managerProfile: -1,
            manageUser: 0,
            manageCountry: 1
        };
    }
})();