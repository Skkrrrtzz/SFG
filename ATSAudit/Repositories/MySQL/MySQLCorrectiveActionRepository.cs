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
using ATSAudit.Repositories;
// using MySqlConnector;'
using Microsoft.Data.SqlClient;

namespace ATSAudit.Repositories
{
    public class MySQLCorrectiveActionRepository : ICorrectiveActionsRepository
    {
        private readonly string _connectionString; 
        public MySQLCorrectiveActionRepository(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            // _connectionString = PIMESSettings.atsAuditConnString;
            _connectionString = configuration.GetConnectionString("MySQLConnection");
        }

        public async Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActions()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CorrectiveActions]";
                return await connection.QueryAsync<CorrectiveActionModel>(query);
            }
        }

        public async Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActionsByCPAR(int cparId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CorrectiveActions] where CPARId = @CPARId";
                return await connection.QueryAsync<CorrectiveActionModel>(query, new { CPARId = cparId });
            }
        }

        public async Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveAction(int correctiveActionId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[CorrectiveActions] where CorrectiveActionId = @CorrectiveActionId";
                return await connection.QueryAsync<CorrectiveActionModel>(query, new { CorrectiveActionId = correctiveActionId });
            }
        }

        public async Task<int> PostCorrectiveAction(CorrectiveActionModel correctiveAction) 
        {
            using (var connection = new MySqlConnection(_connectionString))
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

        public Task<int> CloseCorrectiveAction(int cparId, DateTime closeDate)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteCorrectiveAction(int correctiveActionId)
        {
            using (var connection = new MySqlConnection(_connectionString)) 
            {
                // Console.WriteLine("CorrectiveActionId" + conformityId);

                var query = "delete from [dbo].[CorrectiveActions] where CorrectiveActionId = @CorrectiveActionId";

                return await connection.ExecuteAsync(query, new { CorrectiveActionId = correctiveActionId });
            }
        }

    }
}