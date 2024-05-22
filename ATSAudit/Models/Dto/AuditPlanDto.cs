using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QA_Audit_Fresh.Models.Dto
{
    public class AuditPlanDto
    {
        private DateTime _targetDate;
        public string Requestor { get; set; }
        public string Department { get; set; }
        public string AuditCategory { get; set; }
        // public int Year { get; set; }
        // public int Month { get; set; }
        // public int Day { get; set; }

        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }

        public DateTime TargetDate { get; set; }
        public string GetProperties()
        {
            return $"Department: {Department}\nAudit Category: {AuditCategory}\nTimeStart: {TimeStart}\nTimeEnd: {TimeEnd}\nAuditDate: {TargetDate}";
        }
    }
}