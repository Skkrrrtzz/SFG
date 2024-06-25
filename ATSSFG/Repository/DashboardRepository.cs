using APPCommon.Class;
using ATSSFG.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace ATSSFG.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly string _connectionString;

        public DashboardRepository(IConfiguration configuration)
        {
            _connectionString = PIMESSettings.atsSFGConnString;
        }

        public async Task<List<RFQModel>> GetRFQByQuotationCode(string quotationCode)
        {
            List<RFQModel> result;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM RFQ WHERE QuotationCode = @QuotationCode";

                return result = (await conn.QueryAsync<RFQModel>(query, new { QuotationCode = quotationCode })).ToList();
            }
        }

        public async Task<List<RFQProjectModel>> GetRFQProjectsByQuotationCode(string quotationCode)
        {
            List<RFQProjectModel> result;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM RFQProjects WHERE QuotationCode = @QuotationCode";

                return result = (await conn.QueryAsync<RFQProjectModel>(query, new { QuotationCode = quotationCode })).ToList();
            }
        }

        public async Task<List<MRPBOMProductModel>> GetMRPBOMProducts()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string storedProcedure = "GetMRPBOMProducts";

                var result = await conn.QueryAsync<MRPBOMProductModel>(storedProcedure, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<RFQProjectModel>> GetIncomingRFQProjects()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string storedProcedure = "GetIncomingRFQProjects";

                var result = await conn.QueryAsync<RFQProjectModel>(storedProcedure, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<RFQProjectModel>> GetIncomingRFQProjects_1()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = "SELECT * FROM [GetIncomingRFQ_1]";

                var result = await conn.QueryAsync<RFQProjectModel>(query);
                return result.ToList();
            }
        }
        public async Task<List<RFQProjectModel>> GetIncomingRFQProjects_2()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                string query = "SELECT * FROM GetIncomingRFQ_2";

                var result = await conn.QueryAsync<RFQProjectModel>(query);
                return result.ToList();
            }
        }

        public async Task<List<dynamic>> GetSummaryRFQperMonth(string date)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string storedProcedure = "GetSummaryRFQperMonth";

                var dateParts = date.Split('-');
                if (dateParts.Length != 2 || !int.TryParse(dateParts[0], out int year) || !int.TryParse(dateParts[1], out int month))
                {
                    throw new ArgumentException("Date must be in the format 'yyyy-MM'.");
                }
                var parameters = new DynamicParameters();
                parameters.Add("@month", month);
                parameters.Add("@year", year);

                var result = await conn.QueryAsync<dynamic>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
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

        public async Task<List<dynamic>> GetRFQProjectsSummary()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string storedProcedure = "GetRFQProjectSummary";

                var result = await conn.QueryAsync<dynamic>(storedProcedure, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<dynamic>> CheckingPartNumber()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string storedProcedure = "CheckingPartNumber_SP";

                var result = await conn.QueryAsync<dynamic>(storedProcedure, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<List<dynamic>> GetOpenProjectsSummary()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string storedProcedure = "GetOpenProjectsSummary";

                var result = await conn.QueryAsync<dynamic>(storedProcedure, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<int> UploadMRPBOM(MRPBOMModel model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string storedProcedure = "MRPBOMInsert_SP";

                var parameters = new
                {
                    Product = model.Product,
                    PartNumber = model.PartNumber,
                    Item = model.Item,
                    Level = model.Level,
                    PartNumberTable = model.PartNumberTable,
                    SAPPartNumber = model.SAPPartNumber,
                    DescriptionTable = model.DescriptionTable,
                    Rev = model.Rev,
                    QPA = model.QPA,
                    EQPA = model.EQPA,
                    UOM = model.UOM,
                    Commodity = model.Commodity,
                    MPN = model.MPN,
                    Manufacturer = model.Manufacturer
                };

                return await conn.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> UploadMRPBOMProducts(MRPBOMProductModel model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string storedProcedure = "MRPBOMProductsInsert_SP";

                var parameters = new
                {
                    Product = model.Product,
                    PartNumber = model.PartNumber,
                    Revision = model.Revision,
                    Description = model.Description,
                    DateModified = model.DateModified,
                    PreparedBy = model.PreparedBy,
                    ReviewedBy = model.ReviewedBy
                };

                return await conn.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UploadQuotations(IEnumerable<QuotationModel> models)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (var transaction = conn.BeginTransaction())
                {
                    using (var command = new SqlCommand("dbo.InsertQuotations", conn, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Convert the list of models to a DataTable
                        var table = new DataTable();
                        table.Columns.Add("PartNumber", typeof(string));

                        foreach (var model in models)
                        {
                            table.Rows.Add(model.PartNumber);
                        }

                        var parameter = command.Parameters.AddWithValue("@Quotations", table);
                        parameter.SqlDbType = SqlDbType.Structured;
                        parameter.TypeName = "dbo.QuotationType";

                        await command.ExecuteNonQueryAsync();
                    }
                    transaction.Commit();
                }
            }
        }

        public async Task BulkInsertLastPurchaseInfo(IEnumerable<LastPurchaseInfoModel> models)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (var transaction = conn.BeginTransaction())
                {
                    using (var command = new SqlCommand("dbo.InsertLastPurchaseInfo", conn, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Convert the list of models to a DataTable
                        var table = new DataTable();
                        table.Columns.Add("ItemNo", typeof(string));
                        table.Columns.Add("ForeignName", typeof(string));
                        table.Columns.Add("ItemDescription", typeof(string));
                        table.Columns.Add("Unit", typeof(string));
                        table.Columns.Add("GWRLQty", typeof(decimal));
                        table.Columns.Add("LastPurchasedDate", typeof(DateTime));
                        table.Columns.Add("LastPurchasedUSDPrice", typeof(decimal));
                        table.Columns.Add("CustomerVendorCode", typeof(string));
                        table.Columns.Add("CustomerVendorName", typeof(string));
                        table.Columns.Add("RMWHEREUSED", typeof(string));
                        table.Columns.Add("FGName", typeof(string));

                        foreach (var model in models)
                        {
                            table.Rows.Add(model.ItemNo, model.ForeignName, model.ItemDescription, model.Unit, model.GWRLQty, model.LastPurchasedDate, model.LastPurchasedUSDPrice, model.CustomerVendorCode, model.CustomerVendorName, model.RMWHEREUSED, model.FGName);
                        }

                        var parameter = command.Parameters.AddWithValue("@LastPurchaseInfo", table);
                        parameter.SqlDbType = SqlDbType.Structured;
                        parameter.TypeName = "dbo.LastPurchaseInfoType";

                        await command.ExecuteNonQueryAsync();
                    }
                    transaction.Commit();
                }
            }
        }

        public async Task<bool> MarkAsClosed(string quotationCode, string projectName)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE RFQProjects SET Status = 'CLOSED', ActualCompletionDate = GETDATE() WHERE ProjectName = @ProjectName AND QuotationCode = @QuotationCode";

                var parameters = new
                {
                    ProjectName = projectName,
                    QuotationCode = quotationCode
                };
                var result = await conn.ExecuteAsync(query, parameters);
                return result > 0;
            }
        }
    }
}