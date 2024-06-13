using ATSSFG.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ATSSFG.Pages.Dashboard
{
    public class OpenProjectsModel : PageModel
    {
        #region Declarations
        private readonly IDashboardRepository _dashboardRepository;
        #endregion Declarations

        #region Constructor
        public OpenProjectsModel(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        #endregion Constructor

        #region Functions
        #endregion Functions

        #region Get
        public async Task<IActionResult> OnGetOpenProjectsSummary()
        {
            try
            {
                var result = await _dashboardRepository.GetOpenProjectsSummary();
                return new JsonResult(new { data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = $"Error: {ex.Message}" });
            }
        }
        #endregion Get
    }
}
