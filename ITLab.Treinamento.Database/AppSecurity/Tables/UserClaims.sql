CREATE TABLE [AppSecurity].[UserClaims] (
    [UserClaimId] INT           IDENTITY (1, 1) NOT NULL,
    [UserId]      INT           NOT NULL,
    [ClaimType]   VARCHAR (MAX) NULL,
    [ClaimValue]  VARCHAR (MAX) NULL,
    CONSTRAINT [PK_AppSecurity.UserClaims] PRIMARY KEY CLUSTERED ([UserClaimId] ASC),
    CONSTRAINT [FK_AppSecurity.UserClaims_AppSecurity.User_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppSecurity].[User] ([UserId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [AppSecurity].[UserClaims]([UserId] ASC);


GO
GRANT UPDATE
    ON OBJECT::[AppSecurity].[UserClaims] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[UserClaims] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[UserClaims] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[AppSecurity].[UserClaims] TO [grp_App_ITLabTreinamento]
    AS [dbo];

