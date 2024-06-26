using ATSAudit.Models;

namespace ATSAudit.Services
{
    public interface ICorrectiveActionsRepository
    {
        public Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActions();
        public Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveAction(int correctiveActionId);
        public Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActionsByCPAR(int cparId);
        public Task<int> PostCorrectiveAction(CorrectiveActionModel correction);
        public Task<int> DeleteCorrectiveAction(int correctionId);
        public Task<int> CloseCorrectiveAction(int cparId, DateTime closeDate);
    }
}