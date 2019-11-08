CREATE TABLE [AppSecurity].[GroupRole] (
    [GroupId] INT NOT NULL,
    [RoleId]  INT NOT NULL,
    CONSTRAINT [PK_AppSecurity.GroupRole] PRIMARY KEY CLUSTERED ([GroupId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_AppSecurity.GroupRole_AppSecurity.Group_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [AppSecurity].[Group] ([GroupId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AppSecurity.GroupRole_AppSecurity.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AppSecurity].[Roles] ([RoleId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [AppSecurity].[GroupRole]([RoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_GroupId]
    ON [AppSecurity].[GroupRole]([GroupId] ASC);


GO
GRANT UPDATE
    ON OBJECT::[AppSecurity].[GroupRole] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[GroupRole] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[GroupRole] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[AppSecurity].[GroupRole] TO [grp_App_ITLabTreinamento]
    AS [dbo];

