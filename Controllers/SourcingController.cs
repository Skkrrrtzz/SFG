
using Dapper;
using Microsoft.Data.SqlClient;
using SFG.Models;
using SFG.Data;
using Microsoft.AspNetCore.Mvc;
using SFG.Migrations;
using System.Text.RegularExpressions;
using System;
using System.Globalization;

namespace SFG.Controllers
{
    public class SourcingController : HomeController
    {
        public SourcingController(AppDbContext dataBase) : base(dataBase)
        {
        }
        public IActionResult SourcingRFQForm()
        {
            return View();
        }
        public async Task<IActionResult> ProcessData(string partNumber)
        {
            try
            {
                // Call the CheckMRP method to retrieve MRPBOM data
                var mrpData = await CheckMRP(partNumber);

                // Extract PartNumber from mrpData
                List<string> extractedPartNumbers = ExtractPartNumbers(mrpData);

                // Create a list to store the modified data with the Status column
                List<object> dataWithStatus = new List<object>();

                // Iterate through each row in mrpData and add the Status column
                foreach (var row in mrpData)
                {
                    // Call the Quotations method to retrieve Quotations data for the current PartNumber
                    var quotationStatus = await CheckQuotationsAndLastPurchaseInfo(row.PartNumberTable);

                    // Initialize checkQtyAndEqpa variable
                    string checkQtyAndEqpa = "";

                    // Check if the Status is "COMMON" then call the CheckQtyAndEqpa method
                    if (quotationStatus.Status == "COMMON")
                    {
                        int EQPA = Convert.ToInt32(row.sumEQPA);
                        decimal GWRLQty = Convert.ToDecimal(quotationStatus.DateAndQty[1]);
                        string LastPurchaseDate = quotationStatus.DateAndQty[0];
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
                        Status = quotationStatus.Status,
                        LastPurchaseDate = quotationStatus.DateAndQty[0],
                        GWRLQty = quotationStatus.DateAndQty[1],
                        Remarks = checkQtyAndEqpa,
                    };

                    // Add the modified row to the list
                    dataWithStatus.Add(rowDataWithStatus);
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
                string remarks = ""; // Initialize remarks variable

                // Parse the LastPurchaseDate string into a DateTime object
                DateTime purchaseDate = DateTime.ParseExact(LastPurchaseDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                // Calculate the difference in months between the current date and the last purchase date
                int monthsDifference = (DateTime.Now.Year - purchaseDate.Year) * 12 + (DateTime.Now.Month - purchaseDate.Month);

                // Check if EQPA is less than GWRLQty and LastPurchaseDate is greater than 6 months ago
                if (EQPA > GWRLQty && monthsDifference > 6)
                {
                    remarks = "FOR SOURCING";
                }

                return remarks; // Return the calculated remarks
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
                var mrpData = await GetData(PartNumber, "MRPBOM");

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

                foreach (var item in lastPurchaseData)
                {
                    string date = item.LastPurchasedDate.ToString("MM/dd/yyyy");
                    string qty = item.GWRLQty.ToString();

                    // Add date and quantity to the list
                    dateAndQty.Add(date);
                    dateAndQty.Add(qty);
                }

                // Check if either table has data
                bool existsInQuotations = quotationsData.Any();
                bool existsInLastPurchase = (lastPurchaseData != null);

                if (existsInQuotations || existsInLastPurchase)
                {
                    // PartNumber exists in either Quotations or LastPurchaseInfo table
                    string status = "COMMON";

                    // Include dateAndQty in the return
                    return new { Status = status,DateAndQty = dateAndQty, Remarks = "" };
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
                query = $"SELECT PartNumberTable, MAX(DescriptionTable) AS DescriptionTable, MAX(Rev) AS Rev, MAX(Commodity) AS Commodity, " +
            $"MAX(MPN) AS MPN, MAX(Manufacturer) AS Manufacturer, SUM(CAST(EQPA AS DECIMAL)) AS sumEQPA " +
            $"FROM {tableName} WHERE PartNumber = @partNumber GROUP BY PartNumberTable;";
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
