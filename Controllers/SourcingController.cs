
using Dapper;
using Microsoft.Data.SqlClient;
using SFG.Models;
using SFG.Data;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Newtonsoft.Json;
using SFG.Services;
using System.Net.Mail;
using System.Drawing;

namespace SFG.Controllers
{
    public class SourcingController : HomeController
    {
        private readonly Emailing _emailingService;
        private readonly Exporting _exportingService;
        public SourcingController(AppDbContext dataBase, Exporting exportingService, Emailing emailingService) : base(dataBase)
        {
            _exportingService = exportingService;
            _emailingService = emailingService;
        }
        public IActionResult SourcingForm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SourcingForm(RFQModel RFQ,RFQProjectModel RFQProject)
        {
            if (ModelState.IsValid)
            {
                var RFQData = new RFQModel
                {
                    ProjectName = RFQ.ProjectName,
                    Customer = RFQ.Customer,
                    QuotationCode = RFQ.QuotationCode,
                    CustomerPartNumber = RFQ.CustomerPartNumber,
                    Rev = RFQ.Rev,
                    Description = RFQ.Description,
                    OrigMFR = RFQ.OrigMFR,
                    OrigMPN = RFQ.OrigMPN,
                    Commodity = RFQ.Commodity,
                    Eqpa = RFQ.Eqpa,
                    UoM = RFQ.UoM,
                    Status = RFQ.Status
                };

                var RFQProjectData = new RFQProjectModel
                {
                    ProjectName = RFQProject.ProjectName,
                    Customer = RFQProject.Customer,
                    QuotationCode = RFQProject.QuotationCode,
                    NoItems = RFQProject.NoItems,
                    RequestDate = RFQProject.RequestDate,
                    RequiredDate = RFQProject.RequiredDate,
                    Status = "OPEN"
                };

                _db.RFQ.Add(RFQData);
                _db.RFQProjects.Add(RFQProjectData);
                _db.SaveChanges();

                return RedirectToAction("Success");
            }
            return View();
        }
        public IActionResult Success()
        {
            // This action method can be used to display a success message after form submission
            return View();
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
                    RequiredDate = item.RequiredDate,
                    Status = "OPEN"
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
                        return Json(result);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateId(string customerPartNumber, string rev, string description, string origMFR, string origMPN, string commodity, string eqpa, string uoM, int id, string status)
        {
            try
            {
                string query = "UPDATE RFQ SET CustomerPartNumber = @CustomerPartNumber, Rev = @Rev, DescriprigMPN = @Origtion = @Description, OrigMFR = @OrigMFR, OMPN, Commodity = @Commodity, Eqpa = @Eqpa, UoM = @UoM, Status = @Status WHERE Id = @Id";

                using (SqlConnection conn = new SqlConnection(GetConnection()))
                {
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

                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<bool> AddAnnualForecast(List<int> ids, List<string> annualForecasts)
        {
            try
            {
                // Check if the number of IDs matches the number of annual forecasts
                if (ids.Count != annualForecasts.Count)
                {
                    // Return false indicating failure due to mismatched counts
                    return false;
                }

                // Prepare the SQL query
                string query = "UPDATE RFQ SET AnnualForecast = @AnnualForecast WHERE Id = @Id";

                using (SqlConnection conn = new SqlConnection(GetConnection()))
                {
                    // Iterate through each ID and annual forecast
                    for (int i = 0; i < ids.Count; i++)
                    {
                        // Execute the query for each ID and annual forecast pair
                        await conn.ExecuteAsync(query, new { Id = ids[i], AnnualForecast = annualForecasts[i] });
                    }
                }

                // Return true indicating success
                return true;
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                return false; // Return false indicating failure
            }
        }
        [HttpPost]
        public async Task<IActionResult> Project(string projectName, IFormFile image)
        {
            try
            {
                // Check if rfqData or rfqProjectData is null
                if (string.IsNullOrEmpty(projectName) || image == null || image.Length == 0)
                {
                    return BadRequest("Invalid project name or empty image");
                }

                // Save the image to a temporary file
                string tempImagePath = Path.GetTempFileName();
                using (var stream = new FileStream(tempImagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // GetRFQ method to retrieve RFQ data
                var rfqData = await GetRFQ(projectName);

                // GetRFQProject method to retrieve RFQProject data
                var rfqProjectData = await GetRFQProject(projectName);

                // Check if rfqData or rfqProjectData is null
                if (rfqData == null || rfqProjectData == null)
                {
                    // Delete the temporary file if data retrieval fails
                    if (System.IO.File.Exists(tempImagePath))
                    {
                        System.IO.File.Delete(tempImagePath);
                    }
                    // Return a NotFoundResult or appropriate status code
                    return NotFound();
                }

                // WriteToExcel method to write data to Excel
                var result = await _exportingService.WriteToExcel(rfqData, rfqProjectData, projectName, 1);

                // Send the email with the temporary image file path
                var checkEmail = await SendEmail(tempImagePath);

                // Delete the temporary file after sending email
                if (System.IO.File.Exists(tempImagePath))
                {
                    System.IO.File.Delete(tempImagePath);
                }

                // Check if the writing process was successful
                if (result == false || checkEmail == false)
                {
                    return View("Error Writing to Excel File or Sending Email");
                }

                // Return a view or other appropriate action result
                return Json(result);
            }
            catch (Exception ex)
            {
                // Return an error view with the exception details
                return View("Error", ex);
            }
        }
        private async Task<bool> SendEmail(string imagePath)
        {
            try
            {
                //string email = DepartmentEmails.SourcingEmail;
                string email = "kgajete@pimes.com.ph";
                string emailName = "Sourcing";

                // Read the image bytes from the file
                byte[] imageData = await System.IO.File.ReadAllBytesAsync(imagePath);

                // Convert the image bytes to Base64 string
                string base64Image = Convert.ToBase64String(imageData);

                // Create the image HTML tag with CID (Content-ID)
                string imageHtmlTag = $"<img src=\"data:image/png;base64,{base64Image}\" alt=\"Attached Image\">";

                // Construct the email body
                string body = $"Please process this request: <br>{imageHtmlTag}<br>" +
                              "<br> Please login to ATS Business Control Portal. <br> Thank you!<br>" +
                              "<i style=\"background-color: yellow;\">***This is an auto-generated message, please do not reply***</i>";

                string subject = "RFQ";

                // Check if _emailingService is properly initialized
                if (_emailingService == null)
                {
                    throw new InvalidOperationException("_emailingService is not initialized.");
                }

                // Send the email
                await _emailingService.SendingEmail(emailName, email, subject, body, null);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing email: {ex.Message}");
                return false;
            }
        }


        private async Task<IEnumerable<RFQModel>> GetRFQ(string projectName)
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
        private async Task<RFQProjectModel> GetRFQProject(string projectName)
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
                var status ="OPEN";
                // Setup your database connection
                using (var connection = new SqlConnection(GetConnection()))
                {
                    await connection.OpenAsync();
                    // Begin a transaction
                    var transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert data into RFQProjects table
                        await connection.ExecuteAsync(@"INSERT INTO RFQProjects (ProjectName, Customer, QuotationCode, NoItems, RequestDate, RequiredDate, Status) 
                                                 VALUES (@ProjectName, @Customer, @QuotationCode, @NoItems, @RequestDate, @RequiredDate, @Status)",
                                                        new { ProjectName = projectName, Customer = customer, QuotationCode = quotationCode, NoItems = noItems, RequestDate = requestDate, RequiredDate = requiredDate, Status = @status },
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
        public IActionResult EmailNotification(string email, string name, bool send)
        {
            string subject = "Email Confirmation";
            string body = "If you receive this email, please note that it will be used for sending emails from ATS Business Control Portal. <br> Thank you!<br><i>***This is an auto generated message, please do not reply***<i>";

            if (send)
            {
                _emailingService.SendingEmail(name, email, subject, body,null);
                // Return a JSON response indicating success
                return Json(new { success = true, message = "Email has been Sent!" });
            }
            // Return a JSON response indicating failure
            return Json(new { success = false, message = "Sending Email failed!" });
        }
        private string html()
        {
            string html = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\r\n    <title>Email</title>\r\n    <style media=\"all\" type=\"text/css\">\r\n      /* -------------------------------------\r\n    GLOBAL RESETS\r\n------------------------------------- */\r\n\r\n      body {\r\n        font-family: Helvetica, sans-serif;\r\n        -webkit-font-smoothing: antialiased;\r\n        font-size: 16px;\r\n        line-height: 1.3;\r\n        -ms-text-size-adjust: 100%;\r\n        -webkit-text-size-adjust: 100%;\r\n      }\r\n      .label-value-pair {\r\n        display: flex;\r\n      }\r\n\r\n      .label,\r\n      .value {\r\n        border: 1px solid black;\r\n        padding: 8px;\r\n      }\r\n\r\n      .label {\r\n        flex: 2;\r\n        color: blue;\r\n        font-weight: bold;\r\n        border-bottom: none;\r\n        border-right: none;\r\n      }\r\n\r\n      .value {\r\n        flex: 2;\r\n        font-weight: bold;\r\n        border-bottom: none;\r\n        border-left: none;\r\n      }\r\n      .label-value-pair:last-child .label,\r\n      .label-value-pair:last-child .value {\r\n        border-bottom: 1px solid black; /* Add bottom border to the last label-value pair */\r\n        margin-bottom: 8px;\r\n      }\r\n      /* -------------------------------------\r\n    BODY & CONTAINER\r\n------------------------------------- */\r\n\r\n      body {\r\n        background-color: #f4f5f6;\r\n        margin: 0;\r\n        padding: 0;\r\n      }\r\n\r\n      .body {\r\n        background-color: #f4f5f6;\r\n        width: 100%;\r\n      }\r\n\r\n      .container {\r\n        margin: 0 auto !important;\r\n        max-width: 600px;\r\n        padding: 0;\r\n        padding-top: 24px;\r\n        padding-bottom: 24px;\r\n        width: 600px;\r\n      }\r\n\r\n      .content {\r\n        box-sizing: border-box;\r\n        display: block;\r\n        margin: 0 auto;\r\n        max-width: 600px;\r\n        padding: 0;\r\n      }\r\n      /* -------------------------------------\r\n    HEADER, FOOTER, MAIN\r\n------------------------------------- */\r\n\r\n      .main {\r\n        background: #ffffff;\r\n        border: 1px solid #eaebed;\r\n        border-radius: 16px;\r\n        width: 100%;\r\n      }\r\n\r\n      .wrapper {\r\n        box-sizing: border-box;\r\n        padding: 24px;\r\n      }\r\n\r\n      .footer {\r\n        clear: both;\r\n        padding-top: 24px;\r\n        text-align: center;\r\n        width: 100%;\r\n      }\r\n\r\n      .footer td,\r\n      .footer p,\r\n      .footer span,\r\n      .footer a {\r\n        color: #9a9ea6;\r\n        font-size: 16px;\r\n        text-align: center;\r\n      }\r\n      /* -------------------------------------\r\n    TYPOGRAPHY\r\n------------------------------------- */\r\n\r\n      p {\r\n        font-family: Helvetica, sans-serif;\r\n        font-size: 16px;\r\n        font-weight: normal;\r\n        margin: 0;\r\n        margin-bottom: 16px;\r\n      }\r\n\r\n      a {\r\n        color: #0867ec;\r\n        text-decoration: underline;\r\n      }\r\n      /* -------------------------------------\r\n    BUTTONS\r\n------------------------------------- */\r\n\r\n      .btn {\r\n        box-sizing: border-box;\r\n        min-width: 100% !important;\r\n        width: 100%;\r\n      }\r\n\r\n      .btn > tbody > tr > td {\r\n        padding-bottom: 16px;\r\n      }\r\n\r\n      .btn table {\r\n        width: auto;\r\n      }\r\n\r\n      .btn table td {\r\n        background-color: #ffffff;\r\n        border-radius: 4px;\r\n        text-align: center;\r\n      }\r\n\r\n      .btn a {\r\n        background-color: #ffffff;\r\n        border: solid 2px #0867ec;\r\n        border-radius: 4px;\r\n        box-sizing: border-box;\r\n        color: #0867ec;\r\n        cursor: pointer;\r\n        display: inline-block;\r\n        font-size: 16px;\r\n        font-weight: bold;\r\n        margin: 0;\r\n        padding: 12px 24px;\r\n        text-decoration: none;\r\n        text-transform: capitalize;\r\n      }\r\n\r\n      .btn-primary table td {\r\n        background-color: #0867ec;\r\n      }\r\n\r\n      .btn-primary a {\r\n        background-color: #0867ec;\r\n        border-color: #0867ec;\r\n        color: #ffffff;\r\n      }\r\n\r\n      @media all {\r\n        .btn-primary table td:hover {\r\n          background-color: #f7f4f5 !important;\r\n          color: black;\r\n        }\r\n        .btn-primary a:hover {\r\n          background-color: #f7f4f5 !important;\r\n          border-color: #f7f4f5 !important;\r\n          color: black;\r\n        }\r\n      }\r\n\r\n      /* -------------------------------------\r\n    OTHER STYLES THAT MIGHT BE USEFUL\r\n------------------------------------- */\r\n\r\n      .last {\r\n        margin-bottom: 0;\r\n      }\r\n\r\n      .first {\r\n        margin-top: 0;\r\n      }\r\n\r\n      .align-center {\r\n        text-align: center;\r\n      }\r\n\r\n      .align-right {\r\n        text-align: right;\r\n      }\r\n\r\n      .align-left {\r\n        text-align: left;\r\n      }\r\n\r\n      .text-link {\r\n        color: #0867ec !important;\r\n        text-decoration: underline !important;\r\n      }\r\n\r\n      .clear {\r\n        clear: both;\r\n      }\r\n\r\n      .mt0 {\r\n        margin-top: 0;\r\n      }\r\n\r\n      .mb0 {\r\n        margin-bottom: 0;\r\n      }\r\n\r\n      .preheader {\r\n        color: transparent;\r\n        display: none;\r\n        height: 0;\r\n        max-height: 0;\r\n        max-width: 0;\r\n        opacity: 0;\r\n        overflow: hidden;\r\n        mso-hide: all;\r\n        visibility: hidden;\r\n        width: 0;\r\n      }\r\n\r\n      .powered-by a {\r\n        text-decoration: none;\r\n      }\r\n\r\n      /* -------------------------------------\r\n    RESPONSIVE AND MOBILE FRIENDLY STYLES\r\n------------------------------------- */\r\n\r\n      @media only screen and (max-width: 640px) {\r\n        .main p,\r\n        .main td,\r\n        .main span {\r\n          font-size: 16px !important;\r\n        }\r\n        .wrapper {\r\n          padding: 8px !important;\r\n        }\r\n        .content {\r\n          padding: 0 !important;\r\n        }\r\n        .container {\r\n          padding: 0 !important;\r\n          padding-top: 8px !important;\r\n          width: 100% !important;\r\n        }\r\n        .main {\r\n          border-left-width: 0 !important;\r\n          border-radius: 0 !important;\r\n          border-right-width: 0 !important;\r\n        }\r\n        .btn table {\r\n          max-width: 100% !important;\r\n          width: 100% !important;\r\n        }\r\n        .btn a {\r\n          font-size: 16px !important;\r\n          max-width: 100% !important;\r\n          width: 100% !important;\r\n        }\r\n      }\r\n      /* -------------------------------------\r\n    PRESERVE THESE STYLES IN THE HEAD\r\n------------------------------------- */\r\n\r\n      @media all {\r\n        .ExternalClass {\r\n          width: 100%;\r\n        }\r\n        .ExternalClass,\r\n        .ExternalClass p,\r\n        .ExternalClass span,\r\n        .ExternalClass font,\r\n        .ExternalClass td,\r\n        .ExternalClass div {\r\n          line-height: 100%;\r\n        }\r\n        .apple-link a {\r\n          color: inherit !important;\r\n          font-family: inherit !important;\r\n          font-size: inherit !important;\r\n          font-weight: inherit !important;\r\n          line-height: inherit !important;\r\n          text-decoration: none !important;\r\n        }\r\n        #MessageViewBody a {\r\n          color: inherit;\r\n          text-decoration: none;\r\n          font-size: inherit;\r\n          font-family: inherit;\r\n          font-weight: inherit;\r\n          line-height: inherit;\r\n        }\r\n      }\r\n    </style>\r\n  </head>\r\n  <body>\r\n    <table\r\n      role=\"presentation\"\r\n      border=\"0\"\r\n      cellpadding=\"0\"\r\n      cellspacing=\"0\"\r\n      class=\"body\">\r\n      <tr>\r\n        <td>&nbsp;</td>\r\n        <td class=\"container\">\r\n          <div class=\"content\">\r\n            <!-- START CENTERED WHITE CONTAINER -->\r\n            <table\r\n              role=\"presentation\"\r\n              border=\"0\"\r\n              cellpadding=\"0\"\r\n              cellspacing=\"0\"\r\n              class=\"main\">\r\n              <!-- START MAIN CONTENT AREA -->\r\n              <tr>\r\n                <td class=\"wrapper\">\r\n                  <p>Hi there,</p>\r\n                  <p>Please process this request</p>\r\n                  <div class=\"info\">\r\n                    <div class=\"label-value-pair\">\r\n                      <div class=\"label\">Project Name</div>\r\n                      <div class=\"value\">Value 1</div>\r\n                    </div>\r\n                    <div class=\"label-value-pair\">\r\n                      <div class=\"label\">Customer</div>\r\n                      <div class=\"value\">Value 2</div>\r\n                    </div>\r\n                    <div class=\"label-value-pair\">\r\n                      <div class=\"label\">\r\n                        Quotation Code/Purchase Request Code\r\n                      </div>\r\n                      <div class=\"value\">Value 3</div>\r\n                    </div>\r\n                    <div class=\"label-value-pair\">\r\n                      <div class=\"label\">Number of Items</div>\r\n                      <div class=\"value\">Value 1</div>\r\n                    </div>\r\n                    <div class=\"label-value-pair\">\r\n                      <div class=\"label\">Request Date</div>\r\n                      <div class=\"value\">Value 2</div>\r\n                    </div>\r\n                    <div class=\"label-value-pair\">\r\n                      <div class=\"label\">Required Completion Date</div>\r\n                      <div class=\"value\">Value 3</div>\r\n                    </div>\r\n                  </div>\r\n                  <table\r\n                    role=\"presentation\"\r\n                    border=\"0\"\r\n                    cellpadding=\"0\"\r\n                    cellspacing=\"0\"\r\n                    class=\"btn btn-primary\">\r\n                    <tbody>\r\n                      <tr>\r\n                        <td align=\"left\">\r\n                          <table\r\n                            role=\"presentation\"\r\n                            border=\"0\"\r\n                            cellpadding=\"0\"\r\n                            cellspacing=\"0\">\r\n                            <tbody>\r\n                              <tr>\r\n                                <td>\r\n                                  <a\r\n                                    href=\"http://192.168.5.73:83/\"\r\n                                    target=\"_blank\"\r\n                                    >Open in your browser</a\r\n                                  >\r\n                                </td>\r\n                              </tr>\r\n                            </tbody>\r\n                          </table>\r\n                        </td>\r\n                      </tr>\r\n                    </tbody>\r\n                  </table>\r\n                </td>\r\n              </tr>\r\n\r\n              <!-- END MAIN CONTENT AREA -->\r\n            </table>\r\n\r\n            <!-- END CENTERED WHITE CONTAINER -->\r\n          </div>\r\n        </td>\r\n        <td>&nbsp;</td>\r\n      </tr>\r\n    </table>\r\n  </body>\r\n</html>\r\n";
            return html;
        }
    }
}
