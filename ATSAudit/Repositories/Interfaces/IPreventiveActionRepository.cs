using QA_Audit_Fresh.Models;

namespace QA_Audit_Fresh.Repositories
{
    public interface IPreventiveActionRepository
    {
        public Task<IEnumerable<PreventiveActionModel>> GetPreventiveActions();
        public Task<IEnumerable<PreventiveActionModel>> GetPreventiveAction(int preventiveActionId);
        public Task<IEnumerable<PreventiveActionModel>> GetPreventiveActionsByCPAR(int cparId);
        public Task<int> PostPreventiveAction(PreventiveActionModel correction);
        public Task<int> DeletePreventiveAction(int correctionId);
     
    }
}