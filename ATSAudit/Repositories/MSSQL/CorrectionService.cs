// using Microsoft.CodeAnalysis.CSharp.Syntax;
using ATSAudit.Models;
using Dapper;
// using MySqlConnector;'
using Microsoft.Data.SqlClient;

using APPCommon.Class;

namespace ATSAudit.Repositories
{
    public class CorrectionsService : ICorrectionsRepository
    {
        private readonly string _connectionString; 
        public CorrectionsService(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connectionString = PIMESSettings.atsAuditConnString;
        }

        public async Task<IEnumerable<CorrectionModel>> GetCorrections()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Corrections]";
                return await connection.QueryAsync<CorrectionModel>(query);
            }
        }

        public async Task<IEnumerable<CorrectionModel>> GetCorrectionsByCPAR(int cparId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Corrections] where CPARId = @CPARId";
                return await connection.QueryAsync<CorrectionModel>(query, new { CPARId = cparId });
            }
        }

        public async Task<IEnumerable<CorrectionModel>> GetCorrection(int correctionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Corrections] where CorrectionId = @CorrectionId";
                return await connection.QueryAsync<CorrectionModel>(query, new { CorrectionId = correctionId });
            }
        }

        public async Task<int> PostCorrection(CorrectionModel correction) 
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"insert into [dbo].[Corrections] 
                    (CPARId, CorrectionDescription, EscapeCause, Action) 
                    values (@CPARId, @CorrectionDescription, @EscapeCause, @Action)";
                
                object parameters = new {
                    CPARId = correction.CPARId,
                    CorrectionDescription = correction.CorrectionDescription,
                    EscapeCause = correction.EscapeCause,
                    Action = correction.Action
                };
                
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteCorrection(int correctionId)
        {
            using (var connection = new SqlConnection(_connectionString)) 
            {
                // Console.WriteLine("CorrectionId" + conformityId);

                var query = "delete from [dbo].[Corrections] where CorrectionId = @CorrectionId";

                return await connection.ExecuteAsync(query, new { CorrectionId = correctionId });
            }
        }
    }
}