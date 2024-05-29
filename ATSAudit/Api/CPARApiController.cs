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
    [Route("api/[controller]")]
    public class CPARsController : ControllerBase
    {
        private readonly ICPARRepository _repository;

        public CPARsController(ICPARRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<CPARsModel>> GetCPARs()
        {
            return await _repository.GetCPARs();
        }

        [HttpGet("~/api/auditplans/{planId:int}/cpars")] 
        public async Task<IEnumerable<CPARsModel>> GetCPARsByAuditPlan(int planId) 
        {
            var cpars = _repository.GetCPARsByAuditPlan(planId);
            return await cpars; 
        }

        [HttpGet("{cparId:int}")] 
        public async Task<IEnumerable<CPARsModel>> GetCPAR(int cparId) 
        {
            var cpar = _repository.GetCPAR(cparId);
            return await cpar; 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IEnumerable<CPARsModel>> PostCPAR([FromBody] CPARDto response)
        {
            return await _repository.PostCPAR(new CPARsModel(response));
        }
        
    }
}