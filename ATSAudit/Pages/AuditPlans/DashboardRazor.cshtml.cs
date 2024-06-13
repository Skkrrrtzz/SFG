using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QA_Audit_Fresh.Models;
using QA_Audit_Fresh.Repositories;

namespace QA_Audit_Fresh.Views.AuditPlans
{
    public class DashboardRazor : PageModel
    {
        private readonly IAuditPlansRepository _auditPlans;
        private readonly IConformitiesRepository _conformities;
        private readonly ICPARsRepository _cpars;
        private readonly ICorrectionsRepository _corrections;
        private readonly ICorrectiveActionsRepository _correctiveActions;
        private readonly IPreventiveActionsRepository _preventiveActions;
        private readonly IHttpContextAccessor _httpContext;

        public List<AuditPlanModel>? AuditPlans;


        public DashboardRazor(  IAuditPlansRepository auditPlans, 
                                IConformitiesRepository conformities, 
                                ICPARsRepository cpars, 
                                ICorrectionsRepository corrections, 
                                ICorrectiveActionsRepository correctiveActions,
                                IPreventiveActionsRepository preventiveActions,
                                IHttpContextAccessor httpContext) 
        {
            _auditPlans = auditPlans;
            _conformities = conformities;
            _cpars = cpars;
            _corrections = corrections;
            _correctiveActions = correctiveActions;
            _preventiveActions = preventiveActions;
            _httpContext = httpContext;
        }

        public void OnGet()
        {
            // _httpContext.HttpContext.Session.SetString("userName", z);
            // _httpContext.HttpContext.Session.SetString("MyMode", y);
            // _httpContext.HttpContext.Session.SetString("MyBUCode", x);
            // _httpContext.HttpContext.Session.SetString("MyRole", w);

            // string userName = _httpContext.HttpContext.Session.GetString("userName");
            //Check if user exists in Database
            Console.WriteLine(string.IsNullOrEmpty(HttpContext.User.FindFirstValue("FullName")));
            Console.WriteLine(HttpContext.User.Claims.Count());
            Console.WriteLine(HttpContext.User.Identity.IsAuthenticated);
        }

        //GET: https://localhost:<port>?handler=Conformities&planId=<planId>
        public async Task<PartialViewResult> OnGetConformities(int planId)
        {
            return Partial("Partials/_ConformitiesTable", (List<ConformityModel>) await _conformities.GetConformitiesByAuditPlan(planId));
        }

        //GET: https://localhost:<port>?handler=CPARs&planId=<planId>
        public async Task<PartialViewResult> OnGetCPARs(int planId)
        {
            return Partial("Partials/_CPARsTable", (List<CPARModel>) await _cpars.GetCPARsByAuditPlan(planId));
        }

        //GET: https://localhost:<port>?handler=Corrections&cparId=<cparId>
        public async Task<PartialViewResult> OnGetCorrections(int cparId)
        {
            return Partial("Partials/_CorrectionsTable", (List<CorrectionModel>) await _corrections.GetCorrectionsByCPAR(cparId));
        }

        //GET: https://localhost:<port>?handler=CorrectiveActions&cparId=<cparId>
        public async Task<PartialViewResult> OnGetCorrectiveActions(int cparId)
        {
            return Partial("Partials/_CorrectiveActionsTable", (List<CorrectiveActionModel>) await _correctiveActions.GetCorrectiveActionsByCPAR(cparId));
        }

        //GET: https://localhost:<port>?handler=PreventiveActions&cparId=<cparId>
        public async Task<PartialViewResult> OnGetPreventiveActions(int cparId)
        {
            return Partial("Partials/_PreventiveActionsTable", (List<PreventiveActionModel>) await _preventiveActions.GetPreventiveActionsByCPAR(cparId));
        }
    }
}