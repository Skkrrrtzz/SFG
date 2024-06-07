CREATE TABLE [dbo].[PreventiveActions] (
    [PreventiveActionId]          INT            IDENTITY (1, 1) NOT NULL,
    [CPARId]                      INT            NOT NULL,
    [PreventiveActionDescription] NVARCHAR (MAX) NOT NULL,
    [TargetDate]                  DATETIME2 (7)  NOT NULL,
    [Responsible]                 NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_PreventiveActionId] PRIMARY KEY CLUSTERED ([PreventiveActionId] ASC),
    CONSTRAINT [FK_PreventiveActions_CPARs] FOREIGN KEY ([CPARId]) REFERENCES [dbo].[CPARs] ([CPARId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

