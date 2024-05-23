using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System;

namespace DMDLibrary
{
    public class COMHandler
    {
        public string GetAndConvertExcelFile(string srcDir, string tempDir)
        {
            Guid guid = Guid.NewGuid();

            string outputDir = Path.Combine(tempDir, guid + ".pdf");

            Application excelApp = new Application();

            excelApp.Visible = false;

            Workbook workbook = excelApp.Workbooks.Open(srcDir);

            workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, outputDir);

            workbook.Close(false);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            return outputDir;
        }

        public string ConvertExcelIntoPDFThenByte(ExcelPackage package, string tempDir)
        {
            Guid guid = Guid.NewGuid();

            string fileName = guid.ToString();
            string pdfFilePath = Path.Combine(tempDir, fileName + ".pdf");
            string xlFilePath = Path.Combine(tempDir, fileName + ".xlsx");

            package.SaveAs(xlFilePath);

            Application excelApp = new Application();

            excelApp.Visible = false;

            Workbook workbook = excelApp.Workbooks.Open(xlFilePath);

            workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, pdfFilePath);

            workbook.Close(false);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            //System.IO.File.Delete(pdfFilePath);
            System.IO.File.Delete(xlFilePath);

            return pdfFilePath;
        }

        public byte[] DownloadExcel(string filePath)
        {
            using (ExcelPackage package = new ExcelPackage(filePath))
            {
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return package.GetAsByteArray();
            }
        }

        public void ConvertExceltoPdfAndStoreInSpecifiedPath(string srcDir, string saveIn, string fileName)
        {
            string outputDir = Path.Combine(saveIn, fileName);

            Application excelApp = new Application();

            excelApp.Visible = false;

            Workbook workbook = excelApp.Workbooks.Open(srcDir);

            workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, outputDir);

            workbook.Close(false);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }
    }
}
