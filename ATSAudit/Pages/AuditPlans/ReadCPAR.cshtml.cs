using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QA_Audit_Fresh.Models;
using QA_Audit_Fresh.Repositories;

namespace QA_Audit_Fresh.Views.AuditPlans
{
    [BindProperties]
    public class ReadCPAR : PageModel
    {
        private readonly ICPARsRepository _cpars;

        public ReadCPAR(ICPARsRepository cpars) 
        {
            _cpars = cpars;
        }
        
        public CPARModel? cpar;
        public string? Respondent { get; set; }
        public string? Requestor { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ResponseDueDate { get; set; }
        public string? ISOClause { get; set; }
        public string? ProblemStatement { get; set; }
        public string? PreparedBy { get; set; }
        public string? CheckedBy { get; set; }
        public string? ApprovedBy { get; set; }

        public async void OnGet(int cparId)
        {
            cpar = new List<CPARModel>(await _cpars.GetCPAR(cparId)).FirstOrDefault();

            if (cpar != null)
            {
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

        public void OnGet()
        {

        }

    }
}

