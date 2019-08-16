using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Security.Authorization
{
    /// <summary>
    /// Enum que contém todas as permissões do sistema
    /// o código de cada item desse enum devem ser iguais 
    /// aos dos registros da entidade/tabela Roles
    /// </summary>
    public enum Permissions
    {
        //Permissões gerais do sistema, é aqui que você irá definir as permissões do sistema
        manageCountry = 1,

        //Permissões de segurança (os números zero e negativos devem ser utilizados apenas para área de segurança do sistem)
        manageUser = 0,
        managerProfile = -1,
        userChangePassword = -2,
        manageGeneralSettings = -3,
        manageApiClients = -4
    }
}