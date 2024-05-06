using APPCommon.Class;
using Dapper;
using MySqlConnector;
using System.Data;
using WEBTemplate.Models;

namespace WEBTemplate.Repository
{
    public class LoginRepository : ILoginRepository
    {
        public async Task<IEnumerable<UserLoginModel>> GetLogin(string strpass)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(PIMESSettings.mysqlConnString))
            {
                var sqlParameters = new DynamicParameters();
                sqlParameters.Add("@inmode", "login");
                sqlParameters.Add("@inpass", strpass);

                var result = new List<UserLoginModel>
                                    (await sqlConnection.QueryAsync<UserLoginModel>
                                        ("laptop_role_sp",
                                          sqlParameters,
                                          commandType: CommandType.StoredProcedure
                                        )
                                    );
                return result;
            }
        }

        public async Task<byte[]> GetEmployeeImage(int in_empno)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(PIMESSettings.mysqlConnString))
            {
                var sqlParameters = new DynamicParameters();
                sqlParameters.Add("@varempcode", in_empno);

                var result = (await sqlConnection.QuerySingleOrDefaultAsync<byte[]>
                                     ("emplmaster_employee_image_get",
                                         sqlParameters,
                                         commandType: CommandType.StoredProcedure
                                     )
                                );

                return result;
            }
        }

        public async Task<IEnumerable<UserRoleModel>> GetRole()
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(PIMESSettings.mysqlConnString))
            {
                var sqlParameters = new DynamicParameters();
                sqlParameters.Add("@inmode", "role");
                sqlParameters.Add("@inpass", string.Empty);

                var result = new List<UserRoleModel>
                                    (await sqlConnection.QueryAsync<UserRoleModel>
                                        ("laptop_role_sp",
                                          sqlParameters,
                                          commandType: CommandType.StoredProcedure
                                        )
                                    );
                return result;
            }
        }
    }
}