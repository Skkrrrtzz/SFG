using Microsoft.AspNetCore.Mvc;
using ATSAudit.Models;
using ATSAudit.Repositories;

namespace ATSAudit.Controllers.Api
{
    [ApiController]
    // [Route("api/auditplans")]
    [Route("api/[controller]")]

    public class AuditPlansController : ControllerBase
    {
        private readonly IAuditPlansRepository _repository;

        public AuditPlansController(IAuditPlansRepository repository)
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
        public async Task<IEnumerable<AuditPlanModel>> PostAuditPlan([FromBody] AuditPlanModel request)
        {
            return await _repository.PostAuditPlan(request);
        
        }

        [HttpPost("{planId:int}")]
        public async Task<IActionResult> UpdateStatus(int planId, [FromBody] UpdateStatusDto request)
        {
            if (string.IsNullOrEmpty(request.Status))
            {
                return BadRequest("Status cannot be null or empty.");
            }

            if (request.Status == "Closed" && request.ActualAuditDate == null)
            {
                return BadRequest("ActualAuditDate cannot be null or empty when closing an audit plan.");
            }

            int query;
            if (request.Status == "Closed")
            {
                query = await _repository.UpdateStatus(planId, request.Status, request.ActualAuditDate.Value);
                return Ok(new {request = $"Succesfully updated PlanId {planId} Status to 'Closed'."});
            } else {
                query = await _repository.UpdateStatus(planId, request.Status);
                return Ok(new {request = $"Successfully updated PlanId {planId} Status to '{request.Status}'."});
            }
        }
        
        // [HttpPost("{planId:int}")]
        // public async Task<IActionResult> UpdateStatus(int planId, [FromBody] string status)
        // {
        //     if (string.IsNullOrEmpty(status))
        //     {
        //         return BadRequest("Status cannot be null or empty.");
        //     }

        //     int query = await _repository.UpdateStatus(planId, status);
        //     return Ok(new {Response = "GYATT"});
        // }

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