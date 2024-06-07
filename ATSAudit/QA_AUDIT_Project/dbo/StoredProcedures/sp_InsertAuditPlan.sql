
CREATE PROCEDURE [dbo].[sp_InsertAuditPlan]
    @Requestor nvarchar(30),
    @Department nvarchar(30),
    @AuditCategory nvarchar(30),
    @TargetDate datetime2(7),
    @TimeEnd datetime2(7),
    @AuditorApproved bit,
    @AuditeeApproved bit,
    @Status tinyint
AS
BEGIN
    DECLARE @ReturnedAuditPlan TABLE (
        [PlanId] [int] IDENTITY(1,1) NOT NULL,
        [Requestor] [nvarchar](max) NOT NULL,
        [Department] [nvarchar](max) NOT NULL,
        [AuditCategory] [nvarchar](max) NOT NULL,
        [TargetDate] [datetime2](7) NOT NULL,
        [ActualAuditDate] [datetime2](7) NULL,
        [Status] [tinyint] NOT NULL,
        [Remark] [nvarchar](max) NULL,
        [AuditeeApproved] [bit] NOT NULL,
        [AuditorApproved] [bit] NOT NULL,
        [TimeEnd] [nvarchar](max) NOT NULL
    );

    INSERT INTO [dbo].[AuditPlans] (Requestor, Department, AuditCategory, TargetDate, TimeEnd, AuditorApproved, AuditeeApproved, Status)
    OUTPUT INSERTED.*
    VALUES ( @Requestor, @Department, @AuditCategory, @TargetDate, @TimeEnd, @AuditorApproved, @AuditeeApproved, @Status );
END;

GO

