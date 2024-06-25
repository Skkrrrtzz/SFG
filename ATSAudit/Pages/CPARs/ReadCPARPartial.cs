using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ATSAudit.Models;
using ATSAudit.Repositories;

namespace ATSAudit.Views.AuditPlans
{
    public partial class ReadCPAR : PageModel
    {

        public async Task<IActionResult> OnPostUploadEvidence(List<IFormFile> evidence, string form, string? subform, string id)
        {

            if (evidence != null && evidence.Count > 0)
            {
                try
                {
                    string directoryPath = @"\\DASHBOARDPC\ATSPortals\ATSAuditFiles";
                    string fullPath = $@"{directoryPath}\{form}\{subform}\{id}";

                    foreach (var file in evidence)
                    {
                        string filePath = Path.Combine(directoryPath,  file.FileName);

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

        public async Task<IActionResult> OnGetCheckDirectory(string form, string subform, string id)
        {
            string directoryPath = @"\\DASHBOARDPC\ATSPortals\ATSAuditFiles";
            string fullPath = $@"{directoryPath}\{form}\{subform}\{id}";
            // string filePath = Path.Combine(fullPath, file.FileName);

            //Check if there are any files in the directory
            if (Directory.Exists(fullPath))
            {
                var files = Directory.EnumerateFiles(fullPath);

                //Check if there any files in the directory
                if (files.Any())
                {
                    // return StatusCode(200, "Files found!");
                    return new JsonResult(files) ;
                }

                return StatusCode(404, "No files found");


            }
            else
            {
                return StatusCode(404, "Directory does not exist");
            }


        }

            [NonHandler]
            public IEnumerable<string> CheckDirectory(string form, string subform, string id)
            {

                string directoryPath = @"\\DASHBOARDPC\ATSPortals\ATSAuditFiles";
                string fullPath = $@"{directoryPath}\{form}\{subform}\{id}";

                if(Directory.Exists(fullPath))
                {
                    var files = Directory.EnumerateFiles(fullPath);
                    return files;
                } 
                else 
                {
                    return Enumerable.Empty<string>();
                }
            }
    }
}

