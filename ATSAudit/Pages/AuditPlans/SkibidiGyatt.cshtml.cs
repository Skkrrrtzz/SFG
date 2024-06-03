using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace QA_Audit_Fresh.Views.AuditPlans
{
    public class SkibidiGyatt : PageModel
    {
        public string Text { get; set; } = "SkibidiGyatt";

        public void OnGet()
        {
        }

        public void OnGetSkibidi()
        {
            Text = "What the Sigma?";
        }

        public void OnTaeSkibidi()
        {
            Text = "Tae";
        }
    }
}