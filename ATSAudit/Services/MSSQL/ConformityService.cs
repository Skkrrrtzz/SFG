// using Microsoft.CodeAnalysis.CSharp.Syntax;
using ATSAudit.Models;
using Dapper;
// using MySqlConnector;'
using Microsoft.Data.SqlClient;

using APPCommon.Class;

namespace ATSAudit.Services
{
    public class ConformitiesService : IConformitiesRepository
    {
        private readonly string _connectionString; 
        public ConformitiesService(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connectionString = PIMESSettings.atsAuditConnString;
        }

        public async Task<IEnumerable<ConformityModel>> GetConformities()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Conformities]";
                return await connection.QueryAsync<ConformityModel>(query);
            }
        }

        public async Task<IEnumerable<ConformityModel>> GetConformitiesByAuditPlan(int planId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Conformities] where PlanId = @PlanId";
                return await connection.QueryAsync<ConformityModel>(query, new { PlanId = planId });
            }
        }

        public async Task<IEnumerable<ConformityModel>> GetConformity(int conformityId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Conformities] where ConformityId = @ConformityId";
                return await connection.QueryAsync<ConformityModel>(query, new { ConformityId = conformityId });
            }
        }

        public async Task<int> PostConformity(ConformityModel conformity) 
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"insert into [dbo].[Conformities] 
                    (PlanId, ConformityDescription, ConformityAreaSection) 
                    values (@PlanId, @ConformityDescription, @ConformityAreaSection)";
                
                object parameters = new {
                    conformity.PlanId,
                    conformity.ConformityDescription,
                    conformity.ConformityAreaSection
                };
                
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteConformity(int conformityId)
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {
                // Console.WriteLine("ConformityId" + conformityId);

                var query = "delete from [dbo].[Conformities] where ConformityId = @ConformityId";

                return await connection.ExecuteAsync(query, new { ConformityId = conformityId });
            }
        }
    }
}