using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
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

        public async Task<IEnumerable<CPARsModel>> GetCPARs()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CPARs]";
                return await connection.QueryAsync<CPARsModel>(query);
            }
        }

        public async Task<IEnumerable<CPARsModel>> GetCPARsByAuditPlan(int planId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CPARs] where PlanId = @PlanId";
                return await connection.QueryAsync<CPARsModel>(query, new { PlanId = planId });
            }
        }

        public async Task<IEnumerable<CPARsModel>> GetCPAR(int cparId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CPARs] where cparId = @ConformityId";
                return await connection.QueryAsync<CPARsModel>(query, new { ConformityId = cparId });
            }

        }

        public async Task<IEnumerable<CPARsModel>> PostInitialCPAR(CPARsModel cpar)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // string query = "select * from [dbo].[CPARs] where cparId = @ConformityId";
                string query = @"insert into dbo.CPARs
                                (PlanId, Respondent, Requestor, ResponseDueDate, ISOClause, ProblemStatement)
                                output inserted.*
                                values
                                (@PlanId, @Respondent, @Requestor, @ResponseDueDate, @ISOClause, @ProblemStatement)";

                object parameters = new {
                    PlanId = cpar.PlanId,
                    Respondent = cpar.Respondent,
                    Requestor = cpar.Requestor,
                    // IssueDate = cpar.IssueDate,
                    // ApprovalDate = cpar.ApprovalDate,
                    ResponseDueDate = cpar.ResponseDueDate,
                    ISOClause = cpar.ISOClause,
                    ProblemStatement = cpar.ProblemStatement,
                    // PreparedBy = cpar.PreparedBy
                    // CheckedBy = cpar.CheckedBy
                    // ApprovedBy = cpar.ApprovedBy
                };
                
                return await connection.QueryAsync<CPARsModel>(query, parameters);
            }
        }

        public async Task<int> DeleteCPAR(int cparId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "delete from [dbo].[CPARs] where CPARId = @CPARId";
                return await connection.ExecuteAsync(query, new { CPARId = cparId });
            }
        }
    }
}