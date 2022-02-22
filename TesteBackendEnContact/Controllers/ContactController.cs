using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interfaces.Entities;
using TesteBackendEnContact.Core.Interfaces.Services;

namespace TesteBackendEnContact.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contactService;

        public ContactController(
            ILogger<ContactController> logger,
            IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        [HttpPost("upload-contacts")]
        public async Task<IActionResult> UploadFromFile(IFormFile file)
        {
            var result = await _contactService.SaveContactsFromCSVFileAsync(file.OpenReadStream());

            return result?.Any() == true ? Ok(result) : NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IContact>>> Get()
        {
            var result = await _contactService.GetAllAsync();

            return result?.Any() == true ? Ok(result) : NoContent();
        }

        [HttpGet("search-contact")]
        public async Task<IActionResult> GetByTerm([FromQuery] string searchTerm)
        {
            var result = await _contactService.SearchContacts(searchTerm);

            return result?.Any() == true ? Ok(result) : NoContent();
        }

        [HttpGet("search-contacts-by-company")]
        public async Task<IActionResult> GetByCompany([FromQuery] int companyId)
        {
            var result = await _contactService.GetByCompanyAsync(companyId);

            return result?.Any() == true ? Ok(result) : NoContent();
        }
    }
}