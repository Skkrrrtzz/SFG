using QA_Audit_Fresh.Models;

namespace QA_Audit_Fresh.Repositories
{
    public interface IConformityRepository
    {
        public Task<IEnumerable<ConformityModel>> GetConformities();
        public Task<IEnumerable<ConformityModel>> GetConformity(int conformityId);
        public Task<IEnumerable<ConformityModel>> GetConformitiesByAuditPlan(int cparId);
        public Task<int> PostConformity(ConformityModel conformity);
        public Task<int> DeleteConformity(int conformityId);
     
    }
}