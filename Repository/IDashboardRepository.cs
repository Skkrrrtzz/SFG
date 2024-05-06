using Microsoft.Data.SqlClient;
using SFG.Models;

namespace SFG.Repository
{
    public interface IDashboardRepository
    {
        public Task<List<RFQModel>> GetRFQByQuotationCode(string quotationCode);
        public Task<List<RFQProjectModel>> GetRFQProjectsByQuotationCode(string quotationCode);
        public Task<List<RFQProjectModel>> GetOpenRFQProjects();
        public Task<List<RFQProjectModel>> GetAllRFQProjects();
        public Task<List<MRPBOMProductModel>> GetBOM();
        public Task<int> UploadMRPBOM(MRPBOMModel model);
        public Task<int> UploadMRPBOMProducts(MRPBOMProductModel model);
        public Task UploadLastPurchaseInfo(LastPurchaseInfoModel lastPurchaseInfo);
        public Task UploadQuotations(QuotationModel quotations);
    }
}
