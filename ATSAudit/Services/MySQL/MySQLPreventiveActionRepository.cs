using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using ATSAudit.Models;
using Dapper;
using System.Data;
using System.ComponentModel;
using ATSAudit.Services;
// using MySqlConnector;'
using Microsoft.Data.SqlClient;


namespace ATSAudit.Services
{
    public class MySQLPreventiveActionRepository : IPreventiveActionsRepository
    {
        private readonly string _connectionString; 
        public MySQLPreventiveActionRepository(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            // _connectionString = PIMESSettings.atsAuditConnString;
            _connectionString = configuration.GetConnectionString("MySQLConnection");
        }

        public async Task<IEnumerable<PreventiveActionModel>> GetPreventiveActions()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[PreventiveActions]";
                return await connection.QueryAsync<PreventiveActionModel>(query);
            }
        }

        public async Task<IEnumerable<PreventiveActionModel>> GetPreventiveActionsByCPAR(int cparId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[PreventiveActions] where CPARId = @CPARId";
                return await connection.QueryAsync<PreventiveActionModel>(query, new { CPARId = cparId });
            }
        }

        public async Task<IEnumerable<PreventiveActionModel>> GetPreventiveAction(int preventiveActionId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[PreventiveActions] where PreventiveActionId = @PreventiveActionId";
                return await connection.QueryAsync<PreventiveActionModel>(query, new { PreventiveActionId = preventiveActionId });
            }
        }

        public async Task<int> PostPreventiveAction(PreventiveActionModel preventiveAction) 
        {
            using (var connection = new MySqlConnection(_connectionString))
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
            using (var connection = new MySqlConnection(_connectionString)) 
            {
                // Console.WriteLine("PreventiveActionId" + conformityId);

                var query = "delete from [dbo].[PreventiveActions] where PreventiveActionId = @PreventiveActionId";

                return await connection.ExecuteAsync(query, new { PreventiveActionId = preventiveActionId });
            }
        }
    }
}