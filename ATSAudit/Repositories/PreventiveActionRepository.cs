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
using QA_Audit_Fresh.Models.Dto;
using System.ComponentModel;
using QA_Audit_Fresh.Repositories;
// using MySqlConnector;'
using Microsoft.Data.SqlClient;

using APPCommon.Class;

namespace QA_Audit_Fresh.Repositories
{
    public class PreventiveActionRepository : IPreventiveActionRepository
    {
        private readonly string _connectionString; 
        public PreventiveActionRepository(IConfiguration configuration)
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
                    (CPARId, PreventiveActionDescription, EscapeCause, Action) 
                    values (@CPARId, @PreventiveActionDescription, @EscapeCause, @Action)";
                
                object parameters = new {
                    CPARId = preventiveAction.CPARId,
                    PreventiveActionDescription = preventiveAction.PreventiveActionDescription,
                    EscapeCause = preventiveAction.EscapeCause,
                    Action = preventiveAction.Action
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