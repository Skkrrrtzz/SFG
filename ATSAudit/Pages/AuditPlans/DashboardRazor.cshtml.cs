using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace QA_Audit_Fresh.Views.AuditPlans
{
    public class DashboardRazor : PageModel
    {
        private readonly ILogger<DashboardRazor> _logger;

        public DashboardRazor(ILogger<DashboardRazor> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}