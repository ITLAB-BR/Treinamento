CREATE TABLE [dbo].[Notifications] (
    [NotificationId] INT           IDENTITY (1, 1) NOT NULL,
    [Message]        VARCHAR (150) NOT NULL,
    [ChangeDate]     DATETIME      NULL,
    [ChangeUser]     VARCHAR (25)  NULL,
    [CreationDate]   DATETIME      NOT NULL,
    [CreationUser]   VARCHAR (25)  NOT NULL,
    CONSTRAINT [PK_dbo.Notifications] PRIMARY KEY CLUSTERED ([NotificationId] ASC)
);

