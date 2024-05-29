using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using QA_Audit_Fresh.Models.Dto;

namespace QA_Audit_Fresh.Models
{
    public class PreventiveActionModel
    {
        public PreventiveActionModel(PreventiveActionDto response) {
            CPARId = response.CPARId;
            PreventiveActionDescription = response.PreventiveActionDescription;
            EscapeCause = response.EscapeCause;
            Action = response.Action;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public PreventiveActionModel() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [Key]
        public int PreventiveActionId { get; set; }
        public int CPARId { get; set; }
        public string PreventiveActionDescription { get; set; }
        public string EscapeCause { get; set; }
        public string Action { get; set; }
    }
}