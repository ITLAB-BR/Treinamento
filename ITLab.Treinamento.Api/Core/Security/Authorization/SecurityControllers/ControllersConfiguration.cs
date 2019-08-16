using FluentSecurity;
using ITLab.Treinamento.Api.Controllers;
using ITLab.Treinamento.Api.Core.Security.Policies;

namespace ITLab.Treinamento.Api.Core.Security.Authorization.SecurityControllers
{
    /// <summary>
    /// Classe que configura as permissões para controllers respónsáveis pela parte de segurança da aplicação
    /// Ex.: Criação e configuração de usuários, criação e configurações de roles
    /// OBS.: Aqui só devem ficar configurações referentes a área de segurança. Caso 
    /// necessário configurar alguma outra configuração para o sistema (Ex.: configurar permissão 
    /// para cadastrar cliente) 
    /// Acessar a classe Security > Authorization > AppControllers > ControllersConfiguration.cs 
    /// (configura as controllers gerais da aplicações)
    /// </summary>
    public static class ControllersConfiguration
    {
        public static void Configure(ConfigurationExpression configuration)
        {
            configuration.For<SecurityController>().DenyAnonymousAccess();

            configuration.For<PermissionController>().DenyAnonymousAccess();

            configuration.For<PasswordController>(x => x.ForgotPasswordAsync(null)).AllowAny();
            configuration.For<PasswordController>(x => x.ResetPasswordAsync(null)).AllowAny();
            configuration.For<PasswordController>(x => x.ChangeMyPasswordAsync(null)).AddPolicy(new PermissionPolicy(Permissions.userChangePassword));
            configuration.For<PasswordController>(x => x.ChangeMyPasswordExpiredAsync(null)).AllowAny();

            configuration.For<SettingsController>(t => t.Get(false)).DenyAnonymousAccess();
            configuration.For<SettingsController>(t => t.GetLayoutSkin(false)).AllowAny();
            configuration.For<SettingsController>(t => t.Set(null)).AddPolicy(new PermissionPolicy(Permissions.manageGeneralSettings));

            configuration.For<LogController>().AllowAny();
            configuration.For<LogController>(l => l.Get(null)).AllowAny();

            configuration.For<RoleController>(t => t.List()).AddPolicy(new PermissionPolicy(Permissions.managerProfile));

            var permissionPolicyUserList = new PermissionPolicy(Permissions.manageUser);
            permissionPolicyUserList.Add(Permissions.managerProfile);
            configuration.For<UserController>(x => x.GetList(null)).AddPolicy(permissionPolicyUserList);
            configuration.For<UserController>(x => x.GetItem(0)).AddPolicy(new PermissionPolicy(Permissions.manageUser));
            configuration.For<UserController>(x => x.GetEmail(0)).DenyAuthenticatedAccess();
            configuration.For<UserController>(x => x.GetMyAccount()).DenyAnonymousAccess();
            configuration.For<UserController>(x => x.GetMyAvatar()).DenyAnonymousAccess();
            configuration.For<UserController>(x => x.CreateAsync(null)).AddPolicy(new PermissionPolicy(Permissions.manageUser));
            configuration.For<UserController>(x => x.UpdateAsync(null)).AddPolicy(new PermissionPolicy(Permissions.manageUser));
            configuration.For<UserController>(x => x.UpdateMyAccountAsync(null)).DenyAnonymousAccess();

            configuration.For<GroupController>(x => x.List()).AddPolicy(new PermissionPolicy(Permissions.managerProfile));
            configuration.For<GroupController>(x => x.CreateAsync(null)).AddPolicy(new PermissionPolicy(Permissions.managerProfile));
            configuration.For<GroupController>(x => x.UpdateAsync(null)).AddPolicy(new PermissionPolicy(Permissions.managerProfile));
            configuration.For<GroupController>(x => x.UpdatePermissionsAsync(null)).AddPolicy(new PermissionPolicy(Permissions.managerProfile));
            configuration.For<GroupController>(x => x.UpdateUsersAsync(null)).AddPolicy(new PermissionPolicy(Permissions.managerProfile));

            configuration.For<APIClientController>(p => p.Delete(null)).AddPolicy(new PermissionPolicy(Permissions.manageApiClients));
            configuration.For<APIClientController>(p => p.Post(null)).AddPolicy(new PermissionPolicy(Permissions.manageApiClients));
            configuration.For<APIClientController>(p => p.Put(null)).AddPolicy(new PermissionPolicy(Permissions.manageApiClients));
            configuration.For<APIClientController>(p => p.PutSecret(null)).AddPolicy(new PermissionPolicy(Permissions.manageApiClients));
            configuration.For<APIClientController>(t => t.Get(null)).AddPolicy(new PermissionPolicy(Permissions.manageApiClients));

            configuration.For<FilesController>(t => t.Index(null)).AllowAny();

            
        }
    }
}