using APPCommon.Class;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.Data;
using WEBLogin.Models;

namespace WEBLogin.Repository
{
    public class LoginRepository : ILoginRepository
    {
        public async Task<IEnumerable<UserLoginModel>> GetLogin(string strpass)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(PIMESSettings.mysqlConnString))
            {
                var sqlParameters = new DynamicParameters();
                sqlParameters.Add("@varpass", strpass);

                var result = new List<UserLoginModel>
                                    (await sqlConnection.QueryAsync<UserLoginModel>
                                        ("com_login_sp",
                                          sqlParameters,
                                          commandType: CommandType.StoredProcedure
                                        )
                                    );
                return result;
            }
        }
    }
}
