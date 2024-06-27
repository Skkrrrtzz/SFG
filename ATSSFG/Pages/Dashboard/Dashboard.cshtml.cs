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
        private readonly ISourcingRepository _sourcingRepository;

        #endregion Declaration

        #region Constructor

        public DashboardModel(UploadService uploadService, IDashboardRepository dashboardRepository, ISourcingRepository sourcingRepository)
        {
            _uploadService = uploadService;
            _dashboardRepository = dashboardRepository;
            _sourcingRepository = sourcingRepository;
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

        private async Task<int?> FindPartNumber(string fileName, string partNumber)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(partNumber))
            {
                return null;
            }

            try
            {
                using (var package = new ExcelPackage(new FileInfo(fileName)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int partNumberColumn = 2;

                    // Find the row containing the partNumber
                    for (int row = 14; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, partNumberColumn].Text.Equals(partNumber, StringComparison.OrdinalIgnoreCase))
                        {
                            return row;
                        }
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing query: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> WriteSuggestedSupplierAndComments(string filePath, string quotationCode)
        {
            try
            {
                var result = await _dashboardRepository.GetRFQByQuotationCode(quotationCode);

                if (result == null)
                {
                    return false;
                }

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    int suggestedSupplierColumn = -1;
                    int commentsColumn = -1;

                    // Find the columns for "Cost Engineer's Suggested Supplier" and "Comments"
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (worksheet.Cells[13, col].Text == "Cost Engineer's Suggested Supplier")
                        {
                            suggestedSupplierColumn = col;
                        }
                        else if (worksheet.Cells[13, col].Text == "Comments")
                        {
                            commentsColumn = col;
                        }
                    }

                    // If either column is not found, return false
                    if (suggestedSupplierColumn == -1 || commentsColumn == -1)
                    {
                        Console.WriteLine("One or both of the required columns not found.");
                        return false;
                    }

                    foreach (var item in result)
                    {
                        int? partNumberRow = await FindPartNumber(filePath, item.CustomerPartNumber);

                        if (partNumberRow.HasValue)
                        {
                            worksheet.Cells[partNumberRow.Value, suggestedSupplierColumn].Value = item.SuggestedSupplier;
                            worksheet.Cells[partNumberRow.Value, commentsColumn].Value = item.Comments;
                        }
                        else
                        {
                            Console.WriteLine($"Part number {item.CustomerPartNumber} not found.");
                        }
                    }

                    await package.SaveAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing query: {ex.Message}");
                return false;
            }
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

        public async Task<IActionResult> OnGetIncomingRFQProjects_1Async()
        {
            try
            {
                var result = await _dashboardRepository.GetIncomingRFQProjects_1();
                return new JsonResult(new { data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = $"Error: {ex.Message}" });
            }
        }

        public async Task<IActionResult> OnGetIncomingRFQProjects_2Async()
        {
            try
            {
                var result = await _dashboardRepository.GetIncomingRFQProjects_2();
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
            string filePath = _uploadService.GetRFQFilePNDesc(projectName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            string fileName = Path.GetFileName(filePath);

            var fileStream = new FileStream(filePath, FileMode.Open);

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

        public async Task<IActionResult> OnPostInsertRFQAsync([FromBody] Dictionary<string, object> formData)
        {
            try
            {
                if (formData != null)
                {
                    var rfqProjects = new RFQProjectModel
                    {
                        ProjectName = formData["ProjectName"].ToString(),
                        Customer = formData["Customer"].ToString(),
                        QuotationCode = formData["QuotationCode"].ToString(),
                        NoItems = Convert.ToInt32(formData["NoItems"].ToString()),
                        RequestDate = DateTime.Parse(formData["RequestDate"].ToString()),
                        RequiredDate = DateTime.Parse(formData["RequiredDate"].ToString()),
                        StdTAT = Convert.ToInt32(formData["StdTAT"].ToString()),
                        Status = "OPEN",
                        HasPrices = false
                    };

                    var rfqData = new List<RFQModel>
                    {
                        new RFQModel
                        {
                            ProjectName = formData["ProjectName"]?.ToString(),
                            Customer = formData["Customer"]?.ToString(),
                            QuotationCode = formData["QuotationCode"]?.ToString(),
                            CustomerPartNumber = formData["CustomerPartNumber"]?.ToString(),
                            Rev = formData["Rev"]?.ToString(),
                            Description = formData["Description"]?.ToString(),
                            OrigMPN = formData["OrigMPN"]?.ToString(),
                            OrigMFR = formData["OrigMFR"]?.ToString(),
                            Eqpa = Convert.ToInt32(formData["Eqpa"].ToString()),
                            Commodity = formData["Commodity"]?.ToString(),
                            UoM = formData["UoM"]?.ToString(),
                            Status = formData["Status"]?.ToString(),
                            Remarks = "FOR SOURCING"
                        }
                    };

                    var result = await _sourcingRepository.InsertRFQ(rfqProjects, rfqData);

                    return new JsonResult(new { success = result, message = result ? "RFQ inserted successfully" : "Failed to insert RFQ." });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Failed to insert RFQ." });
                }
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
                    var fileName = _uploadService.GetRFQFilePNDesc(markedAsClosed.ProjectName);

                    if (fileName == null)
                    {
                        return NotFound(new { message = "File not found for the provided project name." });
                    }

                    bool excelResult = await WriteSuggestedSupplierAndComments(fileName, markedAsClosed.QuotationCode);

                    if (excelResult)
                    {
                        return new JsonResult(new { success = true, message = "Marked as closed successfully" });
                    }
                    else
                    {
                        return new JsonResult(new { success = false, message = "Failed to update Excel file." });
                    }
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

        public async Task<IActionResult> OnPostSummaryRFQperMonth([FromBody] string yearMonth)
        {
            try
            {
                var result = await _dashboardRepository.GetSummaryRFQperMonth(yearMonth);
                return new JsonResult(new { data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = $"Error: {ex.Message}" });
            }
        }

        #endregion Post
    }
}