using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models.Dto;

namespace QA_Audit_Fresh.Models
{
    public class CorrectiveActionModel
    {
        public CorrectiveActionModel(CorrectiveActionDto response) {
            CPARId = response.CPARId;
            CorrectiveActionDescription = response.CorrectiveActionDescription;
            TargetDate = response.TargetDate;
            Responsible = response.Responsible;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public CorrectiveActionModel() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [Key]
        public int CorrectiveActionId { get; set; }
        public int CPARId { get; set; }
        public string CorrectiveActionDescription { get; set; }
        public DateTime? TargetDate { get; set; }
        public string Responsible  { get; set; }
    }
}