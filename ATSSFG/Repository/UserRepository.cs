using APPCommon.Class;
using ATSSFG.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ATSSFG.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = PIMESSettings.atsSFGConnString;
        }

        public async Task<IEnumerable<UsersInfoModel>> GetRole()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var result = new List<UsersInfoModel>
                                        (await conn.QueryAsync<UsersInfoModel>
                                            ("laptop_role_sp",
                                              null,
                                              commandType: CommandType.StoredProcedure
                                            )
                                        );
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user data: {ex.Message}");
                throw;
            }
        }

        public async Task<UsersInfoModel> CheckUser(string name, string dept)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Name", name);
                    parameters.Add("@Department", dept);

                    var result = await conn.QueryFirstOrDefaultAsync<UsersInfoModel>("CheckUser_SP", parameters, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user data: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UsersInfoModel>> GetUsersAsync()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    return await conn.QueryAsync<UsersInfoModel>("GetUsers_SP", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user data: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                await using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", id);
                    parameters.Add("@Message", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                    await conn.ExecuteAsync("DeleteUser_SP", parameters, commandType: CommandType.StoredProcedure);

                    string message = parameters.Get<string>("@Message");
                    Console.WriteLine(message);

                    return message == "User deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> EditUserAsync(UsersInfoModel edit)
        {
            try
            {
                await using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", edit.Id);
                    parameters.Add("@Name", edit.Name);
                    parameters.Add("@Email", edit.Email);
                    parameters.Add("@Encoder", edit.Encoder);
                    parameters.Add("@Processor", edit.Processor);
                    parameters.Add("@Viewer", edit.Viewer);
                    parameters.Add("@Admin", edit.Admin);
                    parameters.Add("@Department", edit.Department);
                    parameters.Add("@IsActive", edit.IsActive);
                    parameters.Add("@Message", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                    await conn.ExecuteAsync("EditUser_SP", parameters, commandType: CommandType.StoredProcedure);

                    string message = parameters.Get<string>("@Message");
                    Console.WriteLine(message);

                    return message == "User updated successfully.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                throw;
            }
        }

        public async Task<string> AddUserAsync(UsersInfoModel add)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Name", add.Name);
                    parameters.Add("@Email", add.Email);
                    parameters.Add("@Encoder", add.Encoder);
                    parameters.Add("@Processor", add.Processor);
                    parameters.Add("@Viewer", add.Viewer);
                    parameters.Add("@Admin", add.Admin);
                    parameters.Add("@Department", add.Department);
                    parameters.Add("@Message", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                    await conn.ExecuteAsync("AddUser", parameters, commandType: CommandType.StoredProcedure);

                    string message = parameters.Get<string>("@Message");
                    return message;
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