using ATSAudit.Models;

namespace ATSAudit.Services
{
    public interface ICorrectionsRepository
    {
        public Task<IEnumerable<CorrectionModel>> GetCorrections();
        public Task<IEnumerable<CorrectionModel>> GetCorrection(int conformityNo);
        public Task<IEnumerable<CorrectionModel>> GetCorrectionsByCPAR(int planId);
        public Task<int> PostCorrection(CorrectionModel correction);
        public Task<int> DeleteCorrection(int correctionId);
     
    }
}