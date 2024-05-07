using SFG.Models;

namespace SFG.Repository
{
    public interface ISourcingRepository
    {
        public Task<IEnumerable<dynamic>> GetData(string partNumber, string tableName);

        public Task<RFQProjectModel> GetRFQProject(string projectName);

        public Task<RFQModel> FindById(int id);

        public Task<bool> UpdateById(string customerPartNumber, string rev, string description, string origMFR, string origMPN, string commodity, string eqpa, string uoM, int id, string status);

        public Task<IEnumerable<RFQModel>> GetRFQ(string projectName);

        public Task<IEnumerable<dynamic>> GetLastPurchaseInfo(string partNumber);

        public Task<IEnumerable<dynamic>> RFQQuery(string partNumber, string tableName);

        public Task<bool> TableExists(string tableName);
    }
}