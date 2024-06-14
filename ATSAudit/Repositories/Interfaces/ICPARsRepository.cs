using ATSAudit.Models;

namespace ATSAudit.Repositories
{
    public interface ICPARsRepository
    {
        public Task<IEnumerable<CPARModel>> GetCPARs();
        public Task<IEnumerable<CPARModel>> GetCPARByAuditPlanWithActualAuditDate(int cparId);
        public Task<IEnumerable<CPARModel>> GetCPARsByAuditPlan(int planId);
        public Task<IEnumerable<CPARModel>> GetCPAR(int cparId);
        public Task<IEnumerable<CPARModel>> PostInitialCPAR(CPARModel conformity);
        public Task<int> DeleteCPAR(int conformityId);
    }
}