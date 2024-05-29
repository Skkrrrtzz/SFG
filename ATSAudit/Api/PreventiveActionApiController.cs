using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QA_Audit_Fresh.Models;
using System.Text.Json;
using QA_Audit_Fresh.Models.Dto;
using QA_Audit_Fresh.Repositories;
using System.Net.Http.Json;

namespace QA_Audit_Fresh.Controllers.Api
{
    [ApiController]
    // [Route("api/preventiveAction")]
    [Route("api/[controller]")]
    public class PreventiveActionsController : ControllerBase
    {
        // private readonly AppDbContext _context;
        // private readonly DbSet<AuditPlanModel> _contextPreventiveAction;
        private readonly IPreventiveActionRepository _repository;
        // private readonly dynamic _session;

        public PreventiveActionsController(IPreventiveActionRepository repository)
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
        public async Task<int> PostPreventiveAction([FromBody] PreventiveActionDto response)
        {
            //TODO: Use CreatedAtRoute() to be RESTful
            return await _repository.PostPreventiveAction(new PreventiveActionModel(response));
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