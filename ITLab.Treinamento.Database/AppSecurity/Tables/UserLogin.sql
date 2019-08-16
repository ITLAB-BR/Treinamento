CREATE TABLE [AppSecurity].[UserLogin] (
    [LoginProvider] VARCHAR (128) NOT NULL,
    [ProviderKey]   VARCHAR (128) NOT NULL,
    [UserId]        INT           NOT NULL,
    CONSTRAINT [PK_AppSecurity.UserLogin] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_AppSecurity.UserLogin_AppSecurity.User_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppSecurity].[User] ([UserId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [AppSecurity].[UserLogin]([UserId] ASC);


GO
GRANT UPDATE
    ON OBJECT::[AppSecurity].[UserLogin] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[UserLogin] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[UserLogin] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[AppSecurity].[UserLogin] TO [grp_App_ITLabTreinamento]
    AS [dbo];

