CREATE TABLE [dbo].[NotificationUsers] (
    [NotificationId] INT      NOT NULL,
    [UserId]         INT      NOT NULL,
    [Active]         BIT      NOT NULL,
    [ReadIn]         DATETIME NULL,
    CONSTRAINT [PK_dbo.NotificationUsers] PRIMARY KEY CLUSTERED ([NotificationId] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.NotificationUsers_AppSecurity.User_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppSecurity].[User] ([UserId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.NotificationUsers_dbo.Notifications_NotificationId] FOREIGN KEY ([NotificationId]) REFERENCES [dbo].[Notifications] ([NotificationId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[NotificationUsers]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_NotificationId]
    ON [dbo].[NotificationUsers]([NotificationId] ASC);

