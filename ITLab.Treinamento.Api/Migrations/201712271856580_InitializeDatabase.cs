namespace ITLab.Treinamento.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "AppSecurity.AccessLog",
                c => new
                    {
                        AccessLogId = c.Int(nullable: false, identity: true),
                        AttempAccessDateTime = c.DateTime(nullable: false),
                        ClientIP = c.String(nullable: false, maxLength: 15, unicode: false),
                        Login = c.String(nullable: false, maxLength: 70, unicode: false),
                        Active = c.Boolean(nullable: false),
                        MessageCode = c.String(unicode: false),
                        MessageDescription = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.AccessLogId)
                .Index(t => t.AttempAccessDateTime, name: "IX_AccessLog_AttempAccessDateTime");
            
            CreateTable(
                "AppSecurity.APIClientRefreshToken",
                c => new
                    {
                        APIClientRefreshTokenId = c.String(nullable: false, maxLength: 44, unicode: false),
                        Subject = c.String(nullable: false, maxLength: 50, unicode: false),
                        IssuedUTC = c.DateTime(nullable: false),
                        ExpiresUTC = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false, unicode: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                        APIClientId = c.String(nullable: false, maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.APIClientRefreshTokenId)
                .ForeignKey("AppSecurity.APIClients", t => t.APIClientId, cascadeDelete: true)
                .Index(t => t.APIClientId);
            
            CreateTable(
                "AppSecurity.APIClients",
                c => new
                    {
                        APIClientId = c.String(nullable: false, maxLength: 25, unicode: false),
                        Secret = c.String(unicode: false),
                        Name = c.String(unicode: false),
                        Type = c.Byte(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTimeInMinutes = c.Int(nullable: false),
                        AllowedOrigin = c.String(nullable: false, maxLength: 100, unicode: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.APIClientId);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        Email = c.String(maxLength: 50, unicode: false),
                        CNPJ = c.Long(),
                        CPF = c.Long(),
                        Telephone = c.Long(),
                        BirthDate = c.DateTime(storeType: "date"),
                        Active = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.ClientId)
                .Index(t => t.Name, unique: true, name: "IX_Client_Name");
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Byte(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        Active = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.CountryId)
                .Index(t => t.Name, unique: true, name: "IX_Country_Name");
            
            CreateTable(
                "AppSecurity.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 70, unicode: false),
                        Active = c.Boolean(nullable: false),
                        AccessAllDataVisibility = c.Boolean(nullable: false),
                        AuthenticationType = c.Byte(nullable: false),
                        LastPasswordChangedDate = c.DateTime(),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                        Email = c.String(nullable: false, maxLength: 70, unicode: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                        PhoneNumber = c.String(maxLength: 15, unicode: false),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true, name: "IX_User_UserName");
            
            CreateTable(
                "AppSecurity.UserClaims",
                c => new
                    {
                        UserClaimId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.UserClaimId)
                .ForeignKey("AppSecurity.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "AppSecurity.Group",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.GroupId)
                .Index(t => t.Name, unique: true, name: "IX_GroupName");
            
            CreateTable(
                "AppSecurity.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.RoleId)
                .Index(t => t.Name, unique: true, name: "IX_RoleName");
            
            CreateTable(
                "AppSecurity.UsersRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("AppSecurity.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("AppSecurity.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "AppSecurity.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128, unicode: false),
                        ProviderKey = c.String(nullable: false, maxLength: 128, unicode: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("AppSecurity.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.NotificationUsers",
                c => new
                    {
                        NotificationId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ReadIn = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.NotificationId, t.UserId })
                .ForeignKey("dbo.Notifications", t => t.NotificationId, cascadeDelete: true)
                .ForeignKey("AppSecurity.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.NotificationId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 150, unicode: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.NotificationId);
            
            CreateTable(
                "AppSecurity.PreviousUserPasswords",
                c => new
                    {
                        PreviousUserPasswordsId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PasswordHash = c.String(nullable: false, unicode: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.PreviousUserPasswordsId)
                .ForeignKey("AppSecurity.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "AppSecurity.UserPhoto",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Photo = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("AppSecurity.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 30, unicode: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Start = c.Time(nullable: false, precision: 0),
                        End = c.Time(nullable: false, precision: 0),
                        Color = c.String(maxLength: 7, unicode: false),
                        AllDay = c.Boolean(nullable: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.GeneralSettings",
                c => new
                    {
                        GeneralSettingId = c.Int(nullable: false, identity: true),
                        SettingName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ValueBool = c.Boolean(),
                        ValueInt = c.Int(),
                        ValueString = c.String(unicode: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreationUser = c.String(nullable: false, maxLength: 25, unicode: false),
                        ChangeDate = c.DateTime(),
                        ChangeUser = c.String(maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.GeneralSettingId)
                .Index(t => t.SettingName, unique: true, name: "IX_GeneralSetting_SettingName");
            
            CreateTable(
                "AppSecurity.GroupRole",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupId, t.RoleId })
                .ForeignKey("AppSecurity.Group", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("AppSecurity.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "AppSecurity.GroupUser",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupId, t.UserId })
                .ForeignKey("AppSecurity.Group", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("AppSecurity.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UsersCountry",
                c => new
                    {
                        CountryId = c.Byte(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CountryId, t.UserId })
                .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("AppSecurity.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CountryId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersCountry", "UserId", "AppSecurity.User");
            DropForeignKey("dbo.UsersCountry", "CountryId", "dbo.Country");
            DropForeignKey("AppSecurity.UserPhoto", "UserId", "AppSecurity.User");
            DropForeignKey("AppSecurity.UsersRoles", "UserId", "AppSecurity.User");
            DropForeignKey("AppSecurity.PreviousUserPasswords", "UserId", "AppSecurity.User");
            DropForeignKey("dbo.NotificationUsers", "UserId", "AppSecurity.User");
            DropForeignKey("dbo.NotificationUsers", "NotificationId", "dbo.Notifications");
            DropForeignKey("AppSecurity.UserLogin", "UserId", "AppSecurity.User");
            DropForeignKey("AppSecurity.GroupUser", "UserId", "AppSecurity.User");
            DropForeignKey("AppSecurity.GroupUser", "GroupId", "AppSecurity.Group");
            DropForeignKey("AppSecurity.GroupRole", "RoleId", "AppSecurity.Roles");
            DropForeignKey("AppSecurity.GroupRole", "GroupId", "AppSecurity.Group");
            DropForeignKey("AppSecurity.UsersRoles", "RoleId", "AppSecurity.Roles");
            DropForeignKey("AppSecurity.UserClaims", "UserId", "AppSecurity.User");
            DropForeignKey("AppSecurity.APIClientRefreshToken", "APIClientId", "AppSecurity.APIClients");
            DropIndex("dbo.UsersCountry", new[] { "UserId" });
            DropIndex("dbo.UsersCountry", new[] { "CountryId" });
            DropIndex("AppSecurity.GroupUser", new[] { "UserId" });
            DropIndex("AppSecurity.GroupUser", new[] { "GroupId" });
            DropIndex("AppSecurity.GroupRole", new[] { "RoleId" });
            DropIndex("AppSecurity.GroupRole", new[] { "GroupId" });
            DropIndex("dbo.GeneralSettings", "IX_GeneralSetting_SettingName");
            DropIndex("AppSecurity.UserPhoto", new[] { "UserId" });
            DropIndex("AppSecurity.PreviousUserPasswords", new[] { "UserId" });
            DropIndex("dbo.NotificationUsers", new[] { "UserId" });
            DropIndex("dbo.NotificationUsers", new[] { "NotificationId" });
            DropIndex("AppSecurity.UserLogin", new[] { "UserId" });
            DropIndex("AppSecurity.UsersRoles", new[] { "RoleId" });
            DropIndex("AppSecurity.UsersRoles", new[] { "UserId" });
            DropIndex("AppSecurity.Roles", "IX_RoleName");
            DropIndex("AppSecurity.Group", "IX_GroupName");
            DropIndex("AppSecurity.UserClaims", new[] { "UserId" });
            DropIndex("AppSecurity.User", "IX_User_UserName");
            DropIndex("dbo.Country", "IX_Country_Name");
            DropIndex("dbo.Client", "IX_Client_Name");
            DropIndex("AppSecurity.APIClientRefreshToken", new[] { "APIClientId" });
            DropIndex("AppSecurity.AccessLog", "IX_AccessLog_AttempAccessDateTime");
            DropTable("dbo.UsersCountry");
            DropTable("AppSecurity.GroupUser");
            DropTable("AppSecurity.GroupRole");
            DropTable("dbo.GeneralSettings");
            DropTable("dbo.Event");
            DropTable("AppSecurity.UserPhoto");
            DropTable("AppSecurity.PreviousUserPasswords");
            DropTable("dbo.Notifications");
            DropTable("dbo.NotificationUsers");
            DropTable("AppSecurity.UserLogin");
            DropTable("AppSecurity.UsersRoles");
            DropTable("AppSecurity.Roles");
            DropTable("AppSecurity.Group");
            DropTable("AppSecurity.UserClaims");
            DropTable("AppSecurity.User");
            DropTable("dbo.Country");
            DropTable("dbo.Client");
            DropTable("AppSecurity.APIClients");
            DropTable("AppSecurity.APIClientRefreshToken");
            DropTable("AppSecurity.AccessLog");
        }
    }
}
