using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using QA_Audit_Fresh.Models;
using APPCommon.Class;

namespace QA_Audit_Fresh.Repositories
{
    public class CPARRepository : ICPARRepository
    {
        private readonly string _connectionString;

        public CPARRepository(IConfiguration configuration) {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connectionString = PIMESSettings.atsAuditConnString;
        }

        public async Task<IEnumerable<CPARModel>> GetCPARs()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CPARs]";
                return await connection.QueryAsync<CPARModel>(query);
            }
        }

        public async Task<IEnumerable<CPARModel>> GetCPAR(int cparId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CPARs] where cparId = @ConformityId";
                return await connection.QueryAsync<CPARModel>(query, new { cparId = cparId });
            }

        }

        public async Task<IEnumerable<CPARModel>> PostCPAR(CPARModel cpar)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // string query = "select * from [dbo].[CPARs] where cparId = @ConformityId";
                string query = @"insert into dbo.CPARs(
                        PlanId,
                        Respondent,
                        Requestor,
                        ResponseDueDate,
                        ProblemStatement,
                        PreparedBy
                    )

                    output inserted.*
                    
                    values(
                        @PlanId,
                        @Respondent,
                        @Requestor,
                        @ResponseDueDate,
                        @ProblemStatement,
                        @PreparedBy
                    )";

                object parameters = new {
                    PlanId = cpar.PlanId,
                    Respondent = cpar.Respondent,
                    Requestor = cpar.Requestor,
                    // IssueDate = cpar.IssueDate,
                    // ApprovalDate = cpar.ApprovalDate,
                    ResponseDueDate = cpar.ResponseDueDate,
                    ProblemStatement = cpar.ProblemStatement,
                    PreparedBy = cpar.PreparedBy
                    // CheckedBy = cpar.CheckedBy
                    // ApprovedBy = cpar.ApprovedBy
                };
                return await connection.QueryAsync<CPARModel>(query, parameters);
            }
        }

        public async Task<int> DeleteCPAR(int cparId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "delete from [dbo].[CPARs] where CPARId = @CPARId";
                return await connection.ExecuteAsync(query, new { CPARId = cparId });
            }
        }
    }
}