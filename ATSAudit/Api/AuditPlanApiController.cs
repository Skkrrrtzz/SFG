using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QA_Audit_Fresh.Models;
using System.Text.Json;
using QA_Audit_Fresh.Models.Dto;
using QA_Audit_Fresh.Repositories;

namespace QA_Audit_Fresh.Controllers.Api
{
    [ApiController]
    // [Route("api/auditplans")]
    [Route("api/[controller]")]

    public class AuditPlansController : ControllerBase
    {
        private readonly IAuditPlanRepository _repository;

        public AuditPlansController(IAuditPlanRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all AuditPlans by month.
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet("{month}")] 
        public async Task<IEnumerable<AuditPlanModel>> GetAuditPlansByMonth(int month) 
        {
            var auditPlans = _repository.GetAuditPlansByMonth(month);
            return await auditPlans; 
        }

        [HttpGet] 
        public async Task<IEnumerable<AuditPlanModel>> GetAuditPlans() 
        {
            var auditPlans = _repository.GetAuditPlans();
            return await auditPlans;
        }

        [HttpGet("~/api/departments")]
        public async Task<List<string>> GetDepartments()
        {
            var departments = new List<string> {"Quality Engineering", "Quality Assurance"};
            return await Task.Run(() => departments);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IEnumerable<AuditPlanModel>> PostAuditPlan([FromBody] AuditPlanDto response)
        {
            return await _repository.PostAuditPlan(new AuditPlanModel(response));
        
        }

        [HttpPost("{planId:int}")]
        public async Task<IActionResult> UpdateStatus(int planId, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status cannot be null or empty.");
            }

            int query = await _repository.UpdateStatus(planId, status);
            return Ok(new {Response = "GYATT"});
        }

        [HttpDelete("{planId:int}")]
        public async Task<IActionResult> DeleteAuditPlan (int planId) 
        {
            try 
            {
                var query = await _repository.DeleteAuditPlan(planId);
                if (query == 1) return Ok(query);
                else return BadRequest(query);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting resource.\n" + ex);
            }
        }

    }
}