
CREATE PROCEDURE [dbo].[sp_UpdateStatus]
    @PlanId INT,
    @Status NVARCHAR(12),
    @ActualAuditDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- select * from dbo.AuditPlans where MONTH(TargetDate) = @Month;
    -- insert into [dbo].[AuditPlans] (    IF @Status NOT IN ('Open', 'Closed', 'For Approval')
    IF @Status NOT IN ('Open', 'Closed', 'For Approval')
    BEGIN
        RAISERROR ('Invalid status value: %s', 16, 1, @Status);
        RETURN;
    END

    IF @Status = 'Closed' AND @ActualAuditDate IS NULL
    BEGIN
        RAISERROR ('ActualAuditDate must be provided when status is Closed.', 16, 1);
        RETURN;
    END


    UPDATE dbo.AuditPlans 
        SET 
            ActualAuditDate = CASE WHEN @Status = 'Closed' THEN @ActualAuditDate ELSE ActualAuditDate END,
            Status = CASE
                        WHEN @Status = 'For Approval' THEN 0
                        WHEN @Status = 'Open' THEN 1
                        WHEN @Status = 'Closed' THEN 2
                     END
        WHERE PlanId = @PlanId;

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR ('No audit plan found with PlanId: %d', 16, 1, @PlanId);
    END
END;

GO

