using SFG.Models;

namespace SFG.Repository
{
    public interface ISourcingRepository
    {
        public Task<IEnumerable<dynamic>> GetData(string partNumber, string tableName);
        public Task<RFQProjectModel> GetRFQProject(string projectName);
        public Task<IEnumerable<RFQModel>> GetRFQ(string projectName);
    }
}
