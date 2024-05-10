using APPCommon.Class;
using Dapper;
using MySqlConnector;
using System.Data;
using static APPTemplate.Models.LaptopPassModel;

namespace APPTemplate.Repository
{
    public class LaptopPassRepository : ILaptopPassRepository
    {
        public async Task<IEnumerable<LaptopPassRoleModel>> GetRole()
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(PIMESSettings.mysqlConnString))
            {

                var result = new List<LaptopPassRoleModel>
                                    (await sqlConnection.QueryAsync<LaptopPassRoleModel>
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