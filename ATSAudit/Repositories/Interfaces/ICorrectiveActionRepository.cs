using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models;
using QA_Audit_Fresh.Models.Dto;

namespace QA_Audit_Fresh.Repositories
{
    public interface ICorrectiveActionRepository
    {
        public Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActions();
        public Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveAction(int conformityNo);
        public Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActionsByCPAR(int planId);
        public Task<int> PostCorrectiveAction(CorrectiveActionModel correction);
        public Task<int> DeleteCorrectiveAction(int correctionId);
     
    }
}