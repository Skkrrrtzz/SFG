using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using QA_Audit_Fresh.Models;
using QA_Audit_Fresh.Repositories;

namespace QA_Audit_Fresh.Views.AuditPlans
{
    public class DashboardRazor : PageModel
    {
        private readonly IAuditPlanRepository _repository;
        public List<AuditPlanModel>? auditPlans;

        public DashboardRazor(IAuditPlanRepository repository) 
        {
            _repository = repository;
        }

        public void OnGet(int month)
        {
            var result = _repository.GetAuditPlansByMonth(month);
            result.Wait();

            auditPlans = new List<AuditPlanModel>(result.Result);
            auditPlans.ForEach(Console.WriteLine);
        }

        public PartialViewResult OnGetCalendar() 
        {
            return Partial("Partials/_CalendarTable");
        }
    }
}