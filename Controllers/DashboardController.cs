using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using SFG.Models;
using SFG.Repository;
using SFG.Services;

namespace SFG.Controllers
{
    public class DashboardController : HomeController
    {
        private readonly UploadService _uploadService;
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(UploadService uploadService, IDashboardRepository dashboardRepository)
        {
            _uploadService = uploadService;
            _dashboardRepository = dashboardRepository;
        }

        public IActionResult Dashboard()
        {
            try
            {
                var sessionData = GetSessionData();
                dynamic accinfo = GetUsers(sessionData?.AccountName);
                ViewBag.Department = accinfo.Department;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Checking();
        }

        public IActionResult Library()
        {
            return View();
        }

        public IActionResult ProjectSummaryReport()
        {
            return View();
        }

        public IActionResult OpenProjects()
        {
            return View();
        }

        public IActionResult Closed()
        {
            return View();
        }

        public async Task<IActionResult> GetPNandDescription()
        {
            var bomData = await _dashboardRepository.GetBOM();

            var distinctPNAndDescriptions = bomData.GroupBy(bom => bom.PartNumber).Select(group => new { PartNumber = group.Key, Description = group.First().Description });

            return Json(new { data = distinctPNAndDescriptions });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRFQProjects()
        {
            try
            {
                var result = await _dashboardRepository.GetAllRFQProjects();

                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = $"Error: {ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult ViewSourcingForm()
        {
            return View();
        }

        public async Task<IActionResult> ViewRFQProjects()
        {
            var model = await GetAllRFQProjects();
            return View(model);
        }

        public async Task<IActionResult> ViewRFQForm(string quotationCode)
        {
            try
            {
                var viewModel = new MyViewModel
                {
                    RFQData = await _dashboardRepository.GetRFQByQuotationCode(quotationCode),
                    RFQProjectData = await _dashboardRepository.GetRFQProjectsByQuotationCode(quotationCode)
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving RFQ data: {ex.Message}");
                return StatusCode(500);
            }
        }

        private dynamic GetUsers(string Name)
        {
            using (SqlConnection conn = new(GetConnection()))
            {
                dynamic? acc = conn.QueryFirstOrDefault<dynamic>("SELECT * FROM Users WHERE Name = @name", new { name = Name });
                return acc == null ? null : acc;
            }
        }

        public async Task<IActionResult> IncomingRFQProjects()
        {
            try
            {
                var result = await _dashboardRepository.GetOpenRFQProjects();

                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = $"Error: {ex.Message}" });
            }
        }

        public async Task<IActionResult> GetExcelFile(string pNDesc)
        {
            // Construct the path to the Excel file based on the provided pNDesc
            string filePath = _uploadService.GetFilePathFromPNDesc(pNDesc);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Excel file not found
            }

            // Convert the Excel file to PDF
            string pdfFilePath = _uploadService.GetConvertedToPdf(filePath, 1);

            if (!System.IO.File.Exists(pdfFilePath))
            {
                return NotFound(); // PDF file not found
            }
            // Construct the URL for the PDF file
            //string pdfUrl = Url.Content("~/Uploads/PDF/" + Path.GetFileName(pdfFilePath));

            //return Ok(pdfUrl); // Return the URL of the PDF file
            // Get the PDF file name
            string fileName = Path.GetFileName(pdfFilePath);

            // Open the PDF file as a stream
            var fileStream = new FileStream(pdfFilePath, FileMode.Open);

            // Return the PDF file as a FileStreamResult with the appropriate content type
            return File(fileStream, "application/pdf", fileName);
        }

        public async Task<IActionResult> DownloadExcelFile(string projectName)
        {
            // Construct the path to the Excel file based on the provided pNDesc
            string filePath = _uploadService.GetExportedExcel(projectName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Excel file not found
            }

            // Get the PDF file name
            string fileName = Path.GetFileName(filePath);

            // Open the PDF file as a stream
            var fileStream = new FileStream(filePath, FileMode.Open);

            // Return the PDF file as a FileStreamResult with the appropriate content type
            return File(fileStream, "application/xlsx", fileName);
        }

        public async Task<IActionResult> UploadLastPurchaseInfo(IFormFile file)
        {
            try
            {
                // Check if the uploaded file is null
                if (file == null || file.Length == 0)
                {
                    return Json(new { success = false, message = "No file received" });
                }

                // Check if the file extension is valid (optional, depending on your requirements)
                string fileExtension = Path.GetExtension(file.FileName);
                if (!IsSupportedFileExtension(fileExtension))
                {
                    return Json(new { success = false, message = "Invalid file format. Please upload a valid Excel file." });
                }
                else
                {
                    string tableName = "LastPurchaseInfo";
                    await UploadingExcelFile(file, tableName);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true, message = "File uploaded successfully" });
        }

        public async Task<IActionResult> UploadQuotation(IFormFile file)
        {
            try
            {
                // Check if the uploaded file is null
                if (file == null || file.Length == 0)
                {
                    return Json(new { success = false, message = "No file received" });
                }

                string fileExtension = Path.GetExtension(file.FileName);

                if (!IsSupportedFileExtension(fileExtension))
                {
                    return Json(new { success = false, message = "Invalid file format. Please upload a valid Excel file." });
                }
                else
                {
                    string tableName = "Quotations";
                    await UploadingExcelFile(file, tableName);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true, message = "File uploaded successfully" });
        }

        public async Task<IActionResult> UploadMRPBOM(IFormFile file)
        {
            try
            {
                // Check if the uploaded file is null
                if (file == null || file.Length == 0)
                {
                    return Json(new { success = false, message = "No file received" });
                }

                string fileExtension = Path.GetExtension(file.FileName);

                if (!IsSupportedFileExtension(fileExtension))
                {
                    return Json(new { success = false, message = "Invalid file format. Please upload a valid Excel file." });
                }
                else
                {
                    string tableName = "MRPBOM";
                    await ProcessMRPQBOM(file);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true, message = "File uploaded successfully" });
        }

        private async Task<LastPurchaseInfoModel> SaveLastPurchaseInfo(ExcelWorksheet worksheet, int row)
        {
            return new LastPurchaseInfoModel
            {
                ItemNo = worksheet.Cells[row, 2].Value?.ToString(),
                ForeignName = worksheet.Cells[row, 3].Value?.ToString(),
                ItemDescription = worksheet.Cells[row, 4].Value?.ToString(),
                Unit = worksheet.Cells[row, 5].Value?.ToString(),
                GWRLQty = double.TryParse(worksheet.Cells[row, 6].Value?.ToString(), out var gwrQty) ? (decimal)gwrQty : 0,
                LastPurchasedDate = worksheet.Cells[row, 7].GetValue<DateTime>(),
                LastPurchasedUSDPrice = double.TryParse(worksheet.Cells[row, 8].Value?.ToString(), out var usdPrice) ? (decimal)usdPrice : 0,
                CustomerVendorCode = worksheet.Cells[row, 9].Value?.ToString(),
                CustomerVendorName = worksheet.Cells[row, 10].Value?.ToString(),
                RMWHEREUSED = worksheet.Cells[row, 11].Value?.ToString(),
                FGName = worksheet.Cells[row, 12].Value?.ToString()
            };
        }

        private async Task<QuotationModel> SaveQuotations(ExcelWorksheet worksheet, int row)
        {
            return new QuotationModel
            {
                PartNumber = worksheet.Cells[row, 1].Value?.ToString()
            };
        }

        private async Task<MRPBOMModel> SaveMRPBOM(ExcelWorksheet worksheet, int row)
        {
            return new MRPBOMModel
            {
                Product = GetValueOrDefault(worksheet.Cells[2, 4].Value),
                PartNumber = GetValueOrDefault(worksheet.Cells[3, 4].Value),
                Item = int.TryParse(worksheet.Cells[row, 2].Value?.ToString(), out int itemValue) ? itemValue : 0,
                Level = int.TryParse(worksheet.Cells[row, 2].Value?.ToString(), out int levelValue) ? levelValue : 0,
                PartNumberTable = worksheet.Cells[row, 3].Value?.ToString(),
                SAPPartNumber = worksheet.Cells[row, 4].Value?.ToString(),
                DescriptionTable = worksheet.Cells[row, 5].Value?.ToString(),
                Rev = worksheet.Cells[row, 6].Value?.ToString(),
                QPA = worksheet.Cells[row, 7].Value?.ToString(),
                EQPA = worksheet.Cells[row, 8].Value?.ToString(),
                UOM = worksheet.Cells[row, 9].Value?.ToString(),
                Commodity = worksheet.Cells[row, 10].Value?.ToString(),
                MPN = worksheet.Cells[row, 11].Value?.ToString(),
                Manufacturer = worksheet.Cells[row, 12].Value?.ToString()
            };
        }

        private async Task<MRPBOMProductModel> SaveMRPBOMProducts(ExcelWorksheet worksheet)
        {
            return new MRPBOMProductModel
            {
                Product = GetValueOrDefault(worksheet.Cells[2, 4].Value),
                PartNumber = GetValueOrDefault(worksheet.Cells[3, 4].Value),
                Revision = GetValueOrDefault(worksheet.Cells[4, 4].Value),
                Description = GetValueOrDefault(worksheet.Cells[5, 4].Value),
                DateModified = worksheet.Cells[2, 12].GetValue<DateTime>(),
                PreparedBy = GetValueOrDefault(worksheet.Cells[3, 12].Value),
                ReviewedBy = GetValueOrDefault(worksheet.Cells[4, 12].Value),
            };
        }

        private async Task<string> SaveUploadedFileAndReturnPath(IFormFile file, string partNumber, string description)
        {
            string filePath = await _uploadService.SaveUploadedFile(file, partNumber, description);
            return filePath;
        }

        private async Task ProcessMRPQBOM(IFormFile file)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;
                    using (var package = new ExcelPackage(stream))
                    {
                        await ProcessMRPQBOMFile(package, file);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while uploading the Excel file.", ex);
            }
        }

        private async Task ProcessMRPQBOMFile(ExcelPackage package, IFormFile file)
        {
            using (var worksheet = package.Workbook.Worksheets[1])
            {
                int rowCount = worksheet.Dimension.Rows;

                // Extract PartNumber and Description from specific cells
                string? partNumber = worksheet.Cells[3, 4].Value?.ToString();
                string? description = worksheet.Cells[5, 4].Value?.ToString();

                // Save uploaded file and get its path
                string filePath = await SaveUploadedFileAndReturnPath(file, partNumber, description);

                for (int i = 9; i <= rowCount; i++)
                {
                    bool rowHasValue = false;
                    int columnCount = worksheet.Dimension.Columns;

                    // Check if any cell in the row has a value
                    for (int j = 1; j <= columnCount; j++)
                    {
                        if (!string.IsNullOrEmpty(worksheet.Cells[i, j].Text))
                        {
                            rowHasValue = true;
                            break;
                        }
                    }

                    // If row has a value, process it
                    if (rowHasValue)
                    {
                        var model = await SaveMRPBOM(worksheet, i);

                        var affectedRows = await _dashboardRepository.UploadMRPBOM(model);
                        if (affectedRows == 0)
                        {
                            throw new Exception("Error: No rows affected after uploading.");
                        }
                    }
                }

                // Save MRPBOMProducts
                var productsModel = await SaveMRPBOMProducts(worksheet);
                var affectedRows2 = await _dashboardRepository.UploadMRPBOMProducts(productsModel);

                if (affectedRows2 == 0)
                {
                    throw new Exception("Error: No rows affected after uploading MRPBOMProducts.");
                }
            }
        }

        private async Task UploadingExcelFile(IFormFile file, string tableName)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        for (int i = 2; i <= rowCount; i++)
                        {
                            switch (tableName)
                            {
                                case "LastPurchaseInfo":
                                    {
                                        var lastPurchaseInfo = await SaveLastPurchaseInfo(worksheet, i);
                                        await _dashboardRepository.UploadLastPurchaseInfo(lastPurchaseInfo);
                                        break;
                                    }
                                case "Quotations":
                                    {
                                        var quotations = await SaveQuotations(worksheet, i);
                                        await _dashboardRepository.UploadQuotations(quotations);
                                        break;
                                    }
                                default:
                                    tableName = "Unknown";
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while uploading the Excel file.", ex.InnerException);
            }
        }

        private bool IsSupportedFileExtension(string fileExtension)
        {
            // Add more supported extensions if needed
            string[] supportedExtensions = { ".xlsx", ".xls", ".csv" };
            return supportedExtensions.Contains(fileExtension.ToLower());
        }

        private string GetValueOrDefault(object value)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}