CREATE TABLE [dbo].[AuditPlans] (
    [PlanId]          INT            IDENTITY (1, 1) NOT NULL,
    [Requestor]       NVARCHAR (MAX) NOT NULL,
    [Department]      NVARCHAR (MAX) NOT NULL,
    [AuditCategory]   NVARCHAR (MAX) NOT NULL,
    [TargetDate]      DATETIME2 (7)  NOT NULL,
    [ActualAuditDate] DATETIME2 (7)  NULL,
    [Status]          TINYINT        NOT NULL,
    [Remark]          NVARCHAR (MAX) NULL,
    [AuditeeApproved] BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [AuditorApproved] BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [TimeEnd]         NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    CONSTRAINT [PK_AuditPlans] PRIMARY KEY CLUSTERED ([PlanId] ASC),
    CONSTRAINT [UC_TargetDate] UNIQUE NONCLUSTERED ([TargetDate] ASC)
);


GO

