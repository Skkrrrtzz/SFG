using Microsoft.AspNetCore.Mvc;
using QA_Audit_Fresh.Models;
using QA_Audit_Fresh.Repositories;

namespace QA_Audit_Fresh.Controllers.Api
{
    [ApiController]
    // [Route("api/conformities")]
    [Route("api/[controller]")]
    public class ConformitiesController : ControllerBase
    {
        // private readonly AppDbContext _context;
        // private readonly DbSet<AuditPlanModel> _contextConformity;
        private readonly IConformitiesRepository _repository;
        // private readonly dynamic _session;

        public ConformitiesController(IConformitiesRepository repository)
        {
            _repository = repository;
        }
        [HttpGet] 
        public async Task<IEnumerable<ConformityModel>> GetConformities() 
        {
            var conformities = _repository.GetConformities();
            return await conformities;
        }

        [HttpGet("~/api/auditplans/{planId:int}/conformities")] 
        public async Task<IEnumerable<ConformityModel>> GetConformitiesByAuditPlan(int planId) 
        {
            var conformities = _repository.GetConformitiesByAuditPlan(planId);
            return await conformities; 
        }

       [HttpGet("{conformityId:int}")] 
       public async Task<IEnumerable<ConformityModel>> GetConformity(int conformityId) 
       {
           var conformity = _repository.GetConformity(conformityId);
           return await conformity; 
       }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<int> PostConformity([FromBody] ConformityModel request)
        {
            //TODO: Use CreatedAtRoute() to be RESTful
            return await _repository.PostConformity(request);
        }

        [HttpDelete("{conformityId:int}")]
        public async Task<IActionResult> DeleteConformity (int conformityId) 
        {
            try 
            {
                var conformity = await _repository.GetConformity(conformityId);
                if (conformity == null) return NotFound($"Conformity with ID = {conformityId} not found.");

                var query = await _repository.DeleteConformity(conformityId);
                if (query == 1) return Ok(query);
                else return BadRequest(query);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting resource.");
            }

        }
    }
}