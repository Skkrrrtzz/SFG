using Microsoft.Data.SqlClient;
using Dapper;
using SFG.Models;

namespace SFG.Repository
{
    public class SourcingRepository : ISourcingRepository
    {
        private readonly string ConnectionString = "Data Source=DASHBOARDPC;Initial Catalog=SFGDb;Persist Security Info=True;User ID=sa;Password=test@123;Multiple Active Result Sets=True;Trust Server Certificate=True";
        protected string GetConnection()
        {
            return ConnectionString;
        }
        public async Task<IEnumerable<dynamic>> GetData(string partNumber, string tableName)
        {
            // Check if the specified table exists
            bool tableExists = await TableExists(tableName);
            string query = "";

            // If the table does not exist, throw an exception
            if (!tableExists)
            {
                throw new Exception($"Table '{tableName}' does not exist.");
            }

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

            // Execute the query
            using (SqlConnection conn = new SqlConnection(GetConnection()))
            {
                await conn.OpenAsync();
                return await conn.QueryAsync(query, new { partNumber });
            }
        }
        private async Task<bool> TableExists(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(GetConnection()))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'", conn))
                {
                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }
        public async Task<RFQProjectModel> GetRFQProject(string projectName)
        {
            try
            {
                string query = "SELECT Id, ProjectName, Customer, QuotationCode, NoItems, RequestDate, RequiredDate FROM RFQProjects WHERE ProjectName = @ProjectName";

                using (SqlConnection conn = new SqlConnection(GetConnection()))
                {
                    // Execute the query asynchronously
                    var rfqProjectData = await conn.QueryFirstOrDefaultAsync<RFQProjectModel>(query, new { ProjectName = projectName });

                    // Return the retrieved RFQ data
                    return rfqProjectData;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                Console.WriteLine($"Error processing query: {ex.Message}");
                return null;
            }
        }
        public async Task<IEnumerable<RFQModel>> GetRFQ(string projectName)
        {
            try
            {
                string query = "SELECT * FROM RFQ WHERE ProjectName = @ProjectName AND Remarks = 'FOR SOURCING'";

                using (SqlConnection conn = new SqlConnection(GetConnection()))
                {
                    // Execute the query asynchronously
                    var rfqData = await conn.QueryAsync<RFQModel>(query, new { ProjectName = projectName });

                    // Return the retrieved RFQ data
                    return rfqData;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                Console.WriteLine($"Error processing query: {ex.Message}");
                return null;
            }
        }
    }
}
