using System;
using System.Collections.Generic;
using System.Linq;
using ITLab.Treinamento.Api.Core.Configuration;
using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.IdentityConfig;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ITLab.Treinamento.Api.Core.Security;

namespace ITLab.Treinamento.Api.Migrations
{
    public class Seed
    {
        private readonly AppDbContext _context;

        public Seed(AppDbContext context)
        {
            _context = context.WithUsername("admin");
        }

        private List<User> UsersToAdd { get; set; }
        private List<Role> RolesToAdd { get; set; }
        private List<Group> GroupsToAdd { get; set; }
        private List<Country> CountryAdd { get; set; }
        private List<Client> ClientAdd { get; set; }

        public void InitialDataBase()
        {
            SetDataToInitializeDataBase();

            if (!_context.GeneralSettings.Any()) { SetGeneralSettings(); }
            if (!_context.Roles.Any()) { SetRoles(); }
            if (!_context.Groups.Any()) { SetGroups(); }
            if (!_context.Countries.Any()) { SetCountry(); }
            if (!_context.Clients.Any()) { SetClient(); }
            if (!_context.Users.Any()) { SetUser(); }
            if (!_context.APIClients.Any()) { SetAPIClients(); }
            if (!_context.Users.SelectMany(u => u.Notifications).Any()) { SetNotification(); }
        }

        private void SetNotification()
        {
            foreach (var item in UsersToAdd)
            {
                var users = _context.Users.SingleOrDefault(u => u.Id == item.Id);
                users.Notify("Seja bem-vindo ao sistema!!!");
            }
        }

        private void SetDataToInitializeDataBase()
        {
            RolesToAdd = new List<Role>
            {
                new Role {Id = -4, Name = "Gerenciar os clientes das APIs do sistema"},
                new Role {Id = -3, Name = "Gerenciar os parâmetros gerais do sistema"},
                new Role {Id = -2, Name = "Usuário alterar própria senha"},
                new Role {Id = -1, Name = "Gerenciar perfis de acesso ao sistema"},
                new Role {Id = 0, Name = "Gerenciar usuários ao sistema"},
                new Role {Id = 1, Name = "Gerenciar Paises"}
            };

            CountryAdd = new List<Country>
            {
                new Country {Name = "Brasil", Active = true},
                new Country {Name = "Argentina", Active = true},
                new Country {Name = "Chile", Active = true},
                new Country {Name = "Mexico", Active = true},
            };

            //Exemplo para gerar grande massa de dados para a tabela
            //ClientAdd = new List<Client>();
            //for (int i = 0; i < 100; i++)
            //{
            //    ClientAdd.Add(new Client { Name = "Cliente" + i, CNPJ = i, Email = "cliente@abc.com.br", Telephone = 1212345678, Active = true, CreationDate = DateTime.Now });
            //}

            ClientAdd = new List<Client>
            {
                new Client { Name = "Cliente ABC", CNPJ = 12345678000112, Email = "cliente@abc.com.br", Telephone = 1212345678, Active = true, CreationDate = DateTime.Now },
                new Client { Name = "Cliente BCD", CNPJ = 12345678000123, Email = "cliente@bcd.com.br", Telephone = 1212345678, Active = true, CreationDate = DateTime.Now },
                new Client { Name = "Cliente DEF", CNPJ = 12345678000134, Email = "cliente@def.com.br", Telephone = 1212345678, Active = true, CreationDate = DateTime.Now },
                new Client { Name = "Cliente EFG", CNPJ = 12345678000156, Email = "cliente@efg.com.br", Telephone = 1212345678, Active = false, CreationDate = DateTime.Now },
                new Client { Name = "Cliente GHI", CNPJ = 12345678000167, Email = "cliente@ghi.com.br", Telephone = 1212345678, Active = true, CreationDate = DateTime.Now },
                new Client { Name = "Cliente HIJ", CNPJ = 12345678000178, Email = "cliente@hij.com.br", Telephone = 1212345678, Active = true, CreationDate = DateTime.Now },
                new Client { Name = "Cliente IJK", CPF = 123456787, Email = "cliente@ijk.com.br", Telephone = 1212345678, Active = true, CreationDate = DateTime.Now },
                new Client { Name = "Cliente JKL", CPF = 123456788, Email = "cliente@jkl.com.br", Telephone = 1212345678, Active = true, CreationDate = DateTime.Now },
                new Client { Name = "Cliente KLM", CPF = 123456789, Email = "cliente@klm.com.br", Telephone = 1212345678, Active = true, CreationDate = DateTime.Now }
            };

            UsersToAdd = new List<User>
            {
                new User
                {
                    Name = "Administrator",
                    UserName = "admin",
                    Email = "admin@admin.com.br",
                    Active = true,
                    CreationUser = _context.GetCurrentUser(),
                    AuthenticationType = AuthenticationType.DataBase
                }
            };

            for (int i = 1; i < 100; i++)
            {
                UsersToAdd.Add(new User
                {
                    Name = "Administrator " + i,
                    UserName = "admin" + i,
                    Email = "admin" + i + "@admin.com.br",
                    Active = true,
                    CreationUser = _context.GetCurrentUser(),
                    AuthenticationType = AuthenticationType.DataBase
                });
            }


            GroupsToAdd = new List<Group>
            {
                new Group {Name = "Administração do sistema"}
            };
        }

        private void SetGroups()
        {
            var roleStore = new RoleStore<Role, int, UserRole>(_context);
            var roleManager = new ApplicationRoleManager(roleStore);

            var userStore = new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(_context);
            var userManager = new ApplicationUserManager(userStore);

            var groupManagerStore = new ApplicationGroupStore(_context);
            var groupManager = new ApplicationGroupManager(groupManagerStore, roleManager, userManager);

            foreach (var group in GroupsToAdd)
            {
                var result = groupManager.CreateGroup(group);
                if (!result.Succeeded)
                    throw new Exception(string.Join(",", result.Errors.ToArray()));
                var roles = RolesToAdd.Select(x => x.Id).ToArray();
                var resultSetGroupRoles = groupManager.SetGroupRoles(group.Id, roles);
                if (!resultSetGroupRoles.Succeeded)
                    throw new Exception(string.Join(",", resultSetGroupRoles.Errors.ToArray()));
            }
        }

        private void SetCountry()
        {
            foreach (var country in CountryAdd)
                if (!_context.Countries.Any(d => d.Name == country.Name))
                    _context.Countries.Add(country);
        }

        private void SetClient()
        {
            foreach (var client in ClientAdd)
                if (!_context.Clients.Any(d => d.Name == client.Name))
                    _context.Clients.Add(client);
        }

        private void SetUser()
        {
            var userStore = new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(_context);
            var userManager = new ApplicationUserManager(userStore) { UserLockoutEnabledByDefault = true };

            var roleStore = new RoleStore<Role, int, UserRole>(_context);
            var roleManager = new ApplicationRoleManager(roleStore);

            var groupManagerStore = new ApplicationGroupStore(_context);
            var groupManager = new ApplicationGroupManager(groupManagerStore, roleManager, userManager);

            foreach (var user in UsersToAdd)
            {
                if (userManager.FindByEmail(user.Email) != null) continue;

                var result = userManager.Create(user, "123456");
                if (!result.Succeeded) throw new Exception(string.Join(",", result.Errors.ToArray()));

                groupManager.SetUserGroups(user.Id, GroupsToAdd.Select(x => x.Id).ToArray());

                foreach (var country in CountryAdd)
                    if (user.Countries.All(d => d.Id != country.Id))
                        user.Countries.Add(country);
            }
        }

        private void SetRoles()
        {
            using (var roleManager = new RoleManager<Role, int>(new RoleStore<Role, int, UserRole>(_context)))
            {
                foreach (var role in RolesToAdd)
                {
                    if (roleManager.RoleExists(role.Name)) continue;

                    var result = roleManager.Create(role);
                    if (!result.Succeeded) throw new Exception(string.Join(",", result.Errors.ToArray()));
                }
            }
        }

        private void SetGeneralSettings()
        {
            var settings = new GeneralSettingsApp
            {
                PasswordRequiredMinimumLength = 3,
                PasswordRequireDigit = true,
                PasswordRequireLowercase = false,
                PasswordRequireUppercase = false,
                PasswordRequireNonLetterOrDigit = false,
                PasswordHistoryLimit = 3,
                PasswordExpiresInDays = 365,

                AccessTokenExpireTimeSpanInMinutes = 30,
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeInMinutes = 2,
                MaxFailedAccessAttemptsBeforeLockout = 4,

                AuthenticateDataBase = true,
                AuthenticateActiveDirectory = false,

                ActiveDirectoryType = ActiveDirectoryType.Server,
                ActiveDirectoryDomain = "itlab.local",
                ActiveDirectoryDN = "dc=itlab,dc=local",

                SMTPDeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory,
                SMTPPickupDirectoryLocation = @"c:\temp\ITLab.TreinamentoWebAPI\email",
                SMTPDefaultFromAddress = "template@itlab.com.br",

                LayoutSkin = "skin-grey",

                UploadDirectoryTemp = @"c:\temp\ITLab.TreinamentoWebAPI\UploadFileTemp"
            };

            var generalSettings = SettingHelper.ParseObjectToSettingDataBase(settings);

            foreach (var setting in generalSettings)
            {
                _context.GeneralSettings.Add(setting);
            }

            _context.SaveChanges();
        }

        private void SetAPIClients()
        {
            string alloweOrign;

            alloweOrign = Environment.EnvironmentTpe == Environment.Type.DesenvolvimentoLocal ? "http://localhost:8080" : "http://app.itlab.com.br";

            var apiClientAppAngular = new APIClients
            {
                Id = "WebAngularAppAuth",
                Secret = SecurityHelper.GetHash("abc@123"),
                Name = "AngularJs Front-End Application",
                Type = APIClientTypes.JavaScriptNonconfidencial,
                Active = true,
                RefreshTokenLifeTimeInMinutes = 7200,
                AllowedOrigin = alloweOrign
            };

            var apiClientAppConsole = new APIClients
            {
                Id = "ConsoleAppAuth",
                Secret = SecurityHelper.GetHash("123@abc"),
                Name = "Application Console Test",
                Type = APIClientTypes.NativeConfidencial,
                Active = true,
                RefreshTokenLifeTimeInMinutes = 7200,
                AllowedOrigin = "*"
            };

            _context.APIClients.Add(apiClientAppAngular);
            _context.APIClients.Add(apiClientAppConsole);
            _context.SaveChanges();
        }

        public static void InsertDataTest()
        {
            //CreateTestUser(100);
        }

        private void CreateTestUser(int quantity)
        {
            // data test
            var UsersTestToAdd = new List<User>();
            for (var i = 1; i <= quantity; i++)
            {
                var user = new User
                {
                    Name = $"User {i}",
                    UserName = $"user{i}",
                    Email = $"user{i}@itlab.com.br",
                    Active = (i % 8 != 0),
                    CreationUser = _context.GetCurrentUser(),
                    AuthenticationType = (i % 5 == 0) ? AuthenticationType.ActiveDirectory : AuthenticationType.DataBase
                };
                UsersTestToAdd.Add(user);
            }
            //

            var userStore = new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(_context);
            var userManager = new ApplicationUserManager(userStore) { UserLockoutEnabledByDefault = true };

            var roleStore = new RoleStore<Role, int, UserRole>(_context);
            var roleManager = new ApplicationRoleManager(roleStore);

            var groupManagerStore = new ApplicationGroupStore(_context);
            var groupManager = new ApplicationGroupManager(groupManagerStore, roleManager, userManager);

            foreach (var user in UsersTestToAdd)
            {
                //if (userManager.FindByEmail(user.Email) != null) continue;

                //TODO: Devemos usar o CreateAsync, porém, não conseguimos utilizar dentro do foreach, depois precisa verificar o porque.
                var result = userManager.Create(user, "123456");
                if (!result.Succeeded) throw new Exception(string.Join(",", result.Errors.ToArray()));

                groupManager.SetUserGroups(user.Id, GroupsToAdd.Select(x => x.Id).ToArray());

                foreach (var country in CountryAdd)
                    if (user.Countries.All(d => d.Id != country.Id))
                        user.Countries.Add(country);
            }
        }
    }
}