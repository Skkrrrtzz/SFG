using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QA_Audit_Fresh.Models.Dto
{
    public class CPARDto
    {
        public int PlanId { get; set; }
        public string Respondent { get; set; }
        public string Requestor { get; set; }
        public DateTime ResponseDueDate { get; set; }
        public string ISOClause { get; set; }
        public string ProblemStatement { get; set; }
    }
}