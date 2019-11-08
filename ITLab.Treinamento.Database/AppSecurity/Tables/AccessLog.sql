CREATE TABLE [AppSecurity].[AccessLog] (
    [AccessLogId]          INT           IDENTITY (1, 1) NOT NULL,
    [AttempAccessDateTime] DATETIME      NOT NULL,
    [ClientIP]             VARCHAR (15)  NOT NULL,
    [Login]                VARCHAR (70)  NOT NULL,
    [Active]               BIT           NOT NULL,
    [MessageCode]          VARCHAR (MAX) NULL,
    [MessageDescription]   VARCHAR (MAX) NULL,
    CONSTRAINT [PK_AppSecurity.AccessLog] PRIMARY KEY CLUSTERED ([AccessLogId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_AccessLog_AttempAccessDateTime]
    ON [AppSecurity].[AccessLog]([AttempAccessDateTime] ASC);


GO
GRANT SELECT
    ON OBJECT::[AppSecurity].[AccessLog] TO [grp_App_ITLabTreinamento]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[AppSecurity].[AccessLog] TO [grp_App_ITLabTreinamento]
    AS [dbo];

