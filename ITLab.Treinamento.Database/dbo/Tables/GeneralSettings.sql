CREATE TABLE [dbo].[GeneralSettings] (
    [GeneralSettingId] INT           IDENTITY (1, 1) NOT NULL,
    [SettingName]      VARCHAR (50)  NOT NULL,
    [ValueBool]        BIT           NULL,
    [ValueInt]         INT           NULL,
    [ValueString]      VARCHAR (MAX) NULL,
    [CreationDate]     DATETIME      NOT NULL,
    [CreationUser]     VARCHAR (25)  NOT NULL,
    [ChangeDate]       DATETIME      NULL,
    [ChangeUser]       VARCHAR (25)  NULL,
    CONSTRAINT [PK_dbo.GeneralSettings] PRIMARY KEY CLUSTERED ([GeneralSettingId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GeneralSetting_SettingName]
    ON [dbo].[GeneralSettings]([SettingName] ASC);


GO
GRANT UPDATE
    ON OBJECT::[dbo].[GeneralSettings] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[GeneralSettings] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[GeneralSettings] TO [grp_App_ITLabTreinamento]
    AS [dbo];

