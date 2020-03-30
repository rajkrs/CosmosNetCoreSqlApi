using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComplexFaker;
using CosmosNetCoreSqlApi.Models;
using CosmosNetCoreSqlApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmosNetCoreSqlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        private readonly IContactService _contactService;
        private readonly IFakeDataService _fakeDataService;



        public ContactController(
            IContactService contactService,

            IFakeDataService fakeDataService
            )
        {
            _contactService = contactService;

            _fakeDataService = fakeDataService;
        }


        // GET: api/Contact
        [HttpGet]
        public Task<IEnumerable<Contact>> Get()
        {
            return _contactService.GetQueryAsync("select * from c");
        }

        [HttpGet("{id}")]
        public Task<Contact> Get(string id)
        {
            return _contactService.GetByIdAsync(id);
        }


        [HttpGet("add")]
        public async Task AddRandomAsync()
        {
            var contact = _fakeDataService.GenerateComplex<Contact>();
            contact.UserId = Guid.Parse("9f8a5487-3acf-4eb9-96cf-6b8b0b8da526");
            await _contactService.AddAsync(contact);
        }

        [HttpGet("filter")]
        public Task<IEnumerable<Contact>> GetAll(string id)
        {
            return _contactService.GetAsync(c => c.Address.Id == Guid.Parse("ace2b055-67ce-473d-ace7-40639d4f07dc"));
        }

        



    }
}
