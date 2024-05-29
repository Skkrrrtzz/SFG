using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using QA_Audit_Fresh.Models.Dto;


namespace QA_Audit_Fresh.Models
{
    public class ConformityModel
    {
        public ConformityModel(ConformityDto response)
        {
            PlanId = response.PlanId;
            ConformityDescription = response.Description;
            ConformityAreaSection = response.AreaSection;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public ConformityModel() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public int ConformityId { get; set; }
        public int PlanId { get; set; }
        public string ConformityDescription { get; set; }
        public string? ConformityAreaSection { get; set; }


    }

}