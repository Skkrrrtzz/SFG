
using APPCommon.Class;
using Dapper;
using MySqlConnector;
using System.Data;
using WEBTemplate.Models;
using static WEBTemplate.Models.LaptopPassModel;

namespace WEBTemplate.Repository
{
    public class LaptopPassRepository : ILaptopPassRepository
    {
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
