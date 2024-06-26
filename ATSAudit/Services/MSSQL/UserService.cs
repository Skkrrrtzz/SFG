using ATSAudit.Models;
using Dapper;
using Microsoft.Data.SqlClient;

using APPCommon.Class;

namespace ATSAudit.Services
{
    public class UserService : IUsersRepository
    {
        private readonly string _connectionString; 
        public UserService(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connectionString = PIMESSettings.atsAuditConnString;
        }

        public UserModel GetUser(string user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Users]";
                return connection.QueryFirstOrDefault<UserModel>(query);
            }
        }

        public async Task<UserModel> GetUserAsync(string user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Users]";
                return await connection.QueryFirstOrDefaultAsync<UserModel>(query);
            }
        }

    }
}