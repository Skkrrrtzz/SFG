using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ATSAudit.Models;
using ATSAudit.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ATSAudit.Views.CPARs
{
    public partial class ReadCPAR : PageModel
    {

        public List<CorrectionModel> Corrections { get; set; }
        public static IEnumerable<string>? Files { get; set; }

        public async Task<IActionResult> OnPostUploadEvidence(List<IFormFile> evidence, string form, string? subform, string id)
        {

            if (evidence != null && evidence.Count > 0)
            {
                try
                {
                    string directoryPath = @"\\DASHBOARDPC\ATSPortals\ATSAuditFiles";
                    string fullPath = $@"{directoryPath}\{form}\{subform}\{id}";

                    Console.WriteLine(fullPath);

                    foreach (var file in evidence)
                    {
                        string filePath = Path.Combine(fullPath, file.FileName);

                        //Check if directory exists, otherwise create directory
                        if (!Directory.Exists(fullPath))
                        {
                            Directory.CreateDirectory(fullPath);
                        }

                        if (System.IO.File.Exists(filePath))
                        {
                            return StatusCode(409, $"File `{file.FileName}` already exists");
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    return StatusCode(201, "File(s) uploaded successfully!");
                } 
                catch (Exception ex)
                {
                    return StatusCode(500, $"File was not uploaded due to the following error: {ex.Message}");
                }
            }
            return StatusCode(400, "No file was uploaded.");
        }
    }
}

