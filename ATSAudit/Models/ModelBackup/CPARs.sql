-- EXEC sp_UpdateStatus @PlanId = 1031, @Status = 'Closed', @ActualAuditDate = "2024-06-05"
-- SELECT * FROM dbo.AuditPlans WHERE PlanId = 1029;
-- CREATE VIEW dbo.CPARsActualAuditDate
-- AS
-- SELECT 
-- 	B.*, 
-- 	A.ActualAuditDate
-- FROM 
-- 	dbo.CPARs B
-- LEFT JOIN 
-- 	dbo.AuditPlans A ON B.PlanId = A.PlanId

-- DELETE FROM dbo.CPARs WHERE PlanId = 1029;,

SELECT * FROM CPARsActualAuditDate;