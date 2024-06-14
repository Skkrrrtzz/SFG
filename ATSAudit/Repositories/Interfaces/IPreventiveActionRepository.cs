using ATSAudit.Models;

namespace ATSAudit.Repositories
{
    public interface IPreventiveActionsRepository
    {
        public Task<IEnumerable<PreventiveActionModel>> GetPreventiveActions();
        public Task<IEnumerable<PreventiveActionModel>> GetPreventiveAction(int preventiveActionId);
        public Task<IEnumerable<PreventiveActionModel>> GetPreventiveActionsByCPAR(int cparId);
        public Task<int> PostPreventiveAction(PreventiveActionModel correction);
        public Task<int> DeletePreventiveAction(int correctionId);
     
    }
}