CREATE TABLE [AppSecurity].[Group] (
    [GroupId]      INT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (30) NOT NULL,
    [ChangeDate]   DATETIME     NULL,
    [ChangeUser]   VARCHAR (25) NULL,
    [CreationDate] DATETIME     NOT NULL,
    [CreationUser] VARCHAR (25) NOT NULL,
    CONSTRAINT [PK_AppSecurity.Group] PRIMARY KEY CLUSTERED ([GroupId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GroupName]
    ON [AppSecurity].[Group]([Name] ASC);


GO
GRANT UPDATE
    ON OBJECT::[AppSecurity].[Group] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[Group] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[Group] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[AppSecurity].[Group] TO [grp_App_ITLabTreinamento]
    AS [dbo];

