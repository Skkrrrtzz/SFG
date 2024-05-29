using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models;

namespace QA_Audit_Fresh.Repositories
{
    public interface ICPARRepository
    {
        public Task<IEnumerable<CPARsModel>> GetCPARs();
        public Task<IEnumerable<CPARsModel>> GetCPARsByAuditPlan(int planId);
        public Task<IEnumerable<CPARsModel>> GetCPAR(int cparId);
        public Task<IEnumerable<CPARsModel>> PostCPAR(CPARsModel conformity);
        public Task<int> DeleteCPAR(int conformityId);
    }
}