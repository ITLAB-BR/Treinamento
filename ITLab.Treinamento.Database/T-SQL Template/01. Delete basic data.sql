USE <DATABASE_NAME>
GO

DELETE FROM [GeneralSettings]
DELETE FROM [AppSecurity].[GroupUser]
DELETE FROM [AppSecurity].[Group] 
DELETE FROM [AppSecurity].[UsersRoles]
DELETE FROM [AppSecurity].[Roles] 
DELETE FROM [UsersCountry]
DELETE FROM [AppSecurity].[User]
DELETE FROM [Country]
DELETE FROM [AppSecurity].[APIClients]

DBCC CHECKIDENT ('[AppSecurity].[Group]', RESEED, 0)
DBCC CHECKIDENT ('[AppSecurity].[User]', RESEED, 0)
DBCC CHECKIDENT ('[Country]', RESEED, 0)






 