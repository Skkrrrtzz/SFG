
using APPCommon.Class;
using Dapper;
using MySqlConnector;
using System.Data;
using WEBLogin.Models;
using static WEBLogin.Models.LaptopPassModel;

namespace WEBLogin.Repository
{
    public class LaptopPassRepository : ILaptopPassRepository
    {

        public async Task<IEnumerable<LaptopPassRoleModel>> GetRole()
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(PIMESSettings.mysqlConnString))
            {

                var result = new List<LaptopPassRoleModel>
                                    (await sqlConnection.QueryAsync<LaptopPassRoleModel>
                                        ("laptoprequest_role",
                                          null,
                                          commandType: CommandType.StoredProcedure
                                        )
                                    );
                return result;
            }

        }
    }
}
