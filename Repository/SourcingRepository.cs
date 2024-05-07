using Dapper;
using Microsoft.Data.SqlClient;
using SFG.Models;

namespace SFG.Repository
{
    public class SourcingRepository : ISourcingRepository
    {
        private readonly string _connectionString;

        public SourcingRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<dynamic>> GetData(string partNumber, string tableName)
        {
            bool tableExists = await TableExists(tableName);
            string query = "";

            if (!tableExists)
            {
                throw new Exception($"Table '{tableName}' does not exist.");
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Build the query based on the table name
                if (tableName == "Quotations" || tableName == "LastPurchaseInfo")
                {
                    query = $"SELECT * FROM {tableName} WHERE PartNumber = @partNumber";
                }
                else
                {
                    query = $@"SELECT i.PartNumberTable AS PartNumberTable,
                        MAX(i.DescriptionTable) AS DescriptionTable,
                        MAX(i.Rev) AS Rev,
                        MAX(i.UOM) AS UOM,
                        MAX(i.Commodity) AS Commodity,
                        MAX(i.MPN) AS MPN,
                        MAX(i.Manufacturer) AS Manufacturer,
                        SUM(CAST(i.EQPA AS DECIMAL)) AS sumEQPA
                   FROM MRPBOM i
                   RIGHT JOIN {tableName} p ON p.PartNumber = i.PartNumber
                   WHERE i.PartNumber = @partNumber
                   GROUP BY i.PartNumberTable;";
                }

                return await conn.QueryAsync(query, new { partNumber });
            }
        }

        public async Task<RFQModel> FindById(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM RFQ WHERE Id = @Id";

                    return await conn.QueryFirstOrDefaultAsync<RFQModel>(query, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing query: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateById(string customerPartNumber, string rev, string description, string origMFR, string origMPN, string commodity, string eqpa, string uoM, int id, string status)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE RFQ SET CustomerPartNumber = @CustomerPartNumber, Rev = @Rev, DescriprigMPN = @Origtion = @Description, OrigMFR = @OrigMFR, OMPN, Commodity = @Commodity, Eqpa = @Eqpa, UoM = @UoM, Status = @Status WHERE Id = @Id";

                    var result = await conn.ExecuteAsync(query, new
                    {
                        Id = id,
                        CustomerPartNumber = customerPartNumber,
                        Rev = rev,
                        Description = description,
                        OrigMFR = origMFR,
                        OrigMPN = origMPN,
                        Commodity = commodity,
                        Eqpa = eqpa,
                        UoM = uoM,
                        Status = status
                    });

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing query: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<dynamic>> RFQQuery(string partNumber, string tableName)
        {
            bool tableExists = await TableExists(tableName);

            if (!tableExists)
            {
                throw new Exception($"Table '{tableName}' does not exist.");
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "";

                if (tableName == "RFQ")
                {
                    query = $"SELECT * FROM {tableName} WHERE ProjectName = @partNumber AND Remarks = 'FOR SOURCING'";
                }
                else
                {
                    query = $"SELECT i.Id,i.ProjectName,i.Customer,i.QuotationCode,i.NoItems,i.RequestDate,i.RequiredDate " +
                        $"FROM {tableName} i INNER JOIN RFQ j ON i.Customer = j.Customer AND i.QuotationCode = j.QuotationCode " +
                        $"WHERE i.ProjectName = @partNumber ";
                }

                return await conn.QueryAsync(query, new { partNumber });
            }
        }

        public async Task<bool> TableExists(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'", conn))
                {
                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        public async Task<IEnumerable<dynamic>> GetLastPurchaseInfo(string partNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = $"SELECT MAX(GWRLQty) AS GWRLQty, MAX(LastPurchasedDate) AS LastPurchasedDate FROM LastPurchaseInfo WHERE ForeignName = @partNumber";

                    return await conn.QueryAsync(query, new { partNumber });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing query: {ex.Message}");
                return null;
            }
        }

        public async Task<RFQProjectModel> GetRFQProject(string projectName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT Id, ProjectName, Customer, QuotationCode, NoItems, RequestDate, RequiredDate FROM RFQProjects WHERE ProjectName = @ProjectName";

                    return await conn.QueryFirstOrDefaultAsync<RFQProjectModel>(query, new { ProjectName = projectName });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing query: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<RFQModel>> GetRFQ(string projectName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM RFQ WHERE ProjectName = @ProjectName AND Remarks = 'FOR SOURCING'";

                    var rfqData = await conn.QueryAsync<RFQModel>(query, new { ProjectName = projectName });

                    return rfqData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing query: {ex.Message}");
                return null;
            }
        }
    }
}