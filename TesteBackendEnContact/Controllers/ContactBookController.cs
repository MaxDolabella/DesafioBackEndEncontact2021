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
    public class ContactBookController : ControllerBase
    {
        private readonly ILogger<ContactBookController> _logger;
        private readonly IContactBookRepository _contactBookRepository;

        public ContactBookController(
            ILogger<ContactBookController> logger,
            IContactBookRepository contactBookRepository)
        {
            _logger = logger;
            _contactBookRepository = contactBookRepository;
        }

        [HttpPost]
        public async Task<ActionResult<IContactBook>> Post(SaveContactBookRequest contactBook)
        {
            var createdObj = await _contactBookRepository.SaveAsync(contactBook.ToContactBook());
            var param = new { id = createdObj.Id };

            return CreatedAtAction(nameof(Get), param, createdObj);
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            await _contactBookRepository.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IContactBook>>> Get()
        {
            return Ok(await _contactBookRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IContactBook> Get(int id)
        {
            return await _contactBookRepository.GetAsync(id);
        }
    }
}