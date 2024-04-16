using System.ComponentModel.DataAnnotations;

namespace SFG.Models
{
    public class RFQModel
    {
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Customer { get; set; } = string.Empty;
        public string QuotationCode { get; set; } = string.Empty;
        public string CustomerPartNumber { get; set; } = string.Empty;
        public string Rev { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? OrigMPN { get; set; } = string.Empty;
        public string? OrigMFR { get; set; } = string.Empty;
        public string Commodity { get; set; } = string.Empty;
        public int Eqpa { get; set; } = 0;
        public string UoM { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? LastPurchaseDate { get; set; }
        public string? Remarks { get; set; } = string.Empty;
    }
    public class MyViewModel
    {
        public List<RFQModel> RFQData { get; set; }
        public List<RFQProjectModel> RFQProjectData { get; set; }
    }

}
