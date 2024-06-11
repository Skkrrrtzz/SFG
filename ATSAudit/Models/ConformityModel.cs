using System.ComponentModel.DataAnnotations;


namespace QA_Audit_Fresh.Models
{
    public class ConformityModel
    {
        public ConformityModel() {}

        [Key]
        public int ConformityId { get; set; }
        [Required]
        public int PlanId { get; set; }
        [Required]
        public string? ConformityDescription { get; set; }
        [Required]
        public string? ConformityAreaSection { get; set; }

    }
}