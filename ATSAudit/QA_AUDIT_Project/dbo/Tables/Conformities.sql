CREATE TABLE [dbo].[Conformities] (
    [PlanId]                INT            NOT NULL,
    [ConformityDescription] NVARCHAR (MAX) NOT NULL,
    [ConformityAreaSection] NVARCHAR (MAX) NULL,
    [ConformityId]          INT            IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ConformityId] PRIMARY KEY CLUSTERED ([ConformityId] ASC),
    CONSTRAINT [FK_AuditPlans_Conformities] FOREIGN KEY ([PlanId]) REFERENCES [dbo].[AuditPlans] ([PlanId]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO

