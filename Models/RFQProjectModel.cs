using System.ComponentModel.DataAnnotations;

namespace SFG.Models
{
    public class RFQProjectModel
    {
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public string QuotationCode { get; set; } = string.Empty;
        public int NoItems { get; set; } = 0;
        public DateTime RequestDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
