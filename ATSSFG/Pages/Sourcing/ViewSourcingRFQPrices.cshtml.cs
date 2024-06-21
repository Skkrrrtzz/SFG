using ATSSFG.Models;
using ATSSFG.Repository;
using ATSSFG.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace ATSSFG.Pages.Sourcing
{
    public class ViewSourcingRFQPricesModel : PageModel
    {
        #region Declaration

        private readonly UploadService _uploadService;
        private readonly ISourcingRepository _sourcingRepository;

        #endregion Declaration

        #region Constructor

        public ViewSourcingRFQPricesModel(ISourcingRepository sourcingRepository, UploadService uploadService)
        {
            _uploadService = uploadService;
            _sourcingRepository = sourcingRepository;
        }

        #endregion Constructor

        #region Function

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

                    // If partNumber was not found, return null
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing query: {ex.Message}");
                throw;
            }
        }

        public async Task<PartData> ReadExcelFile(string filePath, string partNumber)
        {
            var partData = new PartData { PartNumber = partNumber };

            try
            {
                int? partNumberRow = await FindPartNumber(filePath, partNumber);

                if (partNumberRow.HasValue)
                {
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

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

                        // Initialize the supplier details dictionary
                        var supplierDetailsDict = new Dictionary<int, SupplierCostDetail>();

                        // Get values and pair them with headers
                        for (int i = 0; i < headers.Count; i++)
                        {
                            string header = headers[i];
                            int col = nonHiddenColumns[i];
                            string cellValue = worksheet.Cells[partNumberRow.Value, col].Text;
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
                    return partData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading Excel file: {ex.Message}");
                return null;
            }
        }

        #endregion Function

        #region Post

        public async Task<IActionResult> OnPostFindPricesAsync([FromBody] PartData data)
        {
            try
            {
                var fileName = _uploadService.GetRFQFilePNDesc(data.ProjectName);

                if (fileName == null)
                {
                    return NotFound(new { message = "File not found for the provided project name." });
                }

                PartData result = await ReadExcelFile(fileName, data.PartNumber);

                if (result != null)
                {
                    return new JsonResult(new { success = true, data = result });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "No prices found for the part number." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }
        #endregion Post
    }
}