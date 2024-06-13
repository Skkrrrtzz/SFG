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
using MySqlConnector;

namespace QA_Audit_Fresh.Repositories
{
    public class MySQLCorrectionRepository : ICorrectionRepository
    {
        private readonly string _connectionString; 
        public MySQLCorrectionRepository(IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("DefaultConnection");
            // _connectionString = PIMESSettings.atsAuditConnString;
            _connectionString = configuration.GetConnectionString("MySQLConnection");
        }

        public async Task<IEnumerable<CorrectionModel>> GetCorrections()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Corrections]";
                return await connection.QueryAsync<CorrectionModel>(query);
            }
        }

        public async Task<IEnumerable<CorrectionModel>> GetCorrectionsByCPAR(int cparId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Corrections] where CPARId = @CPARId";
                return await connection.QueryAsync<CorrectionModel>(query, new { CPARId = cparId });
            }
        }

        public async Task<IEnumerable<CorrectionModel>> GetCorrection(int correctionId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string query = "select * from [dbo].[Corrections] where CorrectionId = @CorrectionId";
                return await connection.QueryAsync<CorrectionModel>(query, new { CorrectionId = correctionId });
            }
        }

        public async Task<int> PostCorrection(CorrectionModel correction) 
        {
            using (var connection = new MySqlConnection(_connectionString))
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
            using (var connection = new MySqlConnection(_connectionString)) 
            {
                // Console.WriteLine("CorrectionId" + conformityId);

                var query = "delete from [dbo].[Corrections] where CorrectionId = @CorrectionId";

                return await connection.ExecuteAsync(query, new { CorrectionId = correctionId });
            }
        }
    }
}