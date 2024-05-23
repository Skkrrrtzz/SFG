using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QA_Audit_Fresh.Models;
using QA_Audit_Fresh.Models.Dto;
using QA_Audit_Fresh.Repositories;

namespace QA_Audit_Fresh.Controllers.Api
{
    [ApiController]
    [Route("api/cpars")]
    public class CPARController : ControllerBase
    {
        private readonly ICPARRepository _repository;

        public CPARController(ICPARRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<CPARModel>> GetCPARs()
        {
            return await _repository.GetCPARs();
        }

        [HttpGet("~/api/auditplans/{planId:int}/cpars")] 
        public async Task<IEnumerable<CPARModel>> GetCPARsByAuditPlan(int planId) 
        {
            var cpars = _repository.GetCPARsByAuditPlan(planId);
            return await cpars; 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IEnumerable<CPARModel>> PostCPAR([FromBody] CPARDto response)
        {
            return await _repository.PostCPAR(new CPARModel(response));
        }
        
    }
}