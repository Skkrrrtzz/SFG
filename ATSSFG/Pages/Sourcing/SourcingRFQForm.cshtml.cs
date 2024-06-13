using ATSSFG.Models;
using ATSSFG.Repository;
using ATSSFG.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;

namespace ATSSFG.Pages.Sourcing
{
    public class SourcingRFQFormModel : PageModel
    {
        #region Declarations

        private readonly ISourcingRepository _sourcingRepository;
        private readonly UploadService _uploadService;
        private readonly Emailing _emailingService;

        #endregion Declarations

        #region Bindings

        public MyViewModel? ViewModel { get; set; }

        #endregion Bindings

        #region Constructor

        public SourcingRFQFormModel(ISourcingRepository sourcingRepository, UploadService uploadService, Emailing emailingService)
        {
            _sourcingRepository = sourcingRepository;
            _uploadService = uploadService;
            _emailingService = emailingService;
        }

        #endregion Constructor

        #region Functions

        private async Task<List<RFQModel>> RFQData(string partNumber)
        {
            try
            {
                var rfqData = await _sourcingRepository.RFQQuery(partNumber, "RFQ");

                List<RFQModel> rfqMappedData = MapRFQData(rfqData);

                return rfqMappedData;
            }
            catch (Exception ex)
            {
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
                    AnnualForecast = item.AnnualForecast,
                    UoM = item.UoM,
                    Status = item.Status
                };
                rfqList.Add(rfq);
            }

            return rfqList;
        }

        private async Task<RFQProjectModel> RFQProjectData(string partNumber)
        {
            try
            {
                var rfqprojData = await _sourcingRepository.RFQQuery(partNumber, "RFQProjects");

                var firstRfqProjData = rfqprojData.FirstOrDefault();
                if (firstRfqProjData == null)
                {
                    throw new Exception("No RFQ project data found");
                }

                RFQProjectModel rfqprojMappedData = MapRFQProjectsData(firstRfqProjData);
                return rfqprojMappedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving RFQ data", ex);
            }
        }

        private RFQProjectModel MapRFQProjectsData(dynamic rawData)
        {
            RFQProjectModel rfq = new RFQProjectModel
            {
                Id = rawData.Id,
                ProjectName = rawData.ProjectName,
                Customer = rawData.Customer,
                QuotationCode = rawData.QuotationCode,
                NoItems = rawData.NoItems,
                RequestDate = rawData.RequestDate,
                RequiredDate = rawData.RequiredDate,
                Status = rawData.Status
            };

            return rfq;
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

        #endregion Functions

        #region Get

        public async Task<IActionResult> OnGetAsync(string partNumber)
        {
            if (string.IsNullOrEmpty(partNumber))
            {
                return BadRequest("Part number is required.");
            }

            try
            {
                ViewModel = new MyViewModel
                {
                    RFQData = await RFQData(partNumber),
                    RFQProjectData = await RFQProjectData(partNumber)
                };

                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToPage("Error", new { message = ex.Message });
            }
        }

        public async Task<IActionResult> OnGetFindIdAsync(int id)
        {
            try
            {
                var result = await _sourcingRepository.FindById(id);

                if (result != null)
                {
                    return new JsonResult(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        #endregion Get

        #region Post

        public async Task<IActionResult> OnPostUpdateIdAsync([FromBody] RFQModel formData)
        {
            try
            {
                var result = await _sourcingRepository.UpdateById(formData);

                return new JsonResult(new { success = true, message = "Data updated successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnPostAddAnnualForecastAsync([FromBody] AddAnnualForecastRequest request)
        {
            try
            {
                if (request.Ids.Count != request.AnnualForecasts.Count)
                {
                    return new JsonResult(new { success = false, message = "Error: ids and annualForecasts must have the same length" });
                }

                var result = await _sourcingRepository.InsertAnnualForecast(request);

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error adding annual forecasts: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnPostProjectAsync(string projectName, IFormFile image)
        {
            try
            {
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

                var rfqData = await _sourcingRepository.GetRFQ(projectName);

                var rfqProjectData = await _sourcingRepository.GetRFQProject(projectName);

                if (rfqData == null || rfqProjectData == null)
                {
                    // Delete the temporary file if data retrieval fails
                    if (System.IO.File.Exists(tempImagePath))
                    {
                        System.IO.File.Delete(tempImagePath);
                    }
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

                if (result == false || checkEmail == false)
                {
                    return new JsonResult(new { success = false, message = $"Error Writing to Excel File or Sending Email" });
                }

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnPostUploadRFQExcelFileAsync(IFormFile file, string fileName)
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

                bool hasPrices = await _sourcingRepository.HasPrices(fileName);

                if (filePath is null)
                {
                    return new JsonResult(new { success = false, message = "Error saving the file." });
                }

                return new JsonResult(new { success = true, message = "File uploaded successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnPostCheckingUploadedFileAsync(IFormFile file)
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
                                return new JsonResult(new { success = false, message = $"Column for '{targetValue}' not found." });
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
                                            return new JsonResult(new { success = false, message = $"Value of cell AD{row} is blank." });
                                        }
                                    }

                                    // Check if cell Parts Lead Time contains letters
                                    if (cellAF != null && cellAF.Any(char.IsLetter))
                                    {
                                        return new JsonResult(new { success = false, message = $"Value of cell AF{row} contains letters." });
                                    }

                                    // Compare the values of cell BOM UOM and Purchasing UOM
                                    if (cellO != cellAE)
                                    {
                                        // Values are different, return the result as JSON
                                        return new JsonResult(new { success = false, message = $"Value of Purchasing UOM in cell {cellAddress} ('{cellAE}') is different from BOM UOM in cell {cellAddress2} ('{cellO}')." });
                                    }
                                }
                            }

                            // If all checks are successful, return an Ok response
                            return new JsonResult(new { success = true, message = "All values are valid." });
                        }
                        else
                        {
                            return new JsonResult(new { success = false, message = "No worksheet found in the Excel file." });
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

        #endregion Post
    }
}