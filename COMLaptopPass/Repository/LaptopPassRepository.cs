using APPCommon.Class;
using COMLaptopPass.Models;
using Dapper;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing.Drawing2D;

namespace COMLaptopPass.Repository
{
    public class LaptopPassRepository : ILaptopPassRepository
    {
        public async Task<IEnumerable<LaptopPassRoleModel>> GetLogin()
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(PIMESSettings.mysqlConnString))
            {
                var result = new ObservableCollection<LaptopPassRoleModel>
                                    (await sqlConnection.QueryAsync<LaptopPassRoleModel>
                                        ("laptop_role_sp",
                                          null,
                                          commandType: CommandType.StoredProcedure
                                        )
                                    );

                return result;
            }
        }

        public async Task<IEnumerable<LaptopPassRequestModel>> GetRequest(LaptopPassParameters param)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(PIMESSettings.mysqlConnString))
            {
                var sqlParameters = new DynamicParameters();
                sqlParameters.Add("@varmode", param.mode);
                sqlParameters.Add("@varrole", param.role);
                sqlParameters.Add("@varuserid", param.userid);
                //sqlParameters.Add("@varrole", pararole);
                //sqlParameters.Add("@varsubmode", parasubmode);
                //sqlParameters.Add("@varrequestid", selectedRequest.requestid);
                //sqlParameters.Add("@varuserid", parauserid);
                //sqlParameters.Add("@varbuid", parabuid);
                //sqlParameters.Add("@varname", selectedRequest.name);
                //sqlParameters.Add("@varempno", selectedRequest.empno);
                //sqlParameters.Add("@varfapno", selectedRequest.fapno);
                sqlParameters.Add("@varstartdate", param.startdate);
                sqlParameters.Add("@varstopdate", param.stopdate);


                var result = new ObservableCollection<LaptopPassRequestModel>
                                    (await sqlConnection.QueryAsync<LaptopPassRequestModel>
                                        ("laptop_request_read_web",
                                          sqlParameters,
                                          commandType: CommandType.StoredProcedure
                                        )
                                    );

                return result;


            }
        }
    }
}
