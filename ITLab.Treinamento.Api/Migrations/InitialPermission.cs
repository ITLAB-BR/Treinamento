using ITLab.Treinamento.Api.Migrations.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Migrations
{
    public partial class InitialPermission : DbMigration
    {
        private string roleName { get { return DataBaseConfig.DataBaseRoleName; } }

        public override void Up()
        {
            if (Environment.EnvironmentTpe == Environment.Type.DesenvolvimentoLocal)
                this.CreateRole(roleName);

            this.GrantPermission("AppSecurity", "User", roleName, Permission.CRUD);
            this.GrantPermission("AppSecurity", "UserClaims", roleName, Permission.CRUD);
            this.GrantPermission("AppSecurity", "UserLogin", roleName, Permission.CRUD);
            this.GrantPermission("AppSecurity", "UserPhoto", roleName, Permission.CRUD);
            this.GrantPermission("AppSecurity", "UsersRoles", roleName, Permission.CRUD);
            this.GrantPermission("AppSecurity", "Roles", roleName, Permission.Select);
            this.GrantPermission("AppSecurity", "GroupUser", roleName, Permission.CRUD);
            this.GrantPermission("AppSecurity", "GroupRole", roleName, Permission.CRUD);
            this.GrantPermission("AppSecurity", "Group", roleName, Permission.CRUD);
            this.GrantPermission("AppSecurity", "PreviousUserPasswords", roleName, Permission.Insert);
            this.GrantPermission("AppSecurity", "PreviousUserPasswords", roleName, Permission.Select);
            this.GrantPermission("AppSecurity", "APIClients", roleName, Permission.Select);
            this.GrantPermission("AppSecurity", "APIClientRefreshToken", roleName, Permission.CRUD);
            this.GrantPermission("dbo", "GeneralSettings", roleName, Permission.Select);
            this.GrantPermission("dbo", "GeneralSettings", roleName, Permission.Insert);
            this.GrantPermission("dbo", "GeneralSettings", roleName, Permission.Update);
            this.GrantPermission("dbo", "Country", roleName, Permission.CRUD);
            this.GrantPermission("dbo", "UsersCountry", roleName, Permission.CRUD);
            this.GrantPermission("AppSecurity", "AccessLog", roleName, Permission.Insert);
            this.GrantPermission("AppSecurity", "AccessLog", roleName, Permission.Select);
        }

        public override void Down()
        {
            this.RevokePermission("AppSecurity", "User", roleName, Permission.CRUD);
            this.RevokePermission("AppSecurity", "UserClaims", roleName, Permission.CRUD);
            this.RevokePermission("AppSecurity", "UserLogin", roleName, Permission.CRUD);
            this.RevokePermission("AppSecurity", "UserPhoto", roleName, Permission.CRUD);
            this.RevokePermission("AppSecurity", "UsersRoles", roleName, Permission.CRUD);
            this.RevokePermission("AppSecurity", "Roles", roleName, Permission.Select);
            this.RevokePermission("AppSecurity", "GroupUser", roleName, Permission.CRUD);
            this.RevokePermission("AppSecurity", "GroupRole", roleName, Permission.CRUD);
            this.RevokePermission("AppSecurity", "Group", roleName, Permission.CRUD);
            this.RevokePermission("AppSecurity", "PreviousUserPasswords", roleName, Permission.Insert);
            this.RevokePermission("AppSecurity", "PreviousUserPasswords", roleName, Permission.Select);
            this.RevokePermission("AppSecurity", "APIClients", roleName, Permission.Select);
            this.RevokePermission("AppSecurity", "APIClientRefreshToken", roleName, Permission.CRUD);
            this.RevokePermission("dbo", "GeneralSettings", roleName, Permission.Select);
            this.RevokePermission("dbo", "GeneralSettings", roleName, Permission.Insert);
            this.RevokePermission("dbo", "GeneralSettings", roleName, Permission.Update);
            this.RevokePermission("dbo", "Country", roleName, Permission.CRUD);
            this.RevokePermission("dbo", "UsersCountry", roleName, Permission.CRUD);
            this.RevokePermission("AppSecurity", "AccessLog", roleName, Permission.Insert);
            this.RevokePermission("AppSecurity", "AccessLog", roleName, Permission.Select);

            if (Environment.EnvironmentTpe == Environment.Type.DesenvolvimentoLocal)
                this.DropRole(roleName);
        }
    }
}