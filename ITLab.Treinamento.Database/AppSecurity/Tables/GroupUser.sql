CREATE TABLE [AppSecurity].[GroupUser] (
    [GroupId] INT NOT NULL,
    [UserId]  INT NOT NULL,
    CONSTRAINT [PK_AppSecurity.GroupUser] PRIMARY KEY CLUSTERED ([GroupId] ASC, [UserId] ASC),
    CONSTRAINT [FK_AppSecurity.GroupUser_AppSecurity.Group_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [AppSecurity].[Group] ([GroupId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AppSecurity.GroupUser_AppSecurity.User_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppSecurity].[User] ([UserId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [AppSecurity].[GroupUser]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_GroupId]
    ON [AppSecurity].[GroupUser]([GroupId] ASC);


GO
GRANT UPDATE
    ON OBJECT::[AppSecurity].[GroupUser] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[GroupUser] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[GroupUser] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[AppSecurity].[GroupUser] TO [grp_App_ITLabTreinamento]
    AS [dbo];

