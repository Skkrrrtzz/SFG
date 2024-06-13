using ATSSFG.Models;
using ATSSFG.Repository;
using ATSSFG.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Globalization;

namespace ATSSFG.Pages.Sourcing
{
    public class ViewSourcingFormModel : PageModel
    {
        #region Declarations

        private readonly Emailing _emailingService;
        private readonly UploadService _uploadService;
        private readonly ISourcingRepository _sourcingRepository;

        #endregion Declarations

        #region Bindings

        [BindProperty]
        public string? PartNumber { get; set; }

        #endregion Bindings

        #region Constructor

        public ViewSourcingFormModel(Emailing emailingService, ISourcingRepository sourcingRepository, UploadService uploadService)
        {
            _emailingService = emailingService;
            _sourcingRepository = sourcingRepository;
            _uploadService = uploadService;
        }

        #endregion Constructor

        #region Functions

        private async Task<IEnumerable<dynamic>> CheckMRP(string PartNumber)
        {
            try
            {
                // Call the GetBOM method to retrieve MRPBOM data
                var mrpData = await _sourcingRepository.GetData(PartNumber, "MRPBOMProducts");

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

        #endregion Functions

        #region Get

        public async Task<IActionResult> OnGetExcelFileAsync(string pNDesc)
        {
            // Construct the path to the Excel file based on the provided pNDesc
            string filePath = _uploadService.GetFilePathFromPNDesc(pNDesc);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Excel file not found
            }

            // Convert the Excel file to PDF
            string pdfFilePath = _uploadService.GetConvertedToPdf(filePath, 2);

            if (!System.IO.File.Exists(pdfFilePath))
            {
                return NotFound(); // PDF file not found
            }
            // Get the PDF file name
            string fileName = Path.GetFileName(pdfFilePath);

            // Open the PDF file as a stream
            var fileStream = new FileStream(pdfFilePath, FileMode.Open);

            // Return the PDF file as a FileStreamResult with the appropriate content type
            return File(fileStream, "application/pdf", fileName);
        }

        #endregion Get

        #region Post

        public async Task<IActionResult> OnPostProcessDataAsync([FromForm] string partNumber)
        {
            if (string.IsNullOrEmpty(partNumber))
            {
                return BadRequest(new { message = "PartNumber is required." });
            }
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
                return new JsonResult(new { success = true, data = dataWithStatus });
            }
            catch (Exception ex)
            {
                // Log the exception or return a generic error message
                return new JsonResult(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnPostRFQUploadAsync([FromBody] Dictionary<string, object> formData)
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
                    StdTAT = Convert.ToInt32(formData["stdTAT"].ToString()),
                    Status = "OPEN",
                    HasPrices = false
                };

                var result = await _sourcingRepository.InsertRFQ(rfqProjects, rfqData);

                return new JsonResult(new { success = true, message = "RFQ data uploaded successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        #endregion Post
    }
}