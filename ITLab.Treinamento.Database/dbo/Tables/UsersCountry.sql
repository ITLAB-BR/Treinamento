CREATE TABLE [dbo].[UsersCountry] (
    [CountryId] TINYINT NOT NULL,
    [UserId]    INT     NOT NULL,
    CONSTRAINT [PK_dbo.UsersCountry] PRIMARY KEY CLUSTERED ([CountryId] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.UsersCountry_AppSecurity.User_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppSecurity].[User] ([UserId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.UsersCountry_dbo.Country_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([CountryId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[UsersCountry]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CountryId]
    ON [dbo].[UsersCountry]([CountryId] ASC);


GO
GRANT UPDATE
    ON OBJECT::[dbo].[UsersCountry] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UsersCountry] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[UsersCountry] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[UsersCountry] TO [grp_App_ITLabTreinamento]
    AS [dbo];

