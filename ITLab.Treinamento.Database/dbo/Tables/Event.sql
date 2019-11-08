CREATE TABLE [dbo].[Event] (
    [EventId]      INT          IDENTITY (1, 1) NOT NULL,
    [Description]  VARCHAR (30) NOT NULL,
    [Date]         DATE         NOT NULL,
    [Start]        TIME (0)     NOT NULL,
    [End]          TIME (0)     NOT NULL,
    [Color]        VARCHAR (7)  NULL,
    [AllDay]       BIT          NOT NULL,
    [ChangeDate]   DATETIME     NULL,
    [ChangeUser]   VARCHAR (25) NULL,
    [CreationDate] DATETIME     NOT NULL,
    [CreationUser] VARCHAR (25) NOT NULL,
    CONSTRAINT [PK_dbo.Event] PRIMARY KEY CLUSTERED ([EventId] ASC)
);

