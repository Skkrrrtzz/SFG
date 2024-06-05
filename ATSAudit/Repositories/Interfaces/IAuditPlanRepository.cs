using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models.Dto;
using QA_Audit_Fresh.Models;

namespace QA_Audit_Fresh.Repositories
{
    public interface IAuditPlanRepository
    {
        public Task<IEnumerable<AuditPlanModel>> GetAuditPlans();
        public Task<IEnumerable<AuditPlanModel>> GetAuditPlansByMonth(int month);
        public Task<IEnumerable<AuditPlanModel>> GetAuditPlan(int planId);
        public Task<IEnumerable<AuditPlanModel>> PostAuditPlan(AuditPlanModel auditPlan);
        public Task<int> UpdateStatus(int planId, string status, DateTime? actualAuditDate);
        public Task<int> UpdateStatus(int planId, string status);
        public Task<int> DeleteAuditPlan(int planId);
    }
}