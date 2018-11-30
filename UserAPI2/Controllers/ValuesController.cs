using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserAPI2.Data;
using Microsoft.EntityFrameworkCore;

namespace UserAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        private ApiUserContext _dbContext;
        public ValuesController(ApiUserContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u=>u.Name=="guandex");
            return Json(user);
        }
    }
}
