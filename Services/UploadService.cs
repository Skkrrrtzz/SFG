using Microsoft.Office.Interop.Excel;

namespace SFG.Services
{
    public class UploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _excelDirectory = "Uploads\\Excel";
        private readonly string _pdfDirectory = "Uploads\\PDF";
        private readonly string _exportedExcelDirectory = "ExportedExcel";

        public UploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveUploadedFile(IFormFile file, string partNumber, string description)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty.");
            }

            // Create a unique file name
            string fileName = $"{partNumber} - {description}{Path.GetExtension(file.FileName)}";

            // Define the directory path where the file will be saved
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", "Excel");

            // If the directory doesn't exist, create it
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Sanitize the file name to remove invalid characters
            string sanitizedFileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

            // Replace the forward slash in the sanitized file name
            sanitizedFileName = sanitizedFileName.Replace("/", "_");

            // Combine the directory path with the unique file name to get the full path
            string filePath = Path.Combine(uploadsFolder, sanitizedFileName);

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
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", "Excel", "RFQ");

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
            string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, _excelDirectory);
            string filePath = Path.Combine(uploadsDirectory, pNDesc + ".xlsx");

            return filePath;
        }

        public string GetExportedExcel(string projectName)
        {
            string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, _exportedExcelDirectory);
            string filePath = Path.Combine(uploadsDirectory, projectName + ".xlsx");

            return filePath;
        }

        public string GetPdfFilePath(string excelFilePath)
        {
            // Get the filename without extension
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(excelFilePath);

            // Construct the PDF file path based on the Excel file path and the PDF directory
            string pdfDirectory = Path.Combine(_webHostEnvironment.WebRootPath, _pdfDirectory);

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
    }
}