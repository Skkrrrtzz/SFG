CREATE TABLE [dbo].[CorrectiveActions] (
    [CorrectiveActionId]          INT            IDENTITY (1, 1) NOT NULL,
    [CPARId]                      INT            NOT NULL,
    [CorrectiveActionDescription] NVARCHAR (MAX) NOT NULL,
    [TargetDate]                  DATETIME2 (7)  NOT NULL,
    [Responsible]                 NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_CorrectiveActionId] PRIMARY KEY CLUSTERED ([CorrectiveActionId] ASC),
    CONSTRAINT [FK_CorrectiveActions_CPARs] FOREIGN KEY ([CPARId]) REFERENCES [dbo].[CPARs] ([CPARId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

