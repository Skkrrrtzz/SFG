using Microsoft.AspNetCore.Mvc;
using ATSAudit.Models;
using ATSAudit.Repositories;

namespace ATSAudit.Controllers.Api
{
    [ApiController]
    // [Route("api/preventiveAction")]
    [Route("api/[controller]")]
    public class PreventiveActionsController : ControllerBase
    {
        // private readonly AppDbContext _context;
        // private readonly DbSet<AuditPlanModel> _contextPreventiveAction;
        private readonly IPreventiveActionsRepository _repository;
        // private readonly dynamic _session;

        public PreventiveActionsController(IPreventiveActionsRepository repository)
        {
            _repository = repository;
        }
        [HttpGet] 
        public async Task<IEnumerable<PreventiveActionModel>> GetPreventiveAction() 
        {
                var preventiveAction = _repository.GetPreventiveActions();
                return await preventiveAction;
        }

        [HttpGet("~/api/auditplans/{planId:int}/preventiveActions")] 
        public async Task<IEnumerable<PreventiveActionModel>> GetPreventiveActionByCPAR(int cparId) 
        {
            var preventiveAction = _repository.GetPreventiveActionsByCPAR(cparId);
            return await preventiveAction; 
        }

       [HttpGet("{preventiveActionId:int}")] 
       public async Task<IEnumerable<PreventiveActionModel>> GetPreventiveAction(int preventiveActionId) 
       {
           var preventiveAction = _repository.GetPreventiveAction(preventiveActionId);
           return await preventiveAction; 
       }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<int> PostPreventiveAction([FromBody] PreventiveActionModel request)
        {
            //TODO: Use CreatedAtRoute() to be RESTful
            return await _repository.PostPreventiveAction(request);
        }

        [HttpDelete("{preventiveActionId:int}")]
        public async Task<IActionResult> DeletePreventiveAction (int preventiveActionId) 
        {
            try 
            {
                var preventiveAction = await _repository.GetPreventiveAction(preventiveActionId);
                if (preventiveAction == null) return NotFound($"PreventiveAction with ID = {preventiveActionId} not found.");

                var query = await _repository.DeletePreventiveAction(preventiveActionId);
                if (query == 1) return Ok(query);
                else return BadRequest(query);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting resource.");
            }

        }
    }
}