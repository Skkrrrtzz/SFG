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
        public async Task<IEnumerable<UserLoginModel>> GetLogin()
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(PIMESSettings.mysqlConnString))
            {
                var result = new ObservableCollection<UserLoginModel>
                                    (await sqlConnection.QueryAsync<UserLoginModel>
                                        ("laptop_role_sp",
                                          null,
                                          commandType: CommandType.StoredProcedure
                                        )
                                    );

                return result;
            }
        }
    }
}
