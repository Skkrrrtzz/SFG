﻿using ATSSFG.Models;

namespace ATSSFG.Repository
{
    public interface IDashboardRepository
    {
        public Task<List<RFQModel>> GetRFQByQuotationCode(string quotationCode);

        public Task<List<RFQProjectModel>> GetRFQProjectsByQuotationCode(string quotationCode);

        public Task<List<RFQProjectModel>> GetIncomingRFQProjects();

        public Task<List<RFQProjectModel>> GetAllRFQProjects();

        public Task<List<dynamic>> GetRFQProjectsSummary();

        public Task<List<dynamic>> GetOpenProjectsSummary();

        public Task<List<dynamic>> CheckingPartNumber();

        public Task<List<MRPBOMProductModel>> GetMRPBOMProducts();

        public Task<int> UploadMRPBOM(MRPBOMModel model);

        public Task<int> UploadMRPBOMProducts(MRPBOMProductModel model);

        public Task UploadQuotations(IEnumerable<QuotationModel> quotations);

        public Task BulkInsertLastPurchaseInfo(IEnumerable<LastPurchaseInfoModel> lastpurchaseinfos);

        public Task<bool> MarkAsClosed(string quotationCode, string projectName);
    }
}