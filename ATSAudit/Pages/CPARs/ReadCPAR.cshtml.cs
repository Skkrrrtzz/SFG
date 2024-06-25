using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ATSAudit.Models;
using ATSAudit.Repositories;

namespace ATSAudit.Views.AuditPlans
{
    [BindProperties]
    public partial class ReadCPAR : PageModel
    {
        private readonly ICPARsRepository _cpars;
        private readonly ICorrectionsRepository _corrections;
        private readonly ICorrectiveActionsRepository _correctiveActions;
        private readonly IPreventiveActionsRepository _preventiveActions;

        public ReadCPAR(    ICPARsRepository cpars,
                            ICorrectionsRepository corrections, 
                            ICorrectiveActionsRepository correctiveActions,
                            IPreventiveActionsRepository preventiveActions) 
        {
            _cpars = cpars;
            _corrections = corrections;
            _correctiveActions = correctiveActions;
            _preventiveActions = preventiveActions;
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

        //GET: https://localhost:<port>?handler=Corrections&cparId=<cparId>
        public async Task<PartialViewResult> OnGetCorrections(int cparId)
        {
            return Partial("Partials/_CPARCorrectionsTable", (List<CorrectionModel>) await _corrections.GetCorrectionsByCPAR(cparId));
        }

        //GET: https://localhost:<port>?handler=CorrectiveActions&cparId=<cparId>
        public async Task<PartialViewResult> OnGetCorrectiveActions(int cparId)
        {
            return Partial("Partials/_CPARCorrectiveActionsTable", (List<CorrectiveActionModel>) await _correctiveActions.GetCorrectiveActionsByCPAR(cparId));
        }

        //GET: https://localhost:<port>?handler=PreventiveActions&cparId=<cparId>
        public async Task<PartialViewResult> OnGetPreventiveActions(int cparId)
        {
            return Partial("Partials/_CPARPreventiveActionsTable", (List<PreventiveActionModel>) await _preventiveActions.GetPreventiveActionsByCPAR(cparId));
        }


        #region POST
        
        #endregion

    }
}

