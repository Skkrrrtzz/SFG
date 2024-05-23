using System.ComponentModel.DataAnnotations;

namespace ATSSFG.Models
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
        public int? AnnualForecast { get; set; } = 0;
        public string UoM { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? LastPurchaseDate { get; set; }
        public string? Remarks { get; set; } = string.Empty;
    }

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

    public class MyViewModel
    {
        public List<RFQModel> RFQData { get; set; }
        public List<RFQProjectModel> RFQProjectData { get; set; }
    }

    public class SupplierCostDetail
    {
        public Dictionary<int, decimal> UnitCosts { get; set; } = new Dictionary<int, decimal>();
        public string Currency { get; set; }
        public string Supplier { get; set; }
        public int MOQ { get; set; }
        public int? SPQ { get; set; }
        public string PurchasingUOM { get; set; }
        public int LeadTimeWeeks { get; set; }
        public string Location { get; set; }
        public string QuoteValidity { get; set; }
        public string SourcingRemarks { get; set; }
        public decimal? ToolingCost { get; set; }
        public int? ToolingLeadTimeWeeks { get; set; }
        public string ToolingSourcingRemarks { get; set; }
    }

    public class PartData
    {
        public string PartNumber { get; set; }
        public string ProjectName { get; set; }
        public List<SupplierCostDetail> SupplierDetails { get; set; } = new List<SupplierCostDetail>();
        public string SuggestedSupplier { get; set; }
        public string Comments { get; set; }
    }

    public class AddAnnualForecastRequest
    {
        public List<int> Ids { get; set; }
        public List<int> AnnualForecasts { get; set; }
    }
}