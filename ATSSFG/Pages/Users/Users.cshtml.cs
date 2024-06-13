using ATSSFG.Models;
using ATSSFG.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ATSSFG.Pages.Users
{
    public class UsersModel : PageModel
    {
        #region Declaration

        private readonly IUserRepository _userRepository;
        #endregion Declaration

        #region Constructor

        public UsersModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion Constructor

        #region Functions

        #endregion Functions

        #region Get
        public async Task<IActionResult> OnGetDisplayUsersAsync()
        {
            //dynamic data = new HomeController(null).GetData();

            //if (data == null)
            //{
            //    return Redirect("http://192.168.5.73:81");
            //}

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
        public async Task<IActionResult> OnPostDeleteUserAsync(int Id)
        {
            try
            {
                var result = await _userRepository.DeleteUserAsync(Id);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }

        public async Task<IActionResult> OnPostEditUserAsync(UsersInfoModel edit)
        {
            try
            {
                var result = await _userRepository.EditUserAsync(edit);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error updating user: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        public async Task<IActionResult> OnPostAddUserAsync(UsersInfoModel user)
        {
            try
            {
                var result = await _userRepository.AddUserAsync(user);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error adding user: {ex.Message}");
                return StatusCode(500, "An error occurred while adding the user.");
            }
        }
        #endregion Post
    }
}
