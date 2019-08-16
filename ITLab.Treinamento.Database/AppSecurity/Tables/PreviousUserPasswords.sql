CREATE TABLE [AppSecurity].[PreviousUserPasswords] (
    [PreviousUserPasswordsId] INT           IDENTITY (1, 1) NOT NULL,
    [UserId]                  INT           NOT NULL,
    [PasswordHash]            VARCHAR (MAX) NOT NULL,
    [CreationDate]            DATETIME      NOT NULL,
    [CreationUser]            VARCHAR (25)  NOT NULL,
    [ChangeDate]              DATETIME      NULL,
    [ChangeUser]              VARCHAR (25)  NULL,
    CONSTRAINT [PK_AppSecurity.PreviousUserPasswords] PRIMARY KEY CLUSTERED ([PreviousUserPasswordsId] ASC),
    CONSTRAINT [FK_AppSecurity.PreviousUserPasswords_AppSecurity.User_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppSecurity].[User] ([UserId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [AppSecurity].[PreviousUserPasswords]([UserId] ASC);


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[PreviousUserPasswords] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[PreviousUserPasswords] TO [grp_App_ITLabTreinamento]
    AS [dbo];

