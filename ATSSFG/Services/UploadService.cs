using ATSSFG.Models;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;

namespace ATSSFG.Services
{
    public class UploadService
    {
        private readonly string _networkDirectory = @"\\Dashboardpc\ATSPortals\ATSSFGFiles";
        private readonly string _excelDirectory = "\\Uploads\\Excel";
        private readonly string _excelRFQDirectory = "\\Uploads\\Excel\\RFQ";
        private readonly string _pdfDirectory = "\\Uploads\\PDF";
        private readonly string _exportedExcelDirectory = "\\ExportedExcel";
        private readonly string _excelTemplate = "\\Template\\Sourcing Form.xlsx";

        public async Task<string> SaveUploadedFile(IFormFile file, string partNumber, string description)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.");
            }

            // Create a unique file name
            string fileName = $"{partNumber} - {description}{Path.GetExtension(file.FileName)}";

            // Define the directory path where the file will be saved
            string networkFolderPath = _networkDirectory + _excelDirectory; // Network folder path

            // If the directory doesn't exist, create it
            if (!Directory.Exists(networkFolderPath))
            {
                Directory.CreateDirectory(networkFolderPath);
            }

            // Sanitize the file name to remove invalid characters
            string sanitizedFileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

            // Replace the forward slash in the sanitized file name
            sanitizedFileName = sanitizedFileName.Replace("/", "_");

            // Combine the directory path with the unique file name to get the full path
            string filePath = Path.Combine(networkFolderPath, sanitizedFileName);

            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; // Return the path to the saved file
        }

        public async Task<string> SaveRFQFile(IFormFile file, string description)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.");
            }

            // Create a unique file name
            string fileName = $"RFQ - {description}{Path.GetExtension(file.FileName)}";

            // Define the directory path where the file will be saved
            string uploadsFolder = Path.Combine(_networkDirectory + _excelDirectory, "RFQ");

            // If the directory doesn't exist, create it
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Combine the directory path with the unique file name to get the full path
            string filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; // Return the path to the saved file
        }

        public string GetFilePathFromPNDesc(string pNDesc)
        {
            // Construct the file path based on the parsed information and the upload directory
            string uploadsDirectory = _networkDirectory + _excelDirectory;

            string filePath = Path.Combine(uploadsDirectory, pNDesc + ".xlsx");

            return filePath;
        }

        public string GetRFQFilePNDesc(string pNDesc)
        {
            // Construct the file path based on the parsed information and the upload directory
            string uploadsDirectory = _networkDirectory + _excelRFQDirectory;

            string filePath = Path.Combine(uploadsDirectory, "RFQ - " + pNDesc + ".xlsx");

            return filePath;
        }

        public string GetExportedExcel(string projectName)
        {
            string uploadsDirectory = _networkDirectory + _exportedExcelDirectory; // change it to the RFQ directory

            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            string filePath = Path.Combine(uploadsDirectory, projectName + ".xlsx");

            return filePath;
        }

        public string GetPdfFilePath(string excelFilePath)
        {
            // Get the filename without extension
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(excelFilePath);

            // Construct the PDF file path based on the Excel file path and the PDF directory
            string pdfDirectory = _networkDirectory + _pdfDirectory;

            // If the directory doesn't exist, create it
            if (!Directory.Exists(pdfDirectory))
            {
                Directory.CreateDirectory(pdfDirectory);
            }

            string pdfFilePath = Path.Combine(pdfDirectory, fileNameWithoutExtension + ".pdf");

            return pdfFilePath;
        }

        public string GetConvertedToPdf(string filePath, int worksheetIndex)
        {
            // Create an instance of Excel Application
            Application excelApplication = new();

            // Disable alerts and warnings during the conversion process
            excelApplication.DisplayAlerts = false;
            excelApplication.Visible = false;

            try
            {
                // Open the Excel file
                Workbook workbook = excelApplication.Workbooks.Open(filePath);

                // Get the worksheet by index
                Worksheet worksheet = workbook.Worksheets[worksheetIndex];

                // Save the worksheet as PDF
                string pdfFilePath = GetPdfFilePath(filePath);
                worksheet.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pdfFilePath);

                return pdfFilePath;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error converting Excel to PDF: {ex.Message}");
                throw;
            }
            finally
            {
                // Close the workbook and Excel application
                excelApplication.Quit();

                // Release the COM objects
                ReleaseComObject(excelApplication);
            }
        }

        // Helper method to release COM objects
        private void ReleaseComObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
            }
            catch
            {
                // Ignore any errors that occur during releasing COM objects
            }
            finally
            {
                obj = null;
            }
        }

        public async Task<bool> WriteToExcel(IEnumerable<RFQModel> rfqData, RFQProjectModel rfqProject, string? projectName)
        {
            try
            {
                string uploadsDirectory = _networkDirectory + _exportedExcelDirectory;
                string filePath = Path.Combine(uploadsDirectory, $"{projectName}.xlsx");

                // If the directory doesn't exist, create it
                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                // Load the Excel template
                using (var package = new ExcelPackage(_networkDirectory + _excelTemplate))
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
                Console.WriteLine($"Error exporting Excel: {ex.Message}");
                throw;
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
                        return true;
                    }
                }
            }
            return false;
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
    }
}