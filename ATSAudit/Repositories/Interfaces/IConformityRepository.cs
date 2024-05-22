using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models;
using QA_Audit_Fresh.Models.Dto;

namespace QA_Audit_Fresh.Repositories
{
    public interface IConformityRepository
    {
        public Task<IEnumerable<ConformityModel>> GetConformities();
        public Task<IEnumerable<ConformityModel>> GetConformity(int conformityNo);
        public Task<IEnumerable<ConformityModel>> GetConformitiesByAuditPlan(int planId);
        public Task<int> PostConformity(ConformityModel conformity);
        public Task<int> DeleteConformity(int conformityId);
     
    }
}