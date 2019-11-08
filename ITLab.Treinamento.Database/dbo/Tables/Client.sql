CREATE TABLE [dbo].[Client] (
    [ClientId]     INT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (30) NOT NULL,
    [Email]        VARCHAR (50) NULL,
    [CNPJ]         BIGINT       NULL,
    [CPF]          BIGINT       NULL,
    [Telephone]    BIGINT       NULL,
    [BirthDate]    DATE         NULL,
    [Active]       BIT          NOT NULL,
    [CreationDate] DATETIME     NOT NULL,
    [CreationUser] VARCHAR (25) NOT NULL,
    [ChangeDate]   DATETIME     NULL,
    [ChangeUser]   VARCHAR (25) NULL,
    CONSTRAINT [PK_dbo.Client] PRIMARY KEY CLUSTERED ([ClientId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Client_Name]
    ON [dbo].[Client]([Name] ASC);

