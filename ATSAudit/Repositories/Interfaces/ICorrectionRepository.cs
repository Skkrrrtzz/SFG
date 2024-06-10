using QA_Audit_Fresh.Models;

namespace QA_Audit_Fresh.Repositories
{
    public interface ICorrectionRepository
    {
        public Task<IEnumerable<CorrectionModel>> GetCorrections();
        public Task<IEnumerable<CorrectionModel>> GetCorrection(int conformityNo);
        public Task<IEnumerable<CorrectionModel>> GetCorrectionsByCPAR(int planId);
        public Task<int> PostCorrection(CorrectionModel correction);
        public Task<int> DeleteCorrection(int correctionId);
     
    }
}