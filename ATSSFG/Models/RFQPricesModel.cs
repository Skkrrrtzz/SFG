namespace ATSSFG.Models
{
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
}
