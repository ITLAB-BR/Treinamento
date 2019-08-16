CREATE TABLE [AppSecurity].[UsersRoles] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT [PK_AppSecurity.UsersRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_AppSecurity.UsersRoles_AppSecurity.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AppSecurity].[Roles] ([RoleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AppSecurity.UsersRoles_AppSecurity.User_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppSecurity].[User] ([UserId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [AppSecurity].[UsersRoles]([RoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [AppSecurity].[UsersRoles]([UserId] ASC);


GO
GRANT UPDATE
    ON OBJECT::[AppSecurity].[UsersRoles] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[UsersRoles] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[UsersRoles] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[AppSecurity].[UsersRoles] TO [grp_App_ITLabTreinamento]
    AS [dbo];

