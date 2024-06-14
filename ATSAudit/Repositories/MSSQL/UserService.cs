using ATSAudit.Models;
using Dapper;
using Microsoft.Data.SqlClient;

using APPCommon.Class;

namespace ATSAudit.Repositories
{
    public class UserService : IUserRepository
    {
        private readonly string _connectionString; 
        public UserService(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connectionString = PIMESSettings.atsAuditConnString;
        }

        public async Task<IEnumerable<UserModel>> CheckUserExists()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[QA_Audit_Users]";
                return await connection.QueryAsync<UserModel>(query);
            }
        }

    }
}