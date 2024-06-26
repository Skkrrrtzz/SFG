using ATSAudit.Models;

namespace ATSAudit.Services
{
    public interface IConformitiesRepository
    {
        public Task<IEnumerable<ConformityModel>> GetConformities();
        public Task<IEnumerable<ConformityModel>> GetConformity(int conformityId);
        public Task<IEnumerable<ConformityModel>> GetConformitiesByAuditPlan(int cparId);
        public Task<int> PostConformity(ConformityModel conformity);
        public Task<int> DeleteConformity(int conformityId);
     
    }
}