using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ATSAudit.Models;
using ATSAudit.Repositories;
using APPCommon.Class;

namespace ATSAudit.Views.AuditPlans
{
    [Authorize]
    public class DashboardRazor : PageModel
    {
        private readonly IAuditPlansRepository _auditPlans;
        private readonly IConformitiesRepository _conformities;
        private readonly ICPARsRepository _cpars;
        private readonly ICorrectionsRepository _corrections;
        private readonly ICorrectiveActionsRepository _correctiveActions;
        private readonly IPreventiveActionsRepository _preventiveActions;
        private readonly IUsersRepository _users;

        public string loader { get; } = PIMESProcedures.randomLoader();
        public AuditPlanModel CurrentAuditPlan { get; set; }
        public UserModel? CurrentUser { get; set; }


        public DashboardRazor(  IAuditPlansRepository auditPlans, 
                                IConformitiesRepository conformities, 
                                ICPARsRepository cpars, 
                                ICorrectionsRepository corrections, 
                                ICorrectiveActionsRepository correctiveActions,
                                IPreventiveActionsRepository preventiveActions,
                                IUsersRepository usersRepository) 
        {
            _auditPlans = auditPlans;
            _conformities = conformities;
            _cpars = cpars;
            _corrections = corrections;
            _correctiveActions = correctiveActions;
            _preventiveActions = preventiveActions;
            _users = usersRepository;
        }

        public void OnGet()
        {
            // string userName = User.FindFirstValue("FullName");

            Console.WriteLine(User.Identity.IsAuthenticated ? "Authenticated!" : "Not Authenticated. :(");
            Console.WriteLine(User.FindFirstValue("FullName"));
            Console.WriteLine(User.FindFirstValue("Password"));
            Console.WriteLine(User.FindFirstValue("EmpNo"));

            //Check if user exists in Database
            string userName = User.FindFirstValue("FullName");
            CurrentUser = _users.GetUser(userName);

            if (CurrentUser == null)
            {
                Console.WriteLine("Uh oh! An error occurred!");
            } 
            else
            {
                Console.WriteLine(CurrentUser.Approver);
                Console.WriteLine(CurrentUser.Requestor);
                Console.WriteLine(CurrentUser.Respondent);
                Console.WriteLine(CurrentUser.Viewer);
            }

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