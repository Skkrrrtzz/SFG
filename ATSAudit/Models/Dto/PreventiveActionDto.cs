using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QA_Audit_Fresh.Models.Dto
{
    public class PreventiveActionDto
    {
        public int PreventiveActionId { get; set; }
        public int CPARId { get; set; }
        public string PreventiveActionDescription { get; set; }
        public DateTime TargetDate { get; set; }
        public string Responsible { get; set; }
    }
}