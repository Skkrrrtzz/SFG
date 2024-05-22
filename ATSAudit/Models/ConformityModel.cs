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

        public ConformityModel() {}

        public int ConformityId { get; set; }
        public int PlanId { get; set; }
        public string ConformityDescription { get; set; }
        public string? ConformityAreaSection { get; set; }


    }

}