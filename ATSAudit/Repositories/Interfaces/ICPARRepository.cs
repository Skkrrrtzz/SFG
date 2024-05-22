using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models;

namespace QA_Audit_Fresh.Repositories
{
    public interface ICPARRepository
    {
        public Task<IEnumerable<CPARModel>> GetCPARs();
        public Task<IEnumerable<CPARModel>> GetCPAR(int conformityId);
        public Task<IEnumerable<CPARModel>> PostCPAR(CPARModel conformity);
        public Task<int> DeleteCPAR(int conformityId);
    }
}