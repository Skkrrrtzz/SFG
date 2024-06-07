CREATE TABLE [dbo].[Corrections] (
    [CorrectionId]          INT            IDENTITY (1, 1) NOT NULL,
    [CPARId]                INT            NOT NULL,
    [CorrectionDescription] NVARCHAR (MAX) NOT NULL,
    [EscapeCause]           NVARCHAR (MAX) NOT NULL,
    [Action]                NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_CorrectionId] PRIMARY KEY CLUSTERED ([CorrectionId] ASC),
    CONSTRAINT [FK_Corrections_CPARs] FOREIGN KEY ([CPARId]) REFERENCES [dbo].[CPARs] ([CPARId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

