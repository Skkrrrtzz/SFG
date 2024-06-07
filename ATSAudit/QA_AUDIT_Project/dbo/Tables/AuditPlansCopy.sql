CREATE TABLE [dbo].[AuditPlansCopy] (
    [PlanId]          INT            NOT NULL,
    [Requestor]       NVARCHAR (MAX) NOT NULL,
    [Department]      NVARCHAR (MAX) NOT NULL,
    [AuditCategory]   NVARCHAR (MAX) NOT NULL,
    [TargetDate]      DATETIME2 (7)  NOT NULL,
    [ActualAuditDate] DATETIME2 (7)  NULL,
    [Status]          NVARCHAR (MAX) NOT NULL,
    [Remark]          NVARCHAR (MAX) NULL,
    [AuditeeApproved] BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [AuditorApproved] BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [TimeEnd]         NVARCHAR (MAX) DEFAULT (N'') NOT NULL
);


GO

