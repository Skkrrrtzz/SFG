using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ATSAudit.Models;
using ATSAudit.Services;
using ATSAudit.DTOs;

namespace ATSAudit.Views.CPARs
{
    [BindProperties]
    public class ReadCPAR : PageModel
    {
        private readonly ICPARsRepository _cpars;
        private readonly ICorrectionsRepository _corrections;
        private readonly ICorrectiveActionsRepository _correctiveActions;
        private readonly IPreventiveActionsRepository _preventiveActions;
        private readonly FileService _files;

        public ReadCPAR(    ICPARsRepository cpars,
                            ICorrectionsRepository corrections, 
                            ICorrectiveActionsRepository correctiveActions,
                            IPreventiveActionsRepository preventiveActions,
                            FileService files
                            ) 
        {
            _cpars = cpars;
            _corrections = corrections;
            _correctiveActions = correctiveActions;
            _preventiveActions = preventiveActions;
            _files = files;
        }
        
        public CPARModel? cpar;
        public int CPARId { get; set; }
        public string? Respondent { get; set; }
        public string? Requestor { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ResponseDueDate { get; set; }
        public string? ISOClause { get; set; }
        public string? ProblemStatement { get; set; }
        public string? PreparedBy { get; set; }
        public string? CheckedBy { get; set; }
        public string? ApprovedBy { get; set; }

        public List<CorrectionModel> Corrections { get; set; }
        public static IEnumerable<string>? Files { get; set; }



        public void OnGet(int cparId)
        {
            cpar = new List<CPARModel>(_cpars.GetCPAR(cparId)).FirstOrDefault();

            if (cpar == null)
            {
                Console.WriteLine("BULOK");
            } 
            else 
            {
                Console.WriteLine($"CPARId: {cpar.CPARId}");
                CPARId = cpar.CPARId;
                Respondent = cpar.Respondent;
                Requestor = cpar.Requestor;
                IssueDate = cpar.IssueDate;
                ResponseDueDate = cpar.ResponseDueDate;
                ISOClause = cpar.ISOClause;
                ProblemStatement = cpar.ProblemStatement;
                PreparedBy = cpar.PreparedBy;
                CheckedBy = cpar.CheckedBy;
                ApprovedBy = cpar.ApprovedBy;
            }
        }

        #region Partials

        //GET: https://localhost:<port>?handler=Corrections&cparId=<cparId>
        public async Task<PartialViewResult> OnGetCorrections(int cparId)
        {
            //Checking Database
            var corrections = await _corrections.GetCorrectionsByCPAR(cparId);

            //Checking Filesystem
            foreach (var correction in corrections)
            {
                correction.HasEvidence = await _files.CheckDirectoryIfEmpty(new ActionItemDTO("CPARs", "Corrections", correction.CorrectionId));
            }

            return Partial("Partials/_CPARCorrectionsTable", (List<CorrectionModel>) corrections);
        }

        //GET: https://localhost:<port>?handler=CorrectiveActions&cparId=<cparId>
        public async Task<PartialViewResult> OnGetCorrectiveActions(int cparId)
        {
            //Checking Database
            var correctiveActions = await _correctiveActions.GetCorrectiveActionsByCPAR(cparId);

            //Checking Filesystem
            foreach (var correctiveAction in correctiveActions)
            {
                correctiveAction.HasEvidence = await _files.CheckDirectoryIfEmpty(new ActionItemDTO("CPARs", "CorrectiveActions", correctiveAction.CorrectiveActionId));
            }

            return Partial("Partials/_CPARCorrectiveActionsTable", (List<CorrectiveActionModel>) correctiveActions);
        }

        //GET: https://localhost:<port>?handler=PreventiveActions&cparId=<cparId>
        public async Task<PartialViewResult> OnGetPreventiveActions(int cparId)
        {
            //Checking Database
            var preventiveActions = await _preventiveActions.GetPreventiveActionsByCPAR(cparId);

            //Checking Filesystem
            foreach (var preventiveAction in preventiveActions)
            {
                // preventiveAction.HasEvidence = await _files.CheckDirectoryIfEmpty("CPARs", "PreventiveActions", preventiveAction.PreventiveActionId);
                preventiveAction.HasEvidence = await _files.CheckDirectoryIfEmpty(new ActionItemDTO("CPARs", "PreventiveActions", preventiveAction.PreventiveActionId));
            }
            return Partial("Partials/_CPARPreventiveActionsTable", (List<PreventiveActionModel>) preventiveActions);
        }

        #endregion

        #region Evidence

        public async Task<IActionResult> OnPostUploadEvidence(List<IFormFile> evidence, string form, string subform, string id)
        {
            ActionItemDTO actionItem = new(form, subform, id);

            if (evidence != null && evidence.Count > 0)
            {
                try
                {
                    // string directoryPath = @"\\DASHBOARDPC\ATSPortals\ATSAuditFiles";
                    // string fullPath = $@"{directoryPath}\{actionItem.Form}\{actionItem.Subform}\{actionItem.Id}";
                    string fullPath = _files.GetFullPath(actionItem);

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

        public async Task<IActionResult> OnGetEvidences(string form, string subform, string id)
        {
            ActionItemDTO actionItem = new ActionItemDTO(form, subform, id);

            if (Directory.Exists(_files.GetFullPath(actionItem)))
            {
                return new OkObjectResult(await _files.GetFileNamesOrEmpty(actionItem));
                // return PhysicalFile();
            }
            return NotFound(Enumerable.Empty<string>());
        }

        public IActionResult OnPostKertzGae()
        {
            return new OkObjectResult("Kertz Gae");
        }

        #endregion
    }
}

