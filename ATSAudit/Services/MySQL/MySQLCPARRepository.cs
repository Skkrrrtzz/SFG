using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using ATSAudit.Models;

namespace ATSAudit.Services
{
    public class MySQLCPARRepository : ICPARsRepository
    {
        private readonly string _connectionString;

        public MySQLCPARRepository(IConfiguration configuration) {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            // _connectionString = PIMESSettings.atsAuditConnString;
            _connectionString = configuration.GetConnectionString("MySQLConnection");
        }

        public async Task<IEnumerable<CPARModel>> GetCPARs()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // string query = "select * from [dbo].[CPARs]";
                string query = "select * from [dbo].[CPARs]";
                return await connection.QueryAsync<CPARModel>(query);
            }
        }
        public async Task<IEnumerable<CPARModel>> GetCPARByAuditPlanWithActualAuditDate(int cparId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // string query = "select * from [dbo].[CPARs]";
                string query = "select * from CPARsActualAuditDate where CPARId = @CPARId";
                return await connection.QueryAsync<CPARModel>(query, new { CPARId = cparId });
            }
        }

        public async Task<IEnumerable<CPARModel>> GetCPARsByAuditPlan(int planId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CPARs] where PlanId = @PlanId";
                return await connection.QueryAsync<CPARModel>(query, new { PlanId = planId });
            }
        }

        public async Task<IEnumerable<CPARModel>> GetCPARAsync(int cparId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CPARs] where cparId = @ConformityId";
                return await connection.QueryAsync<CPARModel>(query, new { ConformityId = cparId });
            }

        }

        public async Task<IEnumerable<CPARModel>> PostInitialCPAR(CPARModel cpar)
        {
            using (var connection = new MySqlConnection(_connectionString))
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

        public IEnumerable<CPARModel> GetCPAR(int cparId)
        {
            throw new NotImplementedException();
        }
    }
}