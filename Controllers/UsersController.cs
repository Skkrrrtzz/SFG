using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFG.Data;
using SFG.Models;

namespace SFG.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext dataBase)
        {
            _db = dataBase;
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
                IEnumerable<dynamic> Users;
                using (var con = _db.Database.GetDbConnection())
                {
                    Users = con.Query($"SELECT A.AccountID, A.AccountName, A.Department, A.Email, SA.PortalRole " +
                                      $"FROM Accounts A INNER JOIN SubAccounts SA ON A.AccountID = SA.AccountID " +
                                      $"WHERE SA.PortalID = 3");
                }

                // Return the data in JSON format inside the try block
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
                await new ATS_Library.Database.Accounts().DeleteAccount(Id);

                return Json(new { success = true, message = $"User deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        public async Task<IActionResult> EditUserAsync(UsersModel edit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Fetch the user to be updated asynchronously
                    var updateUser = await _db.Users.FirstOrDefaultAsync(j => j.Id == edit.Id);

                    if (updateUser != null)
                    {
                        // Update user properties
                        updateUser.Name = edit.Name;
                        updateUser.Email = edit.Email;
                        updateUser.Role = edit.Role;

                        // Update the user in the database asynchronously
                        _db.Users.Update(updateUser);
                        await _db.SaveChangesAsync();

                        return Json(new { success = true, message = $"User updated successfully" });
                    }
                    else
                    {
                        return Json(new { success = false, message = $"User not found or failed to update." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = $"Invalid model state." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        public async Task<IActionResult> AddUserAsync(UsersModel user)
        {
            try
            {
                // Check if username, email, or department already exists
                if (await _db.Users.AnyAsync(u => u.Email == user.Email))
                {
                    return Json(new { success = false, message = $"User with the same email already exists." });
                }

                // If none of the checks failed, add the user to the database asynchronously
                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = $"User added successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // You may want to use a logging library or log to a file/database
                Console.WriteLine($"Error adding user: {ex.Message}");

                // Return an error response to the client
                return Json(new { success = false, message = $"An error occurred while adding the user." });
            }
        }
    }
}