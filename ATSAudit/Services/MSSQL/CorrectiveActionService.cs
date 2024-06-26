// using Microsoft.CodeAnalysis.CSharp.Syntax;
using ATSAudit.Models;
using Dapper;
// using MySqlConnector;'
using Microsoft.Data.SqlClient;

using APPCommon.Class;
using System.Data;

namespace ATSAudit.Services
{
    public class CorrectiveActionsService : ICorrectiveActionsRepository
    {
        private readonly string _connectionString; 
        public CorrectiveActionsService(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connectionString = PIMESSettings.atsAuditConnString;
        }

        public async Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CorrectiveActions]";
                return await connection.QueryAsync<CorrectiveActionModel>(query);
            }
        }

        public async Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActionsByCPAR(int cparId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CorrectiveActions] where CPARId = @CPARId";
                return await connection.QueryAsync<CorrectiveActionModel>(query, new { CPARId = cparId });
            }
        }

        public async Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveAction(int correctiveActionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CorrectiveActions] where CorrectiveActionId = @CorrectiveActionId";
                return await connection.QueryAsync<CorrectiveActionModel>(query, new { CorrectiveActionId = correctiveActionId });
            }
        }

        public async Task<int> PostCorrectiveAction(CorrectiveActionModel correctiveAction) 
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"insert into [dbo].[CorrectiveActions] 
                    (CPARId, CorrectiveActionDescription, TargetDate, Responsible) 
                    values (@CPARId, @CorrectiveActionDescription, @TargetDate, @Responsible)";
                
                object parameters = new {
                    CPARId = correctiveAction.CPARId,
                    CorrectiveActionDescription = correctiveAction.CorrectiveActionDescription,
                    TargetDate = correctiveAction.TargetDate,
                    Responsible = correctiveAction.Responsible
                };
                
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> CloseCorrectiveAction(int cparId, DateTime actualCloseDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync(   "sp_CloseCorrectiveAction", 
                                                        new { CPARId = cparId, ActualCloseDate = actualCloseDate },
                                                        commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> DeleteCorrectiveAction(int correctiveActionId)
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {
                // Console.WriteLine("CorrectiveActionId" + conformityId);

                var query = "delete from [dbo].[CorrectiveActions] where CorrectiveActionId = @CorrectiveActionId";

                return await connection.ExecuteAsync(query, new { CorrectiveActionId = correctiveActionId });
            }
        }
    }
}