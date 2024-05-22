using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using QA_Audit_Fresh.Models;
using Dapper;
using System.Data;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Azure.Core;
using QA_Audit_Fresh.Models.Dto;
using MySqlConnector;
using APPCommon.Class;

namespace QA_Audit_Fresh.Repositories
{
    public class AuditPlanRepository : IAuditPlanRepository
    {
        private readonly string _connectionString; 
        public AuditPlanRepository(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connectionString = PIMESSettings.atsAuditConnString;
        }

        public async Task<IEnumerable<AuditPlanModel>> GetAuditPlans()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[AuditPlans]";
                return await connection.QueryAsync<AuditPlanModel>(query);
            }
        }

        public async Task<IEnumerable<AuditPlanModel>> GetAuditPlansByMonth(int month)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[AuditPlans] where month(TargetDate) = @Month";
                // object[] parameters = [ new { Month = month } ];
                return await connection.QueryAsync<AuditPlanModel>(query, new { Month = month });
            }
        }

        public async Task<IEnumerable<AuditPlanModel>> GetAuditPlan(int planId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[AuditPlans] where PlanId = @PlanId";
                return await connection.QueryAsync<AuditPlanModel>(query, new { PlanId = planId });
            }
        }

        public async Task<IEnumerable<AuditPlanModel>> PostAuditPlan(AuditPlanModel auditPlan) 
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // string query = @"insert into [dbo].[AuditPlans](
                //     Requestor,
                //     Department,
                //     AuditCategory,
                //     TargetDate,
                //     TimeEnd,
                //     AuditorApproved,
                //     AuditeeApproved,
                //     Status) 

                //     values (
                //     @Requestor,
                //     @Department,
                //     @AuditCategory,
                //     @TargetDate,
                //     @TimeEnd,
                //     @AuditorApproved,
                //     @AuditeeApproved,
                //     @Status)";

                object parameters = new {
                    Requestor = auditPlan.Requestor,
                    Department = auditPlan.Department,
                    AuditCategory = auditPlan.AuditCategory,
                    TargetDate = auditPlan.TargetDate,
                    TimeEnd = auditPlan.TimeEnd,
                    AuditorApproved = auditPlan.AuditorApproved,
                    AuditeeApproved = auditPlan.AuditeeApproved,
                    Status = auditPlan.Status};

                // int executePost = await connection.ExecuteAsync(query, parameters);
                return await connection.QueryAsync<AuditPlanModel>(   "sp_InsertAuditPlan",
                                                                                parameters,
                                                                                commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> UpdateStatus(int planId, string status)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // string query = "update [dbo].[AuditPlans]";
                return await connection.ExecuteAsync(   "sp_UpdateStatus", 
                                                        new {  PlanId = planId, Status = status },
                                                        commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> DeleteAuditPlan(int planId)
        {
            using (var connection = new MySqlConnection(_connectionString)) 
            {
                var query = "delete from [dbo].[AuditPlans] where PlanId = @PlanId";

                return await connection.ExecuteAsync(query, new { PlanId = planId });
            }
        }
    }
}