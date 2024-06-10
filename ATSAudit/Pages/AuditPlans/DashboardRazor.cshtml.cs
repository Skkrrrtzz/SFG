using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QA_Audit_Fresh.Models;
using QA_Audit_Fresh.Repositories;

namespace QA_Audit_Fresh.Views.AuditPlans
{
    public class DashboardRazor : PageModel
    {
        private readonly IAuditPlanRepository _auditPlans;
        private readonly IConformityRepository _conformities;
        private readonly ICPARRepository _cpars;
        private readonly ICorrectionRepository _corrections;
        private readonly ICorrectiveActionRepository _correctiveActions;
        private readonly IPreventiveActionRepository _preventiveActions;

        public List<AuditPlanModel>? AuditPlans;

        public DashboardRazor(  IAuditPlanRepository auditPlans, 
                                IConformityRepository conformities, 
                                ICPARRepository cpars, 
                                ICorrectionRepository corrections, 
                                ICorrectiveActionRepository correctiveActions,
                                IPreventiveActionRepository preventiveActions) 
        {
            _auditPlans = auditPlans;
            _conformities = conformities;
            _cpars = cpars;
            _corrections = corrections;
            _correctiveActions = correctiveActions;
            _preventiveActions = preventiveActions;
        }

        public void OnGet(int month)
        {
            var result = _auditPlans.GetAuditPlansByMonth(month);
            result.Wait();

            AuditPlans = new List<AuditPlanModel>(result.Result);
            // auditPlans.ForEach(Console.WriteLine);
        }

        // public PartialViewResult OnGetCalendar() 
        // {
        //     return Partial("Partials/_CalendarTable");
        // }

        //GET: https://localhost:<port>/Conformities?planId=<planId>
        public async Task<PartialViewResult> OnGetConformities(int planId)
        {
            return Partial("Partials/_ConformitiesTable", (List<ConformityModel>) await _conformities.GetConformitiesByAuditPlan(planId));
        }

        //GET: https://localhost:<port>/CPARs?planId=<planId>
        public async Task<PartialViewResult> OnGetCPARs(int planId)
        {
            return Partial("Partials/_CPARsTable", (List<CPARModel>) await _cpars.GetCPARsByAuditPlan(planId));
        }

        //GET: https://localhost:<port>/Corrections?cparId=<cparId>
        public async Task<PartialViewResult> OnGetCorrections(int cparId)
        {
            return Partial("Partials/_CorrectionsTable", (List<CorrectionModel>) await _corrections.GetCorrectionsByCPAR(cparId));
        }

        //GET: https://localhost:<port>/CorrectiveActions?cparId=<cparId>
        public async Task<PartialViewResult> OnGetCorrectiveActions(int cparId)
        {
            return Partial("Partials/_CorrectiveActionsTable", (List<CorrectiveActionModel>) await _correctiveActions.GetCorrectiveActionsByCPAR(cparId));
        }

        //GET: https://localhost:<port>/PreventiveActions?cparId=<cparId>
        public async Task<PartialViewResult> OnGetPreventiveActions(int cparId)
        {
            return Partial("Partials/_PreventiveActionsTable", (List<PreventiveActionModel>) await _preventiveActions.GetPreventiveActionsByCPAR(cparId));
        }
    }
}