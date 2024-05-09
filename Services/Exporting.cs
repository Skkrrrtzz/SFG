using Microsoft.EntityFrameworkCore.Diagnostics;
using OfficeOpenXml;
using SFG.Models;

namespace SFG.Services
{
    public class Exporting
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string ExcelTemplate = "Template\\Sourcing Form.xlsx";
        private readonly string ExcelOutput = "ExportedExcel";

        public Exporting(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> WriteToExcel(IEnumerable<RFQModel> rfqData, RFQProjectModel rfqProject, string? projectName, int rfqDataStartColumn)
        {
            try
            {
                string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, ExcelOutput);
                string filePath = Path.Combine(uploadsDirectory, $"{projectName}.xlsx");

                // Load the Excel template
                using (var package = new ExcelPackage(new FileInfo(Path.Combine(_webHostEnvironment.WebRootPath, ExcelTemplate))))
                {
                    // Access the first worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // Check if cells from E5 to E10 already have values
                    bool hasValues = CheckRFQProjectDataExists(worksheet, "E5", "E10");

                    if (!hasValues)
                    {
                        // Insert values of RFQProjectModel
                        InsertRFQProjectData(worksheet, rfqProject, "E5");
                    }

                    // Insert values of rfqData
                    InsertRFQData(worksheet, rfqData, 14);

                    // Save the Excel file to the specified path asynchronously
                    await package.SaveAsAsync(new FileInfo(filePath));

                    // Return true indicating successful saving
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting to Excel: {ex.Message}");
                // Return false indicating failure
                return false;
            }
        }

        private bool CheckRFQProjectDataExists(ExcelWorksheet worksheet, string startCell, string endCell)
        {
            for (int row = worksheet.Cells[startCell].Start.Row; row <= worksheet.Cells[endCell].End.Row; row++)
            {
                for (int col = worksheet.Cells[startCell].Start.Column; col <= worksheet.Cells[endCell].End.Column; col++)
                {
                    if (worksheet.Cells[row, col].Value != null)
                    {
                        return true; // If any cell has a value, return true
                    }
                }
            }
            return false; // If no cell has a value, return false
        }

        private void InsertRFQProjectData(ExcelWorksheet worksheet, RFQProjectModel rfqProject, string startCell)
        {
            worksheet.Cells[startCell].Value = rfqProject.ProjectName;
            worksheet.Cells[startCell].Offset(1, 0).Value = rfqProject.Customer;
            worksheet.Cells[startCell].Offset(2, 0).Value = rfqProject.QuotationCode;
            worksheet.Cells[startCell].Offset(3, 0).Value = rfqProject.NoItems;
            worksheet.Cells[startCell].Offset(4, 0).Value = rfqProject.RequestDate;
            worksheet.Cells[startCell].Offset(5, 0).Value = rfqProject.RequiredDate;
        }

        private void InsertRFQData(ExcelWorksheet worksheet, IEnumerable<RFQModel> rfqData, int startRow)
        {
            int row = startRow;

            foreach (var rfqModel in rfqData)
            {
                worksheet.Cells[row, 1].Value = rfqModel.Id;
                worksheet.Cells[row, 2].Value = rfqModel.CustomerPartNumber;
                worksheet.Cells[row, 3].Value = rfqModel.Rev;
                worksheet.Cells[row, 5].Value = rfqModel.Description;
                worksheet.Cells[row, 6].Value = rfqModel.OrigMPN;
                worksheet.Cells[row, 7].Value = rfqModel.OrigMFR;
                worksheet.Cells[row, 12].Value = rfqModel.Commodity;
                worksheet.Cells[row, 13].Value = rfqModel.Eqpa;
                worksheet.Cells[row, 14].Value = rfqModel.AnnualForecast;
                worksheet.Cells[row, 15].Value = rfqModel.UoM;
                worksheet.Cells[row, 16].Value = rfqModel.Status;

                row++;
            }
        }

        //private void InsertRFQData(ExcelWorksheet worksheet, IEnumerable<RFQModel> rfqData, int startRow, int startColumn)
        //{
        //    int row = startRow;
        //    int col = startColumn;

        //    foreach (var rfqModel in rfqData)
        //    {
        //        worksheet.Cells[row, col++].Value = rfqModel.Id;
        //        worksheet.Cells[row, col++].Value = rfqModel.CustomerPartNumber;
        //        worksheet.Cells[row, col++].Value = rfqModel.Rev;
        //        // Skip the 4th column if it's hidden
        //        col++;
        //        worksheet.Cells[row, col++].Value = rfqModel.Description;
        //        worksheet.Cells[row, col++].Value = rfqModel.OrigMPN;
        //        worksheet.Cells[row, col++].Value = rfqModel.OrigMFR;
        //        col++;
        //        col++;
        //        col++;
        //        col++;
        //        worksheet.Cells[row, col++].Value = rfqModel.Commodity;
        //        worksheet.Cells[row, col++].Value = rfqModel.Eqpa;
        //        worksheet.Cells[row, col++].Value = rfqModel.AnnualForecast;
        //        worksheet.Cells[row, col++].Value = rfqModel.UoM;
        //        worksheet.Cells[row, col++].Value = rfqModel.Status;

        //        // Reset column index and increment row index for the next iteration
        //        col = startColumn;
        //        row++;
        //    }
        //}
    }
}