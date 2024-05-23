using Dapper;
using Microsoft.Data.SqlClient;
using ATSSFG.Models;

namespace ATSSFG.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<UsersModel>> GetUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Users";

                    return await conn.QueryAsync<UsersModel>(query);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user data: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Users WHERE Id = @Id";
                    var affectedRows = await conn.ExecuteAsync(query, new { Id = id });

                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> EditUserAsync(UsersModel edit)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string selectQuery = "SELECT 1 FROM Users WHERE Id = @Id";
                    var userExists = await conn.ExecuteScalarAsync<int?>(selectQuery, new { Id = edit.Id });

                    if (userExists.HasValue)
                    {
                        string updateQuery = @"UPDATE Users SET Name = @Name, Email = @Email, Role = @Role WHERE Id = @Id";

                        var affectedRows = await conn.ExecuteAsync(updateQuery, new
                        {
                            Id = edit.Id,
                            Name = edit.Name,
                            Email = edit.Email,
                            Role = edit.Role
                        });

                        return affectedRows > 0;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                throw;
            }
        }

        public async Task<string> AddUserAsync(UsersModel add)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string checkQuery = "SELECT 1 FROM Users WHERE Email = @Email";
                    var emailExists = await conn.ExecuteScalarAsync<int?>(checkQuery, new { Email = add.Email });

                    string message;

                    if (emailExists.HasValue)
                    {
                        return message = "User with the same email already exists.";
                    }

                    string insertQuery = @"INSERT INTO Users (Name, Email, Role) VALUES (@Name, @Email, @Role)";

                    var affectedRows = await conn.ExecuteAsync(insertQuery, new
                    {
                        add.Name,
                        add.Email,
                        add.Role
                    });

                    if (affectedRows > 0)
                    {
                        return message = "User added successfully";
                    }
                    else
                    {
                        return message = "Failed to add the user.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                throw;
            }
        }
    }
}