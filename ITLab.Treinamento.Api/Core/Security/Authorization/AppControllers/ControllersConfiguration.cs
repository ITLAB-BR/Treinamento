using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentSecurity;
using ITLab.Treinamento.Api.Controllers;
using ITLab.Treinamento.Api.IdentityConfig;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Core.Security.Policies;
using Microsoft.AspNet.Identity.Owin;

namespace ITLab.Treinamento.Api.Core.Security.Authorization.AppControllers
{
    /// <summary>
    /// Classe que configura as permissões para controllers
    /// OBS.: Aqui devem ficar configurações de controllers exceto as referentes a área de segurança. Caso 
    /// necessário configurar mudar alguma configuração referente a parte de segurança (Ex.: configurar permissão 
    /// para cadastrar usuário ) 
    /// Acessar a classe Security > Authorization > SecurityControllers > ControllersConfiguration.cs (configura as controllers gerais da aplicações)
    /// </summary>
    public static class ControllersConfiguration
    {
        public static void Configure(ConfigurationExpression configuration)
        {
            //OBSERVAÇÃO: O fato de uma nova controller herdar de BaseController
            //não indica que os usuários autenticados terão acessos aos métodos. 
            //O Deny na BaseController serve apenas para negar o acesso a usuários
            //não auenticados, ou seja, ainda se faz necessário configurar o tipo
            //de acesso a sua controller. Caso a sua controller tenha acesso a qualquer
            //usuário autenticado basta configurar como configuration.For<MyController>().DenyAnonymousAccess()
            configuration.For<BaseController>().DenyAnonymousAccess();

            configuration.For<CheckupController>().AllowAny();
            configuration.For<NotificationController>().DenyAnonymousAccess();
            //configuration.For<UserController>(t => t.PostUserImage(null, 0)).AllowAny();

            configuration.For<CountryController>(p => p.Delete(0)).AddPolicy(new PermissionPolicy(Permissions.manageCountry));
            configuration.For<CountryController>(p => p.Post(null)).AddPolicy(new PermissionPolicy(Permissions.manageCountry));
            configuration.For<CountryController>(p => p.Put(null)).AddPolicy(new PermissionPolicy(Permissions.manageCountry));
            configuration.For<CountryController>(p => p.Get(0, false, false)).DenyAnonymousAccess();

            configuration.For<ClientController>().AllowAny();

            configuration.For<EventController>().AllowAny();
            configuration.For<EventController>(e => e.Get(null)).DenyAnonymousAccess();
        }
    }
}