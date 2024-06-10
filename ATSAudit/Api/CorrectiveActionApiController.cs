using Microsoft.AspNetCore.Mvc;
using QA_Audit_Fresh.Models;
using QA_Audit_Fresh.Repositories;

namespace QA_Audit_Fresh.Controllers.Api
{
    [ApiController]
    // [Route("api/conformities")]
    [Route("api/[controller]")]
    public class CorrectiveActionsController : ControllerBase
    {
        // private readonly AppDbContext _context;
        // private readonly DbSet<AuditPlanModel> _contextCorrectiveAction;
        private readonly ICorrectiveActionRepository _repository;
        // private readonly dynamic _session;

        public CorrectiveActionsController(ICorrectiveActionRepository repository)
        {
            _repository = repository;
        }
        [HttpGet] 
        public async Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActions() 
        {
                var conformities = _repository.GetCorrectiveActions();
                return await conformities;
        }

        [HttpGet("~/api/auditplans/{planId:int}/correctiveActions")] 
        public async Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveActionsByCPAR(int cparId) 
        {
            var conformities = _repository.GetCorrectiveActionsByCPAR(cparId);
            return await conformities; 
        }

       [HttpGet("{correctiveActionId:int}")] 
       public async Task<IEnumerable<CorrectiveActionModel>> GetCorrectiveAction(int correctiveActionId) 
       {
           var correctiveAction = _repository.GetCorrectiveAction(correctiveActionId);
           return await correctiveAction; 
       }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<int> PostCorrectiveAction([FromBody] CorrectiveActionModel request)
        {
            //TODO: Use CreatedAtRoute() to be RESTful
            return await _repository.PostCorrectiveAction(request);
        }

        [HttpDelete("{correctiveActionId:int}")]
        public async Task<IActionResult> DeleteCorrectiveAction (int correctiveActionId) 
        {
            try 
            {
                var correctiveAction = await _repository.GetCorrectiveAction(correctiveActionId);
                if (correctiveAction == null) return NotFound($"CorrectiveAction with ID = {correctiveActionId} not found.");

                var query = await _repository.DeleteCorrectiveAction(correctiveActionId);
                if (query == 1) return Ok(query);
                else return BadRequest(query);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting resource.");
            }

        }
    }
}