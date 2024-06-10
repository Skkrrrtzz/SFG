using QA_Audit_Fresh.Models;

namespace QA_Audit_Fresh.Repositories
{
    public interface ICPARRepository
    {
        public Task<IEnumerable<CPARModel>> GetCPARs();
        public Task<IEnumerable<CPARModel>> GetCPARByAuditPlanWithActualAuditDate(int cparId);
        public Task<IEnumerable<CPARModel>> GetCPARsByAuditPlan(int planId);
        public Task<IEnumerable<CPARModel>> GetCPAR(int cparId);
        public Task<IEnumerable<CPARModel>> PostInitialCPAR(CPARModel conformity);
        public Task<int> DeleteCPAR(int conformityId);
    }
}