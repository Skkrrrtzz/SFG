using Dapper;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<bool> UpdateById(RFQModel formData)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE RFQ SET CustomerPartNumber = @CustomerPartNumber, Rev = @Rev, Description = @Description, OrigMFR = @OrigMFR, OrigMPN = @OrigMPN, Commodity = @Commodity, Eqpa = @Eqpa, UoM = @UoM, Status = @Status WHERE Id = @Id";

                    int result = await conn.ExecuteAsync(query, formData);

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing query: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> InsertRFQ(RFQProjectModel rFQProjects, List<RFQModel> rfqData)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    string RFQProjectsQuery = "INSERT INTO RFQProjects (ProjectName, Customer, QuotationCode, NoItems, RequestDate, RequiredDate, Status) " +
                                  "VALUES (@ProjectName, @Customer, @QuotationCode, @NoItems, @RequestDate, @RequiredDate, @Status)";

                    string RFQQuery = "INSERT INTO RFQ (ProjectName, Customer, QuotationCode, LastPurchaseDate, CustomerPartNumber, Description, Rev, Commodity, OrigMPN, OrigMFR, Eqpa, UoM, Status, Remarks) " +
                       "VALUES (@ProjectName, @Customer, @QuotationCode, @LastPurchaseDate, @CustomerPartNumber, @Description, @Rev, @Commodity, @OrigMPN, @OrigMFR, @Eqpa, @UoM, @Status, @Remarks)";

                    await connection.ExecuteAsync(RFQProjectsQuery, rFQProjects);

                    foreach (var item in rfqData)
                    {
                        await connection.ExecuteAsync(RFQQuery, new
                        {
                            ProjectName = rFQProjects.ProjectName,
                            Customer = rFQProjects.Customer,
                            QuotationCode = rFQProjects.QuotationCode,
                            LastPurchaseDate = item.LastPurchaseDate,
                            CustomerPartNumber = item.CustomerPartNumber,
                            Description = item.Description,
                            Rev = item.Rev,
                            Commodity = item.Commodity,
                            OrigMPN = item.OrigMPN,
                            OrigMFR = item.OrigMFR,
                            Eqpa = item.Eqpa,
                            UoM = item.UoM,
                            Status = item.Status,
                            Remarks = item.Remarks
                        });
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting RFQ: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> InsertAnnualForecast(AddAnnualForecastRequest request)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE RFQ SET AnnualForecast = @AnnualForecast WHERE Id = @Id";

                    for (int i = 0; i < request.Ids.Count; i++)
                    {
                        await conn.ExecuteAsync(query, new { Id = request.Ids[i], AnnualForecast = request.AnnualForecasts[i] });
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating AnnualForecast: {ex.Message}");
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
                await conn.OpenAsync();

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

                    return await conn.QueryAsync<RFQModel>(query, new { ProjectName = projectName });
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