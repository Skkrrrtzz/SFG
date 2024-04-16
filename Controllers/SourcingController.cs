
using Dapper;
using Microsoft.Data.SqlClient;
using SFG.Models;
using SFG.Data;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SFG.Controllers
{
    public class SourcingController : HomeController
    {
        public SourcingController(AppDbContext dataBase) : base(dataBase)
        {
        }
        public async Task<IActionResult> SourcingRFQForm(string partNumber)
        {
            try
            {
                var viewModel = new MyViewModel
                {
                    RFQData = await RFQData(partNumber),
                    RFQProjectData = await RFQProjectData(partNumber)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception or return a generic error view
                return View("Error", ex);
            }
        }
        private async Task<List<RFQModel>> RFQData(string partNumber)
        {
            try
            {
                // Call RFQQuery to retrieve RFQ data from the specified table
                var rfqData = await RFQQuery(partNumber, "RFQ");
                

                // Map the retrieved data to RFQModel objects
                List<RFQModel> rfqMappedData = MapRFQData(rfqData);

                return rfqMappedData;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                throw new Exception("Error retrieving RFQ data", ex);
            }
        }
        private async Task<List<RFQProjectModel>> RFQProjectData(string partNumber)
        {
            try
            {
                var rfqprojData = await RFQQuery(partNumber, "RFQProjects");
                List<RFQProjectModel> rfqprojMappedData = MapRFQProjectsData(rfqprojData);
                return rfqprojMappedData;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                throw new Exception("Error retrieving RFQ data", ex);
            }
        }
        private async Task<IEnumerable<dynamic>> RFQQuery(string partNumber, string tableName)
        {
            // Check if the specified table exists
            bool tableExists = await TableExists(tableName);
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
            
            // If the table does not exist, throw an exception
            if (!tableExists)
            {
                throw new Exception($"Table '{tableName}' does not exist.");
            }

            // Execute the query
            using (SqlConnection conn = new SqlConnection(GetConnection()))
            {
                return await conn.QueryAsync(query, new { partNumber });
            }
        }
        private List<RFQModel> MapRFQData(IEnumerable<dynamic> rawData)
        {
            // Convert the raw data to RFQModel objects
            List<RFQModel> rfqList = new List<RFQModel>();

            foreach (var item in rawData)
            {
                RFQModel rfq = new RFQModel
                {
                   Id = item.Id,
                   CustomerPartNumber = item.CustomerPartNumber,
                   Rev = item.Rev,
                   Description = item.Description,
                   OrigMFR = item.OrigMFR,
                   OrigMPN = item.OrigMPN,
                   Commodity = item.Commodity,
                   Eqpa = item.Eqpa,
                   UoM = item.UoM,
                   Status = item.Status
                };
                rfqList.Add(rfq);
            }

            return rfqList;
        }
        private List<RFQProjectModel> MapRFQProjectsData(IEnumerable<dynamic> rawData)
        {
            // Convert the raw data to RFQModel objects
            List<RFQProjectModel> rfqprojList = new List<RFQProjectModel>();

            foreach (var item in rawData)
            {
                RFQProjectModel rfq = new RFQProjectModel
                {
                    Id = item.Id,
                    ProjectName = item.ProjectName,
                    Customer = item.Customer,
                    QuotationCode = item.QuotationCode,
                    NoItems = item.NoItems,
                    RequestDate = item.RequestDate,
                    RequiredDate = item.RequiredDate
                };
                rfqprojList.Add(rfq);
            }

            return rfqprojList;
        }
        [HttpGet]
        public async Task<IActionResult> FindId(int id)
        {
            try
            {
                string query = "SELECT * FROM RFQ WHERE Id = @Id";

                using (SqlConnection conn = new SqlConnection(GetConnection()))
                {
                    var result = await conn.QueryFirstOrDefaultAsync<RFQModel>(query, new { Id = id });
                    if (result != null)
                    {
                        return Json(result); // Return the data if found
                    }
                    else
                    {
                        return NotFound(); // Return 404 Not Found if no data found
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}"); // Return 500 Internal Server Error for any exceptions
            }
        }

        public async Task<IActionResult> ProcessData(string partNumber)
        {
            try
            {
                // Call the CheckMRP method to retrieve MRPBOM data
                var mrpData = await CheckMRP(partNumber);

                // Extract PartNumber from mrpData
                List<string> extractedPartNumbers = ExtractPartNumbers(mrpData);

                // Initialize a list to store data with status
                List<dynamic> dataWithStatus = new List<dynamic>();

                // Iterate through each row in mrpData and add the Status column
                foreach (var row in mrpData)
                {
                    try
                    {
                        // Call the Quotations method to retrieve Quotations data for the current PartNumber
                        var quotationStatus = await CheckQuotationsAndLastPurchaseInfo(row.PartNumberTable);

                        // Initialize variables for EQPA, GWRLQty, and LastPurchaseDate
                        int EQPA = 0;
                        decimal GWRLQty = 0;
                        string LastPurchaseDate = "No Date";
                        string checkQtyAndEqpa = "FOR SOURCING";

                        // Check if the Status is "COMMON"
                        if (quotationStatus.Status == "COMMON")
                        {
                            // Parse EQPA
                            EQPA = Convert.ToInt32(row.sumEQPA);

                            // Parse GWRLQty and LastPurchaseDate from quotationStatus.DateAndQty
                            GWRLQty = Convert.ToDecimal(quotationStatus.DateAndQty[1]);
                            LastPurchaseDate = quotationStatus.DateAndQty[0];

                            // Call the CheckQtyAndEqpa method
                             checkQtyAndEqpa = await CheckQtyAndEqpa(EQPA, GWRLQty, LastPurchaseDate);
                        }

                        // Create a new object representing the row with the Status column added
                        var rowDataWithStatus = new
                        {
                            PartNumber = row.PartNumberTable,
                            Description = row.DescriptionTable,
                            Rev = row.Rev,
                            Commodity = row.Commodity,
                            MPN = row.MPN,
                            Manufacturer = row.Manufacturer,
                            EQPA = row.sumEQPA,
                            UOM = row.UOM,
                            Status = quotationStatus.Status,
                            LastPurchaseDate,
                            GWRLQty,
                            Remarks = checkQtyAndEqpa,
                        };

                        // Add the modified row to the list
                        dataWithStatus.Add(rowDataWithStatus);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it as required
                        Console.WriteLine($"Error processing row: {ex.Message}");
                    }
                }

                // Return success response with the modified data containing the Status column
                return Json(new { success = true, data = dataWithStatus });
            }
            catch (Exception ex)
            {
                // Log the exception or return a generic error message
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
        private async Task<string> CheckQtyAndEqpa(int EQPA, decimal GWRLQty, string LastPurchaseDate)
        {
            try
            {
                string remarks = "";

                // Parse the LastPurchaseDate string into a DateTime object
                DateTime purchaseDate = DateTime.ParseExact(LastPurchaseDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                // Calculate the difference in months between the current date and the last purchase date
                int monthsDifference = (DateTime.Now.Year - purchaseDate.Year) * 12 + (DateTime.Now.Month - purchaseDate.Month);

                // Check if EQPA is less than GWRLQty and LastPurchaseDate is greater than 6 months ago
                if (EQPA > GWRLQty && monthsDifference > 6)
                {
                    remarks = "FOR SOURCING";
                }

                return remarks;
            }
            catch (Exception ex)
            {
                // Log the exception or return a generic error message
                throw new Exception($"Error: {ex.Message}");
            }
        }
        private List<string> ExtractPartNumbers(IEnumerable<dynamic> mrpData)
        {
            List<string> partNumbers = new List<string>();

            foreach (var item in mrpData)
            {
                string partNumber = item?.PartNumberTable?.ToString();
                if (!string.IsNullOrEmpty(partNumber) && !partNumbers.Contains(partNumber))
                {
                    partNumbers.Add(partNumber);
                }
            }

            return partNumbers;
        }
        private async Task<IEnumerable<dynamic>> CheckMRP(string PartNumber)
        {
            try
            {
                // Call the GetBOM method to retrieve MRPBOM data
                var mrpData = await GetData(PartNumber, "MRPBOMProducts");

                return mrpData;
            }
            catch (Exception ex)
            {
                // Log the exception or return a generic error message
                throw new Exception($"Error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<dynamic> CheckQuotationsAndLastPurchaseInfo(string PartNumber)
        {
            try
            {
                string tableName = "Quotations";

                // Call the GetData method to check if the part number exists in the Quotations table and in LastPurchaseInfo
                var quotationsData = await GetData(PartNumber, tableName);
                var lastPurchaseData = await GetLastPurchaseInfo(PartNumber);
                List<string> dateAndQty = new List<string>();

                if (lastPurchaseData != null && lastPurchaseData.Any(item => item.GWRLQty != null || item.LastPurchasedDate != null))
                {
                    foreach (var item in lastPurchaseData)
                    {
                        string date = item.LastPurchasedDate.ToString("MM/dd/yyyy");
                        string qty = (item.GWRLQty ?? 0).ToString();

                        // Add date and quantity to the list
                        dateAndQty.Add(date);
                        dateAndQty.Add(qty);
                    }
                }

                // Check if either table has data
                bool existsInQuotations = quotationsData.Any();
                bool existsInLastPurchase = dateAndQty.Count > 0;

                if (existsInQuotations || existsInLastPurchase)
                {
                    // PartNumber exists in either Quotations or LastPurchaseInfo table
                    string status = "COMMON";

                    // Include dateAndQty in the return
                    return new { Status = status, DateAndQty = dateAndQty, Remarks = "" };
                }
                else
                {
                    // PartNumber does not exist in either Quotations or LastPurchaseInfo table
                    string status = "UNIQUE";
                    string remarks = "FOR SOURCING";
                    // Include dateAndQty in the return
                    return new { Status = status, DateAndQty = dateAndQty, Remarks = remarks };
                }
            }
            catch (Exception ex)
            {
                // Log the exception or return a generic error message
                throw new Exception($"Error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RFQUpload([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                // Deserialize the sourcingData from formData
                var sourcingDataJson = formData["sourcingData"].ToString();
                var sourcingData = JsonConvert.DeserializeObject<List<RFQModel>>(sourcingDataJson);

                // Extract other relevant information from formData
                var requiredDate = DateTime.Parse(formData["requiredDate"].ToString());
                var requestDate = DateTime.Parse(formData["requestDate"].ToString());
                var quotationCode = formData["quotationCode"].ToString();
                var projectName = formData["projectName"].ToString();
                var noItems = Convert.ToInt32(formData["noItems"].ToString());
                var customer = formData["customer"].ToString();

                // Setup your database connection
                using (var connection = new SqlConnection(GetConnection()))
                {
                    await connection.OpenAsync();
                    // Begin a transaction
                    var transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert data into RFQProjects table
                        await connection.ExecuteAsync(@"INSERT INTO RFQProjects (ProjectName, Customer, QuotationCode, NoItems, RequestDate, RequiredDate) 
                                                 VALUES (@ProjectName, @Customer, @QuotationCode, @NoItems, @RequestDate, @RequiredDate)",
                                                        new { ProjectName = projectName, Customer = customer, QuotationCode = quotationCode, NoItems = noItems, RequestDate = requestDate, RequiredDate = requiredDate },
                                                        transaction);

                        // Insert data into RFQ table for each item in sourcingData
                        foreach (var item in sourcingData)
                        {
                            await connection.ExecuteAsync(@"INSERT INTO RFQ (ProjectName, Customer, QuotationCode, LastPurchaseDate, CustomerPartNumber, Description, Rev, Commodity, OrigMPN, OrigMFR, Eqpa, UoM, Status, Remarks)
                                    VALUES (@ProjectName, @Customer, @QuotationCode, @LastPurchaseDate, @CustomerPartNumber, @Description, @Rev, @Commodity, @OrigMPN, @OrigMFR, @Eqpa, @Uom, @Status, @Remarks)",
                                  new
                                  {
                                      ProjectName = projectName,
                                      Customer = customer,
                                      QuotationCode = quotationCode,
                                      item.LastPurchaseDate,
                                      item.CustomerPartNumber,
                                      item.Description,
                                      item.Rev,
                                      item.Commodity,
                                      item.OrigMPN,
                                      item.OrigMFR,
                                      item.Eqpa,
                                      item.UoM,
                                      item.Status,
                                      item.Remarks
                                  },
                                  transaction);
                        }

                        // Commit the transaction
                        transaction.Commit();

                        // Return success response
                        return Json(new { success = true, message = "RFQ data uploaded successfully!" });
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of any exception
                        transaction.Rollback();
                        throw; // Rethrow the exception to be handled at a higher level
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or return a generic error message
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
        private async Task<IEnumerable<dynamic>> GetData(string partNumber, string tableName)
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
                query = $"SELECT i.PartNumberTable AS PartNumberTable, MAX(i.DescriptionTable) AS DescriptionTable,MAX(i.Rev) AS Rev,MAX(i.UOM) AS UOM,MAX(i.Commodity) AS Commodity,MAX(i.MPN) AS MPN," +
       $"MAX(i.Manufacturer) AS Manufacturer,SUM(CAST(i.EQPA AS DECIMAL)) AS sumEQPA FROM MRPBOM i RIGHT JOIN {tableName} p ON p.PartNumber = i.PartNumber " +
       $"WHERE i.PartNumber = {partNumber} GROUP BY i.PartNumberTable;";
            }

            // Execute the query
            using (SqlConnection conn = new SqlConnection(GetConnection()))
            {
                return await conn.QueryAsync(query, new { partNumber });
            }
        }
        private async Task<IEnumerable<dynamic>> GetLastPurchaseInfo(string partNumber)
        {
            string query = $"SELECT MAX(GWRLQty) AS GWRLQty, MAX(LastPurchasedDate) AS LastPurchasedDate FROM LastPurchaseInfo WHERE ForeignName = @partNumber";

            using (SqlConnection conn = new SqlConnection(GetConnection()))
            {
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

    }
}
