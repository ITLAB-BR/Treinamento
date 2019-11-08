CREATE TABLE [dbo].[Country] (
    [CountryId]    TINYINT      IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (30) NOT NULL,
    [Active]       BIT          NOT NULL,
    [CreationDate] DATETIME     NOT NULL,
    [CreationUser] VARCHAR (25) NOT NULL,
    [ChangeDate]   DATETIME     NULL,
    [ChangeUser]   VARCHAR (25) NULL,
    CONSTRAINT [PK_dbo.Country] PRIMARY KEY CLUSTERED ([CountryId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Country_Name]
    ON [dbo].[Country]([Name] ASC);


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Country] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Country] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Country] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Country] TO [grp_App_ITLabTreinamento]
    AS [dbo];

