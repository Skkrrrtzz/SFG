using ATSAudit.Models;

namespace ATSAudit.Services
{
    public interface IAuditPlansRepository
    {
        public Task<IEnumerable<AuditPlanModel>> GetAuditPlans();
        public Task<IEnumerable<AuditPlanModel>> GetAuditPlansByMonth(int month);
        public Task<AuditPlanModel> GetAuditPlan(int planId);
        public Task<IEnumerable<AuditPlanModel>> PostAuditPlan(AuditPlanModel auditPlan);
        public Task<int> UpdateStatus(int planId, string status, DateTime actualAuditDate);
        public Task<int> UpdateStatus(int planId, string status);
        public Task<int> DeleteAuditPlan(int planId);
    }
}