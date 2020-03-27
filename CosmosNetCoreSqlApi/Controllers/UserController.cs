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
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IContactService _contactService;
        private readonly IFakeDataService _fakeDataService;
        


        public UserController(
            IUserService userService,
            IContactService contactService,

            IFakeDataService fakeDataService
            ) {
            _userService = userService;
            _contactService = contactService;
            _fakeDataService = fakeDataService;
        }

        // GET: api/User
        [HttpGet]
        public Task<IEnumerable<User>> Get()
        {
            return _userService.GetQueryAsync("select * from c");
        }

         
        [HttpGet("add")]
        public async Task AddRandomAsync()
        {
            var user = _fakeDataService.Generate<User>();
            
            await _userService.AddAsync(user);
        }

    }
}
