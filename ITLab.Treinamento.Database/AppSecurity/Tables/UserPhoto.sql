CREATE TABLE [AppSecurity].[UserPhoto] (
    [UserId] INT             NOT NULL,
    [Photo]  VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_AppSecurity.UserPhoto] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_AppSecurity.UserPhoto_AppSecurity.User_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppSecurity].[User] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [AppSecurity].[UserPhoto]([UserId] ASC);


GO
GRANT UPDATE
    ON OBJECT::[AppSecurity].[UserPhoto] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[UserPhoto] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[UserPhoto] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[AppSecurity].[UserPhoto] TO [grp_App_ITLabTreinamento]
    AS [dbo];

