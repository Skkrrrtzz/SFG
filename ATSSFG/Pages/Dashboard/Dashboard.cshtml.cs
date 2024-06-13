using ATSSFG.Models;
using ATSSFG.Repository;
using ATSSFG.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;

namespace ATSSFG.Pages.Dashboard
{
    public class DashboardModel : PageModel
    {
        #region Declaration

        private readonly UploadService _uploadService;
        private readonly IDashboardRepository _dashboardRepository;

        #endregion Declaration
        #region Property
        #endregion Property
        #region Constructor

        public DashboardModel(UploadService uploadService, IDashboardRepository dashboardRepository)
        {
            _uploadService = uploadService;
            _dashboardRepository = dashboardRepository;
        }

        #endregion Constructor

        #region Function

        private LastPurchaseInfoModel SaveLastPurchaseInfo(ExcelWorksheet worksheet, int row)
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

        private QuotationModel SaveQuotations(ExcelWorksheet worksheet, int row)
        {
            return new QuotationModel
            {
                PartNumber = worksheet.Cells[row, 1].Value?.ToString()
            };
        }

        private MRPBOMModel SaveMRPBOM(ExcelWorksheet worksheet, int row)
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

        private MRPBOMProductModel SaveMRPBOMProducts(ExcelWorksheet worksheet)
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
                        var model = SaveMRPBOM(worksheet, i);

                        var affectedRows = await _dashboardRepository.UploadMRPBOM(model);
                        if (affectedRows == 0)
                        {
                            throw new Exception("Error: No rows affected after uploading.");
                        }
                    }
                }

                // Save MRPBOMProducts
                var productsModel = SaveMRPBOMProducts(worksheet);
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

                        List<LastPurchaseInfoModel> lastPurchaseInfos = new List<LastPurchaseInfoModel>();
                        List<QuotationModel> quotations = new List<QuotationModel>();

                        for (int i = 2; i <= rowCount; i++)
                        {
                            switch (tableName)
                            {
                                case "LastPurchaseInfo":
                                    lastPurchaseInfos.Add(SaveLastPurchaseInfo(worksheet, i));
                                    break;

                                case "Quotations":
                                    quotations.Add(SaveQuotations(worksheet, i));
                                    break;

                                default:
                                    tableName = "Unknown";
                                    break;
                            }
                        }
                        // Bulk insert
                        switch (tableName)
                        {
                            case "LastPurchaseInfo":
                                await _dashboardRepository.BulkInsertLastPurchaseInfo(lastPurchaseInfos);
                                break;

                            case "Quotations":
                                await _dashboardRepository.UploadQuotations(quotations);
                                break;
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

        #endregion Function

        #region Get

        public async Task<IActionResult> OnGetIncomingRFQProjectsAsync()
        {
            try
            {
                var result = await _dashboardRepository.GetIncomingRFQProjects();
                return new JsonResult(new { data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = $"Error: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnGetAllRFQProjectsAsync()
        {
            try
            {
                var result = await _dashboardRepository.GetAllRFQProjects();

                return new JsonResult(new { data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = $"Error: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnGetPNandDescriptionAsync()
        {
            var bomData = await _dashboardRepository.GetMRPBOMProducts();

            var distinctPNAndDescriptions = bomData.GroupBy(bom => bom.PartNumber).Select(group => new { PartNumber = group.Key, Description = group.First().Description });

            return new JsonResult(new { data = distinctPNAndDescriptions });
        }

        public async Task<IActionResult> OnGetDownloadExcelFileAsync(string projectName)
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

        #endregion Get

        #region Post

        public async Task<IActionResult> OnPostUploadLastPurchaseInfoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new JsonResult(new { success = false, message = "No file received" });
            }

            string fileExtension = Path.GetExtension(file.FileName);
            if (!IsSupportedFileExtension(fileExtension))
            {
                return new JsonResult(new { success = false, message = "Invalid file format. Please upload a valid Excel file." });
            }

            try
            {
                string tableName = "LastPurchaseInfo";
                await UploadingExcelFile(file, tableName);
                return new JsonResult(new { success = true, message = "File uploaded successfully" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "An error occurred during file upload", error = ex.Message });
            }
        }

        public async Task<IActionResult> OnPostUploadQuotationsAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new JsonResult(new { success = false, message = "No file received" });
            }

            string fileExtension = Path.GetExtension(file.FileName);
            if (!IsSupportedFileExtension(fileExtension))
            {
                return new JsonResult(new { success = false, message = "Invalid file format. Please upload a valid Excel file." });
            }

            try
            {
                string tableName = "Quotations";
                await UploadingExcelFile(file, tableName);
                return new JsonResult(new { success = true, message = "File uploaded successfully" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "An error occurred during file upload", error = ex.Message });
            }
        }

        public async Task<IActionResult> OnPostUploadMRPBOMAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new JsonResult(new { success = false, message = "No file received" });
            }

            string fileExtension = Path.GetExtension(file.FileName);

            if (!IsSupportedFileExtension(fileExtension))
            {
                return new JsonResult(new { success = false, message = "Invalid file format. Please upload a valid Excel file." });
            }
            try
            {
                string tableName = "MRPBOM";
                await ProcessMRPQBOM(file);
                return new JsonResult(new { success = true, message = "File uploaded successfully" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> OnPostMarkedAsClosedAsync([FromBody] ProjectAndQuotation markedAsClosed)
        {
            try
            {
                var result = await _dashboardRepository.MarkAsClosed(markedAsClosed.QuotationCode, markedAsClosed.ProjectName);

                if (result)
                {
                    return new JsonResult(new { success = true, message = "Marked as closed successfully" });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Failed to mark as closed." });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        #endregion Post
    }
}