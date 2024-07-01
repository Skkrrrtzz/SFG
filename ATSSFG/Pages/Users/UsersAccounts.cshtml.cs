using ATSSFG.Models;
using ATSSFG.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ATSSFG.Pages.Users
{
    public class UsersAccountsModel : PageModel
    {
        #region Declaration

        private readonly IUsersRepository _userRepository;

        #endregion Declaration

        #region Constructor

        public UsersAccountsModel(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion Constructor

        #region Get
        public async Task<IActionResult> OnGetDisplayUsersAsync()
        {
            try
            {
                var Users = await _userRepository.GetUsersAsync();
                return new JsonResult(new { data = Users });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error fetching user data: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching user data.");
            }
        }
        #endregion Get

        #region Post
        public async Task<IActionResult> OnPostEditUserAsync([FromBody] UsersInfoModel UserData)
        {
            try
            {
                await _userRepository.UpdateUserAsync(UserData);
                return new JsonResult(new { success = true, message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while updating the user." });
            }
        }

        public async Task<IActionResult> OnPostDeleteUserAsync([FromBody] int UserId)
        {
            try
            {
                await _userRepository.DeleteUserAsync(UserId);
                return new JsonResult(new { success = true, message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while deleting the user." });
            }
        }
        #endregion Post
    }
}
