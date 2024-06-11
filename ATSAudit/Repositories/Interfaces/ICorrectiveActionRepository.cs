using QA_Audit_Fresh.Models;

namespace QA_Audit_Fresh.Repositories
{
    public interface ICorrectiveActionsRepository
    {
        public Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActions();
        public Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveAction(int correctiveActionId);
        public Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActionsByCPAR(int cparId);
        public Task<int> PostCorrectiveAction(CorrectiveActionModel correction);
        public Task<int> DeleteCorrectiveAction(int correctionId);
     
    }
}