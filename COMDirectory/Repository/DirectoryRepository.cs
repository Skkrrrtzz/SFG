using APPCommon.Class;
using COMDirectory.Models;
using Dapper;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.Data;

namespace COMDirectory.Repository
{
    public class DirectoryRepository : IDirectoryRepository
    {
        public async Task<IEnumerable<DirectoryModel>> GetDirectory()
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(PIMESSettings.mysqlConnString))
            {
                var result = new List<DirectoryModel>
                                    (sqlConnection.Query<DirectoryModel>
                                        ("emplmaster_directory_read",
                                          null,
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
    }
}