using Dapper;
using Microsoft.Data.SqlClient;
using SFG.Models;

namespace SFG.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly string _connectionString;

        public DashboardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<RFQModel>> GetRFQByQuotationCode(string quotationCode)
        {
            try
            {
                List<RFQModel> result;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM RFQ WHERE QuotationCode = @QuotationCode";

                    result = (await conn.QueryAsync<RFQModel>(query, new { QuotationCode = quotationCode })).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving RFQ data: {ex.Message}");
                throw;
            }
        }

        public async Task<List<RFQProjectModel>> GetRFQProjectsByQuotationCode(string quotationCode)
        {
            try
            {
                List<RFQProjectModel> result;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM RFQProjects WHERE QuotationCode = @QuotationCode";

                    result = (await conn.QueryAsync<RFQProjectModel>(query, new { QuotationCode = quotationCode })).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving RFQ project data: {ex.Message}");
                throw;
            }
        }

        public async Task<List<MRPBOMProductModel>> GetBOM()
        {
            using (SqlConnection conn = new(_connectionString))
            {
                string query = "SELECT * FROM MRPBOMProducts";

                return (await conn.QueryAsync<MRPBOMProductModel>(query)).ToList();
            }
        }

        public async Task<List<RFQProjectModel>> GetOpenRFQProjects()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM RFQProjects WHERE Status = 'OPEN'";

                return (await conn.QueryAsync<RFQProjectModel>(query)).ToList();
            }
        }

        public async Task<List<RFQProjectModel>> GetAllRFQProjects()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM RFQProjects";

                return (await conn.QueryAsync<RFQProjectModel>(query)).ToList();
            }
        }

        public async Task<int> UploadMRPBOM(MRPBOMModel model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO MRPBOM (Product, PartNumber, Item, [Level], PartNumberTable, SAPPartNumber,
                DescriptionTable, Rev, QPA, EQPA, UOM, Commodity, MPN, Manufacturer)
                VALUES (@Product, @PartNumber, @Item, @Level, @PartNumberTable, @SAPPartNumber, @DescriptionTable,
                @Rev, @QPA, @EQPA, @UOM, @Commodity, @MPN, @Manufacturer)";

                return await conn.ExecuteAsync(query, model);
            }
        }

        public async Task<int> UploadMRPBOMProducts(MRPBOMProductModel model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO MRPBOMProducts (Product, PartNumber, Revision, Description, DateModified, PreparedBy, ReviewedBy)
                VALUES (@Product, @PartNumber, @Revision, @Description, @DateModified, @PreparedBy, @ReviewedBy)";

                return await conn.ExecuteAsync(query, model);
            }
        }

        public async Task UploadLastPurchaseInfo(LastPurchaseInfoModel model)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = @"INSERT INTO MRPBOMProducts (ItemNo, ForeignName, ItemDescription, Unit, GWRLQty, LastPurchasedDate, LastPurchasedUSDPrice, CustomerVendorCode,
        CustomerVendorName, RMWHEREUSED, FGName)
        VALUES (@ItemNo, @ForeignName, @ItemDescription, @Unit, @GWRLQty, @LastPurchasedDate, @LastPurchasedUSDPrice, @CustomerVendorCode, @CustomerVendorName, @RMWHEREUSED, @FGName)";

                    await conn.ExecuteAsync(query, model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading Last Purchase Info: {ex.Message}");
                throw;
            }
        }

        public async Task UploadQuotations(QuotationModel model)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = @"INSERT INTO Quotations (PartNumber) VALUES (@PartNumber)";

                    await conn.ExecuteAsync(query, model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading Quotations: {ex.Message}");
                throw;
            }
        }
    }
}