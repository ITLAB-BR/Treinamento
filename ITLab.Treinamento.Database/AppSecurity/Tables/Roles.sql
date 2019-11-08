CREATE TABLE [AppSecurity].[Roles] (
    [RoleId]       INT          NOT NULL,
    [CreationDate] DATETIME     NOT NULL,
    [CreationUser] VARCHAR (25) NOT NULL,
    [ChangeDate]   DATETIME     NULL,
    [ChangeUser]   VARCHAR (25) NULL,
    [Name]         VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AppSecurity.Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_RoleName]
    ON [AppSecurity].[Roles]([Name] ASC);


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[Roles] TO [grp_App_ITLabTreinamento]
    AS [dbo];

