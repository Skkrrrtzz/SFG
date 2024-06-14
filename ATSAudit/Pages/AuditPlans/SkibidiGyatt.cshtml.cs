using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ATSAudit.Views.AuditPlans
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

        // public void OnTaeSkibidi()
        // {
        //     Text = "Tae";
        // }
    }
}