using Microsoft.AspNetCore.Mvc;
using ATSAudit.Models;
using ATSAudit.Services;

namespace ATSAudit.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CPARsController : ControllerBase
    {
        private readonly ICPARsRepository _repository;

        public CPARsController(ICPARsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<CPARModel>> GetCPARs()
        {
            return await _repository.GetCPARs();
        }

        // [HttpGet("~/api/CPARs-with-ActualAuditDate/{cparId:int}")] 
        // public async Task<IEnumerable<CPARsModel>> GetCPARByAuditPlanWithActualAuditDate(int planId)
        // {
        //     var cpars = _repository.GetCPARsByAuditPlanWithActualAuditDate(planId);
        //     return await cpars; 
        // }

        [HttpGet("~/api/auditplans/{planId:int}/cpars")] 
        public async Task<IEnumerable<CPARModel>> GetCPARsByAuditPlan(int planId) 
        {
            var cpars = _repository.GetCPARsByAuditPlan(planId);
            return await cpars; 
        }

        [HttpGet("{cparId:int}")] 
        public async Task<IEnumerable<CPARModel>> GetCPAR(int cparId) 
        {
            var cpar = _repository.GetCPARByAuditPlanWithActualAuditDate(cparId);
            return await cpar; 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IEnumerable<CPARModel>> PostInitialCPAR([FromBody] CPARModel request)
        {
            return await _repository.PostInitialCPAR(request);
        }
        
    }
}