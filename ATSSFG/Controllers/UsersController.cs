using Microsoft.AspNetCore.Mvc;
using ATSSFG.Models;
using ATSSFG.Repository;

namespace ATSSFG.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> GetUsers()
        {
            //dynamic data = new HomeController(null).GetData();

            //if (data == null)
            //{
            //    return Redirect("http://192.168.5.73:81");
            //}

            try
            {
                var Users = await _userRepository.GetUsers();
                return Json(new { data = Users });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error fetching user data: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching user data.");
            }
        }

        public async Task<IActionResult> DeleteUser(int Id)
        {
            try
            {
                var result = await _userRepository.DeleteUser(Id);
                return Json(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }

        public async Task<IActionResult> EditUserAsync(UsersModel edit)
        {
            try
            {
                var result = await _userRepository.EditUserAsync(edit);
                return Json(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error updating user: {ex.Message}");
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        public async Task<IActionResult> AddUserAsync(UsersModel user)
        {
            try
            {
                var result = await _userRepository.AddUserAsync(user);
                return Json(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error adding user: {ex.Message}");
                return StatusCode(500, "An error occurred while adding the user.");
            }
        }
    }
}