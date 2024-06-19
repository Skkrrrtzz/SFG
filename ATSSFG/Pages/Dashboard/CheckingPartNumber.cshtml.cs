using ATSSFG.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ATSSFG.Pages.Dashboard
{
    public class CheckingPartNumberModel : PageModel
    {
        #region Declarations

        private readonly IDashboardRepository _dashboardRepository;

        #endregion Declarations

        #region Constructor

        public CheckingPartNumberModel(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        #endregion Constructor

        #region Get

        public async Task<IActionResult> OnGetCheckingPartNumber()
        {
            try
            {
                var result = await _dashboardRepository.CheckingPartNumber();
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