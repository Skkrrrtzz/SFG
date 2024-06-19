using System.ComponentModel.DataAnnotations;

namespace ATSAudit.Models
{
    public class CorrectionModel
    {
        public CorrectionModel() {}

        [Key]
        public int CorrectionId { get; set; }
        [Required]
        public int CPARId { get; set; }
        [Required]
        public string? CorrectionDescription { get; set; }
        [Required]
        public string? EscapeCause { get; set; }
        [Required]
        public string? Action { get; set; }
    }
}