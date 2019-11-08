USE <DATABASE_NAME>
GO


INSERT [AppSecurity].[APIClients] ([APIClientId], [Secret], [Name], [Type], [Active], [RefreshTokenLifeTimeInMinutes], [AllowedOrigin], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (N'ConsoleAppAuth', N'lCXDroz4HhR1EIx8qaz3C13z/quTXBkQ3Q5hj7Qx3aA=', N'Application Console Test', 2, 1, 7200, N'*', GETDATE(), N'admin', NULL, NULL)
GO
INSERT [AppSecurity].[APIClients] ([APIClientId], [Secret], [Name], [Type], [Active], [RefreshTokenLifeTimeInMinutes], [AllowedOrigin], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (N'WebAngularAppAuth', N'5YV7M1r981yoGhELyB84aC+KiYksxZf1OY3++C1CtRM=', N'AngularJs Front-End Application', 1, 1, 7200, N'http://localhost:8080', GETDATE(), N'admin', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Country] ON 

GO
INSERT [dbo].[Country] ([CountryId], [Name], [Active], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (1, N'Brasil', 1, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[Country] ([CountryId], [Name], [Active], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (2, N'Argentina', 1, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[Country] ([CountryId], [Name], [Active], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (3, N'Chile', 1, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[Country] ([CountryId], [Name], [Active], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (4, N'Mexico', 1, GETDATE(), N'admin', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Country] OFF
GO
SET IDENTITY_INSERT [AppSecurity].[User] ON 

GO
INSERT [AppSecurity].[User] ([UserId], [Name], [Active], [AccessAllDataVisibility], [AuthenticationType], [LastPasswordChangedDate], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (1, N'Administrator', 1, 0, 1, NULL, GETDATE(), N'admin', GETDATE(), N'admin', N'admin@admin.com.br', 0, N'AGDVpUniTXdZyNWT4oZY41fBoROUlg5DdwTu4FQT9yxec01/4sDAtWTLOd8kI+kXhQ==', N'c7b01f99-c1dd-4e73-be14-ca3f1be93157', NULL, 0, 0, NULL, 1, 0, N'admin')
GO
SET IDENTITY_INSERT [AppSecurity].[User] OFF
GO
INSERT [dbo].[UsersCountry] ([CountryId], [UserId]) VALUES (1, 1)
GO
INSERT [dbo].[UsersCountry] ([CountryId], [UserId]) VALUES (2, 1)
GO
INSERT [dbo].[UsersCountry] ([CountryId], [UserId]) VALUES (3, 1)
GO
INSERT [dbo].[UsersCountry] ([CountryId], [UserId]) VALUES (4, 1)
GO
INSERT [AppSecurity].[Roles] ([RoleId], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser], [Name]) VALUES (-4, GETDATE(), N'admin', NULL, NULL, N'Gerenciar os clientes das APIs do sistema')
GO
INSERT [AppSecurity].[Roles] ([RoleId], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser], [Name]) VALUES (-3, GETDATE(), N'admin', NULL, NULL, N'Gerenciar os parâmetros gerais do sistema')
GO																														
INSERT [AppSecurity].[Roles] ([RoleId], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser], [Name]) VALUES (-2, GETDATE(), N'admin', NULL, NULL, N'Usuário alterar própria senha')
GO																														
INSERT [AppSecurity].[Roles] ([RoleId], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser], [Name]) VALUES (-1, GETDATE(), N'admin', NULL, NULL, N'Gerenciar perfis de acesso ao sistema')
GO
INSERT [AppSecurity].[Roles] ([RoleId], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser], [Name]) VALUES (0, GETDATE(), N'admin', NULL, NULL, N'Gerenciar usuários ao sistema')
GO																													   
INSERT [AppSecurity].[Roles] ([RoleId], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser], [Name]) VALUES (1, GETDATE(), N'admin', NULL, NULL, N'Gerenciar Paises')
GO
INSERT [AppSecurity].[UsersRoles] ([UserId], [RoleId]) VALUES (1, -4)
GO
INSERT [AppSecurity].[UsersRoles] ([UserId], [RoleId]) VALUES (1, -3)
GO
INSERT [AppSecurity].[UsersRoles] ([UserId], [RoleId]) VALUES (1, -2)
GO
INSERT [AppSecurity].[UsersRoles] ([UserId], [RoleId]) VALUES (1, -1)
GO
INSERT [AppSecurity].[UsersRoles] ([UserId], [RoleId]) VALUES (1, 0)
GO
INSERT [AppSecurity].[UsersRoles] ([UserId], [RoleId]) VALUES (1, 1)
GO


SET IDENTITY_INSERT [AppSecurity].[Group] ON 

GO
INSERT [AppSecurity].[Group] ([GroupId], [Name], [ChangeDate], [ChangeUser], [CreationDate], [CreationUser]) VALUES (1, N'Administração do sistema', NULL, NULL, GETDATE(), N'admin')
GO
SET IDENTITY_INSERT [AppSecurity].[Group] OFF
GO
INSERT [AppSecurity].[GroupUser] ([GroupId], [UserId]) VALUES (1, 1)
GO
INSERT [AppSecurity].[GroupRole] ([GroupId], [RoleId]) VALUES (1, -4)
GO
INSERT [AppSecurity].[GroupRole] ([GroupId], [RoleId]) VALUES (1, -3)
GO
INSERT [AppSecurity].[GroupRole] ([GroupId], [RoleId]) VALUES (1, -2)
GO
INSERT [AppSecurity].[GroupRole] ([GroupId], [RoleId]) VALUES (1, -1)
GO
INSERT [AppSecurity].[GroupRole] ([GroupId], [RoleId]) VALUES (1, 0)
GO
INSERT [AppSecurity].[GroupRole] ([GroupId], [RoleId]) VALUES (1, 1)
GO

GO
SET IDENTITY_INSERT [dbo].[GeneralSettings] ON 

GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (1, N'PasswordRequiredMinimumLength', NULL, 3, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (2, N'PasswordRequireDigit', 1, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (3, N'PasswordRequireLowercase', 0, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (4, N'PasswordRequireUppercase', 0, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (5, N'PasswordRequireNonLetterOrDigit', 0, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (6, N'PasswordHistoryLimit', NULL, 3, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (7, N'PasswordExpiresInDays', NULL, 365, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (8, N'AccessTokenExpireTimeSpanInMinutes', NULL, 30, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (9, N'UserLockoutEnabledByDefault', 1, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (10, N'DefaultAccountLockoutTimeInMinutes', NULL, 2, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (11, N'MaxFailedAccessAttemptsBeforeLockout', NULL, 4, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (12, N'AuthenticateActiveDirectory', 0, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (13, N'AuthenticateDataBase', 1, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (14, N'ActiveDirectoryType', NULL, 2, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (15, N'ActiveDirectoryDomain', NULL, NULL, N'itlab.local', GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (16, N'ActiveDirectoryDN', NULL, NULL, N'dc=itlab,dc=local', GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (19, N'SMTPDeliveryMethod', NULL, 1, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (20, N'SMTPPickupDirectoryLocation', NULL, NULL, N'c:\temp\ITLabTemplateWebAPI\email', GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (21, N'SMTPServerHost', NULL, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (22, N'SMTPServerPort', NULL, 0, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (23, N'SMTPEnableSsl', 0, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (24, N'SMTPCredentialsUsername', NULL, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (25, N'SMTPCredentialsPassword', NULL, NULL, NULL, GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (26, N'SMTPDefaultFromAddress', NULL, NULL, N'template@itlab.com.br', GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (27, N'LayoutSkin', NULL, NULL, N'skin-grey', GETDATE(), N'admin', NULL, NULL)
GO
INSERT [dbo].[GeneralSettings] ([GeneralSettingId], [SettingName], [ValueBool], [ValueInt], [ValueString], [CreationDate], [CreationUser], [ChangeDate], [ChangeUser]) VALUES (28, N'UploadDirectoryTemp', NULL, NULL, N'c:\temp\ITLabTemplateWebAPI\UploadFileTemp', GETDATE(), N'admin', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[GeneralSettings] OFF
GO
