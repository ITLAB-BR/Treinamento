CREATE TABLE [AppSecurity].[APIClients] (
    [APIClientId]                   VARCHAR (25)  NOT NULL,
    [Secret]                        VARCHAR (MAX) NULL,
    [Name]                          VARCHAR (MAX) NULL,
    [Type]                          TINYINT       NOT NULL,
    [Active]                        BIT           NOT NULL,
    [RefreshTokenLifeTimeInMinutes] INT           NOT NULL,
    [AllowedOrigin]                 VARCHAR (100) NOT NULL,
    [CreationDate]                  DATETIME      NOT NULL,
    [CreationUser]                  VARCHAR (25)  NOT NULL,
    [ChangeDate]                    DATETIME      NULL,
    [ChangeUser]                    VARCHAR (25)  NULL,
    CONSTRAINT [PK_AppSecurity.APIClients] PRIMARY KEY CLUSTERED ([APIClientId] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[APIClients] TO [grp_App_ITLabTreinamento]
    AS [dbo];

