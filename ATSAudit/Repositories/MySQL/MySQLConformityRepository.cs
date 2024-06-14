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
using MySqlConnector;


namespace ATSAudit.Repositories
{
    public class MySQLConformityRepository : IConformitiesRepository
    {
        private readonly string _connectionString; 
        public MySQLConformityRepository(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            // _connectionString = PIMESSettings.atsAuditConnString;
            _connectionString = configuration.GetConnectionString("MySQLConnection");
        }

        public async Task<IEnumerable<ConformityModel>> GetConformities()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Conformities]";
                return await connection.QueryAsync<ConformityModel>(query);
            }
        }

        public async Task<IEnumerable<ConformityModel>> GetConformitiesByAuditPlan(int planId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Conformities] where PlanId = @PlanId";
                return await connection.QueryAsync<ConformityModel>(query, new { PlanId = planId });
            }
        }

        public async Task<IEnumerable<ConformityModel>> GetConformity(int conformityId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Conformities] where ConformityId = @ConformityId";
                return await connection.QueryAsync<ConformityModel>(query, new { ConformityId = conformityId });
            }
        }

        public async Task<int> PostConformity(ConformityModel conformity) 
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = @"insert into [dbo].[Conformities] 
                    (PlanId, ConformityDescription, ConformityAreaSection) 
                    values (@PlanId, @ConformityDescription, @ConformityAreaSection)";
                
                object parameters = new {
                    PlanId = conformity.PlanId,
                    ConformityDescription = conformity.ConformityDescription,
                    ConformityAreaSection = conformity.ConformityAreaSection
                };
                
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteConformity(int conformityId)
        {
            using (var connection = new MySqlConnection(_connectionString)) 
            {
                // Console.WriteLine("ConformityId" + conformityId);

                var query = "delete from [dbo].[Conformities] where ConformityId = @ConformityId";

                return await connection.ExecuteAsync(query, new { ConformityId = conformityId });
            }
        }
    }
}