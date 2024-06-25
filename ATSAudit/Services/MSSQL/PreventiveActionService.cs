// using Microsoft.CodeAnalysis.CSharp.Syntax;
using ATSAudit.Models;
using Dapper;
// using MySqlConnector;'
using Microsoft.Data.SqlClient;

using APPCommon.Class;

namespace ATSAudit.Repositories
{
    public class PreventiveActionsService : IPreventiveActionsRepository
    {
        private readonly string _connectionString; 
        public PreventiveActionsService(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connectionString = PIMESSettings.atsAuditConnString;
        }

        public async Task<IEnumerable<PreventiveActionModel>> GetPreventiveActions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[PreventiveActions]";
                return await connection.QueryAsync<PreventiveActionModel>(query);
            }
        }

        public async Task<IEnumerable<PreventiveActionModel>> GetPreventiveActionsByCPAR(int cparId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[PreventiveActions] where CPARId = @CPARId";
                return await connection.QueryAsync<PreventiveActionModel>(query, new { CPARId = cparId });
            }
        }

        public async Task<IEnumerable<PreventiveActionModel>> GetPreventiveAction(int preventiveActionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[PreventiveActions] where PreventiveActionId = @PreventiveActionId";
                return await connection.QueryAsync<PreventiveActionModel>(query, new { PreventiveActionId = preventiveActionId });
            }
        }

        public async Task<int> PostPreventiveAction(PreventiveActionModel preventiveAction) 
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"insert into [dbo].[PreventiveActions] 
                    (CPARId, PreventiveActionDescription, TargetDate, Responsible) 
                    values (@CPARId, @PreventiveActionDescription, @TargetDate, @Responsible)";
                
                object parameters = new {
                    CPARId = preventiveAction.CPARId,
                    PreventiveActionDescription = preventiveAction.PreventiveActionDescription,
                    TargetDate = preventiveAction.TargetDate,
                    Responsible = preventiveAction.Responsible
                };
                
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeletePreventiveAction(int preventiveActionId)
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {
                // Console.WriteLine("PreventiveActionId" + conformityId);

                var query = "delete from [dbo].[PreventiveActions] where PreventiveActionId = @PreventiveActionId";

                return await connection.ExecuteAsync(query, new { PreventiveActionId = preventiveActionId });
            }
        }
    }
}