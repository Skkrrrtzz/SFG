using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using SFG.Data;
using SFG.Models;
using SFG.Repository;
using SFG.Services;
using System.Globalization;

namespace SFG.Controllers
{
    public class SourcingController : HomeController
    {
        private readonly Emailing _emailingService;
        private readonly ISourcingRepository _sourcingRepository;
        private readonly UploadService _uploadService;

        public SourcingController(AppDbContext dataBase, Emailing emailingService, ISourcingRepository sourcingRepository, UploadService uploadService) : base(dataBase)
        {
            _emailingService = emailingService;
            _sourcingRepository = sourcingRepository;
            _uploadService = uploadService;
        }

        public IActionResult SourcingForm()
        {
            return View();
        }
        public IActionResult SourcingRFQPrices()
        {
            return View();
        }
        public IActionResult Success()
        {
            // This action method can be used to display a success message after form submission
            return View();
        }

        [HttpPost]
        public IActionResult SourcingForm(RFQModel RFQ, RFQProjectModel RFQProject)
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

        [HttpGet]
        public async Task<IActionResult> FindId(int id)
        {
            try
            {
                var result = await _sourcingRepository.FindById(id);

                if (result != null)
                {
                    return Json(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateId([FromBody] RFQModel formData)
        {
            try
            {
                var result = await _sourcingRepository.UpdateById(formData);

                return Json(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAnnualForecast([FromBody] AddAnnualForecastRequest request)
        {
            try
            {
                if (request.Ids.Count != request.AnnualForecasts.Count)
                {
                    return Json(new { success = false, message = "Error: ids and annualForecasts must have the same length" });
                }

                var result = await _sourcingRepository.InsertAnnualForecast(request);

                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding annual forecasts: {ex.Message}");
                return Json(new { success = false, message = $"Error adding annual forecasts: {ex.Message}" });
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
                var rfqData = await _sourcingRepository.GetRFQ(projectName);

                // GetRFQProject method to retrieve RFQProject data
                var rfqProjectData = await _sourcingRepository.GetRFQProject(projectName);

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
                var result = await _uploadService.WriteToExcel(rfqData, rfqProjectData, projectName);

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
                    return Json(new { success = false, message = $"Error Writing to Excel File or Sending Email" });
                }

                // Return a view or other appropriate action result
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRFQPartNumbers(string projectName, string quotationCode)
        {
            try
            {
                var result = await _sourcingRepository.GetRFQPartNumbers(projectName, quotationCode);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
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
                var rfqData = await _sourcingRepository.RFQQuery(partNumber, "RFQ");

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
                var rfqprojData = await _sourcingRepository.RFQQuery(partNumber, "RFQProjects");
                List<RFQProjectModel> rfqprojMappedData = MapRFQProjectsData(rfqprojData);
                return rfqprojMappedData;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                throw new Exception("Error retrieving RFQ data", ex);
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
                              "<br> Please log in to <a href='http://192.168.5.73:83/'>ATS Business Control Portal</a> Thank you!<br>" +
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

        [HttpPost]
        public async Task<IActionResult> RFQUpload([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                var sourcingDataJson = formData["sourcingData"].ToString();
                var rfqData = JsonConvert.DeserializeObject<List<RFQModel>>(sourcingDataJson);

                var rfqProjects = new RFQProjectModel
                {
                    ProjectName = formData["projectName"].ToString(),
                    Customer = formData["customer"].ToString(),
                    QuotationCode = formData["quotationCode"].ToString(),
                    NoItems = Convert.ToInt32(formData["noItems"].ToString()),
                    RequestDate = DateTime.Parse(formData["requestDate"].ToString()),
                    RequiredDate = DateTime.Parse(formData["requiredDate"].ToString()),
                    Status = "OPEN"
                };

                var result = await _sourcingRepository.InsertRFQ(rfqProjects, rfqData);

                return Json(new { success = true, message = "RFQ data uploaded successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        //EMAIL
        public IActionResult EmailNotification(string email, string name, bool send)
        {
            string subject = "Email Confirmation";
            string body = "If you receive this email, please note that it will be used for sending emails from ATS Business Control Portal. <br> Thank you!<br><i>***This is an auto generated message, please do not reply***<i>";

            if (send)
            {
                _emailingService.SendingEmail(name, email, subject, body, null);
                // Return a JSON response indicating success
                return Json(new { success = true, message = "Email has been Sent!" });
            }
            // Return a JSON response indicating failure
            return Json(new { success = false, message = "Sending Email failed!" });
        }

        /*MRP BOM TABLE*/

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
                int rowNum = 1;
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
                        string remarks = "";

                        if (quotationStatus.Status == "COMMON" && quotationStatus.DateAndQty != null && quotationStatus.DateAndQty.Count >= 2)
                        {
                            // Parse EQPA
                            EQPA = Convert.ToInt32(row.sumEQPA);

                            // Parse GWRLQty and LastPurchaseDate from quotationStatus.DateAndQty
                            GWRLQty = Convert.ToDecimal(quotationStatus.DateAndQty[1]);
                            LastPurchaseDate = quotationStatus.DateAndQty[0];

                            // Parse the LastPurchaseDate string into a DateTime object
                            DateTime purchaseDate;
                            if (DateTime.TryParseExact(LastPurchaseDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out purchaseDate))
                            {
                                // Calculate the difference in months between the current date and the last purchase date
                                int monthsDifference = (DateTime.Now.Year - purchaseDate.Year) * 12 + (DateTime.Now.Month - purchaseDate.Month);

                                // Check if EQPA is less than GWRLQty and LastPurchaseDate is greater than 6 months ago
                                if (EQPA > GWRLQty && monthsDifference > 6)
                                {
                                    remarks = "FOR SOURCING";
                                }
                            }
                        }
                        else
                        {
                            remarks = "FOR SOURCING";
                        }

                        // Create a new object representing the row with the Status column added
                        var rowDataWithStatus = new
                        {
                            No = rowNum++,
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
                            Remarks = remarks,
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

        private async Task<IEnumerable<dynamic>> CheckMRP(string PartNumber)
        {
            try
            {
                // Call the GetBOM method to retrieve MRPBOM data
                var mrpData = await _sourcingRepository.GetData(PartNumber, "MRPBOMProducts");
                //var mrpData = await _sourcingRepository.GetData(PartNumber, "MRPBOMProducts");

                return mrpData;
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

        [HttpPost]
        public async Task<dynamic> CheckQuotationsAndLastPurchaseInfo(string PartNumber)
        {
            try
            {
                string tableName = "Quotations";

                // Call the GetData method to check if the part number exists in the Quotations table and in LastPurchaseInfo
                var quotationsData = await _sourcingRepository.GetData(PartNumber, tableName);
                var lastPurchaseData = await _sourcingRepository.GetLastPurchaseInfo(PartNumber);
                List<string> dateAndQty = new List<string>();

                if (lastPurchaseData != null && lastPurchaseData.Any(item => item.GWRLQty != null || item.LastPurchasedDate != null))
                {
                    foreach (var item in lastPurchaseData)
                    {
                        string date = item.LastPurchasedDate != null ? item.LastPurchasedDate.Date.ToString("MM/dd/yyyy") : "N/A";
                        string qty = item.GWRLQty != null ? item.GWRLQty.ToString() : "N/A";

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
                    return new { Status = status, DateAndQty = dateAndQty, Remarks = " " };
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

        /* Checking Excel file for sourcing */

        public async Task<IActionResult> CheckingUploadedFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                // Check if the file is an Excel file
                if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("Only Excel files are allowed.");
                }

                using (var stream = new MemoryStream())
                {
                    // Copy the file contents to a memory stream
                    await file.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var workbook = package.Workbook;
                        if (workbook != null)
                        {
                            var worksheet = workbook.Worksheets[0];

                            string targetValue = "Purchasing UOM";

                            string firstBestPrice = worksheet.Cells["T12"].Value?.ToString();
                            string secondBestPrice = worksheet.Cells["AN12"].Value?.ToString();
                            string thirdBestPrice = worksheet.Cells["BF12"].Value?.ToString();

                            int rowCount = worksheet.Dimension.Rows;
                            int colOIndex = worksheet.Cells["O14"].Start.Column; // Cell BOM UOM
                            //int colAEIndex = 0;

                            // Find the column indices of the targetValue ("Purchasing UOM")
                            var colIndices = new List<int>();
                            for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                            {
                                var cellValue = worksheet.Cells[13, col].Value?.ToString(); // Row 13
                                if (cellValue == targetValue)
                                {
                                    colIndices.Add(col);
                                }
                            }

                            // Check if any "Purchasing UOM" column was found
                            if (colIndices.Count == 0)
                            {
                                return Json(new { success = false, message = $"Column for '{targetValue}' not found." });
                            }

                            foreach (var colAEIndex in colIndices)
                            {
                                // Iterate over each "Purchasing UOM" column
                                for (int row = 14; row <= rowCount; row++) // Start @ row 14
                                {
                                    // Retrieve values from specific cells
                                    var cellO = worksheet.Cells[row, colOIndex].Value?.ToString(); // Value of Cell BOM UOM
                                    var cellAE = worksheet.Cells[row, colAEIndex].Value?.ToString(); // Value of Cell Purchasing UOM
                                    var cellAD = worksheet.Cells[row, colAEIndex - 1].Value?.ToString(); // Value of Cell SPQ
                                    var cellAF = worksheet.Cells[row, colAEIndex + 1].Value?.ToString(); // Value of Cell Parts Lead Time
                                    var cellAddress = worksheet.Cells[row, colAEIndex].Address; // Cell Address Purchasing UOM
                                    var cellAddress2 = worksheet.Cells[row, colOIndex].Address; // Cell Address BOM UOM
                                    // Additional checks for Purchasing UOM value
                                    if (cellAE == "ft" || cellAE == "m" || cellAE == "mm" || cellAE == "inch" || cellAE == "pack")
                                    {
                                        // Check if cell SPQ is blank
                                        if (string.IsNullOrWhiteSpace(cellAD))
                                        {
                                            return Json(new { success = false, message = $"Value of cell AD{row} is blank." });
                                        }
                                    }

                                    // Check if cell Parts Lead Time contains letters
                                    if (cellAF != null && cellAF.Any(char.IsLetter))
                                    {
                                        return Json(new { success = false, message = $"Value of cell AF{row} contains letters." });
                                    }

                                    // Compare the values of cell BOM UOM and Purchasing UOM
                                    if (cellO != cellAE)
                                    {
                                        // Values are different, return the result as JSON
                                        return Json(new { success = false, message = $"Value of Purchasing UOM in cell {cellAddress} ('{cellAE}') is different from BOM UOM in cell {cellAddress2} ('{cellO}')." });
                                    }
                                }
                            }

                            // If all checks are successful, return an Ok response
                            return Ok(new { success = true, message = "All values are valid." });
                        }
                        else
                        {
                            return Json(new { success = false, message = "No worksheet found in the Excel file." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error checking uploaded file: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the file.");
            }
        }

        //public async Task<IActionResult> UploadRFQExcelFile(IFormFile file, string fileName)
        public async Task<IActionResult> UploadRFQExcelFile(IFormFile file, string fileName)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                // Check if the file is an Excel file
                if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("Only Excel files are allowed.");
                }

                string filePath = await _uploadService.SaveRFQFile(file, fileName);

                if (filePath is null)
                {
                    return Json(new { success = false, message = "Error saving the file." });
                }

                return Ok(new { success = true, message = "File uploaded successfully!" });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error checking uploaded file: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the file.");
            }
        }
    }
}