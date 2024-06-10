using Microsoft.AspNetCore.Mvc;
using QA_Audit_Fresh.Models;
using QA_Audit_Fresh.Repositories;

namespace QA_Audit_Fresh.Controllers.Api
{
    [ApiController]
    // [Route("api/conformities")]
    [Route("api/[controller]")]
    public class CorrectionsController : ControllerBase
    {
        // private readonly AppDbContext _context;
        // private readonly DbSet<AuditPlanModel> _contextCorrection;
        private readonly ICorrectionRepository _repository;
        // private readonly dynamic _session;

        public CorrectionsController(ICorrectionRepository repository)
        {
            _repository = repository;
        }
        [HttpGet] 
        public async Task<IEnumerable<CorrectionModel>> GetCorrections() 
        {
                var conformities = _repository.GetCorrections();
                return await conformities;
        }

        [HttpGet("~/api/auditplans/{planId:int}/corrections")] 
        public async Task<IEnumerable<CorrectionModel>> GetCorrectionsByCPAR(int cparId) 
        {
            var conformities = _repository.GetCorrectionsByCPAR(cparId);
            return await conformities; 
        }

       [HttpGet("{conformityId:int}")] 
       public async Task<IEnumerable<CorrectionModel>> GetCorrection(int correctionId) 
       {
           var conformity = _repository.GetCorrection(correctionId);
           return await conformity; 
       }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<int> PostCorrection([FromBody] CorrectionModel request)
        {
            //TODO: Use CreatedAtRoute() to be RESTful
            return await _repository.PostCorrection(request);
        }

        [HttpDelete("{conformityId:int}")]
        public async Task<IActionResult> DeleteCorrection (int conformityId) 
        {
            try 
            {
                var conformity = await _repository.GetCorrection(conformityId);
                if (conformity == null) return NotFound($"Correction with ID = {conformityId} not found.");

                var query = await _repository.DeleteCorrection(conformityId);
                if (query == 1) return Ok(query);
                else return BadRequest(query);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting resource.");
            }

        }
    }
}