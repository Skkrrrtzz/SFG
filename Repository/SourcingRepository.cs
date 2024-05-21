using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using SFG.Models;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

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

        public async Task<IEnumerable<RFQModel>> GetRFQPartNumbers(string projectName, string quotationCode)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT CustomerPartNumber, Description, OrigMPN, OrigMFR, Commodity, Eqpa , UoM, Status FROM RFQ WHERE ProjectName = @ProjectName AND QuotationCode = @QuotationCode";

                    return await conn.QueryAsync<RFQModel>(query, new { ProjectName = projectName, QuotationCode = quotationCode });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing query: {ex.Message}");
                throw;
            }
        }

        public async Task<PartData> FindPartNumber(string fileName, string partNumber)
        {
            var partData = await ReadExcelFile(fileName, partNumber);
            return partData;
        }
        private async Task<PartData> ReadExcelFile(string filePath, string partNumber)
        {
            var partData = new PartData { PartNumber = partNumber };

            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    int partNumberColumn = 2;
                    int startDataColumn = 20;

                    // Read headers and keep track of non-hidden columns
                    var headers = new List<string>();
                    var headerCount = new Dictionary<string, int>();
                    var nonHiddenColumns = new List<int>();

                    for (int col = startDataColumn; col <= colCount; col++)
                    {
                        var column = worksheet.Column(col);
                        if (column.Hidden) // Skip hidden columns
                        {
                            continue;
                        }

                        string header = worksheet.Cells[13, col].Text.Trim();

                        // Skip if the header value is "1"
                        if (header == "1")
                        {
                            continue;
                        }

                        if (headerCount.ContainsKey(header))
                        {
                            headerCount[header]++;
                            header = $"{header}_{headerCount[header]}";
                        }
                        else
                        {
                            headerCount[header] = 1;
                        }

                        headers.Add(header);
                        nonHiddenColumns.Add(col);
                    }

                    // Find the row containing the partNumber
                    int partNumberRow = 0;
                    for (int row = 14; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, partNumberColumn].Text.Equals(partNumber, StringComparison.OrdinalIgnoreCase))
                        {
                            partNumberRow = row;
                            break;
                        }
                    }

                    // If partNumberRow is still 0, the partNumber was not found
                    if (partNumberRow == 0)
                    {
                        throw new Exception($"Part number '{partNumber}' not found in the Excel sheet.");
                    }

                    // Initialize the supplier details dictionary
                    var supplierDetailsDict = new Dictionary<int, SupplierCostDetail>();

                    // Get values and pair them with headers
                    for (int i = 0; i < headers.Count; i++)
                    {
                        string header = headers[i];
                        int col = nonHiddenColumns[i];
                        string cellValue = worksheet.Cells[partNumberRow, col].Text;
                        if (string.IsNullOrWhiteSpace(cellValue))
                        {
                            continue;
                        }

                        try
                        {
                            var match = Regex.Match(header.Trim(), @"^(.*?)(?:_(\d+))?$", RegexOptions.Singleline);
                            string baseHeader = match.Groups[1].Value.Trim();
                            int supplierIndex = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 1;

                            if (!supplierDetailsDict.ContainsKey(supplierIndex))
                            {
                                supplierDetailsDict[supplierIndex] = new SupplierCostDetail();
                            }

                            var supplierDetail = supplierDetailsDict[supplierIndex];

                            switch (baseHeader)
                            {
                                case string s when s.StartsWith("Unit Cost"):
                                    var qtyMatch = Regex.Match(s, @"x(\d+)");
                                    if (qtyMatch.Success)
                                    {
                                        int quantity = int.Parse(qtyMatch.Groups[1].Value);
                                        if (decimal.TryParse(cellValue, out decimal cost))
                                        {
                                            supplierDetail.UnitCosts[quantity] = cost;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Failed to parse cost '{cellValue}' for quantity '{quantity}'");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Failed to match quantity in header '{header}'");
                                    }
                                    break;
                                case "Currency":
                                    supplierDetail.Currency = cellValue;
                                    break;
                                case "Supplier":
                                    supplierDetail.Supplier = cellValue;
                                    break;
                                case "MOQ":
                                    if (int.TryParse(cellValue, out int moq))
                                    {
                                        supplierDetail.MOQ = moq;
                                    }
                                    break;
                                case "SPQ":
                                    supplierDetail.SPQ = string.IsNullOrWhiteSpace(cellValue) ? (int?)null : int.Parse(cellValue);
                                    break;
                                case "Purchasing UOM":
                                    supplierDetail.PurchasingUOM = cellValue;
                                    break;
                                case "Parts Lead Time (Weeks)":
                                    if (int.TryParse(cellValue, out int leadTime))
                                    {
                                        supplierDetail.LeadTimeWeeks = leadTime;
                                    }
                                    break;
                                case "Location":
                                    supplierDetail.Location = cellValue;
                                    break;
                                case "Quote Validity":
                                    supplierDetail.QuoteValidity = cellValue;
                                    break;
                                case "Sourcing Remarks":
                                    supplierDetail.SourcingRemarks = cellValue;
                                    break;
                                case "Tooling Cost":
                                    supplierDetail.ToolingCost = string.IsNullOrWhiteSpace(cellValue) ? (decimal?)null : decimal.Parse(cellValue);
                                    break;
                                case "Tooling Lead Time (weeks)":
                                    supplierDetail.ToolingLeadTimeWeeks = string.IsNullOrWhiteSpace(cellValue) ? (int?)null : int.Parse(cellValue);
                                    break;
                                case "Tooling Sourcing Remarks":
                                    supplierDetail.ToolingSourcingRemarks = cellValue;
                                    break;
                                case "Cost Engineer's Suggested Supplier":
                                    partData.SuggestedSupplier = cellValue;
                                    break;
                                case "Comments":
                                    partData.Comments = cellValue;
                                    break;
                                default:
                                    Console.WriteLine($"Unrecognized header '{header}'");
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing header '{header}': {ex.Message}");
                        }
                    }

                    partData.SupplierDetails = supplierDetailsDict.Values.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading Excel file: {ex.Message}");
            }

            return partData;
        }



        //private async Task<PartData> ReadExcelFile(string filePath, string partNumber)
        //{
        //    var partData = new PartData { PartNumber = partNumber };

        //    try
        //    {
        //        using (var package = new ExcelPackage(new FileInfo(filePath)))
        //        {
        //            var worksheet = package.Workbook.Worksheets[0];
        //            int rowCount = worksheet.Dimension.Rows;
        //            int colCount = worksheet.Dimension.Columns;

        //            int partNumberColumn = 2;
        //            int startDataColumn = 20;

        //            // Read headers
        //            var headers = new List<string>();
        //            var headerCount = new Dictionary<string, int>();

        //            for (int col = startDataColumn; col <= colCount; col++)
        //            {
        //                string header = worksheet.Cells[13, col].Text.Trim();

        //                // Skip if the header value is "1"
        //                if (header == "1")
        //                {
        //                    headers.Add(null); // Add a null placeholder to keep indexing consistent
        //                    continue;
        //                }

        //                if (headerCount.ContainsKey(header))
        //                {
        //                    headerCount[header]++;
        //                    header = $"{header}_{headerCount[header]}";
        //                }
        //                else
        //                {
        //                    headerCount[header] = 1;
        //                }

        //                headers.Add(header);
        //            }

        //            // Find the row containing the partNumber
        //            int partNumberRow = 0;
        //            for (int row = 14; row <= rowCount; row++)
        //            {
        //                if (worksheet.Cells[row, partNumberColumn].Text.Equals(partNumber, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    partNumberRow = row;
        //                    break;
        //                }
        //            }

        //            // If partNumberRow is still 0, the partNumber was not found
        //            if (partNumberRow == 0)
        //            {
        //                throw new Exception($"Part number '{partNumber}' not found in the Excel sheet.");
        //            }

        //            // Initialize the supplier details dictionary
        //            var supplierDetailsDict = new Dictionary<int, SupplierCostDetail>();

        //            // Get values and pair them with headers
        //            for (int col = startDataColumn; col <= colCount; col++)
        //            {
        //                string header = headers[col - startDataColumn];

        //                // Skip values corresponding to skipped headers
        //                if (header == null)
        //                {
        //                    continue;
        //                }

        //                string cellValue = worksheet.Cells[partNumberRow, col].Text;
        //                if (string.IsNullOrWhiteSpace(cellValue))
        //                {
        //                    continue;
        //                }

        //                try
        //                {
        //                    var match = Regex.Match(header.Trim(), @"^(.*?)(?:_(\d+))?$", RegexOptions.Singleline);
        //                    string baseHeader = match.Groups[1].Value.Trim();
        //                    int supplierIndex = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 1;

        //                    if (!supplierDetailsDict.ContainsKey(supplierIndex))
        //                    {
        //                        supplierDetailsDict[supplierIndex] = new SupplierCostDetail();
        //                    }

        //                    var supplierDetail = supplierDetailsDict[supplierIndex];

        //                    switch (baseHeader)
        //                    {
        //                        case string s when s.StartsWith("Unit Cost"):
        //                            var qtyMatch = Regex.Match(s, @"x(\d+)");
        //                            if (qtyMatch.Success)
        //                            {
        //                                int quantity = int.Parse(qtyMatch.Groups[1].Value);
        //                                if (decimal.TryParse(cellValue, out decimal cost))
        //                                {
        //                                    supplierDetail.UnitCosts[quantity] = cost;
        //                                }
        //                                else
        //                                {
        //                                    Console.WriteLine($"Failed to parse cost '{cellValue}' for quantity '{quantity}'");
        //                                }
        //                            }
        //                            else
        //                            {
        //                                Console.WriteLine($"Failed to match quantity in header '{header}'");
        //                            }
        //                            break;

        //                        case "Currency":
        //                            supplierDetail.Currency = cellValue;
        //                            break;

        //                        case "Supplier":
        //                            supplierDetail.Supplier = cellValue;
        //                            break;

        //                        case "MOQ":
        //                            if (int.TryParse(cellValue, out int moq))
        //                            {
        //                                supplierDetail.MOQ = moq;
        //                            }
        //                            break;

        //                        case "SPQ":
        //                            supplierDetail.SPQ = string.IsNullOrWhiteSpace(cellValue) ? (int?)null : int.Parse(cellValue);
        //                            break;

        //                        case "Purchasing UOM":
        //                            supplierDetail.PurchasingUOM = cellValue;
        //                            break;

        //                        case "Parts Lead Time (Weeks)":
        //                            if (int.TryParse(cellValue, out int leadTime))
        //                            {
        //                                supplierDetail.LeadTimeWeeks = leadTime;
        //                            }
        //                            break;

        //                        case "Location":
        //                            supplierDetail.Location = cellValue;
        //                            break;

        //                        case "Quote Validity":
        //                            supplierDetail.QuoteValidity = cellValue;
        //                            break;

        //                        case "Sourcing Remarks":
        //                            supplierDetail.SourcingRemarks = cellValue;
        //                            break;

        //                        case "Tooling Cost":
        //                            supplierDetail.ToolingCost = string.IsNullOrWhiteSpace(cellValue) ? (decimal?)null : decimal.Parse(cellValue);
        //                            break;

        //                        case "Tooling Lead Time (weeks)":
        //                            supplierDetail.ToolingLeadTimeWeeks = string.IsNullOrWhiteSpace(cellValue) ? (int?)null : int.Parse(cellValue);
        //                            break;

        //                        case "Tooling Sourcing Remarks":
        //                            supplierDetail.ToolingSourcingRemarks = cellValue;
        //                            break;

        //                        case "Cost Engineer's Suggested Supplier":
        //                            partData.SuggestedSupplier = cellValue;
        //                            break;

        //                        case "Comments":
        //                            partData.Comments = cellValue;
        //                            break;

        //                        default:
        //                            Console.WriteLine($"Unrecognized header '{header}'");
        //                            break;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"Error processing header '{header}': {ex.Message}");
        //                }
        //            }

        //            partData.SupplierDetails = supplierDetailsDict.Values.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error reading Excel file: {ex.Message}");
        //    }

        //    return partData;
        //}
    }
}