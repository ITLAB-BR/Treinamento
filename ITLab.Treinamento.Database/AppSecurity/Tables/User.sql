CREATE TABLE [AppSecurity].[User] (
    [UserId]                  INT           IDENTITY (1, 1) NOT NULL,
    [Name]                    VARCHAR (70)  NOT NULL,
    [Active]                  BIT           NOT NULL,
    [AccessAllDataVisibility] BIT           NOT NULL,
    [AuthenticationType]      TINYINT       NOT NULL,
    [LastPasswordChangedDate] DATETIME      NULL,
    [CreationDate]            DATETIME      NOT NULL,
    [CreationUser]            VARCHAR (25)  NOT NULL,
    [ChangeDate]              DATETIME      NULL,
    [ChangeUser]              VARCHAR (25)  NULL,
    [Email]                   VARCHAR (70)  NOT NULL,
    [EmailConfirmed]          BIT           NOT NULL,
    [PasswordHash]            VARCHAR (MAX) NULL,
    [SecurityStamp]           VARCHAR (MAX) NULL,
    [PhoneNumber]             VARCHAR (15)  NULL,
    [PhoneNumberConfirmed]    BIT           NOT NULL,
    [TwoFactorEnabled]        BIT           NOT NULL,
    [LockoutEndDateUtc]       DATETIME      NULL,
    [LockoutEnabled]          BIT           NOT NULL,
    [AccessFailedCount]       INT           NOT NULL,
    [UserName]                VARCHAR (25)  NULL,
    CONSTRAINT [PK_AppSecurity.User] PRIMARY KEY CLUSTERED ([UserId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_UserName]
    ON [AppSecurity].[User]([UserName] ASC);


GO
GRANT UPDATE
    ON OBJECT::[AppSecurity].[User] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[User] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[User] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[AppSecurity].[User] TO [grp_App_ITLabTreinamento]
    AS [dbo];

