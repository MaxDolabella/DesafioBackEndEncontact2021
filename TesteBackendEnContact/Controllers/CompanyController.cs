using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Controllers.Models;
using TesteBackendEnContact.Core.Interfaces.Entities;
using TesteBackendEnContact.Core.Interfaces.Repositories;

namespace TesteBackendEnContact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyRepository _companyRepository;

        public CompanyController(
            ILogger<CompanyController> logger,
            ICompanyRepository companyRepository)
        {
            _logger = logger;
            _companyRepository = companyRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ICompany>> Post(SaveCompanyRequest company)
        {
            var createdObj = await _companyRepository.SaveAsync(company.ToCompany());
            var param = new { id = createdObj.Id };

            return CreatedAtAction(nameof(Get), param, createdObj);
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            await _companyRepository.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ICompany>>> Get()
        {
            return Ok(await _companyRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ICompany> Get(int id)
        {
            return await _companyRepository.GetAsync(id);
        }
    }
}