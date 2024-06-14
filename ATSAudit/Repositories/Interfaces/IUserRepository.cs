using ATSAudit.Models;

namespace ATSAudit.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UserModel>> CheckUserExists();
        // public Task<IEnumerable<PreventiveActionModel>> GetPreventiveAction(int preventiveActionId);
        // public Task<IEnumerable<PreventiveActionModel>> GetPreventiveActionsByCPAR(int cparId);
        // public Task<int> PostPreventiveAction(PreventiveActionModel correction);
        // public Task<int> DeletePreventiveAction(int correctionId);
     
    }
}