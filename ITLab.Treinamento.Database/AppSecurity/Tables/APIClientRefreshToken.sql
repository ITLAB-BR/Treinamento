CREATE TABLE [AppSecurity].[APIClientRefreshToken] (
    [APIClientRefreshTokenId] VARCHAR (44)  NOT NULL,
    [Subject]                 VARCHAR (50)  NOT NULL,
    [IssuedUTC]               DATETIME      NOT NULL,
    [ExpiresUTC]              DATETIME      NOT NULL,
    [ProtectedTicket]         VARCHAR (MAX) NOT NULL,
    [ChangeDate]              DATETIME      NULL,
    [ChangeUser]              VARCHAR (25)  NULL,
    [CreationDate]            DATETIME      NOT NULL,
    [CreationUser]            VARCHAR (25)  NOT NULL,
    [APIClientId]             VARCHAR (25)  NOT NULL,
    CONSTRAINT [PK_AppSecurity.APIClientRefreshToken] PRIMARY KEY CLUSTERED ([APIClientRefreshTokenId] ASC),
    CONSTRAINT [FK_AppSecurity.APIClientRefreshToken_AppSecurity.APIClients_APIClientId] FOREIGN KEY ([APIClientId]) REFERENCES [AppSecurity].[APIClients] ([APIClientId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_APIClientId]
    ON [AppSecurity].[APIClientRefreshToken]([APIClientId] ASC);


GO
GRANT UPDATE
    ON OBJECT::[AppSecurity].[APIClientRefreshToken] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[APIClientRefreshToken] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[APIClientRefreshToken] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[AppSecurity].[APIClientRefreshToken] TO [grp_App_ITLabTreinamento]
    AS [dbo];

