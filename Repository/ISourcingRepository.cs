using Microsoft.AspNetCore.Mvc;
using SFG.Models;

namespace SFG.Repository
{
    public interface ISourcingRepository
    {
        public Task<IEnumerable<dynamic>> GetData(string partNumber, string tableName);

        public Task<RFQProjectModel> GetRFQProject(string projectName);

        public Task<RFQModel> FindById(int id);

        public Task<bool> UpdateById(RFQModel formData);

        public Task<bool> InsertAnnualForecast(AddAnnualForecastRequest request);

        public Task<bool> InsertRFQ(RFQProjectModel rfqProjects, List<RFQModel> rfqData);

        public Task<IEnumerable<RFQModel>> GetRFQ(string projectName);

        public Task<IEnumerable<dynamic>> GetLastPurchaseInfo(string partNumber);

        public Task<IEnumerable<dynamic>> RFQQuery(string partNumber, string tableName);

        public Task<IEnumerable<RFQModel>> GetRFQPartNumbers(string projectName, string quotationCode);

        public Task<bool> TableExists(string tableName);
    }
}